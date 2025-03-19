
// Type: TwoBrainsGames.Snails.Configuration.GameSettings
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using System;
using System.IO;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;


namespace TwoBrainsGames.Snails.Configuration
{
  public class GameSettings : BrainSettings, IDataFileSerializable
  {
    public const string CustomStagesExtension = "customStage";
    public const string GAME_SETTINGS_FILE = "snails_debug_settings.xml";
    public const string StagesOutputFolder = "..\\..\\..\\..\\SnailsResourcesCustom";
    public const string StageDataOutputFolder = "..\\..\\..\\..\\SnailsResourcesCustom\\Stages";
    public bool ShowBoardFrames;
    public bool ShowBoardCoordinates;
    public bool ShowPaths;
    public bool ShowObjectIds;
    public bool ShowQuadtree;
    public bool ShowBoardGrid;
    public bool ShowTiles;
    public bool ShowTheInstructionBar;
    public bool ShowStageStats;
    public bool ShowMapScrollIndicators;
    public bool ShowConfirmationMenus;
    public bool ShowFooterMessage;
    public bool PauseGameWhenFocusLost;
    public bool PauseInTutorial;
    public bool AllowToggleFullScreen;
    public int StartupStageNr;
    public ThemeType StartupTheme;
    public bool UseButtonIcons;
    public bool ShowContinueOption;
    public GameSettings.GameEntryPoint EntryPoint;
    public bool DeletePlayerProfile;
    public bool MinimapVisible;
    public string CustomStagesFolder;
    public bool AllowCustomStages;
    public bool AllowDebugWindow;
    public bool AllowCustomUserDataFolder;
    public bool AllowOverscanAdjustment;
    public bool WithAppStore;
    public bool BackQuitsGameOnIntroPicture;
    public bool WithToolSelectionShortcutKeys;
    public int MaxTools;
    public float MaxZoomOut;
    public float LeafTransitionScale;
    public bool ShowEULA;
    public bool ShowGameSettingsWindow;
    public bool ShowCloseTutorialMessage;
    public bool ShowCloseTutorialShortcut;
    public bool ShowBackButtonInThemeSelection;
    public bool ShowCursor;
    public bool ShowAutoSaveScreen;
    public bool ShowGameTitleInPause;
    public bool ShowQuitOptions;
    public bool AsyncProfileLoading;
    public bool UseWaterEffect;
    public string GameplayRecordingPath;
    public bool AllStagesUnlocked;
    public LanguageType Languages;
    public float DefaultSoundVolume;
    public float DefaultMusicVolume;

    public GameSettings.PresentationType PresentationMode
    {
      get
      {
        return this.PresentationModeString == "LD" ? GameSettings.PresentationType.LD : GameSettings.PresentationType.HD;
      }
    }

