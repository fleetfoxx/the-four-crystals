using Godot;

[Tool]
public class HealthBar : Control
{
  [Export]
  public float MaxHealth { get; set; } = 100;

  [Export]
  public float Health { get; set; } = 100;

  [Export]
  public Color ForegroundColor { get; set; } = Colors.Green;

  [Export]
  public Color BackgroundColor { get; set; } = Colors.White;

  private ColorRect _backgroundRect;
  private ColorRect _healthRect;

  public override void _Ready()
  {
    base._Ready();
    _backgroundRect = this.GetExpectedNode<ColorRect>("Background");
    _healthRect = this.GetExpectedNode<ColorRect>("Health");

    if (MaxHealth < 1)
    {
      MaxHealth = 1;
    }
  }

  public override void _Process(float delta)
  {
    base._Process(delta);
    var maxWidth = _backgroundRect.RectSize.x;
    var healthPercentage = Health / MaxHealth;
    var healthWidth = maxWidth * healthPercentage;
    _healthRect.RectSize = new Vector2(healthWidth, _healthRect.RectSize.y);

    _backgroundRect.Color = BackgroundColor;
    _healthRect.Color = ForegroundColor;
  }

  public void UpdateInfo(float health, float maxHealth)
  {
    Health = health;
    MaxHealth = maxHealth;
  }
}
