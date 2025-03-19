
// Type: TwoBrainsGames.Snails.Stages.StageSoundSource
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine.Data.DataFiles;


namespace TwoBrainsGames.Snails.Stages
{
  public class StageSoundSource : ISnailsDataFileSerializable, IDataFileSerializable
  {
    private const float DEFAULT_VOLUME = 0.2f;
    public string Res;
    public float Volume;
    public bool Loop;

    public void InitFromDataFileRecord(DataFileRecord record)
    {
      if (record == null)
        return;
      this.Res = record.GetFieldValue<string>("res");
      this.Volume = record.GetFieldValue<float>("volume", 0.2f);
      this.Loop = record.GetFieldValue<bool>("loop", false);
    }

    public DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      return this.ToDataFileRecord(nameof (StageSoundSource));
    }

    public DataFileRecord ToDataFileRecord(string recordName)
    {
      DataFileRecord dataFileRecord = new DataFileRecord(recordName);
      dataFileRecord.AddField("res", (object) this.Res);
      dataFileRecord.AddField("volume", (object) this.Volume);
      dataFileRecord.AddField("loop", (object) this.Loop);
      return dataFileRecord;
    }
  }
}
