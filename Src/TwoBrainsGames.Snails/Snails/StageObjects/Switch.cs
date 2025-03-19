
// Type: TwoBrainsGames.Snails.StageObjects.Switch
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using System;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.SpacePartitioning;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class Switch : StageObject
  {
    public Switch.SwitchState State { get; set; }

    public Switch.SwitchOnActionType SwitchOnAction { get; set; }

    public bool IsOn => this.State == Switch.SwitchState.On;

    public bool IsOff => this.State == Switch.SwitchState.Off;

    protected BoundingSquare SnailCollisionBB { get; set; }

    public override BoundingSquare QuadtreeCollisionBB => this.SnailCollisionBB;

    public Switch()
    {
    }

    public Switch(StageObjectType type)
      : base(type)
    {
    }

    public override void Copy(StageObject other)
    {
      this.SwitchOnAction = ((Switch) other).SwitchOnAction;
      base.Copy(other);
    }

    public void SwitchOn()
    {
      foreach (StageObject linkedObject in this._linkedObjects)
      {
        if (linkedObject is ISwitchable)
        {
          ISwitchable switchable = linkedObject as ISwitchable;
          if (linkedObject.IsVisible && !linkedObject.IsDead && !linkedObject.IsDisposed)
          {
            switch (this.SwitchOnAction)
            {
              case Switch.SwitchOnActionType.SwitchOn:
                switchable.SwitchOn();
                continue;
              case Switch.SwitchOnActionType.SwitchOff:
                switchable.SwitchOff();
                continue;
              case Switch.SwitchOnActionType.Invert:
                if (switchable.IsOn)
                {
                  switchable.SwitchOff();
                  continue;
                }
                switchable.SwitchOn();
                continue;
              default:
                continue;
            }
          }
        }
      }
      this.State = Switch.SwitchState.On;
    }

    public void SwitchOff()
    {
      foreach (StageObject linkedObject in this._linkedObjects)
      {
        if (linkedObject is ISwitchable && linkedObject.IsVisible && !linkedObject.IsDead && !linkedObject.IsDisposed)
          ((ISwitchable) linkedObject).SwitchOff();
      }
      this.State = Switch.SwitchState.Off;
    }

    public override void UpdateBoundingBox()
    {
      base.UpdateBoundingBox();
      this.SnailCollisionBB = this.AABoundingBox;
    }

    public override void OnCollide(IQuadtreeContainable obj, int listIdx)
    {
      this.OnSnailCollided((Snail) obj);
    }

    protected virtual void OnSnailCollided(Snail snail)
    {
    }

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord(context);
      if (context == ToDataFileRecordContext.StageSave)
        dataFileRecord.AddField("switchOnAction", (object) this.SwitchOnAction.ToString());
      return dataFileRecord;
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this.SwitchOnAction = (Switch.SwitchOnActionType) Enum.Parse(typeof (Switch.SwitchOnActionType), record.GetFieldValue<string>("switchOnAction", this.SwitchOnAction.ToString()), true);
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public enum SwitchState
    {
      Off,
      On,
    }

    public enum SwitchOnActionType
    {
      SwitchOn,
      SwitchOff,
      Invert,
    }
  }
}
