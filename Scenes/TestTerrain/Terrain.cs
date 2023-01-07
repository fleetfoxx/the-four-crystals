using Godot;

public class Terrain : Node2D
{
  [Export]
  private PackedScene _lavaAreaScene;

  [Export]
  private PackedScene _waterTileScene;

  private const int LAVA_INDEX = 0;
  private const int WATER_INDEX = 1;

  private TileMap _walkable;
  private TileMap _deepWater;

  public override void _Ready()
  {
    base._Ready();

    _walkable = this.GetExpectedNode<TileMap>("Walkable");
    _deepWater = this.GetExpectedNode<TileMap>("DeepWater");

    InstantiateTiles<LavaArea>(_walkable, LAVA_INDEX, _lavaAreaScene);
    InstantiateTiles<WaterTile>(_walkable, WATER_INDEX, _waterTileScene);
  }

  private void InstantiateTiles<T>(TileMap tileMap, int tileIndex, PackedScene scene) where T : Node2D
  {
    var cells = tileMap.GetUsedCellsById(tileIndex);

    foreach (Vector2 cellPosition in cells)
    {
      var area = scene.Instance<T>();
      area.Position = tileMap.MapToWorld(cellPosition);
      AddChild(area);
    }
  }
}
