using Godot;

public class Burning : Node2D
{
  [Signal]
  public delegate void DamageSignal(int damage);

  [Export]
  private float _frequency = 2;

  [Export]
  private int _damage = 1;

  [Export]
  private float _duration = 4;

  private Timer _durationTimer;
  private Timer _pingTimer;

  public override void _Ready()
  {
    base._Ready();

    _durationTimer = new Timer();
    _durationTimer.OneShot = true;
    _durationTimer.Connect("timeout", this, nameof(OnDurationTimeout));
    AddChild(_durationTimer);
    _durationTimer.Start(_duration);

    _pingTimer = new Timer();
    _pingTimer.OneShot = false;
    _pingTimer.Connect("timeout", this, nameof(OnPingTimeout));
    AddChild(_pingTimer);
    _pingTimer.Start(_frequency);
  }

  private void OnDurationTimeout()
  {
    QueueFree();
  }

  private void OnPingTimeout()
  {
    EmitSignal(nameof(DamageSignal), _damage);
  }
}
