
// Type: TwoBrainsGames.Snails.StageObjects.CollisionTester
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  internal class CollisionTester : StageObject, ISnailsDataFileSerializable, IDataFileSerializable
  {
    public const string ID = "COLLISION_TESTER";

    private float Angle { get; set; }

    private float Speed { get; set; }

    private CollisionTester.ObjStatus Status { get; set; }

    private int TimeToRelease { get; set; }

    private int ObjectToRelease { get; set; }

    public CollisionTester()
      : base(StageObjectType.CollisionTester)
    {
    }

    public CollisionTester(CollisionTester other)
      : base((StageObject) other)
    {
      this.Copy((StageObject) other);
    }

    public override void Copy(StageObject other) => base.Copy(other);

    public override void Initialize()
    {
      base.Initialize();
      this.Speed = 600f;
      this.Angle = -45f;
      this.TimeToRelease = 200;
      this.ObjectToRelease = 100;
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      if (this.ObjectToRelease <= 0 || this.Status != CollisionTester.ObjStatus.Idle)
        return;
      this.TimeToRelease -= gameTime.ElapsedGameTime.Milliseconds;
      if (this.TimeToRelease > 0)
        return;
      this.LaunchObject((float) BrainGame.Rand.Next(360), 0.01f, BrainGame.Rand.Next(2) == 1 ? MovingObject.WalkDirection.CounterClockwise : MovingObject.WalkDirection.Clockwise);
      this.TimeToRelease = 200;
      --this.ObjectToRelease;
    }

    private void LaunchObject(float angle, float moveSpeed, MovingObject.WalkDirection direction)
    {
      StageObject stageObject = Stage.CurrentStage.StageData.GetObject("WALK_TESTER");
      stageObject.BoardX = this.BoardX;
      stageObject.BoardY = this.BoardY;
      stageObject.UpdateBoundingBox();
      stageObject.EffectsBlender.Add((ITransformEffect) new LinearMoveEffect(this.Speed, angle), 99);
      MovingObject movingObject = (MovingObject) stageObject;
      movingObject.SetDirection(direction);
      movingObject.Speed = (float) (0.0099999997764825821 + (double) BrainGame.Rand.Next(10) / 100.0);
      movingObject.Speed = moveSpeed;
      Stage.CurrentStage.AddObjectInRuntime(stageObject);
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
    }

    public override DataFileRecord ToDataFileRecord() => base.ToDataFileRecord();

    private enum ObjStatus
    {
      Idle,
      ObjectReleased,
    }
  }
}
