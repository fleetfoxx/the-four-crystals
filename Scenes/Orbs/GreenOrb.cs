using Godot;
using System;

[Tool]
public class GreenOrb : Orb
{
  protected override Color Color { get; set; } = Colors.Green;
}
