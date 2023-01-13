using Godot;
using System;

public class BaseTestLevel : Node
{
  public override void _Ready()
  {
    var camera = this.GetExpectedNode<FollowCamera>("FollowCamera");
    var player = this.GetExpectedNode<TestPlayer>("ObjectSort/TestPlayer");
    var spawnPoint = this.GetExpectedNode<Node2D>("PlayerSpawn");

    camera.SetTarget(player);
    player.GlobalPosition = spawnPoint.GlobalPosition;
  }
}
