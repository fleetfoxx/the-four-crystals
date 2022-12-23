using System.Diagnostics;
using Godot;

namespace Enemies.DashEnemy
{
    public class Attacking : EnemyState
    {
        [Export]
        private float _attackSpeed = 50;

        [Export]
        private float _attackDuration = 2;

        private Timer _timer;
        private Node2D _target;
        private Vector2 _attackDirection = Vector2.Zero;

        public override void _Ready()
        {
            base._Ready();

            _timer = GetNodeOrNull<Timer>("AttackTimer");
            Debug.Assert(_timer != null);
            _timer.Connect("timeout", this, nameof(OnTimeout));
        }

        public override void Init(Node2D target)
        {
            Debug.Assert(target != null);
            _target = target;
        }

        public override void Enter()
        {
            base.Enter();
            _timer.Start(_attackDuration);
            _attackDirection = _owner.GlobalPosition
                .DirectionTo(_target.GlobalPosition)
                .Normalized();
        }

        public override void Exit()
        {
            base.Exit();
            _timer.Stop();
        }

        public override void Process(float delta)
        {
            _owner.Velocity = _attackDirection * _attackSpeed;
        }

        private void OnTimeout()
        {
            TransitionTo(nameof(ChargingUp));
        }
    }
}
