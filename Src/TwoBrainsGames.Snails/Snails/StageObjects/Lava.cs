
// Type: TwoBrainsGames.Snails.StageObjects.Lava
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine.Data.DataFiles;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class Lava : Liquid
  {
    public Lava()
      : base(StageObjectType.Lava)
    {
    }

    protected override void Resize()
    {
      base.Resize();
      this._crateCollisionBB = this._liquidAABB;
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      return base.ToDataFileRecord(context);
    }
  }
}
