using Godot;
using System;
using System.Diagnostics;

public class Dodging : PlayerState
{
    public override void Enter()
    {
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
            MovePlayer(Vector2.Down * 10);
        }
        else
        {
            MovePlayer(dodgeDirection * 10);
        }

        TransitionBack();
    }
}
