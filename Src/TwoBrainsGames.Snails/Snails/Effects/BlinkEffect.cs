
// Type: TwoBrainsGames.Snails.Effects.BlinkEffect
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Resources;


namespace TwoBrainsGames.Snails.Effects
{
  public class BlinkEffect : TransformEffectBase
  {
    private double _ellapsed;
    private double _currentVisibleTime;
    private double _currentHiddenTime;
    private double _ellapsedDuration;
    private Sample _blinkSound;

    public bool Visible { get; private set; }

    public double Duration { get; set; }

    public double Decay { get; set; }

    public double VisibleTime { get; set; }

    public double HiddenTime { get; set; }

    public float MinBLink { get; set; }

    public BlinkEffect(double timeVisible, double timeHidden)
      : this(timeVisible, timeHidden, (Sample) null, 0.0, 0.0)
    {
    }

    public BlinkEffect(
      double timeVisible,
      double timeHidden,
      Sample blinkSound,
      double duration,
      double decay)
    {
      this._blinkSound = blinkSound;
      this.Duration = duration;
      this.Decay = decay;
      this.VisibleTime = this._currentVisibleTime = timeVisible;
      this.HiddenTime = this._currentHiddenTime = timeHidden;
      this.MinBLink = 10f;
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      this._ellapsed += this.UseRealTime ? gameTime.ElapsedRealTime.TotalMilliseconds : gameTime.ElapsedGameTime.TotalMilliseconds;
      if (this._ellapsed > this._currentVisibleTime && this.Visible)
      {
        this._ellapsed -= this._currentVisibleTime;
        this.Visible = false;
      }
      else if (this._ellapsed > this._currentHiddenTime && !this.Visible)
      {
        this._ellapsed -= this._currentHiddenTime;
        this.Visible = !this.Visible;
        if (this._blinkSound != null)
          this._blinkSound.Play();
      }
      if (this.Duration != 0.0)
      {
        this._ellapsedDuration += gameTime.ElapsedGameTime.TotalMilliseconds;
        if (this._ellapsedDuration > this.Duration)
          this.Ended = true;
      }
      if (this.Decay == 0.0)
        return;
      this._currentHiddenTime -= this.Decay * (gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0);
      this._currentVisibleTime -= this.Decay * (gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0);
      if (this._currentHiddenTime < (double) this.MinBLink)
        this._currentHiddenTime = (double) this.MinBLink;
      if (this._currentHiddenTime < (double) this.MinBLink)
        this._currentVisibleTime = (double) this.MinBLink;
      if (this.Duration != 0.0)
        return;
      this.Ended = true;
    }

    public override void Reset()
    {
      base.Reset();
      this._ellapsed = 0.0;
      this._ellapsedDuration = 0.0;
      this._currentVisibleTime = this.VisibleTime;
      this._currentHiddenTime = this.VisibleTime;
      this.Visible = true;
    }
  }
}
