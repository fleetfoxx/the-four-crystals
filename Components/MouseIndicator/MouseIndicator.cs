using Godot;
using System;
using System.Diagnostics;

public class MouseIndicator : MeshInstance
{
  private Camera _camera;
  private LineDrawer3D _line;
  private ColorRect _screenCursor;

  public override void _Ready()
  {
    _camera = GetViewport().GetCamera();
    _line = this.GetExpectedNode<LineDrawer3D>("Line");
    _screenCursor = this.GetExpectedNode<ColorRect>("CanvasLayer/ScreenCursor");
  }

  public override void _Input(InputEvent @event)
  {
    if (@event is InputEventMouseButton)
    {
      var mousePosition = ((InputEventMouseButton)@event).Position;
    }
  }

  public override void _Process(float delta)
  {
    var mousePosition = GetViewport().GetMousePosition();
    var from = _camera.ProjectRayOrigin(mousePosition);
    var to = _camera.ProjectRayNormal(mousePosition) * 1000;
    var cursorPosition = new Plane(Vector3.Up, 0).IntersectRay(from, to);

    _screenCursor.RectPosition = mousePosition;

    _line.ClearPoints();
    _line.AddPoint(from);
    _line.AddPoint(to);

    if (cursorPosition.HasValue)
    {
      GlobalTranslation = cursorPosition.Value;
    }
  }
}
