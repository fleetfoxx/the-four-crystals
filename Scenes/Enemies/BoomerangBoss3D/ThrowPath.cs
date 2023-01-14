using Godot;
using System;
using System.Diagnostics;

public class ThrowPath : Path
{
  private Spatial _target;
  private float _speed;
  private PathFollow _thrownObject;


  public override void _Ready()
  {
    _thrownObject = this.GetExpectedNode<PathFollow>("ThrownObject");
  }

  public void ThrowAt(Spatial target, float speed)
  {
    Debug.Assert(target != null);

    _target = target;
    _speed = speed;

    var direction = GlobalTranslation.DirectionTo(_target.GlobalTranslation);
    var distance = GlobalTranslation.DistanceTo(_target.GlobalTranslation);

    var scale = distance / 10;

    Scale = new Vector3(scale, Scale.y, scale);
    Rotation = direction;
  }
}
