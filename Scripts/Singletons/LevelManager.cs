using System;
using System.Diagnostics;
using Godot;

public static class Levels
{
  public const string TestLevelSelect = "res://Scenes/UI/TestLevelSelect/TestLevelSelect.tscn";
  public const string BossBattleTest = "res://Levels/TestBossBattle/TestBossBattle.tscn";
  public const string MovementTest = "res://Levels/MovementTest/MovementTest.tscn";
  public const string TestStealth = "res://Levels/TestStealth/TestStealth.tscn";
  public const string Playground = "res://Levels/Playground/Playground.tscn";
}

public class LevelManager : Node
{
  private static Node _gameNode = null;
  private static Node _currentLevel = null;

  public static void Initialize(Node gameNode)
  {
    Debug.Assert(gameNode != null);
    _gameNode = gameNode;
  }

  public static void LoadLevel(string levelPath)
  {
    if (_gameNode == null)
    {
      throw new Exception("You must initialize the LevelManager before using it.");
    }

    Debug.WriteLine("Loading level " + levelPath);

    if (_currentLevel != null)
    {
      _currentLevel.QueueFree();
    }

    var nextLevel = ResourceLoader.Load<PackedScene>(levelPath);

    if (nextLevel is PackedScene)
    {
      _currentLevel = nextLevel.Instance();
      _gameNode.AddChild(_currentLevel);
    }
    else
    {
      throw new Exception($"Failed to load level. '{levelPath}' isn't a PackedScene.");
    }
  }
}