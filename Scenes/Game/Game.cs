using Godot;
using System;

public class Game : Node
{
  public override void _Ready()
  {
    base._Ready();
    LevelManager.Initialize(this);
    LevelManager.LoadLevel(Levels.TestLevelSelect);
  }

  public override void _Process(float delta)
  {
    base._Process(delta);

    if (Input.IsActionJustPressed("quit"))
    {
      LevelManager.LoadLevel(Levels.TestLevelSelect);
    }
  }
}
