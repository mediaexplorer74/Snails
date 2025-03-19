
// Type: TwoBrainsGames.Snails.StageObjects.StageObject
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class StageObject : 
    Object2D,
    ISnailsDataFileSerializable,
    IDataFileSerializable,
    IQuadtreeContainable
  {
    public const int TRANSF_EFFECT_GRAVITY = 2;
    public const int TRANSF_EFFECT_HOOVER = 1;
    public const int TRANSF_EFFECT_LIQUID_GRAVITY = 3;
    public const int TRANSF_EFFECT_ROTATION = 4;
    public const float OBJ_HOOVER_SPEED = 0.06f;
    public const float OBJ_HOOVER_POWER = 0.3f;
    public const string SPRITE_DEATH_BY_FIRE = "FireDeath";
    public const string SPRITE_DEATH_BY_SPIKES = "SpikesDeath";
    private Color DEFAULT_COLOR = Color.White;
    private StageObjectType _type;
    private bool _canAnimate;
    protected List<StageObject> _linkedObjects;
    protected string _linkString;
    public bool DrawInForeground;
    public StageObjectStaticFlags StaticFlags;
    public StageObjectDynamicFlags DynamicFlags;
    public bool IsStageObject;
    public Vector2 PreviousPosition;
    protected bool Killed;
    protected Matrix _rotationZMatrix = Matrix.Identity;
    private bool _faddingOut;
    private float _fadeAlpha;
    private float _fadeSpeed;
    private Color _shadowColor;
    protected Liquid _inLiquidRef;
    protected BoundingSquare _crateCollisionBB;
    private int _crateCollisionBBIdx;
    private string _deathFireSpriteRes;
    private Sprite _deathFireSprite;
    protected bool _projectWhenKilledByCrate;
    private bool _preLoad;
    protected Sample _killedByFire;
    protected bool _autoDisposeIfDeadOnAnimationEnd;
    public string _contentManagerId;

    public bool PreLoad => this._preLoad;

    protected Color ShadowColor
    {
      get
      {
        if (this._colorChanged)
        {
          Vector4 vector4_1 = Stage.CurrentStage.ShadowColor.ToVector4();
          Vector4 vector4_2 = this.BlendColor.ToVector4();
          this._shadowColor = new Color(new Vector4(vector4_1.X, vector4_1.Y, vector4_1.Z, vector4_1.W * vector4_2.W));
        }
        return this._shadowColor;
      }
    }

    public virtual BoundingSquare QuadtreeCollisionBB => this.AABoundingBox;

    public StageObjectType Type
    {
      get => this._type;
      set => this._type = value;
    }

    public override float Rotation
    {
      get => base.Rotation;
      set
      {
        if ((double) this._rotation == (double) value)
          return;
        base.Rotation = value;
        this._rotationZMatrix = Matrix.CreateRotationZ(this._rotationInRad);
      }
    }

    public bool IsSnail
    {
      get
      {
        return this.Type == StageObjectType.Snail || this.Type == StageObjectType.SnailKing || this.Type == StageObjectType.EvilSnail;
      }
    }

    public bool UseQuadtree => this.CanCollide;

    public bool CanFall
    {
      get => (this.StaticFlags & StageObjectStaticFlags.CanFall) == StageObjectStaticFlags.CanFall;
    }

    public bool CanCollide
    {
      get
      {
        return (this.StaticFlags & StageObjectStaticFlags.CanCollide) == StageObjectStaticFlags.CanCollide;
      }
    }

    public bool CanHoover
    {
      get
      {
        return (this.StaticFlags & StageObjectStaticFlags.CanHoover) == StageObjectStaticFlags.CanHoover;
      }
    }

    public bool CanWalk
    {
      get => (this.StaticFlags & StageObjectStaticFlags.CanWalk) == StageObjectStaticFlags.CanWalk;
    }

    public bool CanWalkOnWalls
    {
      get
      {
        return (this.StaticFlags & StageObjectStaticFlags.CanWalkOnWalls) == StageObjectStaticFlags.CanWalkOnWalls;
      }
    }

    public bool CanDieWithExplosions
    {
      get
      {
        return (this.StaticFlags & StageObjectStaticFlags.CanDieWithExplosions) == StageObjectStaticFlags.CanDieWithExplosions;
      }
    }

    public bool CanDie
    {
      get => (this.StaticFlags & StageObjectStaticFlags.CanDie) == StageObjectStaticFlags.CanDie;
    }

    public bool CanDieWithFire
    {
      get
      {
        return (this.StaticFlags & StageObjectStaticFlags.CanDieWithFire) == StageObjectStaticFlags.CanDieWithFire;
      }
    }

    public bool CanDieWithAnyTypeOfExplosion
    {
      get
      {
        return (this.StaticFlags & StageObjectStaticFlags.CanDieWithAnyTypeOfExplosion) == StageObjectStaticFlags.CanDieWithAnyTypeOfExplosion;
      }
    }

    public bool CanDieWithCrates
    {
      get
      {
        return (this.StaticFlags & StageObjectStaticFlags.CanDieWithCrates) == StageObjectStaticFlags.CanDieWithCrates;
      }
    }

    public bool IgnoreLiquidCollisions
    {
      get
      {
        return (this.StaticFlags & StageObjectStaticFlags.IgnoreLiquidCollisions) == StageObjectStaticFlags.IgnoreLiquidCollisions;
      }
    }

    public bool IsVisible
    {
      get
      {
        return (this.DynamicFlags & StageObjectDynamicFlags.IsVisible) == StageObjectDynamicFlags.IsVisible;
      }
    }

    public bool IsVitaminized
    {
      get
      {
        return (this.DynamicFlags & StageObjectDynamicFlags.IsVitaminized) == StageObjectDynamicFlags.IsVitaminized;
      }
    }

    public bool IsFalling
    {
      get
      {
        return (this.DynamicFlags & StageObjectDynamicFlags.IsFalling) == StageObjectDynamicFlags.IsFalling;
      }
    }

    public bool IsDead
    {
      get => (this.DynamicFlags & StageObjectDynamicFlags.IsDead) == StageObjectDynamicFlags.IsDead;
    }

    public bool IsDisposed
    {
      get
      {
        return (this.DynamicFlags & StageObjectDynamicFlags.IsDisposed) == StageObjectDynamicFlags.IsDisposed;
      }
    }

    public bool IsUnderLiquid
    {
      get
      {
        return (this.DynamicFlags & StageObjectDynamicFlags.IsUnderLiquid) == StageObjectDynamicFlags.IsUnderLiquid;
      }
    }

    public bool IsUnderWater => this._inLiquidRef is Water;

    public bool IgnorePathCollisions { get; protected set; }

    public override BoundingSquare SoundEmmiterBoundingBox => this.QuadtreeCollisionBB;

    public StageObject()
    {
      this.StaticFlags = StageObjectStaticFlags.None;
      this.DynamicFlags = StageObjectDynamicFlags.IsVisible;
      this.SpriteDrawOffset = new Vector2();
      this.BlendColor = this.DEFAULT_COLOR;
      this._shadowColor = Colors.IngameShadows;
      this._crateCollisionBBIdx = -1;
      this._autoDisposeIfDeadOnAnimationEnd = true;
      this._contentManagerId = "__TEMPORARY__";
    }

    public StageObject(StageObjectType type)
      : this()
    {
      this._type = type;
      this._linkedObjects = new List<StageObject>();
      this._canAnimate = true;
    }

    public StageObject(StageObject other)
      : base((Object2D) other)
    {
      this.Copy(other);
    }

    public virtual void Copy(StageObject other)
    {
      this.Copy((Object2D) other);
      this._type = other._type;
      this.StaticFlags = other.StaticFlags;
      this.DynamicFlags = other.DynamicFlags;
      this.DrawInForeground = other.DrawInForeground;
      this.IgnorePathCollisions = other.IgnorePathCollisions;
      this._canAnimate = other._canAnimate;
      this._faddingOut = other._faddingOut;
      this.BlendColor = other.BlendColor;
      this._crateCollisionBBIdx = other._crateCollisionBBIdx;
      this._deathFireSpriteRes = other._deathFireSpriteRes;
      this._projectWhenKilledByCrate = other._projectWhenKilledByCrate;
      this._contentManagerId = other._contentManagerId;
      this._linkedObjects = new List<StageObject>();
      foreach (StageObject linkedObject in this._linkedObjects)
        this._linkedObjects.Add(new StageObject(linkedObject));
    }

    public virtual StageObject Clone()
    {
      StageObject stageObject = StageObjectFactory.Create(this.Type);
      stageObject.Copy(this);
      return stageObject;
    }

    public virtual void LoadContent()
    {
      if (this.SpriteId != null && this.ResourceId != null)
        this.Sprite = BrainGame.ResourceManager.GetSprite(this.ResourceId + "/" + this.SpriteId, this._contentManagerId);
      if (this.CanDieWithFire)
        this._deathFireSprite = BrainGame.ResourceManager.GetSpriteTemporary(this._deathFireSpriteRes);
      this._killedByFire = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/object_burn", (Object2D) this);
    }

    public virtual void Initialize() => this._faddingOut = false;

    public virtual void StageStartupPhaseEnded()
    {
    }

    public void UpdateCrateCollisionBoundingBox()
    {
      if (this._crateCollisionBBIdx == -1)
        return;
      this._crateCollisionBB = this.TransformSpriteFrameBB(this._crateCollisionBBIdx).ToBoundingSquare();
    }

    public virtual void StageInitialize()
    {
    }

    public virtual void AfterBoardInitialize()
    {
    }

    public void RepositionObjectInQuadtree()
    {
      if (this.Quadtree == null)
        return;
      Stage.CurrentStage.Board.RepositionObjectInQuadtree(this);
    }

    public virtual void Hide()
    {
      this.DynamicFlags &= ~StageObjectDynamicFlags.IsVisible;
      this.SpriteAnimationActive = false;
    }

    public virtual void Show()
    {
      this.DynamicFlags |= StageObjectDynamicFlags.IsVisible;
      this.SpriteAnimationActive = true;
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      if (!this.IsVisible)
        return;
      if (this._faddingOut)
      {
        this._fadeAlpha -= this._fadeSpeed * (float) gameTime.ElapsedGameTime.TotalMilliseconds;
        if ((double) this._fadeAlpha < 0.0)
        {
          this.DisposeFromStage();
          return;
        }
        this.BlendColor = new Color(this._fadeAlpha, this._fadeAlpha, this._fadeAlpha, this._fadeAlpha);
      }
      if (this.Killed)
      {
        this.DynamicFlags = StageObjectDynamicFlags.IsVisible | StageObjectDynamicFlags.IsDead;
        this.EffectsBlender.Clear();
        this.EffectsBlender.Add((ITransformEffect) new GravityEffect(Game1.GameSettings.Gravity, 0.0f));
        this.Killed = false;
      }
      else if (this.Quadtree != null)
      {
        if (this.Quadtree.IsObjectInBounds((IQuadtreeContainable) this))
          return;
        if (this is Snail && !this.IsDead)
          ++Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByOutOfStage;
        Stage.CurrentStage.RemoveObject(this);
      }
      else
      {
        if (Stage.CurrentStage.Board.IsObjectInBounds(this) || (double) this.AABoundingBox.Top <= (double) Stage.CurrentStage.Board.BoundingBox.Bottom)
          return;
        if (this is Snail && !this.IsDead && !((Snail) this).IsEvilSnail)
          ++Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByOutOfStage;
        Stage.CurrentStage.RemoveObject(this);
      }
    }

    public virtual void OnStageRemoved()
    {
    }

    public virtual void AfterUpdate(BrainGameTime gameTime)
    {
      if (!this.IsDead)
      {
        if (this.BoundingBoxChanged && !this.IsDisposed)
          this.RepositionObjectInQuadtree();
        if (!this.IsUnderLiquid || this.CheckCollisionWithLiquid(this._inLiquidRef))
          return;
        this.OnExitLiquid();
      }
      else
      {
        if (this.Quadtree == null)
          return;
        this.Quadtree.RemoveObject((IQuadtreeContainable) this);
      }
    }

    public virtual void OnCollide(IQuadtreeContainable obj, int listIdx)
    {
    }

    public override void OnLastFrame()
    {
      if (!this.IsDead || !this._autoDisposeIfDeadOnAnimationEnd)
        return;
      this.DisposeFromStage();
    }

    public virtual void Kill()
    {
      if (!this.CanDie)
        return;
      this.Killed = true;
      if (!this.IsSnail)
        return;
      this.MoveToForeground();
    }

    public virtual void KillByTouchingDeadlyLiquid() => this.KillByFire();

    public virtual void KillByExplosion(Explosion exp) => this.KillMe();

    public virtual void KillByFire()
    {
      this._killedByFire.Play();
      this.SpriteAnimationActive = true;
      this.Sprite = this._deathFireSprite;
      this.CurrentFrame = 0;
      this.UpdateBoundingBox();
      this.KillMe();
    }

    public virtual void KillMe()
    {
      if (!this.CanDie)
        return;
      this.StaticFlags = StageObjectStaticFlags.None;
      this.DynamicFlags = StageObjectDynamicFlags.IsVisible | StageObjectDynamicFlags.IsDead;
      this.EffectsBlender.DisableAll();
      this.MoveToForeground();
    }

    public Explosion Explode(
      Explosion.ExplosionSize tileKillBBsize,
      Explosion.ExplosionSize objKillBBSize,
      Explosion.ExplosionRadiusType radiusType,
      Explosion.ObjectTypeAffected objectsAffected,
      Vector2 explosionCenter,
      bool destroyUnbreakbleTiles,
      Sprite explosionSprite,
      Sprite explosionEndSprite,
      bool dispose,
      bool isUnderLiquid)
    {
      Explosion explosion = (Explosion) Stage.CurrentStage.StageData.GetObject("EXPLOSION");
      explosion.Position = explosionCenter;
      explosion.TileKillBBSize = tileKillBBsize;
      explosion.ObjectKillBBSize = objKillBBSize;
      explosion.RadiusType = radiusType;
      explosion.AffectedObjects = objectsAffected;
      explosion.DestroyUnbreakbleTiles = destroyUnbreakbleTiles;
      if (explosionEndSprite != null)
        explosion.SmokeSprite = explosionEndSprite;
      explosion.ThrowParticles = !this.IsUnderLiquid;
      explosion.SetUnderLiquid(isUnderLiquid);
      explosion.ExplosionSource = this;
      if (explosionSprite != null)
        explosion.Sprite = explosionSprite;
      Stage.CurrentStage.AddObjectInRuntime((StageObject) explosion);
      if (dispose)
        Stage.CurrentStage.DisposeObject(this);
      return explosion;
    }

    protected void MoveToForeground()
    {
      this.DrawInForeground = true;
      Stage.CurrentStage.BackgroundObjectsDrawList.Remove(this);
      Stage.CurrentStage.ForegroundObjectsDrawList.Add(this);
    }

    protected void MoveToBackground()
    {
      this.DrawInForeground = false;
      Stage.CurrentStage.BackgroundObjectsDrawList.Add(this);
      Stage.CurrentStage.ForegroundObjectsDrawList.Remove(this);
    }

    public void SnapIt()
    {
      this.BoardX = this.BoardX;
      this.BoardY = this.BoardY;
    }

    public int BoardX
    {
      get => (int) ((double) this.X / (double) Stage.CurrentStage.Board.TileWidth);
      set => this.X = (float) (value * Stage.CurrentStage.Board.TileWidth);
    }

    public int BoardY
    {
      get => (int) ((double) this.Y / (double) Stage.CurrentStage.Board.TileHeight);
      set => this.Y = (float) (value * Stage.CurrentStage.Board.TileHeight);
    }

    public BoundingSquare GetCurrentFrameRectTransformed()
    {
      Rectangle rect = this.Sprite.Frames[this.CurrentFrame].Rect;
      return new BoundingSquare(new Rectangle((int) ((double) this.Position.X - (double) this.Sprite.Offset.X), (int) ((double) this.Position.Y - (double) this.Sprite.Offset.Y), rect.Width, rect.Height));
    }

    public string BuildLinkString()
    {
      string str = "";
      for (int index = 0; index < this.LinkedObjects.Count; ++index)
      {
        str += this.LinkedObjects[index].UniqueId;
        if (index + 1 != this.LinkedObjects.Count)
          str += ";";
      }
      return str;
    }

    public void DrawParentChild(bool shadow, Sprite parentSprite, Sprite childSprite)
    {
      int index = this.CurrentFrame;
      if (index >= childSprite.Frames.Length)
        index = childSprite.Frames.Length - 1;
      if ((this.SpriteEffect & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally)
      {
        Frame frame1 = childSprite.Frames[index];
        Frame frame2 = parentSprite.Frames[this.CurrentFrame];
        float rotation = this._rotationInRad + (6.28318548f - frame1._rotationInRads);
        Vector2 position = this.Position + Vector2.Transform(new Vector2((float) parentSprite.Frames[this.CurrentFrame].Rect.Width - frame1._offset.X, frame1._offset.Y) - new Vector2((float) frame2.Rect.Width - parentSprite.Offset.X, parentSprite.Offset.Y), this._rotationZMatrix);
        position = new Vector2((float) (int) position.X, (float) (int) position.Y);
        Color opacity = this.DEFAULT_COLOR;
        if (shadow)
        {
          position += GenericConsts.ShadowDepth;
          opacity = this.ShadowColor;
        }
        this.Sprite.Draw(position, frame1.Rect, rotation, this.SpriteEffect, frame1._pivotHorizFlipped, opacity, Stage.CurrentStage.SpriteBatch);
      }
      else
      {
        Frame frame = childSprite.Frames[index];
        float rotation = this._rotationInRad + frame._rotationInRads;
        Vector2 position = this.Position + Vector2.Transform(frame._offset - parentSprite.Offset, this._rotationZMatrix);
        position = new Vector2((float) (int) position.X, (float) (int) position.Y);
        Color opacity = this.DEFAULT_COLOR;
        if (shadow)
        {
          position += GenericConsts.ShadowDepth;
          opacity = this.ShadowColor;
        }
        this.Sprite.Draw(position, frame.Rect, rotation, this.SpriteEffect, frame._pivot, opacity, Stage.CurrentStage.SpriteBatch);
      }
    }

    public virtual void Draw(bool shadow)
    {
      if (!this.IsVisible)
        return;
      if (!shadow)
        this.Draw(Stage.CurrentStage.SpriteBatch, this.Position, this.BlendColor);
      else
        this.Draw(Stage.CurrentStage.SpriteBatch, this._position + GenericConsts.ShadowDepth, this.ShadowColor);
    }

    public virtual void ForegroundDraw()
    {
    }

    public virtual void DisposeFromStage() => Stage.CurrentStage.DisposeObject(this);

    public virtual void OnExplosion(Explosion explosion)
    {
    }

    public virtual void OnAddedToStage()
    {
    }

    public virtual void StopSamples()
    {
    }

    public void FadeOut(float speed)
    {
      this._faddingOut = true;
      this._fadeSpeed = speed / 1000f;
      this.BlendColor = this.DEFAULT_COLOR;
      this._fadeAlpha = 1f;
    }

    public virtual void OnEnterLiquid(Liquid liquid)
    {
      this.DynamicFlags |= StageObjectDynamicFlags.IsUnderLiquid;
      this._inLiquidRef = liquid;
    }

    public virtual void OnExitLiquid()
    {
      this.DynamicFlags &= ~StageObjectDynamicFlags.IsUnderLiquid;
      this._inLiquidRef = (Liquid) null;
    }

    public bool LineCollides(Vector2 p0, Vector2 p1)
    {
      return this.AABoundingBox.IntersectsLine(p0, p1, out Vector2 _);
    }

    public bool CanAcceptCursorInteraction => Stage.CurrentStage._state == Stage.StageState.Playing;

    public bool Collides(StageObject obj) => this.AABoundingBox.Collides(obj.AABoundingBox);

    public bool Collides(Tile tile, Vector2 pos)
    {
      return this.Sprite.FlipCollisionBox(this.SpriteEffect).Transform(0.0f, pos + this.Sprite.Offset).Transform(-this.Position, 0.0f).Collides(this.Sprite.BoundingBox) || this.BoundingBox.Transform(-pos, 0.0f).Collides(tile.Sprite.BoundingBox);
    }

    public virtual void OnCollide(StageObject obj)
    {
    }

    public bool WithLinks => this.LinkedObjects.Count > 0;

    public string LinkString
    {
      get => this._linkString;
      set => this._linkString = value;
    }

    public List<StageObject> LinkedObjects => this._linkedObjects;

    public virtual void AddLinkedObject(StageObject obj) => this._linkedObjects.Add(obj);

    public virtual void SetLinkedObjects(List<StageObject> objList)
    {
      this._linkedObjects = objList;
    }

    public void RemoveLinkedObject(StageObject obj) => this._linkedObjects.Remove(obj);

    public void ClearLinkedObjects() => this._linkedObjects.Clear();

    public bool IsLinkedTo(StageObject obj) => this._linkedObjects.Contains(obj);

    public void LinkTo(StageObject obj)
    {
      this.ClearLinkedObjects();
      if (obj == null)
        return;
      this.AddLinkedObject(obj);
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      string fieldValue1 = record.GetFieldValue<string>("type", (string) null);
      if (!string.IsNullOrEmpty(fieldValue1))
        this._type = (StageObjectType) Enum.Parse(typeof (StageObjectType), fieldValue1, true);
      this.UniqueId = record.GetFieldValue<string>("uid", this.UniqueId);
      this._preLoad = record.GetFieldValue<bool>("preLoad", false);
      this.IsStageObject = record.GetFieldValue<bool>("stageObject", this.IsStageObject);
      this._linkString = record.GetFieldValue<string>("linkTo", this._linkString);
      this.DrawInForeground = record.GetFieldValue<bool>("drawInForeground", this.DrawInForeground);
      this.IgnorePathCollisions = record.GetFieldValue<bool>("ignorePathCollisions", this.IgnorePathCollisions);
      this._crateCollisionBBIdx = record.GetFieldValue<int>("crateCollisionBBIdx", this._crateCollisionBBIdx);
      FlagsType fieldValue2 = record.GetFieldValue<FlagsType>("staticFlags", FlagsType.Zero);
      if (fieldValue2.Value != 0)
        this.StaticFlags = (StageObjectStaticFlags) Enum.Parse(typeof (StageObjectStaticFlags), fieldValue2.ToString(), true);
      fieldValue2 = record.GetFieldValue<FlagsType>("dynamicFlags", FlagsType.Zero);
      if (fieldValue2.Value != 0)
        this.DynamicFlags = (StageObjectDynamicFlags) Enum.Parse(typeof (StageObjectDynamicFlags), fieldValue2.ToString(), true);
      this.Position = new Vector2(record.GetFieldValue<float>("posX", 0.0f), record.GetFieldValue<float>("posY", 0.0f));
      this.BlendColor = record.GetFieldValue<Color>("color", this.BlendColor);
      this._deathFireSpriteRes = record.GetFieldValue<string>("deathFireSpriteRes", "spriteset/stage-objects/FireDeath");
      this._projectWhenKilledByCrate = record.GetFieldValue<bool>("projectWhenKilledByCrate", true);
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public virtual DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord();
      switch (context)
      {
        case ToDataFileRecordContext.StageDataSave:
          dataFileRecord.AddField("canAnimate", (object) this._canAnimate);
          dataFileRecord.AddField("drawInForeground", (object) this.DrawInForeground);
          dataFileRecord.AddField("staticFlags", (object) (int) this.StaticFlags);
          dataFileRecord.AddField("dynamicFlags", (object) (int) this.DynamicFlags);
          dataFileRecord.AddField("stageObject", (object) this.IsStageObject);
          dataFileRecord.AddField("ignorePathCollisions", (object) this.IgnorePathCollisions);
          dataFileRecord.AddField("crateCollisionBBIdx", (object) this._crateCollisionBBIdx);
          dataFileRecord.AddField("deathFireSpriteRes", (object) this._deathFireSpriteRes);
          dataFileRecord.AddField("projectWhenKilledByCrate", (object) this._projectWhenKilledByCrate);
          dataFileRecord.AddField("preLoad", (object) this._preLoad);
          break;
        case ToDataFileRecordContext.StageSave:
          dataFileRecord.RemoveField("res");
          dataFileRecord.RemoveField("sprite");
          dataFileRecord.RemoveField("frame");
          break;
      }
      dataFileRecord.AddField("type", (object) (int) this._type);
      dataFileRecord.AddField("uid", (object) this.UniqueId);
      this._linkString = this.BuildLinkString();
      dataFileRecord.AddField("linkTo", (object) this._linkString);
      dataFileRecord.AddField("posX", (object) this.Position.X);
      dataFileRecord.AddField("posY", (object) this.Position.Y);
      if (this.BlendColor != this.DEFAULT_COLOR)
        dataFileRecord.AddField("color", (object) this.BlendColor);
      return dataFileRecord;
    }

    public Quadtree Quadtree { get; set; }

    public List<QuadtreeNode> QuadtreeNodes { get; set; }

    public int ObjectListIdx { get; set; }

    public bool ShouldTestCollisions => !this.IsDead && this.IsVisible && this.CanCollide;

    public bool IsContained(QuadtreeNode node)
    {
      return node.BoundingBox.Contains(this.QuadtreeCollisionBB);
    }

    public bool Collides(IQuadtreeContainable obj)
    {
      BoundingSquare boundingSquare = this.QuadtreeCollisionBB;
      if (this.Sprite.Frames[this.CurrentFrame].WithCollisionBox)
        boundingSquare = this.TransformBoundingBox(this.Sprite.Frames[this.CurrentFrame].BoundingBox).ToBoundingSquare();
      return obj.Collides(boundingSquare);
    }

    public bool Collides(BoundingSquare bb)
    {
      BoundingSquare boundingSquare = this.QuadtreeCollisionBB;
      if (this.Sprite.Frames[this.CurrentFrame].WithCollisionBox)
        boundingSquare = this.TransformBoundingBox(this.Sprite.Frames[this.CurrentFrame].BoundingBox).ToBoundingSquare();
      return boundingSquare.Collides(bb);
    }

    public virtual bool CheckCollisionWithLiquid(Liquid liquid)
    {
      return this.Collides(liquid.QuadtreeCollisionBB);
    }

    public void DoQuadtreeCollisions(int listIdx)
    {
      if (this.Quadtree == null)
        return;
      this.Quadtree.DoCollisions((IQuadtreeContainable) this, listIdx);
    }

    public bool Contains(Vector2 p) => this.QuadtreeCollisionBB.Contains(p);

    public bool Collides(Vector2 p0, Vector2 p1)
    {
      return this.QuadtreeCollisionBB.IntersectsLine(p0, p1, out Vector2 _);
    }

    public virtual bool CrateToolIsValid(BoundingSquare crateBs)
    {
      return this._crateCollisionBBIdx == -1 || !crateBs.Collides(this._crateCollisionBB);
    }

    public string FormatStringToDumpFile(QuadtreeNode currentNode)
    {
      return string.Format("   obj [{0}] [{1}] pos[{2}  {3}] prev_pos[{4}  {5}] [Contained in the node: {6}] bb[{7} {8} {9} {10} , w: {11} h: {12}] se[{13}]", (object) this.UniqueId, (object) this.GetType().Name, (object) this.Position.X, (object) this.Position.Y, (object) this.PreviousPosition.X, (object) this.PreviousPosition.Y, (object) this.Collides(currentNode.BoundingBox), (object) this.QuadtreeCollisionBB.UpperLeft.X, (object) this.QuadtreeCollisionBB.UpperLeft.Y, (object) this.QuadtreeCollisionBB.LowerRight.X, (object) this.QuadtreeCollisionBB.LowerRight.Y, (object) this.QuadtreeCollisionBB.Width, (object) this.QuadtreeCollisionBB.Height, (object) this.SpriteEffect.ToString());
    }
  }
}
