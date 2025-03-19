
// Type: TwoBrainsGames.Snails.StageObjects.StageEntrance
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class StageEntrance : StageObject, ISnailsDataFileSerializable, IDataFileSerializable
  {
    private const int CRATE_TOOL_VALID_BB_IDX = 0;
    private const int DEFAULT_RELEASE_INTERVAL = 1000;
    private const string SPRITE_ENTRANCE_IDLE = "EntranceIdle";
    private const string SPRITE_ENTRANCE_RELEASING = "Entrance";
    private const string SPRITE_SNAIL_RELEASE_RIGHT = "SnailReleaseRight";
    private const string SPRITE_SNAIL_RELEASE_LEFT = "SnailReleaseLeft";
    private const int KING_SIGN_BB_IDX = 0;
    private const int EVIL_SNAIL_SIGN_BB_IDX = 1;
    protected Sample _eyeBlinkSample;
    protected Sample _entranceSample;
    public StageEntrance.StageEntranceState State;
    public int TotalSnailsToRelease;
    public int SnailsToReleaseBeforeKing;
    public int InitialReleaseDelay;
    public StageEntrance.EntranceReleaseDirection ReleaseDirection;
    public bool ReleasesSnailKing;
    public bool _snailKingReleased;
    private double _ellapsedReleaseTime;
    private double _ellapsedInitialTime;
    private MovingObject.WalkDirection _walkDirection;
    private Sprite _kingSignSprite;
    private Sprite _evilSnailSignSprite;
    private Vector2 _kingSignPos;
    private Vector2 _evilSnailSignPos;
    private Sprite _releasingSnailSprite;
    private Sprite _releasingSnailLeft;
    private Sprite _releasingSnailRight;
    private Sprite _idleSprite;

    public int SnailsToRelease { get; set; }

    public int IntervalToRelease { get; set; }

    public SnailCounter SnailCounter { get; set; }

    public string SnailsToReleaseId { get; set; }

    public bool ReleasesEvilSnails => this.SnailsToReleaseId == "EVIL_SNAIL";

    public StageEntrance()
      : base(StageObjectType.StageEntrance)
    {
      this.TotalSnailsToRelease = this.SnailsToRelease = 0;
      this.SnailsToReleaseBeforeKing = 0;
      this.IntervalToRelease = 1000;
      this.InitialReleaseDelay = 0;
      this.ReleaseDirection = StageEntrance.EntranceReleaseDirection.Clockwise;
      this.State = StageEntrance.StageEntranceState.InitialDelay;
      this.SpriteId = "EntranceIdle";
      this._walkDirection = MovingObject.WalkDirection.Clockwise;
      this.SnailsToReleaseId = "SNAIL";
    }

    public StageEntrance(StageEntrance other)
      : base((StageObject) other)
    {
      this.Copy((StageObject) other);
    }

    public override void Copy(StageObject other)
    {
      base.Copy(other);
      StageEntrance stageEntrance = (StageEntrance) other;
      this.SnailsToRelease = stageEntrance.SnailsToRelease;
      this.TotalSnailsToRelease = this.SnailsToRelease;
      this.SnailsToReleaseBeforeKing = stageEntrance.SnailsToReleaseBeforeKing;
      this.IntervalToRelease = stageEntrance.IntervalToRelease;
      this.InitialReleaseDelay = stageEntrance.InitialReleaseDelay;
      this.ReleaseDirection = stageEntrance.ReleaseDirection;
      this.State = stageEntrance.State;
      this.ReleasesSnailKing = stageEntrance.ReleasesSnailKing;
      this._snailKingReleased = stageEntrance._snailKingReleased;
      this._walkDirection = stageEntrance._walkDirection;
      this._eyeBlinkSample = stageEntrance._eyeBlinkSample;
    }

    public override void Initialize()
    {
      base.Initialize();
      this._ellapsedReleaseTime = (double) (this.IntervalToRelease - 1000);
      this._ellapsedInitialTime = 0.0;
      this.State = StageEntrance.StageEntranceState.InitialDelay;
      this._kingSignPos = this.TransformSpriteFrameBB(0).GetCenter();
      this._evilSnailSignPos = this.TransformSpriteFrameBB(1).GetCenter();
    }

    public override void LoadContent()
    {
      base.LoadContent();
      if (this.SnailCounter != null)
        this.SnailCounter.LoadContent();
      this._eyeBlinkSample = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/snails_eye_blink_echo", (Object2D) this);
      this._entranceSample = BrainGame.ResourceManager.GetSample(AudioTags.SNAIL_ENTRANCE, "STAGE_THEME_RESOURCES", (Object2D) this);
      this._kingSignSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/stage-objects/SnailEntranceKing");
      this._evilSnailSignSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/stage-objects/SnailEntranceEvil");
      this._releasingSnailSprite = BrainGame.ResourceManager.GetSprite(this.ResourceId + "/Entrance", "STAGE_THEME_RESOURCES");
      this._releasingSnailLeft = BrainGame.ResourceManager.GetSprite(this.ResourceId + "/SnailReleaseLeft", "STAGE_THEME_RESOURCES");
      this._releasingSnailRight = BrainGame.ResourceManager.GetSprite(this.ResourceId + "/SnailReleaseRight", "STAGE_THEME_RESOURCES");
      this._idleSprite = BrainGame.ResourceManager.GetSprite(this.ResourceId + "/EntranceIdle", "STAGE_THEME_RESOURCES");
    }

    public override void OnLastFrame()
    {
      switch (this.State)
      {
        case StageEntrance.StageEntranceState.Shaking:
          this.CurrentFrame = 0;
          this.State = StageEntrance.StageEntranceState.Releasing;
          if (this._walkDirection == MovingObject.WalkDirection.Clockwise)
          {
            this.Sprite = this._releasingSnailRight;
            break;
          }
          this.Sprite = this._releasingSnailLeft;
          break;
        case StageEntrance.StageEntranceState.Releasing:
          this.CurrentFrame = 0;
          this.State = StageEntrance.StageEntranceState.Idle;
          this.Sprite = this._idleSprite;
          if (this.ReleasesSnailKing && !this._snailKingReleased && this.TotalSnailsToRelease - this.SnailsToRelease == this.SnailsToReleaseBeforeKing)
          {
            Stage.CurrentStage.ReleaseSnailKing(this.Position, this._walkDirection);
            this._snailKingReleased = true;
          }
          else
            Stage.CurrentStage.ReleaseSnail(this.Position, this._walkDirection, this.SnailsToReleaseId);
          --this.SnailsToRelease;
          if (this.ReleaseDirection != StageEntrance.EntranceReleaseDirection.Both)
            break;
          this._walkDirection = (MovingObject.WalkDirection) ((int) this._walkDirection * -1);
          break;
      }
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      if (this.SnailCounter != null)
        this.SnailCounter.SetCounter(this.SnailsToRelease);
      switch (this.State)
      {
        case StageEntrance.StageEntranceState.InitialDelay:
          this._ellapsedInitialTime += gameTime.ElapsedGameTime.TotalMilliseconds;
          if (this._ellapsedInitialTime <= (double) this.InitialReleaseDelay)
            break;
          this.State = StageEntrance.StageEntranceState.Idle;
          break;
        case StageEntrance.StageEntranceState.Idle:
          if (this.SnailsToRelease <= 0)
            break;
          this._ellapsedReleaseTime += gameTime.ElapsedGameTime.TotalMilliseconds;
          if (this._ellapsedReleaseTime <= (double) this.IntervalToRelease)
            break;
          this.CurrentFrame = 0;
          this.Sprite = this._releasingSnailSprite;
          this.State = StageEntrance.StageEntranceState.Shaking;
          this._ellapsedReleaseTime -= (double) this.IntervalToRelease;
          this._ellapsedReleaseTime = 0.0;
          this._entranceSample.Play();
          break;
      }
    }

    public override void Draw(bool shadow)
    {
      base.Draw(shadow);
      if (Stage.CurrentStage._state != Stage.StageState.Startup)
        return;
      if (this.ReleasesSnailKing)
        this._kingSignSprite.Draw(this._kingSignPos, Stage.CurrentStage.SpriteBatch);
      if (!this.ReleasesEvilSnails)
        return;
      this._evilSnailSignSprite.Draw(this._evilSnailSignPos, Stage.CurrentStage.SpriteBatch);
    }

    public override void AddLinkedObject(StageObject obj)
    {
      this._linkedObjects.Add(obj);
      if (!(obj is SnailCounter))
        return;
      this.SnailCounter = (SnailCounter) obj;
      this.SnailCounter.SetCounter(this.SnailsToRelease);
      this.SnailCounter.ConnectToEntrance(this);
    }

    public void SetSnailCounter(SnailCounter counter)
    {
      this.SnailCounter = counter;
      this.LinkTo((StageObject) counter);
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this.SnailsToRelease = record.GetFieldValue<int>("snails", 0);
      this.TotalSnailsToRelease = this.SnailsToRelease;
      this.IntervalToRelease = record.GetFieldValue<int>("interval", this.IntervalToRelease);
      this.SnailsToReleaseBeforeKing = record.GetFieldValue<int>("snailsToReleaseBeforeKing", this.SnailsToReleaseBeforeKing);
      this.InitialReleaseDelay = record.GetFieldValue<int>("initialDelay", this.InitialReleaseDelay);
      this.SnailsToReleaseId = record.GetFieldValue<string>("snailsToReleaseId", this.SnailsToReleaseId);
      this.ReleaseDirection = (StageEntrance.EntranceReleaseDirection) Enum.Parse(typeof (StageEntrance.EntranceReleaseDirection), record.GetFieldValue<string>("direction", StageEntrance.EntranceReleaseDirection.Clockwise.ToString()), false);
      this.State = (StageEntrance.StageEntranceState) Enum.Parse(typeof (StageEntrance.StageEntranceState), record.GetFieldValue<string>("state", StageEntrance.StageEntranceState.Idle.ToString()), false);
      this.ReleasesSnailKing = record.GetFieldValue<bool>("releasesSnailKing", this.ReleasesSnailKing);
      if (this.ReleaseDirection == StageEntrance.EntranceReleaseDirection.CounterClockwise)
        this._walkDirection = MovingObject.WalkDirection.CounterClockwise;
      else
        this._walkDirection = MovingObject.WalkDirection.Clockwise;
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord(context);
      if (context == ToDataFileRecordContext.StageSave)
      {
        dataFileRecord.AddField("snails", (object) this.SnailsToRelease);
        dataFileRecord.AddField("interval", (object) this.IntervalToRelease);
        dataFileRecord.AddField("initialDelay", (object) this.InitialReleaseDelay);
        dataFileRecord.AddField("direction", (object) this.ReleaseDirection.ToString());
        dataFileRecord.AddField("releasesSnailKing", (object) this.ReleasesSnailKing);
        dataFileRecord.AddField("snailsToReleaseId", (object) this.SnailsToReleaseId);
        dataFileRecord.AddField("snailsToReleaseBeforeKing", (object) this.SnailsToReleaseBeforeKing);
      }
      return dataFileRecord;
    }

    public enum StageEntranceState
    {
      InitialDelay,
      Idle,
      Shaking,
      Releasing,
    }

    public enum EntranceReleaseDirection
    {
      Clockwise,
      CounterClockwise,
      Both,
    }
  }
}
