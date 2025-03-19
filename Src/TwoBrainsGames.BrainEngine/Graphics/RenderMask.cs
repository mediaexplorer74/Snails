
// Type: TwoBrainsGames.BrainEngine.Graphics.RenderMask
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace TwoBrainsGames.BrainEngine.Graphics
{
  public class RenderMask
  {
    private DepthStencilState _stencilBuffer;
    private Sprite _sprite;
    private AlphaTestEffect _alphaTestEffect;
    private DepthStencilState _stencilAlways;
    private SpriteBatch _spriteBatch;
    private RenderTarget2D _renderTarget;

    public Vector2 Position { get; set; }

    public DepthStencilState State => this._stencilBuffer;

    public void LoadContent()
    {
      this._stencilBuffer = new DepthStencilState();
      this._stencilBuffer.StencilFunction = CompareFunction.Equal;
      this._stencilBuffer.StencilPass = StencilOperation.Keep;
      this._stencilBuffer.ReferenceStencil = 0;
      this._stencilBuffer.DepthBufferEnable = false;
      this._stencilBuffer.StencilEnable = true;
      this._spriteBatch = new SpriteBatch(BrainGame.Graphics);
      this._alphaTestEffect = new AlphaTestEffect(BrainGame.Graphics);
      this._alphaTestEffect.VertexColorEnabled = true;
      this._alphaTestEffect.DiffuseColor = Color.White.ToVector3();
      this._alphaTestEffect.AlphaFunction = CompareFunction.NotEqual;
      this._alphaTestEffect.World = Matrix.Identity;
      this._alphaTestEffect.View = Matrix.Identity;
      this._alphaTestEffect.ReferenceAlpha = 0;
      this._alphaTestEffect.Projection = Matrix.CreateTranslation(-0.5f, -0.5f, 0.0f) * BrainGame.RenderEffect.Projection;
      this._stencilAlways = new DepthStencilState();
      this._stencilAlways.StencilEnable = true;
      this._stencilAlways.StencilFunction = CompareFunction.Always;
      this._stencilAlways.StencilPass = StencilOperation.Replace;
      this._stencilAlways.ReferenceStencil = 0;
      this._stencilAlways.DepthBufferEnable = false;
      this._renderTarget = new RenderTarget2D(BrainGame.Graphics, BrainGame.ScreenWidth, BrainGame.ScreenHeight, false, SurfaceFormat.Color, DepthFormat.Depth24Stencil8, 0, RenderTargetUsage.DiscardContents);
    }

    public void BeginDraw()
    {
      BrainGame.Graphics.SetRenderTarget(this._renderTarget);
      this._spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
    }

    public void EndDraw()
    {
      this._spriteBatch.End();
      BrainGame.Graphics.SetRenderTarget((RenderTarget2D) null);
      BrainGame.Graphics.Clear(ClearOptions.Stencil, new Color(0, 0, 0, 1), 0.0f, 0);
      this._spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, (SamplerState) null, this._stencilAlways, (RasterizerState) null, (Effect) this._alphaTestEffect);
      this._spriteBatch.Draw((Texture2D) this._renderTarget, Vector2.Zero, Color.White);
      this._spriteBatch.End();
    }

    public void DrawSprite(Sprite sprite, Vector2 position)
    {
      sprite.Draw(position, this._spriteBatch);
    }

    public static RenderMask CreateFromSprite(string resourceName)
    {
      RenderMask fromSprite = new RenderMask()
      {
        _sprite = BrainGame.ResourceManager.GetSpriteTemporary(resourceName),
        _alphaTestEffect = new AlphaTestEffect(BrainGame.Graphics)
      };
      fromSprite._alphaTestEffect.VertexColorEnabled = true;
      fromSprite._alphaTestEffect.DiffuseColor = Color.White.ToVector3();
      fromSprite._alphaTestEffect.AlphaFunction = CompareFunction.NotEqual;
      fromSprite._alphaTestEffect.World = Matrix.Identity;
      fromSprite._alphaTestEffect.View = Matrix.Identity;
      fromSprite._alphaTestEffect.ReferenceAlpha = 0;
      Matrix translation = Matrix.CreateTranslation(-0.5f, -0.5f, 0.0f);
      fromSprite._alphaTestEffect.Projection = translation * BrainGame.RenderEffect.Projection;
      fromSprite._stencilAlways = new DepthStencilState();
      fromSprite._stencilAlways.StencilEnable = true;
      fromSprite._stencilAlways.StencilFunction = CompareFunction.Always;
      fromSprite._stencilAlways.StencilPass = StencilOperation.Replace;
      fromSprite._stencilAlways.ReferenceStencil = 0;
      fromSprite._stencilAlways.DepthBufferEnable = false;
      return fromSprite;
    }

    public void Render()
    {
      BrainGame.Graphics.Clear(ClearOptions.Stencil, new Color(0, 0, 0, 1), 0.0f, 1);
      this._spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.Opaque, BrainGame.CurrentSampler, this._stencilAlways, (RasterizerState) null, (Effect) this._alphaTestEffect);
      this._sprite.Draw(this.Position, this._spriteBatch);
      this._spriteBatch.End();
    }
  }
}
