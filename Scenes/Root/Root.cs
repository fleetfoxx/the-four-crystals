using Godot;
using System;
using System.Diagnostics;

public class Root : Node2D
{
  [Export]
  private PackedScene _playerScene;

  private Player.Player _player;
  private Node2D _playerSpawn;
  private FollowCamera _camera;
  private HUD _hud;

  public override void _Ready()
  {
    GD.Randomize();

    _playerSpawn = this.GetExpectedNode<Node2D>("PlayerSpawn");
    _hud = this.GetExpectedNode<HUD>("HUD");
    _camera = this.GetExpectedNode<FollowCamera>("FollowCamera");

    SpawnPlayer();
  }

  public override void _Process(float delta)
  {
    base._Process(delta);
    if (Input.IsActionJustPressed("quit"))
    {
      GetTree().Quit();
    }
  }

  private void SpawnPlayer()
  {
    _player = _playerScene.Instance<Player.Player>();
    _player.Position = _playerSpawn.Position;
    _player.Connect(nameof(Player.Player.HealthChangedSignal), this, nameof(HandlePlayerHealthChanged));
    _player.Connect(nameof(Player.Player.DeathSignal), this, nameof(HandlePlayerDeath));
    AddChild(_player);

    _camera.SetTarget(_player);
  }

  private void HandlePlayerHealthChanged(int health)
  {
    Debug.WriteLine($"Player health changed to {health}.");
    // _hud.SetPlayerHealth(health);
  }

  private void HandlePlayerDeath()
  {
    Debug.WriteLine("Player died.");
    _player.QueueFree();
    SpawnPlayer();
  }
}
