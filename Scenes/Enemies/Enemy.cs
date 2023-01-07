using Godot;

namespace Enemies
{
  public class Enemy : KinematicBody2D, IEnemy, IDamageable, IKnockable
  {
    [Export]
    public int MaxHealth { get; set; } = 2;

    [Export]
    public int Health { get; set; } = 2;

    public Vector2 Velocity { get; set; }

    public override void _PhysicsProcess(float delta)
    {
      Velocity = Velocity.LinearInterpolate(Vector2.Zero, 0.2f * delta);

      Velocity = MoveAndSlide(Velocity);

      if (Velocity.IsEqualApprox(Vector2.Zero))
      {
        Velocity = Vector2.Zero;
      }
    }

    public void ApplyDamage(Node source, int amount)
    {
      Health -= amount;

      if (Health <= 0)
      {
        QueueFree();
      }
    }

    public void ApplyKnockback(Vector2 force)
    {
      Velocity += force;
    }
  }
}
