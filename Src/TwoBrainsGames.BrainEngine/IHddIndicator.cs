
// Type: TwoBrainsGames.BrainEngine.IHddIndicator
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace TwoBrainsGames.BrainEngine
{
  public interface IHddIndicator : IBrainComponent
  {
    bool Visible { get; set; }

    void Draw(SpriteBatch spriteBatch);

    void Draw(SpriteBatch spriteBatch, Vector2 position);
  }
}
