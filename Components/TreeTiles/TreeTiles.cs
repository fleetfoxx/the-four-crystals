using Godot;
using System;

public class TreeTiles : TileMap
{
  [Export]
  private PackedScene _treeScene;

  private const int TREE = 0;

  public override void _Ready()
  {
    // var ySort = this.GetExpectedNode<YSort>("SortedTrees");
    var parent = this.GetExpectedParent<Node>();

    foreach (Vector2 cell in GetUsedCellsById(TREE))
    {
      var tree = _treeScene.Instance<Node2D>();
      var position = MapToWorld(cell) + new Vector2(CellSize.x / 2, CellSize.y);
      tree.Position = position;
      parent.CallDeferred("add_child", tree);
    }
  }
}
