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

      var direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");
      _player.Velocity = direction * _player.WalkSpeed;

      if (_player.Velocity.IsEqualApprox(Vector2.Zero))
      {
        // Not currently walking, transition to Idle
        TransitionTo(nameof(Idle));
      }
    }
  }
}
