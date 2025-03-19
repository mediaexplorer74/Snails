
// Type: TwoBrainsGames.Snails.Screens.ThemeSelectionLDScreen
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
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.Snails.Screens.ThemeSelection;
using TwoBrainsGames.Snails.Screens.Transitions;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.Screens
{
  internal class ThemeSelectionLDScreen(ScreenNavigator navigator) : SnailsScreen(navigator, ScreenType.ThemeSelection)
  {
    public const int THEME_COUNT = 4;
    private ThemeSelectionLDScreen.ScreenState _state;
    private UISnailsMenuTitle _title;
    private UIThemeScrollablePanel _themesPanel;
    private UIStagesPanelLD _stagesPanel;
    private UIStageInfo _stageInfo;
    private LevelStage _lastLevelStage;
    private UIBackButton _btnBack;
    private Sample _stageSelectSound;

    private Levels Levels { get; set; }

    private bool StageAutoselected
    {
      get => this.Navigator.GlobalCache.Get<bool>("AUTO_SELECT_STAGE", false);
      set => this.Navigator.GlobalCache.Set("AUTO_SELECT_STAGE", (object) value);
    }

    public override void OnLoad()
    {
      base.OnLoad();
      this.BackgroundImageBlendColor = Colors.ThemeSelectionScrBkColor;
      this.Levels = Levels.Load();
      this._title = new UISnailsMenuTitle((UIScreen) this);
      this._title.TextResourceId = "TITLE_STAGE_SELECTION";
      this._title.ParentAlignment = AlignModes.Horizontaly;
      this._title.BoardSize = UISnailsMenuTitle.TitleSize.Big;
      this._title.Position = new Vector2(0.0f, 500f);
      this._title.ShowEffect = (TransformEffectBase) new SquashEffect(0.85f, 4f, 0.04f, this.BlendColor, new Vector2(1f, 1f));
      this.Controls.Add((UIControl) this._title);
      this._themesPanel = new UIThemeScrollablePanel((UIScreen) this);
      this._themesPanel.Position = new Vector2(200f, 3100f);
      this._themesPanel.OnThemeSelectedStarted += new UIControl.UIEvent(this._themesPanel_OnThemeSelectedStarted);
      this._themesPanel.OnThemeSelected += new UIControl.UIEvent(this._themesPanel_OnThemeSelected);
      this._themesPanel.OnShow += new UIControl.UIEvent(this._themesPanel_OnShow);
      this._themesPanel.OnCancelEnded += new UIControl.UIEvent(this._themesPanel_OnCancelEnded);
      this.Controls.Add((UIControl) this._themesPanel);
      this._stagesPanel = new UIStagesPanelLD((UIScreen) this);
      this._stagesPanel.Name = "_stagesPanel";
      this._stagesPanel.Visible = false;
      this._stagesPanel.OnShow += new UIControl.UIEvent(this._stagesPanel_OnShow);
      this._stagesPanel.OnHide += new UIControl.UIEvent(this._stagesPanel_OnHide);
      this._stagesPanel.OnStageSelected = new UIControl.UIEvent(this.StagesPanel_OnStageSelected);
      this._stagesPanel.OnStageDoubleSelected = new UIControl.UIEvent(this.StagesPanel_OnStageDoubleSelected);
      this._stagesPanel.OnBack += new UIControl.UIEvent(this._stagesPanel_OnBack);
      this._stagesPanel.ParentAlignment = AlignModes.Right;
      this._stagesPanel.Margins.Right = 130f;
      this._stagesPanel.Position = new Vector2(0.0f, 2600f);
      this._stagesPanel.Size = new Size(4450f, 7000f);
      this.Controls.Add((UIControl) this._stagesPanel);
      this._stageInfo = new UIStageInfo((UIScreen) this);
      this._stageInfo.Position = new Vector2(100f, 2450f);
      this._stageInfo.Visible = false;
      this._stageInfo.AcceptControllerInput = true;
      this._stageInfo.OnInfoFilled += new UIControl.UIEvent(this._stageInfo_OnInfoFilled);
      this._stageInfo.OnAccept += new UIControl.UIEvent(this._stageInfo_OnAccept);
      this.Controls.Add((UIControl) this._stageInfo);
      this._btnBack = new UIBackButton((UIScreen) this);
      this._btnBack.ScreenAlignment = UIBackButton.ButtonScreenAlignment.BottomLeft;
      this._btnBack.OnPress += new UIControl.UIEvent(this.btnBack_OnPress);
      this.Controls.Add((UIControl) this._btnBack);
      this.ShowTrialTag = true;
      this.OnBack += new UIControl.UIEvent(this.btnBack_OnPress);
      this.OnOpenTransitionEnded += new UIControl.UIEvent(this.ThemeSelectionScreenLD_OnOpenTransitionEnded);
      this._stageSelectSound = BrainGame.ResourceManager.GetSampleStatic("sfx/menu-item-selected");
      this.BackgroundType = SnailsScreen.ScreenBackgroundType.Image;
    }

    public override void OnUnload()
    {
      base.OnUnload();
      BrainGame.ResourceManager.Unload("STAGE_THUMBNAILS");
    }

    public override void OnStart()
    {
      base.OnStart();
      this.DisableInput();
      this._themesPanel.Initialize();
      this._stagesPanel.ResetPanel();
      this._themesPanel.Visible = true;
      this._stagesPanel.Visible = false;
      this._stageInfo.Visible = false;
      this._btnBack.Visible = true;
      this._title.Visible = true;
      if (this._themesPanel.AncientEgyptTheme.Locked)
        this._themesPanel.AncientEgyptTheme.UpdateUnlockGoal();
      if (this._themesPanel.BotFactoryTheme.Locked)
        this._themesPanel.BotFactoryTheme.UpdateUnlockGoal();
      if (this._themesPanel.OuterSpaceTheme.Locked)
        this._themesPanel.OuterSpaceTheme.UpdateUnlockGoal();
      if (this.StageAutoselected)
      {
        int currentStageNr = Levels.CurrentStageNr;
        ThemeType currentTheme = Levels.CurrentTheme;
        this._themesPanel.SelectThemeWithoutAnimations(currentTheme);
        this._themesPanel.Visible = false;
        this._stagesPanel.SetTheme(this.Levels, currentTheme);
        this._stagesPanel.Visible = true;
        this._stagesPanel.Enabled = true;
        this._stagesPanel.Focus(currentStageNr);
        this._stagesPanel.AcceptControllerInput = true;
        this._stagesPanel.SelectStage(Levels.CurrentLevelStage);
        this._stageInfo.Visible = true;
        this._lastLevelStage = Levels.CurrentLevelStage;
        this._stageInfo.Initialize(Levels.CurrentLevelStage);
        this._state = ThemeSelectionLDScreen.ScreenState.StageSelection;
      }
      this.EnableInput();
      if (BrainGame.MusicManager.IsMusicActive)
        return;
      Game1.ThemeMusic.Play(true);
    }

    private void ThemeSelectionScreenLD_OnOpenTransitionEnded(IUIControl sender)
    {
      int num = this.StageAutoselected ? 1 : 0;
    }

    private void _stageInfo_OnAccept(IUIControl sender)
    {
      if (this._lastLevelStage == null)
        return;
      this._stageSelectSound.Play();
      this.StartSelectedStage();
    }

    private void btnBack_OnPress(IUIControl sender)
    {
      switch (this._state)
      {
        case ThemeSelectionLDScreen.ScreenState.ThemeSelection:
          this.DisableInput();
          this.Navigator.GlobalCache.Set("MAIN_SCREEN_STARTUP_MODE", (object) MainMenuScreen.StartupType.TitleAndMenuVisible);
          this.NavigateTo("MainMenu", (Transition) ScreenTransitions.LeafsClosing, (Transition) ScreenTransitions.LeafsOpening);
          break;
        case ThemeSelectionLDScreen.ScreenState.StageSelection:
          this.DisableInput();
          this._stageInfo.Hide();
          this._stagesPanel.Hide();
          this.StageAutoselected = false;
          this._state = ThemeSelectionLDScreen.ScreenState.ThemeSelection;
          int currentTheme = (int) Levels.CurrentTheme;
          int num;
          if (currentTheme < 2)
          {
            num = 0;
            break;
          }
          if (currentTheme < 2 || currentTheme >= 4)
            break;
          num = 1;
          break;
      }
    }

    private void _themesPanel_OnShow(IUIControl sender)
    {
    }

    private void _themesPanel_OnCancelEnded(IUIControl sender)
    {
      this.EnableInput();
      this._themesPanel.Focus();
      this._themesPanel.AcceptControllerInput = true;
    }

    private void _themesPanel_OnThemeSelectedStarted(IUIControl sender) => this.DisableInput();

    private void _themesPanel_OnThemeSelected(IUIControl sender)
    {
      if (Levels.CurrentTheme != this._themesPanel.SelectedTheme.ThemeId)
        this._stagesPanel.ScrollToTop();
      Levels.CurrentTheme = this._themesPanel.SelectedTheme.ThemeId;
      this._themesPanel.Enabled = false;
      this._themesPanel.Visible = false;
      this._lastLevelStage = (LevelStage) null;
      this._stageInfo.Show();
      this._stagesPanel.Show(this.Levels, this._themesPanel.SelectedTheme.ThemeId);
    }

    private void _stagesPanel_OnHide(IUIControl sender)
    {
      this._themesPanel.Visible = true;
      this._themesPanel.CancelSelection();
      this._themesPanel.Enabled = true;
    }

    private void _stagesPanel_OnShow(IUIControl sender)
    {
      this.EnableInput();
      this._stagesPanel.FocusOnLastUnlocked();
      this._state = ThemeSelectionLDScreen.ScreenState.StageSelection;
    }

    private void StagesPanel_OnStageDoubleSelected(IUIControl sender)
    {
      UIStage uiStage = (UIStage) sender;
      if (Game1.ThemeMusic != null && Game1.ThemeMusic.IsPlaying)
        BrainGame.MusicManager.FadeMusic(0.0f, 500);
      if (uiStage.Locked && BrainGame.IsTrial && !uiStage.LevelStageInfo.AvailableInDemo && Game1.GameSettings.WithAppStore)
      {
        this.NavigateToPurchase();
      }
      else
      {
        if (uiStage.Locked)
          return;
        this.DisableInput();
        this._stagesPanel.RaiseStageLeaveEvent = false;
        uiStage.DoOnLeaveEffect = false;
        this.StartStage(uiStage.StageNr, uiStage.LevelStageInfo);
      }
    }

    private void StagesPanel_OnStageSelected(IUIControl sender)
    {
      UIStage uiStage = (UIStage) sender;
      LevelStage levelStage = this.Levels.GetLevelStage(this._themesPanel.SelectedTheme.ThemeId, uiStage.StageNr);
      if (!levelStage.AvailableInDemo && uiStage.Locked)
      {
        if (!Game1.GameSettings.WithAppStore)
          return;
        this.NavigateToPurchase();
      }
      else if (this._lastLevelStage == null || this._lastLevelStage != null && (this._lastLevelStage.StageNr != levelStage.StageNr || this._lastLevelStage.ThemeId != levelStage.ThemeId))
      {
        this._lastLevelStage = levelStage;
        uiStage.LevelStageInfo = levelStage;
        this._stageInfo.Initialize(levelStage);
        Levels.CurrentStageNr = uiStage.StageNr;
        Levels.CurrentLevelStage = this._lastLevelStage;
      }
      else
        this.StartSelectedStage();
    }

    private void _stageInfo_OnButtonClicked(IUIControl sender) => this.StartSelectedStage();

    private void StartStage(int stageNr, LevelStage levelStage)
    {
      Levels.CurrentStageNr = stageNr;
      this.Navigator.GlobalCache.Set("SELECTED_STAGE_INFO", (object) levelStage);
      this.Navigator.GlobalCache.Set("STAGE_START_SHOW_STAGE_INFO", (object) false);
      this.Navigator.GlobalCache.Set("STAGE_START_SHOW_XBOX_HELP", (object) (BrainGame.Settings.Platform == BrainSettings.PlaformType.XBox));
      this.NavigateTo("InGame", ScreenType.StageStart.ToString(), (Transition) ScreenTransitions.LeafsClosing, (Transition) null);
    }

    private void _stagesPanel_OnStageEnter(IUIControl sender)
    {
      UIStage uiStage = (UIStage) sender;
      if (uiStage.Locked)
        return;
      LevelStage levelStage = this.Levels.GetLevelStage(this._themesPanel.SelectedTheme.ThemeId, uiStage.StageNr);
      uiStage.LevelStageInfo = levelStage;
      this._stageInfo.Initialize(levelStage);
    }

    private void _stagesPanel_OnStageLeave(IUIControl sender)
    {
    }

    private void _stagesPanel_OnBack(IUIControl sender)
    {
    }

    private void _stageInfo_OnInfoFilled(IUIControl sender)
    {
    }

    private void StartSelectedStage()
    {
      BrainGame.MusicManager.FadeMusic(0.0f, 500);
      this.StartStage(this._lastLevelStage.StageNr, this._lastLevelStage);
    }

    private enum ScreenState
    {
      ThemeSelection,
      StageSelection,
    }
  }
}
