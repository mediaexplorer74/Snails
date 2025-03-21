
// Type: TwoBrainsGames.BrainEngine.BrainGame
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Diagnostics;
using System.IO;
using TwoBrainsGames.BrainEngine.Audio;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.BrainEngine.Player;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Screens;
using Windows.Storage;


namespace TwoBrainsGames.BrainEngine
{
  public class BrainGame : Game, IDisposable
  {
    public const string BrainTeamName = "2BrainsGames";
    internal static string Ek = "123#odeprot";
    internal static BrainGame Instance;
    private ScreenNavigator _screenNavigator;
    protected string _contentRootDir;
    protected SampleManager _sampleManager;
    protected MusicManager _musicManager;
    private ResourceManager _resourceManager;
    protected AchievementsManager _achievementsManager;
    protected Camera2D _defaultCamera;
    protected BrainGameTime _gameTime = new BrainGameTime();
    protected BrainSettings _settings;
    protected Random _rand;
    protected Texture2D _rectTexture;
    protected LineBatch _lineBatch;
    protected Camera2D _activeCamera;

    protected GraphicsDeviceManager _graphicsManager;
    protected Viewport _viewport;

    protected float _viewportRatioX;
    protected float _viewportRatioY;
    protected Rectangle _screenRectangle;
    protected BasicEffect _renderEffect;
    private SpriteBatch _spriteBatch;
    private IHddIndicator _hddAccessIcon;
    private SamplerState _sampler2D;
    private SamplerState _currentSampler;
    private PlayerIndex _currentControllerIndex;
    private Color _clearColor;
    private bool _screenModeChanged;
    private bool _displayHDDAccessIcon;
    private Rectangle _viewportRectangle;
    private string _betaName;
    private string _gameName;
    private string _gameFolderName;
    private static string _mainGameFolder;
    private LanguageCode _currentLanguage;
    protected bool _accessingStorage;
    private int _nativeScreenWidth;
    private int _nativeScreenHeight;
    private int _presentationNativeScreenWidth;
    private int _presentationNativeScreenHeight;
    public static bool IsLoading;
    private Cursor _gameCursor;

    public event EventHandler OnLanguageChanged;

    public event EventHandler OnGameActivated;

    public static int ScreenWidth
    {
        get
        {
            return BrainGame.Instance._graphicsManager.PreferredBackBufferWidth;
        }
    }

    public static int ScreenHeight
    {
        get
        {
            return BrainGame.Instance._graphicsManager.PreferredBackBufferHeight;
        }
    }

    public static int NativeScreenWidth
    {
      get => BrainGame.Instance._nativeScreenWidth;
      set => BrainGame.Instance._nativeScreenWidth = value;
    }

    public static int NativeScreenHeight
    {
      get => BrainGame.Instance._nativeScreenHeight;
      set => BrainGame.Instance._nativeScreenHeight = value;
    }

    public static int PresentationNativeScreenWidth
    {
      get => BrainGame.Instance._presentationNativeScreenWidth;
      set => BrainGame.Instance._presentationNativeScreenWidth = value;
    }

    public static int PresentationNativeScreenHeight
    {
      get => BrainGame.Instance._presentationNativeScreenHeight;
      set => BrainGame.Instance._presentationNativeScreenHeight = value;
    }

    public static float ViewportRatioX => BrainGame.Instance._viewportRatioX;

    public static float ViewportRatioY => BrainGame.Instance._viewportRatioY;

    public static GraphicsDevice Graphics => BrainGame.Instance._graphicsManager.GraphicsDevice;

    public static GraphicsDeviceManager GraphicsManager => BrainGame.Instance._graphicsManager;

    public static SpriteBatch SpriteBatch => BrainGame.Instance._spriteBatch;

    public static Camera2D DefaultCamera => BrainGame.Instance._defaultCamera;

    public static Viewport Viewport => BrainGame.Instance._viewport;

    public static BasicEffect RenderEffect
    {
      get => BrainGame.Instance._renderEffect;
      set => BrainGame.Instance._renderEffect = value;
    }

    public static SampleManager SampleManager => BrainGame.Instance._sampleManager;

    public static MusicManager MusicManager => BrainGame.Instance._musicManager;

    public static ResourceManager ResourceManager => BrainGame.Instance._resourceManager;

