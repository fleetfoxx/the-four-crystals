using Godot;
using System;

public interface IDamageable
{
  int Health { get; set; }
  int MaxHealth { get; set; }
  void ApplyDamage(Node source, int amount);
}
