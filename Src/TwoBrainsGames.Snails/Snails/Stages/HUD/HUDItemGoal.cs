
// Type: TwoBrainsGames.Snails.Stages.HUD.HUDItemGoal
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;


namespace TwoBrainsGames.Snails.Stages.HUD
{
  internal class HUDItemGoal : HUDItem
  {
    private Sprite _sprite;
    private Vector2 _spritePosition;

    public override void Initialize(Vector2 position)
    {
      base.Initialize(position);
      this._spritePosition = position + new Vector2(0.0f, 5f);
      this._width = 50f;
    }

    public override void LoadContent()
    {
      this._sprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "GoalIcons");
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      this._sprite.Draw(this._spritePosition, (int) Stage.CurrentStage.LevelStage._goal, spriteBatch);
    }
  }
}
