
// Type: TwoBrainsGames.BrainEngine.Graphics.Sprite
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Data.DataFiles;


namespace TwoBrainsGames.BrainEngine.Graphics
{
  public class Sprite : IDataFileSerializable
  {
    public const float SHADOW_LAYER_DEPTH = 0.1f;
    private Texture2D _texture;
    private Vector2 _offset;
    private int _width;
    private string _id;
    private int _height;
    private int _fps;
    private Frame[] _frames;
    private float _layerDepth;
    private bool _hasShadows;
    private Color _opacity;
    public BoundingSquare[] BoundingBoxes;
    public BoundingCircle[] _boundingSpheres;

    public int FrameCount => this._frames.Length;

    public BoundingSquare BoundingBox => this.BoundingBoxes[0];

    public string Id
    {
      get => this._id;
      set => this._id = value;
    }

    public Texture2D Texture
    {
      get => this._texture;
      set => this._texture = value;
    }

    public Vector2 Offset
    {
      get => this._offset;
      set => this._offset = value;
    }

    public float OffsetX
    {
      get => this._offset.X;
      set => this._offset.X = value;
    }

    public float OffsetY
    {
      get => this._offset.Y;
      set => this._offset.Y = value;
    }

    public int Width
    {
      get => this._width;
      set => this._width = value;
    }

    public int Height
    {
      get => this._height;
      set => this._height = value;
    }

    public int Fps
    {
      get => this._fps;
      set => this._fps = value;
    }

    public Frame[] Frames
    {
      get => this._frames;
      private set => this._frames = value;
    }

    public bool HasAnimations => this._frames.Length > 1 && this._fps != 0;

    public float LayerDepth
    {
      get => this._layerDepth;
      set => this._layerDepth = value;
    }

    public BoundingSquare BoundingSquare
    {
      get => new BoundingSquare(this.Offset, (float) this.Width, (float) this.Height);
    }

    public bool HasShadows
    {
      get => this._hasShadows;
      set => this._hasShadows = value;
    }

    public Color Opacity
    {
      get => this._opacity;
      set => this._opacity = value;
    }

    public bool WithMultipleBoundingBoxes => this.BoundingBoxes.Length > 0;

    public double TotalTime { get; private set; }

    public Sprite()
    {
      this._width = this._height = 0;
      this._offset.X = this._offset.Y = 0.0f;
      this._fps = 0;
      this._layerDepth = 0.0f;
      this._hasShadows = true;
      this._opacity = Color.White;
      this._frames = new Frame[0];
    }

    public Sprite(Sprite other)
    {
      this._texture = other._texture;
      this._offset = other._offset;
      this._width = other._width;
      this._height = other._height;
      this._fps = other._fps;
      this._layerDepth = other._layerDepth;
      this._frames = other._frames;
      this._hasShadows = other._hasShadows;
      this.BoundingBoxes = other.BoundingBoxes;
    }

    public override string ToString() => this._id != null ? this._id : base.ToString();

    public void SetFrames(Frame[] frames) => this._frames = frames;

    public Rectangle GetFrameBoundingBox(int frame)
    {
      return new Rectangle(0, 0, this._frames[frame].Width, this._frames[frame].Height);
    }

    public virtual void Update(GameTime gameTime)
    {
    }

    public virtual void Draw(
      Vector2 position,
      int frameNr,
      Color opacity,
      SpriteBatch spriteBatch)
    {
      this.Draw(position, frameNr, 0.0f, SpriteEffects.None, this._layerDepth, opacity, 1f, spriteBatch);
    }

    public virtual void Draw(Vector2 position, SpriteBatch spriteBatch)
    {
      this.Draw(position, 0, 0.0f, SpriteEffects.None, this.LayerDepth, Color.White, 1f, spriteBatch);
    }

    public virtual void Draw(Vector2 position, int frameNr, SpriteBatch spriteBatch)
    {
      this.Draw(position, frameNr, 0.0f, SpriteEffects.None, this.LayerDepth, Color.White, 1f, spriteBatch);
    }

    public virtual void Draw(
      Vector2 position,
      int frame,
      float rotation,
      SpriteEffects effect,
      SpriteBatch spriteBatch)
    {
      this.Draw(position, frame, rotation, effect, this._layerDepth, Color.White, 1f, spriteBatch);
    }

    public virtual void Draw(
      Vector2 position,
      int frame,
      float rotation,
      SpriteEffects effect,
      Vector2 origin,
      SpriteBatch spriteBatch)
    {
      this.Draw(position, frame, rotation, effect, this._layerDepth, Color.White, 1f, spriteBatch);
    }

    public virtual void Draw(
      Vector2 position,
      int frame,
      float rotation,
      Vector2 origin,
      float scaleX,
      float scaleY,
      Color opacity,
      SpriteBatch spriteBatch)
    {
      Rectangle destinationRectangle = new Rectangle((int) position.X, (int) position.Y, (int) ((double) this._frames[frame].Rect.Width * (double) scaleX), (int) ((double) this._frames[frame].Rect.Height * (double) scaleY));
      spriteBatch.Draw(this._texture, destinationRectangle, new Rectangle?(this._frames[frame].Rect), opacity, MathHelper.ToRadians(rotation), this._offset, SpriteEffects.None, 1f);
    }

