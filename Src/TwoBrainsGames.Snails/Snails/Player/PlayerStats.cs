
// Type: TwoBrainsGames.Snails.Player.PlayerStats
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using System;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.Snails.ToolObjects;


namespace TwoBrainsGames.Snails.Player
{
  public class PlayerStats : IDataFileSerializable
  {
    protected List<PlayerStageStats> _stagesStat;
    protected int _unlockedStages = 1;
    private long _tutorialToolsFlags;
    protected TimeSpan _totalPlayingTime;
    protected TimeSpan _totalRunningTime;
    protected int _totalSnailsSafe;
    protected int _totalSnailsKingSafe;
    protected int _totalSnailsDeadByFire;
    protected int _totalSnailsDeadBySpikes;
    protected int _totalSnailsDeadByDynamite;
    protected int _totalSnailsDeadByCrate;
    protected int _totalSnailsDeadByWater;
    protected int _totalSnailsDeadByAcid;
    protected int _totalSnailsDeadByLaser;
    protected int _totalSnailsDeadBySacrifice;
    protected int _totalSnailsDeadByCrateExplosion;
    protected int _totalSnailsDeadByOutOfStage;
    protected int _totalSnailsDeadByEvilSnail;
    protected int _totalGoldMedals;
    protected int _totalSilverMedals;
    protected int _totalBronzeMedals;
    protected int _totalBoots;

    public List<PlayerStageStats> StagesStat
    {
      get => this._stagesStat;
      set => this._stagesStat = value;
    }

    public TimeSpan TotalPlayingTime
    {
      get => this._totalPlayingTime;
      set => this._totalPlayingTime = value;
    }

    public TimeSpan TotalRunningTime
    {
      get => this._totalRunningTime;
      set => this._totalRunningTime = value;
    }

    public int TotalSnailsSafe
    {
      get => this._totalSnailsSafe;
      set
      {
        this._totalSnailsSafe = value;
        BrainGame.AchievementsManager.Notify(3);
        BrainGame.AchievementsManager.Notify(4);
        BrainGame.AchievementsManager.Notify(5);
      }
    }

    public int TotalSnailsKingSafe
    {
      get => this._totalSnailsKingSafe;
      set
      {
        this._totalSnailsKingSafe = value;
        BrainGame.AchievementsManager.Notify(8);
        BrainGame.AchievementsManager.Notify(9);
      }
    }

    public int TotalSnailsDead
    {
      get
      {
        return this._totalSnailsDeadByFire + this._totalSnailsDeadBySpikes + this._totalSnailsDeadByDynamite + this._totalSnailsDeadByCrate + this._totalSnailsDeadByWater + this._totalSnailsDeadByLaser + this._totalSnailsDeadByAcid + this._totalSnailsDeadByCrateExplosion + this._totalSnailsDeadByOutOfStage + this._totalSnailsDeadBySacrifice + this._totalSnailsDeadByEvilSnail;
      }
    }

    public int SnailsDeadInDifferentWays
    {
      get
      {
        int deadInDifferentWays = 0;
        if (this._totalSnailsDeadByFire > 0)
          ++deadInDifferentWays;
        if (this._totalSnailsDeadBySpikes > 0)
          ++deadInDifferentWays;
        if (this._totalSnailsDeadByDynamite > 0)
          ++deadInDifferentWays;
        if (this._totalSnailsDeadByCrate > 0)
          ++deadInDifferentWays;
        if (this._totalSnailsDeadByWater > 0)
          ++deadInDifferentWays;
        if (this._totalSnailsDeadByLaser > 0)
          ++deadInDifferentWays;
        if (this._totalSnailsDeadByAcid > 0)
          ++deadInDifferentWays;
        if (this._totalSnailsDeadByCrateExplosion > 0)
          ++deadInDifferentWays;
        if (this._totalSnailsDeadByOutOfStage > 0)
          ++deadInDifferentWays;
        if (this._totalSnailsDeadBySacrifice > 0)
          ++deadInDifferentWays;
        if (this._totalSnailsDeadByEvilSnail > 0)
          ++deadInDifferentWays;
        return deadInDifferentWays;
      }
    }

    public int TotalSnailsDeadByFire
    {
      get => this._totalSnailsDeadByFire;
      set
      {
        this._totalSnailsDeadByFire = value;
        BrainGame.AchievementsManager.Notify(10);
        BrainGame.AchievementsManager.Notify(48);
      }
    }

    public int TotalSnailsDeadBySpikes
    {
      get => this._totalSnailsDeadBySpikes;
      set
      {
        this._totalSnailsDeadBySpikes = value;
        BrainGame.AchievementsManager.Notify(17);
        BrainGame.AchievementsManager.Notify(48);
      }
    }

