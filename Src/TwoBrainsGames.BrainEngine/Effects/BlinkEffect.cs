
// Type: TwoBrainsGames.BrainEngine.Effects.BlinkEffect
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI.Controls;


namespace TwoBrainsGames.BrainEngine.Effects
{
  public class BlinkEffect : TransformEffectBase
  {
    private double _ellapsed;
    private double _visibleTime;
    private double _hiddenTime;
    private UIControl _control;
    private Sample _blinkSound;

    public bool Visible { get; private set; }

    public BlinkEffect(double timeVisible, double timeHidden)
      : this(timeVisible, timeHidden, (UIControl) null)
    {
    }

    public BlinkEffect(double timeVisible, double timeHidden, UIControl control)
      : this(timeVisible, timeHidden, control, (Sample) null, Color.White)
    {
    }

    public BlinkEffect(
      double timeVisible,
      double timeHidden,
      UIControl control,
      Sample blinkSound,
      Color color)
    {
      this._visibleTime = timeVisible;
      this._hiddenTime = timeHidden;
      this._control = control;
      this._blinkSound = blinkSound;
      this.ColorVector = color.ToVector4();
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      this._ellapsed += gameTime.ElapsedGameTime.TotalMilliseconds;
      if (this._ellapsed > this._visibleTime && this.Visible)
      {
        this._ellapsed -= this._visibleTime;
        this.Visible = false;
        if (this._control == null)
          return;
        this._control.Hidden = true;
      }
      else
      {
        if (this._ellapsed <= this._hiddenTime || this.Visible)
          return;
        this._ellapsed -= this._hiddenTime;
        this.Visible = !this.Visible;
        if (this._blinkSound != null)
          this._blinkSound.Play();
        if (this._control == null)
          return;
        this._control.Hidden = false;
      }
    }

    public override void Reset()
    {
      base.Reset();
      this._ellapsed = 0.0;
      this.Visible = true;
    }
  }
}
