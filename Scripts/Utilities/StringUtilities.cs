using Godot;

public static class StringUtilities
{
  public static string Dump(this object obj)
  {
    return JSON.Print(obj, " ");
  }
}