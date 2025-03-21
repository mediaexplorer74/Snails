
// Type: TwoBrainsGames.Snails.Screens.AwardsScreen
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.Snails.Screens.Transitions;


namespace TwoBrainsGames.Snails.Screens
{
  internal class AwardsScreen(ScreenNavigator owner) : SnailsScreen(owner, ScreenType.Awards)
  {
    private const float TOP_MARGIN = 700f;
    private const float BOTTOM_MARGIN = 400f;
    private const float SPACING = 350f;
    private const float LEFT_MARGIN = 250f;
    private UISnailsMenuTitle _title;
    private UIBackButton _btnBack;
    private UISnailsScrollablePanel _panel;

    public override void OnLoad()
    {
      base.OnLoad();
      this._title = new UISnailsMenuTitle((UIScreen) this);
      this._title.ParentAlignment = AlignModes.Horizontaly;
      this._title.Position = new Vector2(0.0f, 200f);
      this._title.TextResourceId = "TITLE_SNAILS_AWARDS";
      this.Controls.Add((UIControl) this._title);

      this._panel = new UISnailsScrollablePanel((UIScreen) this, UIScrollablePanel.PanelOrientation.Vertical, 
          !BrainGame.Settings.UseTouch, 10000f);

      this._panel.ParentAlignment = AlignModes.Horizontaly;
      this._panel.Size = new Size(7000f, 6500f);
      this._panel.Position = new Vector2(0.0f, 2400f);
      this.Controls.Add((UIControl) this._panel);
      this._btnBack = new UIBackButton((UIScreen) this);
      this._btnBack.ParentAlignment = AlignModes.Bottom | AlignModes.Left;
      this._btnBack.Margins.Left = 150f;
      this._btnBack.Margins.Bottom = 250f;
      this._btnBack.OnPress += new UIControl.UIEvent(this.btnBack_OnPress);
      this.Controls.Add((UIControl) this._btnBack);
      this.BackgroundImageBlendColor = Colors.AwardsScrBkColor;
      this.BackgroundType = SnailsScreen.ScreenBackgroundType.Image;
      this.ShowTrialTag = false;
    }

    public override void OnStart()
    {
      base.OnStart();
      Vector2 vector2 = new Vector2(250f, 700f);
      this._panel.Clear();
      foreach (BrainAchievement achievement in BrainGame.AchievementsManager.Achievements.Values)
      {
        if (Game1.GameSettings.WithAppStore || !achievement.ShowOnAppStore)
        {
          UIAchievement control = new UIAchievement((UIScreen) this, achievement);
          control.Position = vector2;
          this._panel.Controls.Add((UIControl) control);
          vector2 += new Vector2(0.0f, control.Height + 350f);
        }
      }
      this._panel.Length = vector2.Y + 400f;
      this._panel.Reset();
      this.EnableInput();
    }

    private void btnBack_OnPress(IUIControl sender)
    {
      this.DisableInput();
      this.Navigator.GlobalCache.Set("MAIN_SCREEN_STARTUP_MODE", (object) MainMenuScreen.StartupType.TitleAndMenuVisible);
      this.NavigateTo(ScreenType.MainMenu.ToString(), (Transition) ScreenTransitions.LeafsClosing, (Transition) ScreenTransitions.LeafsOpening);
    }
  }
}
