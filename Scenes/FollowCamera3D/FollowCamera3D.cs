using Godot;
using System;
using System.Diagnostics;

[Tool]
public class FollowCamera3D : Camera
{
  [Export]
  public Vector3 TranslationOffset { get; set; } = Vector3.Zero;

  public Spatial Target { get; set; } = null;

  public override void _Process(float delta)
  {
    var targetTranslation = Target?.GlobalTranslation ?? Vector3.Forward;
    GlobalTranslation = targetTranslation + TranslationOffset;

    LookAt(targetTranslation, Vector3.Up);
  }
}
