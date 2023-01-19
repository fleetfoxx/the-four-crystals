using Godot;
using System;

[Tool]
public class HealthBar3D : Position3D
{
  [Export]
  public int MaxHealth { get; set; } = 10;
  
  [Export]
  public int Health { get; set; } = 10;

  private HealthBar _healthBar = null;

  public override void _Process(float delta)
  {
	if (_healthBar == null)
	{
	  _healthBar = this.GetExpectedNode<HealthBar>("Viewport/GUI/HealthBar");
	}

	_healthBar.UpdateInfo(Health, MaxHealth);
  }
}
