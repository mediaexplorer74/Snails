
// Type: TwoBrainsGames.BrainEngine.Audio.SampleManager
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Resources;


namespace TwoBrainsGames.BrainEngine.Audio
{
  public class SampleManager : GameComponent
  {
    private const float OFF_AUDIBLE_ZONE_DECAY = 0.5f;
    private const int MAX_SAMPLES = 32;
    public const float FRACTION_3D = 250f;
    public const int MAX_SOUND_EFFECTS_AT_SAME_TIME = 2;
    private SampleManager.PlayingSample[] _playingSamples = new SampleManager.PlayingSample[32];
    private AudioListener _audioListener;
    public bool EnabledSamples = true;

    public float MasterVolume
    {
      get => SoundEffect.MasterVolume;
      set => SoundEffect.MasterVolume = value;
    }

    private List<SampleManager.PlayingSoundEffectData> CurrentlyPlayingSoundEffects { get; set; }

    public BoundingSquare AudibleBoundingSquare { get; set; }

    public bool UseAudibleBoundingSquare { get; set; }

    public SampleManager(BrainGame game)
      : base((Game) game)
    {
      this._audioListener = new AudioListener();
      this.CurrentlyPlayingSoundEffects = new List<SampleManager.PlayingSoundEffectData>();
    }

    public void SetAudioListenerPosition(Vector2 camPos)
    {
      this._audioListener.Position = SampleManager.Sample3DPosition(camPos);
    }

    public static Vector3 Sample3DPosition(Vector2 pos)
    {
      return new Vector3(pos.X / 250f, 0.0f, pos.Y / 250f);
    }

    private int GetAvailableSlot()
    {
      for (int availableSlot = 0; availableSlot < this._playingSamples.Length; ++availableSlot)
      {
        if (this._playingSamples[availableSlot] == null)
          return availableSlot;
      }
      return -1;
    }

    public SoundEffectInstance Play(Sample sample) => this.Play(sample, 1f, 0.0f, 0.0f, false);

    public SoundEffectInstance Play(Sample sample, bool loop)
    {
      return this.Play(sample, 1f, 0.0f, 0.0f, loop);
    }

    public SoundEffectInstance Play(Sample sample, float volume)
    {
      return this.Play(sample, volume, 0.0f, 0.0f, false);
    }

    private float XnaPitchToAlPitch(float pitch)
    {
      float alPitch = 1f;
      if ((double) pitch < 0.0)
        alPitch = (float) (1.0 + 0.5 * (double) pitch);
      else if ((double) pitch > 0.0)
        alPitch = 1f + pitch;
      return alPitch;
    }

    public SoundEffectInstance Play(
      Sample sample,
      float volume,
      float pitch,
      float pan,
      bool loop)
    {
      SampleManager.PlayingSoundEffectData playingSoundEffectData = sample != null ? this.GetPlayingSoundEffect(sample.Sound) : throw new BrainException("Invalid sample to play");
      if (playingSoundEffectData == null)
      {
        playingSoundEffectData = new SampleManager.PlayingSoundEffectData(sample.Sound);
        this.CurrentlyPlayingSoundEffects.Add(playingSoundEffectData);
      }
      else if (DateTime.Now.Subtract(playingSoundEffectData._startTime).TotalMilliseconds < sample.PlaySameEffectMinTime)
        return (SoundEffectInstance) null;
      playingSoundEffectData._startTime = DateTime.Now;
      int availableSlot = this.GetAvailableSlot();
      if (availableSlot == -1)
        return (SoundEffectInstance) null;
      this._playingSamples[availableSlot] = new SampleManager.PlayingSample();
      this._playingSamples[availableSlot].CanApply3D = sample.CanApply3D;
      this._playingSamples[availableSlot].Emitter = sample.Emitter;
      this._playingSamples[availableSlot].SoundInstance = sample.Sound.CreateInstance();
      this._playingSamples[availableSlot].SoundInstance.Volume = volume;
      this._playingSamples[availableSlot]._sample = sample;
      if (BrainGame.GameTime.Multiplier > 1)
        pitch *= 0.7f;
      this._playingSamples[availableSlot].SoundInstance.Pitch = pitch;
      this._playingSamples[availableSlot].SoundInstance.Pan = pan;
      this._playingSamples[availableSlot].SoundInstance.IsLooped = loop;
      if (this._playingSamples[availableSlot].CanApply3D)
        this._playingSamples[availableSlot].SoundInstance.Apply3D(this._audioListener, this._playingSamples[availableSlot].Emitter);
      this._playingSamples[availableSlot].SoundInstance.Play();
      if (!this.EnabledSamples)
        this._playingSamples[availableSlot].SoundInstance.Pause();
      this._playingSamples[availableSlot]._insideOfAudibleBB = true;
      return this._playingSamples[availableSlot].SoundInstance;
    }

