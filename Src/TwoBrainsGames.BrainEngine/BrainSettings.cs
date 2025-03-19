
// Type: TwoBrainsGames.BrainEngine.BrainSettings
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using System;
using System.IO;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI;


namespace TwoBrainsGames.BrainEngine
{
  public class BrainSettings
  {
    private const float DEFAULT_GRAVITY = 10f;
    private DataFileRecord _dataFileRoot;
    public string NavigatorContentFolder;
    public string NavigatorContentId;
    public bool ShowDebugInfo;
    public bool ShowBoundingBoxes;
    public bool ShowSpriteFrame;
    public bool ShowSprites;
    public bool ShowResourceManagerData;
    public bool WindowAlwaysActive;
    public bool IsFullScreen;
    public int ScreenWidth;
    public int ScreenHeight;
    public float Gravity;
    public int ExplosionParticles;
    public float ExplosionMinVelocity;
    public float ExplosionMaxVelocity;
    private BrainSettings.GameplayModeType _gameplayMode;
    public BrainSettings.PlaformType Platform;
    public string PresentationModeString;
    public bool CreateAppFolders;
    public bool UseVSync;
    public bool UseAsyncLoading;
    public string StartupScreenGroup;
    public string StartupScreen;
    public bool FPSLocked;
    public bool SupportsShaderEffects;
    public CursorModes MenuCursorMode;
    public bool UseKeyboard;
    public bool UseMouse;
    public bool UseGamepad;
    public bool UseTouch;
    public bool WithRumbble;
    public bool UseAchievements;
    public string GameVersion;
    public string SupportContact;

    public static string DataFileReaderAssembly => "TwoBrainsGames.BrainEngine";

    public static string DataFileReaderType
    {
      get => "TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile.BinaryDataFileReader";
    }

    public static string DataFileWriterAssembly => "TwoBrainsGames.BrainEngine";

    public static string DataFileWriterType
    {
      get => "TwoBrainsGames.BrainEngine.Data.DataFiles.XmlDataFile.XmlDataFileWriter";
    }

    public string NavigatorControlContentFolder
    {
      get => Path.Combine(this.NavigatorContentFolder, "controls");
    }

    public float RatioNativeResolutionWidth
    {
      get => (float) this.ScreenWidth / (float) BrainGame.NativeScreenWidth;
    }

    public float RatioNativeResolutionHeight
    {
      get => (float) this.ScreenHeight / (float) BrainGame.NativeScreenHeight;
    }

    public float RatioPresentationNativeResolutionWidth
    {
      get => (float) this.ScreenWidth / (float) BrainGame.PresentationNativeScreenWidth;
    }

    public float RatioPresentationNativeResolutionHeight
    {
      get => (float) this.ScreenHeight / (float) BrainGame.PresentationNativeScreenHeight;
    }

    public bool PreferMultiSampling
    {
      get => BrainGame.PreferMultiSampling;
      set => BrainGame.PreferMultiSampling = value;
    }

    public BrainSettings.GameplayModeType GameplayMode
    {
      get => this._gameplayMode;
      set
      {
        if (value == this._gameplayMode)
          return;
        this._gameplayMode = value;
        if (BrainGame.ScreenNavigator == null)
          return;
        BrainGame.ScreenNavigator.GameplayModeChanged();
      }
    }

    protected BrainSettings()
    {
      this.GameplayMode = BrainSettings.GameplayModeType.Retail;
      this.Platform = BrainSettings.PlaformType.WP7;
    }

    public void Load(string resourceName)
    {
      this._dataFileRoot = BrainGame.ResourceManager.Load<DataFileRecord>(resourceName, 
          ResourceManager.ResourceManagerCacheType.Static);
      this.Initialize();
    }

