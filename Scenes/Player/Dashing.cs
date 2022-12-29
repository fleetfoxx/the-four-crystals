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
    }

    public override void Process(float delta)
    {
      base.Process(delta);

      // move player in direction at dash speed
      _player.Velocity = _direction * _player.DashSpeed;
    }

    public override void OnTimeout()
    {
      base.OnTimeout();
      TransitionBack();
    }

    private Vector2 GetInputDirection()
    {
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

      if (direction == Vector2.Zero)
      {
        direction = Vector2.Down;
      }

      return direction;
    }
  }
}