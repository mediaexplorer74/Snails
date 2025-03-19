
// Type: TwoBrainsGames.BrainEngine.Data.Content.TutorialReader
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Content;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.Snails;
using TwoBrainsGames.Snails.Tutorials;


namespace TwoBrainsGames.BrainEngine.Data.Content
{
  public class TutorialReader : ContentTypeReader<Tutorial>
  {
    protected override Tutorial Read(ContentReader input, Tutorial existingInstance)
    {
      Tutorial contentObject = new Tutorial();
      GenericContentReader.Read(input, (IDataFileSerializable) contentObject, Game1.Ek);
      return contentObject;
    }
  }
}
