
// Type: TwoBrainsGames.Snails.Game1
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;
using TwoBrainsGames.Snails.Configuration;
using TwoBrainsGames.Snails.Player;
using TwoBrainsGames.Snails.Screens;
using TwoBrainsGames.Snails.Screens.ThemeSelection;
using TwoBrainsGames.Snails.Screens.Transitions;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.Snails.Tutorials;


namespace TwoBrainsGames.Snails
{
  public class Game1 : BrainGame, IDisposable
  {
    internal new static string Ek = "123#odeprot";
    public static Game1 _instance;
    private PlayersProfileManager _profilesManager;
    private Tutorial _tutorial;
    private Music _themeMusic;
    private List<string> _footerMessages;

    internal MainMenuScreen MenuScreen { get; set; }

    public static Game1 Instance => Game1._instance;

    public static Rectangle SafeArea => Game1._instance.GraphicsDevice.Viewport.TitleSafeArea;

    internal static GameSettings GameSettings
    {
      get => (GameSettings) Game1._instance._settings;
      set => Game1._instance._settings = (BrainSettings) value;
    }

    internal static PlayersProfileManager ProfilesManager => Game1._instance._profilesManager;

    internal static Tutorial Tutorial => Game1._instance._tutorial;

    internal static Music ThemeMusic
    {
      get => Game1._instance._themeMusic;
      set => Game1._instance._themeMusic = value;
    }

    internal static List<string> FooterMessages => Game1._instance._footerMessages;

    public Game1()
    {
      Game1._instance = this;
      BrainGame.GameName = "Snails";
      BrainGame.GameFolderName = "Snails";
      BrainGame.BetaName = "Snails Beta";
    }

    protected override void LoadSettings()
    {
      this._settings = (BrainSettings) new GameSettings();
      this._settings.Load("game-settings");
    }

    protected override void OnInitialize()
    {
      this.InitializeProfile();
      this.UpdateGamePlayMode();
      if (Game1.GameSettings.ShowEULA && !this.EULAAgreement())
      {
        this.Quit();
      }
      else
      {
        if (Game1.GameSettings.AllowToggleFullScreen)
          this.QueryScreenMode();
        this.IsFixedTimeStep = false;
        this._graphicsManager.PreferredBackBufferWidth = BrainGame.PresentationNativeScreenWidth;
        this._graphicsManager.PreferredBackBufferHeight = BrainGame.PresentationNativeScreenHeight;
        this._graphicsManager.PreferredDepthStencilFormat = DepthFormat.Depth24Stencil8;
        
        //RnD
        this._graphicsManager.IsFullScreen = false;//Game1.GameSettings.IsFullScreen;
        
        //if (Game1.ProfilesManager.CurrentProfile != null && Game1.GameSettings.AllowToggleFullScreen)
        //  this._graphicsManager.IsFullScreen = Game1.ProfilesManager.CurrentProfile.Fullscreen;
        
        this._graphicsManager.SynchronizeWithVerticalRetrace = Game1.GameSettings.UseVSync;
        this._graphicsManager.ApplyChanges();
        this.SetupRenderViewport();
        this.SetupProjectionMatrix();
        this.IsMouseVisible = false;
        this._defaultCamera = new Camera2D();
        this._defaultCamera.Initialize();
        this.Services.AddService(typeof (SpriteBatch), (object) BrainGame.SpriteBatch);
        this._lineBatch = new LineBatch(this._graphicsManager.GraphicsDevice, 1f);
        this._tutorial = BrainGame.ResourceManager.Load<Tutorial>("tutorials/tutorial", ResourceManager.ResourceManagerCacheType.Static);
        BrainGame.HddAccessIcon = (IHddIndicator) new TwoBrainsGames.Snails.HddAccessIcon();
        BrainGame.HddAccessIcon.LoadContent();
        BrainGame.DisplayHDDAccessIcon = false;
        BrainGame.ScreenNavigator.UseAssyncGroupLoading = Game1.GameSettings.UseAsyncLoading;
        BrainGame.ClearColor = Color.Black;
        BrainGame.GameCursor = (Cursor) new SoftwareCursor();
        if (BrainGame.GameCursor is SoftwareCursor)
        {
          BrainGame.GameCursor.LoadCursor("spriteset/player-cursor/DefaultCursor", 0);
          BrainGame.GameCursor.LoadCursor("spriteset/player-cursor/BusyCursor", 1);
          BrainGame.GameCursor.LoadCursor("spriteset/player-cursor/ForbiddenCursor", 2);
          BrainGame.GameCursor.LoadCursor("spriteset/player-cursor/SaltCursor", 3);
          BrainGame.GameCursor.LoadCursor("spriteset/player-cursor/SaltCursorForbidden", 4);
          BrainGame.GameCursor.LoadCursor("spriteset/player-cursor/OutOfStockCursor", 5);
          BrainGame.GameCursor.LoadCursor("spriteset/player-cursor/PanCursor", 6);
          //RnD
          BrainGame.GameCursor.Visible = true;//false;
          BrainGame.GameCursor.SetCursor(0);
        }
        BrainGame.ScreenNavigator.NavigateTo(Game1.GameSettings.StartupScreenGroup, Game1.GameSettings.StartupScreen);
        ScreenTransitions.Initialize();
        StageObjectFactory.Initialize();
        BrainGame.ResourceManager.CreateUserDefinedResourceManager("STAGE_THUMBNAILS");
        BrainGame.ResourceManager.CreateUserDefinedResourceManager("STAGE_THEME_RESOURCES");
        BrainGame.ResourceManager.CreateUserDefinedResourceManager("TUTORIAL");
        this.InitializeFooterMessages();
        this.OnLanguageChanged += new EventHandler(this.SnailsGame_OnLanguageChanged);
        this.OnGameActivated += new EventHandler(this.SnailsGame_OnGameActivated);
      }
    }

