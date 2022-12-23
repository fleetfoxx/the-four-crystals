using Godot;
using System;
using System.Diagnostics;

public class FollowCamera : Camera2D
{
    [Export]
    private float _stiffness = 10f;

    private Node2D _target;

    public override void _Ready()
    {
        _target = GetNodeOrNull<Player.Player>("../Player");
        Debug.Assert(_target != null);
    }

    public override void _Process(float delta)
    {
        Position = Position.LinearInterpolate(_target.Position, _stiffness * delta);
    }
}
