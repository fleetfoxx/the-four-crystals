using System.Diagnostics;

namespace Geyser
{
  public class Dormant : TimedGeyserState
  {
    public override void Enter(params object[] args)
    {
      base.Enter(args);

      _owner.AnimatedSprite.Frame = 0;
      _owner.WaterArea.Monitoring = false;
      _owner.WaterArea.Monitorable = false;
    }

    protected override void OnTimeout()
    {
      TransitionTo(nameof(Steaming));
    }
  }
}