    private void UpdateGamePlayMode()
    {
      Game1.GameSettings.GameplayMode = /*Guide.IsTrialMode ? BrainSettings.GameplayModeType.Demo :*/ BrainSettings.GameplayModeType.Retail;
    }

    private void SnailsGame_OnGameActivated(object sender, EventArgs e)
    {
      this.UpdateGamePlayMode();
      if (BrainGame.ScreenNavigator == null || BrainGame.ScreenNavigator.GlobalCache.Get<ScreenType>("CURRENT_SCREEN", ScreenType.None) != ScreenType.Purchase)
        return;
      BrainGame.ScreenNavigator.GlobalCache.Set("MAIN_SCREEN_STARTUP_MODE", 
          (object) MainMenuScreen.StartupType.TitleAndMenuVisible);
      BrainGame.ScreenNavigator.NavigateTo("MainMenu", ScreenType.MainMenu.ToString(), 
          (Transition) null, (Transition) null);
    }

    private void SnailsGame_OnLanguageChanged(object sender, EventArgs e)
    {
      this.InitializeFooterMessages();
    }

    public void InitializeFooterMessages()
    {
      if (this._footerMessages == null)
        this._footerMessages = new List<string>();
      else
        this._footerMessages.Clear();
      this._footerMessages.Add(LanguageManager.GetString("MSG_FOOTER_GET_WP_VERSION"));
    }

    private void QueryScreenMode()
    {
    }

    private void InitializeProfile()
    {
      this._profilesManager = PlayersProfileManagerFactory.Create();
      this._profilesManager.BeginLoad();
    }

    public override void SetupRenderViewport()
    {
      float num1 = (float) this._graphicsManager.GraphicsDevice.Viewport.Width / (float) this._graphicsManager.GraphicsDevice.Viewport.Height;
      float num2 = (float) BrainGame.PresentationNativeScreenWidth / (float) BrainGame.PresentationNativeScreenHeight;
      if ((double) num2 < (double) num1)
      {
        float width = (float) this._graphicsManager.GraphicsDevice.Viewport.Height * (float) BrainGame.PresentationNativeScreenWidth / (float) BrainGame.PresentationNativeScreenHeight;
        BrainGame.SetViewport(new Viewport((int) (((float) this._graphicsManager.GraphicsDevice.Viewport.Width - width) / 2f), 0, (int) width, this._graphicsManager.GraphicsDevice.DisplayMode.Height));
      }
      else if ((double) num2 > (double) num1)
      {
        float height = (float) this._graphicsManager.GraphicsDevice.Viewport.Width * (float) BrainGame.PresentationNativeScreenHeight / (float) BrainGame.PresentationNativeScreenWidth;
        BrainGame.SetViewport(new Viewport(0, (int) (((float) this._graphicsManager.GraphicsDevice.Viewport.Height - height) / 2f), this._graphicsManager.GraphicsDevice.Viewport.Width, (int) height));
      }
      else
        BrainGame.SetViewport(this._graphicsManager.GraphicsDevice.Viewport);
    }

