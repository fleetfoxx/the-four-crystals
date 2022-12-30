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

  public static T GetExpectedOwner<T>(this Node node) where T : Node
  {
    var owner = node.GetOwnerOrNull<T>();
    Debug.Assert(owner != null);
    return owner;
  }

  public static T GetExpectedParent<T>(this Node node) where T : Node
  {
    var parent = node.GetParentOrNull<T>();
    Debug.Assert(parent != null);
    return parent;
  }
}