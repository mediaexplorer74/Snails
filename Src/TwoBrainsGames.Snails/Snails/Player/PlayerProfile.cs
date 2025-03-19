
// Type: TwoBrainsGames.Snails.Player.PlayerProfile
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Localization;


namespace TwoBrainsGames.Snails.Player
{
  public class PlayerProfile : IDataFileSerializable
  {
    private const int DEFAULT_MUSIC_VOLUME = 80;
    private const int DEFAULT_SFX_VOLUME = 100;
    protected string _name;
    protected bool _overscanSet;
    protected bool _viewportSet;
    protected Viewport _viewport;
    protected bool _useGamePadVibration;
    protected bool _showTutorial;
    protected PlayerStats _playerStats;
    protected int _soundVolume;
    protected int _musicVolume;
    protected List<int> _tutorialTopicsRead;
    protected int _lastPlayedStageNr;
    protected ThemeType _lastPlayedTheme;
    protected bool _isAnonymous;
    protected List<int> _achievementsEarned;

    public string Name
    {
      get => this._name;
      set => this._name = value;
    }

    public bool IsAnonymous
    {
      get => this._isAnonymous;
      set => this._isAnonymous = value;
    }

    public bool OverscanSet
    {
      get => this._overscanSet;
      set => this._overscanSet = value;
    }

    public bool ViewportSet
    {
      get => this._viewportSet;
      set => this._viewportSet = value;
    }

    public Viewport Viewport
    {
      get => this._viewport;
      set
      {
        this._viewportSet = true;
        this._viewport = value;
      }
    }

    public bool UseGamePadVibration
    {
      get => this._useGamePadVibration;
      set => this._useGamePadVibration = value;
    }

    public PlayerStats PlayerStats
    {
      get => this._playerStats;
      set => this._playerStats = value;
    }

    public int SoundVolume
    {
      get => this._soundVolume;
      set
      {
        this._soundVolume = value >= 0 && value <= 100 ? value : throw new BrainException("Invalid value. Sound volume must be a number between 0 and 100");
      }
    }

    public int MusicVolume
    {
      get => this._musicVolume;
      set
      {
        this._musicVolume = value >= 0 && value <= 100 ? value : throw new BrainException("Invalid value. Music volume must be a number between 0 and 100");
      }
    }

    public bool AnyStagedPlayed => this.PlayerStats.TotalPlayingTime.TotalMilliseconds != 0.0;

    public int LastPlayedStageNr
    {
      get => this._lastPlayedStageNr;
      set => this._lastPlayedStageNr = value;
    }

    public ThemeType LastPlayedTheme
    {
      get => this._lastPlayedTheme;
      set => this._lastPlayedTheme = value;
    }

    public bool HasStartedNewGame => this.AnyStagedPlayed;

    public bool ScreenModeSelected { get; set; }

    public bool Fullscreen { get; set; }

    public string EULAAcceptedInVersion { get; set; }

    public List<int> AchievementsEarned => this._achievementsEarned;

    public PlayerProfile(string name)
    {
      this._name = name;
      this._overscanSet = false;
      this._viewportSet = false;
      this._isAnonymous = false;
      this._playerStats = new PlayerStats();
      this._useGamePadVibration = true;
      this._showTutorial = true;
      this._soundVolume = (int) ((double) Game1.GameSettings.DefaultSoundVolume * 100.0);
      this._musicVolume = (int) ((double) Game1.GameSettings.DefaultMusicVolume * 100.0);
      this._tutorialTopicsRead = new List<int>();
      this._lastPlayedStageNr = 1;
      this._lastPlayedTheme = ThemeType.ThemeA;
      this._playerStats.UnlockFirstStage(ThemeType.ThemeA);
      this._achievementsEarned = new List<int>();
    }

    public bool IsAchievementEarned(int type)
    {
      foreach (int num in this._achievementsEarned)
      {
        if (num == type)
          return true;
      }
      return false;
    }

    public void MarkAchievementEarned(int type)
    {
      foreach (int num in this._achievementsEarned)
      {
        if (num == type)
          return;
      }
      this._achievementsEarned.Add(type);
    }

    public void MarkAchievementNotEarned(int type)
    {
      foreach (int num in this._achievementsEarned)
      {
        if (num == type)
        {
          this._achievementsEarned.Remove(type);
          break;
        }
      }
    }

    public bool IsTutorialTopicRead(int id)
    {
      foreach (int num in this._tutorialTopicsRead)
      {
        if (num == id)
          return true;
      }
      return false;
    }

    public void MarkTutorialTopicAsRead(int id)
    {
      foreach (int num in this._tutorialTopicsRead)
      {
        if (num == id)
          return;
      }
      this._tutorialTopicsRead.Add(id);
    }

    public void ClearAchievements() => this.AchievementsEarned.Clear();

