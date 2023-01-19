using Godot;
using System;
using System.Diagnostics;

public class Boomerang3D : Path, IFlammable
{
  public bool IsOnFire { get; set; } = false;
  public bool IsThrown { get; private set; } = false;

  private float _speed;
  private Area _collider;
  private MeshInstance _mesh;
  private PathFollow _pathFollow;

  public override void _Ready()
  {
    _collider = this.GetExpectedNode<Area>("PathFollow/Collider");
    _collider.Connect("area_entered", this, nameof(HandleAreaEntered));
    _collider.Connect("body_entered", this, nameof(HandleBodyEntered));

    _mesh = this.GetExpectedNode<MeshInstance>("PathFollow/Collider/Mesh");

    _pathFollow = this.GetExpectedNode<PathFollow>("PathFollow");
  }

  public override void _Process(float delta)
  {
    if (IsOnFire)
    {
      var material = new SpatialMaterial();
      material.AlbedoColor = Color.Color8(255, 137, 0);
      _mesh.MaterialOverride = material;
    }
    else
    {
      var material = new SpatialMaterial();
      material.AlbedoColor = Color.Color8(255, 255, 255);
      _mesh.MaterialOverride = material;
    }
  }

  public override void _PhysicsProcess(float delta)
  {
    if (IsThrown)
    {
      _pathFollow.Offset += _speed * delta;
    }
  }

  public void ThrowAt(Spatial target, float speed, float minDistance = 1)
  {
    IsThrown = true;

    var direction = GlobalTranslation.DirectionTo(target.GlobalTranslation);
    var distance = GlobalTranslation.DistanceTo(target.GlobalTranslation);

    if (distance < minDistance)
    {
      distance = minDistance;
    }

    _speed = speed / distance;
    Scale = new Vector3(distance, distance, distance);
    _pathFollow.Scale = Scale.Inverse();
    var lookAt = new Vector3(target.GlobalTranslation.x, GlobalTranslation.y, target.GlobalTranslation.z);
    LookAt(lookAt, Vector3.Up);
  }

  public void Catch()
  {
    IsThrown = false;
    _speed = 0;
  }

  private void HandleAreaEntered(Area2D area)
  {
    InflictDamage(area);
    InflictDestruction(area);
  }

  private void HandleBodyEntered(Node body)
  {
    InflictDamage(body);
    InflictDestruction(body);
  }

  private void InflictDamage(Node node)
  {
    var damage = IsOnFire ? 2 : 1;

    if (node is IDamageable)
    {
      ((IDamageable)node).ApplyDamage(this, damage);
    }

    if (node.Owner is IDamageable)
    {
      ((IDamageable)node.Owner).ApplyDamage(this, damage);
    }
  }

  private void InflictDestruction(Node node)
  {
    if (node is IDestructible)
    {
      ((IDestructible)node).Destroy(this);
    }

    if (node.Owner is IDestructible)
    {
      ((IDestructible)node.Owner).Destroy(this);
    }
  }
}
