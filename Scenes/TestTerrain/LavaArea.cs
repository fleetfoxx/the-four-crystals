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

    Connect("body_entered", this, nameof(HandleBodyEntered));
  }

  private void HandleBodyEntered(Node node)
  {
    // NOTE: This fires when the player walks over this tile.
    // Debug.WriteLine($"{node.Name} entered lava.");
  }
}
