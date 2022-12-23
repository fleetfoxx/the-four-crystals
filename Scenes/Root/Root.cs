using Godot;
using System;

public class Root : Node2D
{
    public override void _Ready()
    {
        GD.Randomize();
    }
}
