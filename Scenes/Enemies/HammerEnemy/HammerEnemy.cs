using System;
using System.Diagnostics;
using Godot;

namespace Enemies.HammerEnemy
{
  public class HammerEnemy : EnemyArea2D
  {
    private HammerEnemyStateMachine _stateMachine;
    private Label _stateLabel;

    public override void _Ready()
    {
      base._Ready();

      _stateMachine = GetNodeOrNull<HammerEnemyStateMachine>(nameof(HammerEnemyStateMachine));
      Debug.Assert(_stateMachine != null);

      _stateLabel = GetNodeOrNull<Label>("StateLabel");
    }

    public override void _Process(float delta)
    {
      if (_stateLabel != null)
      {
        _stateLabel.Text = _stateMachine.GetStateName();
      }
    }
  }
}
