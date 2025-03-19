
// Type: TwoBrainsGames.Snails.Player.PlayersProfileManager
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using System;
using System.Collections.Generic;
using System.IO;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.Player
{
  internal class PlayersProfileManager
  {
    protected string SAVE_FILENAME = "PlayerProfile.bdf";
    protected string DEFAULT_PLAYER_NAME = "Player 1";
    protected string _seletedProfileName;
    protected List<PlayerProfile> _profiles;
    protected PlayerProfile _currentProfile;

    public bool IsCompleted { get; protected set; }

    public PlayerProfile CurrentProfile
    {
      get => this._currentProfile;
      set => this._currentProfile = value;
    }

    public PlayersProfileManager()
    {
      this._seletedProfileName = string.Empty;
      this._profiles = new List<PlayerProfile>(1);
      this._currentProfile = (PlayerProfile) null;
      this.IsCompleted = true;
    }

    protected virtual string GetUserFilename() => this.SAVE_FILENAME;

    public void AddProfile(string name)
    {
      PlayerProfile playerProfile = new PlayerProfile(name);
      this._profiles.Add(playerProfile);
      if (this._profiles.Count != 1)
        return;
      this._currentProfile = playerProfile;
      this._seletedProfileName = name;
    }

    public void RemoveProfile(string name)
    {
      this._profiles.RemoveAll<PlayerProfile>((Func<PlayerProfile, bool>) (match => match.Name == name));
      if (!(name == this._seletedProfileName))
        return;
      this._seletedProfileName = string.Empty;
      if (this._profiles == null || this._profiles.Count != 1)
        return;
      this.SelectProfile("");
    }

    public bool ExistsProfile(string name)
    {
      return this._profiles.Exists<PlayerProfile>((Func<PlayerProfile, bool>) (match => match.Name == name));
    }

    public bool NewProfile(string name)
    {
      if (!this.ExistsProfile(name))
      {
        this.AddProfile(name);
        return true;
      }
      this.SelectProfile(name);
      return false;
    }

    public void SelectProfile(string name)
    {
      if (string.IsNullOrEmpty(name) && this._profiles != null && this._profiles.Count == 1)
      {
        this._currentProfile = this._profiles[0];
        this._seletedProfileName = this._profiles[0].Name;
      }
      this._currentProfile = this.GetProfile(name);
      if (this._currentProfile != null)
      {
        this._seletedProfileName = name;
        Levels.CurrentTheme = this._currentProfile.LastPlayedTheme;
        Levels.CurrentStageNr = this._currentProfile.LastPlayedStageNr;
        BrainGame.SampleManager.MasterVolume = (float) Game1.ProfilesManager.CurrentProfile.SoundVolume / 100f;
        BrainGame.MusicManager.MasterVolume = (float) Game1.ProfilesManager.CurrentProfile.MusicVolume / 100f;
      }
      Achievements.Register();
    }

    public PlayerProfile GetSelectedProfile() => this.GetProfile(this._seletedProfileName);

    public PlayerProfile GetProfile(string name)
    {
      return this._profiles.Find<PlayerProfile>((Func<PlayerProfile, bool>) (match => match.Name == name));
    }

    public void DeleteProfile()
    {
      string userFilename = this.GetUserFilename();
      if (!File.Exists(userFilename))
        return;
      File.Delete(userFilename);
    }

    public virtual int LoadProfile()
    {
      if (this.ExistsProfile(this._seletedProfileName))
      {
        this.SelectProfile(this._seletedProfileName);
        return 1;
      }
      if (this._currentProfile == null)
        this.CreateProfile((string) null);
      return 0;
    }

    public void CreateAnonymousProfile()
    {
      if (this._currentProfile != null)
        return;
      this.NewProfile(this.DEFAULT_PLAYER_NAME);
      this._currentProfile.IsAnonymous = true;
    }

    public virtual void CreateProfile(string name)
    {
      if (this._currentProfile == null)
      {
        if (string.IsNullOrEmpty(name))
          name = this.DEFAULT_PLAYER_NAME;
        this.NewProfile(name);
      }
      this._currentProfile.Viewport = BrainGame.Viewport;
      Achievements.Register();
      this.Save();
    }

    protected DataFile GetDataFileToSave()
    {
      DataFile dataFileToSave = new DataFile();
      DataFileRecord dataFileRecord = new DataFileRecord("Players");
      dataFileRecord.AddField("defaultProfile", (object) this._seletedProfileName);
      foreach (PlayerProfile profile in this._profiles)
        dataFileRecord.AddRecord(profile.ToDataFileRecord());
      dataFileToSave.RootRecord = dataFileRecord;
      return dataFileToSave;
    }

    protected void LoadDataFile(DataFile dataFile)
    {
      if (dataFile == null)
        return;
      this._seletedProfileName = dataFile.RootRecord.GetFieldValue<string>("defaultProfile");
      DataFileRecordList dataFileRecordList = dataFile.RootRecord.SelectRecords("Profile");
      if (dataFileRecordList == null || dataFileRecordList.Count <= 0)
        return;
      this._profiles = new List<PlayerProfile>(dataFileRecordList.Count);
      foreach (DataFileRecord record in dataFileRecordList)
      {
        PlayerProfile playerProfile = new PlayerProfile(string.Empty);
        playerProfile.InitFromDataFileRecord(record);
        this._profiles.Add(playerProfile);
      }
    }

    public virtual void Save()
    {
    }

    public virtual void BeginLoad()
    {
    }

    public virtual void Load()
    {
    }

    public virtual void Update()
    {
    }

    public virtual void SignedInPlayer()
    {
    }
  }
}
