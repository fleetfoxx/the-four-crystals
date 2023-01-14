using Godot;
using System;

public class HUD : CanvasLayer
{
  [Export]
  private PackedScene _healthPipScene;

  private BoxContainer _healthContainer;
  private HealthBar _staminaBar;

  public override void _Ready()
  {
    _healthContainer = this.GetExpectedNode<BoxContainer>("HealthContainer");
    _staminaBar = this.GetExpectedNode<HealthBar>("StaminaBar");
  }

  public override void _Process(float delta)
  {
    foreach (Node child in _healthContainer.GetChildren())
    {
      _healthContainer.RemoveChild(child);
    }

    for (var i = 0; i < PlayerManager.Health; i++)
    {
      var pip = _healthPipScene.Instance();
      _healthContainer.AddChild(pip);
    }

    _staminaBar.UpdateInfo(PlayerManager.Stamina, PlayerManager.MaxStamina);
  }
}
