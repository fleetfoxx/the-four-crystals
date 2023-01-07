using Godot;
using System;
using System.Diagnostics;

public class BoomerangPath
{
  /// <summary>
  /// The angle away from the positive x axis of the starting point on the circle in degrees.
  /// </summary>
  public float InitialAngle { get; set; }
  public float Radius { get; set; }
  public float Speed { get; set; }
}

public class Boomerang : Node2D, IFlammable
{
  public bool IsOnFire { get; set; } = false;

  private BoomerangPath _path = null;
  private Area2D _collider;
  private ColorRect _color;

  public override void _Ready()
  {
    base._Ready();

    _collider = this.GetExpectedNode<Area2D>("Collider");
    _collider.Connect("area_entered", this, nameof(HandleAreaEntered));
    _color = this.GetExpectedNode<ColorRect>("Collider/ColorRect");
  }

  public override void _Process(float delta)
  {
    base._Process(delta);

    if (IsOnFire)
    {
      _color.Color = Color.Color8(255, 137, 0);
    }
    else
    {
      _color.Color = Color.Color8(255, 255, 255);
    }
  }

  public override void _PhysicsProcess(float delta)
  {
    base._PhysicsProcess(delta);

    if (_path != null)
    {
      RotationDegrees += _path.Speed * delta;
    }
  }

  public void Throw(BoomerangPath path)
  {
    _path = path;
    _collider = this.GetExpectedNode<Area2D>("Collider");
    _collider.Position = new Vector2(path.Radius, 0);
    RotationDegrees = path.InitialAngle;
  }

  private void HandleAreaEntered(Area2D area)
  {
    if (area is IDamageable)
    {
      var damage = IsOnFire ? 2 : 1;
      ((IDamageable)area).ApplyDamage(this, damage);
    }

    if (area is IDestructible)
    {
      ((IDestructible)area).Destroy();
    }
  }
}
