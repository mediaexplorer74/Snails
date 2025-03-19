
// Type: TwoBrainsGames.Snails.ToolObjects.ToolSalt
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.ToolObjects
{
  public class ToolSalt : ToolObject
  {
    public const string ID = "TOOL_SALT";
    private BoundingSquare _bsHotStop;
    private Sprite _spriteHotSpot;
    private float _hotSpotRotation;

    public ToolSalt()
      : base(ToolObjectType.Salt)
    {
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this.ObjectSprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/player-cursor", "SaltCursor");
      this._spriteHotSpot = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "SaltCursorHotSpot");
      this._bsHotStop = this.ObjectSprite.BoundingBox;
      this._hotSpotRotation = 0.0f;
    }

    public override void Action(Vector2 position)
    {
      base.Action(position);
      BoundingSquare hotSpot = this.ComputeHotSpot(position.X, position.Y);
      BoardPathNode nearestClickedPath = this.GetNearestClickedPath(hotSpot, position);
      if (nearestClickedPath == null)
        return;
      switch (nearestClickedPath.Value.WallType)
      {
        case PathSegment.SegmentType.Floor:
          this.AddSalt(hotSpot.Left + hotSpot.Width / 2f, nearestClickedPath.Value.P0.Y, Salt.SaltPosition.Floor, nearestClickedPath);
          break;
        case PathSegment.SegmentType.RightWall:
          this.AddSalt(nearestClickedPath.Value.P0.X, hotSpot.Top + hotSpot.Height / 2f, Salt.SaltPosition.Right, nearestClickedPath);
          break;
        case PathSegment.SegmentType.LeftWall:
          this.AddSalt(nearestClickedPath.Value.P0.X, hotSpot.Top + hotSpot.Height / 2f, Salt.SaltPosition.Left, nearestClickedPath);
          break;
        case PathSegment.SegmentType.Ceiling:
          this.AddSalt(hotSpot.Left + hotSpot.Width / 2f, nearestClickedPath.Value.P0.Y, Salt.SaltPosition.Ceiling, nearestClickedPath);
          break;
      }
    }

    private void AddSalt(float x, float y, Salt.SaltPosition pos, BoardPathNode node)
    {
      Salt salt = Stage.CurrentStage.StageData.GetObject("SALT") as Salt;
      salt.PlaceOnPath(x, y, pos, node);
      Stage.CurrentStage.AddObjectInRuntime((StageObject) salt);
    }

    public override bool IsValidAtPosition(Vector2 position)
    {
      BoundingSquare hotSpot = this.ComputeHotSpot(position.X, position.Y);
      return Stage.CurrentStage.Board.Quadtree.GetCollidingObjects(hotSpot, 2).Count > 0;
    }

    private BoundingSquare ComputeHotSpot(float x, float y)
    {
      return new BoundingSquare(new Vector2(x + this._bsHotStop.UpperLeft.X, y + this._bsHotStop.UpperLeft.Y), this._bsHotStop.Width, this._bsHotStop.Height);
    }

    public override void Update(BrainGameTime gameTime)
    {
      this._hotSpotRotation += (float) (0.029999999329447746 * gameTime.ElapsedGameTime.TotalMilliseconds);
    }

    public override void DrawCursor(Vector2 position, bool enabled)
    {
      if (!enabled)
        return;
      this._spriteHotSpot.Draw(position + new Vector2(this._bsHotStop.Left + this._bsHotStop.Width / 2f, this._bsHotStop.Top + this._bsHotStop.Height / 2f), 0, this._hotSpotRotation, SpriteEffects.None, Levels.CurrentLevel.SpriteBatch);
    }

    public override void SetCursorOnBoard(bool enabled)
    {
      if (enabled)
        BrainGame.GameCursor.SetCursor(3);
      else
        BrainGame.GameCursor.SetCursor(4);
    }
  }
}
