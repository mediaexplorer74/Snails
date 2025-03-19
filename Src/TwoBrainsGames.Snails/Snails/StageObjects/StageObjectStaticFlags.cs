
// Type: TwoBrainsGames.Snails.StageObjects.StageObjectStaticFlags
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using System;


namespace TwoBrainsGames.Snails.StageObjects
{
  [Flags]
  public enum StageObjectStaticFlags
  {
    None = 0,
    CanFall = 1,
    CanCollide = 2,
    CanHoover = 4,
    CanWalk = 8,
    CanWalkOnWalls = 16, // 0x00000010
    CanDieWithExplosions = 32, // 0x00000020
    CanDie = 64, // 0x00000040
    CanDieWithFire = 128, // 0x00000080
    CanDieWithAnyTypeOfExplosion = 256, // 0x00000100
    CanDieWithCrates = 1024, // 0x00000400
    IgnoreLiquidCollisions = 2048, // 0x00000800
  }
}
