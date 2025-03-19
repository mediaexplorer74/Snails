
// Type: TwoBrainsGames.Snails.StageObjects.SnailKing
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.Snails.StageObjects.SpriteAccessories;


namespace TwoBrainsGames.Snails.StageObjects
{
  internal class SnailKing : Snail
  {
    private SnailSpriteAccessory _crownAccessory;

    public SnailKing()
      : base(StageObjectType.SnailKing)
    {
    }

    public override void Copy(StageObject other) => base.Copy(other);

    public override void Initialize()
    {
      base.Initialize();
      this.AddAccessory(this._crownAccessory = (SnailSpriteAccessory) new CrownAccessory((Snail) this));
      this._crownAccessory.Visible = true;
    }
  }
}
