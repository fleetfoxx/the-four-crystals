using System;
using System.Diagnostics;
using Godot;

namespace Enemies.HammerEnemy
{
  public class HammerEnemy : EnemyArea2D
  {
    private enum Animations
    {
      Idle,
      Attack
    }

    private HammerEnemyStateMachine _stateMachine;
    private AnimationPlayer _animationPlayer;
    private Label _stateLabel;

    public override void _Ready()
    {
      base._Ready();

      _stateMachine = GetNodeOrNull<HammerEnemyStateMachine>(nameof(HammerEnemyStateMachine));
      Debug.Assert(_stateMachine != null);
      _stateMachine.Connect(nameof(HammerEnemyStateMachine.OnStateTransition), this, nameof(HandleStateTransition));

      _animationPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
      Debug.Assert(_animationPlayer != null);
      _animationPlayer.Connect("animation_finished", this, nameof(HandleAnimationFinished));

      _stateLabel = GetNodeOrNull<Label>("StateLabel");
    }

    public override void _Process(float delta)
    {
      if (_stateLabel != null)
      {
        _stateLabel.Text = _stateMachine.GetStateName();
      }
    }

    private void HandleStateTransition(EnemyState prev, EnemyState next)
    {
      if (next is Idle || next is Wandering)
      {
        _animationPlayer.Play(nameof(Animations.Idle));
      }

      if (next is Attacking)
      {
        _animationPlayer.Play(nameof(Animations.Attack));
      }
    }

    private void HandleAnimationFinished(string anim_name)
    {
      Debug.WriteLine(anim_name);
      if (anim_name == nameof(Animations.Attack))
      {
        _stateMachine.TransitionTo(nameof(Idle));
      }
    }
  }
}
