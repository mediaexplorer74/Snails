
// Type: TwoBrainsGames.BrainEngine.Data.DataFiles.DataFileRecord
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using TwoBrainsGames.BrainEngine.UI;


namespace TwoBrainsGames.BrainEngine.Data.DataFiles
{
  public class DataFileRecord
  {
    private List<DataFileField> _Fields;
    private DataFileRecordList _ChildRecords;
    private DataFileRecord _Parent;
    private string _Name;

    private DataFileRecord Parent
    {
      get => this._Parent;
      set => this._Parent = value;
    }

    public List<DataFileField> Fields
    {
      get => this._Fields;
      private set => this._Fields = value;
    }

    public DataFileRecordList ChildRecords
    {
      get => this._ChildRecords;
      private set => this._ChildRecords = value;
    }

    public string Name
    {
      get => this._Name;
      set => this._Name = value;
    }

    private int Level
    {
      get
      {
        int level = 1;
        for (DataFileRecord parent = this.Parent; parent != null; parent = parent.Parent)
          ++level;
        return level;
      }
    }

    private int Ident => this.Level * 5;

    public DataFileField this[string fieldName]
    {
      set
      {
        for (int index = 0; index < this.Fields.Count; ++index)
        {
          if (this.Fields[index].Name == fieldName)
            this.Fields[index] = value;
        }
      }
      get => this.GetFieldByName(fieldName);
    }

    public DataFileRecord() => this.Fields = new List<DataFileField>();

    public DataFileRecord(string name)
    {
      this.CheckName(name);
      this._ChildRecords = new DataFileRecordList();
      this._Fields = new List<DataFileField>();
      this._Name = name;
    }

    public void AddRecord(DataFileRecord record)
    {
      record.Parent = this;
      this.ChildRecords.Add(record);
    }

    public DataFileRecord AddRecord(string name)
    {
      DataFileRecord record = new DataFileRecord(name);
      this.AddRecord(record);
      return record;
    }

    public void AddRecord(string name, string fieldName, object fieldValue)
    {
      DataFileRecord record = new DataFileRecord(name);
      this.AddRecord(record);
      record.AddField(fieldName, fieldValue);
    }

    public void AddField(DataFileField field) => this.Fields.Add(field);

    public void AddField(string name, object value)
    {
      this.AddField(new DataFileField(name, value));
    }

    public void AddField(string name, object value, bool update)
    {
      if (!update)
      {
        this.AddField(new DataFileField(name, value));
      }
      else
      {
        DataFileField fieldByName = this.GetFieldByName(name);
        if (fieldByName == null)
          this.AddField(new DataFileField(name, value));
        else
          fieldByName.Value = value;
      }
    }

    public void RemoveField(string name) => this._Fields.Remove(this[name]);

    public DataFileRecord SelectRecord(string path) => this.ChildRecords.SelectRecord(path);

    public DataFileRecordList SelectRecords(string path) => this.ChildRecords.SelectRecords(path);

    public DataFileRecord SelectRecordByField(string path, string fieldName, object fieldValue)
    {
      foreach (DataFileRecord selectRecord in this.ChildRecords.SelectRecords(path))
      {
        DataFileField fieldByName = selectRecord.GetFieldByName(fieldName);
        if (fieldByName != null && fieldValue is string && fieldByName.Value is string && (string) fieldByName.Value == (string) fieldValue)
          return selectRecord;
      }
      return (DataFileRecord) null;
    }

    public T GetChildRecordField<T>(string recordName, string fieldName, T defaultVal)
    {
      DataFileRecord dataFileRecord = this.SelectRecord(recordName);
      return dataFileRecord == null ? defaultVal : dataFileRecord.GetFieldValue<T>(fieldName, defaultVal);
    }

    public T GetFieldValue<T>(string fieldName, T defaultVal)
    {
      return this.GetFieldByName(fieldName) == null ? defaultVal : this.GetFieldValue<T>(fieldName);
    }

