using Godot;
using System;

namespace Bomb
{
  public class Bomb : KinematicBody2D
  {
    [Export]
    public PackedScene ExplosionScene { get; set; }

    [Export]
    public int Damage { get; set; } = 1;

    [Export]
    public float Force { get; set; } = 500;

    public Vector2 Velocity { get; set; } = Vector2.Zero;

    public AnimatedSprite AnimatedSprite { get; private set; }

    private BombStateMachine _stateMachine;

    public override void _Ready()
    {
      base._Ready();
      AnimatedSprite = this.GetExpectedNode<AnimatedSprite>("AnimatedSprite");
      _stateMachine = this.GetExpectedNode<BombStateMachine>("BombStateMachine");
    }

    public override void _PhysicsProcess(float delta)
    {
      base._PhysicsProcess(delta);
      Velocity = Velocity.LinearInterpolate(Vector2.Zero, 0.2f * delta);
      Velocity = MoveAndSlide(Velocity);
      if (Velocity.IsEqualApprox(Vector2.Zero))
      {
        Velocity = Vector2.Zero;
      }
    }

    public void ApplyForce(Vector2 force)
    {
      Velocity += force;
    }
  }
}