
// Type: TwoBrainsGames.BrainEngine.Data.DataFiles.DataFileFormatException
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using System;


namespace TwoBrainsGames.BrainEngine.Data.DataFiles
{
  public class DataFileFormatException : BrainException
  {
    public DataFileFormatException()
    {
    }

    public DataFileFormatException(string message)
      : base(message)
    {
    }

    public DataFileFormatException(string message, Exception ex)
      : base(message, ex)
    {
    }
  }
}
