using System.Collections.Generic;
using System.Diagnostics;
using Godot;

public class StateMachine<TState> : Node where TState : StateNode
{
    private const bool DEBUG = false;

    protected TState _currentState;
    protected Stack<string> _history = new Stack<string>();

    public TState GetState()
    {
        return _currentState;
    }

    public string GetStateName()
    {
        return _currentState.Name;
    }

    public void TransitionTo(NodePath nextState)
    {
        var nextStateNode = GetNodeOrNull<TState>(nextState);
        Debug.Assert(nextStateNode != null);
        ExitState();
        EnterState(nextStateNode);
    }

    public void TransitionBack()
    {
        if (_history.Count > 0)
        {
            var previousState = GetNodeOrNull<TState>(_history.Pop());

            Debug.Assert(previousState != null);

            if (DEBUG)
            {
                Debug.WriteLine("Transitioning back to: " + previousState.Name);
            }

            ExitState();
            EnterState(previousState);
        }
    }

    public override void _Ready()
    {
        var initialState = GetChildOrNull<TState>(0);
        Debug.Assert(initialState != null);

        EnterState(initialState);
    }

    public override void _Process(float delta)
    {
        _currentState.Process(delta);
    }

    public override void _PhysicsProcess(float delta)
    {
        _currentState.PhysicsProcess(delta);
    }

    public override void _Input(InputEvent e)
    {
        _currentState.OnInput(e);
    }

    public override void _UnhandledInput(InputEvent e)
    {
        _currentState.UnhandledInput(e);
    }

    public override void _UnhandledKeyInput(InputEventKey e)
    {
        _currentState.UnhandledKeyInput(e);
    }

    protected virtual void EnterState(TState nextState)
    {
        if (DEBUG)
        {
            Debug.WriteLine("Entering state: " + nextState.Name);
        }

        _currentState = nextState;
        _currentState.Connect(
            nameof(StateNode.TransitionToSignal),
            this,
            nameof(HandleTransitionTo)
        );
        _currentState.Connect(
            nameof(StateNode.TransitionBackSignal),
            this,
            nameof(HandleTransitionBack)
        );
        _currentState.Enter();
    }

    protected virtual void ExitState()
    {
        if (DEBUG)
        {
            Debug.WriteLine("Exiting state: " + _currentState.Name);
        }

        _history.Push(_currentState.Name);

        _currentState.Disconnect(
            nameof(StateNode.TransitionToSignal),
            this,
            nameof(HandleTransitionTo)
        );

        _currentState.Disconnect(
            nameof(StateNode.TransitionBackSignal),
            this,
            nameof(HandleTransitionBack)
        );

        _currentState.Exit();
    }

    private void HandleTransitionTo(NodePath nextState)
    {
        TransitionTo(nextState);
    }

    private void HandleTransitionBack()
    {
        TransitionBack();
    }
}
