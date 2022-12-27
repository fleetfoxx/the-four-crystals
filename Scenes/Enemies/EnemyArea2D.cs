using System.Diagnostics;
using Godot;

namespace Enemies
{
  public class EnemyArea2D : Area2D, IEnemy
  {
    [Export]
    protected float _speed = 1f;

    public Vector2 Velocity { get; set; }

    public override void _PhysicsProcess(float delta)
    {
      if (Velocity.IsEqualApprox(Vector2.Zero))
      {
        Velocity = Vector2.Zero;
      }

      Position += Velocity * _speed * delta;
    }

    protected T GetExpectedNode<T>(NodePath path) where T : Node
    {
      var node = GetNodeOrNull<T>(path);
      Debug.Assert(node != null);
      return node;
    }
  }
}
