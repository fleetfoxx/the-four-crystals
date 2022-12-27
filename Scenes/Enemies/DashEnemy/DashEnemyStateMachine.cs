using Godot;
using System;
using System.Diagnostics;

namespace Enemies.DashEnemy
{
  public class DashEnemyStateMachine : StateMachine<EnemyState>
  {
    private Node2D _target = null;

    public void SetTarget(Node2D target)
    {
      _target = target;
    }

    protected override void EnterState(EnemyState nextState, params object[] args)
    {
      nextState.Init(_target);
      base.EnterState(nextState, args);
    }
  }
}
