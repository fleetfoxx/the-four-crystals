using Godot;
using System;
using System.Diagnostics;

namespace Enemies
{
  public abstract class EnemyState : StateNode
  {
    protected IEnemy _owner;

    public override void Enter()
    {
      _owner = GetOwnerOrNull<IEnemy>();
      Debug.Assert(_owner != null);
    }

    public virtual void Init(Node2D target) { }
  }
}
