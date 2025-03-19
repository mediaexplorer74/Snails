
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UISnailsButton
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Effects;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UISnailsButton : UIControl
  {
    private static Vector2 DEFAULT_SCALE = new Vector2(0.85f, 0.85f);
    private UIPanel _pnlContainer;
    private UIButton _btnButton;
    private UITextFontLabel _lblCaption1;
    private UITextFontLabel _lblCaption2;
    private UIImage _image;
    private Sample _focusSound;
    private Sample _shownSound;
    private UISnailsButton.ButtonLabelType _labelType;
    private UISnailsButton.ButtonActionType _buttonAction;

    public event UIControl.UIEvent OnPress;

    public event UIControl.UIEvent OnClickBegin;

    private UISnailsButton.ButtonSizeType SizeType { get; set; }

    private UISnailsButton.ButtonLabelType LabelType
    {
      get => this._labelType;
      set
      {
        this._labelType = value;
        switch (this._labelType)
        {
          case UISnailsButton.ButtonLabelType.Text:
            this._lblCaption1.Visible = true;
            this._lblCaption1.Visible = true;
            this._image.Visible = false;
            this.UpdateLabels();
            break;
          case UISnailsButton.ButtonLabelType.Image:
            this._lblCaption1.Visible = false;
            this._lblCaption1.Visible = false;
            this._image.Visible = true;
            break;
        }
      }
    }

    public UISnailsButton.ButtonActionType ButtonAction
    {
      get => this._buttonAction;
      set
      {
        string spriteResourceName = (string) null;
        this._image.Sprite = (Sprite) null;
        this._buttonAction = value;
        switch (this._buttonAction)
        {
          case UISnailsButton.ButtonActionType.Back:
            spriteResourceName = "spriteset/button-icons/BackIcon";
            break;
          case UISnailsButton.ButtonActionType.Next:
            spriteResourceName = "spriteset/button-icons/NextIcon";
            break;
          case UISnailsButton.ButtonActionType.Start:
            spriteResourceName = "spriteset/button-icons/StartIcon";
            break;
          case UISnailsButton.ButtonActionType.Previous:
            spriteResourceName = "spriteset/button-icons/PreviousIcon";
            break;
          case UISnailsButton.ButtonActionType.MainMenu:
            spriteResourceName = "spriteset/button-icons/MainMenuIcon";
            break;
          case UISnailsButton.ButtonActionType.StageSelection:
            spriteResourceName = "spriteset/button-icons/StageSelIcon";
            break;
          case UISnailsButton.ButtonActionType.Retry:
            spriteResourceName = "spriteset/button-icons/RetryIcon";
            break;
        }
        if (spriteResourceName == null)
          return;
        this._image.Sprite = BrainGame.ResourceManager.GetSpriteStatic(spriteResourceName);
      }
    }

    public new int ControllerActionCode
    {
      get => base.ControllerActionCode;
      set
      {
        base.ControllerActionCode = value;
        this._btnButton.ControllerActionCode = value;
      }
    }

    public override string Text
    {
      get => base.Text;
      set
      {
        base.Text = value;
        this.UpdateLabels();
      }
    }

    private Vector2 Caption1PositionSmall { get; set; }

    private Vector2 Caption2PositionSmall { get; set; }

    private Vector2 Caption1PositionMedium { get; set; }

    private Vector2 Caption2PositionMedium { get; set; }

    public override bool Visible
    {
      get => base.Visible;
      set
      {
        base.Visible = value;
        if (!value || this._pnlContainer == null)
          return;
        this._pnlContainer.Visible = true;
      }
    }

    public UISnailsButton(
      UIScreen screenOwner,
      string textResourceId,
      UISnailsButton.ButtonSizeType type,
      InputBase.InputActions action,
      UIControl.UIEvent pressCallback,
      bool useShowEffects)
      : base(screenOwner)
    {
      this.SizeType = type;
      this.OnEnter += new UIControl.UIEvent(this.UISnailsButton_OnEnter);
      this.OnLeave += new UIControl.UIEvent(this.UISnailsButton_OnLeave);
      this.OnScreenStart += new UIControl.UIEvent(this.UISnailsButton_OnScreenStart);
      this.OnInitializeFromContent += new UIControl.UIEvent(this.UISnailsButton_OnInitializeFromContent);
      this._pnlContainer = new UIPanel(screenOwner);
      this._pnlContainer.Name = nameof (_pnlContainer);
      this._pnlContainer.ParentAlignment = AlignModes.HorizontalyVertically;
      if (useShowEffects)
      {
        this._pnlContainer.ShowEffect = (TransformEffectBase) new SquashEffect(0.7f, 4f, 0.08f, this.BlendColor, UISnailsButton.DEFAULT_SCALE);
        this._pnlContainer.HideEffect = (TransformEffectBase) new PopOutEffect(new Vector2(1.3f, 1.3f), 6f);
      }
      this._pnlContainer.OnShow += new UIControl.UIEvent(this._pnlContainer_OnShow);
      this._pnlContainer.OnHide += new UIControl.UIEvent(this._pnlContainer_OnHide);
      this.Controls.Add((UIControl) this._pnlContainer);
      this._btnButton = new UIButton(screenOwner);
      this._btnButton.SizeMode = ImageSizeMode.Center;
      this._btnButton.PressEffect = (TransformEffectBase) new ColorEffect(Color.White, Color.Gray, 0.4f, true, Color.White, 130.0);
      this._btnButton.OnPress += new UIControl.UIEvent(this._btnButton_OnPress);
      this._btnButton.ShowEffect = (TransformEffectBase) new SquashEffect(0.85f, 4f, 0.03f, this.BlendColor, this.Scale);
      this._btnButton.ParentAlignment = AlignModes.HorizontalyVertically;
      this._btnButton.OnShow += new UIControl.UIEvent(this._btnButton_OnShow);
      this._btnButton.OnShowBegin += new UIControl.UIEvent(this._btnButton_OnShowBegin);
      this._btnButton.OnAccept += new UIControl.UIEvent(this._btnButton_OnAccept);
      this._btnButton.PressSound = BrainGame.ResourceManager.GetSampleStatic("sfx/menu-item-selected");
      this._pnlContainer.Controls.Add((UIControl) this._btnButton);
      switch (type)
      {
        case UISnailsButton.ButtonSizeType.Small:
          this._btnButton.ImageResource = "spriteset/boards/ButtonSmall";
          break;
        case UISnailsButton.ButtonSizeType.Medium:
          this._btnButton.ImageResource = "spriteset/boards/ButtonMedium";
          break;
      }
      Vector2 screenUnits = this.PixelsToScreenUnits(new Vector2((float) this._btnButton.Sprite.Width, (float) this._btnButton.Sprite.Height));
      this._btnButton.Size = new Size(screenUnits.X, screenUnits.Y);
      this._lblCaption1 = new UITextFontLabel(this.ScreenOwner);
      this._lblCaption1.ParentAlignment = AlignModes.HorizontalyVertically;
      this._lblCaption1.Visible = false;
      this._lblCaption1.BlendColor = Colors.ButtonsText;
      this._btnButton.Controls.Add((UIControl) this._lblCaption1);
      this._lblCaption2 = new UITextFontLabel(this.ScreenOwner);
      this._lblCaption2.ParentAlignment = AlignModes.HorizontalyVertically;
      this._lblCaption2.Visible = false;
      this._lblCaption2.BlendColor = Colors.ButtonsText;
      this._btnButton.Controls.Add((UIControl) this._lblCaption2);
      this._image = new UIImage(this.ScreenOwner);
      this._image.ParentAlignment = AlignModes.HorizontalyVertically;
      this._image.Visible = false;
      this._image.BlendColor = Colors.ButtonsText;
      this._image.BlendColor = Colors.ButtonsText;
      this._btnButton.Controls.Add((UIControl) this._image);
      this.TextResourceId = textResourceId;
      this.Size = this._btnButton.Size;
      this._pnlContainer.Size = this.Size;
      this._pnlContainer.Scale = UISnailsButton.DEFAULT_SCALE;
      this.ControllerActionCode = (int) action;
      if (pressCallback != null)
        this.OnPress += new UIControl.UIEvent(pressCallback.Invoke);
      this._focusSound = BrainGame.ResourceManager.GetSampleStatic("sfx/menu-item-focus");
      this._shownSound = BrainGame.ResourceManager.GetSampleStatic("sfx/menu-item-shown");
      this.OnAccept += new UIControl.UIEvent(this.UISnailsButton_OnAccept);
      this.UpdateLabels();
      if (!Game1.GameSettings.UseButtonIcons)
        return;
      this.LabelType = UISnailsButton.ButtonLabelType.Image;
    }

    public override void InitializeFromContent()
    {
      base.InitializeFromContent();
      this.InitializeFromContent(nameof (UISnailsButton));
    }

    private void UISnailsButton_OnInitializeFromContent(IUIControl sender)
    {
      this.Caption1PositionSmall = this.GetContentPropertyValue<Vector2>("caption1PositionSmall", this.Caption1PositionSmall);
      this.Caption2PositionSmall = this.GetContentPropertyValue<Vector2>("caption2PositionSmall", this.Caption2PositionSmall);
      this.Caption1PositionMedium = this.GetContentPropertyValue<Vector2>("caption1PositionMedium", this.Caption1PositionMedium);
      this.Caption2PositionMedium = this.GetContentPropertyValue<Vector2>("caption2PositionMedium", this.Caption2PositionMedium);
      this.UpdateLabels();
    }

    private void UISnailsButton_OnAccept(IUIControl sender)
    {
      if (this.OnClickBegin == null)
        return;
      this.OnClickBegin((IUIControl) this);
    }

    private void _btnButton_OnAccept(IUIControl sender)
    {
      if (this.OnClickBegin == null)
        return;
      this.OnClickBegin((IUIControl) this);
    }

    private void _btnButton_OnShowBegin(IUIControl sender) => this._shownSound.Play();

    private void _btnButton_OnShow(IUIControl sender) => this.InvokeOnShow();

    private void _btnButton_OnPress(IUIControl sender)
    {
      if (this.OnPress == null)
        return;
      this.OnPress((IUIControl) this);
    }

    private void UISnailsButton_OnLeave(IUIControl sender) => this.Reset();

    private void UISnailsButton_OnEnter(IUIControl sender)
    {
      this._pnlContainer.Effect = (ITransformEffect) new ScaleEffect(UISnailsButton.DEFAULT_SCALE, 4f, new Vector2(1f, 1f), false);
      this._focusSound.Play();
      this._lblCaption1.BlendColor = Colors.ButtonsFocusText;
      this._lblCaption2.BlendColor = Colors.ButtonsFocusText;
      this._image.BlendColor = Colors.ButtonsFocusText;
    }

    private void UISnailsButton_OnScreenStart(IUIControl sender) => this.Reset();

    private void Reset()
    {
      this._pnlContainer.Effect = (ITransformEffect) null;
      this._pnlContainer.Scale = UISnailsButton.DEFAULT_SCALE;
      this._lblCaption1.BlendColor = Colors.ButtonsText;
      this._lblCaption2.BlendColor = Colors.ButtonsText;
      this._image.BlendColor = Colors.ButtonsText;
      if (this._image.Sprite != null)
        return;
      this.LabelType = UISnailsButton.ButtonLabelType.Text;
    }

    private void _pnlContainer_OnShow(IUIControl sender) => this.InvokeOnShow();

    private void _pnlContainer_OnHide(IUIControl sender)
    {
      this.InvokeOnHide();
      this.Visible = false;
    }

    private void UpdateLabels()
    {
      if (this._labelType != UISnailsButton.ButtonLabelType.Text)
      {
        this._lblCaption1.Visible = false;
        this._lblCaption2.Visible = false;
      }
      else
      {
        string[] strArray = this.Text.Split('|');
        if (strArray.Length >= 2)
        {
          this._lblCaption1.Visible = true;
          this._lblCaption1.Text = strArray[0];
          this._lblCaption1.ParentAlignment = AlignModes.Horizontaly;
          this._lblCaption1.Position = new Vector2(0.0f, 150f);
          this._lblCaption1.Font = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-medium", ResourceManager.ResourceManagerCacheType.Static);
          this._lblCaption2.Visible = true;
          this._lblCaption2.Text = strArray[1];
          this._lblCaption2.ParentAlignment = AlignModes.Horizontaly;
          this._lblCaption2.Position = new Vector2(0.0f, 550f);
          this._lblCaption2.Font = this._lblCaption1.Font;
          switch (this.SizeType)
          {
            case UISnailsButton.ButtonSizeType.Small:
              this._lblCaption1.Position = this.Caption1PositionSmall;
              this._lblCaption2.Position = this.Caption2PositionSmall;
              break;
            case UISnailsButton.ButtonSizeType.Medium:
              this._lblCaption1.Position = this.Caption1PositionMedium;
              this._lblCaption2.Position = this.Caption2PositionMedium;
              break;
          }
        }
        else
        {
          if (strArray.Length != 1)
            return;
          this._lblCaption1.Visible = true;
          this._lblCaption1.Text = strArray[0];
          this._lblCaption1.ParentAlignment = AlignModes.HorizontalyVertically;
          this._lblCaption2.Visible = false;
          switch (this.SizeType)
          {
            case UISnailsButton.ButtonSizeType.Small:
              this._lblCaption1.Font = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-medium", ResourceManager.ResourceManagerCacheType.Static);
              break;
            case UISnailsButton.ButtonSizeType.Medium:
              this._lblCaption1.Font = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-medium", ResourceManager.ResourceManagerCacheType.Static);
              break;
          }
        }
      }
    }

    public override void Show()
    {
      this._pnlContainer.Visible = true;
      base.Show();
    }

    public enum ButtonSizeType
    {
      Small,
      Medium,
    }

    public enum ButtonLabelType
    {
      Text,
      Image,
    }

    public enum ButtonActionType
    {
      Undefined,
      Back,
      Next,
      Start,
      Previous,
      MainMenu,
      StageSelection,
      Retry,
    }
  }
}
