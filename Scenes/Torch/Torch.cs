using Godot;
using System;

public class Torch : Area2D
{
  public override void _Ready()
  {
    base._Ready();
    Connect("area_entered", this, nameof(HandleAreaEntered));
  }

  public void HandleAreaEntered(Area2D area)
  {
    if (area is IFlammable)
    {
      ((IFlammable)area).IsOnFire = true;
    }

    if (area.Owner is IFlammable)
    {
      ((IFlammable)area.Owner).IsOnFire = true;
    }
  }
}
