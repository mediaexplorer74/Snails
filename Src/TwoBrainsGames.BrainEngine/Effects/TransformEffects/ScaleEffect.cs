
// Type: TwoBrainsGames.BrainEngine.Effects.TransformEffects.ScaleEffect
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;


namespace TwoBrainsGames.BrainEngine.Effects.TransformEffects
{
  public class ScaleEffect : TransformEffectBase
  {
    private Vector2 _initialScale;
    private float _speed;
    private Vector2 _finalScale;
    private bool _loop;
    private float _directionX;
    private float _directionY;
    private float _minX;
    private float _maxX;
    private float _minY;
    private float _maxY;
    private float _decayFactor;
    private float _decayThreshold;

    public ScaleEffect()
      : this(new Vector2(1f, 1f), 0.01f, new Vector2(2f, 2f), false)
    {
    }

    public ScaleEffect(Vector2 initialScale, float speed, Vector2 finalScale, bool loop)
    {
      this.Reset(initialScale, speed, finalScale, loop, 0.0f, 0.0f);
    }

    public ScaleEffect(
      Vector2 initialScale,
      float speed,
      Vector2 finalScale,
      bool loop,
      float decayFactor,
      float decayThreshold)
    {
      this.Reset(initialScale, speed, finalScale, loop, decayFactor, decayThreshold);
    }

    public void Reset(
      Vector2 initialScale,
      float speed,
      Vector2 finalScale,
      bool loop,
      float decayFactor,
      float decayThreshold)
    {
      this._initialScale = initialScale;
      this._speed = speed;
      this._finalScale = finalScale;
      this._loop = loop;
      this._decayFactor = decayFactor;
      this._decayThreshold = decayThreshold;
      this._minX = this._initialScale.X;
      this._maxX = this._finalScale.X;
      if ((double) this._minX > (double) this._maxX)
      {
        this._minX = this._finalScale.X;
        this._maxX = this._initialScale.X;
      }
      this._minY = this._initialScale.Y;
      this._maxY = this._finalScale.Y;
      if ((double) this._minY > (double) this._maxY)
      {
        this._minY = this._finalScale.Y;
        this._maxY = this._initialScale.Y;
      }
      this.Reset();
    }

    public override void Reset()
    {
      base.Reset();
      this._directionX = (double) this._initialScale.X > (double) this._finalScale.X ? -1f : 1f;
      this._directionY = (double) this._initialScale.Y > (double) this._finalScale.Y ? -1f : 1f;
      this.Scale = this._initialScale;
    }

    public override void Update(BrainGameTime gameTime)
    {
      float num = (float) ((double) this._speed * gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0);
      float x = this.Scale.X + num * this._directionX;
      float y = this.Scale.Y + num * this._directionY;
      if ((double) this._decayFactor > 0.0)
      {
        if ((double) this._directionX > 0.0)
        {
          if ((double) this.Scale.X > (double) this._decayThreshold || (double) this.Scale.Y > (double) this._decayThreshold)
            this._speed *= Math.Abs((float) (0.949999988079071 - (double) this._decayFactor * gameTime.ElapsedGameTime.TotalSeconds));
        }
        else if ((double) this.Scale.X < (double) this._decayThreshold || (double) this.Scale.Y < (double) this._decayThreshold)
          this._speed *= Math.Abs((float) (0.949999988079071 - (double) this._decayFactor * gameTime.ElapsedGameTime.TotalSeconds));
        if ((double) Math.Abs(this._speed) <= 0.05000000074505806)
        {
          x = (double) this._directionX <= 0.0 ? this._minX : this._maxX;
          y = (double) this._directionY <= 0.0 ? this._minY : this._maxY;
        }
      }
      if ((double) x > (double) this._maxX)
      {
        x = this._maxX;
        if (this._loop)
          this._directionX = -1f;
      }
      if ((double) x < (double) this._minX)
      {
        x = this._minX;
        if (this._loop)
          this._directionX = 1f;
      }
      if ((double) y > (double) this._maxY)
      {
        y = this._maxY;
        if (this._loop)
          this._directionY = -1f;
      }
      if ((double) y < (double) this._minX)
      {
        y = this._minX;
        if (this._loop)
          this._directionY = 1f;
      }
      this.Scale = new Vector2(x, y);
      if ((double) x != (double) this._finalScale.X || (double) y != (double) this._finalScale.Y || this._loop)
        return;
      this.Ended = true;
    }
  }
}
