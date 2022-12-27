using Godot;
using System;
using System.Linq;

namespace Enemies.HammerEnemy
{
  public class Idle : TimedEnemyState
  {
    public override void Enter(params object[] args)
    {
      base.Enter(args);
      _owner.Velocity = Vector2.Zero;
    }

    public override void OnTimeout()
    {
      base.OnTimeout();
      TransitionTo(nameof(Wandering));
    }
  }
}