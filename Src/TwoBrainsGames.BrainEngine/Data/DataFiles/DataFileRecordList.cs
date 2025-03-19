
// Type: TwoBrainsGames.BrainEngine.Data.DataFiles.DataFileRecordList
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using System.Collections;
using System.Collections.Generic;


namespace TwoBrainsGames.BrainEngine.Data.DataFiles
{
  public class DataFileRecordList : IEnumerable
  {
    private List<DataFileRecord> _Items;

    public DataFileRecord this[int i]
    {
      get => this._Items[i];
      set => this._Items[i] = value;
    }

    private List<DataFileRecord> Items
    {
      get => this._Items;
      set => this._Items = value;
    }

    public int Count => this.Items.Count;

    public DataFileRecordList() => this.Items = new List<DataFileRecord>();

    public void Add(DataFileRecord record) => this.Items.Add(record);

    public void Clear() => this.Items.Clear();

    public virtual DataFileRecord SelectRecord(string path)
    {
      PathManager pathManager = PathManager.Parse(path);
      foreach (DataFileRecord dataFileRecord1 in this.Items)
      {
        if (pathManager.HasSingleField && dataFileRecord1.Name == pathManager.LastField)
          return dataFileRecord1;
        if (dataFileRecord1.ChildRecords.Count > 0)
        {
          DataFileRecord dataFileRecord2 = dataFileRecord1.ChildRecords.SelectRecord(pathManager.RemoveFirst().Path);
          if (dataFileRecord2 != null)
            return dataFileRecord2;
        }
      }
      return (DataFileRecord) null;
    }

    public virtual DataFileRecordList SelectRecords(string path)
    {
      DataFileRecordList dataFileRecordList = new DataFileRecordList();
      PathManager pathManager = PathManager.Parse(path);
      if (pathManager.HasSingleField)
      {
        foreach (DataFileRecord record in this.Items)
        {
          if (record.Name == pathManager.LastField)
            dataFileRecordList.Add(record);
        }
      }
      else
      {
        foreach (DataFileRecord dataFileRecord in this.Items)
        {
          if (dataFileRecord.Name == pathManager.FirstField)
            return dataFileRecord.ChildRecords.SelectRecords(pathManager.RemoveFirst().Path);
        }
      }
      return dataFileRecordList;
    }

    public IEnumerator GetEnumerator()
    {
      return (IEnumerator) new DataFileRecordListEnumerator(this._Items);
    }
  }
}
