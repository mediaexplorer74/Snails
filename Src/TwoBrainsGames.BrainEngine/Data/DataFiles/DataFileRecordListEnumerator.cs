
// Type: TwoBrainsGames.BrainEngine.Data.DataFiles.DataFileRecordListEnumerator
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using System;
using System.Collections;
using System.Collections.Generic;


namespace TwoBrainsGames.BrainEngine.Data.DataFiles
{
  public class DataFileRecordListEnumerator : IEnumerator
  {
    private List<DataFileRecord> _Messages;
    private int position = -1;

    public DataFileRecordListEnumerator(List<DataFileRecord> list) => this._Messages = list;

    public bool MoveNext()
    {
      ++this.position;
      return this.position < this._Messages.Count;
    }

    public void Reset() => this.position = -1;

    object IEnumerator.Current => (object) this.Current;

    public DataFileRecord Current
    {
      get
      {
        try
        {
          return this._Messages[this.position];
        }
        catch (IndexOutOfRangeException ex)
        {
          throw new InvalidOperationException();
        }
      }
    }
  }
}