    public int TotalSnailsDeadByDynamite
    {
      get => this._totalSnailsDeadByDynamite;
      set
      {
        this._totalSnailsDeadByDynamite = value;
        BrainGame.AchievementsManager.Notify(14);
        BrainGame.AchievementsManager.Notify(48);
      }
    }

    public int TotalSnailsDeadByCrateExplosion
    {
      get => this._totalSnailsDeadByCrateExplosion;
      set
      {
        this._totalSnailsDeadByCrateExplosion = value;
        BrainGame.AchievementsManager.Notify(48);
      }
    }

    public int TotalSnailsDeadByCrate
    {
      get => this._totalSnailsDeadByCrate;
      set
      {
        this._totalSnailsDeadByCrate = value;
        BrainGame.AchievementsManager.Notify(48);
      }
    }

    public int TotalSnailsDeadByWater
    {
      get => this._totalSnailsDeadByWater;
      set
      {
        this._totalSnailsDeadByWater = value;
        BrainGame.AchievementsManager.Notify(48);
      }
    }

    public int TotalSnailsDeadByAcid
    {
      get => this._totalSnailsDeadByAcid;
      set
      {
        this._totalSnailsDeadByAcid = value;
        BrainGame.AchievementsManager.Notify(48);
      }
    }

    public int TotalSnailsDeadByLaser
    {
      get => this._totalSnailsDeadByLaser;
      set
      {
        this._totalSnailsDeadByLaser = value;
        BrainGame.AchievementsManager.Notify(18);
        BrainGame.AchievementsManager.Notify(48);
      }
    }

    public int TotalSnailsDeadBySacrifice
    {
      get => this._totalSnailsDeadBySacrifice;
      set
      {
        this._totalSnailsDeadBySacrifice = value;
        BrainGame.AchievementsManager.Notify(48);
      }
    }

    public int TotalSnailsDeadByOutOfStage
    {
      get => this._totalSnailsDeadByOutOfStage;
      set
      {
        this._totalSnailsDeadByOutOfStage = value;
        BrainGame.AchievementsManager.Notify(48);
      }
    }

    public int TotalSnailsDeadByEvilSnail
    {
      get => this._totalSnailsDeadByEvilSnail;
      set
      {
        this._totalSnailsDeadByEvilSnail = value;
        BrainGame.AchievementsManager.Notify(48);
      }
    }

    public int TotalBronzeCoins
    {
      get => this._totalBronzeMedals;
      set
      {
        this._totalBronzeMedals = value;
        BrainGame.AchievementsManager.Notify(25);
        BrainGame.AchievementsManager.Notify(26);
        BrainGame.AchievementsManager.Notify(34);
      }
    }

    public int TotalSilverCoins
    {
      get => this._totalSilverMedals;
      set
      {
        this._totalSilverMedals = value;
        BrainGame.AchievementsManager.Notify(29);
        BrainGame.AchievementsManager.Notify(30);
        BrainGame.AchievementsManager.Notify(35);
      }
    }

    public int TotalGoldCoins
    {
      get => this._totalGoldMedals;
      set
      {
        this._totalGoldMedals = value;
        BrainGame.AchievementsManager.Notify(32);
        BrainGame.AchievementsManager.Notify(33);
        BrainGame.AchievementsManager.Notify(36);
      }
    }

    public int TotalBoosts
    {
      get => this._totalBoots;
      set
      {
        this._totalBoots = value;
        BrainGame.AchievementsManager.Notify(22);
      }
    }

    public PlayerStats() => this.StagesStat = new List<PlayerStageStats>();

    public void UnlockFirstStage(ThemeType theme)
    {
      LevelStage levelStageFromTheme = Levels.Load().GetFirstLevelStageFromTheme(theme);
      if (levelStageFromTheme == null)
        return;
      this.UnlockStage(levelStageFromTheme.StageId);
    }

    private void UnlockStage(string stageId)
    {
      if (this.IsStageUnlocked(stageId))
        return;
      this._stagesStat.Add(new PlayerStageStats(stageId));
    }

    public LevelStage UnlockNextStage(LevelStage currentLevelStage, bool unlockStage)
    {
      LevelStage nextStage = Levels.CurrentLevel.GetNextStage(currentLevelStage);
      if (nextStage == null)
        return (LevelStage) null;
      if (unlockStage)
        this.UnlockStage(nextStage.StageId);
      if (!Game1.GameSettings.AllStagesUnlocked)
      {
        switch (Game1.GameSettings.GameplayMode)
        {
          case BrainSettings.GameplayModeType.Demo:
          case BrainSettings.GameplayModeType.Beta:
            if (!nextStage.AvailableInDemo)
              return this.UnlockNextStage(Levels._instance.GetLevelStage(nextStage.ThemeId, 21), false);
            break;
        }
      }
      return nextStage;
    }

