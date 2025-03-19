
// Type: TwoBrainsGames.BrainEngine.UI.Size
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;


namespace TwoBrainsGames.BrainEngine.UI
{
  public struct Size
  {
    public float Width;
    public float Height;

    public Size(float width, float height)
    {
      this.Width = width;
      this.Height = height;
    }

    public Size(Vector2 vector)
    {
      this.Width = vector.X;
      this.Height = vector.Y;
    }

    public static Size Zero => new Size(0.0f, 0.0f);

    public override string ToString()
    {
      return string.Format("{0},{1}", (object) this.Width, (object) this.Height);
    }
  }
}
