
// Type: TwoBrainsGames.BrainEngine.Data.Content.XDFReader
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Content;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile;


namespace TwoBrainsGames.BrainEngine.Data.Content
{
  public class XDFReader : ContentTypeReader<DataFileRecord>
  {
    protected override DataFileRecord Read(ContentReader input, DataFileRecord existingInstance)
    {
      return new BinaryDataFileReader().Read(input.BaseStream, BrainGame.Ek).RootRecord;
    }
  }
}
