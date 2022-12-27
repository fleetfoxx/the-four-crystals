using System.Diagnostics;
using Godot;

public static class NodeExtensions
{
  public static T GetExpectedNode<T>(this Node node, NodePath path) where T : Node
  {
    var child = node.GetNodeOrNull<T>(path);
    Debug.Assert(child != null);
    return child;
  }
}