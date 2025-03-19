
// Type: TwoBrainsGames.Snails.StageObjects.MovingObject
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.Snails.Effects;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class MovingObject : StageObject
  {
    private const string SPRITE_SNAIL_WALK = "SnailWalk";
    private const string SPRITE_SNAIL_TURN_UP = "SnailTurnUp";
    private const string SPRITE_SNAIL_TURN_DOWN = "SnailTurnDown";
    private const float BOUNCE_XSPEED_DECAY = 0.8f;
    public MovingObject.WalkDirection Direction;
    public float Speed;
    protected Vector2 PivotPoint;
    protected MovingObject.MovingState State;
    public Sprite InnerTurnSprite;
    public Sprite OuterTurnSprite;
    public Sprite WalkSprite;
    protected BoardPathNode PathNode;
    protected MotionEffect _gravityEffect;
    protected LiquidGravityEffect _liquidGravityEffect;
    private HooverEffect _hooverEffect;
    protected SpriteAnimation _bubblesMovementAnim;
    private Sample _bubblesSound;
    protected float _fallingHeight;
    private Sample _impactOnTileSound;
    private Sample _impactOnBreakableTileSound;
    private Sample _inpactOnTileUnderwater;

    public PathSegment.SegmentType WalkSegmentType
    {
      get => !this.IsAttachedToPath ? PathSegment.SegmentType.None : this.PathNode.Value.WallType;
    }

    public Vector2 HeadPoint
    {
      get
      {
        return this.Direction != MovingObject.WalkDirection.Clockwise ? this.BoundingBox.P3 : this.BoundingBox.P2;
      }
    }

    public Vector2 TailPoint
    {
      get
      {
        return this.Direction != MovingObject.WalkDirection.Clockwise ? this.BoundingBox.P2 : this.BoundingBox.P3;
      }
    }

    public bool IsAttachedToPath
    {
      get => this.PathNode != null && this.PathNode.Value != (PathSegment) null;
    }

    public bool IsTurning
    {
      get
      {
        return this.State == MovingObject.MovingState.InnerTurning || this.State == MovingObject.MovingState.OuterTurning;
      }
    }

    public bool IsOuterturning => this.State == MovingObject.MovingState.OuterTurning;

    public bool IsInnerTurning => this.State == MovingObject.MovingState.InnerTurning;

    public MovingObject(StageObjectType objType)
      : base(objType)
    {
    }

    public MovingObject(StageObject other)
      : base(other)
    {
    }

    public override void Copy(StageObject other)
    {
      base.Copy(other);
      MovingObject movingObject = other as MovingObject;
      this.WalkSprite = movingObject.WalkSprite;
      this.InnerTurnSprite = movingObject.InnerTurnSprite;
      this.OuterTurnSprite = movingObject.OuterTurnSprite;
      this.Direction = movingObject.Direction;
      this.Speed = movingObject.Speed;
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this.WalkSprite = this.Sprite;
      if (this.CanWalkOnWalls)
      {
        this.InnerTurnSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "SnailTurnUp");
        this.OuterTurnSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "SnailTurnDown");
      }
      this._bubblesMovementAnim = new SpriteAnimation(BrainGame.ResourceManager.GetSpriteTemporary("spriteset/water", "Bubbles"));
      this._bubblesMovementAnim.Visible = false;
      this._bubblesSound = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/underwater-bubbles");
      if (Stage.CurrentStage == null)
        return;
      this._impactOnTileSound = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.TILE_IMPACT, (Object2D) this);
      this._impactOnBreakableTileSound = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.BREAKBLE_TILE_IMPACT, (Object2D) this);
      this._inpactOnTileUnderwater = BrainGame.ResourceManager.GetSampleTemporary(AudioTags.TILE_IMPACT_UNDERWATER, (Object2D) this);
    }

    public override void Initialize()
    {
      base.Initialize();
      if (!this.EffectsBlender.Contains(2))
      {
        this._gravityEffect = new MotionEffect(Game1.GameSettings.Gravity, Vector2.Zero);
        this._gravityEffect.Active = false;
        this.EffectsBlender.Add((ITransformEffect) this._gravityEffect, 2);
      }
      this._liquidGravityEffect = new LiquidGravityEffect(5f);
      this._liquidGravityEffect.Active = false;
      this.EffectsBlender.Add((ITransformEffect) this._liquidGravityEffect, 3);
    }

    protected void EndTurning()
    {
      this.Sprite = this.WalkSprite;
      this.CurrentFrame = 0;
      this.State = MovingObject.MovingState.Walking;
      if (this.IsAttachedToPath)
      {
        this.Rotation = this.PathNode.Value.Rotation;
        this.UpdateBoundingBox();
        this.PlaceObjectInBeginingPath();
      }
      this.UpdateBoundingBox();
    }

    public override void Hide()
    {
      base.Hide();
      this.HideBubbles();
    }

    public override void OnLastFrame()
    {
      if (this.IsDead)
      {
        base.OnLastFrame();
      }
      else
      {
        if (this.State != MovingObject.MovingState.InnerTurning && this.State != MovingObject.MovingState.OuterTurning)
          return;
        this.EndTurning();
      }
    }

    public override void OnEnterLiquid(Liquid liquid)
    {
      base.OnEnterLiquid(liquid);
      if (!this.IsFalling)
        return;
      this.AddGravityEffect(Vector2.Zero);
    }

    public override void OnExitLiquid()
    {
      base.OnExitLiquid();
      this.HideBubbles();
    }

    protected void PlaceObjectInBeginingPath()
    {
      if (this.Direction == MovingObject.WalkDirection.Clockwise)
      {
        this.Position = this.PathNode.Value.P0;
        MovingObject movingObject = this;
        movingObject.Position = movingObject.Position + this.PathNode.Value.Normal * -this.Sprite.BoundingBox.Left;
      }
      else
      {
        this.Position = this.PathNode.Value.P1;
        MovingObject movingObject = this;
        movingObject.Position = movingObject.Position - this.PathNode.Value.Normal * -this.Sprite.BoundingBox.Left;
      }
    }

    public BoardPathNode[] GetCollidingPaths(BoundingSquare bs)
    {
      List<IQuadtreeContainable> collidingObjects = Stage.CurrentStage.Board.Quadtree.GetCollidingObjects(bs, 2);
      if (collidingObjects.Count == 0)
        return (BoardPathNode[]) null;
      BoardPathNode[] collidingPaths = new BoardPathNode[collidingObjects.Count];
      for (int index = 0; index < collidingObjects.Count; ++index)
        collidingPaths[index] = (BoardPathNode) collidingObjects[index];
      return collidingPaths;
    }

    private void CheckCollisionsWithPaths(OOBoundingBox bbOld)
    {
      if (!this.CanCollide)
        return;
      if (bbOld.Equals(this.BoundingBox))
      {
        BoardPathNode[] collidingPaths = this.GetCollidingPaths(bbOld.ToBoundingSquare());
        if (collidingPaths != null)
        {
          for (int index = 0; index < collidingPaths.Length; ++index)
          {
            this.AdjustObjectToPath(collidingPaths[index].Value, 1f, 1f, 1f, 0.0f);
            this.UpdateBoundingBox();
          }
        }
        bbOld = this.BoundingBox;
      }
      int num1 = 0;
      int num2 = 0;
      Vector2 retIntersectPt;
      BoardPathNode retNode;
      MovingObject.CollidingCorner cornerOfCollision;
      Vector2 bbCollidingPoint;
      while (Stage.CurrentStage.Board.Collides((StageObject) this, ref bbOld, ref this.BoundingBox, out retIntersectPt, out retNode, out cornerOfCollision, out bbCollidingPoint) > 0)
      {
        this.ReactToColision(retIntersectPt, retNode, cornerOfCollision, bbCollidingPoint);
        ++num2;
        if (this.IsAttachedToPath || num1 > 10)
          break;
      }
      if (num2 != 0)
        return;
      this.Quadtree.ObjectMoved((IQuadtreeContainable) this);
      while (Stage.CurrentStage.Board.Collides((StageObject) this, ref bbOld, ref this.BoundingBox, out retIntersectPt, out retNode, out cornerOfCollision, out bbCollidingPoint) > 0)
      {
        this.ReactToColision(retIntersectPt, retNode, cornerOfCollision, bbCollidingPoint);
        if (this.IsAttachedToPath || num1 > 10)
          break;
      }
    }

    public virtual void DoWalk(BrainGameTime gameTime)
    {
    }

    public override void Update(BrainGameTime gameTime)
    {
      this.PreviousPosition = this.Position;
      OOBoundingBox boundingBox = this.BoundingBox;
      base.Update(gameTime);
      if (this.Killed)
      {
        this.Sprite = this.WalkSprite;
        this.CurrentFrame = 0;
        if (!this.BoundingBoxChanged)
          return;
        this.RepositionObjectInQuadtree();
      }
      else
      {
        if (this.IsDisposed || this.IsDead)
          return;
        if (this.IsAttachedToPath && this.CanWalk && this.State == MovingObject.MovingState.Walking)
          this.DoWalk(gameTime);
        if (!this.IsAttachedToPath)
        {
          if (this.CanFall && !this.IsFalling)
            this.SetStateToAirborne();
          if (!this.IgnorePathCollisions && this.CanCollide)
            this.CheckCollisionsWithPaths(boundingBox);
        }
        if (this._bubblesMovementAnim.Visible)
          this._bubblesMovementAnim.Update(gameTime);
        if (!this.IsFalling || (double) this.PreviousPosition.Y >= (double) this.Position.Y)
          return;
        this._fallingHeight += this.Position.Y - this.PreviousPosition.Y;
      }
    }

    protected void RemoveGravityEffect()
    {
      this._gravityEffect.Active = false;
      this._liquidGravityEffect.Active = false;
      this.HideBubbles();
    }

    protected void AddGravityEffect(Vector2 initialSpeed)
    {
      if (this.IsUnderLiquid)
      {
        this._gravityEffect.Active = false;
        this._gravityEffect.Active = false;
        this._liquidGravityEffect.Active = true;
        this._liquidGravityEffect.Reset();
        this._bubblesMovementAnim.Visible = true;
        this._bubblesSound.Play(true);
      }
      else
      {
        this._liquidGravityEffect.Active = false;
        this._gravityEffect.Active = true;
        this._gravityEffect.Reset(initialSpeed);
      }
    }

    private void HideBubbles()
    {
      this._bubblesMovementAnim.Visible = false;
      this._bubblesSound.Stop();
    }

    protected void PlayHitFloorImpactSound(bool tileIsBreakble)
    {
      if ((double) this._fallingHeight <= 30.0)
        return;
      if ((double) (this._fallingHeight / 240f) > 1.0)
        ;
      if (this.IsUnderLiquid)
        this._inpactOnTileUnderwater.Play();
      else if (tileIsBreakble)
        this._impactOnBreakableTileSound.Play();
      else
        this._impactOnTileSound.Play();
    }

    protected virtual void OnHitFloor(bool floorIsBreakable)
    {
      this.PlayHitFloorImpactSound(floorIsBreakable);
      if (!this.CanWalk)
        return;
      this.SetStateToWalk();
    }

    protected virtual void OnTileCollision(PathSegment.SegmentType wallType)
    {
    }

    protected virtual void ReactToColision(
      Vector2 collisionPoint,
      BoardPathNode nodeOfCollision,
      MovingObject.CollidingCorner cornerOfColision,
      Vector2 bbCollidingPoint)
    {
      if (nodeOfCollision.Value.WallType == PathSegment.SegmentType.Floor || this.CanWalkOnWalls && nodeOfCollision.Value.WallType != PathSegment.SegmentType.Floor && nodeOfCollision.Value.Behavior != PathSegmentBehavior.ReverseWalk)
      {
        this.PathNode = nodeOfCollision;
        if (nodeOfCollision.Value.WallType == PathSegment.SegmentType.Floor)
          this.OnHitFloor(nodeOfCollision.Value.FloorIsBreakable);
        else if (nodeOfCollision.Value.WallType == PathSegment.SegmentType.Ceiling)
          this.InvertDirection();
        this.Rotation = this.PathNode.Value.Rotation;
        this.AdjustObjectToPath(0.0f);
        this.RemoveHooverEffect();
        this.RemoveGravityEffect();
        this.DynamicFlags &= ~StageObjectDynamicFlags.IsFalling;
        if (this.CanHoover)
          this.AddHooverEffect();
        this.OnTileCollision(nodeOfCollision.Value.WallType);
      }
      else
      {
        switch (nodeOfCollision.Value.WallType)
        {
          case PathSegment.SegmentType.RightWall:
            this.AddGravityEffect(new Vector2((float) (-(double) this._gravityEffect.CurrentSpeed.X * 0.800000011920929), this._gravityEffect.CurrentSpeed.Y));
            this.Position = new Vector2((float) ((double) collisionPoint.X - (double) this.Sprite.BoundingBox.Width - (double) this.Sprite.BoundingBox.Left - 1.0), this.Position.Y);
            if (this.Direction == MovingObject.WalkDirection.Clockwise)
            {
              this.InvertDirection();
              break;
            }
            break;
          case PathSegment.SegmentType.LeftWall:
            this.AddGravityEffect(new Vector2((float) (-(double) this._gravityEffect.CurrentSpeed.X * 0.800000011920929), this._gravityEffect.CurrentSpeed.Y));
            this.Position = new Vector2((float) ((double) this.Position.X - ((double) this.AABoundingBox.Left - (double) collisionPoint.X) + 1.0), this.Position.Y);
            if (this.Direction == MovingObject.WalkDirection.CounterClockwise)
            {
              this.InvertDirection();
              break;
            }
            break;
          case PathSegment.SegmentType.Ceiling:
            this.AddGravityEffect(new Vector2(this._gravityEffect.CurrentSpeed.X, 0.0f));
            this.Position = new Vector2(this.Position.X, (float) ((double) collisionPoint.Y - (double) this.Sprite.BoundingBox.Top + 1.0));
            break;
        }
      }
      this.UpdateBoundingBox();
      this.Quadtree.ObjectMoved((IQuadtreeContainable) this);
    }

    public void InvertDirection()
    {
      this.SetDirection((MovingObject.WalkDirection) ((int) this.Direction * -1));
    }

    public void SetDirection(MovingObject.WalkDirection walkDirection)
    {
      this.Direction = walkDirection;
      if (this.Direction == MovingObject.WalkDirection.Clockwise)
        this.SpriteEffect = SpriteEffects.None;
      else
        this.SpriteEffect = SpriteEffects.FlipHorizontally;
    }

    protected virtual bool CanWalkOnPath(BoardPathNode node)
    {
      return node != null && (node.Value.Behavior != PathSegmentBehavior.WalkableCW || this.Direction == MovingObject.WalkDirection.Clockwise) && (node.Value.Behavior != PathSegmentBehavior.WalkableCCW || this.Direction == MovingObject.WalkDirection.CounterClockwise) && node.Value.Behavior != PathSegmentBehavior.ReverseWalk && node.Value.Behavior != PathSegmentBehavior.None && (node.Value.WallType == PathSegment.SegmentType.Floor || this.CanWalkOnWalls);
    }

    protected virtual bool CanWalkOnNextSegment() => this.CanWalkOnPath(this.PathNode.Next);

    protected virtual bool CanWalkOnPrevSegment() => this.CanWalkOnPath(this.PathNode.Previous);

    protected void SetStateToInnerTurn(float excessWalk)
    {
      this.State = MovingObject.MovingState.InnerTurning;
      this.Sprite = this.InnerTurnSprite;
      this.CurrentFrame = (int) ((double) excessWalk * (double) this.Sprite.FrameCount / (double) this.Sprite.BoundingBox.Width);
      if (this.CurrentFrame >= this.Sprite.FrameCount)
        this.CurrentFrame = this.Sprite.FrameCount - 1;
      this.Position = this.Direction == MovingObject.WalkDirection.Clockwise ? this.PathNode.Value.P0 : this.PathNode.Value.P1;
      this.UpdateBoundingBox();
    }

    protected void SetStateToOuterTurn(float excessWalk)
    {
      this.State = MovingObject.MovingState.OuterTurning;
      this.Sprite = this.OuterTurnSprite;
      this.CurrentFrame = (int) ((double) excessWalk * (double) this.Sprite.FrameCount / (double) this.Sprite.BoundingBox.Width);
      if (this.CurrentFrame >= this.Sprite.FrameCount)
        this.CurrentFrame = this.Sprite.FrameCount - 1;
      this.Position = this.Direction == MovingObject.WalkDirection.Clockwise ? this.PathNode.Value.P0 : this.PathNode.Value.P1;
      this.UpdateBoundingBox();
    }

    public void SetStateToAirborne()
    {
      this._fallingHeight = 0.0f;
      this.RemoveHooverEffect();
      if (this.IsTurning)
        this.SetStateToWalk();
      if (!this._gravityEffect.Active)
        this.AddGravityEffect(Vector2.Zero);
      this.DynamicFlags |= StageObjectDynamicFlags.IsFalling;
      this.DettachFromPath();
      this.UpdateBoundingBox();
      this.State = MovingObject.MovingState.Airbone;
    }

    public void SetStateToWalk()
    {
      this.CurrentFrame = 0;
      this.Sprite = this.WalkSprite;
      this.State = MovingObject.MovingState.Walking;
      this._SpritePlaybackMode = Object2D.AnimtionPlaybackModes.Loop;
      this.DynamicFlags &= ~StageObjectDynamicFlags.IsFalling;
      if (!this.IsAttachedToPath || this.PathNode.Value.WallType != PathSegment.SegmentType.Floor)
        return;
      this.AdjustObjectToFloor();
    }

    public void Project(float speed)
    {
      this.Project(Vector2.Transform(new Vector2(1f, 0.0f), Matrix.CreateRotationZ(MathHelper.ToRadians((float) BrainGame.Rand.Next(360)))), speed);
    }

    public virtual void Project(Vector2 direction, float speed)
    {
      direction.Normalize();
      this.AddGravityEffect(direction * speed);
    }

    public void ProjectWithRotation(Vector2 direction, float projectionSpeed, float rotationSpeed)
    {
      this.Project(direction, projectionSpeed);
      this.EffectsBlender.Add((ITransformEffect) new RotationEffect(rotationSpeed));
    }

    public virtual void KillWithGravityEffect()
    {
      this.DynamicFlags = StageObjectDynamicFlags.IsVisible | StageObjectDynamicFlags.IsDead;
      this.StaticFlags = StageObjectStaticFlags.CanFall;
      this.AddGravityEffect(Vector2.Zero);
      this.MoveToForeground();
    }

    public override void Kill()
    {
      if (!this.CanDie)
        return;
      base.Kill();
      this.CurrentFrame = 0;
      this.Sprite = this.WalkSprite;
      this.State = MovingObject.MovingState.Stopped;
    }

    public virtual void KillWithProjection()
    {
      this.KillMe();
      this.Rotation = 0.0f;
      float degrees = (float) (BrainGame.Rand.Next(90) - 135);
      float y = (float) Math.Sin((double) MathHelper.ToRadians(degrees));
      this.Project(new Vector2((float) Math.Cos((double) MathHelper.ToRadians(degrees)), y), 30f);
      this._autoDisposeIfDeadOnAnimationEnd = false;
    }

    public override void KillByExplosion(Explosion exp) => this.KillWithProjection();

    public virtual void KillByCrate()
    {
      if (this._projectWhenKilledByCrate)
        this.KillWithProjection();
      else
        this.DettachFromPath();
    }

    public override void Draw(bool shadow)
    {
      base.Draw(shadow);
      if (!this._bubblesMovementAnim.Visible || (double) this._inLiquidRef.GetDepth(this.Position) <= (double) this._bubblesMovementAnim.Height)
        return;
      this._bubblesMovementAnim.Draw(this.Position, Stage.CurrentStage.SpriteBatch);
    }

    public void AddHooverEffect()
    {
      if (this._hooverEffect == null)
      {
        this._hooverEffect = new HooverEffect(0.06f, 0.3f, 0.0f);
        this.EffectsBlender.Add((ITransformEffect) this._hooverEffect, 1);
      }
      this._hooverEffect.Active = true;
      this._hooverEffect.Reset();
      this.Position = new Vector2(this.Position.X, this.Position.Y - 5f);
    }

    public void RemoveHooverEffect()
    {
      if (this._hooverEffect == null)
        return;
      this._hooverEffect.Active = false;
      if (!this.IsAttachedToPath || this.PathNode.Value.WallType != PathSegment.SegmentType.Floor)
        return;
      this.AdjustObjectToFloor();
      this.UpdateBoundingBox();
    }

    protected void SetTailInEndOfPath()
    {
      if (!this.IsAttachedToPath)
        return;
      if (this.Direction == MovingObject.WalkDirection.CounterClockwise)
      {
        switch (this.PathNode.Value.WallType)
        {
          case PathSegment.SegmentType.Floor:
            this.SetObjectLRCornerToPoint(this.PathNode.Value.P1);
            break;
          case PathSegment.SegmentType.RightWall:
            this.SetObjectURCornerToPoint(this.PathNode.Value.P1);
            break;
          case PathSegment.SegmentType.LeftWall:
            this.SetObjectLLCornerToPoint(this.PathNode.Value.P1);
            break;
          case PathSegment.SegmentType.Ceiling:
            this.SetObjectULCornerToPoint(this.PathNode.Value.P1);
            break;
        }
      }
      else
      {
        switch (this.PathNode.Value.WallType)
        {
          case PathSegment.SegmentType.Floor:
            this.SetObjectLLCornerToPoint(this.PathNode.Value.P0);
            break;
          case PathSegment.SegmentType.RightWall:
            this.SetObjectLRCornerToPoint(this.PathNode.Value.P0);
            break;
          case PathSegment.SegmentType.LeftWall:
            this.SetObjectULCornerToPoint(this.PathNode.Value.P0);
            break;
          case PathSegment.SegmentType.Ceiling:
            this.SetObjectURCornerToPoint(this.PathNode.Value.P0);
            break;
        }
      }
    }

    protected void SetObjectLLCornerToPoint(Vector2 point)
    {
      this.Position = new Vector2(this.Position.X + (point.X - this.AABoundingBox.Left), this.Position.Y + (point.Y - this.AABoundingBox.Bottom));
    }

    protected void SetObjectLRCornerToPoint(Vector2 point)
    {
      this.Position = new Vector2(this.Position.X - (this.AABoundingBox.Right - point.X), this.Position.Y + (point.Y - this.AABoundingBox.Bottom));
    }

    protected void SetObjectULCornerToPoint(Vector2 point)
    {
      this.Position = new Vector2(this.Position.X + (point.X - this.AABoundingBox.Left), this.Position.Y - (this.AABoundingBox.Top - point.Y));
    }

    protected void SetObjectURCornerToPoint(Vector2 point)
    {
      this.Position = new Vector2(this.Position.X - (this.AABoundingBox.Right - point.X), this.Position.Y - (this.AABoundingBox.Top - point.Y));
    }

    public void AdjustObjectToPath(float sideWallsMargin)
    {
      if (!this.IsAttachedToPath)
        return;
      this.UpdateBoundingBox();
      this.AdjustObjectToPath(this.PathNode.Value, sideWallsMargin, 0.0f, 0.0f, 0.0f);
    }

    public void PlaceObjectInsidePath()
    {
      if (!this.IsAttachedToPath)
        return;
      PathSegment pathSegment = this.PathNode.Value;
      bool flag1 = (double) this.PathNode.Previous.Value.Rotation != (double) pathSegment.Rotation;
      bool flag2 = (double) this.PathNode.Next.Value.Rotation != (double) pathSegment.Rotation;
      if (!flag1 && !flag2)
        return;
      switch (pathSegment.WallType)
      {
        case PathSegment.SegmentType.Floor:
          if ((double) this.AABoundingBox.LowerLeft.X < (double) pathSegment.P0.X && flag1)
            this.SetObjectLLCornerToPoint(pathSegment.P0);
          if ((double) this.AABoundingBox.LowerRight.X <= (double) pathSegment.P1.X || !flag2)
            break;
          this.SetObjectLRCornerToPoint(pathSegment.P1);
          break;
        case PathSegment.SegmentType.RightWall:
          if ((double) this.AABoundingBox.UpperRight.Y < (double) pathSegment.P1.Y && flag2)
            this.SetObjectURCornerToPoint(pathSegment.P1);
          if ((double) this.AABoundingBox.LowerRight.Y <= (double) pathSegment.P0.Y || !flag1)
            break;
          this.SetObjectLRCornerToPoint(pathSegment.P0);
          break;
        case PathSegment.SegmentType.LeftWall:
          if ((double) this.AABoundingBox.UpperLeft.Y < (double) pathSegment.P0.Y && flag1)
            this.SetObjectULCornerToPoint(pathSegment.P0);
          if ((double) this.AABoundingBox.LowerLeft.Y <= (double) pathSegment.P1.Y || !flag2)
            break;
          this.SetObjectLLCornerToPoint(pathSegment.P1);
          break;
        case PathSegment.SegmentType.Ceiling:
          if ((double) this.AABoundingBox.UpperLeft.X < (double) pathSegment.P1.X && flag2)
            this.SetObjectULCornerToPoint(pathSegment.P1);
          if ((double) this.AABoundingBox.UpperRight.X <= (double) pathSegment.P0.X || !flag1)
            break;
          this.SetObjectURCornerToPoint(pathSegment.P0);
          break;
      }
    }

    public void AdjustObjectToPath(
      PathSegment segment,
      float sideWallsMargin,
      float topWallsMargin,
      float ceilingMargin,
      float floorMargin)
    {
      switch (segment.WallType)
      {
        case PathSegment.SegmentType.Floor:
          this.Position = new Vector2(this.Position.X, this.Position.Y + (segment.P0.Y - this.AABoundingBox.Bottom) - floorMargin);
          break;
        case PathSegment.SegmentType.RightWall:
          this.Position = new Vector2(this.Position.X + (segment.P0.X - this.AABoundingBox.Right) - sideWallsMargin, this.Position.Y);
          break;
        case PathSegment.SegmentType.LeftWall:
          this.Position = new Vector2(this.Position.X - (this.AABoundingBox.Left - segment.P0.X) + sideWallsMargin, this.Position.Y);
          break;
        case PathSegment.SegmentType.Ceiling:
          this.Position = new Vector2(this.Position.X, this.Position.Y - (this.AABoundingBox.Top - segment.P0.Y) + topWallsMargin);
          break;
      }
    }

    protected void AdjustObjectToFloor()
    {
      this.Position = new Vector2(this.Position.X, this.Position.Y + (this.PathNode.Value.P0.Y - this.AABoundingBox.Bottom));
    }

    public void MakeFall()
    {
      if (!this.IsAttachedToPath)
        return;
      switch (this.PathNode.Value.WallType)
      {
        case PathSegment.SegmentType.Floor:
          return;
        case PathSegment.SegmentType.LeftWall:
          this.Sprite = this.WalkSprite;
          this.Position = new Vector2(this.Position.X + 50f, this.Position.Y);
          this.UpdateBoundingBox();
          break;
      }
      this.DettachFromPath();
    }

    public void DettachFromPath() => this.PathNode = (BoardPathNode) null;

    public void AttachToPath(BoardPathNode node) => this.PathNode = node;

    public enum WalkDirection
    {
      CounterClockwise = -1, // 0xFFFFFFFF
      None = 0,
      Clockwise = 1,
    }

    public enum CollidingCorner
    {
      Node,
      UpperLeft,
      UpperRight,
      LowerLeft,
      LowerRight,
    }

    public enum MovingState
    {
      None,
      Stopped,
      Walking,
      InnerTurning,
      OuterTurning,
      Airbone,
    }
  }
}
