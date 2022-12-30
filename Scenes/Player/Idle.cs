using Godot;
using System;
using System.Diagnostics;

namespace Player
{
  public class Idle : PlayerState
  {
    public override void Enter(params object[] args)
    {
      base.Enter(args);
      _player.Velocity = Vector2.Zero;
    }

    public override void Process(float delta)
    {
      var velocity = Input.GetVector("move_left", "move_right", "move_up", "move_down");


      if (!velocity.IsEqualApprox(Vector2.Zero))
      {
        TransitionTo(nameof(Walking));
      }
      else if (Input.IsActionJustPressed("dodge"))
      {
        TransitionTo(nameof(Dashing));
      }
    }
  }
}
