
// Type: TwoBrainsGames.BrainEngine.Effects.TransformEffects.HooverEffect
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;


namespace TwoBrainsGames.BrainEngine.Effects.TransformEffects
{
  public class HooverEffect : TransformEffectBase, ITransformEffect
  {
    protected Vector3 _previousPosition;

    private float Speed { get; set; }

    private float Power { get; set; }

    private float Angle { get; set; }

    private float HooverAngle { get; set; }

    private Vector3 CurrentPosition { get; set; }

    public HooverEffect(float speed, float power, float angle)
    {
      this.Speed = speed;
      this.Power = power;
      this.HooverAngle = angle;
      this.Angle = 0.0f;
    }

    public override void Update(BrainGameTime gameTime)
    {
      this.CurrentPosition = new Vector3(0.0f, (float) (Math.Sin((double) MathHelper.ToRadians(this.Angle)) * (double) this.Power * 10.0), 0.0f);
      this.CurrentPosition = Vector3.Transform(this.CurrentPosition, Matrix.CreateRotationZ(MathHelper.ToRadians(this.HooverAngle)));
      this.Angle += (float) (1.0 * gameTime.ElapsedGameTime.TotalMilliseconds) * this.Speed / this.Power;
      if ((double) this.Angle > 360.0)
        this.Angle -= 360f;
      this.Position = this.CurrentPosition - this._previousPosition;
      this._previousPosition = this.CurrentPosition;
    }

    public override void Reset() => base.Reset();
  }
}
