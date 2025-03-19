
// Type: TwoBrainsGames.Snails.Screens.ThemeSelection.ThemeSelectionScreen
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
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.Snails.Screens.Transitions;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.Screens.ThemeSelection
{
  internal class ThemeSelectionScreen(ScreenNavigator navigator) : SnailsScreen(navigator, ScreenType.ThemeSelection)
  {
    public const int THEME_COUNT = 4;
    private UIStagesPanel _stagesPanel;
    private UIThemesPanel _themesPanel;
    private UIStageInfo _stageInfo;
    private ThemeSelectionScreen.ScreenState _state;
    private UITimer _tmrShowTitle;
    private UIBackButton _btnBack;
    private UISnailsMenuTitle _title;

    private Levels Levels { get; set; }

    private bool StageAutoselected
    {
      get => this.Navigator.GlobalCache.Get<bool>("AUTO_SELECT_STAGE", false);
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
      this._themesPanel = new UIThemesPanel((UIScreen) this);
      this._themesPanel.Name = "_themesPanel";
      this._themesPanel.OnThemeSelectedStarted += new UIControl.UIEvent(this._themesPanel_OnThemeSelectedStarted);
      this._themesPanel.OnThemeSelected += new UIControl.UIEvent(this._themesPanel_OnThemeSelected);
      this._themesPanel.OnShow += new UIControl.UIEvent(this._themesPanel_OnShow);
      this._themesPanel.OnCancelEnded += new UIControl.UIEvent(this._themesPanel_OnCancelEnded);
      this._themesPanel.ParentAlignment = AlignModes.Horizontaly | AlignModes.Top;
      this._themesPanel.Margins.Top = 1800f;
      this.Controls.Add((UIControl) this._themesPanel);
      this._stagesPanel = new UIStagesPanel((UIScreen) this);
      this._stagesPanel.Name = "_stagesPanel";
      this._stagesPanel.Visible = false;
      this._stagesPanel.OnShow += new UIControl.UIEvent(this._stagesPanel_OnShow);
      this._stagesPanel.OnHide += new UIControl.UIEvent(this._stagesPanel_OnHide);
      this._stagesPanel.OnStageSelected = new UIControl.UIEvent(this.StagesPanel_OnStageSelected);
      this._stagesPanel.OnStageEnter += new UIControl.UIEvent(this._stagesPanel_OnStageEnter);
      this._stagesPanel.OnStageLeave += new UIControl.UIEvent(this._stagesPanel_OnStageLeave);
      this._stagesPanel.OnBack += new UIControl.UIEvent(this._stagesPanel_OnBack);
      this._stagesPanel.ParentAlignment = AlignModes.Horizontaly;
      this._stagesPanel.Position = new Vector2(0.0f, 5800f);
      this.Controls.Add((UIControl) this._stagesPanel);
      this._stageInfo = new UIStageInfo((UIScreen) this);
      this._stageInfo.Position = new Vector2(5700f, 1700f);
      this._stageInfo.Visible = false;
      this._stageInfo.AcceptControllerInput = false;
      this.Controls.Add((UIControl) this._stageInfo);
      this._tmrShowTitle = new UITimer((UIScreen) this, 150.0, false);
      this._tmrShowTitle.Enabled = false;
      this._tmrShowTitle.OnTimer += new UIControl.UIEvent(this._tmrShowTitle_OnTimer);
      this.Controls.Add((UIControl) this._tmrShowTitle);
      if (Game1.GameSettings.ShowBackButtonInThemeSelection)
      {
        this._btnBack = new UIBackButton((UIScreen) this);
        this._btnBack.ScreenAlignment = UIBackButton.ButtonScreenAlignment.BottomLeft;
        this._btnBack.OnAccept += new UIControl.UIEvent(this.btnBack_OnPress);
        this.Controls.Add((UIControl) this._btnBack);
      }
      else
        this.OnBack += new UIControl.UIEvent(this.btnBack_OnPress);
      this.OnOpenTransitionEnded += new UIControl.UIEvent(this.ThemeSelectionScreen_OnOpenTransitionEnded);
      this._state = ThemeSelectionScreen.ScreenState.None;
      this.BackgroundType = SnailsScreen.ScreenBackgroundType.Image;
    }

    public override void OnUnload()
    {
      base.OnUnload();
      BrainGame.ResourceManager.Unload("STAGE_THUMBNAILS");
    }

    public override void OnClose()
    {
      base.OnClose();
      BrainGame.ResourceManager.Unload("STAGE_THUMBNAILS");
    }

    public override void OnStart()
    {
      base.OnStart();
      this.DisableInput();
      this._themesPanel.Initialize();
      this._stagesPanel.Reset();
      this._themesPanel.Visible = false;
      this._stagesPanel.Visible = false;
      this._title.Visible = false;
      this._tmrShowTitle.Enabled = false;
      this._btnBack.Visible = false;
      if (this._themesPanel.AncientEgyptTheme.Locked)
        this._themesPanel.AncientEgyptTheme.UpdateUnlockGoal();
      if (this._themesPanel.BotFactoryTheme.Locked)
        this._themesPanel.BotFactoryTheme.UpdateUnlockGoal();
      if (this._themesPanel.OuterSpaceTheme.Locked)
        this._themesPanel.OuterSpaceTheme.UpdateUnlockGoal();
      if (this.StageAutoselected)
      {
        this.EnableInput();
        int currentStageNr = Levels.CurrentStageNr;
        ThemeType currentTheme = Levels.CurrentTheme;
        this._themesPanel.Visible = true;
        this._title.Visible = true;
        this._btnBack.Visible = true;
        this._themesPanel.AcceptControllerInput = false;
        this._themesPanel.SelectThemeWithoutAnimations(currentTheme);
        this._stagesPanel.SetTheme(this.Levels, currentTheme);
        this._stagesPanel.Visible = true;
        this._stagesPanel.Focus(currentStageNr);
        this._state = ThemeSelectionScreen.ScreenState.StageSelection;
        this.DisableInput();
      }
      else
      {
        this._themesPanel.Show();
        this._tmrShowTitle.Enabled = true;
      }
      if (BrainGame.MusicManager.IsMusicActive)
        return;
      Game1.ThemeMusic.Play(true);
    }

    private void _tmrShowTitle_OnTimer(IUIControl sender)
    {
      this._title.Show();
      this._btnBack.Show();
    }

    private void ThemeSelectionScreen_OnOpenTransitionEnded(IUIControl sender)
    {
      if (!this.StageAutoselected)
        return;
      this.EnableInput();
    }

    private void btnBack_OnPress(IUIControl sender)
    {
      switch (this._state)
      {
        case ThemeSelectionScreen.ScreenState.ThemeSelection:
          this.DisableInput();
          this.Navigator.GlobalCache.Set("MAIN_SCREEN_STARTUP_MODE", (object) MainMenuScreen.StartupType.TitleAndMenuVisible);
          this.NavigateTo("MainMenu", (Transition) ScreenTransitions.LeafsClosing, (Transition) ScreenTransitions.LeafsOpening);
          break;
        case ThemeSelectionScreen.ScreenState.StageSelection:
          this.DisableInput();
          this._stageInfo.Visible = false;
          this._stagesPanel.Hide();
          this._state = ThemeSelectionScreen.ScreenState.ThemeSelection;
          break;
      }
    }

    private void _themesPanel_OnShow(IUIControl sender)
    {
      this.EnableInput();
      this._themesPanel.AcceptControllerInput = true;
      this._themesPanel.Focus();
      this._state = ThemeSelectionScreen.ScreenState.ThemeSelection;
    }

    private void _themesPanel_OnCancelEnded(IUIControl sender)
    {
      this.EnableInput();
      this._themesPanel.Focus();
    }

    private void _themesPanel_OnThemeSelectedStarted(IUIControl sender)
    {
      this.DisableInput();
      this.InstructionBar.HideAllLabels();
    }

    private void _themesPanel_OnThemeSelected(IUIControl sender)
    {
      Levels.CurrentTheme = this._themesPanel.SelectedTheme.ThemeId;
      this._themesPanel.Enabled = false;
      this._stagesPanel.Show(this.Levels, this._themesPanel.SelectedTheme.ThemeId);
    }

    private void _stagesPanel_OnHide(IUIControl sender)
    {
      this._themesPanel.CancelSelection();
      this._themesPanel.Enabled = true;
      BrainGame.ResourceManager.Unload("STAGE_THUMBNAILS");
    }

    private void _stagesPanel_OnShow(IUIControl sender)
    {
      this.EnableInput();
      this._stagesPanel.FocusOnLastUnlocked();
      this._state = ThemeSelectionScreen.ScreenState.StageSelection;
    }

    private void StagesPanel_OnStageSelected(IUIControl sender)
    {
      if (Game1.ThemeMusic != null && Game1.ThemeMusic.IsPlaying)
        BrainGame.MusicManager.FadeMusic(0.0f, 500);
      this.DisableInput();
      this._stagesPanel.RaiseStageLeaveEvent = false;
      UIStage uiStage = (UIStage) sender;
      if (uiStage.Locked && BrainGame.IsTrial && !uiStage.LevelStageInfo.AvailableInDemo && Game1.GameSettings.WithAppStore)
      {
        this.NavigateToPurchase();
      }
      else
      {
        if (uiStage.Locked)
          return;
        uiStage.DoOnLeaveEffect = false;
        Levels.CurrentStageNr = uiStage.StageNr;
        this.Navigator.GlobalCache.Set("SELECTED_STAGE_INFO", (object) uiStage.LevelStageInfo);
        this.Navigator.GlobalCache.Set("STAGE_START_SHOW_STAGE_INFO", (object) false);
        this.Navigator.GlobalCache.Set("STAGE_START_SHOW_XBOX_HELP", (object) (BrainGame.Settings.Platform == BrainSettings.PlaformType.XBox));
        this.NavigateTo("InGame", ScreenType.StageStart.ToString(), (Transition) ScreenTransitions.LeafsClosing, (Transition) null);
      }
    }

    private void _stagesPanel_OnStageEnter(IUIControl sender)
    {
      UIStage uiStage = (UIStage) sender;
      if (uiStage.Locked)
        return;
      LevelStage levelStage = this.Levels.GetLevelStage(this._themesPanel.SelectedTheme.ThemeId, uiStage.StageNr);
      uiStage.LevelStageInfo = levelStage;
      this._stageInfo.Visible = true;
      this._stageInfo.Initialize(levelStage);
    }

    private void _stagesPanel_OnStageLeave(IUIControl sender)
    {
      UIStage uiStage = (UIStage) sender;
      if (!this._stageInfo.Visible || this._stageInfo.StageInfo.StageNr != uiStage.StageNr)
        return;
      this._stageInfo.Visible = false;
    }

    private void _stagesPanel_OnBack(IUIControl sender)
    {
    }

    private enum ScreenState
    {
      None,
      ThemeSelection,
      StageSelection,
    }
  }
}
