using Godot;
using System;

public interface INoisy
{
  bool IsMakingNoise { get; }
  float Volume { get; }
}
