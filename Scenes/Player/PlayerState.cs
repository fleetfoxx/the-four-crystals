using Godot;
using System;

public abstract class PlayerState : StateNode
{
    [Signal]
    public delegate void MovePlayerSignal(Vector2 velocityDelta);

    protected void MovePlayer(Vector2 velocityDelta)
    {
        EmitSignal(nameof(MovePlayerSignal), velocityDelta);
    }

    public virtual void Init(Player player) { }
}
