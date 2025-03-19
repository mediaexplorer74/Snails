
// Type: TwoBrainsGames.BrainEngine.Effects.TransformEffects.SquashEffect
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;


namespace TwoBrainsGames.BrainEngine.Effects.TransformEffects
{
  public class SquashEffect : TransformEffectBase
  {
    private float _initialSquashLimit;
    private float _squashLimit;
    private float _decay;
    private float _speed;
    private float _direction;
    private Color _blendColor;

    public Vector2 TargetScale { get; set; }

    public Color BlendColor
    {
      get => this._blendColor;
      set
      {
        this._blendColor = value;
        this.ColorVector = this._blendColor.ToVector4();
      }
    }

    public SquashEffect(float squashLimit, float speed, float decay)
      : this(squashLimit, speed, decay, Color.White, new Vector2(1f, 1f))
    {
    }

    public SquashEffect(
      float squashLimit,
      float speed,
      float decay,
      Color color,
      Vector2 targetScale)
    {
      this._squashLimit = this._initialSquashLimit = squashLimit;
      this._decay = decay;
      this._speed = speed;
      this.TargetScale = targetScale;
      this.BlendColor = color;
      this.Reset();
    }

    public override void Update(BrainGameTime gameTime)
    {
      float num = (float) (gameTime.ElapsedGameTime.TotalMilliseconds * (double) this._speed / 1000.0) * this._direction;
      float x = this.Scale.X + num;
      float y = this.Scale.Y - num;
      if ((double) x < (double) this._squashLimit)
      {
        this._direction *= -1f;
        this._squashLimit += this._decay;
        x = this._squashLimit;
        y = this.TargetScale.Y - this._squashLimit + this.TargetScale.Y;
      }
      else if ((double) y < (double) this._squashLimit)
      {
        this._direction *= -1f;
        this._squashLimit += this._decay;
        y = this._squashLimit;
        x = this.TargetScale.X - this._squashLimit + this.TargetScale.X;
      }
      this.Scale = new Vector2(x, y);
      if ((double) this._squashLimit < (double) this.TargetScale.X)
        return;
      this.Ended = true;
      this.Scale = this.TargetScale;
    }

    public override void Reset()
    {
      base.Reset();
      this._squashLimit = this._initialSquashLimit;
      this._direction = -1f;
      this.ColorVector = this.BlendColor.ToVector4();
      this.Scale = this.TargetScale;
    }
  }
}