    public int StagesNeededToUnlockTheme(ThemeType theme, ThemeType themeToQuery)
    {
      int num = Levels.UnlockedStagesNeeded(theme, themeToQuery) - this.GetTotalUnlockedStagesForTheme(themeToQuery);
      return num >= 0 ? num : 0;
    }

    public bool IsThemeUnlocked(ThemeType theme)
    {
      return this.StagesNeededToUnlockTheme(theme, ThemeType.ThemeA) <= 0 && this.StagesNeededToUnlockTheme(theme, ThemeType.ThemeB) <= 0 && this.StagesNeededToUnlockTheme(theme, ThemeType.ThemeC) <= 0;
    }

    public bool IsStageUnlocked(string stageId)
    {
      Levels._instance.GetLevelStage(stageId);
      return this.GetStageStats(stageId) != null;
    }

    public bool IsTutorialToolSet(ToolObjectType toolType)
    {
      return this._tutorialToolsFlags >> (int) (toolType & (ToolObjectType) 63) == 1L && false;
    }

    public PlayerStageStats GetStageStats(string stageId)
    {
      return this._stagesStat.Find<PlayerStageStats>((Func<PlayerStageStats, bool>) (match => match.StageId == stageId));
    }

    public int GetTotalMedalsForTheme(MedalType medalType, ThemeType theme)
    {
      int totalMedalsForTheme = 0;
      foreach (PlayerStageStats playerStageStats in this._stagesStat)
      {
        LevelStage levelStage = Levels._instance.GetLevelStage(playerStageStats.StageId);
        if (levelStage != null && levelStage.ThemeId == theme && playerStageStats.Medal == medalType)
          ++totalMedalsForTheme;
      }
      return totalMedalsForTheme;
    }

    public int GetUnlockedStagesForTheme(ThemeType theme)
    {
      int unlockedStagesForTheme = 0;
      foreach (PlayerStageStats playerStageStats in this._stagesStat)
      {
        LevelStage levelStage = Levels._instance.GetLevelStage(playerStageStats.StageId);
        if (levelStage != null && levelStage.ThemeId == theme)
          ++unlockedStagesForTheme;
      }
      return unlockedStagesForTheme;
    }

    private int GetTotalUnlockedStagesForTheme(ThemeType theme)
    {
      int unlockedStagesForTheme = 0;
      foreach (PlayerStageStats playerStageStats in this._stagesStat)
      {
        LevelStage levelStage = Levels._instance.GetLevelStage(playerStageStats.StageId);
        if (levelStage != null && levelStage.ThemeId == theme)
          ++unlockedStagesForTheme;
      }
      return unlockedStagesForTheme;
    }

    public void InitFromDataFileRecord(DataFileRecord record)
    {
      this._totalPlayingTime = new TimeSpan(record.GetFieldValue<long>("totalPlayingTime", 0L));
      this._totalRunningTime = new TimeSpan(record.GetFieldValue<long>("totalRunningTime", 0L));
      this._totalSnailsSafe = record.GetFieldValue<int>("totalSnailsSafe", 0);
      this._totalSnailsKingSafe = record.GetFieldValue<int>("totalSnailsKingSafe", 0);
      this._totalSnailsDeadByFire = record.GetFieldValue<int>("totalSnailsDeadByFire", 0);
      this._totalSnailsDeadBySpikes = record.GetFieldValue<int>("totalSnailsDeadBySpikes", 0);
      this._totalSnailsDeadByDynamite = record.GetFieldValue<int>("totalSnailsDeadByDynamite", 0);
      this._totalSnailsDeadByCrate = record.GetFieldValue<int>("totalSnailsDeadByCrate", 0);
      this._totalSnailsDeadByWater = record.GetFieldValue<int>("totalSnailsDeadByWater", 0);
      this._totalSnailsDeadByLaser = record.GetFieldValue<int>("totalSnailsDeadByLaser", 0);
      this._totalSnailsDeadBySacrifice = record.GetFieldValue<int>("totalSnailsDeadBySacrifice", 0);
      this._totalSnailsDeadByCrateExplosion = record.GetFieldValue<int>("totalSnailsDeadByCrateExplosion", 0);
      this._totalSnailsDeadByAcid = record.GetFieldValue<int>("totalSnailsDeadByAcid", 0);
      this._totalSnailsDeadByOutOfStage = record.GetFieldValue<int>("totalSnailsDeadByOutOfStage", 0);
      this._totalSnailsDeadByEvilSnail = record.GetFieldValue<int>("totalSnailsDeadByEvilSnail", 0);
      this._totalGoldMedals = record.GetFieldValue<int>("totalGoldCoins", 0);
      this._totalSilverMedals = record.GetFieldValue<int>("totalSilverCoins", 0);
      this._totalBronzeMedals = record.GetFieldValue<int>("totalBronzeCoins", 0);
      this._totalBoots = record.GetFieldValue<int>("totalBoots", 0);
      DataFileRecord dataFileRecord = record.SelectRecord("Stages");
      if (dataFileRecord == null)
        return;
      DataFileRecordList dataFileRecordList = dataFileRecord.SelectRecords("Stage");
      this._stagesStat = new List<PlayerStageStats>(dataFileRecordList.Count);
      foreach (DataFileRecord record1 in dataFileRecordList)
        this._stagesStat.Add(PlayerStageStats.CreateFromDataFileRecord(record1));
    }

