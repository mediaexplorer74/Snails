
// Type: TwoBrainsGames.Snails.SnailsException
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using System;
using TwoBrainsGames.BrainEngine;


namespace TwoBrainsGames.Snails
{
  internal class SnailsException : BrainException
  {
    public SnailsException()
    {
    }

    public SnailsException(string message)
      : base(message)
    {
    }

    public SnailsException(string message, Exception innerEx)
      : base(message, innerEx)
    {
    }
  }
}
