
// Type: TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile.BinaryDataFileReader
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System.IO;
using System.IO.IsolatedStorage;
using TwoBrainsGames.BrainEngine.Secutiry;


namespace TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile
{
  public class BinaryDataFileReader : IDataFileReader
  {
    private BinaryReader _Reader;
    private bool _WithRootRecord;

    public DataFile Read(Stream stream) => this.Read(stream, (string) null);

    public DataFile Read(Stream stream, string encryptionKey)
    {
      this._WithRootRecord = false;
      DataFile dataFile = new DataFile();
      this._Reader = !string.IsNullOrEmpty(encryptionKey) ? new BinaryReader((Stream) new MemoryStream(Encryption.Decrypt(stream, encryptionKey))) : new BinaryReader(stream);
      this.ReadHeader();
      if (this._WithRootRecord)
        dataFile.RootRecord = this.ReadRecord();
      return dataFile;
    }

    public DataFile Read(string filename) => this.Read(filename, (string) null);

    public DataFile Read(string filename, string encryptionKey)
    {
      this._WithRootRecord = false;
      DataFile dataFile = new DataFile();
      IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication();
      IsolatedStorageFileStream fileStream = new IsolatedStorageFileStream(filename, FileMode.Open, storeForApplication);
      try
      {
        if (fileStream != null)
        {
          this._Reader = !string.IsNullOrEmpty(encryptionKey) ? new BinaryReader((Stream) new MemoryStream(Encryption.Decrypt((Stream) fileStream, encryptionKey))) : new BinaryReader((Stream) fileStream);
          this.ReadHeader();
          if (this._WithRootRecord)
            dataFile.RootRecord = this.ReadRecord();
        }
        return dataFile;
      }
      finally
      {
        if (this._Reader != null)
        {
            this._Reader.Dispose();
        }
     
        fileStream?.Dispose();
      }
    }

    private DataFileRecord ReadRecord()
    {
      string name = this._Reader.ReadInt32() == 40962 ? this._Reader.ReadString() : throw new DataFileFormatException();
      int num1 = this._Reader.ReadInt32();
      int num2 = this._Reader.ReadInt32();
      DataFileRecord dataFileRecord = new DataFileRecord(name);
      for (int index = 0; index < num2; ++index)
        dataFileRecord.AddField(this.ReadField());
      for (int index = 0; index < num1; ++index)
        dataFileRecord.AddRecord(this.ReadRecord());
      if (this._Reader.ReadInt32() != 61442)
        throw new DataFileFormatException();
      return dataFileRecord;
    }

    private DataFileField ReadField()
    {
      string name = this._Reader.ReadInt32() == 40963 ? this._Reader.ReadString() : throw new DataFileFormatException();
      FieldType fieldType = (FieldType) this._Reader.ReadInt32();
      object obj = (object) null;
      switch (fieldType)
      {
        case FieldType.Str:
          obj = (object) this._Reader.ReadString();
          break;
        case FieldType.Int:
          obj = (object) this._Reader.ReadInt32();
          break;
        case FieldType.Float:
        case FieldType.Double:
          obj = (object) this._Reader.ReadDouble();
          break;
        case FieldType.Bool:
          obj = (object) this._Reader.ReadBoolean();
          break;
        case FieldType.Char:
          obj = (object) this._Reader.ReadChar();
          break;
        case FieldType.Long:
          obj = (object) this._Reader.ReadInt64();
          break;
        case FieldType.XNAColor:
          obj = (object) new Color((int) this._Reader.ReadByte(), (int) this._Reader.ReadByte(), (int) this._Reader.ReadByte(), (int) this._Reader.ReadByte());
          break;
      }
      if (this._Reader.ReadInt32() != 61443)
        throw new DataFileFormatException();
      return new DataFileField(name, obj);
    }

    private void ReadHeader()
    {
      if (this._Reader.ReadInt32() != 40961)
        throw new DataFileFormatException();
      this._Reader.ReadInt32();
      if (this._Reader.ReadString() != "BRAINEBINARYDATAFILE")
        throw new DataFileFormatException();
      this._WithRootRecord = this._Reader.ReadInt32() == 1;
      if (this._Reader.ReadInt32() != 61441)
        throw new DataFileFormatException();
    }
  }
}
