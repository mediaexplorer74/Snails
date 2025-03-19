
// Type: TwoBrainsGames.Snails.Stages.PathSegment
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;


namespace TwoBrainsGames.Snails.Stages
{
  public class PathSegment
  {
    private Vector2 _p0;
    private Vector2 _p1;

    public Vector2 P0
    {
      get => this._p0;
      set
      {
        this._p0 = value;
        this.UpdateCenter();
      }
    }

    public Vector2 P1
    {
      get => this._p1;
      set
      {
        this._p1 = value;
        this.UpdateCenter();
      }
    }

    public PathSegmentBehavior Behavior { get; set; }

    public Vector2 Normal { get; private set; }

    public float Length { get; private set; }

    public float Rotation { get; private set; }

    public PathSegment.SegmentType WallType { get; private set; }

    public bool FloorIsBreakable { get; set; }

    public Vector2 Center { get; private set; }

    public PathSegment(Vector2 p0, Vector2 p1)
      : this(p0, p1, PathSegmentBehavior.None, true)
    {
    }

    public PathSegment(
      Vector2 p0,
      Vector2 p1,
      PathSegmentBehavior behavior,
      bool floorIsBreakable)
    {
      this.P0 = p0;
      this.P1 = p1;
      this.Behavior = behavior;
      this.FloorIsBreakable = floorIsBreakable;
      this.Normal = Vector2.Normalize(p1 - p0);
      this.Length = (this.P1 - this.P0).Length();
      this.Rotation = Mathematics.VectorAngle(this.Normal);
      if ((double) this.P0.X < (double) this.P1.X)
        this.WallType = PathSegment.SegmentType.Floor;
      else if ((double) this.P0.X > (double) this.P1.X)
        this.WallType = PathSegment.SegmentType.Ceiling;
      else if ((double) this.P0.X == (double) this.P1.X && (double) this.P0.Y < (double) this.P1.Y)
        this.WallType = PathSegment.SegmentType.LeftWall;
      else
        this.WallType = PathSegment.SegmentType.RightWall;
    }

    public override int GetHashCode() => base.GetHashCode();

    public override bool Equals(object obj) => base.Equals(obj);

    public static bool operator ==(PathSegment a, PathSegment b)
    {
      if (object.ReferenceEquals((object) a, (object) b))
        return true;
      return (object) a != null && (object) b != null && a.P0 == b.P0 && a.P1 == b.P1;
    }

    public static bool operator !=(PathSegment a, PathSegment b) => !(a == b);

    public bool Intersects(BoundingSquare bs)
    {
      Vector3 direction = new Vector3(this.P1.X - this.P0.X, this.P1.Y - this.P0.Y, 0.0f);
      Vector3 position = new Vector3(this.P0.X, this.P0.Y, 0.0f);
      float num = direction.Length();
      direction.Normalize();
      Ray ray = new Ray(position, direction);
      float? nullable = new BoundingBox(new Vector3(bs.UpperLeft.X, bs.UpperLeft.Y, 0.0f), new Vector3(bs.LowerRight.X, bs.LowerRight.Y, 0.0f)).Intersects(ray);
      return nullable.HasValue && (double) nullable.Value < (double) num;
    }

    public bool Intersection(PathSegment S1, ref Point ptIntersection)
    {
      PathSegment pathSegment = this;
      double num1 = ((double) S1.P1.Y - (double) S1.P0.Y) * ((double) pathSegment.P1.X - (double) pathSegment.P0.X) - ((double) S1.P1.X - (double) S1.P0.X) * ((double) pathSegment.P1.Y - (double) pathSegment.P0.Y);
      double num2 = ((double) S1.P1.X - (double) S1.P0.X) * ((double) pathSegment.P0.Y - (double) S1.P0.Y) - ((double) S1.P1.Y - (double) S1.P0.Y) * ((double) pathSegment.P0.X - (double) S1.P0.X);
      double num3 = ((double) pathSegment.P1.X - (double) pathSegment.P0.X) * ((double) pathSegment.P0.Y - (double) S1.P0.Y) - ((double) pathSegment.P1.Y - (double) pathSegment.P0.Y) * ((double) pathSegment.P0.X - (double) S1.P0.X);
      if (num1 == 0.0)
        return false;
      double num4 = num2 / num1;
      double num5 = num3 / num1;
      if (num4 < 0.0 || num4 > 1.0 || num5 < 0.0 || num5 > 1.0)
        return false;
      ptIntersection.X = (int) ((double) pathSegment.P0.X + num4 * ((double) pathSegment.P1.X - (double) pathSegment.P0.X));
      ptIntersection.Y = (int) ((double) pathSegment.P0.Y + num4 * ((double) pathSegment.P1.Y - (double) pathSegment.P0.Y));
      return true;
    }

    public bool Collides(PathSegment other, ref Point I) => this.Intersection(other, ref I);

    public bool Intersection(PathSegment S1, ref Vector2 ptIntersection)
    {
      PathSegment pathSegment = this;
      double num1 = ((double) S1.P1.Y - (double) S1.P0.Y) * ((double) pathSegment.P1.X - (double) pathSegment.P0.X) - ((double) S1.P1.X - (double) S1.P0.X) * ((double) pathSegment.P1.Y - (double) pathSegment.P0.Y);
      double num2 = ((double) S1.P1.X - (double) S1.P0.X) * ((double) pathSegment.P0.Y - (double) S1.P0.Y) - ((double) S1.P1.Y - (double) S1.P0.Y) * ((double) pathSegment.P0.X - (double) S1.P0.X);
      double num3 = ((double) pathSegment.P1.X - (double) pathSegment.P0.X) * ((double) pathSegment.P0.Y - (double) S1.P0.Y) - ((double) pathSegment.P1.Y - (double) pathSegment.P0.Y) * ((double) pathSegment.P0.X - (double) S1.P0.X);
      if (num1 == 0.0)
        return false;
      double num4 = num2 / num1;
      double num5 = num3 / num1;
      if (num4 < 0.0 || num4 > 1.0 || num5 < 0.0 || num5 > 1.0)
        return false;
      ptIntersection = new Vector2(pathSegment.P0.X + (float) (num4 * ((double) pathSegment.P1.X - (double) pathSegment.P0.X)), pathSegment.P0.Y + (float) (num4 * ((double) pathSegment.P1.Y - (double) pathSegment.P0.Y)));
      return true;
    }

    public bool Collides(PathSegment other, ref Vector2 I) => this.Intersection(other, ref I);

    public float Classify(PathSegment segment)
    {
      return Vector3.Cross(new Vector3(this.Normal.X, this.Normal.Y, 0.0f), new Vector3(segment.Normal.X, segment.Normal.Y, 0.0f)).Z;
    }

    private void UpdateCenter()
    {
      this.Center = this.P0 + new Vector2((float) (((double) this.P1.X - (double) this.P0.X) / 2.0), (float) (((double) this.P1.Y - (double) this.P0.Y) / 2.0));
    }

    public override string ToString()
    {
      return string.Format("P0[{0},{1}], P1[{2},{3}]", (object) this.P0.X, (object) this.P0.Y, (object) this.P1.X, (object) this.P1.Y);
    }

    public enum SegmentType
    {
      None,
      Floor,
      RightWall,
      LeftWall,
      Ceiling,
    }
  }
}
