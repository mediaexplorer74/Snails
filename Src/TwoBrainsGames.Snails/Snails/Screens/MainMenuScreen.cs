
// Type: TwoBrainsGames.Snails.Screens.MainMenuScreen
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.Snails.Screens.Transitions;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.Screens
{
  internal class MainMenuScreen(ScreenNavigator owner) : SnailsScreen(owner, ScreenType.MainMenu)
  {
    private const double TIME_IDLE_SHOW_INTRO_PIC = 30000.0;
    private const double TIME_IDLE_SHOW_MENU = 30000.0;
    private const double BLINK_TIME = 500.0;
    private const int BB_IDX_PRESS_ANY_KEY = 0;
    private const int BB_IDX_MENU = 1;
    protected MainMenuScreen.StartupType _startType;
    protected MainMenuScreen.State _state;
    protected UIIntroPicture _introPicture;
    protected UICaption _capPressAnyKey;
    protected UIMainMenuBodyPanel _pnlBody;
    protected UISnailsMenu _mnuMain;
    protected UITimer _timerShowMenu;
    protected UISnailsMenuItem _itmNewGame;
    protected UISnailsMenuItem _itmStageSelection;
    protected UISnailsButton _btnPurchase;

    private bool ShowHowToPlayOption { get; set; }

    private bool ShowCreditsOption { get; set; }

    private bool ShowPurchaseButton
    {
        get
        {
            return false;//BrainGame.IsTrial && Game1.GameSettings.WithAppStore;
        }
    }

    ~MainMenuScreen()
    {
    }

    public override void OnLoad()
    {
      base.OnLoad();
      Game1.ThemeMusic = BrainGame.ResourceManager.GetMusicTemporary("musics/snail-menu-theme");
      this.Name = "MainMenu";
      this.ShowCreditsOption = false;
      this.ShowHowToPlayOption = true;
      this.BackgroundImageBlendColor = Colors.MainMenuScrBkColor;
      this._introPicture = new UIIntroPicture((UIScreen) this);
      this._introPicture.Name = "_introPicture";
      this._introPicture.ParentAlignment = AlignModes.HorizontalyVertically;

      this._introPicture.HideEffect = 
                (TransformEffectBase) new ColorEffect(new Color((int) byte.MaxValue, 
                (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue),
                new Color(0, 0, 0, 0), 0.05f, false);

      this._introPicture.ShowEffect = 
                (TransformEffectBase) new ColorEffect(new Color(0, 0, 0, 0),
                new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 
                (int) byte.MaxValue), 0.025f, false);

      this._introPicture.ScaleChilds(
          new Vector2(Game1.GameSettings.RatioNativeResolutionWidth, 
          Game1.GameSettings.RatioNativeResolutionHeight));
      this.Controls.Add((UIControl) this._introPicture);
      this._capPressAnyKey = new UICaption((UIScreen) this, "", Color.White, 
          UICaption.CaptionStyle.IntroPressAnyKey);
      this._capPressAnyKey.Name = "_capPressAnyKey";
      this._capPressAnyKey.ParentAlignment = AlignModes.Horizontaly;
      this._capPressAnyKey.Position = new Vector2(0.0f, 8000f);
      this._capPressAnyKey.TextResourceId = "LBL_PRESS_ANY_BUTTON";
      this.Controls.Add((UIControl) this._capPressAnyKey);
      this._introPicture.SendToBack();
      this._capPressAnyKey.Effect = (ITransformEffect) new BlinkEffect(800.0, 300.0,
          (UIControl) this._capPressAnyKey, 
          BrainGame.ResourceManager.GetSampleTemporary("sfx/text-blink"),
          this._capPressAnyKey.BlendColor);
      this._pnlBody = new UIMainMenuBodyPanel((UIScreen) this);
      this.Controls.Add((UIControl) this._pnlBody);
      this._mnuMain = new UISnailsMenu((UIScreen) this);
      this._mnuMain.Name = "_mnuMain";
      this._mnuMain.Size = new Size(3000f, 3800f);
      this._mnuMain.Visible = false;
      this._mnuMain.TextResourceId = "MNU_MAIN_MENU";
      this._mnuMain.DefaultItemIndex = 0;
      this._mnuMain.OnMenuShownBegin += new UIControl.UIEvent(this.MainMenu_OnMenuShownBegin);
      this._mnuMain.OnMenuShown += new UIControl.UIEvent(this.MainMenu_OnMenuShown);
      this._mnuMain.OnMenuHideBegin += new UIControl.UIEvent(this.MainMenu_OnMenuHideBegin);
      this._mnuMain.ParentAlignment = AlignModes.HorizontalyVertically;
      this._pnlBody.Controls.Add((UIControl) this._mnuMain);

      //RnD
      this._itmNewGame = this._mnuMain.AddMenuItem("MNU_ITEM_PLAY",
          new UIControl.UIEvent(this.MainMenu_OnNewGame),
          //InputBase.InputActions.Accept,
          InputBase.InputActions.None, 
          false);

      this._itmStageSelection = this._mnuMain.AddMenuItem("MNU_ITEM_STAGE_SELECTION", 
          new UIControl.UIEvent(this.MainMenu_OnThemeSelection),
          InputBase.InputActions.None, 
          false);

      this._mnuMain.AddMenuItem("MNU_ITEM_CREDITS", 
          new UIControl.UIEvent(this.MainMenu_OnCredits),
          InputBase.InputActions.None, 
          false,
          this.ShowCreditsOption);

      this._mnuMain.AddMenuItem("MNU_ITEM_AWARDS", 
          new UIControl.UIEvent(this.OptionsMenu_OnAchievements), 
          InputBase.InputActions.None,
          false,
          true);

      this._mnuMain.AddMenuItem("MNU_ITEM_OPTIONS", 
          new UIControl.UIEvent(this.MainMenu_OnOptions), 
          InputBase.InputActions.None
          );

      if (Game1.GameSettings.ShowQuitOptions)
        this._mnuMain.AddMenuItem("MNU_ITEM_QUIT", 
            new UIControl.UIEvent(this.MainMenu_OnQuit),
            InputBase.InputActions.Back, 
            true);
      else
        this.OnBack += new UIControl.UIEvent(this.MenuConfirm_OnYes);
      this._btnPurchase = new UISnailsButton((UIScreen) this, "BTN_PURCHASE",
          UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, 
          new UIControl.UIEvent(this.btnPurchase_OnClick), true);
      this._btnPurchase.Name = "_btnPurchase";
      this._btnPurchase.ParentAlignment = AlignModes.Bottom | AlignModes.Left;
      this.Controls.Add((UIControl) this._btnPurchase);
      this._timerShowMenu = new UITimer((UIScreen) this, 750.0, false);
      this._timerShowMenu.OnTimer += new UIControl.UIEvent(this.TimerShowMenu_OnTimer);
      this.Controls.Add((UIControl) this._timerShowMenu);

      this.OnOpenTransitionEnded += new UIControl.UIEvent(this.MainMenuScreen_OnOpenTransitionEnded);
      this.OnPopupClosed += new UIControl.UIEvent(this.MainMenuScreen_OnPopupClosed);
      this.OnGameplayModeChanged += new UIControl.UIEvent(this.MainMenuScreen_OnGameplayModeChanged);
      this.ShowTrialTag = true;
    }

    public override void OnStart()
    {
      base.OnStart();
      if (Game1.ThemeMusic != null)
        Game1.ThemeMusic.Play(true);
      this._startType = this.Navigator.GlobalCache.Get<MainMenuScreen.StartupType>(
          "MAIN_SCREEN_STARTUP_MODE", MainMenuScreen.StartupType.IntroPicture);
      switch (this._startType)
      {
        case MainMenuScreen.StartupType.IntroPicture:
          this._introPicture.ResetBackgroundAlpha();
          this._mnuMain.Visible = false;
          this._btnPurchase.Visible = false;
          this._state = MainMenuScreen.State.PressAnyKey;
          this._timerShowMenu.Enabled = false;
          this.InstructionBar.Visible = false;
          break;
        case MainMenuScreen.StartupType.AllHidden:
          this._mnuMain.Visible = false;
          this._timerShowMenu.Reset();
          this._timerShowMenu.Enabled = true;
          this._introPicture.ResetBackgroundAlpha();
          this._capPressAnyKey.Visible = false;
          this._state = MainMenuScreen.State.MainMenu;
          this._btnPurchase.Visible = false;
          break;
        case MainMenuScreen.StartupType.TitleVisibleMenuHidden:
          this._btnPurchase.Visible = false;
          if (this.ShowPurchaseButton)
            this._btnPurchase.Show();
          this._mnuMain.Show();
          this._capPressAnyKey.Visible = false;
          this._state = MainMenuScreen.State.MainMenu;
          break;
        case MainMenuScreen.StartupType.TitleAndMenuVisible:
          this._mnuMain.ShowWithoutEffects();
          this._introPicture.SetBackgroundAlpha();
          this._capPressAnyKey.Visible = false;
          this._state = MainMenuScreen.State.MainMenu;
          this._btnPurchase.Visible = this.ShowPurchaseButton;
          this._mnuMain.AcceptControllerInput = true;
          break;
      }
      this._introPicture.Initialize();
      this._introPicture.SetSaveState(this.Navigator.GlobalCache.Get<UIIntroPicture.IntroPictureSaveState>(
          "INTRO_PICTURE_STATE"));
      this.InputController.ResetTimeIdle();
      if (Game1.ProfilesManager.CurrentProfile != null)
      {
        this._itmNewGame.TextResourceId = Game1.ProfilesManager.CurrentProfile.HasStartedNewGame
                    ? "MNU_ITEM_CONTINUE"
                    : "MNU_ITEM_PLAY";
        this._itmStageSelection.Enabled = Game1.ProfilesManager.CurrentProfile.HasStartedNewGame;
      }
      this.EnableInput();
    }

    public override void OnUpdate(BrainGameTime gameTime)
    {
      base.OnUpdate(gameTime);

      if (Game1.GameSettings.BackQuitsGameOnIntroPicture 
                && this.InputController.ActionBack
                && this._introPicture.Visible)
      {
        this.QuitGame();
      }
      else
      {
        switch (this._state)
        {
          case MainMenuScreen.State.PressAnyKey:
            //if (!BrainGame.IsTrial)
              BrainGame.AchievementsManager.Notify(47);

            if (!this.InputController.CheckActionStartPressed())
              break;

            this._state = MainMenuScreen.State.AssyncronousLoad;
            break;

          case MainMenuScreen.State.MainMenu:
            if (this.InputController.TimeIdleMsecs <= 30000.0)
              break;
            this._introPicture.FadeOutBackground();
            this._mnuMain.Hide();
            this.InputController.ResetTimeIdle();
            this._state = MainMenuScreen.State.IntroPicCursorIdle;
            break;
          case MainMenuScreen.State.IntroPicCursorIdle:
            if (this.InputController.TimeIdleMsecs > 30000.0 || this.InputController.CheckActionStartPressed())
              this._state = MainMenuScreen.State.ShowMainMenu;
            if (this._capPressAnyKey.Visible)
              break;
            this._capPressAnyKey.Show();
            break;

          case MainMenuScreen.State.AssyncronousLoad:
            if (!Game1.GameSettings.AsyncProfileLoading)
            {
              this._state = MainMenuScreen.State.ShowMainMenu;
              break;
            }
            if (!Game1.ProfilesManager.IsCompleted)
              break;
            this._itmNewGame.TextResourceId = Game1.ProfilesManager.CurrentProfile.HasStartedNewGame 
                            ? "MNU_ITEM_CONTINUE" : "MNU_ITEM_PLAY";
            this._itmStageSelection.Enabled = Game1.ProfilesManager.CurrentProfile.HasStartedNewGame;

            if (!Game1.ProfilesManager.CurrentProfile.OverscanSet && Game1.GameSettings.AllowOverscanAdjustment)
            {
              this.Navigator.GlobalCache.Set("OVERSCAN_CALLER_SCREEN", (object) ScreenType.InGameOptions);
              this.NavigateTo(ScreenType.Overscan.ToString(), (Transition) ScreenTransitions.FadeOut, 
                  (Transition) ScreenTransitions.FadeIn);
              break;
            }
            this._state = MainMenuScreen.State.ShowMainMenu;
            break;

          case MainMenuScreen.State.ShowMainMenu:
            this._introPicture.FadeInBackground();
            this._capPressAnyKey.Visible = false;
            this.InputController.ResetTimeIdle();
            this._state = MainMenuScreen.State.MainMenu;
            this._mnuMain.Show();
            break;
        }
      }
    }

    private void MainMenuScreen_OnGameplayModeChanged(IUIControl sender)
    {
      //if (BrainGame.IsTrial)
      //  return;
      this._btnPurchase.Visible = false;
      BrainGame.AchievementsManager.Notify(47);
    }

    private void MainMenuScreen_OnOpenTransitionEnded(IUIControl sender)
    {
      if (this._startType != MainMenuScreen.StartupType.TitleAndMenuVisible)
        return;
      this.EnableInput();
      this._mnuMain.SetFocusOnLastSelectedItem();
    }

    private void MainMenuScreen_OnPopupClosed(IUIControl sender) => this.EnableInput();

    private void MainMenu_OnMenuShownBegin(IUIControl sender)
    {
      if (!this.ShowPurchaseButton)
        return;
      this._btnPurchase.Show();
    }

    private void MainMenu_OnMenuShown(IUIControl sender)
    {
      this.EnableInput();
      this._mnuMain.SetFocusOnLastSelectedItem();
    }

    private void MainMenu_OnMenuHideBegin(IUIControl sender) => this._btnPurchase.Hide();

    private void MainMenu_OnNewGame(IUIControl sender)
    {
      if (Game1.ProfilesManager.CurrentProfile.HasStartedNewGame)
      {
        if (Game1.ThemeMusic != null && Game1.ThemeMusic.IsPlaying)
          BrainGame.MusicManager.FadeMusic(0.0f, 500);

        this.Navigator.GlobalCache.Set("SELECTED_STAGE_INFO", 
            (object) Levels.CurrentLevel.GetCurrentStageInfo());

        this.Navigator.GlobalCache.Get<bool>("STAGE_START_SHOW_XBOX_HELP", true);

        this.NavigateTo(ScreenGroupType.InGame.ToString(), ScreenType.StageStart.ToString(), 
            (Transition) ScreenTransitions.LeafsClosing, (Transition) null);
      }
      else
      {
        this.Navigator.GlobalCache.Set("AUTO_SELECT_STAGE", (object) false);
        this.NavigateTo(ScreenType.ThemeSelection.ToString(), (Transition) ScreenTransitions.LeafsClosing,
            (Transition) ScreenTransitions.LeafsOpening);
      }
    }

    private void MainMenu_OnThemeSelection(IUIControl sender)
    {
      this.Navigator.GlobalCache.Set("AUTO_SELECT_STAGE", (object) false);
      this.NavigateTo(ScreenType.ThemeSelection.ToString(), (Transition) ScreenTransitions.LeafsClosing, (Transition) ScreenTransitions.LeafsOpening);
    }

    private void MainMenu_OnOptions(IUIControl sender)
    {
      this.Navigator.GlobalCache.Set("INTRO_PICTURE_STATE", (object) this._introPicture.GetSaveState());
      this.Navigator.GlobalCache.Set("OPTIONS_STARTUP_MODE", (object) OptionsScreen.StartupType.MenuHidden);
      this.NavigateTo(ScreenType.Options.ToString(), (Transition) null, (Transition) null);
    }

    protected void MainMenu_OnCredits(IUIControl sender)
    {
      this.Navigator.GlobalCache.Set("CREDITS_SCREEN_CALLER", (object) ScreenType.MainMenu);
      this.NavigateTo(ScreenType.Credits.ToString(), (Transition) ScreenTransitions.LeafsClosing,
          (Transition) ScreenTransitions.LeafsOpening);
    }

    private void MainMenu_OnQuit(IUIControl sender) => this.QuitGame();

    private void TimerShowMenu_OnTimer(IUIControl sender) => this._mnuMain.Show();

    private void QuitGame()
    {
      this.NavigateTo(ScreenType.Quit.ToString(), (Transition) ScreenTransitions.FadeOut, (Transition) null);
    }

    private void MenuConfirm_OnYes(IUIControl sender) => this.QuitGame();

    private void btnPurchase_OnClick(IUIControl sender) => Game1.Instance.PurchaseGame();

    private void OptionsMenu_OnAchievements(IUIControl sender)
    {
      this.NavigateTo(ScreenType.Awards.ToString(), (Transition) ScreenTransitions.LeafsClosing, 
          (Transition) ScreenTransitions.LeafsOpening);
    }

    protected enum State
    {
      PressAnyKey,
      MainMenu,
      QuitMenu,
      IntroPicCursorIdle,
      HiddingMenuCursorIdle,
      AssyncronousLoad,
      HowToPlay,
      ShowMainMenu,
    }

    public enum StartupType
    {
      IntroPicture,
      AllHidden,
      TitleVisibleMenuHidden,
      TitleAndMenuVisible,
    }
  }
}
