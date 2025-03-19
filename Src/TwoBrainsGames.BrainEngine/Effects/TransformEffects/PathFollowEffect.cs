
// Type: TwoBrainsGames.BrainEngine.Effects.TransformEffects.PathFollowEffect
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace TwoBrainsGames.BrainEngine.Effects.TransformEffects
{
  public class PathFollowEffect : TransformEffectBase, ITransformEffect
  {
    private List<Vector3> _pathPoints;
    private float _speed;
    private int _currentPointIdx;
    private Vector3 _currentPosition;
    private Vector3 _previousPosition;

    public PathFollowEffect(List<Vector3> pathPoints, float speed, bool connectEndPoints)
    {
      this._pathPoints = pathPoints;
      this._speed = speed;
      this._currentPointIdx = 0;
      if (this._pathPoints.Count <= 0)
        return;
      this._currentPosition = this._previousPosition = this._pathPoints[0];
    }

    public PathFollowEffect(
      Vector3 startPoint,
      Vector3 endPoint,
      float speed,
      bool connectEndPoints)
      : this(new List<Vector3>(), speed, connectEndPoints)
    {
      this._pathPoints.Add(startPoint);
      this._pathPoints.Add(endPoint);
      this._currentPosition = this._previousPosition = this._pathPoints[0];
    }

    public PathFollowEffect(
      Vector2 startPoint,
      Vector2 endPoint,
      float speed,
      bool connectEndPoints)
      : this(new Vector3(startPoint.X, startPoint.Y, 0.0f), new Vector3(endPoint.X, endPoint.Y, 0.0f), speed, connectEndPoints)
    {
    }

    public override void Update(BrainGameTime gameTime)
    {
      this._previousPosition = this._currentPosition;
      if (this._pathPoints[this._currentPointIdx].Equals(this._pathPoints[this._currentPointIdx + 1]))
      {
        this.Ended = true;
      }
      else
      {
        float num = (float) ((double) this._speed * (double) gameTime.ElapsedGameTime.Milliseconds / 10.0);
        Vector3 vector3_1 = this._pathPoints[this._currentPointIdx + 1] - this._pathPoints[this._currentPointIdx];
        Vector3 vector3_2 = vector3_1;
        vector3_2.Normalize();
        this._currentPosition += new Vector3(vector3_2.X * num, vector3_2.Y * num, 0.0f);
        if ((double) ((this._pathPoints[this._currentPointIdx] - this._currentPosition).Length() - vector3_1.Length()) > 0.0)
        {
          this.Ended = true;
          this._currentPosition = this._pathPoints[this._currentPointIdx + 1];
        }
        this.Position = this._currentPosition - this._previousPosition;
      }
    }
  }
}
