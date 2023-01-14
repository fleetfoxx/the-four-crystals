using Godot;

public class PlayerManager : Node
{
  public static int MaxHealth { get; set; } = 5;
  public static int Health { get; set; } = 5;
  public static float MaxStamina { get; set; } = 20;
  public static float Stamina { get; set; } = 20;
}