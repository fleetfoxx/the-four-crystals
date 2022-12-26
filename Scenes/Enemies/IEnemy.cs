using Godot;

namespace Enemies
{
  public interface IEnemy
  {
    Vector2 Velocity { get; set; }
    Vector2 GlobalPosition { get; set; }
  }
}