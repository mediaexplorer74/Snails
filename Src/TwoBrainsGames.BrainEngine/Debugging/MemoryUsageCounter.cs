
// Type: TwoBrainsGames.BrainEngine.Debugging.MemoryUsageCounter
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

//using Microsoft.Phone.Info;
using Microsoft.Xna.Framework;


namespace TwoBrainsGames.BrainEngine.Debugging
{
  internal class MemoryUsageCounter(Game game) : BrainPerformanceCounter(game)
  {
    public override void Update(GameTime gameTime)
    {
       this.Counter = 128f;//(double) (DeviceStatus.ApplicationCurrentMemoryUsage / 1024L);
    }
  }
}
