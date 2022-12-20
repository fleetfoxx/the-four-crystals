using System.Diagnostics;
using Godot;

namespace Enemies.DashEnemy
{
    public class Attacking : EnemyState
    {
        private Timer _timer;
        private Enemy _self;
        private Node2D _target;

        public override void Init(Enemy self, Node2D target)
        {
            Debug.Assert(self != null);
            Debug.Assert(target != null);

            _self = self;
            _target = target;
        }

        public override void Enter()
        {
            _timer = new Timer();
            _timer.WaitTime = 5;
            _timer.OneShot = false;
            AddChild(_timer);
            _timer.Connect("timeout", this, nameof(Dash));
            _timer.Start();
        }

        public override void Exit()
        {
            RemoveChild(_timer);
        }

        private void Dash()
        {
            Debug.WriteLine("Dash");

            var dashDirection = _self.GlobalPosition
                .DirectionTo(_target.GlobalPosition)
                .Normalized();

            Move(dashDirection * 10);
        }
    }
}
