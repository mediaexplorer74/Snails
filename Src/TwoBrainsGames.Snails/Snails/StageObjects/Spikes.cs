
// Type: TwoBrainsGames.Snails.StageObjects.Spikes
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.SpacePartitioning;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class Spikes : StageObject, ISwitchable
  {
    public const string ID = "SPIKES";
    public const int ACTIVATION_TIME = 3000;
    public const int STARTUP_DELAY = 2000;
    public const int RETREAT_TIME = 2000;
    private string RES_SPIKES = "spriteset/stage-objects";
    private string SPRITE_SPIKES_UP = "SpikesUp";
    private string SPRITE_SPIKES_DOWN = "SpikesDown";
    private double _elapsedTimeToActivate;
    private double _elapsedTimeToRelease;
    private Sprite _SpriteSpikesIdle;
    private Sprite _SpriteSpikesUp;
    private Sprite _SpriteSpikesDown;
    private Snail _empaledSnail;
    private Sample _spikeSample;
    private Sample _spikeHitSnailSound;
    private Sample _spikesClosingSound;

    public int ActivationTime { get; set; }

    public int StartupDelay { get; set; }

    private Spikes.SpikesStateType State { get; set; }

    public bool TurnedOn { get; set; }

    public Spikes()
      : base(StageObjectType.Spikes)
    {
      this.ActivationTime = 3000;
      this.StartupDelay = 2000;
      this.TurnedOn = true;
    }

    public Spikes(Spikes other)
      : base((StageObject) other)
    {
      this.Copy((StageObject) other);
    }

    public override void Copy(StageObject other)
    {
      this.ActivationTime = ((Spikes) other).ActivationTime;
      this.StartupDelay = ((Spikes) other).StartupDelay;
      base.Copy(other);
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this._SpriteSpikesIdle = this.Sprite;
      this._SpriteSpikesUp = BrainGame.ResourceManager.GetSpriteTemporary(this.RES_SPIKES, this.SPRITE_SPIKES_UP);
      this._SpriteSpikesDown = BrainGame.ResourceManager.GetSpriteTemporary(this.RES_SPIKES, this.SPRITE_SPIKES_DOWN);
      this._spikeSample = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/spikes", (Object2D) this);
      this._spikeHitSnailSound = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/spikes-hit-snail", (Object2D) this);
      this._spikesClosingSound = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/spikes-closing", (Object2D) this);
    }

    public override void Initialize()
    {
      base.Initialize();
      this._crateCollisionBB = this.TransformBoundingBox(this.Sprite.BoundingBox).ToBoundingSquare();
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      switch (this.State)
      {
        case Spikes.SpikesStateType.StartingUp:
          if (!this.IsOn)
            break;
          this._elapsedTimeToActivate += gameTime.ElapsedGameTime.TotalMilliseconds;
          if (this._elapsedTimeToActivate <= (double) this.StartupDelay)
            break;
          this.State = Spikes.SpikesStateType.Idle;
          this._elapsedTimeToActivate = (double) this.ActivationTime + (this._elapsedTimeToActivate - (double) this.StartupDelay);
          break;
        case Spikes.SpikesStateType.Idle:
          if (!this.IsOn)
            break;
          this._elapsedTimeToActivate += gameTime.ElapsedGameTime.TotalMilliseconds;
          if (this._elapsedTimeToActivate <= (double) this.ActivationTime)
            break;
          this._elapsedTimeToActivate = 0.0;
          this.CurrentFrame = 0;
          this.Sprite = this._SpriteSpikesUp;
          this.State = Spikes.SpikesStateType.Activated;
          this.DoQuadtreeCollisions(0);
          this._elapsedTimeToRelease = this._elapsedTimeToActivate - (double) this.ActivationTime;
          break;
        case Spikes.SpikesStateType.Activated:
          if (!this.IsOn)
            break;
          this.State = Spikes.SpikesStateType.WaitToRelease;
          if (this._spikeSample != null && !this._spikeSample.IsPlaying)
            this._spikeSample.Play();
          this.DoQuadtreeCollisions(0);
          break;
        case Spikes.SpikesStateType.Releasing:
          if (this._empaledSnail == null)
            break;
          this.PlaceSnailOnSpikes();
          break;
        case Spikes.SpikesStateType.WaitToRelease:
          this._elapsedTimeToRelease += gameTime.ElapsedGameTime.TotalMilliseconds;
          if (this._elapsedTimeToRelease <= 2000.0)
            break;
          this.CurrentFrame = 0;
          this.Sprite = this._SpriteSpikesDown;
          this.State = Spikes.SpikesStateType.Releasing;
          this._elapsedTimeToActivate = this._elapsedTimeToRelease - 2000.0;
          this._spikesClosingSound.Play();
          break;
      }
    }

    public override void OnLastFrame()
    {
      base.OnLastFrame();
      if (this.State != Spikes.SpikesStateType.Releasing)
        return;
      this.CurrentFrame = 0;
      this.Sprite = this._SpriteSpikesIdle;
      this.State = Spikes.SpikesStateType.Idle;
      if (this._empaledSnail == null)
        return;
      this._empaledSnail.UnempaleFromSpikes();
      this._empaledSnail = (Snail) null;
    }

    public override void OnCollide(IQuadtreeContainable obj, int listIdx)
    {
      if (this._empaledSnail != null)
        return;
      Snail snail = obj as Snail;
      snail.KillBySpikes(this);
      this._empaledSnail = snail;
      this.PlaceSnailOnSpikes();
      this._spikeHitSnailSound.Play();
    }

    private void PlaceSnailOnSpikes()
    {
      BoundingSquare boundingSquare = this.TransformCurrentFrameBB().ToBoundingSquare();
      switch ((int) this.Rotation)
      {
        case 0:
          this._empaledSnail.Position = new Vector2(boundingSquare.Left - this._empaledSnail.Sprite.BoundingBox.Left, boundingSquare.Top - this._empaledSnail.Sprite.BoundingBox.Top - this._empaledSnail.Sprite.BoundingBox.Height);
          break;
        case 90:
          this._empaledSnail.Position = new Vector2(boundingSquare.Left + boundingSquare.Width + this._empaledSnail.Sprite.BoundingBox.Top + this._empaledSnail.Sprite.BoundingBox.Height, boundingSquare.Top - this._empaledSnail.Sprite.BoundingBox.Left);
          break;
        case 180:
          this._empaledSnail.Position = new Vector2(boundingSquare.Left - this._empaledSnail.Sprite.BoundingBox.Left, boundingSquare.Top + this._empaledSnail.Sprite.BoundingBox.Top + this._empaledSnail.Sprite.BoundingBox.Height + boundingSquare.Height);
          break;
        case 270:
          this._empaledSnail.Position = new Vector2(boundingSquare.Left - this._empaledSnail.Sprite.BoundingBox.Top - this._empaledSnail.Sprite.BoundingBox.Height, boundingSquare.Top - this._empaledSnail.Sprite.BoundingBox.Left);
          break;
      }
    }

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord(context);
      if (context == ToDataFileRecordContext.StageSave)
      {
        dataFileRecord.AddField("activationTime", (object) this.ActivationTime);
        dataFileRecord.AddField("startupDelay", (object) this.StartupDelay);
        dataFileRecord.AddField("on", (object) this.TurnedOn);
      }
      return dataFileRecord;
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this.ActivationTime = record.GetFieldValue<int>("activationTime", 3000);
      this.StartupDelay = record.GetFieldValue<int>("startupDelay", 2000);
      this.TurnedOn = record.GetFieldValue<bool>("on", true);
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public override bool CrateToolIsValid(BoundingSquare crateBs)
    {
      return !crateBs.Collides(this._crateCollisionBB);
    }

    public void SwitchOn() => this.TurnedOn = true;

    public void SwitchOff() => this.TurnedOn = false;

    public bool IsOn => this.TurnedOn;

    private enum SpikesStateType
    {
      StartingUp,
      Idle,
      Activated,
      Releasing,
      WaitToRelease,
      Off,
    }
  }
}
