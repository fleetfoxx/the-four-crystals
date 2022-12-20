using Godot;
using System;

public abstract class StateNode : Node
{
    [Signal]
    public delegate void TransitionToSignal(NodePath nextState);

    [Signal]
    public delegate void TransitionBackSignal();

    protected void TransitionTo(NodePath nextState)
    {
        EmitSignal(nameof(TransitionToSignal), nextState);
    }

    protected void TransitionBack()
    {
        EmitSignal(nameof(TransitionBackSignal));
    }

    public virtual void Enter() { }

    public virtual void Exit() { }

    public virtual void Process(float delta) { }

    public virtual void PhysicsProcess(float delta) { }

    public virtual void OnInput(InputEvent e) { }

    public virtual void UnhandledInput(InputEvent e) { }

    public virtual void UnhandledKeyInput(InputEventKey e) { }
}
