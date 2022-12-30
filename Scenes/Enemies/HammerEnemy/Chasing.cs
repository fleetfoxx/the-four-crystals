using Godot;
using System;

namespace Enemies.HammerEnemy
{
  public class Chasing : TimedEnemyState
  {
    private Node2D _attackTarget = null;
    private Node2D _hammerTarget = null;

    public override void Enter(params object[] args)
    {
      base.Enter(args);
      _attackTarget = (Node2D)args[0];
      _hammerTarget = (Node2D)args[1];
    }

    public override void Exit()
    {
      base.Exit();
      _attackTarget = null;
      _hammerTarget = null;
    }

    public override void Process(float delta)
    {
      base.Process(delta);
      _owner.Velocity = _hammerTarget.GlobalPosition.DirectionTo(_attackTarget.GlobalPosition).Normalized();
    }

    protected override void OnTimeout()
    {
      base.OnTimeout();
      TransitionTo(nameof(Attacking));
    }
  }
}
