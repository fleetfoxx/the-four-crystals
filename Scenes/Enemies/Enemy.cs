using Godot;

namespace Enemies
{
    public class Enemy : KinematicBody2D
    {
        public Vector2 Velocity;

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
