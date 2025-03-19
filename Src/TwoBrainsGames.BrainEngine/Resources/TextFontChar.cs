
// Type: TwoBrainsGames.BrainEngine.Resources.TextFontChar
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Data.DataFiles;


namespace TwoBrainsGames.BrainEngine.Resources
{
  public class TextFontChar : IDataFileSerializable
  {
    private char _charMap;
    private Rectangle _rect;
    private int _spacing;
    private int _spacingAfter;

    public char Character => this._charMap;

    public int Spacing => this._spacing;

    public int SpacingAfter => this._spacingAfter;

    public Rectangle Rectangle => this._rect;

    private TextFontChar()
    {
    }

    public TextFontChar(char ch, Rectangle rect)
    {
      this._charMap = ch;
      this._rect = rect;
    }

    public static TextFontChar FromDataFile(DataFileRecord record)
    {
      TextFontChar textFontChar = new TextFontChar();
      textFontChar.InitFromDataFileRecord(record);
      return textFontChar;
    }

    public void InitFromDataFileRecord(DataFileRecord record)
    {
      this._charMap = record.GetFieldValue<char>("CharMap");
      this._rect = new Rectangle(record.GetFieldValue<int>("Left"), record.GetFieldValue<int>("Top"), record.GetFieldValue<int>("Width"), record.GetFieldValue<int>("Height"));
      this._spacing = record.GetFieldValue<int>("Spacing", 0);
      this._spacingAfter = record.GetFieldValue<int>("SpacingAfter", 0);
    }

    public DataFileRecord ToDataFileRecord()
    {
      DataFileRecord dataFileRecord = new DataFileRecord("Character");
      dataFileRecord.AddField("CharMap", (object) this._charMap);
      dataFileRecord.AddField("Left", (object) this._rect.Left);
      dataFileRecord.AddField("Top", (object) this._rect.Top);
      dataFileRecord.AddField("Width", (object) this._rect.Width);
      dataFileRecord.AddField("Height", (object) this._rect.Height);
      dataFileRecord.AddField("Spacing", (object) this._spacing);
      dataFileRecord.AddField("SpacingAfter", (object) this._spacingAfter);
      return dataFileRecord;
    }
  }
}
