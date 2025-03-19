
// Type: TwoBrainsGames.BrainEngine.UI.Screens.Effects.GaussianBlur
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TwoBrainsGames.BrainEngine.Resources;


namespace TwoBrainsGames.BrainEngine.UI.Screens.Effects
{
  public class GaussianBlur : PostProcessor
  {
    private const string GAUSSIAN_BLUR_RES = "effects/gaussian-blur";
    private const int RADIUS = 7;
    private int _radius;
    private float _amount;
    private float _sigma;
    private float[] _kernel;
    private Vector2[] offsetsHoriz;
    private Vector2[] offsetsVert;
    private RenderTarget2D renderTarget1;
    private RenderTarget2D renderTarget2;
    private int renderTargetWidth;
    private int renderTargetHeight;

    public GaussianBlur(Screen screen)
      : base(screen)
    {
      this.ComputeKernel(7, 0.5f);
    }

    public GaussianBlur(Screen screen, Effect effect, int width, int height)
      : base(screen, effect, width, height)
    {
      this.ComputeKernel(7, 0.5f);
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this._effect = BrainGame.ResourceManager.Load<Effect>("effects/gaussian-blur", ResourceManager.ResourceManagerCacheType.Static);
      this.InitRenderTargets();
    }

    protected void InitRenderTargets()
    {
      this.renderTargetWidth = this._width / 2;
      this.renderTargetHeight = this._height / 2;
      this.renderTarget1 = new RenderTarget2D(this._screen.SpriteBatch.GraphicsDevice, this.renderTargetWidth, this.renderTargetHeight, false, this._screen.SpriteBatch.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.None);
      this.renderTarget2 = new RenderTarget2D(this._screen.SpriteBatch.GraphicsDevice, this.renderTargetWidth, this.renderTargetHeight, false, this._screen.SpriteBatch.GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.None);
      this.ComputeOffsets((float) this.renderTargetWidth, (float) this.renderTargetHeight);
    }

    public void ComputeKernel(int blurRadius, float blurAmount)
    {
      this._radius = blurRadius;
      this._amount = blurAmount;
      this._kernel = (float[]) null;
      this._kernel = new float[this._radius * 2 + 1];
      this._sigma = (float) this._radius / this._amount;
      float num1 = 2f * this._sigma * this._sigma;
      float num2 = (float) Math.Sqrt((double) num1 * Math.PI);
      float num3 = 0.0f;
      for (int index1 = -this._radius; index1 <= this._radius; ++index1)
      {
        float num4 = (float) (index1 * index1);
        int index2 = index1 + this._radius;
        this._kernel[index2] = (float) Math.Exp(-(double) num4 / (double) num1) / num2;
        num3 += this._kernel[index2];
      }
      for (int index = 0; index < this._kernel.Length; ++index)
        this._kernel[index] /= num3;
    }

    public void ComputeOffsets(float textureWidth, float textureHeight)
    {
      this.offsetsHoriz = (Vector2[]) null;
      this.offsetsHoriz = new Vector2[this._radius * 2 + 1];
      this.offsetsVert = (Vector2[]) null;
      this.offsetsVert = new Vector2[this._radius * 2 + 1];
      float num1 = 1f / textureWidth;
      float num2 = 1f / textureHeight;
      for (int index1 = -this._radius; index1 <= this._radius; ++index1)
      {
        int index2 = index1 + this._radius;
        this.offsetsHoriz[index2] = new Vector2((float) index1 * num1, 0.0f);
        this.offsetsVert[index2] = new Vector2(0.0f, (float) index1 * num2);
      }
    }

    public Texture2D PerformGaussianBlur(
      Texture2D srcTexture,
      RenderTarget2D renderTarget1,
      RenderTarget2D renderTarget2,
      SpriteBatch spriteBatch)
    {
      Rectangle destinationRectangle1 = new Rectangle(0, 0, renderTarget1.Width, renderTarget1.Height);
      Rectangle destinationRectangle2 = new Rectangle(0, 0, renderTarget2.Width, renderTarget2.Height);
      this._screen.SpriteBatch.GraphicsDevice.SetRenderTarget(renderTarget1);
      this._effect.CurrentTechnique = this._effect.Techniques[nameof (GaussianBlur)];
      this._effect.Parameters["weights"].SetValue(this._kernel);
      this._effect.Parameters["colorMapTexture"].SetValue((Microsoft.Xna.Framework.Graphics.Texture) srcTexture);
      this._effect.Parameters["offsets"].SetValue(this.offsetsHoriz);
      spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, BrainGame.CurrentSampler, (DepthStencilState) null, (RasterizerState) null, this._effect);
      if (srcTexture != null)
        spriteBatch.Draw(srcTexture, destinationRectangle1, Color.White);
      spriteBatch.End();
      this._screen.SpriteBatch.GraphicsDevice.SetRenderTarget(renderTarget2);
      Texture2D texture = (Texture2D) renderTarget1;
      this._effect.Parameters["colorMapTexture"].SetValue((Microsoft.Xna.Framework.Graphics.Texture) texture);
      this._effect.Parameters["offsets"].SetValue(this.offsetsVert);
      spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.Opaque, BrainGame.CurrentSampler, (DepthStencilState) null, (RasterizerState) null, this._effect);
      spriteBatch.Draw(texture, destinationRectangle2, Color.White);
      spriteBatch.End();
      this._screen.SpriteBatch.GraphicsDevice.SetRenderTarget((RenderTarget2D) null);
      BrainGame.Graphics.Viewport = BrainGame.Viewport;
      return (Texture2D) renderTarget2;
    }

    public override void Draw()
    {
      Texture2D texture = this.PerformGaussianBlur(this._texture, this.renderTarget1, this.renderTarget2, this._screen.SpriteBatch);
      Rectangle destinationRectangle = new Rectangle(0, 0, this._width, this._height);
      this._screen.SpriteBatch.GraphicsDevice.Clear(ClearOptions.Target, Color.CornflowerBlue, 0.0f, 0);
      this._screen.SpriteBatch.Begin(SpriteSortMode.Immediate, (BlendState) null, BrainGame.CurrentSampler, (DepthStencilState) null, (RasterizerState) null, (Effect) BrainGame.RenderEffect);
      this._screen.SpriteBatch.Draw(texture, destinationRectangle, Color.White);
      this._screen.SpriteBatch.End();
    }

    public Texture2D Draw(Texture2D text)
    {
      this._texture = text;
      Texture2D texture = this.PerformGaussianBlur(this._texture, this.renderTarget1, this.renderTarget2, this._screen.SpriteBatch);
      BrainGame.Graphics.Viewport = BrainGame.Viewport;
      this._screen.SpriteBatch.GraphicsDevice.Clear(ClearOptions.Target, Color.Black, 0.0f, 0);
      this._screen.SpriteBatch.Begin(SpriteSortMode.Immediate, (BlendState) null, BrainGame.CurrentSampler, (DepthStencilState) null, (RasterizerState) null, (Effect) BrainGame.RenderEffect);
      this._screen.SpriteBatch.Draw(texture, this._drawRect, Color.White);
      this._screen.SpriteBatch.End();
      return texture;
    }
  }
}
