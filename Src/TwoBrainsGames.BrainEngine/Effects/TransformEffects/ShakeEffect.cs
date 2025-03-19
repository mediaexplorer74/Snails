
// Type: TwoBrainsGames.BrainEngine.Effects.TransformEffects.ShakeEffect
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;


namespace TwoBrainsGames.BrainEngine.Effects.TransformEffects
{
  public class ShakeEffect : TransformEffectBase, ITransformEffect
  {
    private int _ShakeCurrentTime;
    private int _ShakeTime;
    private int _ShakeStrength;
    private Vector2 _PreviousShake;

    public ShakeEffect()
    {
    }

    public ShakeEffect(int shakeTime, int shakeStrength) => this.Reset(shakeTime, shakeStrength);

    public override void Update(BrainGameTime gameTime)
    {
      if (this._ShakeCurrentTime <= 0)
      {
        this.Ended = true;
        this.Active = false;
      }
      else
      {
        int num = this._ShakeCurrentTime * this._ShakeStrength / this._ShakeTime;
        int x = num - BrainGame.Rand.Next(num * 2);
        int y = num - BrainGame.Rand.Next(num * 2);
        this._ShakeCurrentTime -= gameTime.ElapsedGameTime.Milliseconds;
        this.VirtualPosition = new Vector3((float) x - this._PreviousShake.X, (float) y - this._PreviousShake.Y, 0.0f);
        this._PreviousShake = new Vector2((float) x, (float) y);
      }
    }

    public void Reset(int shakeTime, int shakeStrength)
    {
      this.Reset();
      this._ShakeTime = shakeTime;
      this._ShakeStrength = shakeStrength;
      this._ShakeCurrentTime = shakeTime;
    }
  }
}
