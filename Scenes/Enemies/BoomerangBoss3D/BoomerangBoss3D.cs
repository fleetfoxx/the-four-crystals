using Godot;
using System;
using System.Collections.Generic;

public class BoomerangBoss3D : KinematicBody
{
  [Export]
  private PackedScene _boomerangScene;

  [Export]
  private float _throwInterval = 6;

  [Export]
  private float _boomerangSpeed = 200;

  private Timer _throwTimer;
  private Spatial _target;
  private Area _throwCatchArea;
  // Dictionary<instance, wasThrown>
  private Dictionary<Boomerang3D, bool> _activeBoomerangs = new Dictionary<Boomerang3D, bool>();
  private int _maxActiveBoomerangs = 1;

  public override void _Ready()
  {
    _target = GetNode<Spatial>("../Test3DPlayer");

    _throwCatchArea = this.GetExpectedNode<Area>("ThrowCatchArea");
    _throwCatchArea.Connect("area_entered", this, nameof(HandleAreaEntered));
    _throwCatchArea.Connect("area_exited", this, nameof(HandleAreaExited));

    _throwTimer = this.GetExpectedNode<Timer>("ThrowTimer");
    _throwTimer.Connect("timeout", this, nameof(ThrowBoomerangAtTarget));
    _throwTimer.Start(_throwInterval);
  }

  private void HandleAreaEntered(Area2D area)
  {
    if (area.Owner is Boomerang3D && _activeBoomerangs[(Boomerang3D)area.Owner])
    {
      // "Catch" boomerang.
      var boomerang = (Boomerang3D)area.Owner;
      _activeBoomerangs.Remove(boomerang);
      boomerang.QueueFree();
    }
  }

  private async void HandleAreaExited(Area2D area)
  {
    // HACK: Wait for the idle frame so we don't re-throw a boomerang that has been QueueFree'd.
    await ToSignal(GetTree(), "idle_frame");

    if (IsInstanceValid(area) && area.Owner is Boomerang3D)
    {
      // "Throw" boomerang.
      _activeBoomerangs[(Boomerang3D)area.Owner] = true;
    }
  }

  private void ThrowBoomerangAtTarget()
  {
    if (_activeBoomerangs.Count >= _maxActiveBoomerangs) return;
    if (_target is IInvisible && ((IInvisible)_target).IsInvisible) return;

    var boomerang = _boomerangScene.Instance<Boomerang3D>();
    var direction = GlobalTranslation.DirectionTo(_target.GlobalTranslation);
    var distance = Math.Max(GlobalTranslation.DistanceTo(_target.GlobalTranslation) / 2, 50);
    var angle = Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.Pi - 180; // TODO: If I reverse x and y do I get the same result as -180?
    var randomDirection = GD.Randf() < 0.5 ? -1 : 1;
    var speed = _boomerangSpeed / distance * randomDirection;
    var path = new BoomerangPath
    {
      InitialAngle = angle,
      Radius = distance,
      Speed = speed
    };

    AddChild(boomerang);
    boomerang.GlobalTranslation = direction * distance;
    boomerang.Throw(path);

    _activeBoomerangs.Add(boomerang, false);
  }
}
