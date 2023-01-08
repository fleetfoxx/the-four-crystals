using Godot;
using System;
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
  private float _throwInterval = 3;

  [Export]
  private float _boomerangSpeed = 200;

  [Export]
  public int Health { get; set; } = 6;

  [Export]
  public int MaxHealth { get; set; } = 6;

  private HealthBar _healthBar;
  private Node2D _target;
  private bool _wasBoomerangThrown = false;

  public override void _Ready()
  {
    Connect("area_entered", this, nameof(HandleAreaEntered));
    Connect("area_exited", this, nameof(HandleAreaExited));

    _target = GetNode<Node2D>("../MovementTestBall");
    _healthBar = this.GetExpectedNode<HealthBar>("HealthBar");

    GetTree().CreateTimer(_throwInterval).Connect("timeout", this, nameof(ThrowBoomerangAtTarget));
  }

  public override void _Process(float delta)
  {
    base._Process(delta);

    _healthBar.UpdateInfo(Health, MaxHealth);
  }

  private void ThrowBoomerangAtTarget()
  {
    var boomerang = _boomerangScene.Instance<Boomerang>();
    var direction = GlobalPosition.DirectionTo(_target.GlobalPosition);
    var distance = GlobalPosition.DistanceTo(_target.GlobalPosition) / 2;
    var angle = Mathf.Atan2(direction.y, direction.x) * 180 / Mathf.Pi - 180; // TODO: If I reverse x and y do I get the same result as -180?
    var directionRandomization = GD.Randf() < 0.5 ? -1 : 1;
    var path = new BoomerangPath
    {
      InitialAngle = angle,
      Radius = distance,
      Speed = _boomerangSpeed * directionRandomization
  };

    AddChild(boomerang);
    boomerang.GlobalPosition = direction * distance;
    boomerang.Throw(path);
  }

  private void HandleAreaEntered(Area2D area)
  {
    if (area.Owner is Boomerang && _wasBoomerangThrown)
    {
      // "Catch" boomerang.
      var boomerang = (Boomerang)area.Owner;
      boomerang.QueueFree();

      _wasBoomerangThrown = false;
      GetTree().CreateTimer(_throwInterval).Connect("timeout", this, nameof(ThrowBoomerangAtTarget));
    }
  }

  private async void HandleAreaExited(Area2D area)
  {
    // HACK: Wait for the idle frame so we don't re-throw a boomerang that has been QueueFree'd.
    await ToSignal(GetTree(), "idle_frame");

    if (IsInstanceValid(area) && area.Owner is Boomerang)
    {
      // "Throw" boomerang.
      _wasBoomerangThrown = true;
    }
  }

  public void ApplyDamage(Node source, int amount)
  {
    if (source is Boomerang && ((Boomerang)source).IsOnFire)
    {
      Health -= amount;
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
