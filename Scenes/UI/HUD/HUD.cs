using Godot;
using System;

public class HUD : CanvasLayer
{
  [Export]
  private PackedScene _healthPipScene;

  private BoxContainer _healthContainer;

  public override void _Ready()
  {
    _healthContainer = this.GetExpectedNode<BoxContainer>("MarginContainer/HealthContainer");
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
  }
}
