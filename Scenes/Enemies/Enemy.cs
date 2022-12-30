using Godot;

namespace Enemies
{
  public class Enemy : KinematicBody2D, IEnemy
  {
    [Export]
    public float MaxHealth = 100;

    [Export]
    public float Health = 75;

    public Vector2 Velocity { get; set; }

    public override void _PhysicsProcess(float delta)
    {
      Velocity = MoveAndSlide(Velocity);

      if (Velocity.IsEqualApprox(Vector2.Zero))
      {
        Velocity = Vector2.Zero;
      }
    }
  }
}
