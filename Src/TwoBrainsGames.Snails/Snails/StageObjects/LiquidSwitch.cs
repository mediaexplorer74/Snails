
// Type: TwoBrainsGames.Snails.StageObjects.LiquidSwitch
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine.Data.DataFiles;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class LiquidSwitch : Switch
  {
    public const float MAX_PUMP_SPEED = 10f;
    public const float MIN_PUMP_SPEED = 1f;
    private const float DEFAULT_SPEED = 5f;

    public float PumpSpeed { get; set; }

    public LiquidSwitch(StageObjectType objType)
      : base(objType)
    {
      this.PumpSpeed = 5f;
    }

    public override void Initialize() => base.Initialize();

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord(context);
      dataFileRecord.AddField("pumpSpeed", (object) this.PumpSpeed);
      return dataFileRecord;
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this.PumpSpeed = record.GetFieldValue<float>("pumpSpeed");
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }
  }
}
