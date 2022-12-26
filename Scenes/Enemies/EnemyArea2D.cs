using Godot;

namespace Enemies
{
  public class EnemyArea2D : Area2D
  {
    public Vector2 Velocity;

    public override void _PhysicsProcess(float delta)
    {
      if (Velocity.IsEqualApprox(Vector2.Zero))
      {
        Velocity = Vector2.Zero;
      }

      Position = Velocity * delta;
    }
  }
}
