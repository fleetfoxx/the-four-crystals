namespace Geyser
{
  public abstract class TimedGeyserState : TimedStateNode
  {
    protected Geyser _owner;

    public override void Enter(params object[] args)
    {
      _owner = this.GetExpectedOwner<Geyser>();
      base.Enter(args);
    }
  }
}