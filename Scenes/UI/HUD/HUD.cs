using Godot;
using System;

public class HUD : CanvasLayer
{
  [Export]
  private PackedScene _healthPipScene;

  private int _playerHealth = 0;
  private VBoxContainer _healthContainer;

  public override void _Ready()
  {
    _healthContainer = this.GetExpectedNode<VBoxContainer>("HealthContainer");
  }

  public void SetPlayerHealth(int health)
  {
    _playerHealth = health;

    foreach (Node child in _healthContainer.GetChildren())
    {
      _healthContainer.RemoveChild(child);
    }

    for (var i = 0; i < health; i++)
    {
      var pip = _healthPipScene.Instance();
      _healthContainer.AddChild(pip);
    }
  }
}
