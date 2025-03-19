
// Type: TwoBrainsGames.Snails.StageObjects.Salt
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class Salt : MovingObject
  {
    public const string ID = "SALT";
    public Salt.SaltPosition _tilePosition;
    public Sprite _dissolvingSprite;
    private Salt.SaltState _state;

    public Salt()
      : base(StageObjectType.Salt)
    {
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this._dissolvingSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/stage-objects", "SaltDissolving");
    }

    public override void Initialize()
    {
      base.Initialize();
      this._state = Salt.SaltState.Idle;
    }

    public override void AfterBoardInitialize()
    {
      base.AfterBoardInitialize();
      BoardPathNode node = Stage.CurrentStage.Board.PathCollidesWithObject((StageObject) this);
      if (node == null)
        return;
      this.PlaceOnPath(this.Position.X, this.Position.Y, node);
    }

    public override void OnLastFrame()
    {
      base.OnLastFrame();
      if (this._state != Salt.SaltState.Dissolving)
        return;
      this.DisposeFromStage();
    }

    public override void OnCollide(IQuadtreeContainable obj, int listIdx)
    {
      Snail snail = obj as Snail;
      if (!snail.CanBeAffectedBySalt || !snail.CheckCollisionWithHead(this.AABoundingBox))
        return;
      snail.CollidedWithSalt(this);
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      if (this._state == Salt.SaltState.Dissolving)
        return;
      this.DoQuadtreeCollisions(0);
      if (this.IsAttachedToPath || !this.CanCollide)
        return;
      this.StaticFlags &= ~StageObjectStaticFlags.CanCollide;
    }

    public override void OnEnterLiquid(Liquid liquid)
    {
      base.OnEnterLiquid(liquid);
      this.Sprite = this._dissolvingSprite;
      this._state = Salt.SaltState.Dissolving;
      this.MoveToBackground();
    }

    public override void OnExitLiquid() => this.DisposeFromStage();

    private void PlaceOnPath(float x, float y, BoardPathNode node)
    {
      Salt.SaltPosition pos = Salt.SaltPosition.None;
      switch (node.Value.WallType)
      {
        case PathSegment.SegmentType.Floor:
          pos = Salt.SaltPosition.Floor;
          break;
        case PathSegment.SegmentType.RightWall:
          pos = Salt.SaltPosition.Right;
          break;
        case PathSegment.SegmentType.LeftWall:
          pos = Salt.SaltPosition.Left;
          break;
        case PathSegment.SegmentType.Ceiling:
          pos = Salt.SaltPosition.Ceiling;
          break;
      }
      this.PlaceOnPath(x, y, pos, node);
    }

    public void PlaceOnPath(float x, float y, Salt.SaltPosition pos, BoardPathNode node)
    {
      this._tilePosition = pos;
      this.AttachToPath(node);
      this.Rotation = node.Value.Rotation;
      this.Position = new Vector2(x, y);
      this.AdjustObjectToPath(0.0f);
      this.UpdateBoundingBox();
      this.PlaceObjectInsidePath();
      if (!Game1.GameSettings.UseTouch || this.PathNode.Next == null || this.PathNode.Next.Value.WallType == this.PathNode.Value.WallType || this.PathNode.Previous == null || this.PathNode.Previous.Value.WallType == this.PathNode.Value.WallType || (double) (this.PathNode.Value.Center - this.Position).Length() >= 10.0)
        return;
      this.Position = this.PathNode.Value.Center;
      this.AdjustObjectToPath(0.0f);
      this.UpdateBoundingBox();
      this.PlaceObjectInsidePath();
    }

    public enum SaltPosition
    {
      Left,
      Floor,
      Right,
      Ceiling,
      None,
    }

    private enum SaltState
    {
      Idle,
      Dissolving,
    }
  }
}
