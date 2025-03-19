
// Type: TwoBrainsGames.BrainEngine.Effects.ColorEffect
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;


namespace TwoBrainsGames.BrainEngine.Effects
{
  public class ColorEffect : TransformEffectBase
  {
    private Vector4 _startColor;
    private Vector4 _endColor;
    private Vector4 _vColor;
    private float _speed;
    private bool _loop;
    public Color _finalColor;

    public Color StartColor
    {
      set => this._startColor = value.ToVector4();
      get => new Color(this._startColor);
    }

    public Color EndColor
    {
      set => this._endColor = value.ToVector4();
      get => new Color(this._endColor);
    }

    public Color CurrentColor
    {
      set => this._vColor = value.ToVector4();
      get => new Color(this._vColor);
    }

    public override bool Ended
    {
      get => base.Ended;
      set
      {
        base.Ended = value;
        if (!value)
          return;
        this.ColorVector = this._finalColor.ToVector4();
      }
    }

    public ColorEffect(Color startColor, Color endColor, float speed, bool loop)
      : this(startColor, endColor, speed, loop, endColor)
    {
    }

    public ColorEffect(
      Color startColor,
      Color endColor,
      float speed,
      bool loop,
      Color finalColor)
      : this(startColor, endColor, speed, loop, finalColor, 0.0)
    {
    }

    public ColorEffect(
      Color startColor,
      Color endColor,
      float speed,
      bool loop,
      Color finalColor,
      double expirationTime)
    {
      this._startColor = startColor.ToVector4();
      this._endColor = endColor.ToVector4();
      this._vColor = this._startColor;
      this.ColorVector = this._startColor;
      this._speed = speed;
      this._loop = loop;
      this._finalColor = finalColor;
      this._expirationTime = expirationTime;
    }

    public override void Reset()
    {
      base.Reset();
      this._vColor = this._startColor;
    }

    public override void Update(BrainGameTime gameTime)
    {
      float num = (float) ((double) this._speed * (this.UseRealTime ? gameTime.ElapsedRealTime.TotalMilliseconds : gameTime.ElapsedGameTime.TotalMilliseconds) / 10.0);
      this._vColor.X += (this._endColor.X - this._startColor.X) * num;
      this._vColor.Y += (this._endColor.Y - this._startColor.Y) * num;
      this._vColor.Z += (this._endColor.Z - this._startColor.Z) * num;
      this._vColor.W += (this._endColor.W - this._startColor.W) * num;
      if ((double) this._endColor.X > (double) this._startColor.X && (double) this._vColor.X > (double) this._endColor.X)
        this._vColor.X = this._endColor.X;
      if ((double) this._endColor.Y > (double) this._startColor.Y && (double) this._vColor.Y > (double) this._endColor.Y)
        this._vColor.Y = this._endColor.Y;
      if ((double) this._endColor.Z > (double) this._startColor.Z && (double) this._vColor.Z > (double) this._endColor.Z)
        this._vColor.Z = this._endColor.Z;
      if ((double) this._endColor.W > (double) this._startColor.W && (double) this._vColor.W > (double) this._endColor.W)
        this._vColor.W = this._endColor.W;
      if ((double) this._endColor.X < (double) this._startColor.X && (double) this._vColor.X < (double) this._endColor.X)
        this._vColor.X = this._endColor.X;
      if ((double) this._endColor.Y < (double) this._startColor.Y && (double) this._vColor.Y < (double) this._endColor.Y)
        this._vColor.Y = this._endColor.Y;
      if ((double) this._endColor.Z < (double) this._startColor.Z && (double) this._vColor.Z < (double) this._endColor.Z)
        this._vColor.Z = this._endColor.Z;
      if ((double) this._endColor.W < (double) this._startColor.W && (double) this._vColor.W < (double) this._endColor.W)
        this._vColor.W = this._endColor.W;
      if (this._endColor == this._vColor)
      {
        if (this._loop)
        {
          Vector4 endColor = this._endColor;
          this._endColor = this._startColor;
          this._startColor = endColor;
          this._vColor = this._startColor;
        }
        else
          this.Ended = true;
      }
      this.ColorVector = this._vColor;
    }

    public void Reverse()
    {
      Vector4 startColor = this._startColor;
      this._startColor = this._vColor;
      this._endColor = startColor;
      this.Ended = false;
    }

    public void Reset(Color startColor, Color endColor)
    {
      base.Reset();
      this.StartColor = startColor;
      this.EndColor = endColor;
      this._vColor = this._startColor;
      this.Ended = false;
    }
  }
}
