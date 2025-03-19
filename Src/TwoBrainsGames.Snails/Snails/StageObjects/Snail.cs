
// Type: TwoBrainsGames.Snails.StageObjects.Snail
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
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
using TwoBrainsGames.Snails.StageObjects.SpriteAccessories;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class Snail : MovingObject
  {
    public const string ID = "SNAIL";
    public const string KING_ID = "SNAIL_KING";
    public const float SNAIL_LAYER_DEPTH = 0.9f;
    public const float WALKING_SPEED = 0.03f;
    public const float WALKING_SPEED_VITAMINIZED = 0.06f;
    public const float WALKING_SPEED_UNDERWATER = 0.5f;
    public const string SPRITE_SNAIL_WALK = "SnailWalk";
    public const string SPRITE_SNAIL_TURN_UP = "SnailTurnUp";
    public const string SPRITE_SNAIL_TURN_DOWN = "SnailTurnDown";
    protected const string SPRITE_SNAIL_ENTERING_STAGE_EXIT = "ExitingStage";
    public const string SPRITE_SNAIL_HIDDING_IN_SHELL = "HiddingInShell";
    public const string SPRITE_SNAIL_EMPALED = "EmpaledSnail";
    public const string SPRITE_SNAIL_DEAD_BROKEN_SHELL = "DeadSnailBrokenShell";
    public const string SPRITE_DEATH_STAGE_EXIT = "DeathWithStageExit";
    public const string SPRITE_SNAIL_BYTING = "Byte";
    public const string SPRITE_SNAIL_CHEWING = "Chew";
    public const string SPRITE_SNAIL_BREATHING_BUBBLES = "Bubbles";
    public const int EATING_APPLE_TIME = 1700;
    public const int UNDER_WATER_TIME = 40000;
    public const int UNDER_WATER_BREATHING_MAX_TIME = 3000;
    public const float FLOAT_SPEED = 0.15f;
    public const int SHELL_BB_INDEX = 0;
    public const int HEAD_BB_INDEX = 1;
    public const int TRAMPOLIM_BB_INDEX = 2;
    public const int VITAMIN_BB_INDEX = 0;
    public const int SNAIL_HIDDING_SCARE_TIME = 2000;
    public TransformBlender _effectsBlender;
    private double _eatingElapsedGameTime;
    private Sprite _EnteringStageExitSprite;
    public Sprite SpriteHiddingInShell;
    public Sprite SpriteChewing;
    public Sprite SpriteByting;
    public Sprite DeathWithStageExitSprite;
    public Sprite SpriteEmpaled;
    public Sprite SpriteDeadBrokenShell;
    private RocketAccessory _rocketAccessory;
    private HelmetAccessory _helmetAccessory;
    private List<SnailSpriteAccessory> _spriteAcessories;
    private double _genericTimer;
    public Snail.SnailStatus _snailStatus;
    private Sample[] _soundsGenericSnailDeath;
    private Sample _snailEnteringExit;
    private Sample _snailBreathUnderwaterSound;
    protected double _inWaterElapsedGameTime;
    private SpriteAnimation _bubblesAnim;
    private double _showBubblesEllapsedTime;
    private bool _allowRocketPickUp;
    private bool _allowAffectedBySalt;
    private bool _allowEatApple;
    private bool _allowJumpOnTrampoline;
    private bool _allowObjectPickup;
    private bool _allowKilledByEvilSnail;
    private bool _allowSwitchActivation;
    private bool _allowEnterStageExit;
    private Apple _apple;
    private Vitamin _vitamin;
    public bool _deathAccounted;
    public bool _inactiveAccounted;

    public bool AllowStageStatistics { get; set; }

    public bool IsEating
    {
      get
      {
        return (this.DynamicFlags & StageObjectDynamicFlags.IsEating) == StageObjectDynamicFlags.IsEating;
      }
    }

    protected bool IsHidding
    {
      get
      {
        return this._snailStatus == Snail.SnailStatus.Hibernate || this._snailStatus == Snail.SnailStatus.Hidding;
      }
    }

    public bool CanBeSuckedBySwitch
    {
      get
      {
        return this._allowSwitchActivation && this._snailStatus == Snail.SnailStatus.Normal && this.IsVisible && this.IsAttachedToPath;
      }
    }

    public bool CanActivateSwitch
    {
      get
      {
        return this._allowSwitchActivation && this._snailStatus == Snail.SnailStatus.Normal && this.IsVisible && this.IsAttachedToPath;
      }
    }

    public bool CanPickupObject => this._allowObjectPickup;

    public bool CanEatApple
    {
      get
      {
        return this._allowEatApple && this._snailStatus == Snail.SnailStatus.Normal && !this.IsEating && this.IsVisible && !this.IsUnderLiquid && this.IsAttachedToPath && this.PathNode.Value.WallType == PathSegment.SegmentType.Floor;
      }
    }

    public bool CanBeAffectedBySalt
    {
      get
      {
        return this._allowAffectedBySalt && this._snailStatus != Snail.SnailStatus.Hidding && this.IsVisible && this.IsAttachedToPath && !this.IsUnderLiquid;
      }
    }

    public virtual bool CanEatVitamins
    {
      get
      {
        return this._allowRocketPickUp && this._snailStatus != Snail.SnailStatus.Byting && this._snailStatus != Snail.SnailStatus.Chewing && this._snailStatus != Snail.SnailStatus.ExitingStage && this.IsVisible && !this.IsVitaminized;
      }
    }

    public bool CanJumpTranpoline
    {
      get
      {
        if (!this._allowJumpOnTrampoline || !this.IsAttachedtoFloor() || !this.IsVisible || this.IsUnderLiquid)
          return false;
        return this.State == MovingObject.MovingState.Walking || this.State == MovingObject.MovingState.Stopped;
      }
    }

    public bool CanEnterStage
    {
      get
      {
        return this._allowEnterStageExit && this.IsAttachedToPath && this.State == MovingObject.MovingState.Walking && this.IsVisible && this._snailStatus != Snail.SnailStatus.ExitingStage;
      }
    }

    public bool CanBeKilledByEvilSnail
    {
      get => this._allowKilledByEvilSnail && !this.IsDead && this.IsVisible;
    }

    public override Sprite Sprite
    {
      get => base.Sprite;
      set
      {
        if (base.Sprite == value)
          return;
        base.Sprite = value;
        this.SpriteChanged();
      }
    }

    public bool IsSnailKing => this.Type == StageObjectType.SnailKing;

    public bool CanBeAffectedByWater => this.IsVisible && this.IsAttachedToPath && !this.IsDead;

    public bool IsEvilSnail => this is EvilSnail;

    public Snail()
      : this(StageObjectType.Snail)
    {
    }

    protected Snail(StageObjectType type)
      : base(type)
    {
      this._effectsBlender = new TransformBlender();
      this.Speed = 0.03f;
      this._spriteAcessories = new List<SnailSpriteAccessory>();
    }

    public Snail(StageObject other)
      : base(other)
    {
      this.Copy(other);
    }

    public override void Copy(StageObject other)
    {
      base.Copy(other);
      Snail snail = (Snail) other;
      this._EnteringStageExitSprite = snail._EnteringStageExitSprite;
      this.SpriteHiddingInShell = snail.SpriteHiddingInShell;
      this.SpriteByting = snail.SpriteByting;
      this.SpriteChewing = snail.SpriteChewing;
      this.DeathWithStageExitSprite = snail.DeathWithStageExitSprite;
      this.SpriteEmpaled = snail.SpriteEmpaled;
      this.SpriteDeadBrokenShell = snail.SpriteDeadBrokenShell;
      this._allowRocketPickUp = snail._allowRocketPickUp;
      this._allowEatApple = snail._allowEatApple;
      this._allowAffectedBySalt = snail._allowAffectedBySalt;
      this._allowJumpOnTrampoline = snail._allowJumpOnTrampoline;
      this._allowObjectPickup = snail._allowObjectPickup;
      this.AllowStageStatistics = snail.AllowStageStatistics;
      this._allowKilledByEvilSnail = snail._allowKilledByEvilSnail;
      this._allowSwitchActivation = snail._allowSwitchActivation;
      this._allowEnterStageExit = snail._allowEnterStageExit;
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this.WalkSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "SnailWalk");
      this.InnerTurnSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "SnailTurnUp");
      this.OuterTurnSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "SnailTurnDown");
      this._EnteringStageExitSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "ExitingStage");
      this.SpriteHiddingInShell = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "HiddingInShell");
      this.SpriteByting = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "Byte");
      this.SpriteChewing = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "Chew");
      this.DeathWithStageExitSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "DeathWithStageExit");
      this.SpriteEmpaled = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "EmpaledSnail");
      this.SpriteDeadBrokenShell = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "DeadSnailBrokenShell");
      this._bubblesAnim = new SpriteAnimation(BrainGame.ResourceManager.GetSpriteTemporary("spriteset/anim-snails", "Bubbles"));
      this._bubblesAnim.Autohide = true;
      this.AddAccessory((SnailSpriteAccessory) (this._rocketAccessory = new RocketAccessory(this)));
      this.AddAccessory((SnailSpriteAccessory) (this._helmetAccessory = new HelmetAccessory(this)));
      this._killedByFire = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/snail_burn", (Object2D) this);
      this._soundsGenericSnailDeath = new Sample[4];
      this._soundsGenericSnailDeath[0] = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/snail_death_1", (Object2D) this);
      this._soundsGenericSnailDeath[1] = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/snail_death_2", (Object2D) this);
      this._soundsGenericSnailDeath[2] = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/snail_death_3", (Object2D) this);
      this._soundsGenericSnailDeath[3] = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/snail_death_4", (Object2D) this);
      this._snailEnteringExit = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/snail_entering", (Object2D) this);
      this._snailBreathUnderwaterSound = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/snail-breath-underwater", (Object2D) this);
    }

    protected void AddAccessory(SnailSpriteAccessory accessory)
    {
      accessory.LoadContent();
      this._spriteAcessories.Add(accessory);
    }

    public override void OnLastFrame()
    {
      switch (this._snailStatus)
      {
        case Snail.SnailStatus.Hidding:
          break;
        case Snail.SnailStatus.DeathByFire:
          this.Position = new Vector2(this.AABoundingBox.Left + this.AABoundingBox.Width / 2f, this.AABoundingBox.Top + this.AABoundingBox.Height / 2f);
          this.Sprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/stage-objects", "FireDeath");
          this.CurrentFrame = 0;
          this.Rotation = 0.0f;
          this._snailStatus = Snail.SnailStatus.DeathByFireSmoke;
          break;
        case Snail.SnailStatus.DeathBySpikes:
          break;
        case Snail.SnailStatus.DeathWithStageExitEffect:
          break;
        case Snail.SnailStatus.Byting:
          this.SetChewingApple();
          break;
        case Snail.SnailStatus.ExitingStage:
          ++Stage.CurrentStage.Stats.NumSnailsSafe;
          this.DisposeFromStage();
          break;
        case Snail.SnailStatus.DeathByFireSmoke:
          this.DisposeFromStage();
          break;
        default:
          if (this.IsDead)
            break;
          base.OnLastFrame();
          break;
      }
    }

    protected override void OnTileCollision(PathSegment.SegmentType wallType)
    {
      switch (this._snailStatus)
      {
        case Snail.SnailStatus.Normal:
          this.State = MovingObject.MovingState.Walking;
          break;
        case Snail.SnailStatus.Hidding:
        case Snail.SnailStatus.Hibernate:
          this.State = MovingObject.MovingState.Stopped;
          break;
        default:
          this.State = MovingObject.MovingState.Stopped;
          break;
      }
    }

    public override void DoWalk(BrainGameTime gameTime)
    {
      Snail snail = this;
      snail.Position = snail.Position + this.PathNode.Value.Normal * this.Speed * (float) this.Direction * (float) gameTime.ElapsedGameTime.TotalMilliseconds;
      this.UpdateBoundingBox();
      float excessWalk = ((this.Direction == MovingObject.WalkDirection.Clockwise ? this.PathNode.Value.P0 : this.PathNode.Value.P1) - this.HeadPoint).Length() - this.PathNode.Value.Length;
      if ((double) excessWalk <= 0.0)
        return;
      BoardPathNode pathNode = this.PathNode;
      BoardPathNode node = this.Direction == MovingObject.WalkDirection.Clockwise ? this.PathNode.Next : this.PathNode.Previous;
      if (this.CanWalkOnPath(node))
      {
        this.PathNode = node;
        if ((double) this.PathNode.Value.Rotation == (double) this.Rotation)
          return;
        if ((double) pathNode.Value.Classify(this.PathNode.Value) == (double) this.Direction)
          this.SetStateToOuterTurn(excessWalk);
        else
          this.SetStateToInnerTurn(excessWalk);
      }
      else
      {
        this.InvertDirection();
        this.SetTailInEndOfPath();
        this.UpdateBoundingBox();
      }
    }

    public bool CurrentPathReversesWalk()
    {
      return this.PathNode.Value.Behavior == PathSegmentBehavior.ReverseWalk || this.PathNode.Value.Behavior == PathSegmentBehavior.WalkableCCW && this.Direction != MovingObject.WalkDirection.CounterClockwise || this.PathNode.Value.Behavior == PathSegmentBehavior.WalkableCW && this.Direction != MovingObject.WalkDirection.Clockwise;
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      if (this.IsAttachedToPath && this._snailStatus != Snail.SnailStatus.Hibernate && this.CurrentPathReversesWalk())
      {
        if (this.PathNode.Value.Behavior == PathSegmentBehavior.ReverseWalk)
          this.Hibernate();
        else
          this.InvertDirection();
      }
      switch (this._snailStatus)
      {
        case Snail.SnailStatus.Hidding:
          if (!this.IsFalling)
          {
            this._genericTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
            if (this._genericTimer < 0.0)
            {
              this.SetStateToWalk();
              this.UpdateBoundingBox();
              this.RepositionObjectInQuadtree();
              List<IQuadtreeContainable> collidingObjects = this.Quadtree.GetCollidingObjects((IQuadtreeContainable) this, 1);
              bool flag = false;
              foreach (IQuadtreeContainable quadtreeContainable in collidingObjects)
              {
                if (quadtreeContainable is Salt salt && this.CheckCollisionWithHead(salt.AABoundingBox))
                  flag = true;
                if (flag)
                  break;
              }
              if (flag)
                this.InvertDirection();
              this.StaticFlags |= StageObjectStaticFlags.CanWalkOnWalls;
              this._snailStatus = Snail.SnailStatus.Normal;
              this.SetStateToWalk();
              break;
            }
            break;
          }
          break;
        case Snail.SnailStatus.Chewing:
          this._eatingElapsedGameTime += gameTime.ElapsedGameTime.TotalMilliseconds;
          if (this._eatingElapsedGameTime > 1700.0)
          {
            if (this.Collides((StageObject) this._apple) && this._apple.CanBeEaten)
            {
              this.SetBitingApple();
              break;
            }
            this.RemoveEatingApple();
            break;
          }
          break;
        case Snail.SnailStatus.Hibernate:
          if (this.IsAttachedToPath && !this.CurrentPathReversesWalk())
          {
            this.StaticFlags |= StageObjectStaticFlags.CanWalkOnWalls;
            this._snailStatus = Snail.SnailStatus.Normal;
            this.SetStateToWalk();
            break;
          }
          break;
      }
      if (this.IsUnderLiquid && this._snailStatus != Snail.SnailStatus.DeathByWater)
      {
        this._inWaterElapsedGameTime += gameTime.ElapsedGameTime.TotalMilliseconds;
        if (this._inWaterElapsedGameTime > 40000.0)
          this.KillByWater();
      }
      foreach (SnailSpriteAccessory spriteAcessory in this._spriteAcessories)
        spriteAcessory.Update(gameTime);
    }

    public override void AfterUpdate(BrainGameTime gameTime)
    {
      base.AfterUpdate(gameTime);
      if (this.IsDead || this.IsDisposed || !this.IsUnderWater)
        return;
      if (!this._bubblesAnim.Visible)
      {
        if (this._showBubblesEllapsedTime == 0.0)
          this._showBubblesEllapsedTime = (40000.0 - this._inWaterElapsedGameTime) * 3000.0 / 40000.0;
        this._showBubblesEllapsedTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
        if (this._showBubblesEllapsedTime > 0.0 || (double) this._inLiquidRef.GetDepth(this.HeadPoint) <= (double) this._bubblesAnim.Sprite.Frames[0].Height || this._inWaterElapsedGameTime >= 40000.0 - this._bubblesAnim.TotalPlayTime)
          return;
        this._bubblesAnim.Visible = true;
        this._bubblesAnim.Reset();
        this._showBubblesEllapsedTime = 0.0;
        this._bubblesAnim.Position = this.GetHeadBBTramsformed().Center;
        this._snailBreathUnderwaterSound.Play();
      }
      else
        this._bubblesAnim.Update(gameTime);
    }

    public override void OnEnterLiquid(Liquid liquid)
    {
      base.OnEnterLiquid(liquid);
      this.SetUnderWater();
      if (this.IsFalling && !this.IsHidding)
        this.HideInShell(Snail.SnailStatus.Hidding);
      foreach (SnailSpriteAccessory spriteAcessory in this._spriteAcessories)
        spriteAcessory.OnEnterLiquid();
      if (!this.IsVitaminized)
        return;
      this._vitamin.OnSnailEnteredLiquid();
    }

    public override void OnExitLiquid()
    {
      base.OnExitLiquid();
      this.ResetUnderWater();
      foreach (SnailSpriteAccessory spriteAcessory in this._spriteAcessories)
        spriteAcessory.OnExitLiquid();
      if (!this.IsVitaminized)
        return;
      this._vitamin.OnSnailExitedLiquid();
    }

    public void OnEnterStage() => this.ReleaseSlime();

    private void ReleaseSlime()
    {
      Slime slime = (Slime) Stage.CurrentStage.StageData.GetObject("SLIME");
      slime.Position = this.Position;
      slime.Rotation = this.Rotation;
      Stage.CurrentStage.AddObjectInRuntime((StageObject) slime);
    }

    public override void OnStageRemoved() => this.RemoveVitaminIfNeeded();

    private void RemoveVitaminIfNeeded()
    {
      if (this._vitamin == null)
        return;
      this._vitamin.SnailRemoved();
      this._vitamin = (Vitamin) null;
    }

    public override void Draw(bool shadow)
    {
      base.Draw(shadow);
      foreach (SnailSpriteAccessory spriteAcessory in this._spriteAcessories)
        spriteAcessory.Draw(shadow, Stage.CurrentStage.SpriteBatch);
      if (!this._bubblesAnim.Visible)
        return;
      this._bubblesAnim.Draw(this._bubblesAnim.Position, 0.0f, this.SpriteEffect, Stage.CurrentStage.SpriteBatch);
    }

    private void OnKill() => this.RemoveVitaminIfNeeded();

    public override void KillWithProjection()
    {
      this.PlayRandomDeathSound();
      this.KillMe();
      this.Rotation = 0.0f;
      float degrees = (float) (BrainGame.Rand.Next(90) - 135);
      float y = (float) Math.Sin((double) MathHelper.ToRadians(degrees));
      this.DeathWithProjection(new Vector2((float) Math.Cos((double) MathHelper.ToRadians(degrees)), y), 30f);
    }

    private void KillWithFloatingShell()
    {
      this.AddFloatingShell();
      this.OnKill();
    }

    public override void KillByCrate()
    {
      if (!this.IsEvilSnail)
        ++Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByCrate;
      if (!this.IsUnderLiquid)
        this.KillWithProjection();
      else
        this.KillWithFloatingShell();
      this.OnKill();
    }

    public override void KillByTouchingDeadlyLiquid()
    {
      if (!this.IsEvilSnail)
        ++Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByAcid;
      this.KillWithProjection();
      this.OnKill();
    }

    public void KillByEvilSnail()
    {
      if (!this.IsEvilSnail)
        ++Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByEvilSnail;
      this.KillWithProjection();
      this.OnKill();
    }

    public void KillByLaser()
    {
      if (!this.IsEvilSnail)
        ++Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByLaser;
      this.KillWithProjection();
      this.OnKill();
    }

    public void KillBySpikes(Spikes spikes)
    {
      this.PlayRandomDeathSound();
      if (!this.IsEvilSnail)
        ++Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadBySpikes;
      if (this.IsFalling)
        BrainGame.AchievementsManager.Notify(19);
      this.KillMe();
      this.Sprite = this.SpriteEmpaled;
      this.CurrentFrame = 0;
      this._SpritePlaybackMode = Object2D.AnimtionPlaybackModes.PlayOnce;
      this._snailStatus = Snail.SnailStatus.DeathBySpikes;
      this.UpdateBoundingBox();
      if ((double) this._rotation != 0.0 || (double) spikes.Rotation != 180.0)
        this._rotation = spikes.Rotation;
      this.ProjectAccessories(true);
      this.OnKill();
      if (this.AllowStageStatistics && !this._deathAccounted)
      {
        ++Stage.CurrentStage.Stats.NumSnailsDisposed;
        this._deathAccounted = true;
      }
      if (!this.AllowStageStatistics || this._inactiveAccounted)
        return;
      --Stage.CurrentStage.Stats.NumSnailsActive;
      this._inactiveAccounted = true;
    }

    public override void KillByExplosion(Explosion exp)
    {
      if (!this.IsEvilSnail)
      {
        if (exp.ExplosionSource is Box)
          ++Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByCrateExplosion;
        if (exp.ExplosionSource is Dynamite)
          ++Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByDynamite;
      }
      this.KillWithProjection();
      this.OnKill();
    }

    public override void KillByFire()
    {
      if (!this.IsEvilSnail)
        ++Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByFire;
      base.KillByFire();
      this._snailStatus = Snail.SnailStatus.DeathByFire;
      this.ProjectAccessories(true);
      this.OnKill();
    }

    public void KillBySacrificeSwitch()
    {
      if (!this.IsEvilSnail)
        ++Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadBySacrifice;
      this.Hide();
      this.ProjectAccessories(true);
      this.OnKill();
    }

    public void KillByWater()
    {
      if (!this.IsEvilSnail)
        ++Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByWater;
      this.Kill();
      this.AddFloatingShell();
      this.OnKill();
    }

    public void AddFloatingShell()
    {
      SnailShell snailShell = (SnailShell) Stage.CurrentStage.StageData.GetObject("SNAIL_SHELL");
      snailShell.Position = this.Position;
      snailShell.Rotation = this.Rotation;
      snailShell.Direction = this.Direction;
      snailShell.SetDirection(snailShell.Direction);
      snailShell.FloatDeath(this._inLiquidRef, 0.15f, !this.IsHidding && !this.IsTurning);
      snailShell.UpdateBoundingBox();
      if (this.IsAttachedToPath)
        snailShell.AdjustObjectToPath(this.PathNode.Value, 0.0f, 0.0f, 0.0f, 0.0f);
      Stage.CurrentStage.AddObjectInRuntime((StageObject) snailShell);
      this.DisposeFromStage();
    }

    protected override void OnHitFloor(bool floorIsBreakable)
    {
      this.PlayHitFloorImpactSound(floorIsBreakable);
      if ((double) this.Rotation == 90.0)
      {
        if (this.Direction != MovingObject.WalkDirection.CounterClockwise)
        { }
      }
      else if ((double) this.Rotation == 270.0)
      {
        if (this.Direction != MovingObject.WalkDirection.Clockwise)
        { }
      }
      else if ((double) this.Rotation == 180.0)
        this.InvertDirection();
      if (this._snailStatus != Snail.SnailStatus.Normal)
        return;
      this.SetStateToWalk();
    }

    protected virtual void SpriteChanged()
    {
      foreach (SnailSpriteAccessory spriteAcessory in this._spriteAcessories)
      {
        if (spriteAcessory.Visible)
          spriteAcessory.UpdateActiveSprite();
      }
    }

    private BoundingSquare GetHeadBBTramsformed()
    {
      if (this.Sprite.Frames[this.CurrentFrame].WithCollisionBox)
      {
        if (1 < this.Sprite.Frames[this.CurrentFrame].BoundingBoxes.Length)
          return this.TransformBoundingBox(this.Sprite.Frames[this.CurrentFrame].BoundingBoxes[1]).ToBoundingSquare();
      }
      else if (1 < this.Sprite.BoundingBoxes.Length)
        return this.TransformBoundingBox(this.Sprite.BoundingBoxes[1]).ToBoundingSquare();
      return new BoundingSquare();
    }

    private BoundingSquare GetShellBBTramsformed()
    {
      if (this.Sprite.Frames[this.CurrentFrame].WithCollisionBox)
      {
        if (0 < this.Sprite.Frames[this.CurrentFrame].BoundingBoxes.Length)
          return this.TransformBoundingBox(this.Sprite.Frames[this.CurrentFrame].BoundingBoxes[0]).ToBoundingSquare();
      }
      else if (0 < this.Sprite.BoundingBoxes.Length)
        return this.TransformBoundingBox(this.Sprite.BoundingBoxes[0]).ToBoundingSquare();
      return new BoundingSquare();
    }

    public bool CheckCollisionWithHead(BoundingSquare bs)
    {
      return this.GetHeadBBTramsformed().Collides(bs);
    }

    public bool CheckLineCollisionWithShell(Vector2 p0, Vector2 p1)
    {
      return this.GetShellBBTramsformed().IntersectsLine(p0, p1, out Vector2 _);
    }

    public bool CheckCollisionWithTrampolim(BoundingSquare bs) => this.CheckCollisionWithBB(2, bs);

    public bool CheckCollisionWithVitamin(BoundingSquare bs) => this.CheckCollisionWithBB(0, bs);

    public bool CheckCollisionWithLaserBeam(LaserBeam laser)
    {
      return this.CheckLineCollisionWithShell(laser.BeamOrigin, laser.BeamEndPoint);
    }

    public override bool CheckCollisionWithLiquid(Liquid liquid)
    {
      return this.IsHidding ? this.CheckCollisionWithBB(0, liquid.QuadtreeCollisionBB) : this.CheckCollisionWithHead(liquid.QuadtreeCollisionBB);
    }

    public bool CheckCollisionWithBB(int bbIndex, BoundingSquare bs)
    {
      if (this.Sprite.Frames[this.CurrentFrame].WithCollisionBox)
      {
        if (bbIndex < this.Sprite.Frames[this.CurrentFrame].BoundingBoxes.Length)
          return this.TransformBoundingBox(this.Sprite.Frames[this.CurrentFrame].BoundingBoxes[bbIndex]).ToBoundingSquare().Collides(bs);
      }
      else if (bbIndex < this.Sprite.BoundingBoxes.Length)
        return this.TransformBoundingBox(this.Sprite.BoundingBoxes[bbIndex]).ToBoundingSquare().Collides(bs);
      return false;
    }

    public void SetEatingApple(Apple apple)
    {
      if (!this.CanEatApple)
        return;
      this.SetUnvitaminized();
      if (this.IsTurning)
      {
        this.EndTurning();
        if (this.Direction == MovingObject.WalkDirection.CounterClockwise)
          this.SetObjectLLCornerToPoint(apple.AABoundingBox.LowerLeft);
        else
          this.SetObjectLRCornerToPoint(apple.AABoundingBox.LowerRight);
      }
      this._apple = apple;
      this.SetBitingApple();
    }

    private void SetBitingApple()
    {
      this._eatingElapsedGameTime = 0.0;
      this.Sprite = this.SpriteByting;
      this.CurrentFrame = 0;
      this._snailStatus = Snail.SnailStatus.Byting;
      this.State = MovingObject.MovingState.Stopped;
      this.DynamicFlags |= StageObjectDynamicFlags.IsEating;
      this._apple.SnailBite();
    }

    private void SetChewingApple()
    {
      this._eatingElapsedGameTime = 0.0;
      this.Sprite = this.SpriteChewing;
      this.CurrentFrame = 0;
      this._snailStatus = Snail.SnailStatus.Chewing;
    }

    public void RemoveEatingApple()
    {
      this._eatingElapsedGameTime = 0.0;
      this.SetStateToWalk();
      this.DynamicFlags &= ~StageObjectDynamicFlags.IsEating;
      this._snailStatus = Snail.SnailStatus.Normal;
      this._apple.SnailStoppedEating();
      this._apple = (Apple) null;
    }

    public void SetUnvitaminized()
    {
      this.DynamicFlags &= ~StageObjectDynamicFlags.IsVitaminized;
      this.AdjustSnailSpeed();
      this._rocketAccessory.Visible = false;
      this._helmetAccessory.Visible = false;
      if (this._vitamin == null)
        return;
      this._vitamin.SnailRemoved();
      this._vitamin = (Vitamin) null;
    }

    public void SetVitaminized(Vitamin vitamin)
    {
      if (!this.IsEvilSnail)
        ++Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalBoosts;
      this._vitamin = vitamin;
      this.DynamicFlags |= StageObjectDynamicFlags.IsVitaminized;
      this.AdjustSnailSpeed();
      this._rocketAccessory.Visible = true;
      this._helmetAccessory.Visible = true;
    }

    public void SetUnderWater()
    {
      this.DynamicFlags |= StageObjectDynamicFlags.IsUnderLiquid;
      this.AdjustSnailSpeed();
    }

    public void AdjustSnailSpeed()
    {
      if (this.IsVitaminized)
        this.Speed = 0.06f;
      else
        this.Speed = 0.03f;
      if (this.IsUnderLiquid)
        this.Speed *= 0.5f;
      this._frameUpdateMultiplier = this.Speed / 0.03f;
    }

    public void ResetUnderWater()
    {
      this._inWaterElapsedGameTime = 0.0;
      this.AdjustSnailSpeed();
    }

    public void OnEnteringStageExit(StageExit exit)
    {
      if (this._snailEnteringExit != null && !this._snailEnteringExit.IsPlaying)
        this._snailEnteringExit.Play();
      this.Sprite = this._EnteringStageExitSprite;
      this.CurrentFrame = 0;
      this._snailStatus = Snail.SnailStatus.ExitingStage;
      this.State = MovingObject.MovingState.None;
      this.Position = exit.DoorPosition;
      if (!this.IsSnailKing)
        return;
      Stage.CurrentStage.SnailKingDelivered();
    }

    public void JumpOnTrampoline(Trampoline trampoline)
    {
      this.SetStateToAirborne();
      this.Rotation = 0.0f;
      float num = (float) Math.Sin((double) MathHelper.ToRadians(90f - trampoline.Angle));
      Vector2 direction = new Vector2((float) Math.Cos((double) MathHelper.ToRadians(90f - trampoline.Angle)), -num) * trampoline.JumpSpeed;
      if (this.Direction == MovingObject.WalkDirection.Clockwise && (double) direction.X < -0.01 || this.Direction == MovingObject.WalkDirection.CounterClockwise && (double) direction.X > 0.01)
        this.InvertDirection();
      this.Project(direction, trampoline.JumpSpeed);
    }

    public void Hibernate()
    {
      if (!this.IsEvilSnail)
        BrainGame.AchievementsManager.Notify(20);
      this.HideInShell(Snail.SnailStatus.Hibernate);
      this.DettachFromPath();
    }

    public void HideInShell(Snail.SnailStatus hiddingReason)
    {
      this.Rotation = 0.0f;
      this.Sprite = this.SpriteHiddingInShell;
      this.CurrentFrame = 0;
      this.UpdateBoundingBox();
      this._SpritePlaybackMode = Object2D.AnimtionPlaybackModes.PlayOnce;
      switch (this.State)
      {
        case MovingObject.MovingState.InnerTurning:
          if (this.Direction == MovingObject.WalkDirection.Clockwise)
          {
            if (this.IsWalkingOnCeiling())
            {
              this.SetObjectURCornerToPoint(this.Position);
              Snail snail = this;
              snail.Position = snail.Position - new Vector2(30f, 0.0f);
            }
            else if (this.IsClimbingAWall())
              this.SetObjectLLCornerToPoint(this.Position);
            else if (this.IsWalkingOnFloor())
            {
              this.SetObjectLRCornerToPoint(this.Position);
            }
            else
            {
              this.SetObjectULCornerToPoint(this.Position);
              Snail snail = this;
              snail.Position = snail.Position + new Vector2(10f, 0.0f);
            }
          }
          else if (this.IsWalkingOnCeiling())
            this.SetObjectLRCornerToPoint(this.Position);
          else if (this.IsClimbingAWall())
            this.SetObjectLRCornerToPoint(this.Position);
          else if (this.IsWalkingOnFloor())
            this.SetObjectLRCornerToPoint(this.Position);
          else
            this.SetObjectURCornerToPoint(this.Position + new Vector2(-10f, 0.0f));
          this.UpdateBoundingBox();
          break;
        case MovingObject.MovingState.OuterTurning:
          if (this.Direction == MovingObject.WalkDirection.Clockwise)
          {
            if (this.IsWalkingOnFloor())
            {
              this.SetObjectLRCornerToPoint(this.Position);
              Snail snail = this;
              snail.Position = snail.Position - new Vector2(10f, 0.0f);
            }
            else if (this.IsClimbingAWall())
            {
              this.SetObjectURCornerToPoint(this.Position);
              Snail snail = this;
              snail.Position = snail.Position + new Vector2(-10f, 1f);
            }
            else if (this.IsWalkingOnCeiling())
            {
              this.SetObjectULCornerToPoint(this.Position);
              Snail snail = this;
              snail.Position = snail.Position + new Vector2(1f, 1f);
            }
            else
            {
              this.SetObjectLLCornerToPoint(this.Position);
              Snail snail = this;
              snail.Position = snail.Position + new Vector2(10f, 0.0f);
            }
          }
          else if (this.IsWalkingOnFloor())
          {
            this.SetObjectLLCornerToPoint(this.Position);
            Snail snail = this;
            snail.Position = snail.Position + new Vector2(10f, 0.0f);
          }
          else if (this.IsClimbingAWall())
          {
            this.SetObjectULCornerToPoint(this.Position);
            Snail snail = this;
            snail.Position = snail.Position + new Vector2(10f, 1f);
          }
          else if (this.IsWalkingOnCeiling())
          {
            this.SetObjectURCornerToPoint(this.Position);
            Snail snail = this;
            snail.Position = snail.Position + new Vector2(-1f, 1f);
          }
          else
          {
            this.SetObjectLRCornerToPoint(this.Position);
            Snail snail = this;
            snail.Position = snail.Position - new Vector2(10f, 0.0f);
          }
          this.UpdateBoundingBox();
          this.DettachFromPath();
          break;
        default:
          this.AdjustObjectToPath(10f);
          break;
      }
      this.State = MovingObject.MovingState.Stopped;
      this._snailStatus = hiddingReason;
      if (this._snailStatus == Snail.SnailStatus.Hidding)
        this._genericTimer = 2000.0;
      this.StaticFlags &= ~StageObjectStaticFlags.CanWalkOnWalls;
      if (!this.IsAttachedToPath || this.PathNode.Value.WallType == PathSegment.SegmentType.Floor)
        return;
      this.DettachFromPath();
    }

    public bool CollidedWithLiquidTap(LiquidTap tap)
    {
      return this.IsAttachedToPath && this.State == MovingObject.MovingState.Walking && this.CheckCollisionWithHead(tap.AABoundingBox);
    }

    public void CollidedWithSalt(Salt salt)
    {
      if (!this.IsTurning)
      {
        if (this.IsWalkingOnCeiling())
        {
          this.Rotation = 0.0f;
          this.InvertDirection();
        }
        else if (this.IsClimbingAWall())
          this.InvertDirection();
        this.HideInShell(Snail.SnailStatus.Hidding);
      }
      else if (this.IsOuterturning)
      {
        bool flag = this.CurrentFrame < this.Sprite.FrameCount / 2;
        if (this.IsWalkingOnCeiling())
        {
          this.Rotation = 0.0f;
          this.HideInShell(Snail.SnailStatus.Hidding);
          if (flag)
            return;
          if (this.Direction == MovingObject.WalkDirection.Clockwise)
            this.SetObjectULCornerToPoint(salt.AABoundingBox.UpperRight + new Vector2(0.0f, 1f));
          else
            this.SetObjectURCornerToPoint(salt.AABoundingBox.UpperLeft + new Vector2(0.0f, 1f));
          this.InvertDirection();
        }
        else if (this.IsWalkingOnFloor())
        {
          this.HideInShell(Snail.SnailStatus.Hidding);
          if (!flag)
          {
            if (this.Direction == MovingObject.WalkDirection.Clockwise)
              this.SetObjectLRCornerToPoint(salt.AABoundingBox.LowerLeft + new Vector2(0.0f, -1f));
            else
              this.SetObjectLLCornerToPoint(salt.AABoundingBox.LowerRight + new Vector2(0.0f, -1f));
          }
          else
            this.InvertDirection();
        }
        else if (this.IsClimbingAWall())
        {
          this.HideInShell(Snail.SnailStatus.Hidding);
          this.InvertDirection();
          if (!flag)
            return;
          if (this.Direction == MovingObject.WalkDirection.Clockwise)
            this.SetObjectURCornerToPoint(salt.AABoundingBox.LowerLeft + new Vector2(0.0f, 1f));
          else
            this.SetObjectULCornerToPoint(salt.AABoundingBox.LowerRight + new Vector2(0.0f, 1f));
        }
        else
          this.HideInShell(Snail.SnailStatus.Hidding);
      }
      else
      {
        bool flag = this.CurrentFrame < this.Sprite.FrameCount / 2;
        if (this.IsClimbingAWall())
        {
          if (flag)
            return;
          Vector2 position = this.Position;
          this.HideInShell(Snail.SnailStatus.Hidding);
          this.InvertDirection();
          if (this.Direction == MovingObject.WalkDirection.Clockwise)
            this.SetObjectLLCornerToPoint(position + new Vector2(1f, -1f));
          else
            this.SetObjectLRCornerToPoint(position + new Vector2(-1f, -1f));
        }
        else if (this.IsWalkingOnCeiling())
        {
          Vector2 position = this.Position;
          this.HideInShell(Snail.SnailStatus.Hidding);
          this.InvertDirection();
          if (this.Direction == MovingObject.WalkDirection.Clockwise)
            this.SetObjectULCornerToPoint(position + new Vector2(1f, 1f));
          else
            this.SetObjectURCornerToPoint(position + new Vector2(-1f, 1f));
        }
        else if (this.IsWalkingOnFloor())
        {
          Vector2 position = this.Position;
          this.HideInShell(Snail.SnailStatus.Hidding);
          if (this.Direction == MovingObject.WalkDirection.Clockwise)
            this.SetObjectLLCornerToPoint(position + new Vector2(1f, 1f));
          else
            this.SetObjectLRCornerToPoint(position + new Vector2(-1f, 1f));
        }
        else
          this.HideInShell(Snail.SnailStatus.Hidding);
      }
    }

    public void DeathWithProjection(Vector2 direction, float speed)
    {
      this.Project(direction, speed);
      this.ProjectAccessories(true);
      this._snailStatus = Snail.SnailStatus.DeathWithStageExitEffect;
      this.Sprite = this.DeathWithStageExitSprite;
      this.CurrentFrame = 0;
    }

    public void UnempaleFromSpikes()
    {
      this.EffectsBlender.Add((ITransformEffect) new GravityEffect(Game1.GameSettings.Gravity, 0.0f), 2);
      this.Sprite = this.SpriteDeadBrokenShell;
    }

    protected virtual void ProjectAccessories(bool projectCrown)
    {
      for (int index = 0; index < this._spriteAcessories.Count; ++index)
      {
        SnailSpriteAccessory spriteAcessory = this._spriteAcessories[index];
        if (spriteAcessory.Visible && (!(spriteAcessory is CrownAccessory) || projectCrown))
        {
          spriteAcessory.Project();
          this._spriteAcessories.RemoveAt(index);
          --index;
        }
      }
    }

    private bool IsClimbingAWall()
    {
      return this.IsAttachedToPath && this.PathNode != null && (this.Direction == MovingObject.WalkDirection.Clockwise && this.PathNode.Value.WallType == PathSegment.SegmentType.RightWall || this.Direction == MovingObject.WalkDirection.CounterClockwise && this.PathNode.Value.WallType == PathSegment.SegmentType.LeftWall);
    }

    private bool IsWalkingOnCeiling()
    {
      return this.IsAttachedToPath && this.PathNode != null && this.PathNode.Value.WallType == PathSegment.SegmentType.Ceiling;
    }

    private bool IsWalkingOnFloor()
    {
      return this.IsAttachedToPath && this.PathNode != null && this.PathNode.Value.WallType == PathSegment.SegmentType.Floor;
    }

    private bool IsAttachedtoFloor()
    {
      return this.IsAttachedToPath && this.PathNode != null && this.PathNode.Value.WallType == PathSegment.SegmentType.Floor;
    }

    internal void PlayRandomDeathSound()
    {
      this._soundsGenericSnailDeath[BrainGame.Rand.Next(this._soundsGenericSnailDeath.Length)].Play();
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this._allowRocketPickUp = record.GetFieldValue<bool>("allowRocketPickUp", true);
      this._allowEatApple = record.GetFieldValue<bool>("allowEatApple", true);
      this._allowAffectedBySalt = record.GetFieldValue<bool>("allowAffectedBySalt", true);
      this._allowJumpOnTrampoline = record.GetFieldValue<bool>("allowJumpOnTrampoline", true);
      this._allowObjectPickup = record.GetFieldValue<bool>("allowObjectPickup", true);
      this.AllowStageStatistics = record.GetFieldValue<bool>("allowStageStatistics", true);
      this._allowKilledByEvilSnail = record.GetFieldValue<bool>("allowKilledByEvilSnail", true);
      this._allowSwitchActivation = record.GetFieldValue<bool>("allowSwitchActivation", true);
      this._allowEnterStageExit = record.GetFieldValue<bool>("allowEnterStageExit", true);
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord(context);
      switch (context)
      {
        case ToDataFileRecordContext.StageDataSave:
          dataFileRecord.AddField("allowRocketPickUp", (object) this._allowRocketPickUp);
          dataFileRecord.AddField("allowEatApple", (object) this._allowEatApple);
          dataFileRecord.AddField("allowAffectedBySalt", (object) this._allowAffectedBySalt);
          dataFileRecord.AddField("allowJumpOnTrampoline", (object) this._allowJumpOnTrampoline);
          dataFileRecord.AddField("allowObjectPickup", (object) this._allowObjectPickup);
          dataFileRecord.AddField("allowStageStatistics", (object) this.AllowStageStatistics);
          dataFileRecord.AddField("allowKilledByEvilSnail", (object) this._allowKilledByEvilSnail);
          dataFileRecord.AddField("allowSwitchActivation", (object) this._allowSwitchActivation);
          dataFileRecord.AddField("allowEnterStageExit", (object) this._allowEnterStageExit);
          break;
      }
      return dataFileRecord;
    }

    public enum SnailStatus
    {
      Normal,
      Hidding,
      DeathByFire,
      DeathBySpikes,
      DeathWithStageExitEffect,
      Byting,
      Chewing,
      ExitingStage,
      Hibernate,
      DeathByFireSmoke,
      DeathByWater,
    }
  }
}
