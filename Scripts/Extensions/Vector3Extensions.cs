using Godot;

public static class Vector3Extensions
{
  public static bool IsWithinRadiusOf(this Vector3 pointA, Vector3 pointB, float radius)
  {
    return pointA.DistanceTo(pointB) <= radius;
  }
}