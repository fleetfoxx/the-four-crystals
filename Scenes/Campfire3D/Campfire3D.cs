using Godot;
using System;

public class Campfire3D : StaticBody, IInteractable, IDestructible
{
  public bool IsLit { get; private set; }

  private Area _burnArea;
  private MeshInstance _flames;

  public override void _Ready()
  {
    _burnArea = this.GetExpectedNode<Area>("BurnArea");
    _burnArea.Connect("area_entered", this, nameof(HandleBurnAreaEntered));

    _flames = this.GetExpectedNode<MeshInstance>("Flames");
  }

  public override void _Process(float delta)
  {
    if (IsLit)
    {
      _flames.Show();
    }
    else
    {
      _flames.Hide();
    }
  }

  public bool Interact(params object[] args)
  {
    var numberOfLogs = GetNumberOfLogs();

    if (numberOfLogs < 3)
    {
      if (args.Length < 1)
      {
        // You must drop a log.
        return false;
      }

      if (!(args[0] is Log3D))
      {
        // You must drop a log.
        return false;
      }

      var log = (Log3D)args[0];

      var stickParent = log.GetParent();
      stickParent?.RemoveChild(log);
      log.Translation = new Vector3(0, 0.5f, 0);

      if (numberOfLogs == 0)
      {
        log.RotateY(0);
      }
      else if (numberOfLogs == 1)
      {
        log.RotateY(60);
      }
      else if (numberOfLogs == 2)
      {
        log.RotateY(-60);
      }

      log.DisableInteraction();

      AddChild(log);
    }
    else
    {
      IsLit = true;
    }

    return true;
  }

  public int GetNumberOfLogs()
  {
    var count = 0;

    foreach (Node child in GetChildren())
    {
      if (child is Log3D)
      {
        count++;
      }
    }

    return count;
  }

  private void HandleBurnAreaEntered(Area area)
  {
    if (IsLit)
    {
      if (area is IFlammable)
      {
        ((IFlammable)area).IsOnFire = true;
      }

      if (area.Owner is IFlammable)
      {
        ((IFlammable)area.Owner).IsOnFire = true;
      }
    }
  }

  public void Destroy(Node source)
  {
    if (IsLit)
    {
      // HACK: Light the boomerang on fire before IsLit is flipped. We can't be
      // sure HandleAreaEntered above will fire first.
      if (source is Boomerang3D)
      {
        ((Boomerang3D)source).IsOnFire = true;
      }

      QueueFree();
    }
    else
    {
      // Remove all sticks.
      foreach (Node child in GetChildren())
      {
        if (child is Log3D)
        {
          child.QueueFree();
        }
      }
    }
  }
}