    protected virtual void Initialize()
    {
      this.ShowBoundingBoxes = false;
      this.ShowSprites = true;
      this.ExplosionParticles = 6;
      this.ExplosionMinVelocity = 15f;
      this.ExplosionMaxVelocity = 20f;
      this.PresentationModeString = this.GetValue<string>("PresentationMode", (string) null, BrainSettings.ConfigSectionType.Engine);
      BrainGame.NativeScreenWidth = this.GetValue<int>("NativeScreenWidth", BrainSettings.ConfigSectionType.Engine);
      BrainGame.NativeScreenHeight = this.GetValue<int>("NativeScreenHeight", BrainSettings.ConfigSectionType.Engine);
      BrainGame.PresentationNativeScreenWidth = this.GetValue<int>("PresentationNativeScreenWidth", BrainSettings.ConfigSectionType.Engine);
      BrainGame.PresentationNativeScreenHeight = this.GetValue<int>("PresentationNativeScreenHeight", BrainSettings.ConfigSectionType.Engine);
      this.ScreenWidth = this.GetValue<int>("ScreenWidth", BrainSettings.ConfigSectionType.Engine);
      this.ScreenHeight = this.GetValue<int>("ScreenHeight", BrainSettings.ConfigSectionType.Engine);
      this.IsFullScreen = this.GetValue<bool>("IsFullscreen", BrainSettings.ConfigSectionType.Engine);
      this.Gravity = this.GetValue<float>("Gravity", 10f, BrainSettings.ConfigSectionType.Engine);
      this.PreferMultiSampling = this.GetValue<bool>("PreferMultiSampling", true, BrainSettings.ConfigSectionType.Engine);
      this.CreateAppFolders = this.GetValue<bool>("CreateAppFolders", true, BrainSettings.ConfigSectionType.Engine);
      this.UseKeyboard = this.GetValue<bool>("UseKeyboard", true, BrainSettings.ConfigSectionType.Engine);
      this.UseMouse = this.GetValue<bool>("UseMouse", true, BrainSettings.ConfigSectionType.Engine);
      this.UseGamepad = this.GetValue<bool>("UseGamepad", true, BrainSettings.ConfigSectionType.Engine);
      this.UseTouch = this.GetValue<bool>("UseTouch", true, BrainSettings.ConfigSectionType.Engine);
      this.WithRumbble = this.GetValue<bool>("WithRumbble", true, BrainSettings.ConfigSectionType.Engine);
      this.MenuCursorMode = (CursorModes) Enum.Parse(typeof (CursorModes), this.GetValue<string>("MenuCursorMode", BrainSettings.ConfigSectionType.Engine), true);
      this.UseVSync = this.GetValue<bool>("UseVSync", BrainSettings.ConfigSectionType.Engine);
      this.UseAsyncLoading = this.GetValue<bool>("UseAsyncLoading", BrainSettings.ConfigSectionType.Engine);
      this.NavigatorContentFolder = this.GetValue<string>("NavigatorContentFolder", BrainSettings.ConfigSectionType.Engine);
      this.NavigatorContentId = this.GetValue<string>("NavigatorContentId", BrainSettings.ConfigSectionType.Engine);
      this.StartupScreenGroup = this.GetValue<string>("StartupScreenGroup", BrainSettings.ConfigSectionType.Engine);
      this.StartupScreen = this.GetValue<string>("StartupScreen", BrainSettings.ConfigSectionType.Engine);
      this.FPSLocked = this.GetValue<bool>("FPSLocked", BrainSettings.ConfigSectionType.Engine);
      this.SupportsShaderEffects = this.GetValue<bool>("SupportsShaderEffects", BrainSettings.ConfigSectionType.Engine);
      this.UseAchievements = this.GetValue<bool>("UseAchievements", true, BrainSettings.ConfigSectionType.Engine);
      this.GameVersion = this.GetValue<string>("GameVersion", BrainSettings.ConfigSectionType.Engine);
      this.SupportContact = this.GetValue<string>("SupportContact", BrainSettings.ConfigSectionType.Engine);
      this.WindowAlwaysActive = false;
      this.ShowSprites = true;
    }

    protected T GetValue<T>(
      string paramName,
      T defaultVal,
      BrainSettings.ConfigSectionType configSection)
    {
      return this.GetValue<T>(paramName, defaultVal, configSection, false);
    }

    protected T GetValue<T>(string paramName, BrainSettings.ConfigSectionType configSection)
    {
      return this.GetValue<T>(paramName, default (T), configSection, true);
    }

    private T GetValue<T>(
      string paramName,
      T defaultVal,
      BrainSettings.ConfigSectionType configSection,
      bool exceptionIfNotFound)
    {
      this.AssertLoaded();
      DataFileRecord dataFileRecord = this._dataFileRoot.SelectRecord(configSection.ToString() + "\\" + paramName);
      if (dataFileRecord == null)
      {
        if (exceptionIfNotFound)
          throw new BrainException("Settings with name [" + paramName + "] does not exist in the game settings file");
        return defaultVal;
      }
      if (dataFileRecord.GetFieldByName("value") == null)
      {
        if (exceptionIfNotFound)
          throw new BrainException("Settings with name [value] does not exist in the game settings file");
        return defaultVal;
      }
      foreach (DataFileRecord selectRecord in dataFileRecord.SelectRecords("Condition"))
      {
        if (this.EvaluateCondition(selectRecord))
        {
          dataFileRecord = selectRecord;
          break;
        }
      }
      return dataFileRecord.GetFieldValue<T>("value", defaultVal);
    }

    private bool EvaluateCondition(DataFileRecord condRecord)
    {
      if (condRecord == null)
        return false;
      bool condition = false;
      DataFileField fieldByName1 = condRecord.GetFieldByName("platform");
      if (fieldByName1 != null && fieldByName1.Value != null && (string) fieldByName1.Value == this.Platform.ToString())
        condition = true;
      DataFileField fieldByName2 = condRecord.GetFieldByName("presentation");
      if (fieldByName2 != null && fieldByName2.Value != null && (string) fieldByName2.Value == this.PresentationModeString)
        condition = true;
      DataFileField fieldByName3 = condRecord.GetFieldByName("useTouch");
      if (fieldByName3 != null && fieldByName3.Value != null && Convert.ToBoolean((string) fieldByName3.Value) == this.UseTouch)
        condition = true;
      return condition;
    }

    private void AssertLoaded()
    {
      if (this._dataFileRoot == null)
        throw new BrainException("Settings not loaded. call BrainSettings.Load()");
    }

    [Flags]
    public enum PlaformType
    {
      None = 0,
      Windows = 1,
      XBox = 2,
      WP7 = 4,
      MacOSX = 8,
      Linux = 16, // 0x00000010
      iOS = 32, // 0x00000020
      iPhone = 64, // 0x00000040
      iPad = 128, // 0x00000080
      Andriod = 256, // 0x00000100
      Windows8 = 512, // 0x00000200
      Windows8RT = 1024, // 0x00000400
      All = Windows8RT | Windows8 | Andriod | iPad | iPhone | iOS | Linux | MacOSX | WP7 | XBox | Windows, // 0x000007FF
      WindowsXbox = XBox | Windows, // 0x00000003
      MonoGame = Andriod | iPad | iPhone | iOS | Linux | MacOSX, // 0x000001F8
    }

    public enum GameplayModeType
    {
      Retail,
      Demo,
      Beta,
    }

    protected enum ConfigSectionType
    {
      Engine,
      Game,
      Presentation,
      Debug,
    }
  }
}
