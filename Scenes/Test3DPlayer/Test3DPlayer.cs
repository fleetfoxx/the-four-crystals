using Godot;
using System;
using System.Diagnostics;

public class Test3DPlayer : KinematicBody
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

  private FollowCamera3D _camera;
  private AnimationPlayer _animationPlayer;

  public override void _Ready()
  {
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
    Debug.WriteLine(">> Handling interact");
  }
}
