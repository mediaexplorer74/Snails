
// Type: TwoBrainsGames.Snails.StageObjects.SpriteAccessories.RocketAccessory
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;


namespace TwoBrainsGames.Snails.StageObjects.SpriteAccessories
{
  internal class RocketAccessory(Snail snail) : SnailSpriteAccessory(snail), ISnailSpriteAccessory
  {
    private Sprite _spriteWalkAir;
    private Sprite _spriteTurnDownAir;
    private Sprite _spriteTurnUpAir;
    private Sprite _spriteWalkWater;
    private Sprite _spriteTurnDownWater;
    private Sprite _spriteTurnUpWater;

    public override void LoadContent()
    {
      this.LoadContent("spriteset/snail-rocket");
      this._spriteWalkAir = this._spriteWalk;
      this._spriteTurnDownAir = this._spriteTurnDown;
      this._spriteTurnUpAir = this._spriteTurnUp;
      this._spriteWalkWater = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/snail-rocket", "SnailWalkBubbles");
      this._spriteTurnDownWater = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/snail-rocket", "SnailTurnDownBubbles");
      this._spriteTurnUpWater = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/snail-rocket", "SnailTurnUpBubbles");
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      int num = this.Visible ? 1 : 0;
    }

    public override void OnEnterLiquid() => this.UpdateActiveSprite();

    public override void OnExitLiquid() => this.UpdateActiveSprite();

    public override void UpdateActiveSprite()
    {
      if (this._snail.IsUnderLiquid)
      {
        this._spriteWalk = this._spriteWalkWater;
        this._spriteTurnDown = this._spriteTurnDownWater;
        this._spriteTurnUp = this._spriteTurnUpWater;
      }
      else
      {
        this._spriteWalk = this._spriteWalkAir;
        this._spriteTurnDown = this._spriteTurnDownAir;
        this._spriteTurnUp = this._spriteTurnUpAir;
      }
      base.UpdateActiveSprite();
    }

    public Prop CreateProp() => Prop.CreateRocket(this._snail.Position);
  }
}
