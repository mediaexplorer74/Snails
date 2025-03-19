
// Type: TwoBrainsGames.Snails.Stages.HUD.HUDItemSnailsCounter
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;


namespace TwoBrainsGames.Snails.Stages.HUD
{
  internal class HUDItemSnailsCounter : HUDItem
  {
    private Sprite _sprite;
    private Vector2 _spritePosition;
    private Vector2 _stringPosition;
    private string _text;

    public override void Initialize(Vector2 position)
    {
      base.Initialize(position);
      this._spritePosition = position + new Vector2(0.0f, 10f);
      this._stringPosition = position + new Vector2(45f, 10f);
      this.UpdateText();
      this._width = 85f;
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this._sprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "SnailIcon");
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      this._sprite.Draw(this._spritePosition, spriteBatch);
      this._font.DrawString(spriteBatch, this._text, this._stringPosition, new Vector2(1f, 1f), Colors.StageHUDInfoColor);
    }

    private void UpdateText() => this._text = Stage.CurrentStage.Stats.TotalSnails.ToString();

    public override void SnailsStageStatsChanged() => this.UpdateText();
  }
}
