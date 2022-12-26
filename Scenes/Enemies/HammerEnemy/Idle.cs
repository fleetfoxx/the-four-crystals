using Godot;
using System;

namespace Enemies.HammerEnemy
{
  public class Idle : TimedEnemyState
  {
    public override void OnTimeout()
    {
      base.OnTimeout();
      TransitionTo(nameof(Wandering));
    }
  }
}