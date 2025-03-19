
// Type: TwoBrainsGames.BrainEngine.Collision.BoundingCircle
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using TwoBrainsGames.BrainEngine.Data.DataFiles;


namespace TwoBrainsGames.BrainEngine.Collision
{
  public class BoundingCircle : IDataFileSerializable
  {
    public Vector2 _center;
    public float _radius;

    public BoundingCircle()
    {
    }

    public BoundingCircle(Vector2 center, float radius)
    {
      this._center = center;
      this._radius = radius;
    }

    public BoundingCircle Transform(Vector2 pos)
    {
      return new BoundingCircle(this._center + pos, this._radius);
    }

    public bool IntersectsLine(Vector2 p1, Vector2 p2, out Vector2 nearestIntersection)
    {
      nearestIntersection = Vector2.Zero;
      float num1 = p2.X - p1.X;
      float num2 = p2.Y - p1.Y;
      float num3 = (float) ((double) num1 * (double) num1 + (double) num2 * (double) num2);
      float num4 = (float) (2.0 * ((double) num1 * ((double) p1.X - (double) this._center.X) + (double) num2 * ((double) p1.Y - (double) this._center.Y)));
      float num5 = (float) (((double) p1.X - (double) this._center.X) * ((double) p1.X - (double) this._center.X) + ((double) p1.Y - (double) this._center.Y) * ((double) p1.Y - (double) this._center.Y) - (double) this._radius * (double) this._radius);
      float d = (float) ((double) num4 * (double) num4 - 4.0 * (double) num3 * (double) num5);
      if ((double) num3 <= 1E-07 || (double) d < 0.0)
        return false;
      if ((double) d == 0.0)
      {
        float num6 = (float) (-(double) num4 / (2.0 * (double) num3));
        nearestIntersection = new Vector2(p1.X + num6 * num1, p1.Y + num6 * num2);
        return (double) (nearestIntersection - p1).LengthSquared() < (double) (p2 - p1).LengthSquared();
      }
      float num7 = (float) ((-(double) num4 - Math.Sqrt((double) d)) / (2.0 * (double) num3));
      nearestIntersection = new Vector2(p1.X + num7 * num1, p1.Y + num7 * num2);
      return (double) (nearestIntersection - p1).LengthSquared() < (double) (p2 - p1).LengthSquared();
    }

    public BoundingCircle Transform(Vector2 pos, float rotation)
    {
      return new BoundingCircle(Vector2.Transform(this._center, Matrix.CreateRotationZ(MathHelper.ToRadians(rotation))) + pos, this._radius);
    }

    public static BoundingCircle FromDataFileRecord(DataFileRecord record)
    {
      BoundingCircle boundingCircle = new BoundingCircle();
      boundingCircle.InitFromDataFileRecord(record);
      return boundingCircle;
    }

    public BoundingSquare GetContainingSquare()
    {
      return new BoundingSquare(new Vector2(this._center.X - this._radius, this._center.Y - this._radius), this._radius * 2f, this._radius * 2f);
    }

    public void Draw(Color color, Vector2 camPos)
    {
      Vector2 position = new Vector2(1f, 0.0f) * this._radius;
      for (int index = 0; index < 360; index += 20)
      {
        Matrix rotationZ = Matrix.CreateRotationZ(MathHelper.ToRadians(20f));
        Vector2 vector2 = Vector2.Transform(position, rotationZ);
        BrainGame.DrawLine(position - camPos + this._center, vector2 - camPos + this._center, color);
        position = vector2;
      }
    }

    public bool Contains(Vector2 p)
    {
      return (double) (this._center - p).Length() <= (double) this._radius;
    }

    public bool Collides(BoundingSquare bs)
    {
      float num1 = Math.Abs((float) ((double) this._center.X - (double) bs.UpperLeft.X - (double) bs.Width / 2.0));
      float num2 = Math.Abs((float) ((double) this._center.Y - (double) bs.UpperLeft.Y - (double) bs.Height / 2.0));
      if ((double) num1 > (double) bs.Width / 2.0 + (double) this._radius || (double) num2 > (double) bs.Height / 2.0 + (double) this._radius)
        return false;
      return (double) num1 <= (double) bs.Width / 2.0 || (double) num2 <= (double) bs.Height / 2.0 || ((double) num1 - (double) bs.Width / 2.0) * ((double) num1 - (double) bs.Width / 2.0) + ((double) num2 - (double) bs.Height / 2.0) * ((double) num2 - (double) bs.Height / 2.0) <= (double) this._radius * (double) this._radius;
    }

    public override string ToString()
    {
      return string.Format("X: {0}, Y: {1}, Radius: {2}", (object) this._center.X, (object) this._center.Y, (object) this._radius);
    }

    public void InitFromDataFileRecord(DataFileRecord record)
    {
      float fieldValue1 = record.GetFieldValue<float>("X", this._center.X);
      float fieldValue2 = record.GetFieldValue<float>("Y", this._center.Y);
      this._radius = record.GetFieldValue<float>("Radius", this._radius);
      this._center = new Vector2(fieldValue1, fieldValue2);
    }

    public DataFileRecord ToDataFileRecord()
    {
      DataFileRecord dataFileRecord = new DataFileRecord("BoundingSphere");
      dataFileRecord.AddField("X", (object) this._center.X);
      dataFileRecord.AddField("Y", (object) this._center.Y);
      dataFileRecord.AddField("Radius", (object) this._radius);
      return dataFileRecord;
    }
  }
}
