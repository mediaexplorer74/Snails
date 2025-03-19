
// Type: TwoBrainsGames.BrainEngine.Effects.FlickEffect
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;


namespace TwoBrainsGames.BrainEngine.Effects
{
  public class FlickEffect : TransformEffectBase
  {
    public const float _friction = 0.99f;
    private Vector2 _speed;
    private Vector2 _initialSpeed;

    public Vector2 Speed
    {
      get => this._speed;
      set => this._speed = value;
    }

    public FlickEffect(Vector2 speed) => this.Reset(speed);

    public override void Update(BrainGameTime gameTime)
    {
      this.PositionV2 = this._speed * (float) gameTime.ElapsedRealTime.TotalSeconds;
      this._speed *= Math.Abs((float) (0.949999988079071 - 0.99000000953674316 * gameTime.ElapsedGameTime.TotalSeconds));
      if (((double) Math.Abs(this._speed.X) > 25.0 || (double) Math.Abs(this._speed.Y) > 25.0) && !(this._speed == Vector2.Zero))
        return;
      this._speed = Vector2.Zero;
      this.Active = false;
    }

    public override void Reset()
    {
      base.Reset();
      this._speed = this._initialSpeed;
    }

    public void Reset(Vector2 speed)
    {
      base.Reset();
      this._speed = this._initialSpeed = speed;
    }
  }
}
