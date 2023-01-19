using Godot;
using System;
using System.Diagnostics;

public class SoundGrenade : Area, INoisy
{
  [Export]
  public float LateralSpeed { get; private set; } = 5;

  [Export]
  public float MaxHeight { get; private set; } = 6;

  [Export]
  public float DetonationTime { get; private set; } = 5;

  public bool IsThrown { get; private set; } = false;
  public bool IsDetonating { get; private set; } = false;

  public bool IsMakingNoise { get; private set; } = false;

  public float Volume => 1;

  private Vector3 _target = Vector3.Zero;
  private AudioStreamPlayer3D _beep;
  private AnimationPlayer _animationPlayer;
  private MeshInstance _explosionMesh;

  public override void _Ready()
  {
    _beep = this.GetExpectedNode<AudioStreamPlayer3D>("Beep");
    _animationPlayer = this.GetExpectedNode<AnimationPlayer>("AnimationPlayer");
    _animationPlayer.Connect("animation_finished", this, nameof(HandleFinishAnimation));
    _explosionMesh = this.GetExpectedNode<MeshInstance>("Explosion");
    _explosionMesh.Mesh = (Mesh)_explosionMesh.Mesh.Duplicate();
  }

  public void Throw(Vector3 target)
  {
    IsThrown = true;
    _target = target;
    GetTree().CreateTimer(DetonationTime).Connect("timeout", this, nameof(Detonate));
  }

  public override void _Process(float delta)
  {
    if (IsThrown && GlobalTranslation.DistanceTo(_target) <= 0.1)
    {
      FinishTrajectory();
    }

    // if (IsDetonating)
    // {
    //   ((SphereMesh)_explosionMesh.Mesh).Radius += delta;
    //   ((SphereMesh)_explosionMesh.Mesh).Height += delta * 2;
    // }
  }

  public override void _PhysicsProcess(float delta)
  {
    if (IsThrown)
    {
      GlobalTranslation = GlobalTranslation.MoveToward(_target, LateralSpeed * delta);
    }
  }

  private void FinishTrajectory()
  {
    IsThrown = false;
    _target = Vector3.Zero;
  }

  private void Detonate()
  {
    _beep.Play();
    IsDetonating = true;
    IsMakingNoise = true;
    // GetTree().CreateTimer(0.2f).Connect("timeout", this, nameof(Explode));
    _animationPlayer.Play("Explosion");
  }

  private void HandleFinishAnimation(string name)
  {
    if (name == "Explosion")
    {
      Explode();
    }
  }

  private void Explode()
  {
    IsDetonating = false;
    IsMakingNoise = false;
    QueueFree();
  }
}
