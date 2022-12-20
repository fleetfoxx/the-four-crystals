using Godot;
using System;
using System.Diagnostics;

public class DashEnemyStateMachine : StateMachine<EnemyState>
{
    [Signal]
    public delegate void OnMoveSignal(Vector2 velocityDelta);

    private Enemy _self = null;
    private Node2D _target = null;

    public override void _Ready()
    {
        base._Ready();
        _self = GetParentOrNull<Enemy>();
        Debug.Assert(_self != null);
    }

    public void SetTarget(Node2D target)
    {
        _target = target;
    }

    protected override void EnterState(EnemyState nextState)
    {
        nextState.Connect(nameof(EnemyState.MoveSignal), this, nameof(HandleMoveSignal));
        nextState.Init(_self, _target);
        base.EnterState(nextState);
    }

    protected override void ExitState()
    {
        _currentState.Disconnect(nameof(EnemyState.MoveSignal), this, nameof(HandleMoveSignal));

        base.ExitState();
    }

    private void HandleMoveSignal(Vector2 velocityDelta)
    {
        EmitSignal(nameof(OnMoveSignal), velocityDelta);
    }
}
