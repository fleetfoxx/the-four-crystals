using Godot;
using System;

[Tool]
public class Circle : Node2D
{
  [Export]
  public Color Color { get; set; } = Colors.White;

  [Export]
  public float Radius { get; set; } = 1;

  public override void _Process(float delta)
  {
    Update();
  }

  public override void _Draw()
  {
    DrawCircle(Vector2.Zero, Radius, Color);
  }
}
