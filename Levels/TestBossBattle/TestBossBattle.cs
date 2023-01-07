using Godot;
using System;

public class TestBossBattle : Node
{
  public override void _Ready()
  {
    var camera = this.GetExpectedNode<FollowCamera>("FollowCamera");
    var ball = this.GetExpectedNode<MovementTestBall>("ObjectSort/MovementTestBall");
    camera.SetTarget(ball);
  }
}
