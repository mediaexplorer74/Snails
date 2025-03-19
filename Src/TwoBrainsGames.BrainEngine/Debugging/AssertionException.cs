
// Type: TwoBrainsGames.BrainEngine.Debugging.AssertionException
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer


namespace TwoBrainsGames.BrainEngine.Debugging
{
  public class AssertionException : BrainException
  {
    public AssertionException()
    {
    }

    public AssertionException(string message)
      : base("Debug Assertion failed! " + message)
    {
    }
  }
}
