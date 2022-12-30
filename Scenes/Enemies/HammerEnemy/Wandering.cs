using Godot;
using System;

namespace Enemies.HammerEnemy
{
  public class Wandering : TimedEnemyState
  {
    protected override void OnTimeout()
    {
      base.OnTimeout();
      TransitionTo(nameof(Idle));
    }
  }
}