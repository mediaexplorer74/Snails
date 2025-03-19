
// Type: TwoBrainsGames.Snails.Stages.MedalScoreCriteria
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using System;
using TwoBrainsGames.BrainEngine.Data.DataFiles;


namespace TwoBrainsGames.Snails.Stages
{
  public class MedalScoreCriteria : IDataFileSerializable
  {
    public int SnailsNeeded { get; set; }

    public int GoldCoinsNeeded { get; set; }

    public int SilverCoinsNeeded { get; set; }

    public int BronzeCoinsNeeded { get; set; }

    public TimeSpan TimeNeeded { get; set; }

    public Stage StageOwner { get; private set; }

    public int Score
    {
      get
      {
        return this.SnailsNeeded * 5 + (this.GoldCoinsNeeded * 100 + this.SilverCoinsNeeded * 50 + this.BronzeCoinsNeeded * 20) + (int) ((this.StageOwner.LevelStage._targetTime - this.TimeNeeded).TotalSeconds * 1.0);
      }
    }

    public MedalScoreCriteria(Stage stageOwner) => this.StageOwner = stageOwner;

    public static MedalScoreCriteria CreateFromDataFileRecord(
      DataFileRecord record,
      Stage stageOwner)
    {
      MedalScoreCriteria fromDataFileRecord = new MedalScoreCriteria(stageOwner);
      fromDataFileRecord.InitFromDataFileRecord(record);
      return fromDataFileRecord;
    }

    public DataFileRecord ToDataFileRecord(string recordName)
    {
      DataFileRecord dataFileRecord = this.ToDataFileRecord();
      dataFileRecord.Name = recordName;
      return dataFileRecord;
    }

    public override string ToString() => this.Score.ToString();

    public void InitFromDataFileRecord(DataFileRecord record)
    {
      if (record == null)
        return;
      this.SnailsNeeded = record.GetFieldValue<int>("snailsNeeded", 0);
      this.GoldCoinsNeeded = record.GetFieldValue<int>("goldCoinsNeeded", 0);
      this.SilverCoinsNeeded = record.GetFieldValue<int>("silverCoinsNeeded", 0);
      this.BronzeCoinsNeeded = record.GetFieldValue<int>("bronzeCoinsNeeded", 0);
      this.TimeNeeded = record.GetFieldValue<TimeSpan>("timeNeeded", new TimeSpan());
    }

    public DataFileRecord ToDataFileRecord()
    {
      DataFileRecord dataFileRecord = new DataFileRecord("");
      dataFileRecord.AddField("snailsNeeded", (object) this.SnailsNeeded);
      dataFileRecord.AddField("goldCoinsNeeded", (object) this.GoldCoinsNeeded);
      dataFileRecord.AddField("silverCoinsNeeded", (object) this.SilverCoinsNeeded);
      dataFileRecord.AddField("bronzeCoinsNeeded", (object) this.BronzeCoinsNeeded);
      dataFileRecord.AddField("timeNeeded", (object) this.TimeNeeded);
      return dataFileRecord;
    }
  }
}
