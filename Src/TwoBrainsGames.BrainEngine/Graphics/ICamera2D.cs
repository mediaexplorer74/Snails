
// Type: TwoBrainsGames.BrainEngine.Graphics.ICamera2D
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;


namespace TwoBrainsGames.BrainEngine.Graphics
{
  public interface ICamera2D
  {
    Vector2 Position { get; set; }

    float MoveSpeed { get; set; }

    float Rotation { get; set; }

    Vector2 Origin { get; }

    Vector2 Scale { get; }

    Matrix Transform { get; }
  }
}
