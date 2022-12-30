using Godot;
using System;
using System.Diagnostics;

namespace Player
{
  public class Dodging : PlayerState
  {
    [Export]
    private float _dodgeStrength = 750;

    public override void Enter(params object[] args)
    {
      base.Enter(args);

      var dodgeDirection = Input.GetVector("move_left", "move_right", "move_up", "move_down");

      if (dodgeDirection == Vector2.Zero)
      {
        dodgeDirection = Vector2.Down;
      }

      _player.Velocity += dodgeDirection * _dodgeStrength;

      TransitionBack();
    }
  }
}
