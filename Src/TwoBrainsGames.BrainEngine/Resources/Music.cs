
// Type: TwoBrainsGames.BrainEngine.Resources.Music
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using TwoBrainsGames.BrainEngine.Audio;


namespace TwoBrainsGames.BrainEngine.Resources
{
  public class Music : Resource
  {
    private Song _song;
    private MusicManager.MusicState _state;

    public Song Song
    {
      get => this._song;
      set => this._song = value;
    }

    public bool IsPlaying => this._state == MusicManager.MusicState.Playing;

    public bool IsPaused => this._state == MusicManager.MusicState.Paused;

    public bool IsStopped => this._state == MusicManager.MusicState.Stopped;

    public Music()
    {
    }

    public Music(Song song) => this._song = song;

    public override bool Load(ContentManager contentManager)
    {
      this.IsLoaded = false;
      this._song = contentManager.Load<Song>(this.Path);
      if (this._song != (Song) null)
        this.IsLoaded = true;
      return this.IsLoaded;
    }

    public override bool Release(ContentManager contentManager)
    {
      if (this._song != (Song) null)
        this._song.Dispose();
      this.IsLoaded = false;
      return true;
    }

    public void Play() => this.Play(false);

    public void Play(bool loop)
    {
      this._state = MusicManager.MusicState.Playing;
      BrainGame.MusicManager.PlayMusic(this._song, loop);
    }

    public void Pause()
    {
      this._state = MusicManager.MusicState.Paused;
      BrainGame.MusicManager.PauseMusic();
    }

    public void Resume()
    {
      this._state = MusicManager.MusicState.Playing;
      BrainGame.MusicManager.ResumeMusic();
    }

    public void Stop()
    {
      this._state = MusicManager.MusicState.Stopped;
      BrainGame.MusicManager.StopMusic();
    }

    public void Fade(float targetVolume, TimeSpan duration)
    {
      BrainGame.MusicManager.FadeMusic(targetVolume, duration);
      if ((double) targetVolume == 0.0)
        this._state = MusicManager.MusicState.Stopped;
      else
        this._state = MusicManager.MusicState.Playing;
    }

    public void CancelFade()
    {
      BrainGame.MusicManager.CancelFade();
      if ((double) BrainGame.MusicManager.MasterVolume == 0.0)
        this._state = MusicManager.MusicState.Stopped;
      else
        this._state = MusicManager.MusicState.Playing;
    }
  }
}
