using Godot;

namespace Player
{
  public class KnockedBack : TimedPlayerState
  {
    private Vector2 _direction = Vector2.Zero;
    private float _power = 0f;

    public override void Enter(params object[] args)
    {
      base.Enter(args);
      _direction = (Vector2)args[0];
      _power = (float)args[1];
    }

    public override void Process(float delta)
    {
      base.Process(delta);
      _player.Velocity = _direction * _power;
    }

    protected override void OnTimeout()
    {
      base.OnTimeout();
      TransitionBack();
    }
  }
}