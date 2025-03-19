
// Type: TwoBrainsGames.Snails.Player.PlayersProfileManagerWP7
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using System.IO.IsolatedStorage;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile;


namespace TwoBrainsGames.Snails.Player
{
  internal class PlayersProfileManagerWP7 : PlayersProfileManager
  {
    public override void Save()
    {
      new BinaryDataFileWriter().Write(this.SAVE_FILENAME, this.GetDataFileToSave());
    }

    public override void BeginLoad() => this.Load();

    public override void Load()
    {
      IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication();
      IDataFileReader dataFileReader = (IDataFileReader) new BinaryDataFileReader();
      if (storeForApplication.FileExists(this.SAVE_FILENAME))
        this.LoadDataFile(dataFileReader.Read(this.SAVE_FILENAME));
      this.LoadProfile();
    }
  }
}
