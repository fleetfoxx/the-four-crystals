using Enemies;
using Godot;
using System;

namespace Enemies.Bomber
{
  public class Bomber : EnemyArea2D
  {
    [Export]
    public PackedScene BombScene { get; set; }
  }
}