    protected override void Initialize()
    {
      base.Initialize();
      this.PauseInTutorial = this.GetValue<bool>("PauseInTutorial", BrainSettings.ConfigSectionType.Game);
      this.UseButtonIcons = this.GetValue<bool>("UseButtonIcons", BrainSettings.ConfigSectionType.Game);
      this.ShowContinueOption = this.GetValue<bool>("ShowContinueOption", BrainSettings.ConfigSectionType.Game);
      this.ShowConfirmationMenus = this.GetValue<bool>("ShowConfirmationMenus", BrainSettings.ConfigSectionType.Game);
      this.MinimapVisible = this.GetValue<bool>("MinimapVisible", BrainSettings.ConfigSectionType.Game);
      this.ShowTheInstructionBar = this.GetValue<bool>("ShowTheInstructionBar", BrainSettings.ConfigSectionType.Game);
      this.UseWaterEffect = this.GetValue<bool>("UseWaterEffect", BrainSettings.ConfigSectionType.Game);
      this.AllowToggleFullScreen = this.GetValue<bool>("AllowToggleFullScreen", BrainSettings.ConfigSectionType.Game);
      this.ShowBackButtonInThemeSelection = this.GetValue<bool>("ShowBackButtonInThemeSelection", BrainSettings.ConfigSectionType.Game);
      this.ShowEULA = this.GetValue<bool>("ShowEULA", BrainSettings.ConfigSectionType.Game);
      this.AllowCustomUserDataFolder = this.GetValue<bool>("AllowCustomUserDataFolder", BrainSettings.ConfigSectionType.Game);
      this.AllowOverscanAdjustment = this.GetValue<bool>("AllowOverscanAdjustment", BrainSettings.ConfigSectionType.Game);
      this.WithAppStore = this.GetValue<bool>("WithAppStore", BrainSettings.ConfigSectionType.Game);
      this.BackQuitsGameOnIntroPicture = this.GetValue<bool>("BackQuitsGameOnIntroPicture", BrainSettings.ConfigSectionType.Game);
      this.WithToolSelectionShortcutKeys = this.GetValue<bool>("WithToolSelectionShortcutKeys", BrainSettings.ConfigSectionType.Game);
      this.MaxTools = this.GetValue<int>("MaxTools", BrainSettings.ConfigSectionType.Game);
      this.MaxZoomOut = this.GetValue<float>("MaxZoomOut", BrainSettings.ConfigSectionType.Game);
      this.ShowCloseTutorialMessage = this.GetValue<bool>("ShowCloseTutorialMessage", BrainSettings.ConfigSectionType.Game);
      this.ShowCloseTutorialShortcut = this.GetValue<bool>("ShowCloseTutorialShortcut", BrainSettings.ConfigSectionType.Game);
      this.ShowCursor = this.GetValue<bool>("ShowCursor", BrainSettings.ConfigSectionType.Game);
      this.ShowAutoSaveScreen = this.GetValue<bool>("ShowAutoSaveScreen", BrainSettings.ConfigSectionType.Game);
      this.ShowGameTitleInPause = this.GetValue<bool>("ShowGameTitleInPause", BrainSettings.ConfigSectionType.Game);
      this.ShowQuitOptions = this.GetValue<bool>("ShowQuitOptions", BrainSettings.ConfigSectionType.Game);
      this.ShowFooterMessage = this.GetValue<bool>("ShowFooterMessage", BrainSettings.ConfigSectionType.Game);
      this.AsyncProfileLoading = this.GetValue<bool>("AsyncProfileLoading", BrainSettings.ConfigSectionType.Game);
      this.ShowGameSettingsWindow = this.GetValue<bool>("ShowGameSettingsWindow", BrainSettings.ConfigSectionType.Game);
      this.Languages = (LanguageType) Enum.Parse(typeof (LanguageType), this.GetValue<string>("Languages", BrainSettings.ConfigSectionType.Game), true);
      this.DefaultSoundVolume = this.GetValue<float>("DefaultSoundVolume", BrainSettings.ConfigSectionType.Game);
      this.DefaultMusicVolume = this.GetValue<float>("DefaultMusicVolume", BrainSettings.ConfigSectionType.Game);
      this.AllowDebugWindow = this.GetValue<bool>("AllowDebugWindow", BrainSettings.ConfigSectionType.Debug);
      this.ShowBoardFrames = this.GetValue<bool>("ShowBoardFrames", false, BrainSettings.ConfigSectionType.Debug);
      this.ShowBoardCoordinates = this.GetValue<bool>("ShowBoardCoordinates", false, BrainSettings.ConfigSectionType.Debug);
      this.ShowPaths = this.GetValue<bool>("ShowPaths", false, BrainSettings.ConfigSectionType.Debug);
      this.ShowQuadtree = this.GetValue<bool>("ShowQuadtree", false, BrainSettings.ConfigSectionType.Debug);
      this.ShowBoardGrid = this.GetValue<bool>("ShowBoardGrid", false, BrainSettings.ConfigSectionType.Debug);
      this.ShowTiles = this.GetValue<bool>("ShowTiles", false, BrainSettings.ConfigSectionType.Debug);
      this.ShowDebugInfo = this.GetValue<bool>("ShowDebugInfo", false, BrainSettings.ConfigSectionType.Debug);
      this.ShowResourceManagerData = this.GetValue<bool>("ShowResourceManagerData", false, BrainSettings.ConfigSectionType.Debug);
      this.ShowStageStats = this.GetValue<bool>("ShowStageStats", false, BrainSettings.ConfigSectionType.Debug);
      this.StartupStageNr = this.GetValue<int>("StartupStageNr", 1, BrainSettings.ConfigSectionType.Debug);
      this.StartupTheme = (ThemeType) Enum.Parse(typeof (ThemeType), this.GetValue<string>("StartupTheme", ThemeType.ThemeA.ToString(), BrainSettings.ConfigSectionType.Debug), true);
      this.DeletePlayerProfile = this.GetValue<bool>("DeletePlayerProfile", false, BrainSettings.ConfigSectionType.Debug);
      this.AllowCustomStages = this.GetValue<bool>("AllowCustomStages", false, BrainSettings.ConfigSectionType.Debug);
      this.CustomStagesFolder = this.AllowCustomStages ? Path.Combine(BrainGame.GameUserFolderName, "CustomStages") : (string) null;
      this.ShowDebugInfo = false;
      this.PauseGameWhenFocusLost = true;
    }

    public void SaveToFile()
    {
    }

    public void LoadFromFile()
    {
    }

