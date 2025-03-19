
// Type: TwoBrainsGames.BrainEngine.UI.Controls.UIMenuItem
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.BrainEngine.UI.Controls
{
  public class UIMenuItem : UIControl
  {
    public UIControl.UIEvent OnPress;
    private Sprite _sprite;
    private int _hotSpotBBIndex;

    public int Identifier { get; set; }

    protected UIImage BackgroundImage { get; set; }

    public UIMenu MenuOwner => (UIMenu) this.Parent;

    public UILabel Label { get; private set; }

    public MenuItemStyle Style { get; set; }

    public Sprite Image
    {
      get => this._sprite;
      set
      {
        this._sprite = value;
        if (this.BackgroundImage == null && this._sprite != null)
        {
          this.BackgroundImage = new UIImage(this.ScreenOwner, this._sprite);
          this.BackgroundImage.ParentAlignment = AlignModes.HorizontalyVertically;
          this.Controls.InsertAt(0, (UIControl) this.BackgroundImage);
          this.Resize();
        }
        else
          this.BackgroundImage.Sprite = (Sprite) null;
      }
    }

    public new Color BlendColor
    {
      get => base.BlendColor;
      set
      {
        base.BlendColor = value;
        if (this.Label == null)
          return;
        this.Label.BlendColor = value;
      }
    }

    public Vector2 TextScale
    {
      get => this.Label == null ? new Vector2(1f, 1f) : this.Label.Scale;
      set
      {
        if (this.Label == null)
          return;
        this.Label.Scale = value;
        this.Resize();
      }
    }

    public new bool Enabled
    {
      get => base.Enabled;
      set
      {
        base.Enabled = value;
        this.MenuOwner.RepositionItems();
      }
    }

    public override string TextResourceId
    {
      get => this.Label.TextResourceId;
      set => this.Label.TextResourceId = value;
    }

    public override string Text
    {
      get => this.Label.Text;
      set => this.Label.Text = value;
    }

    public int HotSpotBBIndex
    {
      get => this._hotSpotBBIndex;
      set
      {
        this._hotSpotBBIndex = value;
        this.Resize();
      }
    }

    public override BoundingSquare BoundingBox
    {
      get
      {
        return this.HotSpotBBIndex == -1 || this.BackgroundImage == null ? new BoundingSquare(this.AbsolutePositionInPixels, this.SizeInPixelsScaled.Width, this.SizeInPixelsScaled.Height) : new BoundingSquare(this.AbsolutePositionInPixels + new Vector2(this.BackgroundImage.Sprite.BoundingBoxes[this.HotSpotBBIndex].Left, this.BackgroundImage.Sprite.BoundingBoxes[this.HotSpotBBIndex].Top), this.BackgroundImage.Sprite.BoundingBoxes[this.HotSpotBBIndex].Width * this.Scale.X, this.BackgroundImage.Sprite.BoundingBoxes[this.HotSpotBBIndex].Height * this.Scale.Y);
      }
    }

    public UIMenuItem(UIScreen screenOwner, string textResourceId, SpriteFont spriteFont)
      : base(screenOwner)
    {
      this.Initialize(textResourceId, (UILabel) new UISpriteFontLabel(screenOwner, spriteFont));
    }

    public UIMenuItem(UIScreen screenOwner, string textResourceId, TextFont textFont)
      : base(screenOwner)
    {
      this.Initialize(textResourceId, (UILabel) new UITextFontLabel(screenOwner, textFont));
    }

    private void Initialize(string textResourceId, UILabel label)
    {
      this.HotSpotBBIndex = -1;
      this.Size = new Size(0.0f, 0.0f);
      this.Label = label;
      this.Label.ParentAlignment = AlignModes.HorizontalyVertically;
      this.Label.TextResourceId = textResourceId;
      this.Controls.Add((UIControl) this.Label);
      this.Style = MenuItemStyle.Text;
      this.Resize();
    }

    protected virtual void Resize()
    {
      float width = 0.0f;
      float height = 0.0f;
      if (this.Label != null)
      {
        if ((double) this.Label.Size.Width > (double) width)
          width = this.Label.Size.Width;
        if ((double) this.Label.Size.Height > (double) height)
          height = this.Label.Size.Height;
      }
      if (this.BackgroundImage != null)
      {
        if ((double) this.BackgroundImage.Size.Width > (double) width)
          width = this.BackgroundImage.Size.Width;
        if ((double) this.BackgroundImage.Size.Height > (double) height)
          height = this.BackgroundImage.Size.Height;
      }
      this.Size = new Size(width, height);
    }

    public override void Update(BrainGameTime gameTime)
    {
      if (this.DropShadow)
      {
        if (this.BackgroundImage != null)
        {
          this.BackgroundImage.DropShadow = true;
          this.Label.DropShadow = false;
        }
        else
          this.Label.DropShadow = true;
      }
      else
      {
        if (this.BackgroundImage != null)
          this.BackgroundImage.DropShadow = false;
        this.Label.DropShadow = false;
      }
    }
  }
}
