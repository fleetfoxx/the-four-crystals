using Godot;
using System;
using System.Diagnostics;

public class Player : KinematicBody2D
{
    private const float BASICALLY_ZERO = 0.01f;

    [Export]
    private float Speed = 50;

    [Export]
    public readonly float DodgeSpeed = 75;

    [Export]
    private float Friction = 0.25f;

    private PlayerStateMachine _stateMachine;
    private Area2D _hitBox;
    private Label _stateLabel;

    public Vector2 Velocity = Vector2.Zero;

    public override void _Ready()
    {
        _stateMachine = GetNodeOrNull<PlayerStateMachine>("PlayerStateMachine");
        Debug.Assert(_stateMachine != null);
        _stateMachine.Connect(nameof(PlayerStateMachine.OnMoveSignal), this, nameof(HandleMove));

        _hitBox = GetNodeOrNull<Area2D>("HitBox");
        Debug.Assert(_hitBox != null);
        _hitBox.Connect("area_entered", this, nameof(HandleHitBoxCollision));

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
        Velocity = Velocity.LinearInterpolate(Vector2.Zero, Friction);
        Velocity = MoveAndSlide(Velocity);

        // Snap to 0 when the lerp gets small.
        if (Velocity.x < BASICALLY_ZERO && Velocity.x > -BASICALLY_ZERO)
        {
            Velocity.x = 0;
        }

        if (Velocity.y < BASICALLY_ZERO && Velocity.y > -BASICALLY_ZERO)
        {
            Velocity.y = 0;
        }
    }

    private void HandleMove(Vector2 velocityDelta)
    {
        Velocity += velocityDelta * Speed;
    }

    private void HandleHitBoxCollision(Area2D area) { 
        Debug.WriteLine(area.Name);
    }
}
