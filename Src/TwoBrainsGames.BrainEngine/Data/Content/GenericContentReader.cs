
// Type: TwoBrainsGames.BrainEngine.Data.Content.GenericContentReader
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Content;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile;


namespace TwoBrainsGames.BrainEngine.Data.Content
{
  public class GenericContentReader
  {
    public static void Read(
      ContentReader input,
      IDataFileSerializable contentObject,
      string encryptionKey)
    {
      DataFile dataFile = new BinaryDataFileReader().Read(input.BaseStream, encryptionKey);
      contentObject.InitFromDataFileRecord(dataFile.RootRecord);
    }
  }
}