    public void InitFromDataFileRecord(DataFileRecord record)
    {
      this.IsFullScreen = record.GetFieldValue<bool>("IsFullscreen");
      this.ShowBoardFrames = record.GetFieldValue<bool>("ShowBoardFrames");
      this.ShowBoardCoordinates = record.GetFieldValue<bool>("ShowBoardCoordinates");
      this.ShowPaths = record.GetFieldValue<bool>("ShowPaths");
      this.ShowObjectIds = record.GetFieldValue<bool>("ShowObjectIds");
      this.FPSLocked = record.GetFieldValue<bool>("FPSLocked");
      this.ShowQuadtree = record.GetFieldValue<bool>("ShowQuadtree", false);
      this.ShowTiles = record.GetFieldValue<bool>("ShowTiles", true);
      this.ShowTheInstructionBar = record.GetFieldValue<bool>("ShowTheInstructionBar");
      this.ShowStageStats = record.GetFieldValue<bool>("ShowStageStats");
      this.ShowMapScrollIndicators = record.GetFieldValue<bool>("ShowMapScrollIndicators");
      this.PauseGameWhenFocusLost = record.GetFieldValue<bool>("PauseGameWhenFocusLost");
      this.StartupStageNr = record.GetFieldValue<int>("StartupStageNr", 1);
      this.StartupTheme = (ThemeType) Enum.Parse(typeof (ThemeType), record.GetFieldValue<string>("StartupTheme", ThemeType.ThemeA.ToString()), true);
      this.DeletePlayerProfile = record.GetFieldValue<bool>("DeletePlayerProfile", false);
      this.ShowSprites = record.GetFieldValue<bool>("ShowSprites", true);
      this.ShowDebugInfo = record.GetFieldValue<bool>("ShowDebugInfo", false);
      this.WindowAlwaysActive = record.GetFieldValue<bool>("WindowAlwaysActive", false);
      this.AllStagesUnlocked = record.GetFieldValue<bool>("AllStagesUnlocked", false);
      string fieldValue = record.GetFieldValue<string>("EntryPoint", (string) null);
      if (fieldValue != null)
        this.EntryPoint = (GameSettings.GameEntryPoint) Enum.Parse(typeof (GameSettings.GameEntryPoint), fieldValue, true);
      this.ShowBoundingBoxes = record.GetFieldValue<bool>("ShowBB", false);
      this.ShowSpriteFrame = record.GetFieldValue<bool>("ShowSpriteFrame", false);
      this.GameplayMode = (BrainSettings.GameplayModeType) Enum.Parse(typeof (BrainSettings.GameplayModeType), record.GetFieldValue<string>("GameplayMode"), true);
      this.GameplayRecordingPath = record.GetFieldValue<string>("GameplayRecordingPath", string.Empty);
      this.DefaultSoundVolume = record.GetFieldValue<float>("DefaultSoundVolume", 1f);
    }

    public DataFileRecord ToDataFileRecord()
    {
      DataFileRecord dataFileRecord = new DataFileRecord(nameof (GameSettings));
      dataFileRecord.AddField("IsFullscreen", (object) this.IsFullScreen);
      dataFileRecord.AddField("ShowBoardFrames", (object) this.ShowBoardFrames);
      dataFileRecord.AddField("ShowBoardCoordinates", (object) this.ShowBoardCoordinates);
      dataFileRecord.AddField("ShowPaths", (object) this.ShowPaths);
      dataFileRecord.AddField("ShowObjectIds", (object) this.ShowObjectIds);
      dataFileRecord.AddField("FPSLocked", (object) this.FPSLocked);
      dataFileRecord.AddField("ShowQuadtree", (object) this.ShowQuadtree);
      dataFileRecord.AddField("ShowTiles", (object) this.ShowTiles);
      dataFileRecord.AddField("ShowSprites", (object) this.ShowSprites);
      dataFileRecord.AddField("ShowTheInstructionBar", (object) this.ShowTheInstructionBar);
      dataFileRecord.AddField("ShowStageStats", (object) this.ShowStageStats);
      dataFileRecord.AddField("ShowMapScrollIndicators", (object) this.ShowMapScrollIndicators);
      dataFileRecord.AddField("ShowDebugInfo", (object) this.ShowDebugInfo);
      dataFileRecord.AddField("WindowAlwaysActive", (object) this.WindowAlwaysActive);
      dataFileRecord.AddField("PauseGameWhenFocusLost", (object) this.PauseGameWhenFocusLost);
      dataFileRecord.AddField("StartupStageNr", (object) this.StartupStageNr);
      dataFileRecord.AddField("StartupTheme", (object) this.StartupTheme.ToString());
      dataFileRecord.AddField("EntryPoint", (object) this.EntryPoint.ToString());
      dataFileRecord.AddField("ShowBB", (object) this.ShowBoundingBoxes.ToString());
      dataFileRecord.AddField("ShowSpriteFrame", (object) this.ShowSpriteFrame.ToString());
      dataFileRecord.AddField("DeletePlayerProfile", (object) this.DeletePlayerProfile);
      dataFileRecord.AddField("GameplayMode", (object) this.GameplayMode.ToString());
      dataFileRecord.AddField("GameplayRecordingPath", (object) this.GameplayRecordingPath);
      dataFileRecord.AddField("AllStagesUnlocked", (object) this.AllStagesUnlocked);
      dataFileRecord.AddField("Languages", (object) this.Languages.ToString());
      dataFileRecord.AddField("DefaultSoundVolume", (object) this.DefaultSoundVolume);
      return dataFileRecord;
    }

    public enum GameEntryPoint
    {
      Beginning,
      MainMenu,
      StageBriefing,
      StageEditor,
      Awards,
    }

    public enum PresentationType
    {
      HD,
      LD,
    }
  }
}
