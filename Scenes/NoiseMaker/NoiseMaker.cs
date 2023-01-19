using Godot;
using System;

public class NoiseMaker : Area, INoisy
{
  public bool IsMakingNoise { get; private set; } = false;

  [Export]
  public float Volume { get; private set; } = 1;

  [Export]
  public float BeepFrequency { get; set; } = 10;

  [Export]
  public float RandomOffset { get; set; } = 5;

  private AudioStreamPlayer3D _audioPlayer;
  private Timer _beepTimer;
  private AnimationPlayer _animationPlayer;

  public override void _Ready()
  {
    _beepTimer = this.GetExpectedNode<Timer>("BeepTimer");
    _beepTimer.Connect("timeout", this, nameof(HandleBeep));
    StartTimer();

    _audioPlayer = this.GetExpectedNode<AudioStreamPlayer3D>("AudioPlayer");
    _audioPlayer.Connect("finished", this, nameof(HandleFinishBeep));

    _animationPlayer = this.GetExpectedNode<AnimationPlayer>("AnimationPlayer");

    this.GetExpectedNode<AnimationPlayer>("SpinAnimationPlayer").Play("Spin");
  }

  private void StartTimer()
  {
    var frequencyWithOffset = BeepFrequency + (float)GD.RandRange(-RandomOffset, RandomOffset);
    _beepTimer.Start(frequencyWithOffset);
  }

  private void HandleBeep()
  {
    IsMakingNoise = true;
    _audioPlayer.Play();
    _animationPlayer.Play("Beep");
  }

  private void HandleFinishBeep()
  {
    IsMakingNoise = false;
    StartTimer();
  }
}
