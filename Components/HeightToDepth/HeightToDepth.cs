using Godot;
using System;
using System.Diagnostics;

[Tool]
public class HeightToDepth : Node2D
{
  private Node2D _parent = null;

  public override void _Ready()
  {
    SetParent();
    _parent.ZAsRelative = false;
  }

  public override void _Process(float delta)
  {
    if (_parent == null)
    {
      SetParent();
    }

    _parent.ZIndex = (int)Math.Floor(GlobalPosition.y);
  }

  private void SetParent()
  {
    _parent = GetParentOrNull<Node2D>();
    Debug.Assert(_parent != null);
  }
}
