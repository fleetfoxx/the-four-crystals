using Godot;
using System;

[Tool]
public class Campfire : Area2D, IInteractable, IDestructible
{
  public bool IsLit { get; private set; }

  public override void _Ready()
  {
    Connect("area_entered", this, nameof(HandleAreaEntered));
  }

  public override void _Process(float delta)
  {
    Update();
  }

  public override void _Draw()
  {
    var collider = this.GetExpectedNode<CollisionShape2D>("Collider");
    var color = Color.Color8(255, 127, 0, 127);

    if (IsLit)
    {
      color = Color.Color8(255, 0, 0, 127);
    }

    DrawCircle(Vector2.Zero, ((CircleShape2D)collider.Shape).Radius, color);
  }

  public bool Interact(params object[] args)
  {
    var numberOfSticks = GetNumberOfSticks();

    if (numberOfSticks < 3)
    {
      if (args.Length < 1)
      {
        // You must drop a stick.
        return false;
      }

      if (!(args[0] is Stick))
      {
        // You must drop a stick.
        return false;
      }

      var stick = (Stick)args[0];

      var stickParent = stick.GetParent();
      stickParent?.RemoveChild(stick);
      stick.Position = Vector2.Zero;

      if (numberOfSticks == 0)
      {
        stick.RotationDegrees = 0;
      }
      else if (numberOfSticks == 1)
      {
        stick.RotationDegrees = 60;
      }
      else if (numberOfSticks == 2)
      {
        stick.RotationDegrees = -60;
      }

      stick.DisableInteraction();

      AddChild(stick);
    }
    else
    {
      IsLit = true;
    }

    return true;
  }

  public int GetNumberOfSticks()
  {
    var count = 0;

    foreach (Node child in GetChildren())
    {
      if (child is Stick)
      {
        count++;
      }
    }

    return count;
  }

  private void HandleAreaEntered(Area2D area)
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

  /// <summary>
  /// Remove all the sticks in this campfire. If it's lit, put it out.
  /// </summary>
  public void Destroy(Node source)
  {
    foreach (Node child in GetChildren())
    {
      if (child is Stick)
      {
        child.QueueFree();
      }
    }

    // HACK: Light the boomerang on fire before IsLit is flipped. We can't be
    // sure HandleAreaEntered above will fire first.
    if (IsLit && source is Boomerang)
    {
      ((Boomerang)source).IsOnFire = true;
    }

    IsLit = false;
  }
}
