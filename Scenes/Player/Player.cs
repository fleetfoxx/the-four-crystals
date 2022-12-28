using Godot;
using System;
using System.Diagnostics;

namespace Player
{
  public class Player : KinematicBody2D
  {
    [Signal]
    public delegate void HealthChangedSignal(int health);

    [Export]
    private int _maxHealth = 5;
    private int _health = 0;

    private PlayerStateMachine _stateMachine;
    private Area2D _hitBox;
    private Area2D _feetBox;
    private Label _stateLabel;

    public Vector2 Velocity = Vector2.Zero;

    public override void _Ready()
    {
      _stateMachine = this.GetExpectedNode<PlayerStateMachine>("PlayerStateMachine");

      _hitBox = this.GetExpectedNode<Area2D>("HitBox");
      _hitBox.Connect("area_entered", this, nameof(HandleHitBoxCollision));

      _feetBox = this.GetExpectedNode<Area2D>("FeetBox");
      _feetBox.Connect("area_exited", this, nameof(HandleFeetBoxExited));

      _stateLabel = GetNodeOrNull<Label>("StateLabel");

      _health = _maxHealth;

      // HACK: _Ready() is called before the Root node has set up the signal
      // connection so the first signal is missed. Use a timer to delay the
      // initial broadcast.
      GetTree().CreateTimer(1).Connect("timeout", this, nameof(InitialBroadcast));
    }

    private void InitialBroadcast()
    {
      EmitSignal(nameof(HealthChangedSignal), _health);
    }

    public override void _Process(float delta)
    {
      if (_stateLabel.Text != null)
      {
        _stateLabel.Text = _stateMachine.GetStateName();
      }
    }

    public override void _PhysicsProcess(float delta)
    {
      Velocity = MoveAndSlide(Velocity);

      if (Velocity.IsEqualApprox(Vector2.Zero))
      {
        Velocity = Vector2.Zero;
      }
    }

    private void HandleHitBoxCollision(Area2D area)
    {
      // Debug.WriteLine("Player collided with: " + area.Name);
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
