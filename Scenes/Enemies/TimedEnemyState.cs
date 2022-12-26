using System.Diagnostics;
using Godot;

namespace Enemies
{
  public abstract class TimedEnemyState : EnemyState
  {
    [Export]
    private float _duration = 1f;

    private Timer _timer;

    public override void _Ready()
    {
      base._Ready();
      _timer = new Timer();
      _timer.OneShot = true;
      _timer.WaitTime = _duration;
      _timer.Connect("timeout", this, nameof(OnTimeout));
      AddChild(_timer);
    }

    public override void Enter()
    {
      base.Enter();
      _timer.Start();
    }

    public override void Exit()
    {
      base.Exit();
      _timer.Stop();
    }

    public virtual void OnTimeout()
    {
      // Debug.WriteLine($"[{GetType().Name}] {nameof(OnTimeout)} fired.");
    }
  }
}