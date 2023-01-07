using Godot;
using System;

public class Tree : Area2D, IDamageable
{
  [Export]
  private PackedScene _stickScene;

  public int Health { get; set; }

  public int MaxHealth { get; set; }

  public void ApplyDamage(Node source, int amount)
  {
    if (source is Boomerang)
    {
      DropStick();
    }
  }

  private void DropStick()
  {
    var parent = this.GetExpectedParent<Node>();
    var stick = _stickScene.Instance<Stick>();
    var x = (float)GD.RandRange(-10, 10);
    var y = (float)GD.RandRange(0, 10);
    var r = (float)GD.RandRange(0, 360);

    stick.GlobalPosition = GlobalPosition + new Vector2(x, y);
    stick.RotationDegrees = r;

    // NOTE: Defer this call to avoid error (not sure why I need to do it).
    // Possibly because this is the result of a physics collision?
    parent.CallDeferred("add_child", stick);
  }
}
