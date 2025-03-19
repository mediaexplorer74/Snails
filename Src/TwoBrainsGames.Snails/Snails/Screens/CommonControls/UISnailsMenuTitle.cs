
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UISnailsMenuTitle
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Effects;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UISnailsMenuTitle : UIControl
  {
    private static Color TITLE_COLOR = new Color((int) byte.MaxValue, 240, 26);
    private static Vector2 TITLE_FONT_SIZE = new Vector2(1f, 1f);
    private UITextFontLabel _lblTitle;
    private UIImage _imgTitleBack;
    private TextFont _menuTitleFont;
    private UISnailsMenuTitle.TitleSize _boardSize;

    public override bool Visible
    {
      get => base.Visible;
      set
      {
        base.Visible = value;
        if (this._lblTitle != null)
          this._lblTitle.Visible = value;
        if (this._imgTitleBack == null)
          return;
        this._imgTitleBack.Visible = value;
      }
    }

    public override string TextResourceId
    {
      get => this._lblTitle.TextResourceId;
      set => this._lblTitle.TextResourceId = value;
    }

    public UISnailsMenuTitle.TitleSize BoardSize
    {
      get => this._boardSize;
      set
      {
        this._boardSize = value;
        switch (this._boardSize)
        {
          case UISnailsMenuTitle.TitleSize.Medium:
            this._imgTitleBack.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/boards", "MenuTitleMedium");
            break;
          case UISnailsMenuTitle.TitleSize.Big:
            this._imgTitleBack.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/boards", "MenuTitleBig");
            break;
        }
        this.Size = this._imgTitleBack.Size;
      }
    }

    public new Vector2 Scale
    {
      get => base.Scale;
      set
      {
        this._imgTitleBack.Scale = value;
        this._lblTitle.Scale = UISnailsMenuTitle.TITLE_FONT_SIZE * value;
        base.Scale = value;
      }
    }

    public UISnailsMenuTitle(UIScreen ownerScreen)
      : base(ownerScreen)
    {
      this._menuTitleFont = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-big", ResourceManager.ResourceManagerCacheType.Static);
      this._imgTitleBack = new UIImage(ownerScreen);
      this._imgTitleBack.Name = nameof (_imgTitleBack);
      this._imgTitleBack.ParentAlignment = AlignModes.Horizontaly;
      this._imgTitleBack.Position = new Vector2(0.0f, -150f);
      this._imgTitleBack.ParentAlignment = AlignModes.HorizontalyVertically;
      this._imgTitleBack.OnHide += new UIControl.UIEvent(this._imgTitleBack_OnHide);
      this.Controls.Add((UIControl) this._imgTitleBack);
      this._lblTitle = new UITextFontLabel(ownerScreen, this._menuTitleFont, "");
      this._lblTitle.Name = nameof (_lblTitle);
      this._lblTitle.ParentAlignment = AlignModes.Horizontaly;
      this._lblTitle.BlendColor = UISnailsMenuTitle.TITLE_COLOR;
      this._lblTitle.Scale = UISnailsMenuTitle.TITLE_FONT_SIZE;
      this._lblTitle.ParentAlignment = AlignModes.HorizontalyVertically;
      this._lblTitle.OnHide += new UIControl.UIEvent(this._lblTitle_OnHide);
      this.Controls.Add((UIControl) this._lblTitle);
      this.ParentAlignment = AlignModes.Horizontaly;
      this.Size = this._imgTitleBack.Size;
      this.AcceptControllerInput = false;
      this.BoardSize = UISnailsMenuTitle.TitleSize.Medium;
      this.Name = "_menuTitle";
      this.ShowEffect = (TransformEffectBase) new SquashEffect(0.85f, 4f, 0.03f, this.BlendColor, new Vector2(1f, 1f));
      this.HideEffect = (TransformEffectBase) new PopOutEffect(new Vector2(1.2f, 1.2f), 6f);
      this.OnScreenStart += new UIControl.UIEvent(this.UISnailsMenuTitle_OnScreenStart);
    }

    private void UISnailsMenuTitle_OnScreenStart(IUIControl sender) => this.Scale = Vector2.One;

    private void _imgTitleBack_OnHide(IUIControl sender)
    {
      if (this._imgTitleBack.Visible || this._lblTitle.Visible)
        return;
      base.Visible = false;
    }

    private void _lblTitle_OnHide(IUIControl sender)
    {
      if (this._imgTitleBack.Visible || this._lblTitle.Visible)
        return;
      base.Visible = false;
    }

    public enum TitleSize
    {
      Medium,
      Big,
    }
  }
}
