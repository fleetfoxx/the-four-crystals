using Godot;
using System;

namespace Enemies.Bomber
{
  public class Throwing : TimedEnemyState
  {
    public override void Enter(params object[] args)
    {
      base.Enter(args);
      var owner = (Bomber)_owner;
      var bomb = owner.BombScene.Instance<Bomb.Bomb>();
      bomb.Position = Vector2.Zero;
      bomb.ApplyForce(Vector2.Down * 50);
      owner.AddChild(bomb);
    }

    protected override void OnTimeout()
    {
      base.OnTimeout();
      TransitionTo(nameof(Idle));
    }
  }
}