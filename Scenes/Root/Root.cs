using Godot;
using System;
using System.Diagnostics;

public class Root : Node2D
{
  private Player.Player _player;
  private HUD _hud;

  public override void _Ready()
  {
    GD.Randomize();

    _player = this.GetExpectedNode<Player.Player>("Player");
    _player.Connect(nameof(Player.Player.HealthChangedSignal), this, nameof(HandlePlayerHealthChanged));

    _hud = this.GetExpectedNode<HUD>("HUD");
  }

  public override void _Process(float delta)
  {
    base._Process(delta);
    if (Input.IsActionJustPressed("quit"))
    {
      GetTree().Quit();
    }
  }

  private void HandlePlayerHealthChanged(int health)
  {
    Debug.WriteLine("Player health changed to " + health);
    _hud.SetPlayerHealth(health);
  }
}
