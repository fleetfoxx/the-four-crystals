using Godot;

namespace Enemies.HammerEnemy
{
  public class Attacking : EnemyState
  {
    public override void Enter(params object[] args)
    {
      base.Enter(args);
      _owner.Velocity = Vector2.Zero;
    }
  }
}