using Godot;
using System;
using System.Diagnostics;

public class Walking : PlayerState
{
    private Player _player;

    public override void Init(Player player)
    {
        _player = player;
    }

    public override void Process(float delta)
    {
        if (Input.IsActionJustPressed("dodge"))
        {
            TransitionTo(nameof(Dodging));
            return;
        }

        var velocityDelta = Vector2.Zero;

        if (Input.IsActionPressed("ui_up"))
        {
            velocityDelta += Vector2.Up;
        }

        if (Input.IsActionPressed("ui_down"))
        {
            velocityDelta += Vector2.Down;
        }

        if (Input.IsActionPressed("ui_left"))
        {
            velocityDelta += Vector2.Left;
        }

        if (Input.IsActionPressed("ui_right"))
        {
            velocityDelta += Vector2.Right;
        }

        velocityDelta = velocityDelta.Normalized();
        MovePlayer(velocityDelta);

        if (_player.Velocity == Vector2.Zero)
        {
            // Not currently walking, transition to Idle
            TransitionTo(nameof(Idle));
        }
    }
}
