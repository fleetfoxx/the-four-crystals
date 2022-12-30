using System.Diagnostics;
using Godot;

namespace Enemies.DashEnemy
{
  public class Attacking : TimedEnemyState
  {
    private Vector2 _attackDirection = Vector2.Zero;
    private Node2D _target;

    public override void Enter(params object[] args)
    {
      base.Enter(args);
      _target = (Node2D)args[0];
      _attackDirection = _owner.GlobalPosition
          .DirectionTo(_target.GlobalPosition)
          .Normalized();
    }

    public override void Process(float delta)
    {
      _owner.Velocity = _attackDirection * ((DashEnemy)_owner).ChargeSpeed;
    }

    protected override void OnTimeout()
    {
      TransitionTo(nameof(ChargingUp), _target);
    }
  }
}
