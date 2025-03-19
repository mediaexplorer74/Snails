
// Type: TwoBrainsGames.Snails.Stages.WalkFlags
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using System;


namespace TwoBrainsGames.Snails.Stages
{
  [Flags]
  public enum WalkFlags
  {
    None = 0,
    Left = 1,
    Top = 2,
    Right = 4,
    Bottom = 8,
    All = Bottom | Right | Top | Left, // 0x0000000F
    ULCorner = 16, // 0x00000010
    URCorner = 32, // 0x00000020
    LRCorner = 64, // 0x00000040
    LLCorner = 128, // 0x00000080
    MaxFlag = LLCorner | LRCorner | URCorner | ULCorner | All, // 0x000000FF
  }
}
