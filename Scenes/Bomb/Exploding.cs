using Godot;

namespace Bomb
{
  public class Exploding : TimedStateNode
  {
    private Bomb _owner;
    private Explosion _explosion = null;

    public override void Enter(params object[] args)
    {
      base.Enter(args);
      _owner = this.GetExpectedOwner<Bomb>();
      _owner.AnimatedSprite.Visible = false;
      _owner.Velocity = Vector2.Zero;

      _explosion = _owner.ExplosionScene.Instance<Explosion>();
      _explosion.Radius = 20;
      _explosion.Connect("area_entered", this, nameof(HandleAreaEnteredExplosion));
      _owner.AddChild(_explosion);
    }

    protected override void OnTimeout()
    {
      base.OnTimeout();
      _owner.QueueFree();
    }

    private void HandleAreaEnteredExplosion(Area2D area)
    {
      if (area is HitBox && area.Owner is IDamageable)
      {
        if (area.Owner is IDamageable)
        {
          ((IDamageable)area.Owner).ApplyDamage(this, _owner.Damage);
        }

        if (area.Owner is IKnockable)
        {
          var direction = _owner.GlobalPosition.DirectionTo(((Node2D)area.Owner).GlobalPosition);
          ((IKnockable)area.Owner).ApplyKnockback(direction * _owner.Force);
        }
      }
    }
  }
}