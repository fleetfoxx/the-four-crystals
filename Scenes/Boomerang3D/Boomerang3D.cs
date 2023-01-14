using Godot;
using System;

public class Boomerang3D : KinematicBody
{
  public bool IsOnFire { get; set; } = false;

  private BoomerangPath _path = null;
  private Area _collider;
  private MeshInstance _mesh;

  public override void _Ready()
  {
    _collider = this.GetExpectedNode<Area>("Collider");
    _collider.Connect("area_entered", this, nameof(HandleAreaEntered));
    _collider.Connect("body_entered", this, nameof(HandleBodyEntered));

    _mesh = this.GetExpectedNode<MeshInstance>("Mesh");
  }

  public override void _Process(float delta)
  {
    base._Process(delta);

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
    base._PhysicsProcess(delta);

    if (_path != null)
    {
      // TODO: update to 3D rotation
      var newY = RotationDegrees.y + _path.Speed * delta;
      RotationDegrees = new Vector3(RotationDegrees.x, newY, RotationDegrees.z);
    }
  }

  public void Throw(BoomerangPath path)
  {
    // TODO
    // _path = path;
    // _collider = this.GetExpectedNode<Area>("Collider");
    // _collider.Position = new Vector2(path.Radius, 0);
    // RotationDegrees = path.InitialAngle;
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
