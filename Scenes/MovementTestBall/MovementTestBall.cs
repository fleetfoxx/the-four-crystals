/* Based on https://github.com/Shinjingi/Unity2D-Platform-Character-Controller/blob/main/Assets/Scripts/Capabilities/Move.cs */

using Godot;
using System;
using System.Diagnostics;
using System.Linq;

public class MovementTestBall : KinematicBody2D
{
  [Export]
  public float MaxSpeed { get; set; } = 800;

  [Export]
  public float MaxDodgeSpeed { get; set; } = 1200;

  [Export]
  public float DodgeDuration { get; set; } = 0.1f;

  [Export]
  public float DodgeCooldown { get; set; } = 0.5f;

  [Export]
  public float MaxAcceleration { get; set; } = 10;

  [Export]
  public float Friction { get; set; } = 500;

  [Export]
  public PackedScene CampfireScene { get; set; }

  private Vector2 _direction;
  private Vector2 _desiredVelocity;
  private Vector2 _velocity;
  private float _maxSpeedChange;
  private float _acceleration;
  private bool _canDodge = true;
  private bool _isDodging = false;
  private Node2D _carrying = null;

  private Area2D _pickupArea;

  public override void _Ready()
  {
    base._Ready();

    _pickupArea = this.GetExpectedNode<Area2D>("PickupArea");
  }

  public override void _Process(float delta)
  {
    base._Process(delta);

    if (Input.IsActionJustPressed("dodge") && !_isDodging && _canDodge)
    {
      _isDodging = true;
      _canDodge = false;

      GetTree().CreateTimer(DodgeDuration).Connect("timeout", this, nameof(OnDodgeEnd));
      GetTree().CreateTimer(DodgeCooldown).Connect("timeout", this, nameof(OnDodgeCooldownEnd));
    }

    _direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");

    if (_isDodging)
    {
      var direction = _direction.Length() == 0 ? Vector2.Down : _direction.Normalized();
      _desiredVelocity = direction * Math.Max(MaxDodgeSpeed - Friction, 0);
    }
    else
    {
      _desiredVelocity = _direction * Math.Max(MaxSpeed - Friction, 0);
    }

    if (Input.IsActionJustPressed("interact"))
    {
      HandleInteract();
    }
  }

  private void HandleInteract()
  {
    foreach (Area2D area in _pickupArea.GetOverlappingAreas())
    {
      if (_carrying == null && area is ICarrayble)
      {
        var parent = area.GetParent();
        parent.RemoveChild(area);
        CallDeferred("add_child", area);
        area.Position = Vector2.Zero;
        _carrying = area;
        return;
      }
      else if (area is IInteractable)
      {
        if (area is Campfire)
        {
          var campfire = (Campfire)area;

          if (campfire.GetNumberOfSticks() < 3 && _carrying is Stick)
          {
            var wasSuccessful = campfire.Interact(_carrying);

            if (wasSuccessful)
            {
              _carrying = null;
            }
          }
          else if (campfire.GetNumberOfSticks() == 3 && !campfire.IsLit)
          {
            campfire.Interact();
          }
          return;
        }
        else
        {
          ((IInteractable)area).Interact();
          return;
        }
      }
    }

    // If there's nothing to interact with or pick up, check if there's anything to drop.
    if (_carrying != null)
    {
      RemoveChild(_carrying);
      GetParent().AddChild(_carrying);
      _carrying.GlobalPosition = GlobalPosition;
      _carrying = null;
    }
  }

  public override void _PhysicsProcess(float delta)
  {
    base._PhysicsProcess(delta);
    _maxSpeedChange = MaxAcceleration * delta;
    _velocity = _velocity.LinearInterpolate(_desiredVelocity, _maxSpeedChange);
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
}
