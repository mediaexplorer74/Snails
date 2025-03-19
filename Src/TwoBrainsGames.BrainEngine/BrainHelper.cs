
// Type: TwoBrainsGames.BrainEngine.BrainHelper
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;


namespace TwoBrainsGames.BrainEngine
{
  public static class BrainHelper
  {
    public static float FindAngleBetweenTwoVectors(Vector2 v1, Vector2 v2)
    {
      v1.Normalize();
      v2.Normalize();
      float num = (float) Math.Acos((double) Vector2.Dot(v1, v2));
      return (double) Math.Abs(num) < 0.0001 ? 0.0f : num * (float) BrainHelper.signal(v1, v2);
    }

    private static int signal(Vector2 v1, Vector2 v2)
    {
      return (double) v1.Y * (double) v2.X - (double) v2.Y * (double) v1.X <= 0.0 ? -1 : 1;
    }
  }
}
