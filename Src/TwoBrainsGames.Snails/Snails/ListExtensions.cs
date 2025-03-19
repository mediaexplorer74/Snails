
// Type: TwoBrainsGames.Snails.ListExtensions
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using System;
using System.Collections.Generic;


namespace TwoBrainsGames.Snails
{
  public static class ListExtensions
  {
    public static int RemoveAll<T>(this List<T> list, Func<T, bool> match)
    {
      int num = 0;
      for (int index = list.Count - 1; index >= 0; --index)
      {
        if (match(list[index]))
        {
          list.RemoveAt(index);
          ++num;
        }
      }
      return num;
    }

    public static bool Exists<T>(this List<T> list, Func<T, bool> match)
    {
      for (int index = 0; index < list.Count; ++index)
      {
        if (match(list[index]))
          return true;
      }
      return false;
    }

    public static T Find<T>(this List<T> list, Func<T, bool> match)
    {
      if (list == null)
        return default (T);
      foreach (T obj in list)
      {
        if (match(obj))
          return obj;
      }
      return default (T);
    }
  }
}
