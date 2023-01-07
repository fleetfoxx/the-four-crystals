using Godot;

public abstract class TimedStateNode : StateNode
{
  [Export]
  public float Duration { get; set; } = 1f;

  private Timer _timer;

  public override void _Ready()
  {
    base._Ready();
    _timer = new Timer();
    _timer.OneShot = true;
    _timer.Connect("timeout", this, nameof(OnTimeout));
    AddChild(_timer);
  }

  public override void Enter(params object[] args)
  {
    base.Enter(args);
    _timer.Start(Duration);
  }

  public override void Exit()
  {
    base.Exit();
    _timer.Stop();
  }

  protected virtual void OnTimeout() { }
}