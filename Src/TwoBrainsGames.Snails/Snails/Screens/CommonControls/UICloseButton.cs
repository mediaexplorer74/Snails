
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UICloseButton
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UICloseButton : UIButton
  {
    private UIImage _imgButton;
    private UICloseButton.ButtonFaceType _faceType;

    public UICloseButton.ButtonFaceType FaceType
    {
      get => this._faceType;
      set
      {
        this._faceType = value;
        string spriteResourceName = (string) null;
        switch (this._faceType)
        {
          case UICloseButton.ButtonFaceType.Dark:
            spriteResourceName = "spriteset/button-icons/CloseIcon";
            break;
          case UICloseButton.ButtonFaceType.Light:
            spriteResourceName = "spriteset/button-icons/CloseIconLight";
            break;
        }
        if (spriteResourceName == null)
          return;
        this._imgButton.Sprite = BrainGame.ResourceManager.GetSpriteStatic(spriteResourceName);
      }
    }

    public bool UseHotKey
    {
      set
      {
        this.ControllerActionCode = 0;
        if (!value)
          return;
        this.ControllerActionCode = 2;
      }
    }

    public UICloseButton(UIScreen screenOwner)
      : base(screenOwner)
    {
      this._imgButton = new UIImage(screenOwner, "spriteset/button-icons/CloseIcon");
      this._imgButton.ParentAlignment = AlignModes.HorizontalyVertically;
      this._imgButton.Effect = (ITransformEffect) new ScaleEffect(new Vector2(1f, 1f), 0.2f, new Vector2(0.94f, 0.94f), true);
      this.Controls.Add((UIControl) this._imgButton);
      this.PressEffect = (TransformEffectBase) new ColorEffect(Color.White, Color.Gray, 0.4f, true, Color.White, 130.0);
      this.OnScreenStart += new UIControl.UIEvent(this.UICloseButton_OnScreenStart);
      this.Size = this._imgButton.Size;
      this.PressSound = BrainGame.ResourceManager.GetSampleStatic("sfx/menu-item-selected");
    }

    private void UICloseButton_OnScreenStart(IUIControl sender) => this.BlendColor = Color.White;

    public enum ButtonFaceType
    {
      Dark,
      Light,
    }
  }
}
