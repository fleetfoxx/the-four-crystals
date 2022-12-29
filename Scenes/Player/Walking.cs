using Godot;
using System;
using System.Diagnostics;

namespace Player
{
  public class Walking : PlayerState
  {
    public override void Process(float delta)
    {
      if (Input.IsActionJustPressed("dodge"))
      {
        TransitionTo(nameof(Dashing));
        return;
      }

      var direction = Vector2.Zero;

      if (Input.IsActionPressed("ui_up"))
      {
        direction += Vector2.Up;
      }

      if (Input.IsActionPressed("ui_down"))
      {
        direction += Vector2.Down;
      }

      if (Input.IsActionPressed("ui_left"))
      {
        direction += Vector2.Left;
      }

      if (Input.IsActionPressed("ui_right"))
      {
        direction += Vector2.Right;
      }

      direction = direction.Normalized();
      _player.Velocity = direction * _player.WalkSpeed;

      if (_player.Velocity.IsEqualApprox(Vector2.Zero))
      {
        // Not currently walking, transition to Idle
        TransitionTo(nameof(Idle));
      }
    }
  }
}
