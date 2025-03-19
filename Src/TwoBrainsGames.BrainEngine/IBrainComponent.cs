
// Type: TwoBrainsGames.BrainEngine.IBrainComponent
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Graphics;


namespace TwoBrainsGames.BrainEngine
{
  public interface IBrainComponent
  {
    SpriteBatch SpriteBatch { get; }

    void Initialize();

    void LoadContent();

    void Update(BrainGameTime gameTime);

    void Draw();

    void UnloadContent();
  }
}
