
// Type: TwoBrainsGames.BrainEngine.UI.Controls.UIImage
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.BrainEngine.UI.Controls
{
  public class UIImage : UIControl
  {
    private SpriteAnimation _animation;
    private ImageSizeMode _sizeMode;

    public event UIImage.LastFrameHandler OnLastFrame;

    public string ResourceName { get; set; }

    public bool Animate { get; set; }

    public Vector2 Offset { get; set; }

    public int FrameCount => this._animation.FrameCount;

    public int CurrentFrame
    {
      get => this._animation.CurrentFrame;
      set => this._animation.CurrentFrame = value;
    }

    public override Vector2 AbsolutePositionInPixels
    {
      get
      {
        Vector2 positionInPixels = base.AbsolutePositionInPixels;
        if (this._animation.Sprite != null)
          positionInPixels -= this._animation.Sprite.Offset;
        return positionInPixels;
      }
    }

    public override BoundingSquare BoundingBox
    {
      get
      {
        return this._animation.Sprite == null ? new BoundingSquare(this.AbsolutePositionInPixels, this.SizeInPixels.Width, this.SizeInPixels.Height) : new BoundingSquare(this.AbsolutePositionInPixels - this.ScreenUnitToPixels(this._animation.Sprite.Offset), this.SizeInPixels.Width, this.SizeInPixels.Height);
      }
    }

    public ImageSizeMode SizeMode
    {
      get => this._sizeMode;
      set
      {
        this._sizeMode = value;
        this.CalculateSize();
      }
    }

    public Sprite Sprite
    {
      set
      {
        if (this._animation == null)
          this._animation = new SpriteAnimation(value);
        else
          this._animation.Sprite = value;
        this.CalculateSize();
      }
      get => this._animation == null ? (Sprite) null : this._animation.Sprite;
    }

    private Vector2 SpriteOffset
    {
      get
      {
        return this._animation == null || this._animation.Sprite == null ? Vector2.Zero : this._animation.Sprite.Offset * this.Scale;
      }
    }

    internal bool UseHotSpot { get; set; }

    public UIImage(UIScreen screenOwner)
      : this(screenOwner, (Sprite) null)
    {
      this.UseHotSpot = false;
    }

    public UIImage(UIScreen screenOwner, string resourceName)
      : this(screenOwner, resourceName, "__STATIC__")
    {
    }

    public UIImage(UIScreen screenOwner, string resourceName, string resourceManagerId)
      : this(screenOwner)
    {
      if (resourceName == null)
        this.Initialize((Sprite) null);
      else
        this.Initialize(BrainGame.ResourceManager.GetSprite(resourceName, resourceManagerId));
    }

    public UIImage(UIScreen screenOwner, Sprite sprite)
      : base(screenOwner)
    {
      this.Initialize(sprite);
    }

    private void Initialize(Sprite sprite)
    {
      this.SizeMode = ImageSizeMode.Autosize;
      this.Sprite = sprite;
      this.AcceptControllerInput = false;
      this._animation.OnLastFrame += new SpriteAnimation.LastFrameHandler(this._animation_OnLastFrame);
      this.Animate = true;
    }

    private void _animation_OnLastFrame()
    {
      if (this.OnLastFrame == null)
        return;
      this.OnLastFrame();
    }

    internal override void ParentChanged()
    {
      base.ParentChanged();
      this.CalculateSize();
    }

    public override void OnResize() => this.CalculateSize();

    public override void Load()
    {
      if (this.ResourceName == null)
        return;
      this.Sprite = BrainGame.ResourceManager.GetSpriteTemporary(BrainPath.GetDirectoryName(this.ResourceName), BrainPath.GetFileName(this.ResourceName));
    }

    public override void Update(BrainGameTime gameTime)
    {
      if (this._animation == null || this._animation.Sprite == null || !this.Animate)
        return;
      this._animation.Update(gameTime);
    }

    public override void Draw()
    {
      if (this._animation == null || this._animation.Sprite == null)
        return;
      Vector2 vector2 = Vector2.Zero;
      if (this.UseHotSpot)
        vector2 = new Vector2((this._animation.Sprite.BoundingBox.Left + this._animation.Sprite.OffsetX) * this.Scale.X, (this._animation.Sprite.BoundingBox.Top + this._animation.Sprite.OffsetY) * this.Scale.Y);
      if (this.DropShadow)
        this.Draw(base.AbsolutePositionInPixels + this.ShadowDistance - vector2 - this.Offset, this.SpriteOffset, this.ShadowColor);
      this.Draw(base.AbsolutePositionInPixels - vector2 - this.Offset, this.SpriteOffset, this.BlendColor);
    }

    private void Draw(Vector2 position, Vector2 offset, Color color)
    {
      if (this.SizeMode != ImageSizeMode.Stretch)
      {
        if (offset == Vector2.Zero)
          this._animation.Draw(position, this.Rotation, color, this.Scale, Vector2.Zero, this.SpriteBatch);
        else
          this._animation.Draw(position, this.Rotation, color, this.Scale, offset, this.SpriteBatch);
      }
      else
        this._animation.Draw(this.ClientRectInPixels, color, this.SpriteBatch);
    }

    private void CalculateSize()
    {
      switch (this.SizeMode)
      {
        case ImageSizeMode.Stretch:
          if (this.Parent == null)
            break;
          this.Size = this.Parent.Size;
          break;
        case ImageSizeMode.Autosize:
          if (this.Sprite == null)
            break;
          if (this.UseHotSpot)
          {
            this.Size = this.PixelsToScreenUnits(new Size(this.Sprite.BoundingBoxes[0].Width, this.Sprite.BoundingBoxes[0].Height));
            break;
          }
          this.Size = this.PixelsToScreenUnits(new Size((float) this.Sprite.Frames[0].Width, (float) this.Sprite.Frames[0].Height));
          break;
      }
    }

    public delegate void LastFrameHandler();
  }
}
