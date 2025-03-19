
// Type: TwoBrainsGames.BrainEngine.Effects.TransformEffects.GravityEffect
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;


namespace TwoBrainsGames.BrainEngine.Effects.TransformEffects
{
  public class GravityEffect : TransformEffectBase, ITransformEffect
  {
    private float _gravity;
    private double _t;
    private float _initialSpeed;
    private float prev;

    public GravityEffect(float gravity, float initialSpeed)
    {
      this._initialSpeed = initialSpeed;
      this._gravity = gravity;
      this._t = 0.0;
      this.prev = 0.0f;
    }

    public override void Update(BrainGameTime gameTime)
    {
      this._t += gameTime.ElapsedGameTime.TotalMilliseconds / 100.0;
      float num = (float) ((double) this._initialSpeed * this._t - 0.5 * (double) this._gravity * (this._t * this._t));
      this.Position = new Vector3(0.0f, (float) -((double) num - (double) this.prev), 0.0f);
      this.prev = num;
    }
  }
}
