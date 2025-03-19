
// Type: TwoBrainsGames.Snails.ContentReaders.ScreensDataReader
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Content;
using TwoBrainsGames.BrainEngine.Data.Content;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.Snails.ContentReaders
{
  public class ScreensDataReader : ContentTypeReader<ScreensData>
  {
    protected override ScreensData Read(ContentReader input, ScreensData existingInstance)
    {
      ScreensData contentObject = new ScreensData();
      GenericContentReader.Read(input, (IDataFileSerializable) contentObject, Game1.Ek);
      return contentObject;
    }
  }
}
