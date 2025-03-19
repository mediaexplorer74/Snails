
// Type: TwoBrainsGames.Snails.StageObjects.TileObject
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class TileObject : StageObject, ISnailsDataFileSerializable, IDataFileSerializable
  {
    protected string _tileId;
    protected Tile _tile;

    public TileObject(StageObjectType type)
      : base(type)
    {
    }

    public TileObject(TileObject other)
      : base((StageObject) other)
    {
    }

    public override void Copy(StageObject other)
    {
      base.Copy(other);
      this._tileId = (other as TileObject)._tileId;
    }

    public override void Initialize()
    {
      base.Initialize();
      if (string.IsNullOrEmpty(this._tileId))
        return;
      this._tile = Levels._instance.StageData.GetTile(this._tileId);
      this.BlendColor = this._tile.BlendColor;
    }

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord(context);
      if (context == ToDataFileRecordContext.StageDataSave)
        dataFileRecord.AddField("tileId", (object) this._tileId);
      return dataFileRecord;
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this._tileId = record.GetFieldValue<string>("tileId", this._tileId);
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }
  }
}
