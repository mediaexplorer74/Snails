
// Type: TwoBrainsGames.BrainEngine.UI.Screens.Transitions.FadeOutTransition
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace TwoBrainsGames.BrainEngine.UI.Screens.Transitions
{
  public class FadeOutTransition : Transition
  {
    private float _alpha;
    private float _alphaMin;
    private float _alphaMax = 1f;
    private float _speed;
    private Color _color;

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

    public FadeOutTransition(float fadeSpeed)
      : this(fadeSpeed, Color.Black)
    {
    }

    public FadeOutTransition(float fadeSpeed, Color color)
    {
      this._speed = fadeSpeed;
      this._color = color;
    }

    public override void Initialize()
    {
      base.Initialize();
      this._alpha = this._alphaMin;
    }

    public override void Update(BrainGameTime gameTime)
    {
      if (this._ended)
        return;
      this._alpha += this._speed * (float) gameTime.ElapsedRealTime.TotalMilliseconds;
      if ((double) this._alpha <= (double) this._alphaMax)
        return;
      this._alpha = this._alphaMax;
      this._ended = true;
    }

    public override void Draw()
    {
      Color color = new Color((float) this._color.R, (float) this._color.G, (float) this._color.B, this._alpha);
      BrainGame.SpriteBatch.Begin(SpriteSortMode.Immediate, (BlendState) null, BrainGame.CurrentSampler, (DepthStencilState) null, (RasterizerState) null, (Effect) BrainGame.RenderEffect);
      BrainGame.DrawRectangleFilled(BrainGame.SpriteBatch, new Rectangle(0, 0, BrainGame.ScreenWidth, BrainGame.ScreenHeight), color);
      BrainGame.SpriteBatch.End();
    }
  }
}
