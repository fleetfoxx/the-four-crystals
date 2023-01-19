using Godot;
using System;

public class Log3D : Area, ICarryable
{
  public void DisableInteraction()
  {
    Monitorable = false;
    Monitoring = false;
  }
}
