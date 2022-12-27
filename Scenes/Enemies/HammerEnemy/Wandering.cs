using Godot;
using System;

namespace Enemies.HammerEnemy
{
  public class Wandering : TimedEnemyState
  {
    public override void OnTimeout()
    {
      base.OnTimeout();
      TransitionTo(nameof(Idle));
    }
  }
}