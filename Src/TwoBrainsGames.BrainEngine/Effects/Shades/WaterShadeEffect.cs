
// Type: TwoBrainsGames.BrainEngine.Effects.Shades.WaterShadeEffect
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI.Screens.Effects;


namespace TwoBrainsGames.BrainEngine.Effects.Shades
{
  public class WaterShadeEffect : PostProcessor
  {
    private const string WATER_SHADE_RES = "effects/water-waves";
    private RenderTarget2D _renderTarget;
    private int _renderTargetWidth;
    private int _renderTargetHeight;
    private float _elapsedTime;

    public WaterShadeEffect(Screen screen)
      : base(screen)
    {
    }

    public WaterShadeEffect(Screen screen, Effect effect, int width, int height)
      : base(screen, effect, width, height)
    {
    }

    public override void LoadContent()
    {
      this._effect = BrainGame.ResourceManager.Load<Effect>("effects/water-waves", ResourceManager.ResourceManagerCacheType.Static);
    }

    public override void Initialize()
    {
      base.Initialize();
      this._renderTargetWidth = this._width;
      this._renderTargetHeight = this._height;
      this._renderTarget = new RenderTarget2D(this._screen.SpriteBatch.GraphicsDevice, this._renderTargetWidth, this._renderTargetHeight, false, this._screen.SpriteBatch.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.None);
    }

    public Texture2D PerformWaterShade(
      Texture2D srcTexture,
      RenderTarget2D renderTarget,
      SpriteBatch spriteBatch)
    {
      Rectangle destinationRectangle = new Rectangle(0, 0, renderTarget.Width, renderTarget.Height);
      this._screen.SpriteBatch.GraphicsDevice.SetRenderTarget(renderTarget);
      this._effect.CurrentTechnique = this._effect.Techniques["UnderWater"];
      this._effect.Parameters["fTimer"].SetValue(this._elapsedTime);
      this._effect.Parameters["colorMapTexture"].SetValue((Microsoft.Xna.Framework.Graphics.Texture) srcTexture);
      spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, BrainGame.CurrentSampler, (DepthStencilState) null, (RasterizerState) null, this._effect);
      spriteBatch.Draw(srcTexture, destinationRectangle, Color.White);
      spriteBatch.End();
      this._screen.SpriteBatch.GraphicsDevice.SetRenderTarget((RenderTarget2D) null);
      BrainGame.Graphics.Viewport = BrainGame.Viewport;
      return (Texture2D) renderTarget;
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      this._elapsedTime += (float) gameTime.ElapsedGameTime.Milliseconds / 500f;
    }

    public Texture2D Draw(Texture2D text)
    {
      this._texture = text;
      return this.PerformWaterShade(this._texture, this._renderTarget, this._screen.SpriteBatch);
    }
  }
}
