using Godot;
using System;
using System.Diagnostics;

public class Test3DPlayer : KinematicBody, IDamageable, IInvisible
{
  [Export]
  public float MaxSpeed { get; set; } = 800;

  [Export]
  public float MaxDodgeSpeed { get; set; } = 1200;

  [Export(PropertyHint.Range, "0,1,")]
  public float SneakSpeedScale { get; set; } = 0.5f;

  [Export]
  public float StealthStaminaCost { get; set; } = 4;

  [Export]
  public float DodgeStaminaCost { get; set; } = 10;

  [Export]
  public float StaminaRecharge { get; set; } = 8;

  [Export]
  public float DodgeDuration { get; set; } = 0.1f;

  [Export]
  public float DodgeCooldown { get; set; } = 0.5f;

  [Export]
  public float MaxAcceleration { get; set; } = 10;

  [Export]
  public float Friction { get; set; } = 500;

  [Export]
  private PackedScene _soundGrenadeScene;

  public int MaxHealth { get => PlayerManager.MaxHealth; set => PlayerManager.MaxHealth = value; }
  public int Health { get => PlayerManager.Health; set => PlayerManager.Health = value; }
  public float Stamina { get => PlayerManager.Stamina; set => PlayerManager.Stamina = value; }
  public float MaxStamina { get => PlayerManager.MaxStamina; set => PlayerManager.MaxStamina = value; }

  public Spatial Carrying { get; private set; } = null;
  public bool IsInvisible { get; private set; }

  private Vector2 _direction;
  private Vector2 _desiredVelocity;
  private Vector3 _velocity;
  private float _maxSpeedChange;
  private float _acceleration;
  private bool _canDodge = true;
  private bool _isDodging = false;

  private Area _pickupArea;
  private FollowCamera3D _camera;
  private AnimationPlayer _animationPlayer;

  public override void _Ready()
  {
    _pickupArea = this.GetExpectedNode<Area>("PickupArea");
    _camera = this.GetExpectedNode<FollowCamera3D>("../FollowCamera3D");
    _animationPlayer = this.GetExpectedNode<AnimationPlayer>("AnimationPlayer");
  }

  public override void _Process(float delta)
  {
    if (Input.IsActionJustPressed("stealth"))
    {
      IsInvisible = true;
      _animationPlayer.Play("Stealth");
    }

    if ((Input.IsActionJustReleased("stealth") && IsInvisible) || Stamina <= 0)
    {
      IsInvisible = false;
      _animationPlayer.PlayBackwards("Stealth");
    }

    if (IsInvisible)
    {
      Stamina -= StealthStaminaCost * delta;

      if (Carrying != null)
      {
        DropCarriedObject();
      }
    }
    else if (Stamina < MaxStamina)
    {
      Stamina += StaminaRecharge * delta;

      if (Stamina > MaxStamina)
      {
        Stamina = MaxStamina;
      }
    }

    if (Input.IsActionJustPressed("dodge") && !_isDodging && _canDodge && Stamina >= DodgeStaminaCost)
    {
      _isDodging = true;
      _canDodge = false;

      Stamina -= DodgeStaminaCost;

      GetTree().CreateTimer(DodgeDuration).Connect("timeout", this, nameof(OnDodgeEnd));
      GetTree().CreateTimer(DodgeCooldown).Connect("timeout", this, nameof(OnDodgeCooldownEnd));
    }

    _direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
    _direction = _direction.Rotated(-_camera.Rotation.y);

    if (_isDodging)
    {
      var direction = _direction.Length() == 0 ? Vector2.Down : _direction.Normalized();
      _desiredVelocity = direction * Math.Max(MaxDodgeSpeed - Friction, 0);
    }
    else
    {
      _desiredVelocity = _direction * Math.Max(MaxSpeed - Friction, 0);
    }

    if (IsInvisible)
    {
      _desiredVelocity *= SneakSpeedScale;
    }

    if (Input.IsActionJustPressed("interact"))
    {
      HandleInteract();
    }

    if (Input.IsActionJustPressed("throw"))
    {
      var grenade = _soundGrenadeScene.Instance<SoundGrenade>();
      var mousePosition = GetViewport().GetMousePosition();
      var camera = GetViewport().GetCamera();
      var from = camera.ProjectRayOrigin(mousePosition);
      var to = camera.ProjectRayNormal(mousePosition) * 1000;
      var cursorPosition = new Plane(Vector3.Up, 0).IntersectRay(from, to);

      if (cursorPosition.HasValue)
      {
        GetParent().AddChild(grenade);
        grenade.GlobalTranslation = GlobalTranslation;
        grenade.Throw(cursorPosition.Value);
      }
    }
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    base._UnhandledInput(@event);
  }

  public override void _PhysicsProcess(float delta)
  {
    _maxSpeedChange = MaxAcceleration * delta;
    _velocity = _velocity.LinearInterpolate(new Vector3(_desiredVelocity.x, 0, _desiredVelocity.y), _maxSpeedChange);
    _velocity = MoveAndSlide(_velocity);
  }

  private void OnDodgeEnd()
  {
    _isDodging = false;
  }

  private void OnDodgeCooldownEnd()
  {
    _canDodge = true;
  }

  private void DropCarriedObject()
  {
    RemoveChild(Carrying);
    GetParent().AddChild(Carrying);
    Carrying.GlobalTranslation = GlobalTranslation;
    Carrying = null;
  }

  private void HandleInteract()
  {
    foreach (Area area in _pickupArea.GetOverlappingAreas())
    {
      Debug.WriteLine($">> Handling interact with {area.Name}");
      if (!IsInvisible && Carrying == null && area is ICarryable)
      {
        Debug.WriteLine($">> Picking up carryable {area.Name}");
        var parent = area.GetParent();
        parent.RemoveChild(area);
        CallDeferred("add_child", area);
        area.Translation = new Vector3(0, 3, 0);
        Carrying = area;
        return;
      }
      else if (area is IInteractable)
      {
        Debug.WriteLine($">> Interacting with interactable {area.Name}");
        ((IInteractable)area).Interact();
        return;
      }
    }

    foreach (PhysicsBody body in _pickupArea.GetOverlappingBodies())
    {
      if (body is Campfire3D)
      {
        Debug.WriteLine($">> Interacting with campfire");
        var campfire = (Campfire3D)body;

        if (campfire.GetNumberOfLogs() < 3 && Carrying is Log3D)
        {
          var wasSuccessful = campfire.Interact(Carrying);

          if (wasSuccessful)
          {
            Carrying = null;
          }
        }
        else if (campfire.GetNumberOfLogs() == 3 && !campfire.IsLit)
        {
          campfire.Interact();
        }
        return;
      }
    }

    // If there's nothing to interact with or pick up, check if there's anything to drop.
    if (Carrying != null)
    {
      DropCarriedObject();
    }
  }

  public void ApplyDamage(Node source, int amount)
  {
    Health -= amount;
  }
}
