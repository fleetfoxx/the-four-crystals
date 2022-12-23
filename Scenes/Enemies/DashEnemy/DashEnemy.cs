using Enemies.DashEnemy;
using Godot;
using System;
using System.Diagnostics;

namespace Enemies.DashEnemy
{
    public class DashEnemy : Enemy
    {
        private DashEnemyStateMachine _stateMachine;
        private Label _stateLabel;
        private Area2D _aggro;
        private Area2D _feetBox;

        public override void _Ready()
        {
            _stateMachine = GetNodeOrNull<DashEnemyStateMachine>("DashEnemyStateMachine");
            Debug.Assert(_stateMachine != null);

            _aggro = GetNodeOrNull<Area2D>("Aggro");
            Debug.Assert(_aggro != null);
            _aggro.Connect("area_entered", this, nameof(HandleAggroEntered));
            _aggro.Connect("area_exited", this, nameof(HandleAggroExited));

            _feetBox = GetNodeOrNull<Area2D>("FeetBox");
            Debug.Assert(_feetBox != null);
            _feetBox.Connect("area_exited", this, nameof(HandleFeetBoxExited));

            _stateLabel = GetNodeOrNull<Label>("StateLabel");
        }

        public override void _Process(float delta)
        {
            if (_stateLabel.Text != null)
            {
                _stateLabel.Text = _stateMachine.GetStateName();
            }
        }

        private void HandleAggroEntered(Area2D area)
        {
            // Debug.WriteLine(area.Name + " entered");
            _stateMachine.SetTarget(area);
            _stateMachine.TransitionTo(nameof(ChargingUp));
        }

        private void HandleAggroExited(Area2D area)
        {
            // Debug.WriteLine(area.Name + " exited");
            _stateMachine.SetTarget(null);
            _stateMachine.TransitionTo(nameof(Idle));
        }

        private void HandleFeetBoxExited(Area2D area)
        {
            if (area is Arena)
            {
                _stateMachine.TransitionTo(nameof(Falling));
            }
        }
    }
}
