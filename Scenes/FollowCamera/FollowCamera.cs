using Godot;
using System;
using System.Diagnostics;

public class FollowCamera : Camera2D
{
    private Node2D Target;

    public override void _Ready()
    {
        Target = GetNodeOrNull<Player>("../Player");
        Debug.Assert(Target != null);
    }

    public override void _Process(float delta)
    {
        Position = Position.LinearInterpolate(Target.Position, 0.1f);
    }
}
