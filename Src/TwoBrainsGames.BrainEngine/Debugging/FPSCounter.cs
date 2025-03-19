
// Type: TwoBrainsGames.BrainEngine.Debugging.FPSCounter
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;


namespace TwoBrainsGames.BrainEngine.Debugging
{
  public class FPSCounter(Game game) : BrainPerformanceCounter(game)
  {
    private float totalFrames;

    private float ElapsedTime { get; set; }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      this.ElapsedTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
      ++this.totalFrames;
      if ((double) this.ElapsedTime < 1000.0)
        return;
      this.Counter = (double) this.totalFrames;
      this.totalFrames = 0.0f;
      this.ElapsedTime -= 1000f;
    }
  }
}
