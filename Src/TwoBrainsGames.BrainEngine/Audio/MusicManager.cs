
// Type: TwoBrainsGames.BrainEngine.Audio.MusicManager
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using System;


namespace TwoBrainsGames.BrainEngine.Audio
{
  public class MusicManager : GameComponent
  {
    private const float DEFAULT_VOLUME = 0.5f;
    private bool _isMusicPaused;
    private bool _isFading;
    private Song _currentMusic;
    private float _saveMasterVolume;
    private MusicManager.MusicState _state;
    public float _sourceVolume;
    public float _targetVolume;
    private TimeSpan _elapsedTime;
    private TimeSpan _fadeDuration;
    public bool EnabledMusic = true;

    public event MusicManager.FadeOutMusicHandler OnFadeOut;

    public Song CurrentSong => this._currentMusic;

    public float MasterVolume
    {
      get => this._saveMasterVolume;
      set
      {
        if (MediaPlayer.GameHasControl)
          MediaPlayer.Volume = value;
        this._saveMasterVolume = value;
      }
    }

    public bool IsMusicActive
    {
      get => this._currentMusic != (Song) null && this._state != MusicManager.MusicState.Stopped;
    }

    public bool IsMusicPaused => this._currentMusic != (Song) null && this._isMusicPaused;

    public MusicManager(BrainGame game)
      : base((Game) game)
    {
      this.MasterVolume = 0.5f;
    }

    public void PlayMusic(Song song) => this.PlayMusic(song, false);

    public void PlayMusic(Song song, bool loop)
    {
      if (!MediaPlayer.GameHasControl || !(song != this._currentMusic))
        return;
      if (this._currentMusic != (Song) null && this._state == MusicManager.MusicState.Playing)
      {
        this._state = MusicManager.MusicState.Stopped;
        MediaPlayer.Stop();
      }
      this._currentMusic = song;
      MediaPlayer.Volume = this._saveMasterVolume;
      this._isMusicPaused = false;
      MediaPlayer.IsRepeating = loop;
      MediaPlayer.Play(this._currentMusic);
      this._state = MusicManager.MusicState.Playing;
      if (this.EnabledMusic)
        return;
      this._state = MusicManager.MusicState.Paused;
      MediaPlayer.Pause();
    }

    public void PauseMusic()
    {
      if (!MediaPlayer.GameHasControl || !(this._currentMusic != (Song) null) || this._isMusicPaused)
        return;
      if (this.EnabledMusic)
        MediaPlayer.Pause();
      this._isMusicPaused = true;
      this._state = MusicManager.MusicState.Paused;
    }

    public void ResumeMusic()
    {
      if (!MediaPlayer.GameHasControl || !(this._currentMusic != (Song) null) || !this._isMusicPaused)
        return;
      if (this.EnabledMusic)
      {
        MediaPlayer.Volume = this._saveMasterVolume;
        MediaPlayer.Resume();
      }
      this._isMusicPaused = false;
      this._state = MusicManager.MusicState.Playing;
    }

    public void StopMusic()
    {
      if (!MediaPlayer.GameHasControl)
        return;
      if (this._currentMusic != (Song) null && this._state != MusicManager.MusicState.Stopped)
      {
        this._state = MusicManager.MusicState.Stopped;
        this._isMusicPaused = false;
        MediaPlayer.Stop();
        MediaPlayer.Volume = this._saveMasterVolume;
      }
      this._currentMusic = (Song) null;
    }

    public float GetFadeVolume()
    {
      return MathHelper.Lerp(this._sourceVolume, this._targetVolume, (float) this._elapsedTime.Ticks / (float) this._fadeDuration.Ticks);
    }

    public void FadeMusic(float targetVolume, int msecs)
    {
      this.FadeMusic(targetVolume, new TimeSpan(0, 0, 0, 0, msecs));
    }

    public void FadeMusic(float targetVolume, TimeSpan fadeDuration)
    {
      this._sourceVolume = this.MasterVolume;
      this._targetVolume = targetVolume;
      this._fadeDuration = fadeDuration;
      this._elapsedTime = TimeSpan.Zero;
      this._isFading = true;
    }

    public void CancelFade()
    {
      if (!this._isFading)
        return;
      this._isFading = false;
      this.StopMusic();
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      if (!BrainGame.IsGameActive)
        return;
      if (this._currentMusic != (Song) null && this._state == MusicManager.MusicState.Stopped)
      {
        this._currentMusic = (Song) null;
        this._isMusicPaused = false;
      }
      if (!MediaPlayer.GameHasControl || !this._isFading || this._isMusicPaused || this._state != MusicManager.MusicState.Playing)
        return;
      this._elapsedTime += gameTime.ElapsedGameTime;
      if (this._elapsedTime >= this._fadeDuration)
      {
        this._elapsedTime = this._fadeDuration;
        this._isFading = false;
      }
      MediaPlayer.Volume = this.GetFadeVolume();
      if (this._isFading)
        return;
      this.StopMusic();
      if (this.OnFadeOut == null)
        return;
      this.OnFadeOut();
    }

    public void DisableMusic()
    {
      this.EnabledMusic = false;
      this.PauseMusic();
    }

    public void EnableMusic()
    {
      this.EnabledMusic = true;
      this.ResumeMusic();
    }

    public enum MusicState
    {
      Stopped,
      Playing,
      Paused,
    }

    public delegate void FadeOutMusicHandler();
  }
}
