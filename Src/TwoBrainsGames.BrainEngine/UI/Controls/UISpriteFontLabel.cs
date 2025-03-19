
// Type: TwoBrainsGames.BrainEngine.UI.Controls.UISpriteFontLabel
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.BrainEngine.UI.Controls
{
  public class UISpriteFontLabel : UILabel
  {
    public SpriteFont Font { get; set; }

    public UISpriteFontLabel(UIScreen screenOwner)
      : this(screenOwner, (SpriteFont) null, "")
    {
    }

    public UISpriteFontLabel(UIScreen screenOwner, SpriteFont font)
      : this(screenOwner, font, "")
    {
    }

    public UISpriteFontLabel(UIScreen screenOwner, SpriteFont font, string text)
      : base(screenOwner)
    {
      this.Initialize(font, text);
    }

    public UISpriteFontLabel(UIScreen screenOwner, string fontResourceName, string text)
      : base(screenOwner)
    {
      this.Initialize(BrainGame.ResourceManager.Load<SpriteFont>(fontResourceName), text);
    }

    private void Initialize(SpriteFont font, string text)
    {
      this.Autosize = true;
      this.Font = font;
      this.Text = text;
      this.CalculateSize();
    }

    public override void Draw()
    {
      if (this.Font == null)
        return;
      Vector2 position = this.AbsolutePositionInPixels;
      position = new Vector2((float) (int) position.X, (float) (int) position.Y);
      if (this.DropShadow)
        this.SpriteBatch.DrawString(this.Font, this.Text, position + this.ShadowDistance, this.ShadowColor, 0.0f, Vector2.Zero, this.Scale, SpriteEffects.None, 1f);
      this.SpriteBatch.DrawString(this.Font, this.Text, position, this.BlendColor, 0.0f, Vector2.Zero, this.Scale, SpriteEffects.None, 1f);
    }

    protected override void CalculateSize()
    {
      if (this.Font == null || this.Text == null)
        return;
      Vector2 vector2 = this.Font.MeasureString(this.Text);
      this.Size = this.PixelsToScreenUnits(new Size((float) (int) vector2.X, (float) (int) vector2.Y));
    }
  }
}
