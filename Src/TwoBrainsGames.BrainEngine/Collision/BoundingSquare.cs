
// Type: TwoBrainsGames.BrainEngine.Collision.BoundingSquare
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Data.DataFiles;


namespace TwoBrainsGames.BrainEngine.Collision
{
  public struct BoundingSquare : IDataFileSerializable
  {
    public Vector2 UpperLeft;
    public Vector2 LowerRight;
    public Vector2 LowerLeft;
    public Vector2 UpperRight;
    public BoundingBox Rectangle;

    public float Width => this.LowerRight.X - this.LowerLeft.X;

    public float Height => this.LowerRight.Y - this.UpperLeft.Y;

    public float Left => this.UpperLeft.X;

    public float Right => this.LowerRight.X;

    public float Top => this.UpperLeft.Y;

    public float Bottom => this.LowerRight.Y;

    public Vector2 Center => this.UpperLeft + new Vector2(this.Width / 2f, this.Height / 2f);

    public BoundingSquare(Vector2 ul, Vector2 lr)
    {
      this.UpperLeft = ul;
      this.LowerRight = lr;
      this.LowerLeft = new Vector2(ul.X, lr.Y);
      this.UpperRight = new Vector2(lr.X, ul.Y);
      this.Rectangle = new BoundingBox();
      this.UpdateRect();
    }

    public BoundingSquare(Microsoft.Xna.Framework.Rectangle rect)
      : this(new Vector2((float) rect.X, (float) rect.Y), new Vector2((float) (rect.X + rect.Width), (float) (rect.Y + rect.Height)))
    {
    }

    public BoundingSquare(Vector2 ul, float width, float height)
      : this(ul, new Vector2(ul.X + width, ul.Y + height))
    {
    }

    public Microsoft.Xna.Framework.Rectangle ToRect()
    {
      return new Microsoft.Xna.Framework.Rectangle((int) this.UpperLeft.X, (int) this.UpperLeft.Y, (int) this.Width, (int) this.Height);
    }

    public void Draw(Color color, Vector2 camPos)
    {
      BrainGame.DrawLine(this.UpperLeft - camPos, this.UpperRight - camPos, color);
      BrainGame.DrawLine(this.UpperRight - camPos, this.LowerRight - camPos, color);
      BrainGame.DrawLine(this.LowerRight - camPos, this.LowerLeft - camPos, color);
      BrainGame.DrawLine(this.LowerLeft - camPos, this.UpperLeft - camPos, color);
    }

    public void Draw(SpriteBatch spriteBatch, Color color, Vector2 camPos)
    {
      Microsoft.Xna.Framework.Rectangle rect = this.ToRect();
      rect.X -= (int) camPos.X;
      rect.Y -= (int) camPos.Y;
      BrainGame.DrawRectangleFrame(spriteBatch, rect, color, 1);
    }

    public OOBoundingBox Transform(Matrix mat)
    {
      return new OOBoundingBox(Vector2.Transform(this.UpperLeft, mat), Vector2.Transform(this.UpperRight, mat), Vector2.Transform(this.LowerRight, mat), Vector2.Transform(this.LowerLeft, mat));
    }

    public OOBoundingBox Transform(Vector2 position, float rotation)
    {
      Matrix rotationZ = Matrix.CreateRotationZ(MathHelper.ToRadians(rotation));
      return this.Transform(Matrix.CreateTranslation(new Vector3(position.X, position.Y, 0.0f)) * rotationZ);
    }

    public void TransformInPlace(Vector2 translation)
    {
      this.UpperLeft += translation;
      this.UpperRight += translation;
      this.LowerLeft += translation;
      this.LowerRight += translation;
      this.UpdateRect();
    }

    private void UpdateRect()
    {
      this.Rectangle = new BoundingBox(new Vector3(this.UpperLeft.X, this.UpperLeft.Y, 0.0f), new Vector3(this.LowerRight.X, this.LowerRight.Y, 0.0f));
    }

    public OOBoundingBox Transform(float rotation, Vector2 position)
    {
      return (double) rotation != 0.0 ? this.Transform(Matrix.CreateRotationZ(MathHelper.ToRadians(rotation)) * Matrix.CreateTranslation(new Vector3(position.X, position.Y, 0.0f))) : this.Transform(Matrix.CreateTranslation(new Vector3(position.X, position.Y, 0.0f)));
    }

    public BoundingCircle GetContainedCircle()
    {
      return (double) this.Height < (double) this.Width ? new BoundingCircle(this.Center, this.Height / 2f) : new BoundingCircle(this.Center, this.Width / 2f);
    }

