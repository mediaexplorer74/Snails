
// Type: TwoBrainsGames.Snails.StageObjects.Prop
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class Prop : MovingObject, ISnailsDataFileSerializable, IDataFileSerializable
  {
    public Prop()
      : base(StageObjectType.Prop)
    {
    }

    public Prop(Prop other)
      : base((StageObject) other)
    {
      this.Copy((StageObject) other);
    }

    public static Prop CreateProp(string spriteSet, string spriteName, Vector2 position)
    {
      Prop prop = new Prop();
      prop.LoadContent();
      prop.Initialize();
      prop.Sprite = BrainGame.ResourceManager.GetSpriteTemporary(spriteSet, spriteName);
      prop.Position = position;
      return prop;
    }

    public static Prop CreateRocket(Vector2 position)
    {
      return Prop.CreateProp("spriteset/snail-props", "Rocket", position);
    }

    public static Prop CreateHelmet(Vector2 position)
    {
      return Prop.CreateProp("spriteset/snail-props", "Helmet", position);
    }

    public static Prop CreateCrown(Vector2 position)
    {
      return Prop.CreateProp("spriteset/snail-props", "Crown", position);
    }

    public static Prop CreateKingsCape(Vector2 position)
    {
      return Prop.CreateProp("spriteset/snail-props", "KingsCape", position);
    }

    public static Prop CreateShell(Vector2 position)
    {
      return Prop.CreateProp("spriteset/snail-props", "Shell", position);
    }
  }
}
