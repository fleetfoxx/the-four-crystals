using Godot;

namespace Player
{
  public class KnockedBack : TimedPlayerState
  {
    private Vector2 _force = Vector2.Zero;

    public override void Enter(params object[] args)
    {
      base.Enter(args);
      _force = (Vector2)args[0];
    }

    public override void Process(float delta)
    {
      base.Process(delta);
      _player.Velocity = _force;
    }

    protected override void OnTimeout()
    {
      base.OnTimeout();
      TransitionTo(nameof(Idle));
    }
  }
}