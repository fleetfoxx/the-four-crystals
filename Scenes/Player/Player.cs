using Godot;
using System;
using System.Diagnostics;

namespace Player
{
  public class Player : KinematicBody2D
  {
    [Signal]
    public delegate void HealthChangedSignal(int health);

    [Signal]
    public delegate void DeathSignal();

    [Export]
    private int _maxHealth = 5;
    private int _health = 0;
    public int Health
    {
      get { return _health; }
      set
      {
        _health = value;

        if (_health <= 0)
        {
          EmitSignal(nameof(DeathSignal));
        }
        else
        {
          EmitSignal(nameof(HealthChangedSignal), _health);
        }
      }
    }

    [Export]
    public float WalkSpeed = 150f;

    [Export]
    public float DashSpeed = 300f;

    [Export]
    public float KnockbackSpeed = 300f;


    #region Status effects
    [Export]
    private PackedScene _burningScene;
    public bool IsBurning { get => GetNodeOrNull<Burning>("Burning") != null; }
    #endregion

    public bool IsDashing { get => _stateMachine.GetState() is Dashing; }

    private Sprite _playerSprite;
    private PlayerStateMachine _stateMachine;
    private Area2D _hitBox;
    private Area2D _feetBox;
    private Label _stateLabel;

    public Vector2 Velocity = Vector2.Zero;

    public override void _Ready()
    {
      _playerSprite = this.GetExpectedNode<Sprite>("PlayerSprite");

      _stateMachine = this.GetExpectedNode<PlayerStateMachine>("PlayerStateMachine");

      _hitBox = this.GetExpectedNode<Area2D>("HitBox");
      _hitBox.Connect("area_entered", this, nameof(HandleHitBoxCollision));

      _feetBox = this.GetExpectedNode<Area2D>("FeetBox");
      _feetBox.Connect("area_entered", this, nameof(HandleFeetBoxEntered));
      _feetBox.Connect("area_exited", this, nameof(HandleFeetBoxExited));

      _stateLabel = GetNodeOrNull<Label>("StateLabel");

      Health = _maxHealth;

      // HACK: _Ready() is called before the Root node has set up the signal
      // connection so the first signal is missed. Use a timer to delay the
      // initial broadcast.
      GetTree().CreateTimer(0.1f).Connect("timeout", this, nameof(InitialBroadcast));
    }

    private void InitialBroadcast()
    {
      EmitSignal(nameof(HealthChangedSignal), Health);
    }

    public override void _Process(float delta)
    {
      if (_stateLabel.Text != null)
      {
        _stateLabel.Text = _stateMachine.GetStateName();
      }

      if (Velocity.x < 0)
      {
        _playerSprite.FlipH = true;
      }
      else if (Velocity.x > 0)
      {
        _playerSprite.FlipH = false;
      }

      CheckForStatusEffects();
    }

    public override void _PhysicsProcess(float delta)
    {
      Velocity = MoveAndSlide(Velocity);

      if (Velocity.IsEqualApprox(Vector2.Zero))
      {
        Velocity = Vector2.Zero;
      }
    }

    public void ApplyDamage(int damage)
    {
      Health -= damage;
    }

    public void ApplyKnockback(Vector2 direction, float power)
    {
      if (!(_stateMachine.GetState() is KnockedBack))
      {
        _stateMachine.TransitionTo(nameof(KnockedBack), direction, power);
      }
    }

    private void HandleHitBoxCollision(Area2D area)
    {
      // Debug.WriteLine("Player collided with: " + area.Name);
    }

    private void HandleFeetBoxEntered(Area2D area)
    {
      if (area is LavaArea)
      {
        // Debug.WriteLine("Player in lava.");
      }
    }

    private void HandleFeetBoxExited(Area2D area)
    {
      if (area is Arena)
      {
        _stateMachine.TransitionTo(nameof(Falling));
      }
    }

    private void CheckForStatusEffects()
    {
      var overlappingAreas = _feetBox.GetOverlappingAreas();

      foreach (var area in overlappingAreas)
      {
        if (area is LavaArea && !IsBurning && !IsDashing)
        {
          var burning = _burningScene.Instance<Burning>();
          burning.Connect(nameof(Burning.DamageSignal), this, nameof(HandleBurnDamage));
          AddChild(burning);
        }

        if (area is WaterTile)
        {
          if (IsBurning)
          {
            GetNode<Burning>("Burning").QueueFree();
          }
        }
      }
    }

    private void HandleBurnDamage(int damage)
    {
      ApplyDamage(damage);
    }
  }
}
