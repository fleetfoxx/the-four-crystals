using Godot;

public class FollowCamera : Camera2D
{
  [Export]
  private float _stiffness = 8f;

  private Node2D _target = null;

  public override void _PhysicsProcess(float delta)
  {
    if (_target == null) return;
    GlobalPosition = GlobalPosition.LinearInterpolate(_target.GlobalPosition, _stiffness * delta);
  }

  public void SetTarget(Node2D target)
  {
    _target = target;
  }
}
