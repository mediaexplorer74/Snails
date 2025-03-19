
// Type: TwoBrainsGames.Snails.Screens.StageCompletedScreen
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;
using TwoBrainsGames.Snails.Player;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.Snails.Screens.Transitions;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.Screens
{
  internal class StageCompletedScreen(ScreenNavigator owner) : SnailsScreen(owner, ScreenType.StageCompleted)
  {
    protected UISnailsMenuTitle _title;
    protected UIValuedCaption _capSnailsDelivered;
    protected UIValuedCaption _capTimeTaken;
    protected UIValuedCaption _capSnailsBonus;
    protected UIValuedCaption _capCoinsBonus;
    protected UIValuedCaption _capTimeBonus;
    protected UIValuedCaption _capTotalPoints;
    protected UISnailsBoard _leafsBoard;
    protected UISnailsBoard _board;
    protected UITimer _tmrShowCaptions;
    protected UISnailsMedal _medal;
    protected UIImage _imgHighscore;
    protected UISnailsButton _btnNext;
    protected UISnailsButton _btnQuit;
    protected UISnailsButton _btnAgain;
    protected ThemeType _unlockedTheme;
    protected bool _allowNextStage;
    private LevelStage _nextStage;
    protected bool _hasHighscore;
    protected Sample _goldMedalSound;
    protected Sample _ticticSound;

    private ThemeType UnlockedTheme
    {
      get => this._unlockedTheme;
      set
      {
        this._unlockedTheme = value;
        this.Navigator.GlobalCache.Set("THEME_UNLOCK_UNLOCKED_THEME", (object) this._unlockedTheme);
      }
    }

    protected StageCompletedScreen.ScreenState State { get; set; }

    private StageStats StageStatistics => Stage.CurrentStage.Stats;

    private float TopMargin { get; set; }

    private float LineSpacing { get; set; }

    private float SectionSpacing { get; set; }

    private float CaptionWidth { get; set; }

    private float CaptionLeftMargin { get; set; }

    public override void OnLoad()
    {
      base.OnLoad();
      this.BackgroundImage = (Sprite) null;
      this.Name = "StageCompleted";
      this.OnInitializeFromContent += new UIControl.UIEvent(this.StageCompletedScreen_OnInitializeFromContent);
      this.OnAfterInitializeFromContent += new UIControl.UIEvent(this.StageCompletedScreen_OnAfterInitializeFromContent);
      this._leafsBoard = new UISnailsBoard((UIScreen) this, UISnailsBoard.BoardType.LeafsMedium);
      this._leafsBoard.Name = "_leafsBoard";
      this.Controls.Add((UIControl) this._leafsBoard);
      this._board = new UISnailsBoard((UIScreen) this, UISnailsBoard.BoardType.LightWoodMedium);
      this._board.Name = "_board";
      this._board.ParentAlignment = AlignModes.Horizontaly;
      this._board.OnShow += new UIControl.UIEvent(this._board_OnShow);
      this._leafsBoard.Controls.Add((UIControl) this._board);
      this._title = new UISnailsMenuTitle((UIScreen) this);
      this._title.Name = "_title";
      this._title.TextResourceId = "TITLE_STAGE_COMPLETED";
      this._title.BoardSize = UISnailsMenuTitle.TitleSize.Big;
      this._board.Controls.Add((UIControl) this._title);
      Color completedCaptions = Colors.StageCompletedCaptions;
      this._capSnailsDelivered = new UIValuedCaption((UIScreen) this, "LBL_STAGE_COMPL_SNAILS_DELIVERED", (object) 0, completedCaptions, completedCaptions, UICaption.CaptionStyle.StageCompletedCaptions, 0.0f, true);
      this._capSnailsDelivered.Name = "_capSnailsDelivered";
      this._capSnailsDelivered.Mode = UIValuedCaption.CaptionMode.Simple;
      this._capSnailsDelivered.OnShow += new UIControl.UIEvent(this._captions_OnShow);
      this._board.Controls.Add((UIControl) this._capSnailsDelivered);
      this._capTimeTaken = new UIValuedCaption((UIScreen) this, "LBL_STAGE_COMPL_TIME_TAKEN", (object) new TimeSpan(0, 0, 0), completedCaptions, completedCaptions, UICaption.CaptionStyle.StageCompletedCaptions, 0.0f, true);
      this._capTimeTaken.OnShow += new UIControl.UIEvent(this._captions_OnShow);
      this._capTimeTaken.Mode = UIValuedCaption.CaptionMode.Simple;
      this._board.Controls.Add((UIControl) this._capTimeTaken);
      Color completedBonusCaptions = Colors.StageCompletedBonusCaptions;
      this._capSnailsBonus = new UIValuedCaption((UIScreen) this, "LBL_STAGE_COMPL_SNAILS_BONUS", (object) 6, completedBonusCaptions, completedBonusCaptions, UICaption.CaptionStyle.StageCompletedCaptions, 0.0f, true);
      this._capSnailsBonus.OnShow += new UIControl.UIEvent(this._captions_OnShow);
      this._capSnailsBonus.Mode = UIValuedCaption.CaptionMode.Simple;
      this._board.Controls.Add((UIControl) this._capSnailsBonus);
      this._capTimeBonus = new UIValuedCaption((UIScreen) this, "LBL_STAGE_COMPL_TIME_BONUS", (object) new TimeSpan(0, 0, 0), completedBonusCaptions, completedBonusCaptions, UICaption.CaptionStyle.StageCompletedCaptions, 0.0f, true);
      this._capTimeBonus.OnShow += new UIControl.UIEvent(this._captions_OnShow);
      this._capTimeBonus.Mode = UIValuedCaption.CaptionMode.Multiplier;
      this._capTimeBonus.Mode = UIValuedCaption.CaptionMode.Simple;
      this._board.Controls.Add((UIControl) this._capTimeBonus);
      this._capCoinsBonus = new UIValuedCaption((UIScreen) this, "LBL_STAGE_COMPL_COIN_BONUS", (object) 0, completedBonusCaptions, completedBonusCaptions, UICaption.CaptionStyle.StageCompletedCaptions, 0.0f, true);
      this._capCoinsBonus.OnShow += new UIControl.UIEvent(this._captions_OnShow);
      this._capCoinsBonus.Mode = UIValuedCaption.CaptionMode.Simple;
      this._board.Controls.Add((UIControl) this._capCoinsBonus);
      this._capTotalPoints = new UIValuedCaption((UIScreen) this, "LBL_STAGE_COMPL_TOTAL_POINTS", (object) 0, completedBonusCaptions, completedBonusCaptions, UICaption.CaptionStyle.StageCompletedCaptions, 0.0f, true);
      this._capTotalPoints.OnShow += new UIControl.UIEvent(this._captions_OnShow);
      this._capTotalPoints.Mode = UIValuedCaption.CaptionMode.Simple;
      this._board.Controls.Add((UIControl) this._capTotalPoints);
      this._tmrShowCaptions = new UITimer((UIScreen) this, 600.0, false);
      this._tmrShowCaptions.OnTimer += new UIControl.UIEvent(this._tmrShowCaptions_OnTimer);
      this.Controls.Add((UIControl) this._tmrShowCaptions);
      this._btnQuit = new UISnailsButton((UIScreen) this, "BTN_STAGE_SELECTION", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.Back, new UIControl.UIEvent(this.btnBack_OnAccept), true);
      this._btnQuit.Name = "_btnQuit";
      this._btnQuit.ParentAlignment = AlignModes.Bottom;
      this._btnQuit.Position = new Vector2(0.0f, 0.0f);
      this._btnQuit.ButtonAction = UISnailsButton.ButtonActionType.StageSelection;
      this._board.Controls.Add((UIControl) this._btnQuit);
      this._btnAgain = new UISnailsButton((UIScreen) this, "BTN_PLAY_AGAIN", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, new UIControl.UIEvent(this.btnAgain_OnAccept), true);
      this._btnAgain.Name = "_btnAgain";
      this._btnAgain.ParentAlignment = AlignModes.Bottom;
      this._btnAgain.Position = new Vector2(1300f, 0.0f);
      this._btnAgain.ButtonAction = UISnailsButton.ButtonActionType.Retry;
      this._board.Controls.Add((UIControl) this._btnAgain);
      this._btnNext = new UISnailsButton((UIScreen) this, "BTN_NEXT_STAGE", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, new UIControl.UIEvent(this.btnNext_OnAccept), true);
      this._btnNext.Name = "_btnNext";
      this._btnNext.ParentAlignment = AlignModes.Bottom;
      this._btnNext.Position = new Vector2(2600f, 0.0f);
      this._btnNext.ButtonAction = UISnailsButton.ButtonActionType.Next;
      this._board.Controls.Add((UIControl) this._btnNext);
      this._medal = new UISnailsMedal((UIScreen) this, MedalType.None);
      this._medal.Name = "_medal";
      this._medal.OnShow += new UIControl.UIEvent(this._medal_OnShow);
      this._board.Controls.Add((UIControl) this._medal);
      this._imgHighscore = new UIImage((UIScreen) this, "spriteset/common-elements-1/NewHighscore", "__STATIC__");
      this._imgHighscore.Name = "_imgHighscore";
      this._imgHighscore.ShowEffect = (TransformEffectBase) new SquashEffect(0.8f, 4f, 0.04f, this.BlendColor, this.Scale);
      this._imgHighscore.Position = new Vector2(2630f, 3650f);
      this._goldMedalSound = BrainGame.ResourceManager.GetSampleStatic("sfx/gold-medal");
      this._ticticSound = BrainGame.ResourceManager.GetSampleTemporary("sfx/stage-completed-tic-tic");
      this.WithBlurEffect = true;
    }

    private void StageCompletedScreen_OnInitializeFromContent(IUIControl sender)
    {
      this.TopMargin = this.GetContentPropertyValue<float>("topMargin", this.TopMargin);
      this.LineSpacing = this.GetContentPropertyValue<float>("lineSpacing", this.LineSpacing);
      this.SectionSpacing = this.GetContentPropertyValue<float>("sectionSpacing", this.SectionSpacing);
      this.CaptionWidth = this.GetContentPropertyValue<float>("captionWidth", this.CaptionWidth);
      this.CaptionLeftMargin = this.GetContentPropertyValue<float>("captionLeftMargin", this.CaptionLeftMargin);
    }

    private void StageCompletedScreen_OnAfterInitializeFromContent(IUIControl sender)
    {
      this._capSnailsDelivered.Position = new Vector2(this.CaptionLeftMargin, this.TopMargin);
      this._capTimeTaken.Position = this._capSnailsDelivered.Position + new Vector2(0.0f, this.LineSpacing);
      this._capSnailsBonus.Position = this._capTimeTaken.Position + new Vector2(0.0f, this.SectionSpacing);
      this._capTimeBonus.Position = this._capSnailsBonus.Position + new Vector2(0.0f, this.LineSpacing);
      this._capCoinsBonus.Position = this._capTimeBonus.Position + new Vector2(0.0f, this.LineSpacing);
      this._capTotalPoints.Position = this._capCoinsBonus.Position + new Vector2(0.0f, this.SectionSpacing);
      this._capSnailsDelivered.Width = this.CaptionWidth;
      this._capTimeTaken.Width = this.CaptionWidth;
      this._capSnailsBonus.Width = this.CaptionWidth;
      this._capTimeBonus.Width = this.CaptionWidth;
      this._capCoinsBonus.Width = this.CaptionWidth;
      this._capTotalPoints.Width = this.CaptionWidth;
    }

    public override void OnStart()
    {
      base.OnStart();
      BrainGame.MusicManager.FadeMusic(0.0f, 500);
      this._nextStage = (LevelStage) null;
      this.ProfileStatsUpdate();
      this.DisableInput();
      this._capSnailsDelivered.Visible = false;
      this._capTimeTaken.Visible = false;
      this._capSnailsBonus.Visible = false;
      this._capTimeBonus.Visible = false;
      this._capTotalPoints.Visible = false;
      this._capCoinsBonus.Visible = false;
      this._leafsBoard.Visible = false;
      this._tmrShowCaptions.Parameter = (object) 0;
      this._tmrShowCaptions.Enabled = false;
      this._medal.Visible = false;
      this._medal.Reset();
      this.State = StageCompletedScreen.ScreenState.Bluring;
      if (Stage.CurrentStage.LevelStage._goal == GoalType.SnailKiller)
      {
        this._capSnailsDelivered.Value = (object) Stage.CurrentStage.Stats.NumSnailsDead;
        this._capSnailsDelivered.Text = LanguageManager.GetString("LBL_STAGE_COMPL_SNAILS_KILLED");
      }
      else
      {
        this._capSnailsDelivered.Value = (object) Stage.CurrentStage.Stats.NumSnailsSafe;
        this._capSnailsDelivered.Text = LanguageManager.GetString("LBL_STAGE_COMPL_SNAILS_DELIVERED");
      }
      this._capTimeTaken.Value = (object) Stage.CurrentStage.Stats.TimeTaken;
      this._capSnailsBonus.Value = (object) Stage.CurrentStage.Stats.SnailsDeliveredPointsWon;
      this._capTimeBonus.Value = (object) Stage.CurrentStage.Stats.TimePointsWon;
      this._capCoinsBonus.Value = (object) Stage.CurrentStage.Stats.CoinPointsWon;
      this._capTotalPoints.Value = (object) Stage.CurrentStage.Stats.TotalScore;
      this._medal.Face = Stage.CurrentStage.Stats.MedalWon;
      this._imgHighscore.Visible = false;
      BrainGame.GameCursor.SetCursor(0);
      this._btnAgain.Visible = false;
      this._btnNext.Visible = false;
      this._btnQuit.Visible = false;
      this.OnBlurEffectEnded += new EventHandler(this.StageCompletedScreen_OnBlurEffectEnded);
      this._ticticSound.Play(true);
    }

    public override void OnUpdate(BrainGameTime gameTime)
    {
      base.OnUpdate(gameTime);
      if (this._goldMedalSound != null)
        this._goldMedalSound.FadeUpdate(gameTime);
      switch (this.State)
      {
        case StageCompletedScreen.ScreenState.ShowingLabels:
          if (!this.InputController.ActionAccept)
            break;
          this._tmrShowCaptions.Enabled = false;
          this._capSnailsDelivered.QuickShow();
          this._capTimeTaken.QuickShow();
          this._capSnailsBonus.QuickShow();
          this._capTimeBonus.QuickShow();
          this._capCoinsBonus.QuickShow();
          this._capTotalPoints.QuickShow();
          this.State = StageCompletedScreen.ScreenState.ShowLabelsSkipped;
          this.AcceptControllerInput = true;
          break;
      }
    }

    private void FadeSamples()
    {
      if (this._goldMedalSound == null)
        return;
      this._goldMedalSound.FadeOut(new TimeSpan(0, 0, 1));
    }

    protected void btnAgain_OnAccept(IUIControl sender)
    {
      this.FadeSamples();
      if (this.UnlockedTheme != ThemeType.None)
        this.ShowThemeUnlocked("InGame", ScreenType.StageStart, (LeafTransition) null);
      else
        this.NavigateTo(ScreenType.StageStart.ToString(), (Transition) ScreenTransitions.LeafsClosing, (Transition) null);
    }

    protected void btnBack_OnAccept(IUIControl sender)
    {
      this.FadeSamples();
      if (this.UnlockedTheme != ThemeType.None)
      {
        this.ShowThemeUnlocked("MainMenu", ScreenType.ThemeSelection, ScreenTransitions.LeafsOpening);
      }
      else
      {
        this.Navigator.GlobalCache.Set("AUTO_SELECT_STAGE", (object) true);
        this.NavigateTo("MainMenu", ScreenType.ThemeSelection.ToString(), (Transition) ScreenTransitions.LeafsClosing, (Transition) ScreenTransitions.LeafsOpening);
      }
    }

    protected void btnNext_OnAccept(IUIControl sender)
    {
      this.FadeSamples();
      if (this._allowNextStage && this._nextStage != null)
      {
        this.Navigator.GlobalCache.Set("SELECTED_STAGE_INFO", (object) this._nextStage);
        this.Navigator.GlobalCache.Set("STAGE_START_SHOW_STAGE_INFO", (object) false);
        this.Navigator.GlobalCache.Set("STAGE_START_SHOW_XBOX_HELP", (object) false);
        if (this.UnlockedTheme != ThemeType.None)
          this.ShowThemeUnlocked("InGame", ScreenType.StageStart, (LeafTransition) null);
        else
          this.NavigateTo(ScreenType.StageStart.ToString(), (Transition) ScreenTransitions.LeafsClosing, (Transition) null);
      }
      else
        this.NavigateToPurchase();
    }

    private void ShowThemeUnlocked(
      string screenGroupToNavigate,
      ScreenType screenToNavigate,
      LeafTransition openingTransition)
    {
      this.Navigator.GlobalCache.Set("THEME_UNLOCK_NEXT_SCREEN_GROUP", (object) screenGroupToNavigate);
      this.Navigator.GlobalCache.Set("THEME_UNLOCK_NEXT_SCREEN", (object) screenToNavigate);
      this.Navigator.GlobalCache.Set("THEME_UNLOCK_OPEN_TRANSITION", (object) openingTransition);
      this.PopUp(ScreenType.ThemeUnlocked.ToString(), false);
    }

    protected void _captions_OnShow(IUIControl sender)
    {
      switch (this.State)
      {
        case StageCompletedScreen.ScreenState.ShowingLabels:
          this._tmrShowCaptions.Reset();
          this._tmrShowCaptions.Enabled = true;
          break;
        case StageCompletedScreen.ScreenState.ShowLabelsSkipped:
          if (this._medal.Visible)
            break;
          this.ShowMedal();
          break;
      }
    }

    protected void StageCompletedScreen_OnBlurEffectEnded(object sender, EventArgs e)
    {
      this._leafsBoard.Show();
      this.State = StageCompletedScreen.ScreenState.ShowingBoard;
    }

    protected void _tmrShowCaptions_OnTimer(IUIControl sender)
    {
      int parameter = (int) this._tmrShowCaptions.Parameter;
      int num = parameter + 1;
      switch (parameter)
      {
        case 0:
          this._capSnailsDelivered.Show();
          break;
        case 1:
          this._capTimeTaken.Show();
          break;
        case 2:
          this._capSnailsBonus.Show();
          break;
        case 3:
          this._capTimeBonus.Show();
          break;
        case 4:
          this._capCoinsBonus.Show();
          break;
        case 5:
          this._capTotalPoints.Show();
          break;
        case 6:
          if (!this._medal.Visible)
          {
            this.ShowMedal();
            break;
          }
          break;
      }
      this._tmrShowCaptions.Parameter = (object) num;
    }

    protected void _board_OnShow(IUIControl sender)
    {
      this.State = StageCompletedScreen.ScreenState.ShowingLabels;
      this._tmrShowCaptions.Reset();
      this._tmrShowCaptions.Enabled = true;
    }

    protected void _medal_OnShow(IUIControl sender) => this.ShowLabelsEnded();

    private void ProfileStatsUpdate()
    {
      this.UnlockedTheme = ThemeType.None;
      if (Game1.ProfilesManager.CurrentProfile == null)
        return;
      LevelStage nextStageInfo = Levels.CurrentLevel.GetNextStageInfo();
      if (nextStageInfo != null && !nextStageInfo.IsCustomStage)
      {
        Game1.ProfilesManager.CurrentProfile.LastPlayedStageNr = nextStageInfo.StageNr;
        Game1.ProfilesManager.CurrentProfile.LastPlayedTheme = nextStageInfo.ThemeId;
      }
      bool themeBUnlocked = Game1.ProfilesManager.CurrentProfile.PlayerStats.IsThemeUnlocked(ThemeType.ThemeB);
      bool themeCUnlocked = Game1.ProfilesManager.CurrentProfile.PlayerStats.IsThemeUnlocked(ThemeType.ThemeC);
      bool themeDUnlocked = Game1.ProfilesManager.CurrentProfile.PlayerStats.IsThemeUnlocked(ThemeType.ThemeD);
      PlayerStageStats stageStats = Game1.ProfilesManager.CurrentProfile.PlayerStats.GetStageStats(Stage.CurrentStage.LevelStage.StageId);
      if (stageStats != null)
      {
        ++stageStats.TimesPlayed;
        if (this.StageStatistics.NumSnailsSafe > stageStats.NumSnailsSafe)
          stageStats.NumSnailsSafe = this.StageStatistics.NumSnailsSafe;
        if (stageStats.NumSnailsSafe == this.StageStatistics.NumSnailsToSave)
          BrainGame.AchievementsManager.Notify(6);
        if (this.StageStatistics.NumGoldCoins > stageStats.NumGoldCoins)
          stageStats.NumGoldCoins = this.StageStatistics.NumGoldCoins;
        if (this.StageStatistics.NumSilverCoins > stageStats.NumSilverCoins)
          stageStats.NumSilverCoins = this.StageStatistics.NumSilverCoins;
        if (this.StageStatistics.NumBronzeCoins > stageStats.NumBronzeCoins)
          stageStats.NumBronzeCoins = this.StageStatistics.NumBronzeCoins;
        this._hasHighscore = false;
        if (Stage.CurrentStage.Stats.TotalScore >= stageStats.Highscore)
        {
          stageStats.Highscore = Stage.CurrentStage.Stats.TotalScore;
          this._hasHighscore = true;
          stageStats.CompletionTime = new TimeSpan(this.StageStatistics.Timer.Ticks);
        }
        if (Stage.CurrentStage.Stats.MedalWon > stageStats.Medal)
          stageStats.Medal = Stage.CurrentStage.Stats.MedalWon;
      }
      this._nextStage = Game1.ProfilesManager.CurrentProfile.PlayerStats.UnlockNextStage(Stage.CurrentStage.LevelStage, true);
      if (this._nextStage != null)
      {
        this.Navigator.GlobalCache.Set("NEXT_STAGE_AVAILABLE", (object) true);
        this.UnlockedTheme = this.CheckIfThemeUnlocked(themeBUnlocked, themeCUnlocked, themeDUnlocked);
        if (this.UnlockedTheme != ThemeType.None)
          Game1.ProfilesManager.CurrentProfile.PlayerStats.UnlockFirstStage(this.UnlockedTheme);
        this._allowNextStage = true;
      }
      else
        this._allowNextStage = false;
      if (Game1.ProfilesManager.CurrentProfile.PlayerStats.GetClearStagesForTheme(Levels.CurrentTheme) == 21)
      {
        switch (Levels.CurrentTheme)
        {
          case ThemeType.ThemeA:
            BrainGame.AchievementsManager.Notify(37);
            break;
          case ThemeType.ThemeB:
            BrainGame.AchievementsManager.Notify(39);
            break;
          case ThemeType.ThemeC:
            BrainGame.AchievementsManager.Notify(41);
            break;
          case ThemeType.ThemeD:
            BrainGame.AchievementsManager.Notify(43);
            break;
        }
      }
      if (Game1.ProfilesManager.CurrentProfile.PlayerStats.IsAllMedalsGet(MedalType.Bronze))
        BrainGame.AchievementsManager.Notify(44);
      if (Game1.ProfilesManager.CurrentProfile.PlayerStats.IsAllMedalsGet(MedalType.Silver))
        BrainGame.AchievementsManager.Notify(45);
      if (Game1.ProfilesManager.CurrentProfile.PlayerStats.IsAllMedalsGet(MedalType.Gold))
        BrainGame.AchievementsManager.Notify(46);
      Game1.ProfilesManager.Save();
    }

    private ThemeType CheckIfThemeUnlocked(
      bool themeBUnlocked,
      bool themeCUnlocked,
      bool themeDUnlocked)
    {
      if (!themeBUnlocked && Game1.ProfilesManager.CurrentProfile.PlayerStats.IsThemeUnlocked(ThemeType.ThemeB))
        return ThemeType.ThemeB;
      if (!themeCUnlocked && Game1.ProfilesManager.CurrentProfile.PlayerStats.IsThemeUnlocked(ThemeType.ThemeC))
        return ThemeType.ThemeC;
      return !themeDUnlocked && Game1.ProfilesManager.CurrentProfile.PlayerStats.IsThemeUnlocked(ThemeType.ThemeD) ? ThemeType.ThemeD : ThemeType.None;
    }

    private void ShowLabelsEnded()
    {
      this.EnableInput();
      this.InstructionBar.HideAllLabels();
      this.InstructionBar.ShowLabel(UIInstructionLabel.LabelActionTypes.StartNextStage);
      this.InstructionBar.ShowLabel(UIInstructionLabel.LabelActionTypes.Quit);
      this._board.Focus();
      this.State = StageCompletedScreen.ScreenState.Idle;
      this._btnAgain.Show();
      if (this._allowNextStage || !this._allowNextStage && Game1.GameSettings.WithAppStore)
        this._btnNext.Show();
      this._btnQuit.Show();
      if (this._btnNext.Visible)
        this._btnNext.Focus();
      else
        this._btnQuit.Focus();
      this._ticticSound.Stop();
    }

    private void ShowMedal()
    {
      if (this._medal.Face != MedalType.None)
      {
        this._medal.Show();
        if (this._medal.Face == MedalType.Gold)
          this._goldMedalSound.Play();
      }
      else
        this.ShowLabelsEnded();
      if (!this._hasHighscore)
        return;
      this._imgHighscore.Show();
    }

    protected enum ScreenState
    {
      Idle,
      ShowingBoard,
      ShowingLabels,
      ShowLabelsSkipped,
      Bluring,
    }
  }
}
