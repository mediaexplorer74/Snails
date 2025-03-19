
// Type: TwoBrainsGames.Snails.LanguageType
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using System;


namespace TwoBrainsGames.Snails
{
  [Flags]
  public enum LanguageType
  {
    English = 1,
    Portuguese = 2,
    German = 4,
    French = 8,
    Spanish = 16, // 0x00000010
    Italian = 32, // 0x00000020
  }
}
