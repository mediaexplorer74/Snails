
// Type: TwoBrainsGames.Snails.StageObjects.DirectionalBox
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using System;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class DirectionalBox : Box
  {
    public const string ID_CW = "DIRECTIONAL_BOX_CW";
    public const string ID_CCW = "DIRECTIONAL_BOX_CCW";
    private const float ARROW_ROTATION_SPEED = 20f;
    private DirectionalBox.WalkDirection _walkDirection;

    public DirectionalBox()
      : base(StageObjectType.DirectionalBox)
    {
    }

    public DirectionalBox(Copper other)
      : base((Box) other)
    {
      this.Copy((StageObject) other);
    }

    public override void Copy(StageObject other)
    {
      base.Copy(other);
      this._walkDirection = (other as DirectionalBox)._walkDirection;
    }

    public override void LoadContent() => base.LoadContent();

    public override void OnLastFrame()
    {
      this.BoxDeployed(true, true, true);
      Stage.CurrentStage.RemoveObject((StageObject) this);
    }

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord(context);
      if (context == ToDataFileRecordContext.StageDataSave)
        dataFileRecord.AddField("walkDirection", (object) this._walkDirection.ToString());
      return dataFileRecord;
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this._walkDirection = (DirectionalBox.WalkDirection) Enum.Parse(typeof (DirectionalBox.WalkDirection), record.GetFieldValue<string>("walkDirection", this._walkDirection.ToString()), true);
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    private enum WalkDirection
    {
      Clockwise,
      CounterClockwise,
    }
  }
}