        public T GetFieldValue<T>(string fieldName)
        {
            DataFileField fieldByName = this.GetFieldByName(fieldName);
            if (fieldByName == null)
                return default(T);
            if (fieldByName.Value == null)
                return default(T);
            if (typeof(T) == fieldByName.Value.GetType())
                return (T)fieldByName.Value;
            if (typeof(T) == typeof(string))
                return (T)(object)fieldByName.Value.ToString();
            if (typeof(T) == typeof(int))
                return (T)(object)this.GetIntValue(fieldByName);
            if (typeof(T) == typeof(long))
                return (T)(object)this.GetLongValue(fieldByName);
            if (typeof(T) == typeof(float))
                return (T)(object)this.GetFloatValue(fieldByName);
            if (typeof(T) == typeof(bool))
                return (T)(object)this.GetBoolValue(fieldByName);
            if (typeof(T) == typeof(char))
                return (T)(object)this.GetCharValue(fieldByName);
            if (typeof(T) == typeof(TimeSpan))
                return (T)(object)this.GetTimeSpanValue(fieldByName);
            if (typeof(T) == typeof(double))
                return (T)(object)this.GetDoubleValue(fieldByName);
            if (typeof(T) == typeof(FlagsType))
                return (T)(object)this.GetFlagsTypeValue(fieldByName);
            if (typeof(T) == typeof(Color))
                return (T)(object)this.GetColorValue(fieldByName);
            if (typeof(T) == typeof(Vector2))
                return (T)(object)this.GetVector2Value(fieldByName);
            if (typeof(T) == typeof(Size))
            {
                Vector2 vector2Value = this.GetVector2Value(fieldByName);
                return (T)(object)new Size(vector2Value.X, vector2Value.Y);
            }
            return default(T);
        }

    private void CheckName(string name)
    {
      if (name.Contains("\\") || name.Contains("//") || name.Contains(">") || name.Contains("<"))
        throw new DataFileFormatException("Record name cannot contain characters '\\', '/', '>' or '<'.");
    }

    public DataFileField GetFieldByName(string name)
    {
      foreach (DataFileField field in this.Fields)
      {
        if (field.Name == name)
          return field;
      }
      return (DataFileField) null;
    }

    private int GetIntValue(DataFileField field)
    {
      if ((object) field.Value.GetType() == (object) typeof (int))
        return (int) field.Value;
      return (object) field.Value.GetType() == (object) typeof (string) || (object) field.Value.GetType() == (object) typeof (float) || (object) field.Value.GetType() == (object) typeof (double) || (object) field.Value.GetType() == (object) typeof (bool) ? Convert.ToInt32(field.Value) : throw new DataFileFormatException("Invalid DataType while converting Field value to int.");
    }

    private long GetLongValue(DataFileField field)
    {
      if ((object) field.Value.GetType() == (object) typeof (long))
        return (long) field.Value;
      return (object) field.Value.GetType() == (object) typeof (string) || (object) field.Value.GetType() == (object) typeof (int) || (object) field.Value.GetType() == (object) typeof (float) || (object) field.Value.GetType() == (object) typeof (double) || (object) field.Value.GetType() == (object) typeof (bool) ? Convert.ToInt64(field.Value) : throw new DataFileFormatException("Invalid DataType while converting Field value to long.");
    }

    private float GetFloatValue(DataFileField field)
    {
      if ((object) field.Value.GetType() == (object) typeof (float))
        return (float) field.Value;
      if ((object) field.Value.GetType() == (object) typeof (double))
        return (float) Convert.ToDecimal(field.Value);
      if ((object) field.Value.GetType() == (object) typeof (string))
        return this.ParseFloat(field.Value.ToString());
      return (object) field.Value.GetType() == (object) typeof (int) || (object) field.Value.GetType() == (object) typeof (bool) ? (float) Convert.ToDouble(field.Value) : throw new DataFileFormatException("Invalid DataType while converting Field value to float.");
    }