    protected override void SetupProjectionMatrix()
    {
      this._renderEffect = new BasicEffect(BrainGame.Graphics);
      this._renderEffect.World = Matrix.Identity;
      this._renderEffect.View = Matrix.Identity;
      int num1 = 0;
      int num2 = 0;
      int nativeScreenWidth1 = BrainGame.PresentationNativeScreenWidth;
      int nativeScreenHeight1 = BrainGame.PresentationNativeScreenHeight;
      int width = this._graphicsManager.GraphicsDevice.Viewport.Width;
      int height = this._graphicsManager.GraphicsDevice.Viewport.Height;
      int num3 = Game1.GameSettings.ScreenWidth;
      int num4 = Game1.GameSettings.ScreenHeight;
      float num5 = (float) nativeScreenWidth1 / (float) nativeScreenHeight1;
      float num6 = (float) width / (float) height;
      if ((double) num5 > (double) num6)
      {
        int num7 = width * nativeScreenHeight1 / height;
        num4 = nativeScreenHeight1;
        num1 = Math.Abs((nativeScreenWidth1 - num7) / 2);
      }
      else if ((double) num5 <= (double) num6)
      {
        int num8 = nativeScreenWidth1 * height / width;
        num3 = nativeScreenWidth1;
        num2 = Math.Abs((nativeScreenHeight1 - num8) / 2);
      }
      int num9 = 1;
      if (Game1.GameSettings.Platform == BrainSettings.PlaformType.Windows8 || Game1.GameSettings.Platform == BrainSettings.PlaformType.Windows8RT)
        num9 = -1;
      int nativeScreenWidth2 = BrainGame.PresentationNativeScreenWidth;
      int nativeScreenHeight2 = BrainGame.PresentationNativeScreenHeight;
      int x = 0;
      int y = 0;
      this._screenRectangle = new Rectangle(x, y, Game1.GameSettings.ScreenWidth, Game1.GameSettings.ScreenHeight);
      this._renderEffect.Projection = Matrix.CreateTranslation((float) -x - 0.5f * (float) num9, (float) y - 0.5f * (float) num9, 0.0f) * Matrix.CreateOrthographicOffCenter(0.0f, (float) nativeScreenWidth2, (float) nativeScreenHeight2, 0.0f, -1f, 1f);
      this._renderEffect.TextureEnabled = true;
      this._renderEffect.VertexColorEnabled = true;
      this._viewportRatioX = this._viewportRatioY = 1f;
    }

    public override Screen CreateScreen(ScreenNavigator navigator, string screenId)
    {
      ScreenType screenType = (ScreenType) Enum.Parse(typeof (ScreenType), screenId, true);
      switch (screenType)
      {
        case ScreenType.BrainsLogo:
          return (Screen) new BrainsLogoScreen(navigator);
        case ScreenType.MainMenu:
          return (Screen) new MainMenuScreen(navigator);
        case ScreenType.Gameplay:
          return (Screen) new GameplayScreen(navigator);
        case ScreenType.InGameOptions:
          return (Screen) new InGameOptionsScreen(navigator);
        case ScreenType.Options:
          return (Screen) new OptionsScreen(navigator);
        case ScreenType.Credits:
          return (Screen) new CreditsScreen(navigator);
        case ScreenType.Overscan:
          return (Screen) new OverscanScreen(navigator);
        case ScreenType.ThemeSelection:
          return Game1.GameSettings.PresentationMode == GameSettings.PresentationType.HD ? (Screen) new ThemeSelectionScreen(navigator) : (Screen) new ThemeSelectionLDScreen(navigator);
        case ScreenType.DebugOptions:
          return (Screen) new DebugOptionsScreen(navigator);
        case ScreenType.StageCompleted:
          return (Screen) new StageCompletedScreen(navigator);
        case ScreenType.StageStart:
          return (Screen) new StageStartScreen(navigator);
        case ScreenType.AutoSave:
          return (Screen) new AutoSaveScreen(navigator);
        case ScreenType.Startup:
          return (Screen) new StartupScreen(navigator);
        case ScreenType.Quit:
          return (Screen) new QuitGameScreen(navigator);
        case ScreenType.NewGame:
          return (Screen) new NewGameScreen(navigator);
        case ScreenType.MissionFailed:
          return (Screen) new MissionFailedScreen(navigator);
        case ScreenType.ThemeUnlocked:
          return (Screen) new ThemeUnlockedScreen(navigator);
        case ScreenType.XBoxControllerHelp:
          return (Screen) new XBoxHelpScreen(navigator);
        case ScreenType.HowToPlay:
          return (Screen) new HowToPlayScreen(navigator);
        case ScreenType.Purchase:
          return (Screen) new PurchaseScreen(navigator);
        case ScreenType.Awards:
          return (Screen) new AwardsScreen(navigator);
        case ScreenType.PlayerStats:
          return (Screen) new PlayerStatsScreen(navigator);
        default:
          throw new BrainException("Invalid ScreenType [" + screenType.ToString() + "]");
      }
    }

    private void SelectGameSettings()
    {
      throw new SnailsException("SelectGameSettings is not valid in non Forms application.");
    }

    private bool EULAAgreement()
    {
      throw new SnailsException("EULAAgreement is not valid in non Forms application.");
    }

    public void PurchaseGame()
    {
      if (!BrainGame.IsTrial)
        return;
      //Guide.ShowMarketplace(PlayerIndex.One);
    }
  }
}
