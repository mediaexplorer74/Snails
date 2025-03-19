
// Type: TwoBrainsGames.BrainEngine.Data.DataFiles.DataFileField
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using TwoBrainsGames.BrainEngine.UI;


namespace TwoBrainsGames.BrainEngine.Data.DataFiles
{
  public class DataFileField
  {
    private string _Name;
    private object _Value;

    public string Name
    {
      get => this._Name;
      set => this._Name = value;
    }

    public object Value
    {
      get => this._Value;
      set
      {
        this._Value = value == null || (object) value.GetType() == (object) typeof (string) || (object) value.GetType() == (object) typeof (int) || (object) value.GetType() == (object) typeof (float) || (object) value.GetType() == (object) typeof (bool) || (object) value.GetType() == (object) typeof (double) || (object) value.GetType() == (object) typeof (long) || (object) value.GetType() == (object) typeof (char) || (object) value.GetType() == (object) typeof (TimeSpan) || (object) value.GetType() == (object) typeof (Color) ? value : throw new Exception("Type '" + value.GetType().Name + "' not supported by DataFiles.");
      }
    }

    public FieldType Type
    {
      get
      {
        if (this.Value == null)
          return FieldType.Null;
        if ((object) this.Value.GetType() == (object) typeof (string))
          return FieldType.Str;
        if ((object) this.Value.GetType() == (object) typeof (int))
          return FieldType.Int;
        if ((object) this.Value.GetType() == (object) typeof (float))
          return FieldType.Float;
        if ((object) this.Value.GetType() == (object) typeof (bool))
          return FieldType.Bool;
        if ((object) this.Value.GetType() == (object) typeof (double))
          return FieldType.Double;
        if ((object) this.Value.GetType() == (object) typeof (long))
          return FieldType.Long;
        if ((object) this.Value.GetType() == (object) typeof (char))
          return FieldType.Char;
        if ((object) this.Value.GetType() == (object) typeof (TimeSpan))
          return FieldType.TimeSpan;
        if ((object) this.Value.GetType() == (object) typeof (Color))
          return FieldType.XNAColor;
        if ((object) this.Value.GetType() == (object) typeof (Vector2))
          return FieldType.Vector2;
        return (object) this.Value.GetType() == (object) typeof (Size) ? FieldType.Size : FieldType.NotSupported;
      }
    }

    public DataFileField(string name) => this.Name = name;

    public DataFileField(string name, object value)
    {
      this.Name = name;
      this.Value = value;
    }

    public override string ToString()
    {
      return "    Name: " + this.Name + ", Value: " + this.Value.ToString() + "\n";
    }
  }
}
