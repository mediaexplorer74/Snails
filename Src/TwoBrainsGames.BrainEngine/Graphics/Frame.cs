
// Type: TwoBrainsGames.BrainEngine.Graphics.Frame
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Data.DataFiles;


namespace TwoBrainsGames.BrainEngine.Graphics
{
  public struct Frame : IDataFileSerializable
  {
    private float _rotation;
    public float _rotationInRads;
    public Vector2 _offset;
    public Vector2 _pivot;
    public Vector2 _offsetHorizFlipped;
    public Vector2 _pivotHorizFlipped;
    public int PlayTime;
    public Rectangle Rect;
    public BoundingSquare[] BoundingBoxes;
    public bool WithCollisionBox;

    public BoundingSquare BoundingBox => this.BoundingBoxes[0];

    public bool WithMultipleBoundingBoxes => this.BoundingBoxes.Length > 0;

    public int X
    {
      get => this.Rect.X;
      set => this.Rect.X = value;
    }

    public int Y
    {
      get => this.Rect.Y;
      set => this.Rect.Y = value;
    }

    public int Width
    {
      get => this.Rect.Width;
      set => this.Rect.Width = value;
    }

    public int Height
    {
      get => this.Rect.Height;
      set => this.Rect.Height = value;
    }

    public float Rotation
    {
      get => this._rotation;
      set
      {
        if ((double) this._rotation == (double) value)
          return;
        this._rotation = value;
        this._rotationInRads = MathHelper.ToRadians(this._rotation);
      }
    }

    public Vector2 Offset
    {
      get => this._offset;
      set => this._offset = value;
    }

    public static Frame FromDataFileRecord(DataFileRecord record)
    {
      Frame frame = new Frame();
      frame.InitFromDataFileRecord(record);
      return frame;
    }

    public void InitFromDataFileRecord(DataFileRecord record)
    {
      this.Rect = new Rectangle(record.GetFieldValue<int>("Left", this.Rect.Left), record.GetFieldValue<int>("Top", this.Rect.Top), record.GetFieldValue<int>("Width", this.Rect.Width), record.GetFieldValue<int>("Height", this.Rect.Height));
      this.Rotation = record.GetFieldValue<float>("Rotation", this._rotation);
      this._offset = new Vector2(record.GetFieldValue<float>("OffsetX", this._offset.X), record.GetFieldValue<float>("OffsetY", this._offset.Y));
      this._pivot = new Vector2(record.GetFieldValue<float>("PivotX", this._pivot.X), record.GetFieldValue<float>("PivotY", this._pivot.Y));
      this._offsetHorizFlipped = new Vector2((float) this.Rect.Width - this._offset.X, this._offset.Y);
      this._pivotHorizFlipped = new Vector2((float) this.Rect.Width - this._pivot.X, this._pivot.Y);
      this.PlayTime = record.GetFieldValue<int>("PlayTime", this.PlayTime);
      this.WithCollisionBox = false;
      DataFileRecordList dataFileRecordList = record.SelectRecords("ColisionZones\\BoundingBox");
      if (dataFileRecordList.Count <= 0)
        return;
      this.WithCollisionBox = true;
      this.BoundingBoxes = new BoundingSquare[dataFileRecordList.Count];
      for (int i = 0; i < dataFileRecordList.Count; ++i)
      {
        Vector2 ul = new Vector2((float) dataFileRecordList[i].GetFieldValue<int>("Left"), (float) dataFileRecordList[i].GetFieldValue<int>("Top"));
        this.BoundingBoxes[i] = new BoundingSquare(ul, (float) dataFileRecordList[i].GetFieldValue<int>("Width"), (float) dataFileRecordList[i].GetFieldValue<int>("Height"));
      }
    }

    public DataFileRecord ToDataFileRecord()
    {
      DataFileRecord dataFileRecord = new DataFileRecord(nameof (Frame));
      dataFileRecord.AddField("Left", (object) this.Rect.Left);
      dataFileRecord.AddField("Top", (object) this.Rect.Top);
      dataFileRecord.AddField("Width", (object) this.Rect.Width);
      dataFileRecord.AddField("Height", (object) this.Rect.Height);
      dataFileRecord.AddField("PlayTime", (object) this.PlayTime);
      if (this.WithCollisionBox)
      {
        DataFileRecord record = new DataFileRecord("ColisionZones");
        for (int index = 0; index < this.BoundingBoxes.Length; ++index)
          record.AddRecord(this.BoundingBoxes[index].ToDataFileRecord());
        dataFileRecord.AddRecord(record);
      }
      dataFileRecord.AddField("Rotation", (object) this._rotation);
      dataFileRecord.AddField("OffsetX", (object) this._offset.X);
      dataFileRecord.AddField("OffsetY", (object) this._offset.Y);
      dataFileRecord.AddField("PivotX", (object) this._pivot.X);
      dataFileRecord.AddField("PivotY", (object) this._pivot.Y);
      return dataFileRecord;
    }
  }
}
