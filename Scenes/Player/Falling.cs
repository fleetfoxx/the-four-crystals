using Godot;
using System;
using System.Diagnostics;

namespace Player
{
  public class Falling : PlayerState
  {
    public override void Enter(params object[] args)
    {
      base.Enter(args);
      Debug.WriteLine("Falling");
      _player.Velocity = Vector2.Zero;
    }
  }
}
