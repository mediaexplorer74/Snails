
// Type: TwoBrainsGames.Snails.StageObjects.SingleLightEmitter
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  internal class SingleLightEmitter : StageObject
  {
    protected string LightSourceId { get; set; }

    protected LightSource LightSource { get; set; }

    public LightSource.LightState State { get; set; }

    protected Vector2 LightMaskScale { get; set; }

    public bool IsOn => this.LightSource.IsOn;

    public bool IsOff => this.LightSource.IsOff;

    public SingleLightEmitter(StageObjectType type)
      : base(type)
    {
      this.LightSource = new LightSource();
      this.LightMaskScale = new Vector2(1f, 1f);
    }

    public override void Copy(StageObject other)
    {
      base.Copy(other);
      SingleLightEmitter singleLightEmitter = (SingleLightEmitter) other;
      this.LightSourceId = singleLightEmitter.LightSourceId;
      this.State = singleLightEmitter.State;
      this.LightMaskScale = singleLightEmitter.LightMaskScale;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LightSource = Stage.CurrentStage.StageData.GetLightSource(this.LightSourceId);
      this.LightSource.Scale = this.LightMaskScale;
      this.LightSource.Position = this.BoundingBox.GetCenter();
    }

    public override void StageInitialize()
    {
      base.StageInitialize();
      Stage.CurrentStage.LightManager.AddLightSource(this.LightSource);
    }

    public override void DisposeFromStage()
    {
      base.DisposeFromStage();
      Stage.CurrentStage.LightManager.RemoveLightSource(this.LightSource);
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this.LightSource = new LightSource();
      this.LightSourceId = record.GetFieldValue<string>("lightSourceId", this.LightSourceId);
      if (string.IsNullOrEmpty(this.LightSourceId))
        throw new SnailsException("SingleLightEmitter objects must have a lightSourceId!!");
      this.State = (LightSource.LightState) Enum.Parse(typeof (LightSource.LightState), record.GetFieldValue<string>("state", LightSource.LightState.On.ToString()), true);
      this.LightMaskScale = new Vector2(record.GetFieldValue<float>("lightScaleX", this.LightMaskScale.X), record.GetFieldValue<float>("lightScaleY", this.LightMaskScale.Y));
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord(context);
      if (context == ToDataFileRecordContext.StageDataSave)
      {
        dataFileRecord.AddField("lightSourceId", (object) this.LightSourceId);
        dataFileRecord.AddField("lightScaleX", (object) this.LightMaskScale.X);
        dataFileRecord.AddField("lightScaleY", (object) this.LightMaskScale.Y);
      }
      dataFileRecord.AddField("state", (object) this.State.ToString());
      return dataFileRecord;
    }
  }
}
