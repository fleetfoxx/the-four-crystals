using Godot;
using System;
using System.Diagnostics;

namespace Player
{
    public class Dodging : PlayerState
    {
        [Export]
        private float _dodgeStrength = 750;

        public override void Enter()
        {
            base.Enter();

            var dodgeDirection = Vector2.Zero;

            if (Input.IsActionPressed("ui_up"))
            {
                dodgeDirection += Vector2.Up;
            }

            if (Input.IsActionPressed("ui_down"))
            {
                dodgeDirection += Vector2.Down;
            }

            if (Input.IsActionPressed("ui_left"))
            {
                dodgeDirection += Vector2.Left;
            }

            if (Input.IsActionPressed("ui_right"))
            {
                dodgeDirection += Vector2.Right;
            }

            dodgeDirection = dodgeDirection.Normalized();

            if (dodgeDirection == Vector2.Zero)
            {
                dodgeDirection = Vector2.Down;
            }

            _player.Velocity += dodgeDirection * _dodgeStrength;

            TransitionBack();
        }
    }
}
