using Godot;
using System;

[Tool]
public class Orb : Area2D, IInteractable
{
  protected virtual Color Color { get; set; } = Colors.White;

  public override void _Process(float delta)
  {
    Update();
  }

  public override void _Draw()
  {
    var collider = this.GetExpectedNode<CollisionShape2D>("Collider");
    var radius = ((CircleShape2D)collider.Shape).Radius;
    DrawCircle(Vector2.Zero, radius, Color);
  }

  public virtual bool Interact(params object[] args)
  {
    LevelManager.LoadLevel(Levels.TestLevelSelect);
    return true;
  }
}
