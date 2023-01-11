using Godot;
using System;

public class TreeTiles : TileMap
{
  [Export]
  private PackedScene _treeScene;

  [Export]
  private PackedScene _treeEnemyScene;

  private const int TREE = 0;

  public override void _Ready()
  {
    var parent = this.GetExpectedParent<Node>();

    foreach (Vector2 cell in GetUsedCellsById(TREE))
    {
      var random = GD.Randf();
      var shouldSpawnEnemy = random <= 0.5;
      var position = MapToWorld(cell) + new Vector2(CellSize.x / 2, CellSize.y);
      Node2D toSpawn;

      if (shouldSpawnEnemy)
      {
        toSpawn = _treeEnemyScene.Instance<Node2D>();
      }
      else
      {
        toSpawn = _treeScene.Instance<Node2D>();
      }

      toSpawn.Position = position;
      parent.CallDeferred("add_child", toSpawn);
    }
  }
}
