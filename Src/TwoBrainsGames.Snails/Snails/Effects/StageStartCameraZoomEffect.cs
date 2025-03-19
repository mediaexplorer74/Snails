
// Type: TwoBrainsGames.Snails.Effects.StageStartCameraZoomEffect
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.Effects
{
  internal class StageStartCameraZoomEffect : TransformEffectBase
  {
    private const double EFFECT_DURATION = 500.0;
    private Vector2 _pointOfInterest;
    private double _ellapsedTime;
    private InGameCamera _camera;
    private float _distance;
    private float _speed;
    private float _zoomSpeed;
    private Vector2 _currentScale;

    public StageStartCameraZoomEffect(InGameCamera camera) => this._camera = camera;

    public StageStartCameraZoomEffect(Vector2 initialScale, Vector2 poi, InGameCamera camera)
    {
      this._camera = camera;
      this.Reset(initialScale, poi);
    }

    public override void Update(BrainGameTime gameTime)
    {
      this._ellapsedTime += gameTime.ElapsedRealTime.TotalMilliseconds;
      float num1 = this._speed * (float) gameTime.ElapsedRealTime.TotalMilliseconds;
      Vector2 vector2 = this._pointOfInterest - this._camera.Position;
      vector2.Normalize();
      this.PositionV2 = vector2 * num1;
      float num2 = this._zoomSpeed * (float) gameTime.ElapsedGameTime.TotalMilliseconds;
      this._currentScale += new Vector2(num2, num2);
      if ((double) this._currentScale.X > 1.0 || (double) this._currentScale.Y > 1.0)
      {
        this.Scale = new Vector2(1f, 1f);
        this.Active = false;
      }
      else
        this.Scale = this._currentScale;
    }

    public void Reset(Vector2 initialScale, Vector2 poi)
    {
      this.Reset();
      this._ellapsedTime = 0.0;
      this._pointOfInterest = poi;
      this._distance = (this._camera.Position - poi).Length();
      this._speed = this._distance / 500f;
      this._currentScale = initialScale;
      this.Scale = this.LastScale = initialScale;
      this._zoomSpeed = (float) ((1.0 - (double) initialScale.X) / 500.0);
    }
  }
}
