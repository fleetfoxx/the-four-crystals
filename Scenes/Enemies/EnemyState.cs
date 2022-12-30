using Godot;
using System;
using System.Diagnostics;

namespace Enemies
{
  public abstract class EnemyState : StateNode
  {
    protected IEnemy _owner;

    public override void Enter(params object[] args)
    {
      _owner = GetOwnerOrNull<IEnemy>();
      Debug.Assert(_owner != null);
    }
  }
}
