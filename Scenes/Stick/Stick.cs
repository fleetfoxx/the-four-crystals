using Godot;
using System;

public class Stick : Area2D, ICarrayble
{
  public void DisableInteraction()
  {
    Monitorable = false;
    Monitoring = false;
  }
}
