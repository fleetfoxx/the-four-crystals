namespace Geyser
{
  public class Steaming : TimedGeyserState
  {
    public override void Enter(params object[] args)
    {
      base.Enter(args);
      _owner.AnimatedSprite.Frame = 1;
    }

    protected override void OnTimeout()
    {
      TransitionTo(nameof(Erupting));
    }
  }
}