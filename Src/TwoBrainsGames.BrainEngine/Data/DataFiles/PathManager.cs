
// Type: TwoBrainsGames.BrainEngine.Data.DataFiles.PathManager
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer


namespace TwoBrainsGames.BrainEngine.Data.DataFiles
{
  internal class PathManager
  {
    private string[] _PathFields;

    public bool HasSingleField => this._PathFields.Length == 1;

    public string LastField
    {
      get
      {
        return this._PathFields.Length <= 0 ? (string) null : this._PathFields[this._PathFields.Length - 1];
      }
    }

    public string FirstField => this._PathFields.Length == 0 ? (string) null : this._PathFields[0];

    public string Path
    {
      get
      {
        string path = "";
        for (int index = 0; index < this._PathFields.Length; ++index)
        {
          path += this._PathFields[index];
          if (index < this._PathFields.Length - 1)
            path += (string) (object) '\\';
        }
        return path;
      }
    }

    public static PathManager Parse(string path)
    {
      return new PathManager()
      {
        _PathFields = path.Split('\\')
      };
    }

    public PathManager RemoveFirst()
    {
      string path = "";
      for (int index = 1; index < this._PathFields.Length; ++index)
      {
        path += this._PathFields[index];
        if (index < this._PathFields.Length - 1)
          path += (string) (object) '\\';
      }
      return PathManager.Parse(path);
    }
  }
}
