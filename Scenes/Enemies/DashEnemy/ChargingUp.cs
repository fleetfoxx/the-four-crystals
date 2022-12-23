using System.Diagnostics;
using Godot;

namespace Enemies.DashEnemy
{
    public class ChargingUp : EnemyState
    {
        [Export]
        private float _duration = 5;

        private Timer _timer;

        public override void _Ready()
        {
            base._Ready();

            _timer = GetNodeOrNull<Timer>("TransitionTimer");
            Debug.Assert(_timer != null);
            _timer.Connect("timeout", this, nameof(OnTimeout));
        }

        public override void Enter()
        {
            base.Enter();
            _timer.Start(_duration);
            _owner.Velocity = Vector2.Zero;
        }

        public override void Exit() {
            base.Exit();
            _timer.Stop();
        }

        private void OnTimeout()
        {
            TransitionTo(nameof(Attacking));
        }
    }
}
