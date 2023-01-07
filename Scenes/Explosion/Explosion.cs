using Godot;
using System;
using System.Diagnostics;

[Tool]
public class Explosion : Area2D
{
  [Export]
  public float Radius { get; set; } = 10;

  public override void _Process(float delta)
  {
    base._Process(delta);
    var collider = this.GetExpectedNode<CollisionShape2D>("Collider");
    ((CircleShape2D)collider.Shape).Radius = Radius;
    Update();
  }

  public override void _Draw()
  {
    base._Draw();
    DrawCircle(Position, Radius, Color.Color8(255, 255, 0, 127));
  }
}
