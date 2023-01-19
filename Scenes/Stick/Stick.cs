using Godot;
using System;

public class Stick : Area2D, ICarryable
{
  public void DisableInteraction()
  {
    Monitorable = false;
    Monitoring = false;
  }
}
