using Godot;
using System;
using System.Collections.Generic;

public class PlayerInventory : Node
{
  public static List<IInventoryItem> Inventory { get; private set; } = new List<IInventoryItem>();

  public static void Add(IInventoryItem item)
  {
    Inventory.Add(item);
  }
}
