
// Type: TwoBrainsGames.Snails.Screens.StageStartScreen
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.Snails.Screens.Transitions;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.Screens
{
  internal class StageStartScreen(ScreenNavigator owner) : SnailsScreen(owner, ScreenType.StageStart)
  {
    protected UISnailsMenuTitle _title;
    protected UISnailsBoard _board;
    protected UISnailsStageGoalIcon _goalIcon;
    protected UISnailsThemeIcon _themeIcon;
    protected UIValuedCaption _capStageNr;
    protected UIValuedCaption _capMode;
    protected UIValuedCaption _capTotalSnails;
    protected UIValuedCaption _capTime;
    protected UIValuedCaption _capToDeliver;
    protected UISnailsButton _btnStageSelection;
    protected UISnailsButton _btnStart;
    protected UISnailsButton _btnMainMenu;
    protected UISnailsButton _btnLoadSolution;
    protected UISnailsButton _btnStartAndSaveSolution;
    private UIXBoxControls _xboxController;
    private Vector2 _label4Pos;
    private Vector2 _label5Pos;

    private StageStartScreen.ScreenState State { get; set; }

    private LevelStage LevelStageInfo { get; set; }

    private float LineSpacing { get; set; }

    private bool ShowStageBriefing
    {
      get => this.Navigator.GlobalCache.Get<bool>("STAGE_START_SHOW_STAGE_INFO", false);
    }

    private bool ShowXBoxHelp
    {
      get => this.Navigator.GlobalCache.Get<bool>("STAGE_START_SHOW_XBOX_HELP", false);
    }

    public override void OnLoad()
    {
      base.OnLoad();
      this.BackgroundImage = (Sprite) null;
      this.Name = "StageStart";
      this.OnInitializeFromContent += new UIControl.UIEvent(this.StageStartScreen_OnInitializeFromContent);
      this.OnAfterInitializeFromContent += new UIControl.UIEvent(this.StageStartScreen_OnAfterInitializeFromContent);
      this.BackgroundType = SnailsScreen.ScreenBackgroundType.Leafs;
      this._board = new UISnailsBoard((UIScreen) this, UISnailsBoard.BoardType.LightWoodMediumLong);
      this._board.Name = "_board";
      this._board.ParentAlignment = AlignModes.Horizontaly;
      this._board.Position = new Vector2(0.0f, 3000f);
      this._board.OnHide += new UIControl.UIEvent(this._board_OnHide);
      this._board.Size = new Size(this._board.Size.Width, this._board.Size.Height + 1500f);
      this.Controls.Add((UIControl) this._board);
      this._title = new UISnailsMenuTitle((UIScreen) this);
      this._title.Name = "_title";
      this._title.TextResourceId = "TITLE_STAGE_BRIEFING";
      this._title.BoardSize = UISnailsMenuTitle.TitleSize.Big;
      this._board.Controls.Add((UIControl) this._title);
      this._themeIcon = new UISnailsThemeIcon((UIScreen) this);
      this._themeIcon.Name = "_themeIcon";
      this._board.Controls.Add((UIControl) this._themeIcon);
      this._capStageNr = new UIValuedCaption((UIScreen) this, "LBL_BRIEFING_STAGE", (object) 0, Color.White, Color.White, UICaption.CaptionStyle.StageStartBoardCaptions, 0.0f, false);
      this._capStageNr.Name = "_capStageNr";
      this._capStageNr.Mode = UIValuedCaption.CaptionMode.Simple;
      this._capStageNr.AnimateValue = false;
      this._board.Controls.Add((UIControl) this._capStageNr);
      this._capMode = new UIValuedCaption((UIScreen) this, "LBL_BRIEFING_MODE", (object) 0, Color.White, Color.White, UICaption.CaptionStyle.StageStartBoardCaptions, 0.0f, false);
      this._capMode.Mode = UIValuedCaption.CaptionMode.Simple;
      this._capMode.AnimateValue = false;
      this._board.Controls.Add((UIControl) this._capMode);
      this._goalIcon = new UISnailsStageGoalIcon((UIScreen) this);
      this._goalIcon.IconSize = UISnailsStageGoalIcon.GoalIconSize.Small;
      this._board.Controls.Add((UIControl) this._goalIcon);
      this._capTotalSnails = new UIValuedCaption((UIScreen) this, "LBL_BRIEFING_TOTAL_SNAILS", (object) 0, Color.White, Color.White, UICaption.CaptionStyle.StageStartBoardCaptions, 0.0f, false);
      this._capTotalSnails.Mode = UIValuedCaption.CaptionMode.Simple;
      this._capTotalSnails.AnimateValue = false;
      this._board.Controls.Add((UIControl) this._capTotalSnails);
      this._capToDeliver = new UIValuedCaption((UIScreen) this, "LBL_BRIEFING_TO_DELIVER", (object) 0, Color.White, Color.White, UICaption.CaptionStyle.StageStartBoardCaptions, 0.0f, false);
      this._capToDeliver.Mode = UIValuedCaption.CaptionMode.Simple;
      this._capToDeliver.AnimateValue = false;
      this._board.Controls.Add((UIControl) this._capToDeliver);
      this._capTime = new UIValuedCaption((UIScreen) this, "LBL_BRIEFING_REF_TIME", (object) 0, Color.White, Color.White, UICaption.CaptionStyle.StageStartBoardCaptions, 0.0f, false);
      this._capTime.Mode = UIValuedCaption.CaptionMode.Simple;
      this._capTime.AnimateValue = false;
      this._board.Controls.Add((UIControl) this._capTime);
      this._asyncLoad.OnAsyncOperationEnded += new UIControl.UIEvent(this._asyncLoad_OnAsyncOperationEnded);
      this._btnStart = new UISnailsButton((UIScreen) this, "BNT_START", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, new UIControl.UIEvent(this.btnStart_OnClick), false);
      this._btnStart.Name = "_btnStart";
      this._btnStart.ParentAlignment = AlignModes.Bottom;
      this._btnStart.OnShow += new UIControl.UIEvent(this.btnStart_OnShow);
      this._btnStart.ButtonAction = UISnailsButton.ButtonActionType.Start;
      this._board.Controls.Add((UIControl) this._btnStart);
      this._btnStageSelection = new UISnailsButton((UIScreen) this, "BTN_STAGE_SELECTION", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.Back, new UIControl.UIEvent(this.btnStageSelection_OnBack), false);
      this._btnStageSelection.Name = "_btnStageSelection";
      this._btnStageSelection.ParentAlignment = AlignModes.Bottom;
      this._btnStageSelection.ButtonAction = UISnailsButton.ButtonActionType.Back;
      this._board.Controls.Add((UIControl) this._btnStageSelection);
      this._btnMainMenu = new UISnailsButton((UIScreen) this, "BTN_MAIN_MENU", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, new UIControl.UIEvent(this.btnMainMenu_OnClick), false);
      this._btnMainMenu.Name = "_btnMainMenu";
      this._btnMainMenu.ParentAlignment = AlignModes.Bottom;
      this._btnMainMenu.ButtonAction = UISnailsButton.ButtonActionType.MainMenu;
      this._board.Controls.Add((UIControl) this._btnMainMenu);
      this._btnLoadSolution = new UISnailsButton((UIScreen) this, "BTN_LOAD_SOLUTION", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, new UIControl.UIEvent(this.btnLoadSolution_OnClick), false);
      this._btnLoadSolution.Name = "_btnLoadSolution";
      this._btnLoadSolution.ParentAlignment = AlignModes.Bottom;
      this._btnStartAndSaveSolution = new UISnailsButton((UIScreen) this, "BTN_START_AND_SAVE_SOLUTION", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, new UIControl.UIEvent(this.btnStartAndSaveSolution_OnClick), false);
      this._btnStartAndSaveSolution.Name = "_btnStartAndSaveSolution";
      this._btnStartAndSaveSolution.ParentAlignment = AlignModes.Bottom;
      if (BrainGame.Settings.Platform != BrainSettings.PlaformType.XBox)
        return;
      this._xboxController = new UIXBoxControls((UIScreen) this);
      this._xboxController.Position = new Vector2(0.0f, 3200f);
      this._xboxController.ParentAlignment = AlignModes.Horizontaly;
      this._xboxController.OnDismiss += new UIControl.UIEvent(this._xboxController_OnDismiss);
      this._xboxController.OnDismissButtonShown += new UIControl.UIEvent(this._xboxController_OnDismissButtonShown);
      this._xboxController.ButtonCaptionType = UISnailsWindow.ButtonType.Start;
      this.Controls.Add((UIControl) this._xboxController);
    }

    private void StageStartScreen_OnInitializeFromContent(IUIControl sender)
    {
      this.LineSpacing = this.GetContentPropertyValue<float>("lineSpacing", this.LineSpacing);
    }

    private void StageStartScreen_OnAfterInitializeFromContent(IUIControl sender)
    {
      this._capStageNr.UpdateLayout();
      this._capMode.Position = this._capStageNr.Position + new Vector2(0.0f, this.LineSpacing);
      this._capTotalSnails.Position = this._capMode.Position + new Vector2(0.0f, this.LineSpacing);
      this._capToDeliver.Position = this._capTotalSnails.Position + new Vector2(0.0f, this.LineSpacing);
      this._capTime.Position = this._capToDeliver.Position + new Vector2(0.0f, this.LineSpacing);
      this._label4Pos = this._capToDeliver.Position;
      this._label5Pos = this._capTime.Position;
      this._capMode.Width = this._capStageNr.Width;
      this._capTotalSnails.Width = this._capStageNr.Width;
      this._capToDeliver.Width = this._capStageNr.Width;
      this._capTime.Width = this._capStageNr.Width;
      this._capMode.UpdateLayout();
      this._goalIcon.Position = new Vector2((float) ((double) this._capMode.Position.X + (double) this._capMode.Width + 100.0), this._capMode.ValuePosition.Y - 100f);
    }

    private void btnStart_OnShow(IUIControl sender)
    {
      this.EnableInput();
      this.InstructionBar.HideAllLabels();
      this.InstructionBar.ShowLabel(UIInstructionLabel.LabelActionTypes.Start);
      this.InstructionBar.ShowLabel(UIInstructionLabel.LabelActionTypes.Back);
      this._btnStart.Focus();
    }

    public override void OnStart()
    {
      base.OnStart();
      this.DisableInput();
      ThemeType currentTheme = Levels.CurrentTheme;
      int currentStageNr = Levels.CurrentStageNr;
      this.LevelStageInfo = this.Navigator.GlobalCache.Get<LevelStage>("SELECTED_STAGE_INFO", (LevelStage) null);
      if (this.LevelStageInfo == null)
      {
        ThemeType startupTheme = Game1.GameSettings.StartupTheme;
        int startupStageNr = Game1.GameSettings.StartupStageNr;
        if (Levels.CurrentLevel == null)
          throw new SnailsException("Levels not loaded! It should be loaded GameplayScreen.LoadContent()");
        this.LevelStageInfo = Levels.CurrentLevel.GetLevelStage(startupTheme, startupStageNr);
        Levels.CurrentStageNr = startupStageNr;
        Levels.CurrentTheme = startupTheme;
        Levels.CurrentCustomStageFilename = (string) null;
      }
      else
      {
        Levels.CurrentStageNr = this.LevelStageInfo.StageNr;
        Levels.CurrentTheme = this.LevelStageInfo.ThemeId;
        Levels.CurrentCustomStageFilename = this.LevelStageInfo.CustomStageFilename;
      }
      if (Game1.GameSettings.UseAsyncLoading)
        this.LoadStageAsync();
      else
        this.LoadStageSync();
      if (this._xboxController != null)
        this._xboxController.Visible = false;
      this._board.Visible = false;
      if (this.ShowXBoxHelp && this._xboxController != null)
      {
        this._xboxController.DismissButtonVisible = false;
        this._xboxController.Show();
      }
      else if (this.ShowStageBriefing)
      {
        this.InitializeLabels();
        this._board.Show();
      }
      Color themeCaption = Colors.ThemeCaptions[(int) Levels.CurrentTheme];
      Color themeValue = Colors.ThemeValues[(int) Levels.CurrentTheme];
      this._capStageNr.CaptionColor = themeCaption;
      this._capMode.CaptionColor = themeCaption;
      this._capTotalSnails.CaptionColor = themeCaption;
      this._capTime.CaptionColor = themeCaption;
      this._capToDeliver.CaptionColor = themeCaption;
      this._capStageNr.ValueColor = themeValue;
      this._capMode.ValueColor = themeValue;
      this._capTotalSnails.ValueColor = themeValue;
      this._capTime.ValueColor = themeValue;
      this._capToDeliver.ValueColor = themeValue;
      this._btnStart.Visible = false;
      this._btnStageSelection.Visible = false;
      this._btnMainMenu.Visible = false;
      this._btnLoadSolution.Visible = false;
      this._btnStartAndSaveSolution.Visible = false;
    }

    private void InitializeLabels()
    {
      this._goalIcon.Goal = this.LevelStageInfo._goal;
      this._themeIcon.Theme = this.LevelStageInfo.ThemeId;
      this._capStageNr.Value = this.LevelStageInfo.IsCustomStage ? (object) "-" : (object) this.LevelStageInfo.StageNr;
      this._capMode.Value = (object) Formater.FormatModeName(this.LevelStageInfo._goal);
      this._capTime.Value = (object) this.LevelStageInfo._targetTime;
      this._capTime.Text = this.LevelStageInfo._goal == GoalType.TimeAttack ? LanguageManager.GetString("LBL_BRIEFING_TIME_LIMIT") : LanguageManager.GetString("LBL_BRIEFING_REF_TIME");
      this._capTotalSnails.Value = (object) this.LevelStageInfo._snailsToRelease;
      this._capToDeliver.Value = (object) this.LevelStageInfo._snailsToSave;
      if (this.LevelStageInfo._goal != GoalType.SnailKing)
      {
        this._capToDeliver.Text = this.LevelStageInfo._goal == GoalType.SnailKiller ? LanguageManager.GetString("LBL_BRIEFING_TO_KILL") : LanguageManager.GetString("LBL_BRIEFING_TO_DELIVER");
        this._capToDeliver.Visible = true;
        this._capToDeliver.Position = this._label4Pos;
        this._capTime.Position = this._label5Pos;
      }
      else
      {
        this._capToDeliver.Visible = false;
        this._capTime.Position = this._label4Pos;
      }
    }

    private void btnMainMenu_OnClick(IUIControl sender)
    {
      this.State = StageStartScreen.ScreenState.MainMenu;
      this.DisableInput();
      this._board.Hide();
    }

    private void btnStart_OnClick(IUIControl sender) => this.StartStage(false, false);

    private void btnStageSelection_OnBack(IUIControl sender)
    {
      this.State = StageStartScreen.ScreenState.GoBack;
      this.DisableInput();
      this._board.Hide();
    }

    private void btnLoadSolution_OnClick(IUIControl sender) => this.StartStage(true, false);

    private void btnStartAndSaveSolution_OnClick(IUIControl sender) => this.StartStage(false, true);

    private void _board_OnHide(IUIControl sender)
    {
      switch (this.State)
      {
        case StageStartScreen.ScreenState.GoBack:
          this.Navigator.GlobalCache.Set("AUTO_SELECT_STAGE", (object) true);
          this.NavigateTo("MainMenu", ScreenType.ThemeSelection.ToString(), (Transition) ScreenTransitions.LeafsClosed, (Transition) ScreenTransitions.LeafsOpening);
          break;
        case StageStartScreen.ScreenState.StartGame:
          if (Game1.ProfilesManager.CurrentProfile != null)
          {
            LevelStage currentLevelStage = Levels.CurrentLevelStage;
            if (currentLevelStage != null && !currentLevelStage.IsCustomStage)
            {
              Game1.ProfilesManager.CurrentProfile.LastPlayedStageNr = Levels.CurrentStageNr;
              Game1.ProfilesManager.CurrentProfile.LastPlayedTheme = Levels.CurrentTheme;
              Game1.ProfilesManager.Save();
            }
          }
          this.StartGame();
          break;
        case StageStartScreen.ScreenState.MainMenu:
          this.Navigator.GlobalCache.Set("MAIN_SCREEN_STARTUP_MODE", (object) MainMenuScreen.StartupType.TitleAndMenuVisible);
          this.NavigateTo("MainMenu", ScreenType.MainMenu.ToString(), (Transition) ScreenTransitions.LeafsClosed, (Transition) ScreenTransitions.LeafsOpening);
          break;
      }
    }

    private void _xboxController_OnDismiss(IUIControl sender)
    {
      if (this.ShowStageBriefing)
        this._board.Show();
      this.StartGame();
    }

    private void _xboxController_OnDismissButtonShown(IUIControl sender)
    {
      this.EnableInput();
      this._xboxController.Focus();
    }

    private void StartStage(bool loadSolution, bool saveSolution)
    {
      BrainGame.ScreenNavigator.GlobalCache.Set("GAMEPLAY_RECORDER_LOAD_SOLUTION", (object) loadSolution);
      BrainGame.ScreenNavigator.GlobalCache.Set("GAMEPLAY_RECORDER_SAVE_SOLUTION", (object) saveSolution);
      this.State = StageStartScreen.ScreenState.StartGame;
      this.DisableInput();
      this._board.Hide();
      if (this._btnStartAndSaveSolution.Visible)
        this._btnStartAndSaveSolution.Hide();
      if (!this._btnLoadSolution.Visible)
        return;
      this._btnLoadSolution.Hide();
    }

    private void LoadStageAsync()
    {
      BrainGame.IsLoading = true;
      BrainGame.GameCursor.SetCursor(1);
      this._asyncLoad.ClearOperations();
      this._asyncLoad.AddOperation((IAsyncOperation) Levels._instance);
      if (!Game1.Tutorial._loaded)
        this._asyncLoad.AddOperation((IAsyncOperation) Game1.Tutorial);
      this._asyncLoad.StartLoad();
    }

    private void LoadStageSync()
    {
      Levels._instance.BeginLoad();
      Game1.Tutorial.BeginLoad();
      this.LoadEnded();
    }

    private void _asyncLoad_OnAsyncOperationEnded(IUIControl sender)
    {
      BrainGame.IsLoading = false;
      this.LoadEnded();
    }

    private void LoadEnded()
    {
      BrainGame.GameCursor.SetCursor(0);
      if (this.ShowStageBriefing)
      {
        this._btnStart.Show();
        this._btnStageSelection.Show();
        this._btnMainMenu.Show();
        Levels.CurrentLevel.StageSound.PlayMusic();
      }
      else if (this.ShowXBoxHelp && this._xboxController != null)
      {
        this._xboxController.ShowDismissButton();
      }
      else
      {
        if (this.ShowXBoxHelp)
          return;
        this.StartGame();
      }
    }

    private void StartGame()
    {
      this.NavigateTo(ScreenType.Gameplay.ToString(), (Transition) null, (Transition) ScreenTransitions.LeafsOpening);
    }

    private enum ScreenState
    {
      None,
      GoBack,
      StartGame,
      MainMenu,
    }
  }
}
