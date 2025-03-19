
// Type: TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile.BinaryDataFileWriter
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using System.IO;
using System.IO.IsolatedStorage;
using TwoBrainsGames.BrainEngine.Secutiry;
using TwoBrainsGames.BrainEngine.UI;


namespace TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile
{
  public class BinaryDataFileWriter : IDataFileWriter
  {
    private BinaryWriter _Writer;

    public Stream ToStream(DataFile dataFile)
    {
      MemoryStream output = new MemoryStream();
      try
      {
        this._Writer = new BinaryWriter((Stream) output);
        this.WriteHeader(dataFile);
        this.WriteRecord(dataFile.RootRecord);
        output.Position = 0L;
        MemoryStream destination = new MemoryStream();
        output.CopyTo((Stream) destination);
        destination.Position = 0L;
        return (Stream) destination;
      }
      finally
      {
        if (this._Writer != null)
        {
            this._Writer.Flush();
            this._Writer.Dispose();//.Close();
        }

        output?.Flush();
        output?.Dispose();//output?.Close();
       }
    }

    public void Write(BinaryWriter sr, DataFile file) => this.Write(sr, file, (string) null);

    public void Write(BinaryWriter sr, DataFile file, string encryptionKey)
    {
      Stream stream = this.ToStream(file);
      if (string.IsNullOrEmpty(encryptionKey))
      {
        byte[] buffer = new byte[8192];
        int count;
        while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
          sr.Write(buffer, 0, count);
      }
      else
      {
        byte[] buffer = Encryption.Encrypt(stream, encryptionKey);
        sr.Write(buffer);
      }
    }

    public void Write(string filename, DataFile dataFile)
    {
      this.Write(filename, dataFile, (string) null);
    }

        public void Write(string filename, DataFile dataFile, string encryptionKey)
        {
            IsolatedStorageFileStream fileStream = null;
            Stream stream = null;
            try
            {
                IsolatedStorageFile storeForApplication = IsolatedStorageFile.GetUserStoreForApplication();
                fileStream = new IsolatedStorageFileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite,
                    storeForApplication);
                stream = this.ToStream(dataFile);
                if (string.IsNullOrEmpty(encryptionKey))
                {
                    byte[] buffer = new byte[8192];
                    int count;
                    while ((count = stream.Read(buffer, 0, buffer.Length)) > 0)
                        fileStream.Write(buffer, 0, count);
                }
                else
                {
                    byte[] buffer = Encryption.Encrypt(stream, encryptionKey);
                    fileStream.Write(buffer, 0, buffer.Length);
                }
            }
            finally
            {
                fileStream?.Flush();
                fileStream?.Dispose();
                stream?.Flush();
                stream?.Dispose();
            }
        }

    private void WriteHeader(DataFile dataFile)
    {
      this._Writer.Write(40961);
      this._Writer.Write(1);
      this._Writer.Write("BRAINEBINARYDATAFILE");
      this._Writer.Write(dataFile.RootRecord == null ? 0 : 1);
      this._Writer.Write(61441);
    }

    private void WriteRecord(DataFileRecord record)
    {
      if (record == null)
        return;
      this._Writer.Write(40962);
      this._Writer.Write(record.Name);
      this._Writer.Write(record.ChildRecords.Count);
      this._Writer.Write(record.Fields.Count);
      foreach (DataFileField field in record.Fields)
        this.WriteField(this._Writer, field);
      foreach (DataFileRecord childRecord in record.ChildRecords)
        this.WriteRecord(childRecord);
      this._Writer.Write(61442);
    }

    private void WriteField(BinaryWriter writer, DataFileField field)
    {
      this._Writer.Write(40963);
      this._Writer.Write(field.Name);
      this._Writer.Write((int) field.Type);
      switch (field.Type)
      {
        case FieldType.Null:
          this._Writer.Write(61443);
          break;
        case FieldType.Str:
          this._Writer.Write(field.Value.ToString());
          goto case FieldType.Null;
        case FieldType.Int:
          this._Writer.Write((int) field.Value);
          goto case FieldType.Null;
        case FieldType.Float:
          this._Writer.Write(Convert.ToDouble(field.Value));
          goto case FieldType.Null;
        case FieldType.Bool:
          this._Writer.Write((bool) field.Value);
          goto case FieldType.Null;
        case FieldType.Double:
          this._Writer.Write((double) field.Value);
          goto case FieldType.Null;
        case FieldType.Char:
          this._Writer.Write((char) field.Value);
          goto case FieldType.Null;
        case FieldType.Long:
          this._Writer.Write((long) field.Value);
          goto case FieldType.Null;
        case FieldType.XNAColor:
          Color color = (Color) field.Value;
          this._Writer.Write(color.R);
          this._Writer.Write(color.G);
          this._Writer.Write(color.B);
          this._Writer.Write(color.A);
          goto case FieldType.Null;
        case FieldType.Vector2:
          Vector2 vector2 = (Vector2) field.Value;
          this._Writer.Write(Convert.ToDouble(vector2.X));
          this._Writer.Write(Convert.ToDouble(vector2.Y));
          goto case FieldType.Null;
        case FieldType.Size:
          Size size = (Size) field.Value;
          this._Writer.Write(Convert.ToDouble(size.Width));
          this._Writer.Write(Convert.ToDouble(size.Height));
          goto case FieldType.Null;
        default:
          throw new DataFileFormatException("Unexpected FieldType '" + field.Type.ToString() + "'.");
      }
    }
  }
}
