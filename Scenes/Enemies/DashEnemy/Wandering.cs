using Godot;

namespace Enemies.DashEnemy
{
    public class Wandering : EnemyState
    {
        private Timer _timer;

        public override void Enter()
        {
            _timer = new Timer();
            _timer.WaitTime = 10;
            _timer.OneShot = true;
            AddChild(_timer);
            _timer.Connect("timeout", this, nameof(OnTimeout));
            _timer.Start();
        }

        public override void Exit()
        {
            RemoveChild(_timer);
        }

        private void OnTimeout()
        {
            TransitionTo(nameof(Idle));
        }
    }
}
