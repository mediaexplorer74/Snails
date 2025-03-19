
// Type: TwoBrainsGames.Snails.Stages.HUD.HUDItemSnailsDelivered
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;


namespace TwoBrainsGames.Snails.Stages.HUD
{
  internal class HUDItemSnailsDelivered : HUDItem
  {
    private Sprite _sprite;
    private Vector2 _spritePosition;
    private Vector2 _stringPosition;
    private string _deliveredString;
    private int _frameNr;

    public override void Initialize(Vector2 position)
    {
      base.Initialize(position);
      this._spritePosition = position + new Vector2(0.0f, 5f);
      this._stringPosition = position + new Vector2(50f, 10f);
      this.UpdateString();
      this._width = 120f;
      this._frameNr = (int) Stage.CurrentStage.LevelStage.ThemeId;
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this._sprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "DeliveredIcon");
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      this._sprite.Draw(this._spritePosition, this._frameNr, spriteBatch);
      this._font.DrawString(spriteBatch, this._deliveredString, this._stringPosition, new Vector2(1f, 1f), Colors.StageHUDInfoColor);
    }

    private void UpdateString()
    {
      this._deliveredString = string.Format("{0}/{1}", (object) Stage.CurrentStage.Stats.NumSnailsSafe, (object) Stage.CurrentStage.Stats.NumSnailsToSave);
    }

    public override void SnailsStageStatsChanged() => this.UpdateString();
  }
}
