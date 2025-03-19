
// Type: TwoBrainsGames.BrainEngine.Data.DataFiles.FlagsType
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer


namespace TwoBrainsGames.BrainEngine.Data.DataFiles
{
  public struct FlagsType
  {
    public int Value;

    public static FlagsType Zero => new FlagsType();

    public override string ToString() => this.Value.ToString();
  }
}
