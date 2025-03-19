
// Type: TwoBrainsGames.BrainEngine.BrainGameTime
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;


namespace TwoBrainsGames.BrainEngine
{
  public class BrainGameTime
  {
    private TimeSpan _elapsedGameTime = TimeSpan.Zero;
    private TimeSpan _totalGameTime = TimeSpan.Zero;
    private TimeSpan _elapsedRealTime = TimeSpan.Zero;
    private TimeSpan _totalRealTime = TimeSpan.Zero;
    private int _multiplier = 1;

    public void Update(GameTime gameTime)
    {
      this._elapsedRealTime = gameTime.ElapsedGameTime;
      this._totalRealTime = gameTime.TotalGameTime;
      this._elapsedGameTime = new TimeSpan(gameTime.ElapsedGameTime.Ticks * (long) this._multiplier);
      this._totalGameTime = this._totalGameTime.Add(this._elapsedGameTime);
    }

    public void SetMultiplier(float multiplier)
    {
      this._multiplier = (int) multiplier;
      this._elapsedGameTime = new TimeSpan(this._elapsedRealTime.Ticks * (long) this._multiplier);
    }

    public void Reset() => this._multiplier = 1;

    public int Multiplier
    {
      get => this._multiplier;
      set => this._multiplier = value;
    }

    public TimeSpan ElapsedGameTime
    {
      get => this._elapsedGameTime;
      set => this._elapsedGameTime = value;
    }

    public TimeSpan TotalGameTime
    {
      get => this._totalGameTime;
      set => this._totalGameTime = value;
    }

    public TimeSpan ElapsedRealTime
    {
      get => this._elapsedRealTime;
      set => this._elapsedRealTime = value;
    }

    public TimeSpan TotalRealTime
    {
      get => this._totalRealTime;
      set => this._totalRealTime = value;
    }
  }
}