    public static AchievementsManager AchievementsManager
    {
      get => BrainGame.Instance._achievementsManager;
    }

    public static bool IsGameActive => BrainGame.Instance.IsActive;

    public static bool IsTrial
    {
      get => BrainGame.Instance._settings.GameplayMode == BrainSettings.GameplayModeType.Demo;
    }

    public static ScreenNavigator ScreenNavigator => BrainGame.Instance._screenNavigator;

    public static BrainGameTime GameTime => BrainGame.Instance._gameTime;

    public static bool IsFullscreen => BrainGame.Instance._graphicsManager.IsFullScreen;

    public static bool PreferMultiSampling
    {
      get => BrainGame.Instance._graphicsManager.PreferMultiSampling;
      set => BrainGame.Instance._graphicsManager.PreferMultiSampling = value;
    }

    public static IHddIndicator HddAccessIcon
    {
      get => BrainGame.Instance._hddAccessIcon;
      set => BrainGame.Instance._hddAccessIcon = value;
    }

    public static Cursor GameCursor
    {
      get => BrainGame.Instance._gameCursor;
      set => BrainGame.Instance._gameCursor = value;
    }

    public static IntPtr WindowHandle => BrainGame.Instance.Window.Handle;

    public static SamplerState Sampler2D => BrainGame.Instance._sampler2D;

    public static SamplerState CurrentSampler => BrainGame.Instance._currentSampler;

    public static PlayerIndex CurrentControllerIndex
    {
      get => BrainGame.Instance._currentControllerIndex;
      set => BrainGame.Instance._currentControllerIndex = value;
    }

    public static Color ClearColor
    {
      get => BrainGame.Instance._clearColor;
      set => BrainGame.Instance._clearColor = value;
    }

    public static bool DisplayHDDAccessIcon
    {
      get => BrainGame.Instance._displayHDDAccessIcon;
      set => BrainGame.Instance._displayHDDAccessIcon = value;
    }

    public static Rectangle ViewportRectangle => BrainGame.Instance._viewportRectangle;

    public static Rectangle ScreenRectangle => BrainGame.Instance._screenRectangle;

    public static string BetaName
    {
      get => BrainGame.Instance._betaName;
      protected set => BrainGame.Instance._betaName = value;
    }

    public static string GameName
    {
      get => BrainGame.Instance._gameName;
      protected set => BrainGame.Instance._gameName = value;
    }

    public static string GameFolderName
    {
      get => BrainGame.Instance._gameFolderName;
      protected set => BrainGame.Instance._gameFolderName = value;
    }

    public static string GameVersion
    {
        get
        {
            return BrainGame.Instance.GetGameVersion();
        }
    }

    public static string GameUserFolderName
    {
      get
      {
        if (!string.IsNullOrEmpty(BrainGame._mainGameFolder))
          return BrainGame._mainGameFolder;

                StorageFolder folder = ApplicationData.Current.LocalFolder;
                
                BrainGame._mainGameFolder = folder.Path != null
                    ? Path.Combine(Path.Combine(folder.Path, "2BrainsGames"),
                    BrainGame.GameFolderName)
                    : throw new BrainException(string.IsNullOrEmpty(folder.Path)
                    + " not accessable!");

        if (!Directory.Exists(BrainGame._mainGameFolder))
          Directory.CreateDirectory(BrainGame._mainGameFolder);
        
        return BrainGame._mainGameFolder;
      }
    }

    public Camera2D ActiveCamera
    {
      get => this._activeCamera;
      set => this._activeCamera = value;
    }

    protected bool IsExiting { get; private set; }

    public static LanguageCode CurrentLanguage
    {
      get => BrainGame.Instance._currentLanguage;
      set
      {
        BrainGame.Instance._currentLanguage = value;
        LanguageManager.LanguageChanged();
        if (BrainGame.Instance._screenNavigator == null)
          return;
        if (BrainGame.Instance.OnLanguageChanged != null)
          BrainGame.Instance.OnLanguageChanged((object) BrainGame.Instance, new EventArgs());
        BrainGame.Instance._screenNavigator.LanguageChanged();
      }
    }

    public static Random Rand => BrainGame.Instance._rand;

    public static Texture2D RectTexture => BrainGame.Instance._rectTexture;

