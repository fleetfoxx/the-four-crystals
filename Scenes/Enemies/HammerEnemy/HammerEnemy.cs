using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Godot;

namespace Enemies.HammerEnemy
{
  public class HammerEnemy : EnemyArea2D
  {
    // These should match the animation names in the AnimationPlayer.
    private enum Animations
    {
      Idle,
      Chase,
      Attack
    }

    private HammerEnemyStateMachine _stateMachine;
    private AnimationPlayer _animationPlayer;
    private Area2D _aggroArea;
    private Node2D _hammerTarget;
    private Label _stateLabel;

    private Logger _logger = LoggerFactory.CreateLogger(typeof(HammerEnemy));
    private List<Node2D> _attackTargets = new List<Node2D>();


    public override void _Ready()
    {
      base._Ready();

      _stateMachine = GetExpectedNode<HammerEnemyStateMachine>(nameof(HammerEnemyStateMachine));
      _stateMachine.Connect(nameof(HammerEnemyStateMachine.OnStateTransition), this, nameof(HandleStateTransition));

      _animationPlayer = GetExpectedNode<AnimationPlayer>("AnimationPlayer");
      _animationPlayer.Connect("animation_finished", this, nameof(HandleAnimationFinished));

      _aggroArea = GetExpectedNode<Area2D>("AggroArea");
      _aggroArea.Connect("body_entered", this, nameof(HandleAggroBodyEntered));
      _aggroArea.Connect("body_exited", this, nameof(HandleAggroBodyExited));
      _aggroArea.Connect("area_entered", this, nameof(HandleAggroAreaEntered));
      _aggroArea.Connect("area_exited", this, nameof(HandleAggroAreaExited));

      _hammerTarget = GetExpectedNode<Node2D>("HammerTarget");

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

      if (next is Chasing)
      {
        _animationPlayer.Play(nameof(Animations.Chase));
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
        if (_attackTargets.Any())
        {
          _stateMachine.TransitionTo(nameof(Chasing), _attackTargets[0], _hammerTarget);
        }
        else
        {
          _stateMachine.TransitionTo(nameof(Idle));
        }
      }
    }

    private void HandleAggroAreaEntered(Area2D area)
    {
      // _logger.Log(area.Owner.Name + " entered aggro.");
    }

    private void HandleAggroAreaExited(Area2D area)
    {
      // _logger.Log(area.Owner.Name + " exited aggro.");
    }

    private void HandleAggroBodyEntered(Node node)
    {
      if (node is Player.Player)
      {
        _attackTargets.Add((Player.Player)node);

        if (_stateMachine.GetState() is Idle || _stateMachine.GetState() is Wandering)
        {
          _stateMachine.TransitionTo(nameof(Chasing), _attackTargets[0], _hammerTarget);
        }
      }
    }

    private void HandleAggroBodyExited(Node node)
    {
      if (node is Player.Player)
      {
        _attackTargets.Remove((Player.Player)node);
      }
    }
  }
}
