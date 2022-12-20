using Enemies.DashEnemy;
using Godot;
using System;
using System.Diagnostics;

public class DashEnemy : Enemy
{
    private const float BASICALLY_ZERO = 0.01f;

    [Export]
    private float Speed = 50;

    [Export]
    private float Friction = 0.25f;

    public Vector2 Velocity = Vector2.Zero;

    private DashEnemyStateMachine _stateMachine;
    private Label _stateLabel;
    private Area2D _aggro;

    public override void _Ready()
    {
        _stateMachine = GetNodeOrNull<DashEnemyStateMachine>("DashEnemyStateMachine");
        Debug.Assert(_stateMachine != null);
        _stateMachine.Connect(nameof(DashEnemyStateMachine.OnMoveSignal), this, nameof(HandleMove));

        _aggro = GetNodeOrNull<Area2D>("Aggro");
        Debug.Assert(_aggro != null);
        _aggro.Connect("area_entered", this, nameof(HandleAggroEntered));
        _aggro.Connect("area_exited", this, nameof(HandleAggroExited));

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

    private void HandleAggroEntered(Area2D area)
    {
        Debug.WriteLine(area.Name + " entered");
        _stateMachine.SetTarget(area);
        _stateMachine.TransitionTo(nameof(Attacking));
    }

    private void HandleAggroExited(Area2D area)
    {
        Debug.WriteLine(area.Name + " exited");
        _stateMachine.SetTarget(null);
        _stateMachine.TransitionTo(nameof(Idle));
    }
}
