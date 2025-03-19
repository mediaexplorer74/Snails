
// Type: TwoBrainsGames.BrainEngine.Data.Content.StageDataReader
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Content;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.Snails;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.BrainEngine.Data.Content
{
  public class StageDataReader : ContentTypeReader<StageData>
  {
    protected override StageData Read(ContentReader input, StageData existingInstance)
    {
      StageData contentObject = new StageData();
      GenericContentReader.Read(input, (IDataFileSerializable) contentObject, Game1.Ek);
      return contentObject;
    }
  }
}