    public void InitFromDataFileRecord(DataFileRecord record)
    {
      this._name = record.GetFieldValue<string>("name");
      DataFileRecord dataFileRecord = record.SelectRecord("Settings");
      Game1.GameSettings.CustomStagesFolder = dataFileRecord.GetFieldValue<string>("customStagesFolder", Game1.GameSettings.CustomStagesFolder);
      this._overscanSet = dataFileRecord.GetFieldValue<bool>("overscanSet", this._overscanSet);
      this._viewportSet = dataFileRecord.GetFieldValue<bool>("viewportSet", this._viewportSet);
      if (this._viewportSet)
      {
        this._viewport.X = dataFileRecord.GetFieldValue<int>("viewportX", this._viewport.X);
        this._viewport.Y = dataFileRecord.GetFieldValue<int>("viewportY", this._viewport.Y);
        this._viewport.Width = dataFileRecord.GetFieldValue<int>("viewportWidth", this._viewport.Width);
        this._viewport.Height = dataFileRecord.GetFieldValue<int>("viewportHeight", this._viewport.Height);
      }
      this._useGamePadVibration = dataFileRecord.GetFieldValue<bool>("useGamePadVibration", this._useGamePadVibration);
      this._showTutorial = dataFileRecord.GetFieldValue<bool>("showTutorial", this._showTutorial);
      this._soundVolume = dataFileRecord.GetFieldValue<int>("soundVolume", this._soundVolume);
      this._musicVolume = dataFileRecord.GetFieldValue<int>("musicVolume", this._musicVolume);
      this._lastPlayedStageNr = dataFileRecord.GetFieldValue<int>("lastPlayedStageNr", this._lastPlayedStageNr);
      this.ScreenModeSelected = dataFileRecord.GetFieldValue<bool>("screenModeSelected", this.ScreenModeSelected);
      this.Fullscreen = dataFileRecord.GetFieldValue<bool>("fullscreen", this.Fullscreen);
      this.EULAAcceptedInVersion = dataFileRecord.GetFieldValue<string>("eulaAcceptedInVersion", this.EULAAcceptedInVersion);
      this._lastPlayedTheme = (ThemeType) Enum.Parse(typeof (ThemeType), dataFileRecord.GetFieldValue<string>("lastPlayedTheme", this._lastPlayedTheme.ToString()), true);
      BrainGame.CurrentLanguage = (LanguageCode) Enum.Parse(typeof (LanguageCode), dataFileRecord.GetFieldValue<string>("language", BrainGame.CurrentLanguage.ToString()), true);
      string fieldValue1 = dataFileRecord.GetFieldValue<string>("tutorialTopicsRead", "");
      this._tutorialTopicsRead = new List<int>();
      string str1 = fieldValue1;
      char[] chArray1 = new char[1]{ ';' };
      foreach (string str2 in str1.Split(chArray1))
      {
        if (!string.IsNullOrEmpty(str2))
          this._tutorialTopicsRead.Add(Convert.ToInt32(str2));
      }
      string fieldValue2 = dataFileRecord.GetFieldValue<string>("achievementsEarned", "");
      this._achievementsEarned = new List<int>();
      string str3 = fieldValue2;
      char[] chArray2 = new char[1]{ ';' };
      foreach (string str4 in str3.Split(chArray2))
      {
        if (!string.IsNullOrEmpty(str4))
          this._achievementsEarned.Add(Convert.ToInt32(str4));
      }
      this._playerStats.InitFromDataFileRecord(record.SelectRecord("Statistics"));
    }

    public virtual DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public virtual DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = new DataFileRecord("Profile");
      dataFileRecord.AddField("name", (object) this._name);
      DataFileRecord record = new DataFileRecord("Settings");
      record.AddField("customStagesFolder", (object) Game1.GameSettings.CustomStagesFolder);
      record.AddField("overscanSet", (object) this._overscanSet);
      record.AddField("viewportSet", (object) this._viewportSet);
      if (this._viewportSet)
      {
        record.AddField("viewportX", (object) this._viewport.X);
        record.AddField("viewportY", (object) this._viewport.Y);
        record.AddField("viewportWidth", (object) this._viewport.Width);
        record.AddField("viewportHeight", (object) this._viewport.Height);
      }
      record.AddField("useGamePadVibration", (object) this._useGamePadVibration);
      record.AddField("showTutorial", (object) this._showTutorial);
      record.AddField("soundVolume", (object) this._soundVolume);
      record.AddField("musicVolume", (object) this._musicVolume);
      record.AddField("lastPlayedStageNr", (object) this._lastPlayedStageNr);
      record.AddField("lastPlayedTheme", (object) this._lastPlayedTheme.ToString());
      record.AddField("language", (object) BrainGame.CurrentLanguage.ToString());
      record.AddField("screenModeSelected", (object) this.ScreenModeSelected);
      record.AddField("fullscreen", (object) this.Fullscreen);
      record.AddField("eulaAcceptedInVersion", (object) this.EULAAcceptedInVersion);
      string str1 = "";
      foreach (int num in this._tutorialTopicsRead)
        str1 = str1 + num.ToString() + ";";
      record.AddField("tutorialTopicsRead", (object) str1);
      string str2 = "";
      foreach (int num in this._achievementsEarned)
        str2 = str2 + num.ToString() + ";";
      record.AddField("achievementsEarned", (object) str2);
      dataFileRecord.AddRecord(record);
      dataFileRecord.AddRecord(this._playerStats.ToDataFileRecord());
      return dataFileRecord;
    }
  }
}
