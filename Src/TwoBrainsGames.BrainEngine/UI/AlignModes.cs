
// Type: TwoBrainsGames.BrainEngine.UI.AlignModes
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using System;


namespace TwoBrainsGames.BrainEngine.UI
{
  [Flags]
  public enum AlignModes
  {
    None = 0,
    Horizontaly = 1,
    Vertically = 2,
    HorizontalyVertically = Vertically | Horizontaly, // 0x00000003
    Right = 4,
    Bottom = 8,
    Top = 256, // 0x00000100
    Left = 512, // 0x00000200
  }
}
