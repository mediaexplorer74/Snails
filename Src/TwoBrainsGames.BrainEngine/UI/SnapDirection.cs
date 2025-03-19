
// Type: TwoBrainsGames.BrainEngine.UI.SnapDirection
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using System;


namespace TwoBrainsGames.BrainEngine.UI
{
  [Flags]
  public enum SnapDirection
  {
    None = 0,
    Left = 1,
    Right = 2,
    Up = 4,
    Down = 8,
    All = Down | Up | Right | Left, // 0x0000000F
  }
}
