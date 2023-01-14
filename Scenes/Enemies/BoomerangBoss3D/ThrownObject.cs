using Godot;
using System;
using System.Diagnostics;

public class ThrownObject : PathFollow
{
  public bool IsThrown { get; private set; }

  private float _speed = 1;

  public override void _PhysicsProcess(float delta)
  {
    if (IsThrown)
    {
      Offset += _speed * delta;
    }
  }

  public void Throw(float speed)
  {
    IsThrown = true;
    _speed = speed;
  }

  public void Catch()
  {
    IsThrown = false;
    Offset = 0;
  }
}
