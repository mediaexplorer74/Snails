
// Type: TwoBrainsGames.BrainEngine.Mathematics
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;


namespace TwoBrainsGames.BrainEngine
{
  public class Mathematics
  {
    public static float VectorAngle(Vector2 v1)
    {
      float degrees = MathHelper.ToDegrees((float) Math.Asin((double) v1.Y));
      if ((double) v1.X < 0.0)
        return 180f - degrees;
      return (double) v1.Y < 0.0 ? 360f + degrees : degrees;
    }

    public static float AngleBetweenNormalizedVectors(Vector2 v1, Vector2 v2)
    {
      float num = (float) Math.Acos((double) Vector2.Dot(v1, v2));
      return (double) Math.Abs(num) < 0.0001 ? 0.0f : MathHelper.ToDegrees(num * (float) Mathematics.Signal(v1, v2));
    }

    private static int Signal(Vector2 v1, Vector2 v2)
    {
      return (double) v1.Y * (double) v2.X - (double) v2.Y * (double) v1.X <= 0.0 ? -1 : 1;
    }

    public static bool IsPointOnLine(Vector2 p0, Vector2 p1, Vector2 ptToCheck)
    {
      float num1 = (double) p1.Y - (double) p0.Y == 0.0 ? 1f : p1.Y - p0.Y;
      float num2 = (double) p1.X - (double) p0.X == 0.0 ? 1f : p1.X - p0.X;
      return ((double) ptToCheck.Y - (double) p0.Y) / (double) num1 == ((double) ptToCheck.X - (double) p0.X) / (double) num2;
    }

    public static float ClassifyPointInRay(Vector2 point, Vector2 rayStart, Vector2 rayEnd)
    {
      return (float) (((double) point.Y - (double) rayStart.Y) * ((double) rayEnd.X - (double) rayStart.X) - ((double) point.X - (double) rayStart.X) * ((double) rayEnd.Y - (double) rayStart.Y));
    }

    public static Vector2 RandomizeVector(int minAngle, int maxAngle)
    {
      float degrees = (float) (minAngle + BrainGame.Rand.Next(maxAngle - minAngle));
      float y = (float) Math.Sin((double) MathHelper.ToRadians(-degrees));
      return new Vector2((float) Math.Cos((double) MathHelper.ToRadians(degrees)), y);
    }

    public static bool LineLineIntersection(
      Vector2 p0,
      Vector2 p1,
      Vector2 p2,
      Vector2 p3,
      out Vector2 i)
    {
      i = Vector2.Zero;
      Vector2 vector2_1;
      vector2_1.X = p1.X - p0.X;
      vector2_1.Y = p1.Y - p0.Y;
      Vector2 vector2_2;
      vector2_2.X = p3.X - p2.X;
      vector2_2.Y = p3.Y - p2.Y;
      float num1 = (float) ((-(double) vector2_1.Y * ((double) p0.X - (double) p2.X) + (double) vector2_1.X * ((double) p0.Y - (double) p2.Y)) / (-(double) vector2_2.X * (double) vector2_1.Y + (double) vector2_1.X * (double) vector2_2.Y));
      float num2 = (float) (((double) vector2_2.X * ((double) p0.Y - (double) p2.Y) - (double) vector2_2.Y * ((double) p0.X - (double) p2.X)) / (-(double) vector2_2.X * (double) vector2_1.Y + (double) vector2_1.X * (double) vector2_2.Y));
      if ((double) num1 < 0.0 || (double) num1 > 1.0 || (double) num2 < 0.0 || (double) num2 > 1.0)
        return false;
      i.X = p0.X + num2 * vector2_1.X;
      i.Y = p0.Y + num2 * vector2_1.Y;
      return true;
    }

    public static Vector2 RotateVector(Vector2 v, float angleInDegrees)
    {
      float radians = MathHelper.ToRadians(angleInDegrees);
      float num1 = (float) Math.Cos((double) radians);
      float num2 = (float) Math.Sin((double) radians);
      return new Vector2((float) ((double) v.X * (double) num1 - (double) v.Y * (double) num2), (float) ((double) v.X * (double) num2 + (double) v.Y * (double) num1));
    }

    public static Vector2 TransformVector(Vector2 toTransform, float rotation, Vector2 position)
    {
      Matrix matrix = Matrix.CreateRotationZ(MathHelper.ToRadians(rotation)) * Matrix.CreateTranslation(position.X, position.Y, 0.0f);
      return Vector2.Transform(toTransform, matrix);
    }

    public static float ConvertAngleTo180(float angle)
    {
      return (double) angle <= 180.0 ? angle : angle - 360f;
    }
  }
}
