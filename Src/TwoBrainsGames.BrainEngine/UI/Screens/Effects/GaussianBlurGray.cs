
// Type: TwoBrainsGames.BrainEngine.UI.Screens.Effects.GaussianBlurGray
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Resources;


namespace TwoBrainsGames.BrainEngine.UI.Screens.Effects
{
  public class GaussianBlurGray : GaussianBlur
  {
    private const string GAUSSIAN_BLUR_RES = "effects/gaussian-blur-gray";

    public GaussianBlurGray(Screen screen)
      : base(screen)
    {
    }

    public GaussianBlurGray(Screen screen, Effect effect, int width, int height)
      : base(screen, effect, width, height)
    {
    }

    public override void LoadContent()
    {
      this._effect = BrainGame.ResourceManager.Load<Effect>("effects/gaussian-blur-gray", ResourceManager.ResourceManagerCacheType.Static);
      this.InitRenderTargets();
    }
  }
}
