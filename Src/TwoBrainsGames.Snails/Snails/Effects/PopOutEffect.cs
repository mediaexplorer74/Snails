
// Type: TwoBrainsGames.Snails.Effects.PopOutEffect
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;


namespace TwoBrainsGames.Snails.Effects
{
  public class PopOutEffect : TransformEffectBase
  {
    private bool _increasing;
    private Vector2 _maxScale;
    private float _speed;
    private float _initialSpeed;
    private Color _blendColor;
    private Vector2 _initialScale;

    public PopOutEffect(Vector2 maxScale, float speed)
      : this(maxScale, speed, Color.White, new Vector2(1f, 1f))
    {
    }

    public PopOutEffect(Vector2 maxScale, float speed, Color color, Vector2 initialScale)
    {
      this._maxScale = maxScale;
      this._speed = speed;
      this._initialSpeed = speed;
      this._initialScale = initialScale;
      this._blendColor = color;
      this.Reset();
    }

    public override void Update(BrainGameTime gameTime)
    {
      float num = (float) ((double) this._speed * gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0);
      float x;
      float y;
      if (this._increasing)
      {
        x = this.Scale.X + num;
        y = this.Scale.Y + num;
        if ((double) x > (double) this._maxScale.X)
          x = this._maxScale.X;
        if ((double) y > (double) this._maxScale.Y)
          y = this._maxScale.Y;
        if ((double) x == (double) this._maxScale.X && (double) y == (double) this._maxScale.Y)
          this._increasing = false;
      }
      else
      {
        x = this.Scale.X - num;
        y = this.Scale.Y - num;
        if ((double) x < 0.0)
          x = 0.0f;
        if ((double) y < 0.0)
          y = 0.0f;
        if ((double) x == 0.0 && (double) y == 0.0)
          this.Ended = true;
      }
      this.Scale = new Vector2(x, y);
    }

    public override void Reset()
    {
      base.Reset();
      this._increasing = true;
      this._speed = this._initialSpeed;
      this.ColorVector = this._blendColor.ToVector4();
      this.Scale = this._initialScale;
    }
  }
}
