using Enemies.DashEnemy;
using Godot;
using System;
using System.Diagnostics;

namespace Enemies.DashEnemy
{
  public class DashEnemy : Enemy
  {
    [Export]
    public float WalkSpeed = 25f;

    [Export]
    public float ChargeSpeed = 55f;

    private Sprite _sprite;
    private DashEnemyStateMachine _stateMachine;
    private HealthBar _healthBar;
    private Area2D _aggro;
    private Area2D _feetBox;
    private Label _stateLabel;

    public override void _Ready()
    {
      _sprite = this.GetExpectedNode<Sprite>("Sprite");

      _stateMachine = this.GetExpectedNode<DashEnemyStateMachine>("DashEnemyStateMachine");

      _healthBar = this.GetExpectedNode<HealthBar>("HealthBar");

      _aggro = this.GetExpectedNode<Area2D>("Aggro");
      _aggro.Connect("body_entered", this, nameof(HandleAggroBodyEntered));
      _aggro.Connect("body_exited", this, nameof(HandleAggroBodyExited));

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
    }

    private void HandleAggroBodyEntered(Node node)
    {
      if (node is Player.Player)
      {
        _stateMachine.TransitionTo(nameof(ChargingUp), node);
      }
    }

    private void HandleAggroBodyExited(Node node)
    {
      if (node is Player.Player)
      {
        _stateMachine.TransitionTo(nameof(Idle));
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
