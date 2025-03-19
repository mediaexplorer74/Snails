
// Type: TwoBrainsGames.Snails.Screens.HowToPlayScreen
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.Snails.Tutorials;


namespace TwoBrainsGames.Snails.Screens
{
  internal class HowToPlayScreen : SnailsScreen
  {
    private UIHowToPlayWindow _howToPlayWindow;

    public HowToPlayScreen(ScreenNavigator owner)
      : base(owner, ScreenType.HowToPlay)
    {
      this.Name = "HowToPlay";
      this.BackgroundColor = new Color(0, 0, 0, 200);
      this._howToPlayWindow = new UIHowToPlayWindow((UIScreen) this);
      this._howToPlayWindow.Name = nameof (_howToPlayWindow);
      this._howToPlayWindow.ParentAlignment = AlignModes.Horizontaly;
      this._howToPlayWindow.OnDismiss += new UIControl.UIEvent(this._howToPlayWindow_OnDismiss);
      this._howToPlayWindow.OnShow += new UIControl.UIEvent(this._howToPlayWindow_OnShow);
      this._howToPlayWindow.OnDismissPressed += new UIControl.UIEvent(this._howToPlayWindow_OnDismissPressed);
      this._howToPlayWindow.UseCloseHotKey = true;
      this.Controls.Add((UIControl) this._howToPlayWindow);
    }

    public override void OnClose()
    {
      base.OnClose();
      BrainGame.ResourceManager.Unload("TUTORIAL");
    }

    public override void OnLoad()
    {
      base.OnLoad();
      this.BackgroundImage = (Sprite) null;
    }

    public override void OnStart()
    {
      this.WithBlurEffect = this.Navigator.GlobalCache.Get<bool>("SHOW_BLUR", false);
      base.OnStart();
      this._howToPlayWindow.Topics = BrainGame.ScreenNavigator.GlobalCache.Get<List<TutorialTopic>>("TUTORIAL_TOPIC_LIST", new List<TutorialTopic>());
      this.DisableInput();
      this._howToPlayWindow.Visible = false;
      this._howToPlayWindow.Show();
    }

    private void _howToPlayWindow_OnShow(IUIControl sender)
    {
      this.EnableInput();
      this._howToPlayWindow.Focus();
    }

    private void _howToPlayWindow_OnDismiss(IUIControl sender) => this.Close();

    private void _howToPlayWindow_OnDismissPressed(IUIControl sender)
    {
      this._howToPlayWindow.Close();
    }

    public static void PopUp(List<TutorialTopic> topics) => HowToPlayScreen.PopUp(topics, false);

    public static void PopUp(List<TutorialTopic> topics, bool showBlur)
    {
      BrainGame.ScreenNavigator.GlobalCache.Set("SHOW_BLUR", (object) showBlur);
      BrainGame.ScreenNavigator.GlobalCache.Set("TUTORIAL_TOPIC_LIST", (object) topics);
      BrainGame.ScreenNavigator.PopUp(ScreenType.HowToPlay.ToString());
    }
  }
}