    public DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = new DataFileRecord("Statistics");
      dataFileRecord.AddField("totalPlayingTime", (object) this._totalPlayingTime.Ticks);
      dataFileRecord.AddField("totalRunningTime", (object) this._totalRunningTime.Ticks);
      dataFileRecord.AddField("totalSnailsSafe", (object) this._totalSnailsSafe);
      dataFileRecord.AddField("totalSnailsKingSafe", (object) this._totalSnailsKingSafe);
      dataFileRecord.AddField("totalSnailsDeadByFire", (object) this._totalSnailsDeadByFire);
      dataFileRecord.AddField("totalSnailsDeadBySpikes", (object) this._totalSnailsDeadBySpikes);
      dataFileRecord.AddField("totalSnailsDeadByDynamite", (object) this._totalSnailsDeadByDynamite);
      dataFileRecord.AddField("totalSnailsDeadByCrate", (object) this._totalSnailsDeadByCrate);
      dataFileRecord.AddField("totalSnailsDeadByWater", (object) this._totalSnailsDeadByWater);
      dataFileRecord.AddField("totalSnailsDeadByLaser", (object) this._totalSnailsDeadByLaser);
      dataFileRecord.AddField("totalSnailsDeadBySacrifice", (object) this._totalSnailsDeadBySacrifice);
      dataFileRecord.AddField("totalSnailsDeadByCrateExplosion", (object) this._totalSnailsDeadByCrateExplosion);
      dataFileRecord.AddField("totalSnailsDeadByAcid", (object) this._totalSnailsDeadByAcid);
      dataFileRecord.AddField("totalSnailsDeadByOutOfStage", (object) this._totalSnailsDeadByOutOfStage);
      dataFileRecord.AddField("totalSnailsDeadByEvilSnail", (object) this._totalSnailsDeadByEvilSnail);
      dataFileRecord.AddField("totalGoldCoins", (object) this._totalGoldMedals);
      dataFileRecord.AddField("totalSilverCoins", (object) this._totalSilverMedals);
      dataFileRecord.AddField("totalBronzeCoins", (object) this._totalBronzeMedals);
      dataFileRecord.AddField("totalBoots", (object) this._totalBoots);
      DataFileRecord record = new DataFileRecord("Stages");
      if (this._stagesStat != null && this._stagesStat.Count > 0)
      {
        foreach (PlayerStageStats playerStageStats in this._stagesStat)
          record.AddRecord(playerStageStats.ToDataFileRecord());
      }
      dataFileRecord.AddRecord(record);
      return dataFileRecord;
    }

    public int Level { get; set; }

    internal int GetClearStagesForTheme(ThemeType themeType)
    {
      int clearStagesForTheme = 0;
      foreach (PlayerStageStats playerStageStats in this._stagesStat)
      {
        if (playerStageStats.WasPlayed)
        {
          LevelStage levelStage = Levels._instance.GetLevelStage(playerStageStats.StageId);
          if (levelStage != null && levelStage.ThemeId == themeType)
            ++clearStagesForTheme;
        }
      }
      return clearStagesForTheme;
    }

    public bool IsAllMedalsGet(MedalType medal)
    {
      int num = 0;
      foreach (PlayerStageStats playerStageStats in this._stagesStat)
      {
        ++num;
        if (playerStageStats.Medal < medal)
          return false;
      }
      return num == 84;
    }
  }
}
