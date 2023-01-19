using Godot;
using System;
using System.Diagnostics;

public class GreenOrb3D : Area, IInteractable
{
  private AnimationPlayer _animationPlayer;

  public override void _Ready()
  {
    _animationPlayer = this.GetExpectedNode<AnimationPlayer>("AnimationPlayer");

    Connect("body_entered", this, nameof(HandleBodyEntered));
    Connect("body_exited", this, nameof(HandleBodyExited));
  }

  public bool Interact(params object[] args)
  {
    LevelManager.LoadLevel(Levels.TestLevelSelect);
    return true;
  }

  private void HandleBodyEntered(Node body)
  {
    Debug.WriteLine($"{body.Name} detected by orb.");

    if (body is Test3DPlayer)
    {
      _animationPlayer.Play("DetectPlayer");
    }
  }

  private void HandleBodyExited(Node body)
  {
    Debug.WriteLine($"{body.Name} un-detected by orb.");

    if (body is Test3DPlayer)
    {
      _animationPlayer.PlayBackwards("DetectPlayer");
    }
  }
}
