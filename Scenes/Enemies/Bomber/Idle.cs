using Godot;
using System;

namespace Enemies.Bomber
{
  public class Idle : TimedEnemyState
  {
    protected override void OnTimeout()
    {
      base.OnTimeout();
      TransitionTo(nameof(Throwing));
    }
  }
}