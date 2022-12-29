using Godot;
using System;
using System.Diagnostics;

public class TestTerrain : TileMap
{
  [Export]
  private PackedScene _lavaAreaScene;

  private const int LAVA_INDEX = 0;

  public override void _Ready()
  {
    base._Ready();
    var lavaCells = GetUsedCellsById(LAVA_INDEX);

    foreach (Vector2 cellPosition in lavaCells)
    {
      var area = _lavaAreaScene.Instance<LavaArea>();
      area.Position = MapToWorld(cellPosition);
      AddChild(area);
    }
  }
}
