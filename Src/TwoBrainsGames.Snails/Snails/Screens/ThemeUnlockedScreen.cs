
// Type: TwoBrainsGames.Snails.Screens.ThemeUnlockedScreen
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.Snails.Screens.Transitions;


namespace TwoBrainsGames.Snails.Screens
{
  internal class ThemeUnlockedScreen(ScreenNavigator owner) : SnailsScreen(owner, ScreenType.ThemeUnlocked)
  {
    private const float LINE_SPACING = 400f;
    private LeafTransition _leafs;
    protected UISnailsBoard _board;
    protected UISnailsMenuTitle _title;
    protected UISnailsButton _btnStageSel;
    protected UISnailsButton _btnProceed;
    protected UISnailsThemeIcon _imgTheme;
    protected UICaption[] _capHelp;
    protected UIStars _stars;
    private Sample _unlockSound;

    private ThemeType UnlockedTheme { get; set; }

    protected ScreenType NextScreen { get; set; }

    private string NextGroup { get; set; }

    private LeafTransition OpeningTransition { get; set; }

    private Vector2 MessagePosition { get; set; }

    public override void OnLoad()
    {
      base.OnLoad();
      this.Name = "ThemeUnlocked";
      this.BackgroundImage = (Sprite) null;
      this.BackgroundColor = new Color(0, 0, 0, 175);
      this.OnBeforeControlsDraw += new UIControl.UIEvent(this.ThemeUnlockedScreen_OnBeforeControlsDraw);
      this.OnInitializeFromContent += new UIControl.UIEvent(this.ThemeUnlockedScreen_OnInitializeFromContent);
      this.OnAfterInitializeFromContent += new UIControl.UIEvent(this.ThemeUnlockedScreen_OnAfterInitializeFromContent);
      this._leafs = new LeafTransition(LeafTransition.State.ClosedStopped);
      this._leafs.Initialize();
      this._board = new UISnailsBoard((UIScreen) this, UISnailsBoard.BoardType.LightWoodMedium);
      this._board.Name = "_board";
      this._board.ParentAlignment = AlignModes.Horizontaly;
      this._board.OnShow += new UIControl.UIEvent(this._board_OnShow);
      this._board.Size = new Size(this._board.Width, 5800f);
      this.Controls.Add((UIControl) this._board);
      this._title = new UISnailsMenuTitle((UIScreen) this);
      this._title.Name = "_title";
      this._title.TextResourceId = "TITLE_THEME_UNLOCK";
      this._title.BoardSize = UISnailsMenuTitle.TitleSize.Big;
      this._board.Controls.Add((UIControl) this._title);
      this._imgTheme = new UISnailsThemeIcon((UIScreen) this);
      this._imgTheme.Name = "_imgTheme";
      this._imgTheme.ParentAlignment = AlignModes.Horizontaly;
      this._board.Controls.Add((UIControl) this._imgTheme);
      string[] multiString = LanguageManager.GetMultiString("LBL_THEME_UNLOCK_HELP");
      this._capHelp = new UICaption[multiString.Length];
      for (int index = 0; index < multiString.Length; ++index)
      {
        this._capHelp[index] = new UICaption((UIScreen) this, multiString[index], Color.White, UICaption.CaptionStyle.NormalTextSmall);
        this._capHelp[index].ParentAlignment = AlignModes.Horizontaly;
        this._board.Controls.Add((UIControl) this._capHelp[index]);
      }
      this._btnStageSel = new UISnailsButton((UIScreen) this, "BTN_STAGE_SELECTION", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.Back, new UIControl.UIEvent(this.btnStageSel_OnAccept), false);
      this._btnStageSel.Name = "_btnStageSel";
      this._btnStageSel.ParentAlignment = AlignModes.Bottom;
      this._btnStageSel.ButtonAction = UISnailsButton.ButtonActionType.StageSelection;
      this._board.Controls.Add((UIControl) this._btnStageSel);
      this._btnProceed = new UISnailsButton((UIScreen) this, "BTN_PROCEED", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, new UIControl.UIEvent(this.btnProceed_OnAccept), false);
      this._btnProceed.Name = "_btnProceed";
      this._btnProceed.ParentAlignment = AlignModes.Bottom;
      this._btnProceed.ButtonAction = UISnailsButton.ButtonActionType.Next;
      this._board.Controls.Add((UIControl) this._btnProceed);
      this._stars = new UIStars((UIScreen) this, 20, 2600, 500, 1000);
      this._stars.Name = "_stars";
      this._stars.Size = new Size(this._board.Size.Width, 500f);
      this._board.Controls.Add((UIControl) this._stars);
      this._unlockSound = BrainGame.ResourceManager.GetSampleTemporary("sfx/theme-unlocked");
    }

