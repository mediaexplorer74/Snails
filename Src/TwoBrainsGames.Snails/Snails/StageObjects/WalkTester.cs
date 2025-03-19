
// Type: TwoBrainsGames.Snails.StageObjects.WalkTester
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;


namespace TwoBrainsGames.Snails.StageObjects
{
  internal class WalkTester : MovingObject, ISnailsDataFileSerializable, IDataFileSerializable
  {
    public const string ID = "WALK_TESTER";

    public WalkTester()
      : base(StageObjectType.WalkTester)
    {
    }

    public WalkTester(WalkTester other)
      : base((StageObject) other)
    {
      this.Copy((StageObject) other);
    }

    public override void Copy(StageObject other) => base.Copy(other);

    public override void Initialize()
    {
      base.Initialize();
      this.Speed = 0.05f;
      this.SetDirection(MovingObject.WalkDirection.CounterClockwise);
      this.Position = new Vector2(224f, 100f);
      this.UpdateBoundingBox();
    }

    public override void Update(BrainGameTime gameTime) => base.Update(gameTime);

    void IDataFileSerializable.InitFromDataFileRecord(DataFileRecord record)
    {
      this.InitFromDataFileRecord(record);
    }

    DataFileRecord IDataFileSerializable.ToDataFileRecord() => this.ToDataFileRecord();
  }
}
