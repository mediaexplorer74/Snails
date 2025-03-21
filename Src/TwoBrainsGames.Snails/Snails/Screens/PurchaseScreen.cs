
// Type: TwoBrainsGames.Snails.Screens.PurchaseScreen
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.Snails.Screens.Transitions;


namespace TwoBrainsGames.Snails.Screens
{
  internal class PurchaseScreen(ScreenNavigator owner) : SnailsScreen(owner, ScreenType.Purchase)
  {
    private Color GetGameColor = new Color((int) byte.MaxValue, 136, 25);
    private Color FeaturesColor = new Color(240, (int) byte.MaxValue, 25);
    protected UISnailsButton _btnPurchase;
    protected UISnailsButton _btnMainMenu;
    private UIImage _imgScreen1;
    private UIImage _imgScreen2;
    private UIImage _imgScreen3;
    private UIImage _imgTitle;

    public override void OnLoad()
    {
      base.OnLoad();
      this.BackgroundType = SnailsScreen.ScreenBackgroundType.Leafs;
      this._imgScreen1 = new UIImage((UIScreen) this, "spriteset/purchase/Photo1", "__TEMPORARY__");
      this._imgScreen1.Rotation = -8f;
      this._imgScreen1.Position = new Vector2(2300f, 2500f);
      this.Controls.Add((UIControl) this._imgScreen1);
      this._imgScreen2 = new UIImage((UIScreen) this, "spriteset/purchase/Photo2", "__TEMPORARY__");
      this._imgScreen2.Rotation = 10f;
      this._imgScreen2.Position = new Vector2(2900f, 6600f);
      this.Controls.Add((UIControl) this._imgScreen2);
      this._imgScreen3 = new UIImage((UIScreen) this, "spriteset/purchase/Photo3", "__TEMPORARY__");
      this._imgScreen3.Rotation = 3f;
      this._imgScreen3.Position = new Vector2(4400f, 4200f);
      this.Controls.Add((UIControl) this._imgScreen3);
      UIPanel control1 = new UIPanel((UIScreen) this);
      control1.Position = new Vector2(4700f, 2000f);
      control1.Size = new Size(4800f, 7000f);
      control1.BackgroundColor = new Color(0, 0, 0, 90);
      this.Controls.Add((UIControl) control1);
      this._imgTitle = new UIImage((UIScreen) this, "spriteset/common-elements-1/SnailsLogTitle", "__STATIC__");
      this._imgTitle.Position = new Vector2(0.0f, -1500f);
      this._imgTitle.ParentAlignment = AlignModes.Horizontaly;
      this._imgTitle.Scale = new Vector2(0.7f, 0.7f);
      control1.Controls.Add((UIControl) this._imgTitle);
      UICaption control2 = new UICaption((UIScreen) this, "", this.GetGameColor, UICaption.CaptionStyle.Heading1);
      control2.TextResourceId = "LBL_GET_FULLVERSION";
      control2.Position = new Vector2(150f, 950f);
      control2.Scale = new Vector2(0.7f, 0.7f);
      control1.Controls.Add((UIControl) control2);
      UICaption control3 = new UICaption((UIScreen) this, "", this.FeaturesColor, UICaption.CaptionStyle.NormalText);
      control3.TextResourceId = "LBL_4_THEMES";
      control3.Position = new Vector2(600f, 2200f);
      control1.Controls.Add((UIControl) control3);
      UICaption control4 = new UICaption((UIScreen) this, "", this.FeaturesColor, UICaption.CaptionStyle.NormalText);
      control4.TextResourceId = "LBL_84_STAGES";
      control4.Position = new Vector2(600f, 2700f);
      control1.Controls.Add((UIControl) control4);
      UICaption control5 = new UICaption((UIScreen) this, "", this.FeaturesColor, UICaption.CaptionStyle.NormalText);
      control5.TextResourceId = "LBL_4_MODES";
      control5.Position = new Vector2(600f, 3200f);
      control1.Controls.Add((UIControl) control5);
      UICaption control6 = new UICaption((UIScreen) this, "", this.FeaturesColor, UICaption.CaptionStyle.NormalText);
      control6.TextResourceId = "LBL_NEW_TOOLS";
      control6.Position = new Vector2(600f, 3700f);
      control1.Controls.Add((UIControl) control6);
      UICaption control7 = new UICaption((UIScreen) this, "", this.FeaturesColor, UICaption.CaptionStyle.NormalText);
      control7.TextResourceId = "LBL_LOT_OF_FUN";
      control7.Position = new Vector2(600f, 4200f);
      control1.Controls.Add((UIControl) control7);
      this._btnPurchase = new UISnailsButton((UIScreen) this, "BTN_PURCHASE", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, new UIControl.UIEvent(this.btnPurchase_OnClick), true);
      this._btnPurchase.Name = "_btnPurchase";
      this._btnPurchase.Position = new Vector2(300f, 5000f);
      this._btnPurchase.Visible = false;
      this._btnPurchase.OnShow += new UIControl.UIEvent(this._btnPurchase_OnShow);
      control1.Controls.Add((UIControl) this._btnPurchase);
      this._btnMainMenu = new UISnailsButton((UIScreen) this, "BTN_MAIN_MENU", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.Back, new UIControl.UIEvent(this.btnMainMenu_OnClick), false);
      this._btnMainMenu.Name = "_btnMainMenu";
      this._btnMainMenu.Position = new Vector2(2700f, 5000f);
      this._btnMainMenu.Visible = false;
      this._btnMainMenu.OnAcceptBegin += new UIControl.UIEvent(this._btnMainMenu_OnAcceptBegin);
      control1.Controls.Add((UIControl) this._btnMainMenu);
      UICaption control8 = new UICaption((UIScreen) this, "", new Color(0, 250, 55), UICaption.CaptionStyle.NormalText);
      control8.TextResourceId = "LBL_SUPPORT_US";
      control8.ParentAlignment = AlignModes.Horizontaly | AlignModes.Bottom;
      control8.Margins.Bottom = 300f;
      this.Controls.Add((UIControl) control8);
    }

    private void _btnPurchase_OnShow(IUIControl sender) => this.EnableInput();

    public override void OnStart()
    {
      base.OnStart();
      this.DisableInput();
      this._btnPurchase.Show();
      this._btnMainMenu.Show();
    }

    public override void OnUpdate(BrainGameTime gameTime)
    {
      base.OnUpdate(gameTime);
      //if (BrainGame.IsTrial)
      //  return;
      this.NavigateToMain();
    }

    private void btnMainMenu_OnClick(IUIControl sender) => this.NavigateToMain();

    private void btnPurchase_OnClick(IUIControl sender) => Game1.Instance.PurchaseGame();

    private void _btnMainMenu_OnAcceptBegin(IUIControl sender) => this.DisableInput();

    private void NavigateToMain()
    {
      this.DisableInput();
      this.Navigator.GlobalCache.Set("MAIN_SCREEN_STARTUP_MODE", (object) MainMenuScreen.StartupType.TitleAndMenuVisible);
      this.NavigateTo("MainMenu", ScreenType.MainMenu.ToString(), (Transition) ScreenTransitions.LeafsClosed, (Transition) ScreenTransitions.LeafsOpening);
    }
  }
}
