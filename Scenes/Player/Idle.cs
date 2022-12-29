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
      if (
          Input.IsActionPressed("ui_up")
          || Input.IsActionPressed("ui_down")
          || Input.IsActionPressed("ui_left")
          || Input.IsActionPressed("ui_right")
      )
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
