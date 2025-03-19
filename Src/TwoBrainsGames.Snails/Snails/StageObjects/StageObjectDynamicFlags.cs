
// Type: TwoBrainsGames.Snails.StageObjects.StageObjectDynamicFlags
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using System;


namespace TwoBrainsGames.Snails.StageObjects
{
  [Flags]
  public enum StageObjectDynamicFlags
  {
    None = 0,
    IsVisible = 1,
    IsVitaminized = 2,
    IsFalling = 4,
    IsDead = 8,
    IsDisposed = 16, // 0x00000010
    IsEating = 32, // 0x00000020
    IsUnderLiquid = 64, // 0x00000040
  }
}
