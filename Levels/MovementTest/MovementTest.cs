using Godot;
using System;

public class MovementTest : Node
{
  public override void _Ready()
  {
    var player = this.GetExpectedNode<Player.Player>("Player");
    var camera = this.GetExpectedNode<FollowCamera>("FollowCamera");
    var ball = this.GetExpectedNode<MovementTestBall>("MovementTestBall");
    camera.SetTarget(ball);
  }
}
