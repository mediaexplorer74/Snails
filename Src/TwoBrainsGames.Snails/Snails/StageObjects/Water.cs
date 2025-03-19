
// Type: TwoBrainsGames.Snails.StageObjects.Water
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class Water : Liquid
  {
    public const float WATER_GRAVITY = 5f;
    private Sample _waterSample;

    public Water()
      : base(StageObjectType.Water)
    {
    }

    public override void Copy(StageObject other) => base.Copy(other);

    public override void Initialize() => base.Initialize();

    public override void LoadContent()
    {
      base.LoadContent();
      this._waterSample = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/water", (Object2D) this);
    }

    public override void StageStartupPhaseEnded()
    {
      base.StageStartupPhaseEnded();
      this._waterSample.Play(true);
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
