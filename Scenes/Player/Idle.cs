using Godot;
using System;
using System.Diagnostics;

public class Idle : PlayerState
{
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
            TransitionTo(nameof(Dodging));
        }
    }
}
