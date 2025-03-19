
// Type: TwoBrainsGames.BrainEngine.UI.Screens.Effects.PostProcessor
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace TwoBrainsGames.BrainEngine.UI.Screens.Effects
{
  public class PostProcessor : IBrainComponent
  {
    protected Screen _screen;
    protected Texture2D _texture;
    protected Effect _effect;
    protected int _width;
    protected int _height;
    protected Rectangle _drawRect;
    public Rectangle Rectangle;

    public SpriteBatch SpriteBatch => this._screen.SpriteBatch;

    public Texture2D Texture
    {
      get => this._texture;
      set
      {
        this._texture = value;
        if (this._effect.Parameters["colorMapTexture"] == null)
          return;
        this._effect.Parameters["colorMapTexture"].SetValue((Microsoft.Xna.Framework.Graphics.Texture) value);
      }
    }

    public Effect Effect
    {
      get => this._effect;
      set => this._effect = value;
    }

    public PostProcessor(Screen screen)
    {
      this._screen = screen;
      this.InitializeEffect((Effect) null, this._screen.SpriteBatch.GraphicsDevice.Viewport.Width, this._screen.SpriteBatch.GraphicsDevice.Viewport.Height);
    }

    public PostProcessor(Screen screen, Effect effect, int width, int height)
    {
      this._screen = screen;
      this.InitializeEffect(effect, width, height);
    }

    public void InitializeEffect(Effect effect, int width, int height)
    {
      this._effect = effect;
      this._texture = new Texture2D(this._screen.SpriteBatch.GraphicsDevice, 1, 1);
      this._texture.SetData<Color>(new Color[1]
      {
        Color.White
      });
      this._width = width;
      this._height = height;
      this.Rectangle = new Rectangle(0, 0, width, height);
      this._drawRect = new Rectangle(0, 0, BrainGame.ScreenWidth, BrainGame.ScreenHeight);
    }

    public virtual void Draw()
    {
    }

    public virtual void Initialize()
    {
    }

    public virtual void LoadContent()
    {
    }

    public virtual void Update(BrainGameTime gameTime)
    {
    }

    public virtual void UnloadContent()
    {
    }
  }
}
