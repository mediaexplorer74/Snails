
// Type: TwoBrainsGames.Snails.Screens.GameplayScreen
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Input;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.Screens
{
  public class GameplayScreen : Screen
  {
    public GameplayInput Input;
    private static GameplayScreen _Instance;

    public Levels Levels { get; private set; }

    public static GameplayScreen Instance => GameplayScreen._Instance;

    public Stage CurrentStage => this.Levels.Stage;

    public GameplayScreen(ScreenNavigator owner)
      : base(owner)
    {
      GameplayScreen._Instance = this;
      this.OnScreenModeChanged += new UIControl.UIEvent(this.GameplayScreen_OnScreenModeChanged);
      this.OnGameLostFocus += new UIControl.UIEvent(this.GameplayScreen_OnGameLostFocus);
      this.OnOpenTransitionEnded += new UIControl.UIEvent(this.GameplayScreen_OnOpenTransitionEnded);
    }

    private void GameplayScreen_OnOpenTransitionEnded(IUIControl sender)
    {
      this.Levels.Stage.OnOpenTransitionEnded();
    }

    public override void OnLoad()
    {
      this._inputController = (InputBase) new GameplayInput();
      this._inputController.Initialize();
      this.Input = (GameplayInput) this._inputController;
      BrainGame.ResourceManager.Load<DataFileRecord>("stages/levels", ResourceManager.ResourceManagerCacheType.Static);
      this.Levels = Levels.Load();
    }

    public override void OnStart()
    {
      this._inputController.Reset();
      this.Levels.Stage.IsPaused = false;
      this.Levels.Stage.Start();
      BrainGame.GameCursor.Visible = Game1.GameSettings.ShowCursor;
      if (Game1.GameSettings.UseGamepad)
        this.CenterCursor();
      BrainGame.SampleManager.UseAudibleBoundingSquare = true;
    }

    public override void OnUpdate(BrainGameTime gameTime)
    {
      this.Levels.HandleEvents(gameTime);
      this.Levels.Update(gameTime);
    }

    public override void OnDraw() => this.Levels.Draw();

    public override void OnUnload()
    {
      this.Levels.UnloadContent();
      this.Levels = Levels._instance = (Levels) null;
    }

    private void GameplayScreen_OnScreenModeChanged(IUIControl sender)
    {
    }

    private void GameplayScreen_OnGameLostFocus(IUIControl sender)
    {
      if (Stage.CurrentStage == null)
        return;
      Stage.CurrentStage.GameLostFocus();
    }
  }
}
