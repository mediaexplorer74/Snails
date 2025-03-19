
// Type: TwoBrainsGames.BrainEngine.UI.Screens.Transitions.FadeInTransition
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace TwoBrainsGames.BrainEngine.UI.Screens.Transitions
{
  public class FadeInTransition : Transition
  {
    private float _alpha;
    private float _alphaMin;
    private float _alphaMax = 1f;
    private float _speed;
    private Color _fadeColor;

    public float AlphaMin
    {
      get => this._alphaMin;
      set => this._alphaMin = value;
    }

    public float AlphaMax
    {
      get => this._alphaMax;
      set => this._alphaMax = value;
    }

    public FadeInTransition(float fadeSpeed)
      : this(Color.Black, fadeSpeed)
    {
    }

    public FadeInTransition(Color fadeColor, float fadeSpeed)
    {
      this._speed = fadeSpeed;
      this._fadeColor = fadeColor;
    }

    public override void Initialize()
    {
      base.Initialize();
      this._alpha = this._alphaMax;
    }

    public override void Update(BrainGameTime gameTime)
    {
      if (this._ended)
        return;
      this._alpha -= this._speed * (float) gameTime.ElapsedRealTime.TotalMilliseconds;
      if ((double) this._alpha >= (double) this._alphaMin)
        return;
      this._alpha = this._alphaMin;
      this._ended = true;
    }

    public override void Draw()
    {
      Color color = new Color((float) this._fadeColor.R, (float) this._fadeColor.G, (float) this._fadeColor.B, this._alpha);
      BrainGame.SpriteBatch.Begin(SpriteSortMode.Immediate, (BlendState) null, BrainGame.CurrentSampler, (DepthStencilState) null, (RasterizerState) null, (Effect) BrainGame.RenderEffect);
      BrainGame.DrawRectangleFilled(BrainGame.SpriteBatch, new Rectangle(0, 0, BrainGame.ScreenWidth, BrainGame.ScreenHeight), color);
      BrainGame.SpriteBatch.End();
    }
  }
}
