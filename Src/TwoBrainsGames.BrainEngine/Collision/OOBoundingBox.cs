
// Type: TwoBrainsGames.BrainEngine.Collision.OOBoundingBox
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;


namespace TwoBrainsGames.BrainEngine.Collision
{
  public struct OOBoundingBox
  {
    public Vector2 P0;
    public Vector2 P1;
    public Vector2 P2;
    public Vector2 P3;

    public OOBoundingBox(Rectangle rect)
    {
      this.P0 = new Vector2((float) rect.Left, (float) rect.Top);
      this.P1 = new Vector2((float) (rect.Left + rect.Width), (float) rect.Top);
      this.P2 = new Vector2((float) (rect.Left + rect.Width), (float) (rect.Top + rect.Height));
      this.P3 = new Vector2((float) rect.Left, (float) (rect.Top + rect.Height));
    }

    public OOBoundingBox(Vector2 ul, Vector2 lr)
    {
      this.P0 = new Vector2(ul.X, ul.Y);
      this.P1 = new Vector2(lr.X, ul.Y);
      this.P2 = new Vector2(lr.X, lr.Y);
      this.P3 = new Vector2(ul.X, lr.Y);
    }

    public OOBoundingBox(Vector2 p1, Vector2 p2, Vector2 p3, Vector2 p4)
    {
      this.P0 = p1;
      this.P1 = p2;
      this.P2 = p3;
      this.P3 = p4;
    }

    public OOBoundingBox(float left, float top, float width, float height)
    {
      this.P0 = new Vector2(left, top);
      this.P1 = new Vector2(left + width, top);
      this.P2 = new Vector2(left + width, top + height);
      this.P3 = new Vector2(left, top + height);
    }

    public void Draw(Color color, Vector2 camPos)
    {
      BrainGame.DrawLine(this.P0 - camPos, this.P1 - camPos, color);
      BrainGame.DrawLine(this.P1 - camPos, this.P2 - camPos, color);
      BrainGame.DrawLine(this.P2 - camPos, this.P3 - camPos, color);
      BrainGame.DrawLine(this.P3 - camPos, this.P0 - camPos, color);
    }

    public Vector2 GetCenter()
    {
      Vector2 i;
      Mathematics.LineLineIntersection(this.P0, this.P2, this.P1, this.P3, out i);
      return i;
    }

    public OOBoundingBox Transform(Matrix mat)
    {
      return new OOBoundingBox(Vector2.Transform(this.P0, mat), Vector2.Transform(this.P1, mat), Vector2.Transform(this.P2, mat), Vector2.Transform(this.P3, mat));
    }

    public OOBoundingBox Transform(Vector2 position, float rotation)
    {
      Matrix rotationZ = Matrix.CreateRotationZ(MathHelper.ToRadians(rotation));
      return this.Transform(Matrix.CreateTranslation(new Vector3(position.X, position.Y, 0.0f)) * rotationZ);
    }

    public void TransformInPlace(Vector2 position)
    {
      this.P0 += position;
      this.P1 += position;
      this.P2 += position;
      this.P3 += position;
    }

    public OOBoundingBox Transform(float rotation, Vector2 position)
    {
      return this.Transform(Matrix.CreateRotationZ(MathHelper.ToRadians(rotation)) * Matrix.CreateTranslation(new Vector3(position.X, position.Y, 0.0f)));
    }

    public bool Collides(BoundingSquare bbox) => bbox.Collides(this);

    public bool Collides(OOBoundingBox bbox) => false;

    public Rectangle ToRect()
    {
      float x1 = this.P0.X;
      float y1 = this.P0.Y;
      float x2 = this.P0.X;
      float y2 = this.P0.Y;
      if ((double) x1 > (double) this.P1.X)
        x1 = this.P1.X;
      if ((double) x1 > (double) this.P2.X)
        x1 = this.P2.X;
      if ((double) x1 > (double) this.P3.X)
        x1 = this.P3.X;
      if ((double) y1 > (double) this.P1.Y)
        y1 = this.P1.Y;
      if ((double) y1 > (double) this.P2.Y)
        y1 = this.P2.Y;
      if ((double) y1 > (double) this.P3.Y)
        y1 = this.P3.Y;
      if ((double) x2 < (double) this.P1.X)
        x2 = this.P1.X;
      if ((double) x2 < (double) this.P2.X)
        x2 = this.P2.X;
      if ((double) x2 < (double) this.P3.X)
        x2 = this.P3.X;
      if ((double) y2 < (double) this.P1.Y)
        y2 = this.P1.Y;
      if ((double) y2 < (double) this.P2.Y)
        y2 = this.P2.Y;
      if ((double) y2 < (double) this.P3.Y)
        y2 = this.P3.Y;
      return new Rectangle((int) x1, (int) y1, (int) ((double) x2 - (double) x1), (int) ((double) y2 - (double) y1));
    }

    public bool Contains(int x, int y) => this.ToRect().Contains(x, y);

    public bool IsInside(Vector2 vLowerLeft, Vector2 vUpperRight)
    {
      return (double) this.P3.X >= (double) vLowerLeft.X && (double) this.P1.X <= (double) vUpperRight.X && (double) this.P3.Y >= (double) vLowerLeft.Y && (double) this.P1.Y <= (double) vUpperRight.Y;
    }

