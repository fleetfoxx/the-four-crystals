namespace Geyser
{
  public abstract class GeyserState : StateNode
  {
    protected Geyser _owner;

    public override void Enter(params object[] args)
    {
      _owner = this.GetExpectedOwner<Geyser>();
      base.Enter(args);
    }
  }
}