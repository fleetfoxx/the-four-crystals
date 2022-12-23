using System.Diagnostics;
using Godot;

namespace Enemies.DashEnemy
{
    public class Wandering : EnemyState
    {
        [Export]
        private float _wanderSpeed = 10;

        [Export]
        private float _wanderDuration = 10;

        private Timer _timer;
        private Vector2 _wanderDirection = Vector2.Zero;

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
            _timer.Start(_wanderDuration);

            var randomAngle = GD.Randf() * 2 * Mathf.Pi;
            _wanderDirection = Vector2.Right.Rotated(randomAngle);
        }

        public override void Process(float delta)
        {
            _owner.Velocity = _wanderDirection * _wanderSpeed;
        }

        private void OnTimeout()
        {
            TransitionTo(nameof(Idle));
        }
    }
}
