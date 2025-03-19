
// Type: TwoBrainsGames.BrainEngine.Parsers
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;


namespace TwoBrainsGames.BrainEngine
{
  public class Parsers
  {
    public static Color ParseColor(string colorString)
    {
      colorString = !string.IsNullOrEmpty(colorString) ? colorString.Replace("{", "") : throw new BrainException("Error parsing color, colorString cannot be null or empty.");
      colorString = colorString.Replace("}", "");
      int[] numArray = new int[4];
      string[] strArray1 = colorString.Split(' ');
      for (int index = 0; index < 4; ++index)
      {
        string[] strArray2 = strArray1[index].Split(':');
        numArray[index] = Convert.ToInt32(strArray2[1]);
      }
      return new Color(numArray[0], numArray[1], numArray[2], numArray[3]);
    }
  }
}
