using Godot;
using System;
using System.Diagnostics;

namespace Enemies.TreeEnemy
{
  public class TreeEnemy : Enemy
  {
    [Export]
    private float _speed = 25;

    private Area2D _awarenessArea;
    private ColorRect _leaves;

    public override void _Ready()
    {
      base._Ready();

      _awarenessArea = this.GetExpectedNode<Area2D>("AwarenessArea");
      _leaves = this.GetExpectedNode<ColorRect>("Leaves");
    }

    public override void _Process(float delta)
    {
      base._Process(delta);

      var overlappingBodies = _awarenessArea.GetOverlappingBodies();
      var isChasing = false;

      if (overlappingBodies.Count > 0)
      {
        foreach (PhysicsBody2D body in overlappingBodies)
        {
          if (body is TestPlayer)
          {
            var player = (TestPlayer)body;
            if (!player.IsInvisible && player.Carrying is Stick)
            {
              Velocity = GlobalPosition.DirectionTo(player.GlobalPosition) * _speed;
              isChasing = true;
            }
          }
        }
      }

      if (!isChasing)
      {
        Velocity = Vector2.Zero;
      }

      UpdateColor(isChasing);
    }

    private void UpdateColor(bool isChasing)
    {
      if (isChasing)
      {
        _leaves.Color = Color.Color8(165, 57, 28);
      }
      else
      {
        _leaves.Color = Color.Color8(0, 102, 25);
      }
    }
  }
}