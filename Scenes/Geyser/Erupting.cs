namespace Geyser
{
  public class Erupting : TimedGeyserState
  {
    public override void Enter(params object[] args)
    {
      base.Enter(args);
      
      _owner.AnimatedSprite.Frame = 2;
      _owner.WaterArea.Monitoring = true;
      _owner.WaterArea.Monitorable = true;
    }

    protected override void OnTimeout()
    {
      TransitionTo(nameof(Dormant));
    }
  }
}