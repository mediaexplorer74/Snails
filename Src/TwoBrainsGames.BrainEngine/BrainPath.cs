
// Type: TwoBrainsGames.BrainEngine.BrainPath
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using System.IO;


namespace TwoBrainsGames.BrainEngine
{
  public class BrainPath
  {
    public static string GetDirectoryName(string path)
    {
      return Path.GetDirectoryName(BrainPath.NormalizePath(path));
    }

    public static string GetFileName(string path)
    {
      return Path.GetFileName(BrainPath.NormalizePath(path));
    }

    private static string NormalizePath(string path)
    {
      path = path.Replace('/', '\\');
      if (Path.DirectorySeparatorChar != '\\')
        path = path.Replace('\\', Path.DirectorySeparatorChar);
      return path;
    }
  }
}
