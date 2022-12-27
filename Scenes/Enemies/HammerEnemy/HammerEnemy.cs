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
    private Area2D _aggroArea;
    private Label _stateLabel;
    private Logger _logger = LoggerFactory.CreateLogger(typeof(HammerEnemy));

    public override void _Ready()
    {
      base._Ready();

      _stateMachine = GetNodeOrNull<HammerEnemyStateMachine>(nameof(HammerEnemyStateMachine));
      Debug.Assert(_stateMachine != null);
      _stateMachine.Connect(nameof(HammerEnemyStateMachine.OnStateTransition), this, nameof(HandleStateTransition));

      _animationPlayer = GetNodeOrNull<AnimationPlayer>("AnimationPlayer");
      Debug.Assert(_animationPlayer != null);
      _animationPlayer.Connect("animation_finished", this, nameof(HandleAnimationFinished));

      _aggroArea = GetExpectedNode<Area2D>("AggroArea");
      _aggroArea.Connect("area_entered", this, nameof(HandleAggroAreaEntered));
      _aggroArea.Connect("area_exited", this, nameof(HandleAggroAreaExited));

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
      if (anim_name == nameof(Animations.Attack))
      {
        _stateMachine.TransitionTo(nameof(Idle));
      }
    }

    private void HandleAggroAreaEntered(Area2D area)
    {
      _logger.Log(area.Owner.Name + " entered aggro.");
    }

    private void HandleAggroAreaExited(Area2D area)
    {
      _logger.Log(area.Owner.Name + " exited aggro.");
    }
  }
}
