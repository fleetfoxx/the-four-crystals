using Godot;
using System;

public class NoiseMakerPathFollow : PathFollow
{
  [Export]
  private float _speed = 1;

  public override void _Process(float delta)
  {
    Offset += _speed * delta;
  }
}
