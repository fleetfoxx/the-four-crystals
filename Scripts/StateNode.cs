using Godot;
using System;

public abstract class StateNode : Node
{
  protected object[] _args = Array.Empty<object>();

  [Signal]
  public delegate void TransitionToSignal(NodePath nextState, object[] args);

  [Signal]
  public delegate void TransitionBackSignal();

  protected void TransitionTo(NodePath nextState, params object[] args)
  {
    EmitSignal(nameof(TransitionToSignal), nextState, args);
  }

  protected void TransitionBack()
  {
    EmitSignal(nameof(TransitionBackSignal));
  }

  public virtual void Enter(params object[] args)
  {
    _args = args;
  }

  public virtual void Exit() { }

  public virtual void Process(float delta) { }

  public virtual void PhysicsProcess(float delta) { }

  public virtual void OnInput(InputEvent e) { }

  public virtual void UnhandledInput(InputEvent e) { }

  public virtual void UnhandledKeyInput(InputEventKey e) { }
}
