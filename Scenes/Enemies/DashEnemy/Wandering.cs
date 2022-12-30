using System.Diagnostics;
using Godot;

namespace Enemies.DashEnemy
{
  public class Wandering : TimedEnemyState
  {
    private Vector2 _wanderDirection = Vector2.Zero;

    public override void Enter(params object[] args)
    {
      base.Enter(args);

      var randomAngle = GD.Randf() * 2 * Mathf.Pi;
      _wanderDirection = Vector2.Right.Rotated(randomAngle);
    }

    public override void Process(float delta)
    {
      _owner.Velocity = _wanderDirection * ((DashEnemy)_owner).WalkSpeed;
    }

    protected override void OnTimeout()
    {
      TransitionTo(nameof(Idle));
    }
  }
}
