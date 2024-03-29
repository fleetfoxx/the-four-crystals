using Godot;
using System;
using System.Diagnostics;

public class LavaArea : Area2D
{
  private Sprite _debugSprite;

  public override void _Ready()
  {
    base._Ready();
    _debugSprite = this.GetExpectedNode<Sprite>("DebugSprite");
    _debugSprite.Visible = false;
  }
}
