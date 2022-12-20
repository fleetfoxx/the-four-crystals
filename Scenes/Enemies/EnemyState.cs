using Godot;
using System;

public abstract class EnemyState : StateNode
{
    [Signal]
    public delegate void MoveSignal(Vector2 velocityDelta);

    public virtual void Init(Enemy self, Node2D target) { }

    protected void Move(Vector2 velocityDelta)
    {
        EmitSignal(nameof(MoveSignal), velocityDelta);
    }
}
