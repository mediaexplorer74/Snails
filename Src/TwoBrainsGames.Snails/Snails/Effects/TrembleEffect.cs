
// Type: TwoBrainsGames.Snails.Effects.TrembleEffect
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;


namespace TwoBrainsGames.Snails.Effects
{
  internal class TrembleEffect : TransformEffectBase
  {
    private const float MIN_ANGLE = -1f;
    private const float MAX_ANGLE = 1f;
    private int _direction;

    public override void Update(BrainGameTime gameTime)
    {
      this.Rotation += (float) (0.05000000074505806 * (double) this._direction * gameTime.ElapsedGameTime.TotalMilliseconds);
      if ((double) this.Rotation < -1.0)
      {
        this.Rotation = -1f;
        this._direction *= -1;
      }
      else
      {
        if ((double) this.Rotation <= 1.0)
          return;
        this.Rotation = 1f;
        this._direction *= -1;
      }
    }

    public override void Reset()
    {
      base.Reset();
      this._direction = 1;
      this.Rotation = 0.0f;
    }
  }
}
