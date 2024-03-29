using Godot;
using System;

public class TestBossBattle : Node
{
  public override void _Ready()
  {
    var camera = this.GetExpectedNode<FollowCamera>("FollowCamera");
    var player = this.GetExpectedNode<TestPlayer>("ObjectSort/TestPlayer");
    camera.SetTarget(player);
  }
}