    public static LineBatch LineBatch => BrainGame.Instance._lineBatch;

    public static void DrawRectangleFilled(
      SpriteBatch spriteBatch,
      Rectangle rectangle,
      Color color)
    {
      spriteBatch.Draw(BrainGame.Instance._rectTexture, rectangle, color);
    }

    public static void DrawLine(Vector2 pt1, Vector2 pt2, Color color)
    {
      BrainGame.DrawLine(pt1, pt2, color, 1f);
    }

    public static void DrawLine(Vector2 pt1, Vector2 pt2, Color color, float layerDepth)
    {
      BrainGame.Instance._lineBatch.Batch(pt1, pt2, color, layerDepth);
    }

    public static void DrawRectangleFrame(
      SpriteBatch spriteBatch,
      Rectangle rectangle,
      Color color,
      int lineWidth)
    {
      if (lineWidth == 0)
        lineWidth = 1;
      spriteBatch.Draw(BrainGame.Instance._rectTexture, 
          new Rectangle(rectangle.Left, rectangle.Top, lineWidth, rectangle.Height), color);
      spriteBatch.Draw(BrainGame.Instance._rectTexture, 
          new Rectangle(rectangle.Right, rectangle.Top, lineWidth, rectangle.Height), color);
      spriteBatch.Draw(BrainGame.Instance._rectTexture, 
          new Rectangle(rectangle.Left, rectangle.Top, rectangle.Width, lineWidth), color);
      spriteBatch.Draw(BrainGame.Instance._rectTexture, 
          new Rectangle(rectangle.Left, rectangle.Bottom, rectangle.Width, lineWidth), color);
    }

    public static BrainSettings Settings
    {
      get => BrainGame.Instance._settings;
      set => BrainGame.Instance._settings = value;
    }

    public BrainGame()
    {
      BrainGame.Instance = this;
      this._graphicsManager = new GraphicsDeviceManager((Game) this);
    }

    public virtual Screen CreateScreen(ScreenNavigator owner, string screenId)
    {
      throw new BrainException(
          "Override BrainGame.CreateScreen() in your Game class to create Screen instances.");
    }

    protected virtual void LoadSettings()
    {
    }

    protected override void Initialize()
    {
      this._contentRootDir = "Content";
      this._resourceManager = new ResourceManager((IServiceProvider) this.Services, 
          this._contentRootDir);
      this.LoadSettings();
      this._sampleManager = new SampleManager(this);
      this.Components.Add((IGameComponent) this._sampleManager);
      this._musicManager = new MusicManager(this);
      this.Components.Add((IGameComponent) this._musicManager);
      if (this._settings.UseAchievements)
      {
        this._achievementsManager = new AchievementsManager();
        this._achievementsManager.Load("achievements");
      }
      this._activeCamera = new Camera2D();
      this._rand = new Random(DateTime.Now.Millisecond);
      this._rectTexture = new Texture2D(this._graphicsManager.GraphicsDevice, 1, 1);
      this._rectTexture.SetData<Color>(new Color[1]
      {
        Color.White
      });
      this._lineBatch = new LineBatch(this._graphicsManager.GraphicsDevice, 1f);
      this._screenNavigator = new ScreenNavigator(this);

        try
        {
            this._screenNavigator.LoadContent();
        }
        catch (Exception ex)
        {
            Debug.WriteLine("[ex] BrainGame - " + ex.Message);
        }

      this._spriteBatch = new SpriteBatch(this.GraphicsDevice);
      base.Initialize();
      this.Setup2DTextureSampling();
      this._currentSampler = BrainGame.Sampler2D;
      this._currentControllerIndex = PlayerIndex.One;
      this._clearColor = Color.Black;
      this._currentLanguage = LanguageManager.GetDefaultSystemLanguage();
      this.OnInitialize();
    }

    protected override void Update(Microsoft.Xna.Framework.GameTime gameTime)
    {
      base.Update(gameTime);
      if (this._accessingStorage || this.IsExiting)
        return;
      if (this._screenModeChanged)
      {
        this.SetupRenderViewport();
        this.SetupProjectionMatrix();
        this._screenModeChanged = false;
        this._screenNavigator.SendScreenModeChangedToScreens();
      }
      this._gameTime.Update(gameTime);
      this._screenNavigator.Update(this._gameTime);
    }

