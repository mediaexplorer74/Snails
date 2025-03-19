
// Type: TwoBrainsGames.BrainEngine.UI.Screens.Transitions.GrayoutTransition
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace TwoBrainsGames.BrainEngine.UI.Screens.Transitions
{
  public class GrayoutTransition : Transition
  {
    private const int TRANSITION_TIME = 200;
    private Texture2D _texture;
    private float _time;
    public bool Ended;
    private UIScreen _screen;
    private float _alpha;
    private bool _fadingOut;
    private Rectangle _drawRect;
    private int _iterations;
    private int _strenghCounter;
    private double _iterationTime;

    protected RenderTarget2D RenderTarget => this._texture as RenderTarget2D;

    public Texture2D TextureToBlur
    {
      get => this._texture;
      set => this._texture = value;
    }

    public Color BackgroundColor { get; set; }

    public bool IsTransitionOut { get; private set; }

    protected UIScreen ScreenOwner => this._screen;

    public GrayoutTransition(UIScreen screen)
      : this(screen, false, 1, 10.0)
    {
    }

    public GrayoutTransition(UIScreen screen, bool isGray, int iterations, double iterationTime)
    {
      this._screen = screen;
      this._drawRect = new Rectangle(0, 0, BrainGame.ScreenWidth, BrainGame.ScreenHeight);
      this._iterations = iterations;
      this._iterationTime = iterationTime;
      this.Reset();
      this.BackgroundColor = new Color(0, 0, 0, 200);
    }

    public override void Reset()
    {
      base.Reset();
      this._alpha = 1f;
      this.Ended = false;
      this._time = 0.0f;
      this._fadingOut = false;
      this._strenghCounter = 0;
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      if (this.Ended)
      {
        if (!this._fadingOut)
          return;
        this._alpha -= (float) (0.0099999997764825821 * gameTime.ElapsedRealTime.TotalMilliseconds);
        if ((double) this._alpha >= 0.0)
          return;
        this._fadingOut = false;
        this.IsTransitionOut = true;
        this.InvokeTransitonEnded();
      }
      else
      {
        this._time += (float) gameTime.ElapsedRealTime.TotalMilliseconds;
        if ((double) this._time <= this._iterationTime)
          return;
        ++this._strenghCounter;
        this._time = 0.0f;
        if (this._strenghCounter <= this._iterations)
          return;
        this.Ended = true;
        this.IsTransitionOut = false;
        this.InvokeTransitonEnded();
      }
    }

    public override void Draw()
    {
      base.Draw();
      BrainGame.Graphics.Viewport = BrainGame.Viewport;
      this._screen.SpriteBatch.Begin(SpriteSortMode.Immediate, (BlendState) null, BrainGame.CurrentSampler, (DepthStencilState) null, (RasterizerState) null, (Effect) BrainGame.RenderEffect);
      this._screen.SpriteBatch.Draw(this._texture, this._drawRect, new Color(this._alpha, this._alpha, this._alpha, this._alpha));
      this._screen.SpriteBatch.End();
      if (!(this.BackgroundColor != Color.Transparent) || this.BackgroundColor.A == byte.MaxValue)
        return;
      this._screen.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, (DepthStencilState) null, (RasterizerState) null, (Effect) BrainGame.RenderEffect);
      this._screen.SpriteBatch.Draw(UIScreen.ClearTexture, this._drawRect, this.BackgroundColor);
      this._screen.SpriteBatch.End();
    }

    public virtual void PerformBlur()
    {
      this.Reset();
      this.Ended = true;
    }

    public override void TransitionOut()
    {
      this._fadingOut = true;
      this._alpha = 1f;
    }
  }
}
