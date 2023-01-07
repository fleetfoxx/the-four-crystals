using Godot;
using System;

namespace Player
{
  public class Dashing : TimedPlayerState
  {
    private Vector2 _direction = Vector2.Zero;

    public override void Enter(params object[] args)
    {
      base.Enter(args);

      // save initial direction
      _direction = GetInputDirection();

      _player.PhysicsCollider.Disabled = true;
    }

    public override void Exit()
    {
      base.Exit();
      _player.PhysicsCollider.Disabled = false;
    }

    public override void Process(float delta)
    {
      base.Process(delta);

      // move player in direction at dash speed
      _player.Velocity = _direction * _player.DashSpeed;
    }

    protected override void OnTimeout()
    {
      base.OnTimeout();
      TransitionBack();
    }

    private Vector2 GetInputDirection()
    {
      var direction = Input.GetVector("move_left", "move_right", "move_up", "move_down");

      if (direction == Vector2.Zero)
      {
        direction = Vector2.Down;
      }

      return direction;
    }
  }
}