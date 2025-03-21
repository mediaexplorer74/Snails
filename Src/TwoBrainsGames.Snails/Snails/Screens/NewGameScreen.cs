
// Type: TwoBrainsGames.Snails.Screens.NewGameScreen
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;
using TwoBrainsGames.Snails.Screens.Transitions;


namespace TwoBrainsGames.Snails.Screens
{
  internal class NewGameScreen(ScreenNavigator owner) : UIScreen(owner)
  {
    private const string CURSOR_SPRITE = "DefaultCursor";
    private const string LABEL_TITLE = "Profiles";
    private const string LABEL_INFO = "Choose your player's name: ";
    private TextFont _fontTextBigger;
    private UITextFontLabelInput _textInput;
    private TextFont _textFont;

    public override void OnLoad()
    {
      this._fontTextBigger = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-big", 
          ResourceManager.ResourceManagerCacheType.Static);
      this._textFont = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-medium", 
          ResourceManager.ResourceManagerCacheType.Static);
      UITextFontLabel control1 = new UITextFontLabel((UIScreen) this, this._fontTextBigger, "Profiles");
      control1.Position = new Vector2(0.0f, 1500f);
      control1.ParentAlignment = AlignModes.Horizontaly;
      this.Controls.Add((UIControl) control1);
      UITextFontLabel control2 = new UITextFontLabel((UIScreen) this, this._textFont, 
          "Choose your player's name: ");
      control2.Position = new Vector2(950f, 3200f);
      this.Controls.Add((UIControl) control2);
      this._textInput = new UITextFontLabelInput((UIScreen) this, this._textFont);
      this._textInput.Position = new Vector2(5900f, 3200f);
      this.Controls.Add((UIControl) this._textInput);
    }

    private void BackButton_OnPress(IUIControl sender) => this.BackToScreen();

    public override void OnStart()
    {
    }

    private void BackToScreen()
    {
      string screenId = this.Navigator.GlobalCache.Get<string>("NEW_GAME_BACK_SCREEN");
      if (screenId == ScreenType.MainMenu.ToString())
        this.Navigator.GlobalCache.Set("MAIN_SCREEN_STARTUP_MODE", 
            (object) MainMenuScreen.StartupType.TitleAndMenuVisible);
      this.NavigateTo(screenId, (Transition) ScreenTransitions.LeafsClosing, 
          (Transition) ScreenTransitions.LeafsOpening);
    }

    public override void OnUpdate(BrainGameTime gameTime)
    {
      if (this.InputController.ActionAccept && this._textInput.HasText())
      {
        Game1.ProfilesManager.CreateProfile(this._textInput.Text);
        this.NavigateTo(ScreenGroupType.InGame.ToString(), ScreenType.StageStart.ToString(), 
            (Transition) ScreenTransitions.FadeOut, (Transition) ScreenTransitions.FadeIn);
      }
      else
      {
        if (!this.InputController.ActionBack)
          return;
        this.BackToScreen();
      }
    }
  }
}
