
// Type: TwoBrainsGames.Snails.Effects.LiquidGravityEffect
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;


namespace TwoBrainsGames.Snails.Effects
{
  public class LiquidGravityEffect : TransformEffectBase
  {
    private float _gravity;

    public LiquidGravityEffect(float gravity) => this._gravity = gravity;

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      this.Position = new Vector3(0.0f, (float) gameTime.ElapsedGameTime.TotalMilliseconds / 100f * this._gravity, 0.0f);
    }
  }
}
