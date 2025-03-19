
// Type: TwoBrainsGames.Snails.ToolObjects.ToolDynamite
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.ToolObjects
{
  public class ToolDynamite : ToolObject
  {
    private const string SPRITE_PLAYER_CURSOR_BOMB_SELECTED = "BombToolSelected";
    public const string ID = "TOOL_DYNAMITE";
    private Sprite _spriteHotSpot;
    private float _hotSpotRotation;
    private Dynamite _dynamite;

    public ToolDynamite()
      : base(ToolObjectType.Dynamite)
    {
    }

    public ToolDynamite(ToolObjectType type)
      : base(type)
    {
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this._spriteHotSpot = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/dynamite/DynamiteRadius");
      this._hotSpotRotation = 0.0f;
    }

    public override void Action(Vector2 position)
    {
      if (this.Quantity <= 0)
        return;
      base.Action(position);
      Dynamite dynamite = (Dynamite) Stage.CurrentStage.StageData.GetObject("DYNAMITE");
      dynamite.Position = position;
      dynamite.UpdateBoundingBox();
      Stage.CurrentStage.AddObjectInRuntime((StageObject) dynamite);
    }

    public override void OnSelect()
    {
      base.OnSelect();
      if (this._dynamite != null)
        return;
      this._dynamite = (Dynamite) Stage.CurrentStage.StageData.GetObject("DYNAMITE");
      this._dynamite.Extinguish();
    }

    public override void Update(BrainGameTime gameTime)
    {
      this._hotSpotRotation += (float) (0.0099999997764825821 * gameTime.ElapsedGameTime.TotalMilliseconds);
    }

    public override void DrawCursor(Vector2 position, bool enabled)
    {
      this._dynamite.Position = position;
      this._dynamite.Draw(false);
      if (!enabled)
        return;
      this._spriteHotSpot.Draw(position, 0, this._hotSpotRotation, SpriteEffects.None, Levels.CurrentLevel.SpriteBatch);
    }
  }
}