    public BoundingSquare Transform(Vector2 position)
    {
      return new BoundingSquare(this.UpperLeft + position, this.Width, this.Height);
    }

    public bool Contains(Vector2 point)
    {
      return (double) point.X >= (double) this.UpperLeft.X && (double) point.X <= (double) this.LowerRight.X && (double) point.Y >= (double) this.UpperLeft.Y && (double) point.Y <= (double) this.LowerRight.Y;
    }

    public bool Contains(BoundingSquare bs)
    {
      return this.Rectangle.Contains(bs.Rectangle) == ContainmentType.Contains;
    }

    public bool Intersects(BoundingSquare bs) => this.Rectangle.Intersects(bs.Rectangle);

    public bool Collides(OOBoundingBox ooBbox)
    {
      return this.Contains(ooBbox.P0) || this.Contains(ooBbox.P1) || this.Contains(ooBbox.P2) || this.Contains(ooBbox.P3);
    }

    public bool Collides(BoundingSquare bbox) => this.Rectangle.Intersects(bbox.Rectangle);

    public static BoundingSquare FromDataFileRecord(DataFileRecord record)
    {
      BoundingSquare boundingSquare = new BoundingSquare();
      boundingSquare.InitFromDataFileRecord(record);
      return boundingSquare;
    }

    public bool IntersectsLine(Vector2 lineP0, Vector2 lineP1, out Vector2 nearestColliddingPoint)
    {
      nearestColliddingPoint = Vector2.Zero;
      float num1 = 999999f;
      float num2 = -1f;
      Vector2 i;
      if (Mathematics.LineLineIntersection(lineP0, lineP1, this.UpperLeft, this.UpperRight, out i))
      {
        Vector2 vector2 = lineP0 - i;
        num2 = num1 = vector2.Length();
        nearestColliddingPoint = i;
      }
      if (Mathematics.LineLineIntersection(lineP0, lineP1, this.UpperRight, this.LowerRight, out i))
      {
        num2 = (lineP0 - i).Length();
        if ((double) num2 < (double) num1)
        {
          num1 = num2;
          nearestColliddingPoint = i;
        }
      }
      if (Mathematics.LineLineIntersection(lineP0, lineP1, this.LowerRight, this.LowerLeft, out i))
      {
        num2 = (lineP0 - i).Length();
        if ((double) num2 < (double) num1)
        {
          num1 = num2;
          nearestColliddingPoint = i;
        }
      }
      if (Mathematics.LineLineIntersection(lineP0, lineP1, this.LowerLeft, this.UpperLeft, out i))
      {
        num2 = (lineP0 - i).Length();
        if ((double) num2 < (double) num1)
          nearestColliddingPoint = i;
      }
      return (double) num2 != -1.0;
    }

    public void InitFromDataFileRecord(DataFileRecord record)
    {
      float fieldValue1 = record.GetFieldValue<float>("Left", this.UpperLeft.X);
      float fieldValue2 = record.GetFieldValue<float>("Top", this.UpperLeft.Y);
      float fieldValue3 = record.GetFieldValue<float>("Width", this.Width);
      float fieldValue4 = record.GetFieldValue<float>("Height", this.Height);
      this.UpperLeft = new Vector2(fieldValue1, fieldValue2);
      this.LowerRight = new Vector2(fieldValue1 + fieldValue3, fieldValue2 + fieldValue4);
      this.UpperRight = new Vector2(fieldValue1 + fieldValue3, fieldValue2);
      this.LowerLeft = new Vector2(fieldValue1, fieldValue2 + fieldValue4);
      this.UpdateRect();
    }

    public DataFileRecord ToDataFileRecord()
    {
      DataFileRecord dataFileRecord = new DataFileRecord("BoundingBox");
      dataFileRecord.AddField("Left", (object) this.UpperLeft.X);
      dataFileRecord.AddField("Top", (object) this.UpperLeft.Y);
      dataFileRecord.AddField("Width", (object) this.Width);
      dataFileRecord.AddField("Height", (object) this.Height);
      return dataFileRecord;
    }

    public override string ToString()
    {
      return string.Format("{0} {1}", (object) this.UpperLeft.ToString(), (object) this.LowerRight.ToString());
    }

    public static BoundingSquare CreateFromDataFileRecord(DataFileRecord record)
    {
      BoundingSquare fromDataFileRecord = new BoundingSquare();
      fromDataFileRecord.InitFromDataFileRecord(record);
      return fromDataFileRecord;
    }
  }
}
