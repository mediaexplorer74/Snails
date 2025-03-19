
// Type: TwoBrainsGames.Snails.StageObjects.Explosion
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class Explosion : StageObject
  {
    public const string ID = "EXPLOSION";
    public const int EXPLOSION_TIME = 1300;
    public const int RUMBLE_SHAKE_TIME = 750;
    public const int CAMERA_SHAKE_TIME = 800;
    public const int CAMERA_SHAKE_TIME_LIQUID = 500;
    public const int CAMERA_SHAKE_STRENGHT = 10;
    public const int CAMERA_SHAKE_STRENGHT_LIQUID = 3;
    public const float EXPLOSION_LAYER_DEPTH = 0.8f;
    public const int SPRITE_BB_IDX_SMALL_EXPLOSION = 0;
    public const int SPRITE_BB_IDX_BIG_EXPLOSION = 1;
    private int _countSnailsExploded;
    protected Explosion.ExplosionType _typeExplosion;
    protected Sample[] _explosionSample;
    protected Sample _underWaterExplosionSample;
    protected Sample _soundToPlay;
    private BoundingCircle _tilesCollisionBS;
    private BoundingSquare _tilesCollisionBB;
    private BoundingCircle _objCollisionBS;
    private BoundingSquare _objCollisionBB;
    private Explosion.StatusType _explosionStatus;
    private Sprite _smokeSprite;

    private int CameraShakeStrength => !this.IsUnderLiquid ? 10 : 3;

    private int CameraShakeTime => !this.IsUnderLiquid ? 800 : 500;

    public BoundingSquare ObjCollisionBB => this._objCollisionBB;

    private bool Exploded { get; set; }

    public Explosion.ExplosionSize TileKillBBSize { get; set; }

    public Explosion.ExplosionSize ObjectKillBBSize { get; set; }

    public Explosion.ObjectTypeAffected AffectedObjects { get; set; }

    public Explosion.ExplosionRadiusType RadiusType { get; set; }

    public bool DestroyUnbreakbleTiles { get; set; }

    public bool ThrowParticles { get; set; }

    public Sprite SmokeSprite
    {
      get => this._smokeSprite;
      set => this._smokeSprite = value;
    }

    public override BoundingSquare QuadtreeCollisionBB => this._objCollisionBB;

    public StageObject ExplosionSource { get; set; }

    public Explosion()
      : base(StageObjectType.Explosion)
    {
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this._explosionSample = new Sample[3];
      this._explosionSample[0] = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/explosion-1", (Object2D) this);
      this._explosionSample[1] = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/explosion-2", (Object2D) this);
      this._explosionSample[2] = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/explosion-3", (Object2D) this);
      this._underWaterExplosionSample = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/explosion_underwater", (Object2D) this);
      this._smokeSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/stage-objects", "ExplosionSmoke");
    }

    private Sample GetSoundToPlay()
    {
      return this.IsUnderLiquid ? this._underWaterExplosionSample : this._explosionSample[BrainGame.Rand.Next(this._explosionSample.Length)];
    }

    public override void Copy(StageObject other)
    {
      base.Copy(other);
      this._explosionSample = (other as Explosion)._explosionSample;
    }

    public BoundingSquare GetObjCollisionBB()
    {
      return this.Sprite.BoundingBoxes[(int) this.TileKillBBSize].Transform(this.Position);
    }

    public override void OnAddedToStage()
    {
      if (this.RadiusType == Explosion.ExplosionRadiusType.Circle)
      {
        this._tilesCollisionBS = this.TransformBoundingCircle(this.Sprite._boundingSpheres[(int) this.TileKillBBSize]);
        this._tilesCollisionBB = this._tilesCollisionBS.GetContainingSquare();
        this._objCollisionBS = this.TransformBoundingCircle(this.Sprite._boundingSpheres[(int) this.ObjectKillBBSize]);
        this._objCollisionBB = this._objCollisionBS.GetContainingSquare();
      }
      else
      {
        this._tilesCollisionBB = this.Sprite.BoundingBoxes[(int) this.TileKillBBSize].Transform(this.Position);
        this._objCollisionBB = this.GetObjCollisionBB();
      }
      this.RepositionObjectInQuadtree();
      Stage.CurrentStage.SendExplosionNotification(this);
    }

    public override void OnCollide(IQuadtreeContainable obj, int listIdx)
    {
      bool flag = false;
      StageObject stageObject = (StageObject) obj;
      if (!stageObject.CanDieWithExplosions || !stageObject.CanDieWithAnyTypeOfExplosion && ((this.AffectedObjects & Explosion.ObjectTypeAffected.Objects) != Explosion.ObjectTypeAffected.Objects && !(stageObject is Snail) || (this.AffectedObjects & Explosion.ObjectTypeAffected.Snails) != Explosion.ObjectTypeAffected.Snails && stageObject is Snail))
        return;
      if (this.RadiusType == Explosion.ExplosionRadiusType.Circle)
      {
        if (this._objCollisionBS.Collides(stageObject.QuadtreeCollisionBB))
        {
          (obj as StageObject).KillByExplosion(this);
          flag = true;
        }
      }
      else if (this._objCollisionBB.Collides(stageObject.QuadtreeCollisionBB))
      {
        (obj as StageObject).KillByExplosion(this);
        flag = true;
      }
      if (!flag || !(stageObject is Snail) || ((Snail) stageObject).IsEvilSnail || !(this.ExplosionSource is Dynamite))
        return;
      ++this._countSnailsExploded;
      if (this._countSnailsExploded < 70)
        return;
      BrainGame.AchievementsManager.Notify(15);
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      if (this._explosionStatus == Explosion.StatusType.Smoking)
        return;
      if (this.Exploded)
      {
        this.DoQuadtreeCollisions(1);
        this.DoQuadtreeCollisions(0);
      }
      else
      {
        if ((this.AffectedObjects & Explosion.ObjectTypeAffected.Tiles) == Explosion.ObjectTypeAffected.Tiles)
          this.CheckCollisionWithTiles();
        Stage.CurrentStage.Camera.Shake(this.CameraShakeTime, this.CameraShakeStrength);
        if (Game1.GameSettings.WithRumbble)
          Stage.CurrentStage.Rumble.AddEffect(750, 1f, 1f);
        if (!this.Exploded)
        {
          this._soundToPlay = this.GetSoundToPlay();
          this._soundToPlay.Play();
        }
        this.Exploded = true;
      }
    }

    private void CheckCollisionWithTiles()
    {
      TileCellCoords tileCellCoordsAt1 = Stage.CurrentStage.Board.GetTileCellCoordsAt(this._tilesCollisionBB.UpperLeft);
      TileCellCoords tileCellCoordsAt2 = Stage.CurrentStage.Board.GetTileCellCoordsAt(this._tilesCollisionBB.LowerRight);
      if (tileCellCoordsAt1.ColIndex < 0)
        tileCellCoordsAt1.ColIndex = 0;
      if (tileCellCoordsAt1.RowIndex < 0)
        tileCellCoordsAt1.RowIndex = 0;
      for (int colIndex = tileCellCoordsAt1.ColIndex; colIndex <= tileCellCoordsAt2.ColIndex && colIndex < Stage.CurrentStage.Board.Columns; ++colIndex)
      {
        for (int rowIndex = tileCellCoordsAt1.RowIndex; rowIndex <= tileCellCoordsAt2.RowIndex && rowIndex < Stage.CurrentStage.Board.Rows; ++rowIndex)
        {
          if (Stage.CurrentStage.Board.Tiles[rowIndex, colIndex] != null && Stage.CurrentStage.Board.Tiles[rowIndex, colIndex].Tile != null && (Stage.CurrentStage.Board.Tiles[rowIndex, colIndex].Tile.IsBreakable || this.DestroyUnbreakbleTiles))
          {
            BoundingSquare boundingSquare = new BoundingSquare(new Vector2((float) (colIndex * 60), (float) (rowIndex * 60)), 60f, 60f);
            if (this.RadiusType == Explosion.ExplosionRadiusType.Circle)
            {
              if (this._tilesCollisionBS.Collides(boundingSquare))
              {
                if (this.ThrowParticles)
                  Stage.CurrentStage.Board.Tiles[rowIndex, colIndex].Break();
                Stage.CurrentStage.Board.RemoveTileAt(colIndex, rowIndex);
              }
            }
            else if (this._tilesCollisionBB.Collides(boundingSquare))
            {
              if (this.ThrowParticles)
                Stage.CurrentStage.Board.Tiles[rowIndex, colIndex].Break();
              Stage.CurrentStage.Board.RemoveTileAt(colIndex, rowIndex);
            }
          }
        }
      }
    }

    public override void OnLastFrame()
    {
      switch (this._explosionStatus)
      {
        case Explosion.StatusType.Exploding:
          this._explosionStatus = Explosion.StatusType.Smoking;
          this.CurrentFrame = 0;
          this.Sprite = this._smokeSprite;
          break;
        case Explosion.StatusType.Smoking:
          this.DisposeFromStage();
          break;
      }
    }

    public void SetUnderLiquid(bool underLiquid) => this.DrawInForeground = !underLiquid;

    public enum ExplosionSize
    {
      Small,
      Medium,
    }

    public enum ExplosionType
    {
      Dynamite,
      Tile,
    }

    public enum ExplosionRadiusType
    {
      Circle,
      Square,
    }

    private enum StatusType
    {
      Exploding,
      Smoking,
    }

    [Flags]
    public enum ObjectTypeAffected
    {
      Tiles = 1,
      Snails = 2,
      Objects = 4,
      All = Objects | Snails | Tiles, // 0x00000007
    }
  }
}
