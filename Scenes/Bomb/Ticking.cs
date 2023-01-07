namespace Bomb
{
  public class Ticking : TimedStateNode
  {
    private Bomb _owner;

    public override void Enter(params object[] args)
    {
      base.Enter(args);
      _owner = this.GetExpectedOwner<Bomb>();
      _owner.AnimatedSprite.Frame = 1;
    }

    protected override void OnTimeout()
    {
      base.OnTimeout();
      TransitionTo(nameof(Exploding));
    }
  }
}