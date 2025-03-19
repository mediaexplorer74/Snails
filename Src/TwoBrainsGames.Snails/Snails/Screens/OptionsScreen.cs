
// Type: TwoBrainsGames.Snails.Screens.OptionsScreen
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.Snails.Screens.Transitions;


namespace TwoBrainsGames.Snails.Screens
{
  internal class OptionsScreen(ScreenNavigator owner) : SnailsScreen(owner, ScreenType.Options)
  {
    private UIMainMenuBodyPanel _pnlBody;
    private UISnailsMenu _mnuOptions;
    private UISoundMenu _mnuSound;
    private UIXBoxControls _xboxController;
    private UISnailsMenuItem _itmToggleFullscreen;
    private UILanguageMenu _mnuLanguage;
    private OptionsScreen.StartupType _startType;
    private UISnailsMenuItem _itemScreenSize;
    private UITimer _tmrGenerir;
    protected UIIntroPicture _introPicture;

    private OptionsScreen.ScreenState State { get; set; }

    private bool ShowHowToPlayOption { get; set; }

    private bool ShowCreditsOption { get; set; }

    public override void OnLoad()
    {
      base.OnLoad();
      this.Name = "Options";
      this.BackgroundImage = (Sprite) null;
      this.ShowCreditsOption = true;
      this.ShowHowToPlayOption = true;
      this._introPicture = new UIIntroPicture((UIScreen) this);
      this._introPicture.Name = "_introPicture2";
      this._introPicture.ParentAlignment = AlignModes.HorizontalyVertically;
      this._introPicture.HideEffect = (TransformEffectBase) new ColorEffect(new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), new Color(0, 0, 0, 0), 0.05f, false);
      this._introPicture.ShowEffect = (TransformEffectBase) new ColorEffect(new Color(0, 0, 0, 0), new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue), 0.025f, false);
      this._introPicture.ScaleChilds(new Vector2(Game1.GameSettings.RatioNativeResolutionWidth, Game1.GameSettings.RatioNativeResolutionHeight));
      this.Controls.Add((UIControl) this._introPicture);
      this._pnlBody = new UIMainMenuBodyPanel((UIScreen) this);
      this.Controls.Add((UIControl) this._pnlBody);
      this._mnuOptions = new UISnailsMenu((UIScreen) this);
      this._mnuOptions.Name = "_mnuOptions";
      this._mnuOptions.ParentAlignment = AlignModes.HorizontalyVertically;
      this._mnuOptions.Size = new Size(3000f, 3800f);
      this._mnuOptions.TextResourceId = "MNU_ITEM_OPTIONS";
      this._mnuOptions.DefaultItemIndex = 0;
      this._mnuOptions.ItemSize = UISnailsMenu.MenuItemSize.Medium;
      this._mnuOptions.OnMenuShown += new UIControl.UIEvent(this._mnuOptions_OnMenuShown);
      this._mnuOptions.OnItemSelectedBegin += new UIControl.UIEvent(this._mnuOptions_OnItemSelectedBegin);
      this._mnuOptions.OnBackPressed += new UIControl.UIEvent(this._mnuOptions_OnBackPressed);
      this._mnuOptions.WithBackButton = true;
      this._pnlBody.Controls.Add((UIControl) this._mnuOptions);
      this._mnuOptions.AddMenuItem("MNU_ITEM_SOUND_SETTINGS", new UIControl.UIEvent(this.OptionsMenu_OnSoundSettings), InputBase.InputActions.None);
      this._itmToggleFullscreen = this._mnuOptions.AddMenuItem((string) null, new UIControl.UIEvent(this.OptionsMenu_OnToggleFullscreen), InputBase.InputActions.None, false, Game1.GameSettings.AllowToggleFullScreen);
      this._mnuOptions.AddMenuItem("MNU_ITEM_LANGUAGE", new UIControl.UIEvent(this.OptionsMenu_OnLanguage), InputBase.InputActions.None);
      this._mnuOptions.AddMenuItem("MNU_ITEM_CONTROLS", new UIControl.UIEvent(this.OptionsMenu_OnControlsHelp), InputBase.InputActions.None, true, Game1.GameSettings.UseGamepad);
      this._itemScreenSize = this._mnuOptions.AddMenuItem("MNU_ITEM_SCREEN_SIZE", new UIControl.UIEvent(this.OptionsMenu_OnScreenSize), InputBase.InputActions.None, false, Game1.GameSettings.AllowOverscanAdjustment);
      this._mnuOptions.AddMenuItem("MNU_ITEM_HOW_TO_PLAY", new UIControl.UIEvent(this.OptionsMenu_OnHowToPlay), InputBase.InputActions.None, false, this.ShowHowToPlayOption);
      this._mnuOptions.AddMenuItem("MNU_ITEM_CREDITS", new UIControl.UIEvent(this.OptionsMenu_OnCredits), InputBase.InputActions.None, false, this.ShowCreditsOption);
      this._mnuLanguage = new UILanguageMenu((UIScreen) this);
      this._mnuLanguage.OnHide += new UIControl.UIEvent(this._mnuLanguage_OnHide);
      this._mnuLanguage.OnShow += new UIControl.UIEvent(this._mnuLanguage_OnShow);
      this._mnuLanguage.ParentAlignment = AlignModes.HorizontalyVertically;
      this._mnuLanguage.OnLanguageSelected += new UIControl.UIEvent(this._mnuLanguage_OnLanguageSelected);
      this._pnlBody.Controls.Add((UIControl) this._mnuLanguage);
      this._mnuSound = new UISoundMenu((UIScreen) this);
      this._mnuSound.OnHide += new UIControl.UIEvent(this._mnuSound_OnHide);
      this._mnuSound.ParentAlignment = AlignModes.HorizontalyVertically;
      this._mnuSound.OnShow += new UIControl.UIEvent(this._mnuSound_OnShow);
      this._pnlBody.Controls.Add((UIControl) this._mnuSound);
      this._tmrGenerir = new UITimer((UIScreen) this);
      this.Controls.Add((UIControl) this._tmrGenerir);
      this._xboxController = new UIXBoxControls((UIScreen) this);
      this._xboxController.ParentAlignment = AlignModes.HorizontalyVertically;
      this._xboxController.OnDismiss += new UIControl.UIEvent(this._xboxController_OnDismiss);
      this._xboxController.OnShow += new UIControl.UIEvent(this._xboxController_OnShow);
      this._pnlBody.Controls.Add((UIControl) this._xboxController);
      this.OnOpenTransitionEnded += new UIControl.UIEvent(this.OptionsScreen_OnOpenTransitionEnded);
      this.OnPopupClosed += new UIControl.UIEvent(this.OptionsScreen_OnPopupClosed);
      this.ShowTrialTag = true;
    }

    private void OptionsScreen_OnOpenTransitionEnded(IUIControl sender)
    {
      if (this._startType != OptionsScreen.StartupType.MenuVisible)
        return;
      this.EnableInput();
      this._mnuOptions.SetFocus(this._itemScreenSize);
    }

    private void OptionsScreen_OnPopupClosed(IUIControl sender) => this.EnableInput();

    public override void OnStart()
    {
      base.OnStart();
      this._introPicture.Initialize();
      this._introPicture.ShowBackgroundPanel();
      this._introPicture.SetBackgroundAlpha();
      this._introPicture.SetSaveState(this.Navigator.GlobalCache.Get<UIIntroPicture.IntroPictureSaveState>("INTRO_PICTURE_STATE"));
      this.State = OptionsScreen.ScreenState.None;
      this.FooterMessage.Visible = true;
      this._mnuOptions.IdxLastItemSelected = 0;
      this.SetToggleScreenModeMenuItemName();
      this._mnuLanguage.Visible = false;
      this._mnuSound.Visible = false;
      this._xboxController.Visible = false;
      this.DisableInput();
      this._startType = this.Navigator.GlobalCache.Get<OptionsScreen.StartupType>("OPTIONS_STARTUP_MODE", OptionsScreen.StartupType.MenuHidden);
      switch (this._startType)
      {
        case OptionsScreen.StartupType.MenuHidden:
          this._mnuOptions.Visible = false;
          this._mnuOptions.Show();
          break;
        case OptionsScreen.StartupType.MenuVisible:
          this._mnuOptions.ShowWithoutEffects();
          break;
      }
    }

    private void _tmrGenerir_OnTimerShowMainMenu(IUIControl sender) => this._mnuOptions.Show();

    private void _mnuOptions_OnMenuShown(IUIControl sender)
    {
      this.EnableInput();
      this._mnuOptions.SetFocusOnLastSelectedItem();
      this.State = OptionsScreen.ScreenState.MainMenu;
    }

    private void _mnuOptions_OnItemSelectedBegin(IUIControl sender) => this.DisableInput();

    private void OptionsMenu_OnSoundSettings(IUIControl sender)
    {
      this.DisableInput();
      this.State = OptionsScreen.ScreenState.MusicToggle;
      this._mnuSound.Show();
    }

    private void OptionsMenu_OnScreenSize(IUIControl sender)
    {
      this.DisableInput();
      this.Navigator.GlobalCache.Set("OPTIONS_LAST_SELECTED_ITEM", (object) this._itemScreenSize);
      this.Navigator.GlobalCache.Set("OVERSCAN_CALLER_SCREEN", (object) ScreenType.Options);
      this.NavigateTo(ScreenType.Overscan.ToString(), (Transition) ScreenTransitions.LeafsClosing, (Transition) ScreenTransitions.LeafsOpening);
    }

    private void OptionsMenu_OnControlsHelp(IUIControl sender)
    {
      this.DisableInput();
      this.State = OptionsScreen.ScreenState.ControlsHelp;
      this._xboxController.Show();
    }

    private void OptionsMenu_OnLanguage(IUIControl sender)
    {
      this.DisableInput();
      this.State = OptionsScreen.ScreenState.Language;
      this._mnuLanguage.Show();
    }

    private void OptionsMenu_OnToggleFullscreen(IUIControl sender)
    {
      BrainGame.ToggleFullScreen();
      this.SetToggleScreenModeMenuItemName();
      Game1.ProfilesManager.CurrentProfile.Fullscreen = Game1.GameSettings.IsFullScreen;
      Game1.ProfilesManager.Save();
      this.EnableInput();
    }

    private void OptionsMenu_OnHowToPlay(IUIControl sender)
    {
      this.DisableInput();
      HowToPlayScreen.PopUp(Game1.Tutorial.Topics);
    }

    private void OptionsMenu_OnCredits(IUIControl sender)
    {
      this.Navigator.GlobalCache.Set("CREDITS_SCREEN_CALLER", (object) ScreenType.Options);
      this.NavigateTo(ScreenType.Credits.ToString(), (Transition) ScreenTransitions.LeafsClosing, (Transition) ScreenTransitions.LeafsOpening);
    }

    private void OptionsMenu_OnAwards(IUIControl sender)
    {
      this.NavigateTo(ScreenType.Awards.ToString(), (Transition) ScreenTransitions.LeafsClosing, (Transition) ScreenTransitions.LeafsOpening);
    }

    private void SetToggleScreenModeMenuItemName()
    {
      if (this._itmToggleFullscreen == null)
        return;
      this._itmToggleFullscreen.TextResourceId = BrainGame.GraphicsManager.IsFullScreen ? "MNU_ITEM_WINDOWED" : "MNU_ITEM_FULLSCREEN";
    }

    private void OptionsMenu_OnPlayerStats(IUIControl sender)
    {
    }

    private void OptionsMenu_OnBackToMain(IUIControl sender) => this.NavigateToMain();

    private void _mnuOptions_OnBackPressed(IUIControl sender) => this.NavigateToMain();

    private void NavigateToMain()
    {
      Game1.ProfilesManager.Save();
      this.Navigator.GlobalCache.Set("INTRO_PICTURE_STATE", (object) this._introPicture.GetSaveState());
      this.Navigator.GlobalCache.Set("MAIN_SCREEN_STARTUP_MODE", (object) MainMenuScreen.StartupType.TitleVisibleMenuHidden);
      this.NavigateTo(ScreenType.MainMenu.ToString(), (Transition) null, (Transition) null);
    }

    private void _mnuSound_OnHide(IUIControl sender) => this._mnuOptions.Show();

    private void _mnuSound_OnShow(IUIControl sender) => this.EnableInput();

    private void _mnuLanguage_OnHide(IUIControl sender) => this._mnuOptions.Show();

    private void _mnuLanguage_OnShow(IUIControl sender)
    {
      this.EnableInput();
      this._mnuLanguage.SetFocus((int) BrainGame.CurrentLanguage);
    }

    private void _xboxController_OnDismiss(IUIControl sender) => this._mnuOptions.Show();

    private void _xboxController_OnShow(IUIControl sender)
    {
      this.EnableInput();
      this._xboxController.Focus();
    }

    private void _mnuLanguage_OnLanguageSelected(IUIControl sender) => this.NavigateToMain();

    private enum ScreenState
    {
      None,
      MainMenu,
      SoundFxToggle,
      MusicToggle,
      Language,
      ControlsHelp,
      HowToPlay,
    }

    public enum StartupType
    {
      MenuHidden,
      MenuVisible,
    }
  }
}
