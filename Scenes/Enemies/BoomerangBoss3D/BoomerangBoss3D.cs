using Godot;
using System;
using System.Collections.Generic;

public class BoomerangBoss3D : KinematicBody, IDamageable
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
  private float _minBoomerangDistance = 2;

  private HealthBar3D _healthBar;
  private Timer _throwTimer;
  private Spatial _target;
  private Area _throwCatchArea;
  // Dictionary<instance, wasThrown>
  private Dictionary<Boomerang3D, bool> _activeBoomerangs = new Dictionary<Boomerang3D, bool>();
  private int _maxActiveBoomerangs = 1;

  [Export]
  public int MaxHealth { get; set; } = 6;
  public int Health { get; set; } = 6;

  public void ApplyDamage(Node source, int amount)
  {
    if (source is Boomerang3D && ((Boomerang3D)source).IsOnFire)
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

  public override void _Ready()
  {
    _target = GetNode<Spatial>("../Test3DPlayer");

    _throwCatchArea = this.GetExpectedNode<Area>("ThrowCatchArea");
    _throwCatchArea.Connect("area_entered", this, nameof(HandleAreaEntered));
    _throwCatchArea.Connect("area_exited", this, nameof(HandleAreaExited));

    _throwTimer = this.GetExpectedNode<Timer>("ThrowTimer");
    _throwTimer.Connect("timeout", this, nameof(ThrowBoomerangAtTarget));
    _throwTimer.Start(_throwInterval);

    _healthBar = this.GetExpectedNode<HealthBar3D>("HealthBar3D");
  }

  public override void _Process(float delta)
  {
    _healthBar.Health = Health;
    _healthBar.MaxHealth = MaxHealth;
  }

  private void HandleAreaEntered(Area2D area)
  {
    if (area.Owner is Boomerang3D && _activeBoomerangs[(Boomerang3D)area.Owner])
    {
      // "Catch" boomerang.
      var boomerang = (Boomerang3D)area.Owner;
      boomerang.Catch();
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
    boomerang.Translation = new Vector3(boomerang.Translation.x, 0.5f, boomerang.Translation.z);
    AddChild(boomerang);
    boomerang.ThrowAt(_target, _boomerangSpeed, _minBoomerangDistance);

    _activeBoomerangs.Add(boomerang, false);
  }
}
