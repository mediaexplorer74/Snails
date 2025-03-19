
// Type: TwoBrainsGames.BrainEngine.BrainException
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using System;


namespace TwoBrainsGames.BrainEngine
{
  public class BrainException : Exception
  {
    public BrainException()
    {
    }

    public BrainException(string message)
      : base(message)
    {
    }

    public BrainException(string message, Exception inner)
      : base(message, inner)
    {
    }
  }
}
