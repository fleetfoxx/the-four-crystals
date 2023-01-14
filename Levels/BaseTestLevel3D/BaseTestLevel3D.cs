using Godot;
using System;

public class BaseTestLevel3D : Node
{
  public override void _Ready()
  {
    var camera = this.GetExpectedNode<FollowCamera3D>("FollowCamera3D");
    var player = this.GetExpectedNode<Test3DPlayer>("Test3DPlayer");
    camera.Target = player;
  }
}
