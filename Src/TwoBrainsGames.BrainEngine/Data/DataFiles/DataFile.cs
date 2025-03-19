
// Type: TwoBrainsGames.BrainEngine.Data.DataFiles.DataFile
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile;


namespace TwoBrainsGames.BrainEngine.Data.DataFiles
{
  public class DataFile
  {
    private DataFileRecord _RootRecord;

    public DataFileRecord RootRecord
    {
      get => this._RootRecord;
      set
      {
        this._RootRecord = value != null ? value : throw new BrainException("Root node cannot be null.");
      }
    }

    public DataFile() => this.Clear();

    public static IDataFileReader CreateReader(string assemblyName, string typeName)
    {
      return (IDataFileReader) new BinaryDataFileReader();
    }

    public static IDataFileWriter CreateWriter(string assemblyName, string typeName)
    {
      return (IDataFileWriter) new BinaryDataFileWriter();
    }

    public virtual DataFileRecord SelectRecord(string path)
    {
      if (this.RootRecord == null)
        return (DataFileRecord) null;
      PathManager pathManager = PathManager.Parse(path);
      return this.RootRecord.Name != pathManager.FirstField ? (DataFileRecord) null : this.RootRecord.ChildRecords.SelectRecord(pathManager.RemoveFirst().Path);
    }

    public virtual DataFileRecordList SelectRecords(string path)
    {
      if (this.RootRecord == null)
        return (DataFileRecordList) null;
      PathManager pathManager = PathManager.Parse(path);
      return this.RootRecord.Name != pathManager.FirstField ? (DataFileRecordList) null : this.RootRecord.ChildRecords.SelectRecords(pathManager.RemoveFirst().Path);
    }

    public void Clear() => this.RootRecord = new DataFileRecord("Root");

    public override string ToString()
    {
      string str = "";
      if (this.RootRecord != null)
        str += this.RootRecord.ToString();
      return str;
    }
  }
}
