using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public class BoomerangBoss : Area2D, IDamageable
{
  [Signal]
  public delegate void DeathSignal();

  [Export]
  private PackedScene _boomerangScene;

  [Export]
  private PackedScene _greenOrbScene;

  [Export]
  private float _throwInterval = 6;

  [Export]
  private float _boomerangSpeed = 200;

  [Export]
  public int Health { get; set; } = 6;

  [Export]
  public int MaxHealth { get; set; } = 6;

  private Timer _throwTimer;
  private HealthBar _healthBar;
  private Node2D _target;
  // Dictionary<instance, wasThrown>
  private Dictionary<Boomerang, bool> _activeBoomerangs = new Dictionary<Boomerang, bool>();
  private int _maxActiveBoomerangs = 1;

  public override void _Ready()
  {
    Connect("area_entered", this, nameof(HandleAreaEntered));
    Connect("area_exited", this, nameof(HandleAreaExited));

    _target = GetNode<Node2D>("../MovementTestBall");
    _healthBar = this.GetExpectedNode<HealthBar>("HealthBar");

    _throwTimer = this.GetExpectedNode<Timer>("ThrowTimer");
    _throwTimer.Connect("timeout", this, nameof(ThrowBoomerangAtTarget));
    _throwTimer.Start(_throwInterval);
  }

  public override void _Process(float delta)
  {
    base._Process(delta);

    _healthBar.UpdateInfo(Health, MaxHealth);
  }

  private void ThrowBoomerangAtTarget()
  {
    if (_activeBoomerangs.Count >= _maxActiveBoomerangs) return;

    var boomerang = _boomerangScene.Instance<Boomerang>();
    var direction = GlobalPosition.DirectionTo(_target.GlobalPosition);
    var distance = Math.Max(GlobalPosition.DistanceTo(_target.GlobalPosition) / 2, 50);
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
    boomerang.GlobalPosition = direction * distance;
    boomerang.Throw(path);

    _activeBoomerangs.Add(boomerang, false);
  }

  private void HandleAreaEntered(Area2D area)
  {
    if (area.Owner is Boomerang && _activeBoomerangs[(Boomerang)area.Owner])
    {
      // "Catch" boomerang.
      var boomerang = (Boomerang)area.Owner;
      _activeBoomerangs.Remove(boomerang);
      boomerang.QueueFree();
    }
  }

  private async void HandleAreaExited(Area2D area)
  {
    // HACK: Wait for the idle frame so we don't re-throw a boomerang that has been QueueFree'd.
    await ToSignal(GetTree(), "idle_frame");

    if (IsInstanceValid(area) && area.Owner is Boomerang)
    {
      // "Throw" boomerang.
      _activeBoomerangs[(Boomerang)area.Owner] = true;
    }
  }

  public void ApplyDamage(Node source, int amount)
  {
    if (source is Boomerang && ((Boomerang)source).IsOnFire)
    {
      Health -= amount;
      _maxActiveBoomerangs++;
      _throwTimer.WaitTime = _throwInterval / _maxActiveBoomerangs;
    }

    if (Health <= 0)
    {
      var orb = _greenOrbScene.Instance();
      GetParent().AddChild(orb);

      EmitSignal(nameof(DeathSignal));
      QueueFree();
    }
  }
}