    public virtual void Draw(Vector2 position, Rectangle rcSource, SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(this._texture, position, new Rectangle?(rcSource), Color.White);
    }

    public virtual void Draw(
      Vector2 position,
      Rectangle rcDest,
      float rotation,
      SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(this._texture, position, new Rectangle?(rcDest), Color.White, rotation, Vector2.Zero, 1f, SpriteEffects.None, 1f);
    }

    public virtual void Draw(
      Vector2 position,
      int frame,
      Rectangle rcDest,
      Color color,
      float rotation,
      SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(this._texture, rcDest, new Rectangle?(this.Frames[frame].Rect), color, MathHelper.ToRadians(rotation), this.Offset, SpriteEffects.None, 1f);
    }

    public virtual void Draw(
      Vector2 position,
      Rectangle rcDest,
      Color color,
      SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(this._texture, rcDest, color);
    }

    public virtual void Draw(Rectangle rcDest, int frame, Color color, SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(this._texture, rcDest, new Rectangle?(this._frames[frame].Rect), color);
    }

    public virtual void Draw(
      Vector2 position,
      int frame,
      float rotation,
      SpriteEffects effect,
      float layerDepth,
      Color opacity,
      float scale,
      SpriteBatch spriteBatch)
    {
      float radians = MathHelper.ToRadians(rotation);
      Vector2 position1 = new Vector2((float) (int) position.X, (float) (int) position.Y);
      if ((effect & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally)
      {
        Vector2 origin = new Vector2((float) this._frames[frame].Rect.Width - this.Offset.X, this.OffsetY);
        spriteBatch.Draw(this._texture, position1, new Rectangle?(this._frames[frame].Rect), opacity, radians, origin, scale, effect, layerDepth);
      }
      else
        spriteBatch.Draw(this._texture, position1, new Rectangle?(this._frames[frame].Rect), opacity, radians, this._offset, scale, effect, layerDepth);
    }

    public virtual void Draw(
      Rectangle source,
      Rectangle dest,
      Color opacity,
      SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(this._texture, dest, new Rectangle?(source), opacity);
    }

    public virtual void DrawNoRound(Vector2 position, int frame, SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(this._texture, position, new Rectangle?(this._frames[frame].Rect), Color.White, 0.0f, this._offset, 1f, SpriteEffects.None, 1f);
    }

    public virtual void Draw(
      Vector2 position,
      int frame,
      float rotation,
      SpriteEffects effect,
      Color opacity,
      float scale,
      SpriteBatch spriteBatch)
    {
      float radians = MathHelper.ToRadians(rotation);
      if ((effect & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally)
      {
        Vector2 origin = new Vector2((float) this._frames[frame].Rect.Width - this.Offset.X, this.OffsetY);
        spriteBatch.Draw(this._texture, position, new Rectangle?(this._frames[frame].Rect), opacity, radians, origin, scale, effect, 1f);
      }
      else
        spriteBatch.Draw(this._texture, position, new Rectangle?(this._frames[frame].Rect), opacity, radians, this._offset, scale, effect, 1f);
    }

    public void Draw(
      Vector2 position,
      Rectangle rect,
      float rotation,
      SpriteEffects effect,
      Vector2 pivot,
      Color opacity,
      SpriteBatch spriteBatch)
    {
      spriteBatch.Draw(this._texture, position, new Rectangle?(rect), opacity, rotation, pivot, 1f, effect, 1f);
    }

    public BoundingSquare FlipCollisionBox(SpriteEffects effects)
    {
      if (effects == SpriteEffects.None || (effects & SpriteEffects.FlipHorizontally) != SpriteEffects.FlipHorizontally)
        return this.BoundingBox;
      Vector2 ul = new Vector2(this.BoundingBox.Width - this.BoundingBox.LowerRight.X, this.BoundingBox.UpperLeft.Y);
      Vector2 lr = new Vector2(ul.X + this.BoundingBox.Width, this.BoundingBox.LowerRight.Y);
      return new BoundingSquare(ul, lr);
    }

    public static Sprite FromDataFileRecord(DataFileRecord record)
    {
      Sprite sprite = new Sprite();
      sprite.InitFromDataFileRecord(record);
      return sprite;
    }

    public OOBoundingBox TransformBoundingBox(
      BoundingSquare bs,
      Vector2 position,
      float rotation,
      SpriteEffects spriteEffect)
    {
      if ((spriteEffect & SpriteEffects.FlipHorizontally) != SpriteEffects.FlipHorizontally)
        return bs.Transform(rotation, position);
      BoundingSquare boundingSquare = bs;
      return new BoundingSquare(new Vector2(-boundingSquare.LowerRight.X, boundingSquare.UpperLeft.Y), boundingSquare.Width, boundingSquare.Height).Transform(rotation, position);
    }

    public DataFileRecord ToDataFileRecord() => this.ToDataFileRecord(0);

    public DataFileRecord ToDataFileRecord(int context)
    {
      DataFileRecord dataFileRecord = new DataFileRecord(nameof (Sprite));
      dataFileRecord.AddField("Id", (object) this._id);
      dataFileRecord.AddField("Fps", (object) this._fps);
      dataFileRecord.AddField("Width", (object) this._width);
      dataFileRecord.AddField("Height", (object) this._height);
      dataFileRecord.AddField("OffsetX", (object) this._offset.X);
      dataFileRecord.AddField("OffsetY", (object) this._offset.Y);
      DataFileRecord record1 = new DataFileRecord("Frames");
      if (this._id == "Leaf")
        this._id = "Leaf";
      for (int index1 = 0; index1 < this._frames.Length; ++index1)
      {
        if (this._frames[index1].WithCollisionBox)
        {
          for (int index2 = 0; index2 < this._frames[index1].BoundingBoxes.Length; ++index2)
            this._frames[index1].BoundingBoxes[index2] = this._frames[index1].BoundingBoxes[index2].Transform(this.Offset);
        }
        record1.AddRecord(this._frames[index1].ToDataFileRecord());
      }
      dataFileRecord.AddRecord(record1);
      DataFileRecord record2 = new DataFileRecord("ColisionZones");
      for (int index = 0; index < this.BoundingBoxes.Length; ++index)
        record2.AddRecord(this.BoundingBoxes[index].Transform(this.Offset).ToDataFileRecord());
      if (this._boundingSpheres != null)
      {
        for (int index = 0; index < this._boundingSpheres.Length; ++index)
          record2.AddRecord(this._boundingSpheres[index].Transform(this.Offset).ToDataFileRecord());
      }
      dataFileRecord.AddRecord(record2);
      return dataFileRecord;
    }

    public void InitFromDataFileRecord(DataFileRecord record)
    {
      this._id = record.GetFieldValue<string>("Id", this._id);
      if (this._id == "Leaf")
        this._id = "Leaf";
      this._fps = record.GetFieldValue<int>("Fps", this._fps);
      this._width = record.GetFieldValue<int>("Width", this._width);
      this._height = record.GetFieldValue<int>("Height", this._height);
      this._offset = new Vector2(record.GetFieldValue<float>("OffsetX", this.OffsetX), record.GetFieldValue<float>("OffsetY", this.OffsetY));
      DataFileRecordList dataFileRecordList1 = record.SelectRecords("Frames\\Frame");
      if (dataFileRecordList1.Count == 0)
      {
        this._frames = new Frame[1];
        this._frames[0] = new Frame()
        {
          Width = this._width,
          Height = this._height,
          X = 0,
          Y = 0,
          PlayTime = 0
        };
      }
      else
      {
        this._frames = new Frame[dataFileRecordList1.Count];
        this.Fps = record.GetFieldValue<int>("Fps", this._fps);
        for (int i = 0; i < dataFileRecordList1.Count; ++i)
        {
          Frame frame = Frame.FromDataFileRecord(dataFileRecordList1[i]);
          if (frame.WithCollisionBox)
          {
            for (int index = 0; index < frame.BoundingBoxes.Length; ++index)
              frame.BoundingBoxes[index] = frame.BoundingBoxes[index].Transform(-this.Offset);
          }
          this._frames[i] = frame;
          if (this._frames[i].PlayTime != 0)
            this.TotalTime += (double) this._frames[i].PlayTime;
          else
            this.TotalTime += this.Fps == 0 ? 0.0 : (double) (1000 / this.Fps);
        }
        this.Width = this._frames[0].Width;
        this.Height = this._frames[0].Height;
      }
      DataFileRecordList dataFileRecordList2 = record.SelectRecords("ColisionZones\\BoundingBox");
      if (dataFileRecordList2.Count > 0)
      {
        this.BoundingBoxes = new BoundingSquare[dataFileRecordList2.Count];
        for (int i = 0; i < dataFileRecordList2.Count; ++i)
        {
          Vector2 ul = new Vector2((float) dataFileRecordList2[i].GetFieldValue<int>("Left"), (float) dataFileRecordList2[i].GetFieldValue<int>("Top"));
          this.BoundingBoxes[i] = new BoundingSquare(ul, (float) dataFileRecordList2[i].GetFieldValue<int>("Width"), (float) dataFileRecordList2[i].GetFieldValue<int>("Height")).Transform(-this.Offset);
        }
      }
      else
      {
        this.BoundingBoxes = new BoundingSquare[1];
        this.BoundingBoxes[0] = new BoundingSquare(-this.Offset, (float) this.Frames[0].Rect.Width, (float) this.Frames[0].Rect.Height);
      }
      DataFileRecordList dataFileRecordList3 = record.SelectRecords("ColisionZones\\BoundingSphere");
      if (dataFileRecordList3.Count <= 0)
        return;
      this._boundingSpheres = new BoundingCircle[dataFileRecordList3.Count];
      for (int i = 0; i < dataFileRecordList3.Count; ++i)
        this._boundingSpheres[i] = BoundingCircle.FromDataFileRecord(dataFileRecordList3[i]).Transform(-this.Offset);
    }
  }
}
