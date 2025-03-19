
// Type: TwoBrainsGames.BrainEngine.Data.Content.SpriteSetReader
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Content;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Resources;


namespace TwoBrainsGames.BrainEngine.Data.Content
{
  public class SpriteSetReader : ContentTypeReader<SpriteSet>
  {
    protected override SpriteSet Read(ContentReader input, SpriteSet existingInstance)
    {
      SpriteSet contentObject = new SpriteSet();
      GenericContentReader.Read(input, (IDataFileSerializable) contentObject, BrainGame.Ek);
      contentObject.LoadContent(input.ContentManager);
      return contentObject;
    }
  }
}