    public void ChangePlayingPitch(float pitch)
    {
      for (int index = 0; index < this._playingSamples.Length; ++index)
      {
        if (this._playingSamples[index] != null && this._playingSamples[index].SoundInstance.State == SoundState.Playing)
          this._playingSamples[index].SoundInstance.Pitch = pitch;
      }
    }

    public void StopAll()
    {
      for (int index = 0; index < this._playingSamples.Length; ++index)
      {
        if (this._playingSamples[index] != null)
        {
          this._playingSamples[index].SoundInstance.Stop();
          this._playingSamples[index].SoundInstance.Dispose();
          this._playingSamples[index] = (SampleManager.PlayingSample) null;
        }
      }
      this.CurrentlyPlayingSoundEffects.Clear();
    }

    public void ResumeAll() => this.PauseResumeSounds(true);

    public void PauseAll() => this.PauseResumeSounds(false);

    private void PauseResumeSounds(bool resume)
    {
      if (!this.EnabledSamples)
        return;
      if (resume)
      {
        for (int index = 0; index < this._playingSamples.Length; ++index)
        {
          if (this._playingSamples[index] != null && this._playingSamples[index].SoundInstance.State == SoundState.Paused)
            this._playingSamples[index].SoundInstance.Resume();
        }
      }
      else
      {
        for (int index = 0; index < this._playingSamples.Length; ++index)
        {
          if (this._playingSamples[index] != null && this._playingSamples[index].SoundInstance.State == SoundState.Playing)
            this._playingSamples[index].SoundInstance.Pause();
        }
      }
    }

    public void DisableSamples()
    {
      this.PauseAll();
      this.EnabledSamples = false;
    }

    public void EnableSamples()
    {
      this.EnabledSamples = true;
      this.ResumeAll();
    }

    public override void Update(GameTime gameTime)
    {
      if (!this.EnabledSamples)
        return;
      for (int index = 0; index < this._playingSamples.Length; ++index)
      {
        if (this._playingSamples[index] != null && this._playingSamples[index].SoundInstance != null)
        {
          switch (this._playingSamples[index].SoundInstance.State)
          {
            case SoundState.Playing:
            case SoundState.Paused:
              if (this._playingSamples[index].CanApply3D)
              {
                this._playingSamples[index].SoundInstance.Apply3D(this._audioListener, this._playingSamples[index].Emitter);
                continue;
              }
              continue;
            case SoundState.Stopped:
              this._playingSamples[index].SoundInstance.Dispose();
              this._playingSamples[index].SoundInstance = (SoundEffectInstance) null;
              this._playingSamples[index] = (SampleManager.PlayingSample) null;
              continue;
            default:
              continue;
          }
        }
      }
    }

    private SampleManager.PlayingSoundEffectData GetPlayingSoundEffect(SoundEffect effect)
    {
      foreach (SampleManager.PlayingSoundEffectData playingSoundEffect in this.CurrentlyPlayingSoundEffects)
      {
        if (playingSoundEffect._soundEffect == effect)
          return playingSoundEffect;
      }
      return (SampleManager.PlayingSoundEffectData) null;
    }

    private class PlayingSample
    {
      public bool CanApply3D;
      public SoundEffectInstance SoundInstance;
      public AudioEmitter Emitter;
      public Sample _sample;
      public bool _insideOfAudibleBB;

      public bool CheckInsideAudibleBB()
      {
        return this._sample.ObjEmitter == null || BrainGame.SampleManager.AudibleBoundingSquare.Collides(this._sample.ObjEmitter.SoundEmmiterBoundingBox);
      }
    }

    private class PlayingSoundEffectData
    {
      public SoundEffect _soundEffect;
      public DateTime _startTime;

      public PlayingSoundEffectData(SoundEffect soundEffect) => this._soundEffect = soundEffect;
    }
  }
}
