using Godot;
using System;
using System.Diagnostics;

namespace Player
{
    public class Player : KinematicBody2D
    {
        private PlayerStateMachine _stateMachine;
        private Area2D _hitBox;
        private Area2D _feetBox;
        private Label _stateLabel;

        public Vector2 Velocity = Vector2.Zero;

        public override void _Ready()
        {
            _stateMachine = GetNodeOrNull<PlayerStateMachine>("PlayerStateMachine");
            Debug.Assert(_stateMachine != null);

            _hitBox = GetNodeOrNull<Area2D>("HitBox");
            Debug.Assert(_hitBox != null);
            _hitBox.Connect("area_entered", this, nameof(HandleHitBoxCollision));

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

        public override void _PhysicsProcess(float delta)
        {
            Velocity = MoveAndSlide(Velocity);

            if (Velocity.IsEqualApprox(Vector2.Zero))
            {
                Velocity = Vector2.Zero;
            }
        }

        private void HandleHitBoxCollision(Area2D area)
        {
            // Debug.WriteLine("Player collided with: " + area.Name);
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
