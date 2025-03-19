
// Type: TwoBrainsGames.Snails.Stages.Levels
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Resources;


namespace TwoBrainsGames.Snails.Stages
{
  public class Levels : 
    IBrainComponent,
    ISnailsDataFileSerializable,
    IDataFileSerializable,
    IAsyncOperation
  {
    public const int CUSTOM_STAGE_NR = 9999;
    private const int NUM_THEMES = 4;
    public const int MAX_NUMBER_STAGES_PER_THEME = 21;
    public const int TOTAL_NUM_STAGES = 84;
    public static Levels _instance;
    public static ThemeType CurrentTheme = ThemeType.ThemeA;
    public static LevelStage CurrentLevelStage;
    public static int CurrentStageNr = 1;
    public static string CurrentCustomStageFilename;
    private List<LevelStage> _stages = new List<LevelStage>();
    private List<LevelStage>[] _stagesByTheme;
    private StageData _stageData;
    private Stage _stage;
    private string _previousTheme = string.Empty;
    private string _currentStageResourceId;
    private ResourceManager _stageResourceManager;
    private SpriteBatch _spriteBatch;
    private StageSound _currentStageSound;
    private ThemeSettings[] _themeSettings;
    public static ThemeSettings CurrentThemeSettings;

    public Stage Stage => this._stage;

    public StageData StageData => this._stageData;

    public List<LevelStage> Stages
    {
      get => this._stages;
      set => this._stages = value;
    }

    public List<LevelStage>[] StagesByTheme
    {
      get => this._stagesByTheme;
      set => this._stagesByTheme = value;
    }

    public int StageIdx { get; private set; }

    public LevelStage FirstLevelStage => this._stages[0];

    internal static Levels CurrentLevel => Levels._instance;

    public StageSound StageSound => this._currentStageSound;

    public Levels()
    {
      Levels._instance = Levels._instance == null ? this : throw new SnailsException("Ups! Levels is a singleton, why create two instances!? I had problems with this, so I've added this exception.");
      this._stageResourceManager = BrainGame.CreateResourceManager();
      this._spriteBatch = new SpriteBatch(BrainGame.Graphics);
    }

    public static Levels Load()
    {
      if (Levels._instance != null)
        return Levels._instance;
      Levels levels = new Levels();
      DataFileRecord record = BrainGame.ResourceManager.Load<DataFileRecord>("stages\\levels", ResourceManager.ResourceManagerCacheType.Static);
      levels.InitFromDataFileRecord(record);
      levels.Initialize();
      return levels;
    }

    public SpriteBatch SpriteBatch => this._spriteBatch;

    public void Initialize()
    {
    }

    public void HandleEvents(BrainGameTime gameTime) => this._stage.HandleEvents(gameTime);

    public void Update(BrainGameTime gameTime) => this._stage.Update(gameTime);

    public void Draw()
    {
      this._stage.DrawWaterTexture();
      this._stage.Draw();
    }

    public void UnloadContent()
    {
      BrainGame.ResourceManager.Unload("STAGE_THEME_RESOURCES");
      BrainGame.ResourceManager.Unload("TUTORIAL");
      this.UnloadCurrentStage();
    }

    private void UnloadCurrentStage()
    {
      this._currentStageResourceId = (string) null;
      if (Stage.CurrentStage == null)
        return;
      Stage.CurrentStage.UnloadContent();
    }

    public void LoadContent()
    {
      this.ReloadStageData();
      this._currentStageSound = this._themeSettings[(int) Levels.CurrentTheme]._sounds;
      this._currentStageSound.Initialize();
      this._currentStageSound.LoadContent();
    }

    public void ReloadStageData()
    {
      this._stageData = BrainGame.ResourceManager.Load<StageData>("stages/stagedata", "STAGE_THEME_RESOURCES");
      this._stageData.LoadContent(Levels.CurrentTheme);
    }

    public LevelStage GetCurrentStageInfo()
    {
      return Levels.CurrentLevelStage != null && Levels.CurrentLevelStage.IsCustomStage ? Levels.CurrentLevelStage : this.FindStageInfo(Levels.CurrentTheme, Levels.CurrentStageNr);
    }

    public LevelStage GetNextStageInfo()
    {
      if (this._stage.LevelStage.IsCustomStage)
        return (LevelStage) null;
      ThemeType theme = Levels.CurrentTheme;
      int stageNr = Levels.CurrentStageNr + 1;
      if (stageNr > 21)
      {
        stageNr = 1;
        theme = Levels.CurrentTheme + 1;
        if (theme > ThemeType.ThemeD)
          theme = ThemeType.ThemeA;
      }
      return this.FindStageInfo(theme, stageNr);
    }

    public int GetStagesCountForTheme(ThemeType theme)
    {
      return this._stagesByTheme[(int) theme] == null ? 0 : this._stagesByTheme[(int) theme].Count;
    }

    public void IncrementStage()
    {
      ++Levels.CurrentStageNr;
      if (Levels.CurrentStageNr <= 21)
        return;
      Levels.CurrentStageNr = 1;
      ++Levels.CurrentTheme;
      if (Levels.CurrentTheme <= ThemeType.ThemeD)
        return;
      Levels.CurrentTheme = ThemeType.ThemeA;
    }

    public void StartNextStage()
    {
      ++Levels.CurrentStageNr;
      if (Levels.CurrentStageNr > 21)
      {
        Levels.CurrentStageNr = 1;
        ++Levels.CurrentTheme;
        if (Levels.CurrentTheme > ThemeType.ThemeD)
          Levels.CurrentTheme = ThemeType.ThemeA;
      }
      this.LoadStage(Levels.CurrentTheme, Levels.CurrentStageNr);
    }

    public void StartPrevStage()
    {
      --Levels.CurrentStageNr;
      if (Levels.CurrentStageNr <= 0)
      {
        Levels.CurrentStageNr = 1;
        --Levels.CurrentTheme;
        if (Levels.CurrentTheme < ThemeType.ThemeA)
          Levels.CurrentTheme = ThemeType.ThemeA;
      }
      this.LoadStage(Levels.CurrentTheme, Levels.CurrentStageNr);
    }

    public void LoadStage(ThemeType theme, int stageNr)
    {
      this.LoadStage(this.FindStageInfo(theme, stageNr).StageId, Stage.StageLoadingContext.Gameplay);
    }

    public Stage LoadStage(string stageId, Stage.StageLoadingContext loadingContext)
    {
      Stage.LoadingContext = loadingContext;
      this.StageIdx = this.FindStageIndexById(stageId);
      if (this.StageIdx == -1)
        this.StageIdx = 0;
      LevelStage stage = this._stages[this.StageIdx];
      Levels.CurrentTheme = stage.ThemeId;
      Levels.CurrentStageNr = stage.StageNr;
      Levels.CurrentThemeSettings = this._themeSettings[(int) Levels.CurrentTheme];
      if (this._previousTheme != stage.Theme)
      {
        if (!string.IsNullOrEmpty(this._previousTheme))
          this.UnloadContent();
        this.LoadContent();
        this._previousTheme = stage.Theme;
      }
      this.UnloadCurrentStage();
      this._currentStageResourceId = string.Format("stages/{0}/{1}", (object) stage.Theme, (object) stage.StageId);
      DataFileRecord stageRecord = BrainGame.ResourceManager.Load<DataFileRecord>(this._currentStageResourceId, ResourceManager.ResourceManagerCacheType.Temporary);
      this._stage = new Stage(stage);
      this._stage.InitFromDataFileRecord(stageRecord);
      this._stage.Initialize();
      this._stage.LoadContent();
      this._stage.LevelStage = stage;
      Levels.CurrentLevelStage = stage;
      if (loadingContext == Stage.StageLoadingContext.Gameplay && this._stage.Key != stage.StageKey)
        throw new SnailsException("Unable to load stage.");
      return this._stage;
    }

    public int FindStageIndexById(string id)
    {
      for (int index = 0; index < this._stages.Count; ++index)
      {
        if (this._stages[index].StageId == id)
          return index;
      }
      return -1;
    }

    public LevelStage FindStageInfo(ThemeType theme, int stageNr)
    {
      for (int index = 0; index < this._stages.Count; ++index)
      {
        if (this._stages[index].StageNr == stageNr && this._stages[index].ThemeId == theme)
          return this._stages[index];
      }
      throw new SnailsException("Stage with theme [" + theme.ToString() + "] and stageNr[" + stageNr.ToString() + "] not found.");
    }

    public LevelStage GetLevelStage(ThemeType theme, int nr)
    {
      for (int index = 0; index < this._stages.Count; ++index)
      {
        if (this._stages[index].ThemeId == theme && this._stages[index].StageNr == nr)
          return this._stages[index];
      }
      throw new SnailsException("LevelStage not found [theme=" + theme.ToString() + "] [stageNr=" + nr.ToString() + "]");
    }

    public LevelStage GetFirstLevelStageFromTheme(ThemeType theme)
    {
      foreach (LevelStage stage in this._stages)
      {
        if (stage.ThemeId == theme)
          return stage;
      }
      return (LevelStage) null;
    }

    public LevelStage GetLevelStage(int stageIdx) => this._stages[stageIdx];

    public LevelStage GetLevelStage(string stageId)
    {
      foreach (LevelStage stage in this._stages)
      {
        if (stage.StageId == stageId)
          return stage;
      }
      return (LevelStage) null;
    }

    public int GetStageIndex(LevelStage levelStage)
    {
      return (int) levelStage.ThemeId * 21 + levelStage.StageNr - 1;
    }

    public LevelStage GetNextStage(LevelStage levelStage)
    {
      if (levelStage == null)
        return this._stages[0];
      if (levelStage.IsCustomStage)
        return (LevelStage) null;
      int index = this.GetStageIndex(levelStage) + 1;
      return index >= 84 ? (LevelStage) null : this._stages[index];
    }

    public static Levels FromDataFileRecord(DataFileRecord record)
    {
      Levels levels = new Levels();
      levels.InitFromDataFileRecord(record);
      return levels;
    }

    public static int UnlockedStagesNeeded(ThemeType themeToUnlock, ThemeType theme)
    {
      switch (themeToUnlock)
      {
        case ThemeType.ThemeA:
          return 0;
        case ThemeType.ThemeB:
          return theme == ThemeType.ThemeA ? 6 : 0;
        case ThemeType.ThemeC:
          switch (theme)
          {
            case ThemeType.ThemeA:
              return 12;
            case ThemeType.ThemeB:
              return 10;
            default:
              return 0;
          }
        case ThemeType.ThemeD:
          switch (theme)
          {
            case ThemeType.ThemeA:
              return 17;
            case ThemeType.ThemeB:
              return 13;
            case ThemeType.ThemeC:
              return 9;
            default:
              return 0;
          }
        default:
          return 0;
      }
    }

    public bool IsLockedInDemo(ThemeType theme)
    {
      foreach (LevelStage levelStage in this._stagesByTheme[(int) theme])
      {
        if (levelStage.AvailableInDemo)
          return false;
      }
      return true;
    }

    public void InitFromDataFileRecord(DataFileRecord record)
    {
      this._stagesByTheme = new List<LevelStage>[4];
      this._themeSettings = new ThemeSettings[4];
      foreach (DataFileRecord selectRecord1 in record.SelectRecords("Level"))
      {
        int num = 1;
        ThemeType index = (ThemeType) Enum.Parse(typeof (ThemeType), selectRecord1.GetFieldValue<string>("theme"), true);
        this._stagesByTheme[(int) index] = new List<LevelStage>();
        this._themeSettings[(int) index] = new ThemeSettings();
        this._themeSettings[(int) index]._sounds = new StageSound();
        DataFileRecord record1 = selectRecord1.SelectRecord("Sound");
        if (record1 != null)
          this._themeSettings[(int) index]._sounds.InitFromDataFileRecord(record1);
        this._themeSettings[(int) index]._shadowColor = selectRecord1.GetFieldValue<Color>("shadowColor");
        this._themeSettings[(int) index]._themeType = index;
        foreach (DataFileRecord selectRecord2 in selectRecord1.SelectRecords("Stages\\Stage"))
        {
          LevelStage levelStage = new LevelStage();
          levelStage.ThemeId = index;
          levelStage.StageId = selectRecord2.GetFieldValue<string>("id");
          levelStage.StageKey = selectRecord2.GetFieldValue<string>("key");
          levelStage._snailsToSave = selectRecord2.GetFieldValue<int>("snailsToSave");
          levelStage._snailsToRelease = selectRecord2.GetFieldValue<int>("snailsToRelease");
          levelStage._targetTime = selectRecord2.GetFieldValue<TimeSpan>("targetTime");
          levelStage._goldMedalTime = selectRecord2.GetFieldValue<TimeSpan>("goldMedalTime");
          levelStage._goldMedalScore = selectRecord2.GetFieldValue<int>("goldMedalScore");
          levelStage._goal = (GoalType) selectRecord2.GetFieldValue<int>("goal");
          levelStage.AvailableInDemo = selectRecord2.GetFieldValue<bool>("availableInDemo", false);
          levelStage.StageNr = num++;
          this._stages.Add(levelStage);
          this._stagesByTheme[(int) index].Add(levelStage);
        }
      }
    }

    public DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      List<string> stringList = new List<string>();
      string str = (string) null;
      foreach (LevelStage stage in this._stages)
      {
        if (str != stage.Theme)
        {
          stringList.Add(stage.Theme);
          str = stage.Theme;
        }
      }
      DataFileRecord dataFileRecord = new DataFileRecord(nameof (Levels));
      foreach (ThemeSettings themeSetting in this._themeSettings)
      {
        DataFileRecord record1 = new DataFileRecord("Level");
        record1.AddField("theme", (object) themeSetting._themeType.ToString());
        record1.AddField("shadowColor", (object) themeSetting._shadowColor);
        record1.AddRecord(themeSetting._sounds.ToDataFileRecord());
        DataFileRecord record2 = new DataFileRecord("Stages");
        foreach (LevelStage stage in this._stages)
        {
          if (stage.ThemeId == themeSetting._themeType)
          {
            DataFileRecord record3 = new DataFileRecord("Stage");
            record3.AddField("id", (object) stage.StageId);
            if (stage.StageKey != null)
              record3.AddField("key", (object) stage.StageKey);
            record3.AddField("snailsToSave", (object) stage._snailsToSave);
            record3.AddField("snailsToRelease", (object) stage._snailsToRelease);
            record3.AddField("targetTime", (object) stage._targetTime);
            record3.AddField("goal", (object) (int) stage._goal);
            record3.AddField("availableInDemo", (object) stage.AvailableInDemo);
            record3.AddField("goldMedalTime", (object) stage._goldMedalTime);
            record3.AddField("goldMedalScore", (object) stage._goldMedalScore);
            record2.AddRecord(record3);
          }
        }
        record1.AddRecord(record2);
        dataFileRecord.AddRecord(record1);
      }
      return dataFileRecord;
    }

    public void BeginLoad() => this.LoadStage(Levels.CurrentTheme, Levels.CurrentStageNr);

    public object AsyncLoadingParams
    {
      set
      {
      }
    }
  }
}