    private float ParseFloat(string floatString)
    {
      return (float) Convert.ToDouble(floatString.Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator).Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator), (IFormatProvider) CultureInfo.CurrentCulture);
    }

    private double GetDoubleValue(DataFileField field)
    {
      if ((object) field.Value.GetType() == (object) typeof (double))
        return (double) field.Value;
      if ((object) field.Value.GetType() == (object) typeof (float))
        return (double) Convert.ToDecimal(field.Value);
      if ((object) field.Value.GetType() == (object) typeof (string))
        return Convert.ToDouble(field.Value.ToString().Replace(",", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator).Replace(".", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator), (IFormatProvider) CultureInfo.CurrentCulture);
      return (object) field.Value.GetType() == (object) typeof (int) || (object) field.Value.GetType() == (object) typeof (bool) ? Convert.ToDouble(field.Value) : throw new DataFileFormatException("Invalid DataType while converting Field value to double.");
    }

    private FlagsType GetFlagsTypeValue(DataFileField field)
    {
      if (field.Value == null)
        return FlagsType.Zero;
      string[] strArray = field.Value.ToString().Split('|');
      FlagsType flagsTypeValue = new FlagsType();
      foreach (string s in strArray)
      {
        int result;
        if (!int.TryParse(s, out result))
          throw new DataFileFormatException("Invalid DataType while converting Field value to FlagsType. Expecting ints separated by '|', received " + (object) strArray);
        flagsTypeValue.Value += result;
      }
      return flagsTypeValue;
    }

    private bool GetBoolValue(DataFileField field)
    {
      if ((object) field.Value.GetType() == (object) typeof (bool))
        return (bool) field.Value;
      if ((object) field.Value.GetType() == (object) typeof (float) || (object) field.Value.GetType() == (object) typeof (int))
        return Convert.ToBoolean(field.Value);
      if ((object) field.Value.GetType() != (object) typeof (string))
        throw new DataFileFormatException("Invalid DataType while converting Field value to bool.");
      bool result;
      bool.TryParse(field.Value.ToString(), out result);
      return result;
    }

    private char GetCharValue(DataFileField field)
    {
      if ((object) field.Value.GetType() == (object) typeof (char))
        return (char) field.Value;
      return (object) field.Value.GetType() == (object) typeof (float) || (object) field.Value.GetType() == (object) typeof (double) || (object) field.Value.GetType() == (object) typeof (int) || (object) field.Value.GetType() == (object) typeof (string) ? Convert.ToChar(field.Value) : throw new DataFileFormatException("Invalid DataType while converting Field value to bool.");
    }

    private Color GetColorValue(DataFileField field)
    {
      if ((object) field.Value.GetType() == (object) typeof (Color))
        return (Color) field.Value;
      return (object) field.Value.GetType() == (object) typeof (string) ? this.ParseColor(field.Value.ToString()) : throw new DataFileFormatException("Invalid DataType while converting Field value to TimeSpan.");
    }

    private Color ParseColor(string colorString) => Parsers.ParseColor(colorString);

    private Vector2 GetVector2Value(DataFileField field)
    {
      if ((object) field.Value.GetType() == (object) typeof (Vector2))
        return (Vector2) field.Value;
      return (object) field.Value.GetType() == (object) typeof (string) ? this.ParseVector2(field.Value.ToString()) : throw new DataFileFormatException("Invalid DataType while converting Field value to TimeSpan.");
    }

    private Vector2 ParseVector2(string vector2String)
    {
      string[] strArray = vector2String.Split(',');
      float x = 0.0f;
      if (strArray.Length > 0)
        x = this.ParseFloat(strArray[0]);
      float y = 0.0f;
      if (strArray.Length > 1)
        y = this.ParseFloat(strArray[1]);
      return new Vector2(x, y);
    }

    private TimeSpan GetTimeSpanValue(DataFileField field)
    {
      if ((object) field.Value.GetType() == (object) typeof (TimeSpan))
        return (TimeSpan) field.Value;
      return (object) field.Value.GetType() == (object) typeof (string) ? TimeSpan.Parse((string) field.Value) : throw new Exception("Invalid DataType while converting Field value to TimeSpan.");
    }

    public override string ToString()
    {
      string str = "" + "RECORD\n" + "  Name  : " + this.Name + "\n" + "  Parent: " + (this.Parent == null ? "ROOT" : this.Parent.Name) + "\n" + "  FIELDS\n";
      foreach (DataFileField field in this.Fields)
        str += field.ToString();
      foreach (DataFileRecord childRecord in this.ChildRecords)
        str += childRecord.ToString();
      return str;
    }
  }
}
