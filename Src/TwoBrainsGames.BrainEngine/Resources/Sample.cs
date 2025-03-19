
// Type: TwoBrainsGames.BrainEngine.Resources.Sample
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using System;
using TwoBrainsGames.BrainEngine.Audio;
using TwoBrainsGames.BrainEngine.Graphics;


namespace TwoBrainsGames.BrainEngine.Resources
{
  public class Sample : Resource
  {
    private SoundEffect _sample;
    private SoundEffectInstance _sampleInstance;
    private bool _isFading;
    public float _sourceVolume;
    public float _targetVolume;
    private TimeSpan _elapsedTime;
    private TimeSpan _fadeDuration;
    private bool _canApply3D;
    private Object2D _objEmitter;
    private AudioEmitter _emitter;

    public event Sample.FadeOutSampleHandler OnFadeOut;

    public double PlaySameEffectMinTime { get; set; }

    public SoundEffect Sound
    {
      get => this._sample;
      set => this._sample = value;
    }

    public AudioEmitter Emitter
    {
      get
      {
        if (!this._canApply3D)
          return (AudioEmitter) null;
        this._emitter.Position = SampleManager.Sample3DPosition(this._objEmitter.Position);
        return this._emitter;
      }
    }

    public Object2D ObjEmitter => this._objEmitter;

    public bool IsFading => this._isFading;

    public bool IsDisposed => this._sampleInstance != null && this._sampleInstance.IsDisposed;

    public bool IsPlaying
    {
      get
      {
        return this._sampleInstance != null && !this._sampleInstance.IsDisposed && this._sampleInstance.State == SoundState.Playing;
      }
    }

    public bool IsStopped
    {
      get
      {
        return this._sampleInstance != null && !this._sampleInstance.IsDisposed && this._sampleInstance.State == SoundState.Stopped;
      }
    }

    public bool IsPaused
    {
      get
      {
        return this._sampleInstance != null && !this._sampleInstance.IsDisposed && this._sampleInstance.State == SoundState.Paused;
      }
    }

    public bool CanApply3D
    {
      get => this._canApply3D;
      set => this._canApply3D = value;
    }

    public Sample()
      : this((SoundEffect) null, (Object2D) null)
    {
    }

    public Sample(SoundEffect sound)
      : this(sound, (Object2D) null)
    {
    }

    public Sample(SoundEffect sound, Object2D objEmitter)
    {
      this._sample = sound;
      this._objEmitter = objEmitter;
      if (this._objEmitter != null)
      {
        this._canApply3D = true;
        this._emitter = new AudioEmitter();
        this._emitter.Position = new Vector3(this._objEmitter.Position.X, this._objEmitter.Position.Y, 0.0f);
      }
      this.PlaySameEffectMinTime = 20.0;
    }

    public override bool Load(ContentManager contentManager)
    {
      this.IsLoaded = false;
      this._sample = contentManager.Load<SoundEffect>(this.Path);
      if (this._sample != null)
        this.IsLoaded = true;
      return this.IsLoaded;
    }

    public override bool Release(ContentManager contentManager)
    {
      if (this._sample != null)
        this._sample.Dispose();
      this.IsLoaded = false;
      return true;
    }

    public void Play()
    {
      if (this.IsPlaying)
        return;
      this._isFading = false;
      this.Play(1f, 0.0f, 0.0f, false);
    }

    public void Play(bool loop)
    {
      if (this.IsPlaying)
        return;
      this.Play(1f, 0.0f, 0.0f, loop);
    }

    public void Play(float volume)
    {
      if (this.IsPlaying)
        return;
      this.Play(volume, 0.0f, 0.0f, false);
    }

    public void Play(float volume, bool loop)
    {
      if (this.IsPlaying)
        return;
      this.Play(volume, 0.0f, 0.0f, loop);
    }

    private void Play(float volume, float pitch, float pan, bool loop)
    {
      this._sampleInstance = BrainGame.SampleManager.Play(this, volume, pitch, pan, loop);
    }

    public void Pause()
    {
      if (this._sampleInstance == null || this._sampleInstance.IsDisposed)
        return;
      this._sampleInstance.Pause();
    }

    public void Resume()
    {
      if (this._sampleInstance == null || this._sampleInstance.IsDisposed)
        return;
      this._sampleInstance.Resume();
    }

    public void Stop()
    {
      if (this._sampleInstance == null || this._sampleInstance.IsDisposed)
        return;
      this._sampleInstance.Stop();
    }

    public Sample.SampleState State()
    {
      return this._sampleInstance != null ? (Sample.SampleState) this._sampleInstance.State : Sample.SampleState.Invalid;
    }

    public float GetFadeVolume()
    {
      return MathHelper.Lerp(this._sourceVolume, this._targetVolume, (float) this._elapsedTime.Ticks / (float) this._fadeDuration.Ticks);
    }

    public void Fade(float sourceVolume, float targetVolume, TimeSpan fadeDuration)
    {
      this._sourceVolume = sourceVolume;
      this._targetVolume = targetVolume;
      this._fadeDuration = fadeDuration;
      this._elapsedTime = TimeSpan.Zero;
      this._isFading = true;
    }

    public void FadeIn(TimeSpan fadeDuration)
    {
      if (this._isFading)
        return;
      this.Fade(0.0f, 1f, fadeDuration);
    }

    public void FadeOut(TimeSpan fadeDuration)
    {
      if (this._isFading)
        return;
      this.Fade(1f, 0.0f, fadeDuration);
    }

    public void FadeCancel() => this._isFading = false;

    public void FadeUpdate(BrainGameTime gameTime)
    {
      if (!this._isFading || this._sampleInstance == null || this._sampleInstance.IsDisposed || this._sampleInstance.State != SoundState.Playing)
        return;
      this._elapsedTime += gameTime.ElapsedGameTime;
      if (this._elapsedTime >= this._fadeDuration)
      {
        this._elapsedTime = this._fadeDuration;
        this._isFading = false;
      }
      this._sampleInstance.Volume = this.GetFadeVolume();
      if (this._isFading || (double) this._sampleInstance.Volume != 0.0)
        return;
      this.Stop();
      if (this.OnFadeOut == null)
        return;
      this.OnFadeOut();
    }

    public enum SampleState
    {
      Invalid = -1, // 0xFFFFFFFF
      Playing = 0,
      Paused = 1,
      Stopped = 2,
    }

    public delegate void FadeOutSampleHandler();
  }
}
