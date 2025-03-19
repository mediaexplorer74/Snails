
// Type: TwoBrainsGames.Snails.Screens.InGameOptionsScreen
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;
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
  internal class InGameOptionsScreen(ScreenNavigator owner) : SnailsScreen(owner, ScreenType.InGameOptions)
  {
    private InGameOptionsScreen.ScreenState _state;
    private UISnailsBoard _board;
    private UISnailsTitle _gameTitle;
    private UISnailsMenu _mnuMain;
    private UISnailsMenu _mnuConfirm;
    private UISnailsMenu _mnuOptions;
    private UISoundMenu _mnuSoundSettings;
    private UISnailsMenuItem _itmResumeGame;
    private UISnailsMenuItem _itmToggleFullscreen;
    private UIGoldMedalInfoPanel _goldMedalPanel;
    private UICloseButton _btnClose;
    private UIPanel _pnlMenu;
    private bool _musicActiveWhenOpened;

    public override void OnLoad()
    {
      base.OnLoad();
      this.BackgroundImage = (Sprite) null;
      this.Name = "InGameOptions";
      this._board = new UISnailsBoard((UIScreen) this, UISnailsBoard.BoardType.LeafsMedium);
      this._board.Name = "_board";
      this._board.ParentAlignment = AlignModes.Horizontaly;
      this._board.OnShow += new UIControl.UIEvent(this._board_OnShow);
      this._board.OnHide += new UIControl.UIEvent(this._board_OnHide);
      this._board.OnHideBegin += new UIControl.UIEvent(this._board_OnHideBegin);
      this.Controls.Add((UIControl) this._board);
      this._pnlMenu = new UIPanel((UIScreen) this);
      this._pnlMenu.Name = "_pnlMenu";
      this._pnlMenu.Size = new Size(3000f, 4300f);
      this._board.Controls.Add((UIControl) this._pnlMenu);
      this._mnuMain = new UISnailsMenu((UIScreen) this);
      this._mnuMain.Size = new Size(3000f, 3800f);
      this._mnuMain.TextResourceId = "MNU_GAME_PAUSED";
      this._mnuMain.ParentAlignment = AlignModes.HorizontalyVertically;
      this._mnuMain.OnMenuShown += new UIControl.UIEvent(this._mnuMain_OnMenuShown);
      this._mnuMain.OnItemSelectedBegin += new UIControl.UIEvent(this._mnuMain_OnItemSelectedBegin);
      this._pnlMenu.Controls.Add((UIControl) this._mnuMain);
      if (Game1.GameSettings.ShowContinueOption)
        this._itmResumeGame = this._mnuMain.AddMenuItem("MNU_ITEM_RESUME_GAME", new UIControl.UIEvent(this.Menu_OnReturnToGame), InputBase.InputActions.Back, false);
      this._mnuMain.AddMenuItem("MNU_ITEM_OPTIONS", new UIControl.UIEvent(this.Menu_OnOptions), InputBase.InputActions.None);
      this._mnuMain.AddMenuItem("MNU_ITEM_QUIT_STAGE", new UIControl.UIEvent(this.Menu_OnQuitStage), InputBase.InputActions.None, Game1.GameSettings.ShowConfirmationMenus);
      this._mnuConfirm = new UISnailsMenu((UIScreen) this);
      this._mnuConfirm.Name = "_mnuConfirm";
      this._mnuConfirm.ParentAlignment = AlignModes.HorizontalyVertically;
      this._mnuConfirm.Size = new Size(3000f, 3800f);
      this._mnuConfirm.TitleSize = UISnailsMenuTitle.TitleSize.Big;
      this._mnuConfirm.Visible = false;
      this._mnuConfirm.TextResourceId = "MNU_CONFIRM_QUIT_STAGE";
      this._mnuConfirm.DefaultItemIndex = 1;
      this._mnuConfirm.OnMenuShown += new UIControl.UIEvent(this.ConfirmQuitMenu_OnMenuShown);
      this._mnuConfirm.OnItemSelectedBegin += new UIControl.UIEvent(this._mnuConfirm_OnItemSelectedBegin);
      this._pnlMenu.Controls.Add((UIControl) this._mnuConfirm);
      this._mnuConfirm.AddMenuItem("MNU_ITEM_YES", new UIControl.UIEvent(this.MenuConfirm_OnYes), InputBase.InputActions.None, false);
      this._mnuConfirm.AddMenuItem("MNU_ITEM_NO", new UIControl.UIEvent(this.MenuConfirm_OnNo), InputBase.InputActions.Back);
      this._mnuOptions = new UISnailsMenu((UIScreen) this);
      this._mnuOptions.Name = "_mnuOptions";
      this._mnuOptions.ParentAlignment = AlignModes.HorizontalyVertically;
      this._mnuOptions.Size = new Size(3000f, 3800f);
      this._mnuOptions.TextResourceId = "MNU_ITEM_OPTIONS";
      this._mnuOptions.DefaultItemIndex = 0;
      this._mnuOptions.ItemSize = UISnailsMenu.MenuItemSize.Medium;
      this._mnuOptions.OnMenuShown += new UIControl.UIEvent(this._mnuOptions_OnMenuShown);
      this._mnuOptions.OnItemSelectedBegin += new UIControl.UIEvent(this._mnuOptions_OnItemSelectedBegin);
      this._mnuOptions.OnBackPressed += new UIControl.UIEvent(this.OptionsMenu_OnBackToMain);
      this._mnuOptions.WithBackButton = true;
      this._pnlMenu.Controls.Add((UIControl) this._mnuOptions);
      this._mnuOptions.AddMenuItem("MNU_ITEM_SOUND_SETTINGS", new UIControl.UIEvent(this.OptionsMenu_OnSoundSettings), InputBase.InputActions.None);
      if (Game1.GameSettings.AllowToggleFullScreen)
        this._itmToggleFullscreen = this._mnuOptions.AddMenuItem("", new UIControl.UIEvent(this.OptionsMenu_OnToggleFullscreen), InputBase.InputActions.None, false);
      if (Game1.GameSettings.UseGamepad)
        this._mnuOptions.AddMenuItem("MNU_ITEM_CONTROLS", new UIControl.UIEvent(this.OptionsMenu_OnControlsHelp), InputBase.InputActions.None, false);
      this._mnuOptions.AddMenuItem("MNU_ITEM_HOW_TO_PLAY", new UIControl.UIEvent(this.OptionsMenu_OnHowToPlay), InputBase.InputActions.None, false);
      this._gameTitle = new UISnailsTitle((UIScreen) this);
      this._gameTitle.Position = new Vector2(0.0f, 500f);
      this._gameTitle.ParentAlignment = AlignModes.Horizontaly;
      this._gameTitle.Mode = UISnailsTitle.TitleMode.Log;
      if (Game1.GameSettings.ShowGameTitleInPause)
        this.Controls.Add((UIControl) this._gameTitle);
      this._mnuSoundSettings = new UISoundMenu((UIScreen) this);
      this._mnuSoundSettings.Name = "_mnuSoundSettings";
      this._mnuSoundSettings.OnHide += new UIControl.UIEvent(this._mnuSoundSettings_OnHide);
      this._mnuSoundSettings.OnShow += new UIControl.UIEvent(this._mnuSoundSettings_OnShow);
      this._mnuSoundSettings.ParentAlignment = AlignModes.HorizontalyVertically;
      this._mnuSoundSettings.StopPlayMusic = true;
      this._pnlMenu.Controls.Add((UIControl) this._mnuSoundSettings);
      this._goldMedalPanel = new UIGoldMedalInfoPanel((UIScreen) this);
      this._goldMedalPanel.Name = "_goldMedalPanel";
      this._goldMedalPanel.ParentAlignment = AlignModes.Horizontaly | AlignModes.Bottom;
      this.Controls.Add((UIControl) this._goldMedalPanel);
      this._btnClose = new UICloseButton((UIScreen) this);
      this._btnClose.ParentAlignment = AlignModes.Right | AlignModes.Top;
      this._btnClose.OnPress += new UIControl.UIEvent(this._btnClose_OnPress);
      this._btnClose.Margins.Top = 300f;
      this._btnClose.Margins.Right = 500f;
      this._btnClose.FaceType = UICloseButton.ButtonFaceType.Light;
      if (!Game1.GameSettings.ShowGameTitleInPause)
        this._board.Controls.Add((UIControl) this._btnClose);
      this.OnPopupClosed += new UIControl.UIEvent(this.InGameOptionsScreen_OnPopupClosed);
      this.OnBlurEffectFadeEnded += new EventHandler(this.InGameOptionsScreen_OnBlurEffectFadeEnded);
      this.OnBlurEffectEnded += new EventHandler(this.InGameOptionsScreen_OnBlurEffectEnded);
      this.WithBlurEffect = true;
    }

    public override void OnStart()
    {
      base.OnStart();
      Game1.ProfilesManager.Save();
      this.DisableInput();
      this._gameTitle.Visible = false;
      this._board.Visible = false;
      this._mnuMain.Visible = false;
      this._mnuOptions.Visible = false;
      this._mnuConfirm.Visible = false;
      this._state = InGameOptionsScreen.ScreenState.Idle;
      this.SetToggleScreenModeMenuItemName();
      BrainGame.GameCursor.SetCursor(0);
      this._mnuSoundSettings.Visible = false;
      this._goldMedalPanel.Visible = false;
      this._gameTitle.WithShake = false;
      this._musicActiveWhenOpened = BrainGame.MusicManager.IsMusicActive;
    }

    public override void OnUpdate(BrainGameTime gameTime)
    {
      base.OnUpdate(gameTime);
      if (this._state != InGameOptionsScreen.ScreenState.SoundToggle)
        return;
      if (Game1.GameSettings.UseGamepad)
      {
        if (!this.InputController.ActionBack && !this.InputController.ActionAccept)
          return;
        this.DisableInput();
      }
      else
      {
        if (!this.InputController.ActionBack)
          return;
        this.DisableInput();
      }
    }

    private void InGameOptionsScreen_OnPopupClosed(IUIControl sender) => this.EnableInput();

    private void InGameOptionsScreen_OnBlurEffectFadeEnded(object sender, EventArgs e)
    {
      this.Close();
      Stage.CurrentStage.ResumeGame();
    }

    private void InGameOptionsScreen_OnBlurEffectEnded(object sender, EventArgs e)
    {
      this._gameTitle.Show();
      this._board.Show();
    }

    private void _board_OnShow(IUIControl sender)
    {
      this._state = InGameOptionsScreen.ScreenState.Idle;
      this._mnuMain.Show();
      this._goldMedalPanel.Show();
    }

    private void _board_OnHide(IUIControl sender) => this.FadeBlurOut();

    private void _board_OnHideBegin(IUIControl sender) => this._goldMedalPanel.Hide();

    private void _mnuMain_OnMenuShown(IUIControl sender)
    {
      this.EnableInput();
      if (this.CursorMode != CursorModes.SnapToControl)
        return;
      this._itmResumeGame.Focus();
    }

    private void _mnuMain_OnItemSelectedBegin(IUIControl sender) => this.DisableInput();

    private void Menu_OnReturnToGame(IUIControl sender) => this.ReturnToGame();

    private void Menu_OnOptions(IUIControl sender)
    {
      this.DisableInput();
      this._mnuOptions.Show();
    }

    private void Menu_OnMainMenu(IUIControl sender)
    {
      this.DisableInput();
      if (Game1.GameSettings.ShowConfirmationMenus)
      {
        this._state = InGameOptionsScreen.ScreenState.BackMainMenuConfirmation;
        this._mnuConfirm.TextResourceId = "MNU_CONFIRM_QUIT_STAGE";
        this._mnuConfirm.TitleSize = UISnailsMenuTitle.TitleSize.Medium;
        this._mnuConfirm.Show();
      }
      else
        this.QuitToMainMenu();
    }

    private void Menu_OnQuitStage(IUIControl sender)
    {
      this.DisableInput();
      if (Game1.GameSettings.ShowConfirmationMenus)
      {
        this._state = InGameOptionsScreen.ScreenState.BackToStageSelConfirmation;
        this._mnuConfirm.TextResourceId = "MNU_CONFIRM_QUIT_STAGE";
        this._mnuConfirm.TitleSize = UISnailsMenuTitle.TitleSize.Medium;
        this._mnuConfirm.Show();
      }
      else
        this.ReturnToStageSelection();
    }

    private void Menu_OnRestartStage(IUIControl sender)
    {
      if (Game1.GameSettings.ShowConfirmationMenus)
      {
        this.DisableInput();
        this._state = InGameOptionsScreen.ScreenState.RestartStageConfirmation;
        this._mnuConfirm.TextResourceId = "MNU_CONFIRM_RESTART";
        this._mnuConfirm.TitleSize = UISnailsMenuTitle.TitleSize.Big;
        this._mnuConfirm.Show();
      }
      else
        this.RestartStage();
    }

    private void _mnuOptions_OnItemSelectedBegin(IUIControl sender) => this.DisableInput();

    private void _mnuOptions_OnMenuShown(IUIControl sender)
    {
      this.EnableInput();
      this._state = InGameOptionsScreen.ScreenState.Idle;
      if (this.CursorMode != CursorModes.SnapToControl)
        return;
      this._mnuOptions.SetFocus(0);
    }

    private void OptionsMenu_OnSoundSettings(IUIControl sender)
    {
      this.DisableInput();
      this._state = InGameOptionsScreen.ScreenState.SoundToggle;
      this._mnuSoundSettings.Show();
    }

    private void OptionsMenu_OnControlsHelp(IUIControl sender)
    {
      this.Navigator.PopUp(ScreenType.XBoxControllerHelp.ToString());
      this.EnableInput();
    }

    private void OptionsMenu_OnHowToPlay(IUIControl sender)
    {
      HowToPlayScreen.PopUp(Game1.Tutorial.Topics);
      this.EnableInput();
    }

    private void OptionsMenu_OnBackToMain(IUIControl sender)
    {
      this.DisableInput();
      this._mnuMain.Show();
    }

    private void OptionsMenu_OnToggleFullscreen(IUIControl sender)
    {
      BrainGame.ToggleFullScreen();
      this.SetToggleScreenModeMenuItemName();
      Game1.ProfilesManager.CurrentProfile.Fullscreen = Game1.GameSettings.IsFullScreen;
      Game1.ProfilesManager.Save();
      this.EnableInput();
    }

    private void SetToggleScreenModeMenuItemName()
    {
      if (this._itmToggleFullscreen == null)
        return;
      this._itmToggleFullscreen.TextResourceId = BrainGame.GraphicsManager.IsFullScreen ? "MNU_ITEM_WINDOWED" : "MNU_ITEM_FULLSCREEN";
    }

    private void _mnuConfirm_OnItemSelectedBegin(IUIControl sender) => this.DisableInput();

    private void ConfirmQuitMenu_OnMenuShown(IUIControl sender)
    {
      this.EnableInput();
      if (this.CursorMode != CursorModes.SnapToControl)
        return;
      this._mnuConfirm.SetFocus(1);
    }

    private void QuitToMainMenu()
    {
      BrainGame.ResourceManager.Unload("TUTORIAL");
      this.Navigator.GlobalCache.Set("MAIN_SCREEN_STARTUP_MODE", (object) MainMenuScreen.StartupType.TitleAndMenuVisible);
      this.NavigateTo("MainMenu", ScreenType.MainMenu.ToString(), (Transition) ScreenTransitions.LeafsClosing, (Transition) ScreenTransitions.LeafsOpening);
    }

    private void ReturnToStageSelection()
    {
      Stage.CurrentStage.QuitStage();
      this.Navigator.GlobalCache.Set("AUTO_SELECT_STAGE", (object) true);
      this.NavigateTo("MainMenu", ScreenType.ThemeSelection.ToString(), (Transition) ScreenTransitions.LeafsClosing, (Transition) ScreenTransitions.LeafsOpening);
    }

    private void RestartStage()
    {
      this.Navigator.GlobalCache.Set("STAGE_START_SHOW_STAGE_INFO", (object) false);
      this.Navigator.GlobalCache.Set("STAGE_START_SHOW_XBOX_HELP", (object) false);
      this.NavigateTo(ScreenType.StageStart.ToString(), (Transition) ScreenTransitions.LeafsClosing, (Transition) null);
    }

    private void MenuConfirm_OnYes(IUIControl sender)
    {
      switch (this._state)
      {
        case InGameOptionsScreen.ScreenState.BackToStageSelConfirmation:
          this.ReturnToStageSelection();
          break;
        case InGameOptionsScreen.ScreenState.RestartStageConfirmation:
          this.RestartStage();
          break;
        case InGameOptionsScreen.ScreenState.BackMainMenuConfirmation:
          this.QuitToMainMenu();
          break;
      }
    }

    private void MenuConfirm_OnNo(IUIControl sender)
    {
      this.DisableInput();
      this._mnuMain.Show();
    }

    private void _mnuSoundSettings_OnHide(IUIControl sender) => this._mnuOptions.Show();

    private void _mnuSoundSettings_OnShow(IUIControl sender) => this.EnableInput();

    private void _btnClose_OnPress(IUIControl sender) => this.ReturnToGame();

    private void ReturnToGame()
    {
      if (!this._musicActiveWhenOpened)
        BrainGame.MusicManager.StopMusic();
      else
        BrainGame.MusicManager.ResumeMusic();
      this.DisableInput();
      this._board.Hide();
      this._gameTitle.Hide();
    }

    private enum ScreenState
    {
      Idle,
      ReturnToGame,
      BackToMenu,
      Options,
      BackToStageSelConfirmation,
      RestartStageConfirmation,
      SoundToggle,
      BackMainMenuConfirmation,
    }
  }
}
