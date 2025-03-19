
// Type: TwoBrainsGames.BrainEngine.UI.Controls.UIButton
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.BrainEngine.UI.Controls
{
  public class UIButton : UIControl
  {
    private Sprite _enabledSprite;
    private Sprite _disabledSprite;
    public ImageSizeMode _sizeMode;

    public event UIControl.UIEvent OnPress;

    public event UIControl.UIEvent OnBeforePress;

    public event UIControl.UIEvent OnDoublePress;

    public event UIControl.UIEvent OnBeforeDoublePress;

    public new Vector2 Scale
    {
      get => base.Scale;
      set => base.Scale = value;
    }

    public ImageSizeMode SizeMode
    {
      get => this._sizeMode;
      set
      {
        this._sizeMode = value;
        this.ComputeSize();
      }
    }

    public UIImage Image { get; set; }

    private UITextFontLabel _lblCaption { get; set; }

    public TransformEffectBase PressEffect { get; set; }

    public Sprite Sprite
    {
      get => this._enabledSprite;
      set
      {
        this._enabledSprite = value;
        if (this.Enabled)
          this.Image.Sprite = this._enabledSprite;
        this.ComputeSize();
      }
    }

    public Sprite DisabledSprite
    {
      get => this._disabledSprite == null ? this._enabledSprite : this._disabledSprite;
      set
      {
        this._disabledSprite = value;
        if (this.Enabled)
          return;
        this.Image.Sprite = this._disabledSprite;
        this.ComputeSize();
      }
    }

    public new int ControllerActionCode
    {
      get => base.ControllerActionCode;
      set
      {
        base.ControllerActionCode = value;
        this.Image.ControllerActionCode = value;
      }
    }

    public new bool Enabled
    {
      get => base.Enabled;
      set
      {
        base.Enabled = value;
        if (this.Enabled)
          this.Image.Sprite = this.Sprite;
        else
          this.Image.Sprite = this.DisabledSprite;
      }
    }

    public new ITransformEffect Effect
    {
      set => this.Image.Effect = value;
      get => this.Image.Effect;
    }

    public string ImageResource
    {
      set => this.Sprite = BrainGame.ResourceManager.GetSpriteStatic(value);
    }

    public Sample PressSound { get; set; }

    public int CurrentFrame
    {
      get => this.Image.CurrentFrame;
      set => this.Image.CurrentFrame = value;
    }

    public int FrameCount => this.Image.FrameCount;

    public bool AnimateImage
    {
      get => this.Image.Animate;
      set => this.Image.Animate = value;
    }

    public Vector2 ImagePosition
    {
      get => this.Image.Position;
      set => this.Image.Position = value;
    }

    public UIButton(UIScreen screenOwner)
      : this(screenOwner, (string) null)
    {
    }

    public UIButton(UIScreen screenOwner, string spriteResource)
      : base(screenOwner)
    {
      this.Image = new UIImage(screenOwner, spriteResource);
      this.Image.UseHotSpot = true;
      this.Controls.Add((UIControl) this.Image);
      this.OnAccept += new UIControl.UIEvent(this.Image_OnAccept);
      this.OnAcceptBegin += new UIControl.UIEvent(this.UIButton_OnAcceptBegin);
      this.OnControllerAction += new UIControl.UIEvent(this.Image_OnAccept);
      this.OnDoubleTapAccept += new UIControl.UIEvent(this.UIButton_OnDoubleTapAccept);
      this.OnDoubleTapAcceptBegin += new UIControl.UIEvent(this.UIButton_OnDoubleTapAcceptBegin);
      this.SizeMode = ImageSizeMode.Autosize;
    }

    private void UIButton_OnAcceptBegin(IUIControl sender)
    {
      if (this.OnBeforePress == null)
        return;
      this.OnBeforePress((IUIControl) this);
    }

    private void UIButton_OnDoubleTapAcceptBegin(IUIControl sender)
    {
      if (this.OnBeforeDoublePress == null)
        return;
      this.OnBeforeDoublePress((IUIControl) this);
    }

    private void ComputeSize()
    {
      if (this.Image.Sprite == null)
        return;
      switch (this.SizeMode)
      {
        case ImageSizeMode.Center:
          this.Image.ParentAlignment = AlignModes.HorizontalyVertically;
          break;
        case ImageSizeMode.Autosize:
          this.Size = this.Image.Size;
          break;
        case ImageSizeMode.HorizontalCenter:
          this.Image.ParentAlignment = AlignModes.Horizontaly;
          break;
      }
    }

    private void Image_OnAccept(IUIControl sender)
    {
      if (this.PressEffect == null)
      {
        if (this.PressSound != null)
          this.PressSound.Play();
        if (this.OnPress == null)
          return;
        this.OnPress((IUIControl) this);
      }
      else
      {
        if (this.PressSound != null)
          this.PressSound.Play();
        this.Busy = true;
        this.PressEffect.Reset();
        this.PressEffect.OnEnd = new TransformEffectBase.OnEndEvent(this.PressEffect_OnEnd);
        this.EffectsBlender.Add((ITransformEffect) this.PressEffect);
      }
    }

    private void UIButton_OnDoubleTapAccept(IUIControl sender)
    {
      if (this.PressEffect == null)
      {
        if (this.PressSound != null)
          this.PressSound.Play();
        if (this.OnDoublePress == null)
          return;
        this.OnDoublePress((IUIControl) this);
      }
      else
      {
        if (this.PressSound != null)
          this.PressSound.Play();
        this.Busy = true;
        this.PressEffect.Reset();
        this.PressEffect.OnEnd = new TransformEffectBase.OnEndEvent(this.PressEffect_OnEnd);
        this.EffectsBlender.Add((ITransformEffect) this.PressEffect);
      }
    }

    private void PressEffect_OnEnd(object param)
    {
      if (this.OnPress != null)
        this.OnPress((IUIControl) this);
      this.Busy = false;
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      this.ComputeSize();
    }

    public void SetText(string text, string fontResourceName)
    {
      TextFont textFont = BrainGame.ResourceManager.Load<TextFont>(fontResourceName, ResourceManager.ResourceManagerCacheType.Static);
      if (this._lblCaption == null)
      {
        this._lblCaption = new UITextFontLabel(this.ScreenOwner);
        this._lblCaption.ParentAlignment = AlignModes.HorizontalyVertically;
        this.Controls.Add((UIControl) this._lblCaption);
      }
      this._lblCaption.Font = textFont;
      this._lblCaption.Text = text;
    }
  }
}