    private void ThemeUnlockedScreen_OnInitializeFromContent(IUIControl sender)
    {
      this.MessagePosition = this.GetContentPropertyValue<Vector2>("messagePosition", this.MessagePosition);
    }

    private void ThemeUnlockedScreen_OnAfterInitializeFromContent(IUIControl sender)
    {
      string[] multiString = LanguageManager.GetMultiString("LBL_THEME_UNLOCK_HELP");
      Vector2 messagePosition = this.MessagePosition;
      for (int index = 0; index < multiString.Length; ++index)
      {
        this._capHelp[index].Position = messagePosition;
        messagePosition += new Vector2(0.0f, 400f);
      }
    }

    public override void OnStart()
    {
      base.OnStart();
      this.DisableInput();
      this._board.Visible = false;
      this._board.Show();
      this.NextScreen = this.Navigator.GlobalCache.Get<ScreenType>("THEME_UNLOCK_NEXT_SCREEN", ScreenType.None);
      this.NextGroup = this.Navigator.GlobalCache.Get<string>("THEME_UNLOCK_NEXT_SCREEN_GROUP", (string) null);
      this.OpeningTransition = this.Navigator.GlobalCache.Get<LeafTransition>("THEME_UNLOCK_OPEN_TRANSITION", (LeafTransition) null);
      this.UnlockedTheme = this.Navigator.GlobalCache.Get<ThemeType>("THEME_UNLOCK_UNLOCKED_THEME", ThemeType.None);
      if (this.NextScreen == ScreenType.ThemeSelection)
      {
        this._btnStageSel.Visible = false;
        this._btnProceed.ParentAlignment = AlignModes.Horizontaly | AlignModes.Bottom;
      }
      else
      {
        this._btnStageSel.Visible = true;
        this._btnProceed.ParentAlignment = AlignModes.Bottom;
      }
      this._imgTheme.Theme = this.UnlockedTheme;
      foreach (UIControl uiControl in this._capHelp)
        uiControl.BlendColor = Colors.ThemeCaptions[(int) this.UnlockedTheme];
      this._stars.Initialize();
      this.DisableInput();
      switch (this.UnlockedTheme)
      {
        case ThemeType.ThemeB:
          BrainGame.AchievementsManager.Notify(38);
          break;
        case ThemeType.ThemeC:
          BrainGame.AchievementsManager.Notify(40);
          break;
        case ThemeType.ThemeD:
          BrainGame.AchievementsManager.Notify(42);
          break;
      }
      this._unlockSound.Play();
    }

    private void ThemeUnlockedScreen_OnBeforeControlsDraw(IUIControl sender)
    {
    }

    private void btnProceed_OnAccept(IUIControl sender)
    {
      this.DisableInput();
      this.Navigator.GlobalCache.Set("AUTO_SELECT_STAGE", (object) false);
      this.NavigateTo(this.NextGroup, this.NextScreen.ToString(), (Transition) ScreenTransitions.LeafsClosing, (Transition) this.OpeningTransition);
    }

    private void btnStageSel_OnAccept(IUIControl sender)
    {
      this.DisableInput();
      this.Navigator.GlobalCache.Set("AUTO_SELECT_STAGE", (object) false);
      this.NavigateTo("MainMenu", ScreenType.ThemeSelection.ToString(), (Transition) ScreenTransitions.LeafsClosing, (Transition) ScreenTransitions.LeafsOpening);
    }

    private void _board_OnShow(IUIControl sender)
    {
      this.EnableInput();
      this._btnProceed.Focus();
    }
  }
}
