using Godot;
using System;
using System.Diagnostics;

namespace Player
{
  public abstract class PlayerState : StateNode
  {
    protected Player _player;

    public override void Enter(params object[] args)
    {
      base.Enter(args);
      _player = GetOwnerOrNull<Player>();
      Debug.Assert(_player != null);
    }
  }
}
