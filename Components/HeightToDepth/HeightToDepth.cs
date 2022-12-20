using Godot;
using System;
using System.Diagnostics;

public class HeightToDepth : Node2D
{
    private Node2D _parent;

    public override void _Ready()
    {
        _parent = GetParentOrNull<Node2D>();
        Debug.Assert(_parent != null);
        _parent.ZAsRelative = false;
    }

    public override void _Process(float delta)
    {
        _parent.ZIndex = (int)Math.Floor(GlobalPosition.y);
    }
}
