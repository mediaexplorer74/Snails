
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UIBackButton
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
  internal class UIBackButton : UIButton
  {
    private UIImage _imgBack;
    private UIBackButton.ButtonScreenAlignment _screenAlignment;

    public UIBackButton.ButtonScreenAlignment ScreenAlignment
    {
      get => this._screenAlignment;
      set
      {
        this._screenAlignment = value;
        switch (this._screenAlignment)
        {
          case UIBackButton.ButtonScreenAlignment.BottomLeft:
            this.ParentAlignment = AlignModes.Bottom | AlignModes.Left;
            this.Margins.Left = 150f;
            this.Margins.Bottom = 250f;
            break;
          case UIBackButton.ButtonScreenAlignment.BottomRight:
            this.ParentAlignment = AlignModes.Right | AlignModes.Bottom;
            this.Margins.Right = 150f;
            this.Margins.Bottom = 250f;
            break;
        }
      }
    }

    public UIBackButton(UIScreen screenOwner)
      : base(screenOwner)
    {
      this._imgBack = new UIImage(screenOwner, "spriteset/button-icons/MenuBackIcon");
      this._imgBack.Effect = (ITransformEffect) new HooverEffect(0.2f, 0.5f, -90f);
      this.Controls.Add((UIControl) this._imgBack);
      this.Size = this._imgBack.Size;
      this.ControllerActionCode = 2;
      this.PressEffect = (TransformEffectBase) new ColorEffect(Color.White, Color.Gray, 0.4f, true, Color.White, 130.0);
      this.ShowEffect = (TransformEffectBase) new SquashEffect(0.85f, 4f, 0.03f, this.BlendColor, this.Scale);
      this.PressSound = BrainGame.ResourceManager.GetSampleStatic("sfx/menu-item-selected");
    }

    public enum ButtonScreenAlignment
    {
      None,
      BottomLeft,
      BottomRight,
    }
  }
}
