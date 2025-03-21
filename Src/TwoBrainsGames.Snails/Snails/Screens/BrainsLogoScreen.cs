
// Type: TwoBrainsGames.Snails.Screens.BrainsLogoScreen
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;
using TwoBrainsGames.Snails.Screens.Transitions;


namespace TwoBrainsGames.Snails.Screens
{
  internal class BrainsLogoScreen : UIScreen
  {
    private UIImage _imgLogo;
    private UITimer _tmrSkip;
    private UIImage _imgFacebok;
    private UIImage _imgTwitter;
    private UIPanel _panelSocial;

    private BrainsLogoScreen.ScreenState State { get; set; }

    public BrainsLogoScreen(ScreenNavigator owner)
      : base(owner)
    {
      this.BackgroundColor = Color.White;
      this._imgLogo = new UIImage((UIScreen) this, "spriteset/brains-logo/BrainsLogo", 
          "__TEMPORARY__");
      this._imgLogo.ParentAlignment = AlignModes.HorizontalyVertically;
      this.Controls.Add((UIControl) this._imgLogo);
      this._tmrSkip = new UITimer((UIScreen) this, 2500.0, false);
      this._tmrSkip.OnTimer += new UIControl.UIEvent(this._tmrSkip_OnTimer);
      this.Controls.Add((UIControl) this._tmrSkip);
      this._panelSocial = new UIPanel((UIScreen) this);
      this._panelSocial.ParentAlignment = AlignModes.Horizontaly | AlignModes.Bottom;
      this._panelSocial.Margins.Bottom = 500f;
      this._panelSocial.Size = this.NativeResolution(new Size(1000f, 1000f));
      this.Controls.Add((UIControl) this._panelSocial);
      this._imgFacebok = new UIImage((UIScreen) this, "spriteset/menu-elements-1/Facebook", 
          "__TEMPORARY__");
      this._imgFacebok.ParentAlignment = AlignModes.Vertically | AlignModes.Left;
      this._panelSocial.Controls.Add((UIControl) this._imgFacebok);
      this._imgTwitter = new UIImage((UIScreen) this, "spriteset/menu-elements-1/Twitter", 
          "__TEMPORARY__");
      this._imgTwitter.ParentAlignment = AlignModes.Vertically | AlignModes.Right;
      this._panelSocial.Controls.Add((UIControl) this._imgTwitter);

      this.OnAccept += new UIControl.UIEvent(this.BrainsLogoScreen_OnAccept);

      BrainGame.DisplayHDDAccessIcon = false;
    }

    private void BrainsLogoScreen_OnAccept(IUIControl sender)
    {
        this.NavigateToMain();
    }

    public override void OnStart()
    {
      base.OnStart();
      this._tmrSkip.Enabled = true;
    }

    private void _tmrSkip_OnTimer(IUIControl sender) => this.NavigateToMain();

    private void NavigateToMain()
    {
      BrainGame.DisplayHDDAccessIcon = true;
      BrainGame.ClearColor = Color.White;

      if (Game1.GameSettings.ShowAutoSaveScreen)
        this.NavigateTo(ScreenType.AutoSave.ToString(), 
            (Transition) ScreenTransitions.FadeOut, 
            (Transition) ScreenTransitions.FadeIn);
      else
        this.NavigateTo("MainMenu", ScreenType.MainMenu.ToString(), 
            (Transition) ScreenTransitions.FadeOutWhite,
            (Transition) ScreenTransitions.FadeInWhite);
    }

    private enum ScreenState
    {
      Startup,
      BetaInfo,
      Logo,
    }
  }
}
