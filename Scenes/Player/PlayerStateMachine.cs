using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;

public class PlayerStateMachine : StateMachine<PlayerState>
{
    [Signal]
    public delegate void OnMoveSignal(Vector2 velocityDelta);

    private Player _player;

    public override void _Ready()
    {
        _player = GetParentOrNull<Player>();
        Debug.Assert(_player != null);

        base._Ready();
    }

    protected override void EnterState(PlayerState nextState)
    {
        nextState.Init(_player);

        nextState.Connect(
            nameof(PlayerState.MovePlayerSignal),
            this,
            nameof(HandleMovePlayerSignal)
        );

        base.EnterState(nextState);
    }

    protected override void ExitState()
    {
        _currentState.Disconnect(
            nameof(PlayerState.MovePlayerSignal),
            this,
            nameof(HandleMovePlayerSignal)
        );

        base.ExitState();
    }

    private void HandleMovePlayerSignal(Vector2 velocityDelta)
    {
        EmitSignal(nameof(OnMoveSignal), velocityDelta);
    }
}
