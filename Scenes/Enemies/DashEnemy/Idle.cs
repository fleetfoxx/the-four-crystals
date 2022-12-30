using System.Diagnostics;
using Godot;

namespace Enemies.DashEnemy
{
  public class Idle : TimedEnemyState
  {
    public override void Enter(params object[] args)
    {
      base.Enter(args);
      _owner.Velocity = Vector2.Zero;
    }

    protected override void OnTimeout()
    {
      TransitionTo(nameof(Wandering));
    }
  }
}
