using Godot;

public class TestTerrain : TileMap
{
  [Export]
  private PackedScene _lavaAreaScene;

  [Export]
  private PackedScene _waterTileScene;

  private const int LAVA_INDEX = 0;
  private const int WATER_INDEX = 1;

  public override void _Ready()
  {
    base._Ready();

    InstantiateTiles<LavaArea>(LAVA_INDEX, _lavaAreaScene);
    InstantiateTiles<WaterTile>(WATER_INDEX, _waterTileScene);
  }

  private void InstantiateTiles<T>(int tileIndex, PackedScene scene) where T : Node2D
  {
    var cells = GetUsedCellsById(tileIndex);

    foreach (Vector2 cellPosition in cells)
    {
      var area = scene.Instance<T>();
      area.Position = MapToWorld(cellPosition);
      AddChild(area);
    }
  }
}
