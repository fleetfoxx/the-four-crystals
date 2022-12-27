using System;
using System.Collections.Generic;
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
    private Node2D _hammerTarget;
    private Label _stateLabel;

    private List<Node2D> _attackTargets = new List<Node2D>();
    private Logger _logger = LoggerFactory.CreateLogger(typeof(HammerEnemy));

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
        _logger.Log(node.Name + " entered aggro.");
        _attackTargets.Add((Player.Player)node);
        _stateMachine.TransitionTo(nameof(Attacking));
      }
    }

    private void HandleAggroBodyExited(Node node)
    {
      if (node is Player.Player)
      {
        _logger.Log(node.Name + " exited aggro.");
        _attackTargets.Remove((Player.Player)node);
      }
    }
  }
}