    protected override void Draw(Microsoft.Xna.Framework.GameTime gameTime)
    {
      if (this._screenModeChanged)
        return;
      this._screenNavigator.Draw();
      base.Draw(gameTime);
    }

    protected override void OnActivated(object sender, EventArgs args)
    {
      base.OnActivated(sender, args);
      this.BrainActivated(sender, args);
    }

    protected override void OnDeactivated(object sender, EventArgs args)
    {
      base.OnDeactivated(sender, args);
      this.BrainDeactivated(sender, args);
    }

    private void BrainActivated(object sender, EventArgs args)
    {
      if (this._settings != null && this._settings.WindowAlwaysActive)
        return;
      if (this._screenNavigator != null)
        this._screenNavigator.GameWindowActivated();
      if (this.OnGameActivated == null)
        return;
      this.OnGameActivated((object) this, new EventArgs());
    }

    private void BrainDeactivated(object sender, EventArgs args)
    {
      if (this._settings.WindowAlwaysActive || this._screenNavigator == null)
        return;
      this._screenNavigator.GameWindowDeactivated();
    }

    public void Quit()
    {
      this.IsExiting = true;
      this.Exit();
    }

    public static void QuitGame() => BrainGame.Instance.Quit();

    protected override void UnloadContent()
    {
      base.UnloadContent();
      if (this._musicManager != null)
        this._musicManager.StopMusic();
      if (this._sampleManager != null)
        this._sampleManager.StopAll();
      if (this._resourceManager == null)
        return;
      this._resourceManager.Unload();
    }

    protected virtual void OnInitialize()
    {
    }

    private void CreateAppFolders()
    {
      if (Directory.Exists(BrainGame.GameUserFolderName))
        return;
      Directory.CreateDirectory(BrainGame.GameUserFolderName);
    }

    private void Setup2DTextureSampling()
    {
      this._sampler2D = new SamplerState();
      this._sampler2D.AddressU = TextureAddressMode.Clamp;
      this._sampler2D.AddressV = TextureAddressMode.Clamp;
      this._sampler2D.AddressW = TextureAddressMode.Clamp;
      this._sampler2D.Filter = TextureFilter.Anisotropic;
    }

    protected virtual void SetupProjectionMatrix()
    {
    }

    public virtual void SetupRenderViewport()
    {
    }

    public static void AddComponent(GameComponent component)
    {
      BrainGame.Instance.Components.Add((IGameComponent) component);
    }

    public static void SetViewport(Viewport viewport)
    {
      BrainGame.Instance._viewport = new Viewport(viewport.Bounds);
      BrainGame.GraphicsManager.GraphicsDevice.Viewport = BrainGame.Instance._viewport;
      BrainGame.GraphicsManager.ApplyChanges();
      BrainGame.Instance._viewportRectangle = new Rectangle(viewport.X, viewport.Y, viewport.Width, viewport.Height);
    }

    public static void ResetGameTime() => BrainGame.Instance._gameTime.Reset();

    public static void ToggleFullScreen()
    {
      BrainGame.GraphicsManager.ToggleFullScreen();
      BrainGame.Instance._screenModeChanged = true;
    }

    public static ResourceManager CreateResourceManager()
    {
      return new ResourceManager((IServiceProvider) BrainGame.Instance.Services, BrainGame.Instance._contentRootDir);
    }

    private string GetGameVersion() => (string) null;

    public virtual void ProcessException(Exception ex)
    {
      try
      {
        if (this._graphicsManager != null)
        {
          if (this._graphicsManager.IsFullScreen)
          {
            this._graphicsManager.IsFullScreen = false;
            this._graphicsManager.ApplyChanges();
          }
        }
      }
      catch (Exception ex1)
      {
          Debug.WriteLine("[ex] " + ex1.Message);
      }
      throw ex;
    }

    private bool InitializeBeta() => true;

    public static float RandomizeFloat(float minValue, float maxValue, int decimals)
    {
      int maxValue1 = (int) (((double) maxValue - (double) minValue) * 100.0 * (double) decimals);
      return minValue + (float) ((double) BrainGame.Rand.Next(maxValue1) / (double) decimals / 100.0);
    }
  }
}
