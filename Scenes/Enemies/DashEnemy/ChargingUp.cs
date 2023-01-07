using System.Diagnostics;
using Godot;

namespace Enemies.DashEnemy
{
  public class ChargingUp : TimedEnemyState
  {
    private Node2D _target;

    public override void Enter(params object[] args)
    {
      base.Enter(args);
      _target = (Node2D)args[0];
      _owner.Velocity = Vector2.Zero;
    }

    protected override void OnTimeout()
    {
      if (IsInstanceValid(_target))
      {
        TransitionTo(nameof(Attacking), _target);
      }
      else
      {
        TransitionTo(nameof(Idle));
      }
    }
  }
}
