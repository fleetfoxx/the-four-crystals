using Godot;
using System;
using System.Diagnostics;

namespace Player
{
    public abstract class PlayerState : StateNode
    {
        protected Player _player;

        public override void Enter()
        {
            _player = GetOwnerOrNull<Player>();
            Debug.Assert(_player != null);
        }
    }
}
