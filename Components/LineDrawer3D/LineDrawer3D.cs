using Godot;
using System;
using System.Collections.Generic;

public class LineDrawer3D : ImmediateGeometry
{
  private List<Vector3> _points = new List<Vector3>();

  public void ClearPoints()
  {
    _points = new List<Vector3>();
  }

  public void AddPoint(Vector3 point)
  {
    _points.Add(point);
  }

  public override void _Process(float delta)
  {
    Clear();
    Begin(Mesh.PrimitiveType.Lines);

    foreach (var point in _points)
    {
      AddVertex(point);
    }

    End();
  }
}
