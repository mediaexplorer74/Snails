
// Type: TwoBrainsGames.BrainEngine.Effects.ParticlesEffect
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Graphics;


namespace TwoBrainsGames.BrainEngine.Effects
{
  public class ParticlesEffect
  {
    protected int _numParticles;
    protected bool _ended;

    public int NumParticles
    {
      get => this._numParticles;
      set => this._numParticles = value;
    }

    public bool Ended
    {
      get => this._ended;
      set => this._ended = value;
    }

    public ParticlesEffect() => this._numParticles = 0;

    public virtual void Update(BrainGameTime gameTime)
    {
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
    }
  }
}
