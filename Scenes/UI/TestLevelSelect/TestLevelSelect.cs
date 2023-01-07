using Godot;
using Godot.Collections;

public class TestLevelSelect : CanvasLayer
{
  private Dictionary<string, string> _levels = new Dictionary<string, string> {
    {"Movement Test", Levels.MovementTest},
    {"Boss Battle Test", Levels.BossBattleTest}
  };

  public override void _Ready()
  {
    Button button;

    var buttonContainer = this.GetExpectedNode<VBoxContainer>("UI/ScrollContainer/ButtonContainer");

    foreach (var level in _levels)
    {
      button = new Button();
      var binds = new Godot.Collections.Array { level.Value };
      button.Text = level.Key;
      button.Connect("pressed", this, nameof(HandleButtonPressed), binds);
      buttonContainer.AddChild(button);
    }

    button = new Button();
    button.Text = "Quit";
    button.Connect("pressed", this, nameof(HandleQuitPressed));
    buttonContainer.AddChild(button);
  }

  private void HandleButtonPressed(string path)
  {
    LevelManager.LoadLevel(path);
  }

  private void HandleQuitPressed()
  {
    GetTree().Quit();
  }
}
