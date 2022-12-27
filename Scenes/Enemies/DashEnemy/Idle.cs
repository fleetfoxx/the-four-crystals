using System.Diagnostics;
using Godot;

namespace Enemies.DashEnemy
{
  public class Idle : EnemyState
  {
    [Export]
    private float _idleDuration = 5;

    private Timer _timer;

    public override void _Ready()
    {
      base._Ready();

      _timer = GetNodeOrNull<Timer>("TransitionTimer");
      Debug.Assert(_timer != null);
      _timer.Connect("timeout", this, nameof(OnTimeout));
    }

    public override void Enter(params object[] args)
    {
      base.Enter(args);
      _timer.Start(_idleDuration);
      _owner.Velocity = Vector2.Zero;
    }

    private void OnTimeout()
    {
      TransitionTo(nameof(Wandering));
    }
  }
}
