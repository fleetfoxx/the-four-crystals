using Godot;
using System;

namespace Geyser
{
  public class Geyser : Node2D
  {
    public AnimatedSprite AnimatedSprite;
    public Area2D WaterArea;

    private GeyserStateMachine _stateMachine;

    public override void _Ready()
    {
      base._Ready();
      AnimatedSprite = this.GetExpectedNode<AnimatedSprite>("AnimatedSprite");
      WaterArea = this.GetExpectedNode<Area2D>("WaterArea");
      _stateMachine = this.GetExpectedNode<GeyserStateMachine>("StateMachine");
    }

    public override void _PhysicsProcess(float delta)
    {
      base._PhysicsProcess(delta);

      if (_stateMachine.GetState() is Erupting)
      {
        foreach (Area2D area in WaterArea.GetOverlappingAreas())
        {
          if (area is HitBox)
          {
            if (area.Owner is IDamageable)
            {
              ((IDamageable)area.Owner).ApplyDamage(this, 1);
            }

            if (area.Owner is IKnockable && area.Owner is Node2D)
            {
              var direction = GlobalPosition.DirectionTo(((Node2D)area.Owner).GlobalPosition);
              ((IKnockable)area.Owner).ApplyKnockback(direction * 500);
            }
          }
        }
      }
    }
  }
}