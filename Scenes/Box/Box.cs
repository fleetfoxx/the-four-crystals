using Enemies.DashEnemy;
using Godot;
using System;

public class Box : StaticBody2D
{
  public void Destroy()
  {
    // TODO: play animation
    QueueFree();
  }
}
