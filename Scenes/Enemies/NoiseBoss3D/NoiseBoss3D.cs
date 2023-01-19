using Godot;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

public class NoiseBoss3D : KinematicBody
{
  [Export]
  private float _speed = 1;
  private Area _noiseDetectionArea;
  private Vector3? _target = null;
  private Vector3 _velocity = Vector3.Zero;

  public override void _Ready()
  {
    _noiseDetectionArea = this.GetExpectedNode<Area>("NoiseDetectionArea");
  }

  public override void _Process(float delta)
  {
    Vector3? closestTarget = null;

    if (_target.HasValue)
    {
      _velocity = GlobalTranslation.DirectionTo(_target.Value) * _speed;
    }
    else
    {
      // TODO: also check overlapping bodies
      foreach (Area area in _noiseDetectionArea.GetOverlappingAreas())
      {
        if (area is INoisy && ((INoisy)area).IsMakingNoise)
        {
          if (closestTarget == null)
          {
            closestTarget = area.GlobalTranslation;
          }
          else if (area.GlobalTranslation.DistanceTo(GlobalTranslation) < closestTarget.Value.DistanceTo(GlobalTranslation))
          {
            closestTarget = area.GlobalTranslation;
          }
        }
        else if (area.Owner is INoisy && ((INoisy)area.Owner).IsMakingNoise)
        {
          if (closestTarget == null)
          {
            closestTarget = ((Spatial)area.Owner).GlobalTranslation;
          }
          else if (((Spatial)area.Owner).GlobalTranslation.DistanceTo(GlobalTranslation) < closestTarget.Value.DistanceTo(GlobalTranslation))
          {
            closestTarget = ((Spatial)area.Owner).GlobalTranslation;
          }
        }
      }

      if (closestTarget != null)
      {
        _target = new Vector3(closestTarget.Value.x, 0, closestTarget.Value.z);
        GetTree().CreateTimer(5).Connect("timeout", this, nameof(EndPursuit));
      }
      else
      {
        _velocity = Vector3.Zero;
      }
    }

    // Debug.WriteLine($"Me: {GlobalTranslation} Target: {(_target.HasValue ? _target.Value.ToString() : "NULL")}");

    if (_target.HasValue && GlobalTranslation.IsWithinRadiusOf(_target.Value, 0.1f))
    {
      _target = null;
    }
  }

  public override void _PhysicsProcess(float delta)
  {
    _velocity = MoveAndSlide(_velocity);
  }

  private void EndPursuit()
  {
    _target = null;
  }
}