    public BoundingBox ToBoundingBox()
    {
      return new BoundingBox(new Vector3(this.P0.X, this.P0.Y, 0.0f), new Vector3(this.P2.X, this.P2.Y, 0.0f));
    }

    public OOBoundingBox.CollidingSegment IntersectsLine(
      Vector2 lineP0,
      Vector2 lineP1,
      out Vector2 nearestColliddingPoint)
    {
      nearestColliddingPoint = Vector2.Zero;
      OOBoundingBox.CollidingSegment collidingSegment = OOBoundingBox.CollidingSegment.None;
      float num1 = 999999f;
      Vector2 i;
      if (Mathematics.LineLineIntersection(lineP0, lineP1, this.P0, this.P1, out i))
      {
        num1 = (lineP0 - i).Length();
        nearestColliddingPoint = i;
        collidingSegment = OOBoundingBox.CollidingSegment.P0P1;
      }
      if (Mathematics.LineLineIntersection(lineP0, lineP1, this.P1, this.P2, out i))
      {
        float num2 = (lineP0 - i).Length();
        if ((double) num2 < (double) num1)
        {
          num1 = num2;
          nearestColliddingPoint = i;
          collidingSegment = OOBoundingBox.CollidingSegment.P1P2;
        }
      }
      if (Mathematics.LineLineIntersection(lineP0, lineP1, this.P2, this.P3, out i))
      {
        float num3 = (lineP0 - i).Length();
        if ((double) num3 < (double) num1)
        {
          num1 = num3;
          nearestColliddingPoint = i;
          collidingSegment = OOBoundingBox.CollidingSegment.P2P3;
        }
      }
      if (Mathematics.LineLineIntersection(lineP0, lineP1, this.P3, this.P0, out i))
      {
        if ((double) (lineP0 - i).Length() < (double) num1)
        {
          nearestColliddingPoint = i;
          collidingSegment = OOBoundingBox.CollidingSegment.P3P0;
        }
      }
      return collidingSegment;
    }

    public bool IntersectsLine(
      Vector2 lineP0,
      Vector2 lineP1,
      out Vector2 intersectingPoint1,
      out Vector2 intersectingPoint2)
    {
      int num = 0;
      intersectingPoint1 = Vector2.Zero;
      intersectingPoint2 = Vector2.Zero;
      Vector2 i;
      if (Mathematics.LineLineIntersection(lineP0, lineP1, this.P0, this.P1, out i))
      {
        intersectingPoint1 = i;
        ++num;
      }
      if (Mathematics.LineLineIntersection(lineP0, lineP1, this.P1, this.P2, out i))
      {
        if (num > 0)
          intersectingPoint2 = i;
        else
          intersectingPoint1 = i;
        ++num;
      }
      if (Mathematics.LineLineIntersection(lineP0, lineP1, this.P2, this.P3, out i))
      {
        if (num > 0)
          intersectingPoint2 = i;
        else
          intersectingPoint1 = i;
        ++num;
      }
      if (Mathematics.LineLineIntersection(lineP0, lineP1, this.P3, this.P0, out i))
      {
        if (num > 0)
          intersectingPoint2 = i;
        else
          intersectingPoint1 = i;
        ++num;
      }
      return num > 0;
    }

    public BoundingSquare ToBoundingSquare()
    {
      float x1 = Math.Min(Math.Min(Math.Min(this.P0.X, this.P1.X), this.P2.X), this.P3.X);
      float x2 = Math.Max(Math.Max(Math.Max(this.P0.X, this.P1.X), this.P2.X), this.P3.X);
      float y1 = Math.Min(Math.Min(Math.Min(this.P0.Y, this.P1.Y), this.P2.Y), this.P3.Y);
      float y2 = Math.Max(Math.Max(Math.Max(this.P0.Y, this.P1.Y), this.P2.Y), this.P3.Y);
      return new BoundingSquare(new Vector2(x1, y1), new Vector2(x2, y2));
    }

    public void Initialize(Vector2 ul, Vector2 lr)
    {
      this.P0 = new Vector2(ul.X, ul.Y);
      this.P1 = new Vector2(lr.X, ul.Y);
      this.P2 = new Vector2(lr.X, lr.Y);
      this.P3 = new Vector2(ul.X, lr.Y);
    }

    public void TransformInPlace(float rotation, Vector2 position)
    {
      if ((double) rotation != 0.0)
        this.TransformInPlace(Matrix.CreateRotationZ(MathHelper.ToRadians(rotation)) * Matrix.CreateTranslation(new Vector3(position.X, position.Y, 0.0f)));
      this.TransformInPlace(Matrix.CreateTranslation(new Vector3(position.X, position.Y, 0.0f)));
    }

    public void TransformInPlace(Matrix mat)
    {
      this.P0 = Vector2.Transform(this.P0, mat);
      this.P1 = Vector2.Transform(this.P1, mat);
      this.P2 = Vector2.Transform(this.P2, mat);
      this.P3 = Vector2.Transform(this.P3, mat);
    }

    public bool Equals(OOBoundingBox bb)
    {
      return bb.P0 == this.P0 && bb.P1 == this.P1 && bb.P2 == this.P2 && bb.P3 == this.P3;
    }

    public enum CollidingSegment
    {
      None,
      P0P1,
      P1P2,
      P2P3,
      P3P0,
    }
  }
}
