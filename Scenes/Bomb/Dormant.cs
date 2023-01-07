namespace Bomb
{
  public class Dormant : TimedStateNode
  {
    private Bomb _owner;

    public override void Enter(params object[] args)
    {
      base.Enter(args);
      _owner = this.GetExpectedOwner<Bomb>();
      _owner.AnimatedSprite.Frame = 0;
    }

    protected override void OnTimeout()
    {
      base.OnTimeout();
      TransitionTo(nameof(Ticking));
    }
  }
}