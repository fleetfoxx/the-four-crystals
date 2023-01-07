using Enemies.DashEnemy;
using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Enemies.DashEnemy
{
  public class DashEnemy : Enemy
  {
    [Export]
    public float WalkSpeed = 25f;

    [Export]
    public float ChargeSpeed = 55f;

    [Export]
    public int _damage = 1;

    public bool IsDocile
    {
      get
      {
        var state = _stateMachine.GetState();
        return state is Idle || state is Wandering;
      }
    }

    private Sprite _sprite;
    private DashEnemyStateMachine _stateMachine;
    private HealthBar _healthBar;
    private Area2D _aggro;
    private Area2D _hitBox;
    private Area2D _feetBox;
    private Label _stateLabel;
    private List<Node2D> _attackTargets = new List<Node2D>();


    public override void _Ready()
    {
      _sprite = this.GetExpectedNode<Sprite>("Sprite");

      _stateMachine = this.GetExpectedNode<DashEnemyStateMachine>("DashEnemyStateMachine");

      _healthBar = this.GetExpectedNode<HealthBar>("HealthBar");

      _aggro = this.GetExpectedNode<Area2D>("Aggro");
      _aggro.Connect("body_entered", this, nameof(HandleAggroBodyEntered));
      _aggro.Connect("body_exited", this, nameof(HandleAggroBodyExited));

      _hitBox = this.GetExpectedNode<Area2D>("HitBox");
      _hitBox.Connect("body_entered", this, nameof(HandleBodyEnteredHitBox));
      _hitBox.Connect("area_entered", this, nameof(HandleAreaEnteredHitBox));

      _feetBox = this.GetExpectedNode<Area2D>("FeetBox");
      _feetBox.Connect("area_exited", this, nameof(HandleFeetBoxExited));

      _stateLabel = GetNodeOrNull<Label>("StateLabel");
    }

    public override void _Process(float delta)
    {
      if (_stateLabel != null)
      {
        _stateLabel.Text = _stateMachine.GetStateName();
      }

      if (Velocity.x < 0)
      {
        _sprite.FlipH = true;
      }
      else if (Velocity.x > 0)
      {
        _sprite.FlipH = false;
      }

      _healthBar.MaxHealth = MaxHealth;
      _healthBar.Health = Health;

      if (IsDocile && _attackTargets.Any())
      {
        while (_attackTargets.Any())
        {
          var target = _attackTargets[0];

          if (IsInstanceValid(target))
          {
            _stateMachine.TransitionTo(nameof(ChargingUp), target);
            break;
          }
          else
          {
            _attackTargets.Remove(target);
          }
        }
      }
    }

    private void HandleAggroBodyEntered(Node node)
    {
      if (node is Player.Player)
      {
        _attackTargets.Add((Player.Player)node);
      }
    }

    private void HandleAggroBodyExited(Node node)
    {
      if (node is Player.Player)
      {
        _attackTargets.Remove((Player.Player)node);
      }
    }

    private void HandleBodyEnteredHitBox(Node node)
    {
      if (node is Box)
      {
        ((Box)node).Destroy();
        _stateMachine.TransitionTo(nameof(Idle));
      }
    }

    private void HandleAreaEnteredHitBox(Area2D area)
    {
      if (area.Owner is Player.Player)
      {
        if (_stateMachine.GetState() is Attacking)
        {
          var player = (Player.Player)area.Owner;
          var direction = Velocity.Normalized();
          player.ApplyKnockback(direction * ChargeSpeed);
          player.ApplyDamage(this, _damage);
          _stateMachine.TransitionTo(nameof(ChargingUp), area.Owner);
        }
      }
    }

    private void HandleFeetBoxExited(Area2D area)
    {
      if (area is Arena)
      {
        _stateMachine.TransitionTo(nameof(Falling));
      }
    }
  }
}
