
// Type: TwoBrainsGames.Snails.Screens.PlayerStatsScreen
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
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
  internal class PlayerStatsScreen : SnailsScreen
  {
    private const float TOP_MARGIN = 500f;
    private const float BOTTOM_MARGIN = 500f;
    private const float CAP_WIDTH = 900f;
    private const float LINE_SPACING = 100f;
    private const float LEFT_MARGIN = 300f;
    private UISnailsMenuTitle _title;
    private UISnailsScrollablePanel _panel;
    private UIPanel _pnlStats;
    private UIPlayerStat _capPlayingTime;
    private UIPlayerStat _capRunningTime;
    private UIPlayerStat _capTotalSnailsSafe;
    private UIPlayerStat _capTotalSnailsKingSafe;
    private UIPlayerStat _capTotalSnailsDeadByFire;
    private UIPlayerStat _capTotalSnailsDeadBySpikes;
    private UIPlayerStat _capTotalSnailsDeadByDynamite;
    private UIPlayerStat _capTotalSnailsDeadByCrate;
    private UIPlayerStat _capTotalSnailsDeadByWater;
    private UIPlayerStat _capTotalSnailsDeadByLaser;
    private UIPlayerStat _capTotalSnailsDeadBySacrifice;
    private UIPlayerStat _capTotalSnailsDeadByCrateExplosion;
    private UIPlayerStat _capTotalSnailsDeadByAcid;
    private UIPlayerStat _capTotalSnailsDeadByOutOfStage;
    private UIPlayerStat _capTotalSnailsDeadByEvilSnail;
    private UIPlayerStat _capSnailsDeadInDifferentWays;
    private UIPlayerStat _capTotalGoldMedals;
    private UIPlayerStat _capTotalSilverMedals;
    private UIPlayerStat _capTotalBronzeMedals;
    private UIPlayerStat _capTotalBoots;
    private UIPanel _pnlAchievs;
    private UIBackButton _btnBack;
    private UISnailsButton _btnReset;
    private UISnailsButton _btnClearTrophies;
    private UISnailsButton _btnStats;
    private UISnailsButton _btnAchievs;

    public PlayerStatsScreen(ScreenNavigator owner)
      : base(owner, ScreenType.PlayerStats)
    {
      this._title = new UISnailsMenuTitle((UIScreen) this);
      this._title.ParentAlignment = AlignModes.Horizontaly | AlignModes.Top;
      this._title.TextResourceId = "TITLE_PLAYER_STATS";
      this.Controls.Add((UIControl) this._title);
      this._panel = new UISnailsScrollablePanel((UIScreen) this, UIScrollablePanel.PanelOrientation.Vertical, !BrainGame.Settings.UseTouch, 10000f);
      this._panel.ParentAlignment = AlignModes.Horizontaly;
      this._panel.Size = new Size(7000f, 7500f);
      this._panel.Position = new Vector2(0.0f, 2000f);
      this.Controls.Add((UIControl) this._panel);
      this._pnlStats = new UIPanel((UIScreen) this);
      this._panel.Controls.Add((UIControl) this._pnlStats);
      Vector2 pos = new Vector2(300f, 500f);
      this._capPlayingTime = this.AddStat(ref pos, "LBL_PLAY_TIME", UIPlayerStat.PlayerStatType.PlayingTime);
      this._capRunningTime = this.AddStat(ref pos, "LBL_RUNNING_TIME", UIPlayerStat.PlayerStatType.RunningTime);
      this._capTotalSnailsSafe = this.AddStat(ref pos, "LBL_TOT_SNAILS_SAFE", UIPlayerStat.PlayerStatType.TotalSnailsSafe);
      this._capTotalSnailsKingSafe = this.AddStat(ref pos, "LBL_TOT_KING_SAFE", UIPlayerStat.PlayerStatType.TotalSnailsKingSafe);
      this._capTotalSnailsDeadByFire = this.AddStat(ref pos, "LBL_TOT_DEAD_FIRE", UIPlayerStat.PlayerStatType.TotalSnailsDeadByFire);
      this._capTotalSnailsDeadBySpikes = this.AddStat(ref pos, "LBL_TOT_DEAD_SPIKES", UIPlayerStat.PlayerStatType.TotalSnailsDeadBySpikes);
      this._capTotalSnailsDeadByDynamite = this.AddStat(ref pos, "LBL_TOT_DEAD_DYNAMITE", UIPlayerStat.PlayerStatType.TotalSnailsDeadByDynamite);
      this._capTotalSnailsDeadByCrate = this.AddStat(ref pos, "LBL_TOT_DEAD_CRATE", UIPlayerStat.PlayerStatType.TotalSnailsDeadByCrate);
      this._capTotalSnailsDeadByWater = this.AddStat(ref pos, "LBL_TOT_DEAD_WATER", UIPlayerStat.PlayerStatType.TotalSnailsDeadByWater);
      this._capTotalSnailsDeadByLaser = this.AddStat(ref pos, "LBL_TOT_DEAD_LASER", UIPlayerStat.PlayerStatType.TotalSnailsDeadByLaser);
      this._capTotalSnailsDeadBySacrifice = this.AddStat(ref pos, "LBL_DEAD_SACRIFICE", UIPlayerStat.PlayerStatType.TotalSnailsDeadBySacrifice);
      this._capTotalSnailsDeadByCrateExplosion = this.AddStat(ref pos, "LBL_DEAD_EXPLOSION", UIPlayerStat.PlayerStatType.TotalSnailsDeadByCrateExplosion);
      this._capTotalSnailsDeadByAcid = this.AddStat(ref pos, "LBL_DEAD_ACID", UIPlayerStat.PlayerStatType.TotalSnailsDeadByAcid);
      this._capTotalSnailsDeadByOutOfStage = this.AddStat(ref pos, "LBL_DEAD_OUTOFSTAGE", UIPlayerStat.PlayerStatType.TotalSnailsDeadByOutOfStage);
      this._capTotalSnailsDeadByEvilSnail = this.AddStat(ref pos, "LBL_DEAD_EVILSNAIL", UIPlayerStat.PlayerStatType.TotalSnailsDeadByEvilSnail);
      this._capSnailsDeadInDifferentWays = this.AddStat(ref pos, "LBL_DEAD_DIFFERENTWAYS", UIPlayerStat.PlayerStatType.SnailsDeadInDifferentWays);
      this._capTotalBronzeMedals = this.AddStat(ref pos, "LBL_TOT_BRONZE_MEDALS", UIPlayerStat.PlayerStatType.TotalBronzeMedals);
      this._capTotalSilverMedals = this.AddStat(ref pos, "LBL_SILVER_MEDALS", UIPlayerStat.PlayerStatType.TotalSilverMedals);
      this._capTotalGoldMedals = this.AddStat(ref pos, "LBL_TOT_GOLD_MEDALS", UIPlayerStat.PlayerStatType.TotalGoldMedals);
      this._capTotalBoots = this.AddStat(ref pos, "LBL_TOT_BOOSTS", UIPlayerStat.PlayerStatType.TotalBoots);
      this._pnlStats.Size = new Size(this._panel.Width, pos.Y + 500f);
      this._pnlAchievs = new UIPanel((UIScreen) this);
      this._panel.Controls.Add((UIControl) this._pnlAchievs);
      this.AddAchievements();
      this._btnBack = new UIBackButton((UIScreen) this);
      this._btnBack.ScreenAlignment = UIBackButton.ButtonScreenAlignment.BottomLeft;
      this._btnBack.OnAccept += new UIControl.UIEvent(this.btnBack_OnPress);
      this.Controls.Add((UIControl) this._btnBack);
      this._btnReset = new UISnailsButton((UIScreen) this, "BTN_RESET_STATS", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, new UIControl.UIEvent(this.btnReset_OnPress), true);
      this._btnReset.ParentAlignment = AlignModes.Bottom | AlignModes.Left;
      this._btnReset.Margins.Right = 150f;
      this._btnReset.Margins.Bottom = 1500f;
      this._btnReset.Scale = this.FromNativeResolution(this._btnReset.Scale);
      this.Controls.Add((UIControl) this._btnReset);
      this._btnClearTrophies = new UISnailsButton((UIScreen) this, "BTN_RESET_TROPHIES", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, new UIControl.UIEvent(this.btnClearTrophies_OnPress), true);
      this._btnClearTrophies.Name = nameof (_btnClearTrophies);
      this._btnClearTrophies.ParentAlignment = AlignModes.Bottom | AlignModes.Left;
      this._btnClearTrophies.Margins.Right = 150f;
      this._btnClearTrophies.Margins.Bottom = 2750f;
      this._btnClearTrophies.Scale = this.FromNativeResolution(this._btnClearTrophies.Scale);
      this.Controls.Add((UIControl) this._btnClearTrophies);
      this._btnStats = new UISnailsButton((UIScreen) this, "BTN_VIEW_STATS", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, new UIControl.UIEvent(this.btnStats_OnPress), true);
      this._btnStats.ParentAlignment = AlignModes.Bottom | AlignModes.Left;
      this._btnStats.Margins.Right = 150f;
      this._btnStats.Margins.Bottom = 4000f;
      this._btnStats.Scale = this.FromNativeResolution(this._btnStats.Scale);
      this.Controls.Add((UIControl) this._btnStats);
      this._btnAchievs = new UISnailsButton((UIScreen) this, "BTN_VIEW_ACHIEVEMENTS", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, new UIControl.UIEvent(this.btnAchievs_OnPress), true);
      this._btnAchievs.ParentAlignment = AlignModes.Bottom | AlignModes.Left;
      this._btnAchievs.Margins.Right = 150f;
      this._btnAchievs.Margins.Bottom = 5250f;
      this._btnAchievs.Scale = this.FromNativeResolution(this._btnAchievs.Scale);
      this.Controls.Add((UIControl) this._btnAchievs);
    }

    public override void OnStart()
    {
      base.OnStart();
      this.RefreshValues();
      this.ActivateStatsPanel();
      this.EnableInput();
    }

    private void ActivateStatsPanel()
    {
      this._pnlStats.Visible = true;
      this._pnlAchievs.Visible = false;
      this._panel.Length = this._pnlStats.Height;
      this._panel.ScrollToTop();
      this.RefreshValues();
    }

    private void ActivateAchievementsPanel()
    {
      this._pnlStats.Visible = false;
      this._pnlAchievs.Visible = true;
      this._panel.Length = this._pnlAchievs.Height;
      this._panel.ScrollToTop();
      this.RefreshValues();
    }

    private void RefreshValues()
    {
      this._capPlayingTime.Value = (object) Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalPlayingTime;
      this._capPlayingTime.CaptionFormat = UIValuedCaption.CaptionMode.FulltimeHours;
      this._capRunningTime.Value = (object) Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalRunningTime;
      this._capRunningTime.CaptionFormat = UIValuedCaption.CaptionMode.FulltimeHours;
      this._capTotalSnailsSafe.Value = (object) Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsSafe;
      this._capTotalSnailsKingSafe.Value = (object) Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsKingSafe;
      this._capTotalSnailsDeadByFire.Value = (object) Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByFire;
      this._capTotalSnailsDeadBySpikes.Value = (object) Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadBySpikes;
      this._capTotalSnailsDeadByDynamite.Value = (object) Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByDynamite;
      this._capTotalSnailsDeadByCrate.Value = (object) Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByCrate;
      this._capTotalSnailsDeadByWater.Value = (object) Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByWater;
      this._capTotalSnailsDeadByLaser.Value = (object) Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByLaser;
      this._capTotalSnailsDeadBySacrifice.Value = (object) Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadBySacrifice;
      this._capTotalSnailsDeadByCrateExplosion.Value = (object) Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByCrateExplosion;
      this._capTotalSnailsDeadByAcid.Value = (object) Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByAcid;
      this._capTotalSnailsDeadByOutOfStage.Value = (object) Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByOutOfStage;
      this._capTotalSnailsDeadByEvilSnail.Value = (object) Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByEvilSnail;
      this._capSnailsDeadInDifferentWays.Value = (object) Game1.ProfilesManager.CurrentProfile.PlayerStats.SnailsDeadInDifferentWays;
      this._capTotalGoldMedals.Value = (object) Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalGoldCoins;
      this._capTotalSilverMedals.Value = (object) Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSilverCoins;
      this._capTotalBronzeMedals.Value = (object) Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalBronzeCoins;
      this._capTotalBoots.Value = (object) Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalBoosts;
      foreach (UIAchievement control in this._pnlAchievs.Controls)
        control.Refresh();
    }

    private void AddAchievements()
    {
      Vector2 vector2 = new Vector2(300f, 500f);
      foreach (BrainAchievement achievement in BrainGame.AchievementsManager.Achievements.Values)
      {
        if (Game1.GameSettings.WithAppStore || !achievement.ShowOnAppStore)
        {
          UIAchievement control = new UIAchievement((UIScreen) this, achievement);
          control.Position = vector2;
          this._pnlAchievs.Controls.Add((UIControl) control);
          vector2 += new Vector2(0.0f, control.Height + 100f);
        }
      }
      this._pnlAchievs.Height = vector2.Y;
    }

    private UIPlayerStat AddStat(
      ref Vector2 pos,
      string textResource,
      UIPlayerStat.PlayerStatType statType)
    {
      UIPlayerStat control = new UIPlayerStat((UIScreen) this, textResource, pos, statType);
      control.OnReset += new UIPlayerStat.UIPlayerStatEvent(this.ResetStat);
      this._pnlStats.Controls.Add((UIControl) control);
      pos += new Vector2(0.0f, control.Height + 100f);
      return control;
    }

    private void btnBack_OnPress(IUIControl sender)
    {
      this.DisableInput();
      this.Navigator.GlobalCache.Set("MAIN_SCREEN_STARTUP_MODE", (object) MainMenuScreen.StartupType.TitleAndMenuVisible);
      this.NavigateTo(ScreenType.MainMenu.ToString(), (Transition) ScreenTransitions.LeafsClosing, (Transition) ScreenTransitions.LeafsOpening);
    }

    private void btnAchievs_OnPress(IUIControl sender) => this.ActivateAchievementsPanel();

    private void btnStats_OnPress(IUIControl sender) => this.ActivateStatsPanel();

    private void btnReset_OnPress(IUIControl sender)
    {
      Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalPlayingTime = TimeSpan.Zero;
      Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalRunningTime = TimeSpan.Zero;
      Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsSafe = 0;
      Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsKingSafe = 0;
      Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByFire = 0;
      Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadBySpikes = 0;
      Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByDynamite = 0;
      Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByCrate = 0;
      Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByWater = 0;
      Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByLaser = 0;
      Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadBySacrifice = 0;
      Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByCrateExplosion = 0;
      Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByAcid = 0;
      Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByOutOfStage = 0;
      Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByEvilSnail = 0;
      Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalGoldCoins = 0;
      Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSilverCoins = 0;
      Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalBronzeCoins = 0;
      Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalBoosts = 0;
      Game1.ProfilesManager.Save();
      this.RefreshValues();
    }

    private void btnClearTrophies_OnPress(IUIControl sender)
    {
      Game1.ProfilesManager.CurrentProfile.ClearAchievements();
      Game1.ProfilesManager.Save();
      this.RefreshValues();
    }

    private void ResetStat(IUIControl sender, UIPlayerStat.PlayerStatType stat)
    {
      switch (stat)
      {
        case UIPlayerStat.PlayerStatType.PlayingTime:
          Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalPlayingTime = TimeSpan.Zero;
          break;
        case UIPlayerStat.PlayerStatType.RunningTime:
          Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalRunningTime = TimeSpan.Zero;
          break;
        case UIPlayerStat.PlayerStatType.TotalSnailsSafe:
          Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsSafe = 0;
          break;
        case UIPlayerStat.PlayerStatType.TotalSnailsKingSafe:
          Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsKingSafe = 0;
          break;
        case UIPlayerStat.PlayerStatType.TotalSnailsDeadByFire:
          Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByFire = 0;
          break;
        case UIPlayerStat.PlayerStatType.TotalSnailsDeadBySpikes:
          Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadBySpikes = 0;
          break;
        case UIPlayerStat.PlayerStatType.TotalSnailsDeadByDynamite:
          Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByDynamite = 0;
          break;
        case UIPlayerStat.PlayerStatType.TotalSnailsDeadByCrate:
          Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByCrate = 0;
          break;
        case UIPlayerStat.PlayerStatType.TotalSnailsDeadByWater:
          Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByWater = 0;
          break;
        case UIPlayerStat.PlayerStatType.TotalSnailsDeadByLaser:
          Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByLaser = 0;
          break;
        case UIPlayerStat.PlayerStatType.TotalSnailsDeadBySacrifice:
          Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadBySacrifice = 0;
          break;
        case UIPlayerStat.PlayerStatType.TotalGoldMedals:
          Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalGoldCoins = 0;
          break;
        case UIPlayerStat.PlayerStatType.TotalSilverMedals:
          Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSilverCoins = 0;
          break;
        case UIPlayerStat.PlayerStatType.TotalBronzeMedals:
          Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalBronzeCoins = 0;
          break;
        case UIPlayerStat.PlayerStatType.TotalBoots:
          Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalBoosts = 0;
          break;
        case UIPlayerStat.PlayerStatType.TotalSnailsDeadByCrateExplosion:
          Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByCrateExplosion = 0;
          break;
        case UIPlayerStat.PlayerStatType.TotalSnailsDeadByAcid:
          Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByAcid = 0;
          break;
        case UIPlayerStat.PlayerStatType.TotalSnailsDeadByOutOfStage:
          Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByOutOfStage = 0;
          break;
        case UIPlayerStat.PlayerStatType.TotalSnailsDeadByEvilSnail:
          Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByEvilSnail = 0;
          break;
      }
      Game1.ProfilesManager.Save();
      this.RefreshValues();
    }
  }
}
