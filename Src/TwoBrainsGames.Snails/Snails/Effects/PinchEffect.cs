
// Type: TwoBrainsGames.Snails.Effects.PinchEffect
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.Effects
{
  internal class PinchEffect : TransformEffectBase
  {
    private float _startScale;
    private float _speed;

    public PinchEffect(InGameCamera camera)
    {
    }

    public PinchEffect(float startScale, float lastScaleDelta)
    {
      this.Reset(startScale, lastScaleDelta);
    }

        public override void Update(BrainGameTime gameTime)
        {
            this._speed *= Math.Abs((float)(0.99000000953674316 - 0.99000000953674316 * gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0));
            if ((double)Math.Abs(this._speed) <= 0.0099999997764825821)
            {
                this.Active = false;
            }
            else
            {
                this.Scale += new Vector2(this._speed, this._speed);
            }
        }

    public void Reset(float startScale, float lastScaleDelta)
    {
      this.Reset();
      this._startScale = startScale;
      this.Scale = new Vector2(this._startScale, this._startScale);
      this._speed = lastScaleDelta;
    }
  }
}
