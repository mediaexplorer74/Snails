
// Type: TwoBrainsGames.Snails.StageObjects.SpriteAccessories.HelmetAccessory
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer


namespace TwoBrainsGames.Snails.StageObjects.SpriteAccessories
{
  internal class HelmetAccessory(Snail snail) : SnailSpriteAccessory(snail), ISnailSpriteAccessory
  {
    public override void LoadContent() => this.LoadContent("spriteset/snail-helmet");

    public Prop CreateProp() => Prop.CreateHelmet(this._snail.Position);
  }
}
