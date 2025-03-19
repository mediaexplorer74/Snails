
// Type: TwoBrainsGames.BrainEngine.Effects.TransformEffects.LinearMoveEffect
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;


namespace TwoBrainsGames.BrainEngine.Effects.TransformEffects
{
  public class LinearMoveEffect : TransformEffectBase, ITransformEffect
  {
    private float _Speed;
    private float _AngleSin;
    private float _AngleCos;
    private Vector3 _direction;

    public LinearMoveEffect(float speed, float angle)
    {
      this._Speed = speed;
      this._AngleSin = (float) Math.Sin((double) MathHelper.ToRadians(angle));
      this._AngleCos = (float) Math.Cos((double) MathHelper.ToRadians(angle));
      this._direction = new Vector3(this._AngleCos, -this._AngleSin, 0.0f);
    }

    public override void Update(BrainGameTime gameTime)
    {
      this.Position = this._direction * (float) ((double) this._Speed * (double) gameTime.ElapsedGameTime.Milliseconds / 10.0);
    }
  }
}
