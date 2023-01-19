using Godot;
using System;
using System.Diagnostics;

public class Tree3D : StaticBody, IDamageable
{
  [Export]
  private PackedScene _logScene;

  public int Health { get; set; }

  public int MaxHealth { get; set; }

  public void ApplyDamage(Node source, int amount)
  {
    Debug.WriteLine($"Applied damage by {source.Name}");
    if (source is Boomerang3D)
    {
      DropStick();
    }
  }

  private void DropStick()
  {
    Debug.WriteLine("Dropping stick");
    var parent = this.GetExpectedParent<Node>();
    var log = _logScene.Instance<Log3D>();
    var x = (float)GD.RandRange(-3, 3);
    var z = (float)GD.RandRange(-3, 3);
    var r = (float)GD.RandRange(0, 2 * Math.PI);

    log.GlobalTranslation = GlobalTranslation + new Vector3(x, 0.5f, z);
    log.RotateY(r);

    // NOTE: Defer this call to avoid error (not sure why I need to do it).
    // Possibly because this is the result of a physics collision?
    parent.CallDeferred("add_child", log);
  }
}
