
// Type: TwoBrainsGames.Snails.Player.PlayerStageStats
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using System;
using TwoBrainsGames.BrainEngine.Data.DataFiles;


namespace TwoBrainsGames.Snails.Player
{
  public class PlayerStageStats : IDataFileSerializable
  {
    protected string _stageId;
    protected TimeSpan _completionTime;
    protected int _numSnailsSafe;
    protected int _numGoldCoins;
    protected int _numSilverCoins;
    protected int _numBronzeCoins;
    protected MedalType _medal;
    protected int _highscore;

    public string StageId
    {
      get => this._stageId;
      set => this._stageId = value;
    }

    public TimeSpan CompletionTime
    {
      get => this._completionTime;
      set => this._completionTime = value;
    }

    public int NumSnailsSafe
    {
      get => this._numSnailsSafe;
      set => this._numSnailsSafe = value;
    }

    public int NumGoldCoins
    {
      get => this._numGoldCoins;
      set => this._numGoldCoins = value;
    }

    public int NumSilverCoins
    {
      get => this._numSilverCoins;
      set => this._numSilverCoins = value;
    }

    public int NumBronzeCoins
    {
      get => this._numBronzeCoins;
      set => this._numBronzeCoins = value;
    }

    public MedalType Medal
    {
      get => this._medal;
      set => this._medal = value;
    }

    public int Highscore
    {
      get => this._highscore;
      set => this._highscore = value;
    }

    public int TimesPlayed { get; set; }

    public bool WasPlayed => this.TimesPlayed > 0;

    private PlayerStageStats()
    {
    }

    public PlayerStageStats(string stageId) => this.StageId = stageId;

    public static PlayerStageStats CreateFromDataFileRecord(DataFileRecord record)
    {
      PlayerStageStats fromDataFileRecord = new PlayerStageStats();
      fromDataFileRecord.InitFromDataFileRecord(record);
      return fromDataFileRecord;
    }

    public void InitFromDataFileRecord(DataFileRecord record)
    {
      this._stageId = record.GetFieldValue<string>("stageId");
      this._completionTime = new TimeSpan(record.GetFieldValue<long>("completionTime", 0L));
      this._numSnailsSafe = record.GetFieldValue<int>("numSnailsSafe", 0);
      this._numGoldCoins = record.GetFieldValue<int>("numGoldCoins", 0);
      this._numSilverCoins = record.GetFieldValue<int>("numSilverCoins", 0);
      this._numBronzeCoins = record.GetFieldValue<int>("numBronzeCoins", 0);
      this._medal = (MedalType) record.GetFieldValue<int>("medal", 0);
      this._highscore = record.GetFieldValue<int>("highscore", 0);
      this.TimesPlayed = record.GetFieldValue<int>("timesPlayed", 0);
    }

    public DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = new DataFileRecord("Stage");
      dataFileRecord.AddField("stageId", (object) this._stageId);
      dataFileRecord.AddField("completionTime", (object) this._completionTime.Ticks);
      dataFileRecord.AddField("numSnailsSafe", (object) this._numSnailsSafe);
      dataFileRecord.AddField("numGoldCoins", (object) this._numGoldCoins);
      dataFileRecord.AddField("numSilverCoins", (object) this._numSilverCoins);
      dataFileRecord.AddField("numBronzeCoins", (object) this._numBronzeCoins);
      dataFileRecord.AddField("medal", (object) (int) this._medal);
      dataFileRecord.AddField("highscore", (object) this._highscore);
      dataFileRecord.AddField("timesPlayed", (object) this.TimesPlayed);
      return dataFileRecord;
    }
  }
}
