
// Type: TwoBrainsGames.BrainEngine.UI.Controls.UITextFontLabel
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.BrainEngine.UI.Controls
{
  public class UITextFontLabel : UILabel
  {
    private TextFont _font;

    public TextFont Font
    {
      get => this._font;
      set
      {
        this._font = value;
        this.CalculateSize();
      }
    }

    private Vector2[] TextLinePositions { get; set; }

    public float LineSpacing => this._font != null ? (float) this._font.LineHeight : 0.0f;

    public override Size Size
    {
      get => base.Size;
      set
      {
        base.Size = value;
        if (this.Autosize)
          return;
        this.CalculateSize();
      }
    }

    public UITextFontLabel(UIScreen screenOwner)
      : base(screenOwner)
    {
    }

    public UITextFontLabel(UIScreen screenOwner, TextFont font)
      : this(screenOwner, font, "")
    {
    }

    public UITextFontLabel(UIScreen screenOwner, TextFont font, string text)
      : base(screenOwner)
    {
      this.Font = font;
      this.Text = text;
      this.Autosize = true;
    }

    public override void Draw()
    {
      if (this.Font == null)
        return;
      for (int index = 0; index < this.TextLines.Length; ++index)
      {
        if (this.DropShadow)
          this.Font.DrawString(this.SpriteBatch, this.TextLines[index], this.AbsolutePositionInPixels + this.TextLinePositions[index], this.Scale, this.ShadowColor);
        this.Font.DrawString(this.SpriteBatch, this.TextLines[index], this.AbsolutePositionInPixels + this.TextLinePositions[index], this.Scale, this.BlendColor);
      }
    }

    protected override void CalculateSize()
    {
      if (this.Autosize)
        this.Size = new Size(this.MeasureWidth(), this.MeasureHeight());
      if (this.TextLines == null)
        return;
      float y = 0.0f;
      switch (this.VerticalAligment)
      {
        case VerticalTextAligment.Bottom:
          y = this.ScreenUnitToPixelsY(this.Size.Height) - this.SizeInPixels.Height;
          break;
        case VerticalTextAligment.Center:
          y = (float) ((double) this.ScreenUnitToPixelsY(this.Size.Height) / 2.0 - (double) this.SizeInPixels.Height / 2.0);
          break;
      }
      this.TextLinePositions = new Vector2[this.LineCount];
      for (int index = 0; index < this.TextLines.Length; ++index)
      {
        float x = 0.0f;
        switch (this.HorizontalAligment)
        {
          case HorizontalTextAligment.Right:
            x = this.SizeInPixels.Width - this.Font.MeasureString(this.TextLines[index], this.Scale);
            break;
          case HorizontalTextAligment.Center:
            x = (float) ((double) this.SizeInPixels.Width / 2.0 - (double) this.Font.MeasureString(this.TextLines[index], this.Scale) / 2.0);
            break;
        }
        this.TextLinePositions[index] = new Vector2(x, y);
        y += this.LineSpacing;
      }
    }

    private float MeasureWidth()
    {
      if (this.Font == null)
        return 0.0f;
      float val = 0.0f;
      foreach (string textLine in this.TextLines)
      {
        float num = this.Font.MeasureString(textLine, this.Scale);
        if ((double) num > (double) val)
          val = num;
      }
      return this.PixelsToScreenUnitsX(val);
    }

    private float MeasureHeight()
    {
      if (this.Font == null)
        return 0.0f;
      float val = 0.0f;
      for (int index = 0; index < this.TextLines.Length; ++index)
      {
        val += this.Font.MeasureStringHeight(this.TextLines[index], this.Scale);
        if (index != this.TextLines.Length - 1)
          val += this.LineSpacing;
      }
      return this.PixelsToScreenUnitsY(val);
    }
  }
}
