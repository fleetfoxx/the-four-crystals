using Godot;
using System;

public class WaterTile : Area2D
{
  private Sprite _debugSprite;

  public override void _Ready()
  {
    base._Ready();
    _debugSprite = this.GetExpectedNode<Sprite>("DebugSprite");
    _debugSprite.Visible = false;
  }
}
