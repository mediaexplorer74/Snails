
// Type: TwoBrainsGames.Snails.StageObjects.DynamiteBox
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class DynamiteBox : Box, ISnailsDataFileSerializable, IDataFileSerializable
  {
    public new const string ID = "DYNAMITE_BOX";
    public const int EXPLOSION_TIME = 9000;
    public const string SPRITE_BOX = "CommonTiles";
    private string RES_DYNAMITE_BOX = "spriteset/stage-objects";
    private string SPRITE_BOX_TIMER_FONT = "BoxTimeCounterFont";
    protected Sprite _spriteCounter;
    protected DynamiteBox.DynamiteBoxStatus _status;
    protected double _elapsedTimeToExplode;
    protected bool _isActive;
    protected bool _counterVisible;
    protected Sample _tickSample;
    private string _counterString;
    protected int _currentSecond;
    private int _char1;
    private int _char2;
    private Vector2 _counterPosition;
    private Vector2 _counterChar2Position;
    private string _implosionSpriteRes;
    private string _implosionEndSpriteRes;
    private Sprite _implosionSprite;
    private Sprite _implosionEndSprite;
    private Explosion _explosion;

    public new Sprite Sprite
    {
      get => base.Sprite;
      set => base.Sprite = value;
    }

    public DynamiteBox()
      : this(StageObjectType.DynamiteBox)
    {
    }

    public DynamiteBox(StageObjectType type)
      : base(type)
    {
      this._status = DynamiteBox.DynamiteBoxStatus.BoxDrop;
      this._elapsedTimeToExplode = 0.0;
      this._isActive = true;
      this._counterVisible = true;
      this._currentSecond = 0;
    }

    public override void Copy(StageObject other)
    {
      base.Copy(other);
      DynamiteBox dynamiteBox = other as DynamiteBox;
      this._currentSecond = dynamiteBox._currentSecond;
      this._spriteCounter = dynamiteBox._spriteCounter;
      this._counterVisible = dynamiteBox._counterVisible;
      this._tickSample = dynamiteBox._tickSample;
      this._implosionSpriteRes = dynamiteBox._implosionSpriteRes;
      this._implosionEndSpriteRes = dynamiteBox._implosionEndSpriteRes;
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this._spriteCounter = BrainGame.ResourceManager.GetSpriteTemporary(this.RES_DYNAMITE_BOX, this.SPRITE_BOX_TIMER_FONT);
      this._tickSample = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/crate-timer-tick");
      if (!string.IsNullOrEmpty(this._implosionSpriteRes))
        this._implosionSprite = BrainGame.ResourceManager.GetSpriteTemporary(this._implosionSpriteRes);
      if (string.IsNullOrEmpty(this._implosionEndSpriteRes))
        return;
      this._implosionEndSprite = BrainGame.ResourceManager.GetSpriteTemporary(this._implosionEndSpriteRes);
    }

    public override void Initialize()
    {
      base.Initialize();
      this.PreComputeCounterData(9);
    }

    public override void StageInitialize()
    {
      base.StageInitialize();
      this.BoxDeployed(Stage.LoadingContext == Stage.StageLoadingContext.Gameplay, false, false);
    }

    public override void OnLastFrame()
    {
      if (this._status != DynamiteBox.DynamiteBoxStatus.BoxDrop)
        return;
      this.BoxDeployed(true, true, true);
      this.SwitchObjects();
    }

    protected override void BoxDeployed(bool addTile, bool addPaths, bool checkCollisions)
    {
      base.BoxDeployed(addTile, addPaths, checkCollisions);
      this._status = DynamiteBox.DynamiteBoxStatus.Counting;
      this.SetSpriteWhenIdle();
      this.PreComputeCounterData(9);
      this._deployStatus = Box.BoxDeployStatus.Deployed;
    }

    protected virtual void SnailCollided(Snail snail)
    {
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      switch (this._status)
      {
        case DynamiteBox.DynamiteBoxStatus.Counting:
          this._elapsedTimeToExplode += gameTime.ElapsedGameTime.TotalMilliseconds;
          if (this._elapsedTimeToExplode > 9000.0)
          {
            this._elapsedTimeToExplode = 0.0;
            this.Explode();
            break;
          }
          int seconds = (int) Math.Ceiling((9000.0 - this._elapsedTimeToExplode) / 1000.0);
          if (seconds == this._currentSecond)
            break;
          this._tickSample.Play();
          this._currentSecond = seconds;
          this.PreComputeCounterData(seconds);
          break;
        case DynamiteBox.DynamiteBoxStatus.Explode:
          this.Explode();
          break;
      }
    }

    public override void Draw(bool shadow)
    {
      base.Draw(shadow);
      if (!this._counterVisible)
        return;
      if (this._counterString.Length == 1)
      {
        this._spriteCounter.Draw(this._counterPosition, this._char1, Stage.CurrentStage.SpriteBatch);
      }
      else
      {
        this._spriteCounter.Draw(this._counterPosition, this._char1, Stage.CurrentStage.SpriteBatch);
        this._spriteCounter.Draw(this._counterChar2Position, this._char2, Stage.CurrentStage.SpriteBatch);
      }
    }

    public override void OnCollide(IQuadtreeContainable obj, int listIdx)
    {
      if (this._deployStatus == Box.BoxDeployStatus.Deploying)
        base.OnCollide(obj, listIdx);
      else if (this._status == DynamiteBox.DynamiteBoxStatus.Explode)
        ((StageObject) obj).KillByExplosion(this._explosion);
      else
        this.SnailCollided((Snail) obj);
    }

    public override void KillByExplosion(Explosion exp) => this.Explode();

    protected void Explode()
    {
      this._status = DynamiteBox.DynamiteBoxStatus.Explode;
      TileCellCoords coordsFromPosition = Stage.CurrentStage.Board.GetCoordsFromPosition(this.Position);
      if (Stage.CurrentStage.Board.Tiles[coordsFromPosition.RowIndex, coordsFromPosition.ColIndex] != null)
      {
        if (!this.IsUnderLiquid)
          Stage.CurrentStage.Board.Tiles[coordsFromPosition.RowIndex, coordsFromPosition.ColIndex].Break();
        Stage.CurrentStage.Board.RemoveTileAt(coordsFromPosition.ColIndex, coordsFromPosition.RowIndex);
      }
      bool flag = false;
      if (this.IsUnderLiquid)
      {
        float top = this._inLiquidRef.QuadtreeCollisionBB.Top;
        float y = this.Y;
        if ((double) top <= (double) y || (double) top <= (double) y + (double) this.AABoundingBox.Height / 2.0)
          flag = true;
      }
      this._explosion = this.Explode(Explosion.ExplosionSize.Small, Explosion.ExplosionSize.Medium, Explosion.ExplosionRadiusType.Square, Explosion.ObjectTypeAffected.Snails, new Vector2(this.Position.X + (float) (this.Sprite.Width / 2), this.Position.Y + (float) (this.Sprite.Height / 2)), false, flag ? this._implosionSprite : (Sprite) null, flag ? this._implosionEndSprite : (Sprite) null, false, this.IsUnderLiquid);
      this.AABoundingBox = this._explosion.GetObjCollisionBB();
      this.DoQuadtreeCollisions(0);
      Stage.CurrentStage.DisposeObject((StageObject) this);
    }

    public void PreComputeCounterData(int seconds)
    {
      this._counterString = seconds.ToString();
      float num;
      if (this._counterString.Length == 1)
      {
        this._char1 = (int) this._counterString[0] - 48;
        this._char2 = -1;
        num = (float) this._spriteCounter.Frames[this._char1].Width;
      }
      else
      {
        this._char1 = (int) this._counterString[0] - 48;
        this._char2 = (int) this._counterString[1] - 48;
        num = (float) (this._spriteCounter.Frames[this._char1].Width + this._spriteCounter.Frames[this._char2].Width);
      }
      this._counterPosition = this.Position + new Vector2((float) (this.Sprite.Width / 2) - num / 2f, (float) (this.Sprite.Height / 2 - this._spriteCounter.Height / 2));
      this._counterChar2Position = this._counterPosition + new Vector2((float) this._spriteCounter.Frames[this._char1].Width, 0.0f);
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this._implosionSpriteRes = record.GetFieldValue<string>("implosionSpriteRes", this._implosionSpriteRes);
      this._implosionEndSpriteRes = record.GetFieldValue<string>("implosionEndSpriteRes", this._implosionEndSpriteRes);
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord(context);
      dataFileRecord.AddField("implosionSpriteRes", (object) this._implosionSpriteRes);
      dataFileRecord.AddField("implosionEndSpriteRes", (object) this._implosionEndSpriteRes);
      return dataFileRecord;
    }

    protected enum DynamiteBoxStatus
    {
      BoxDrop,
      Counting,
      Explode,
      Deployed,
      ExplosionDelay,
      Idle,
    }
  }
}
