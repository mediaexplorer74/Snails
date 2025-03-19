
// Type: TwoBrainsGames.Snails.Effects.FloatingEffect
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.Snails.StageObjects;


namespace TwoBrainsGames.Snails.Effects
{
  public class FloatingEffect : TransformEffectBase, ITransformEffect
  {
    private float _speed;
    private float _angle;
    private int _amplitude;
    private int _interval;
    private int _signal;
    private Vector2 _auxObjectPosition;
    private Liquid _liquidObj;
    private bool _hasHoover;
    private int _hooverValue;

    public float Angle
    {
      get => this._angle;
      set => this._angle = value;
    }

    public int Amplitude
    {
      get => this._amplitude;
      set => this._amplitude = value;
    }

    public int Interval
    {
      get => this._interval;
      set => this._interval = value;
    }

    public int Signal
    {
      get => this._signal;
      set => this._signal = value;
    }

    public Rectangle FloatArea
    {
      get
      {
        Rectangle rect = this._liquidObj.QuadtreeCollisionBB.ToRect();
        if (this._hasHoover)
        {
          rect.Y += this._hooverValue;
          rect.Height -= this._hooverValue;
        }
        return rect;
      }
    }

    public FloatingEffect(float speed, Vector2 objPos, Liquid liquidObj)
      : this(speed, objPos, liquidObj, false, 0)
    {
    }

    public FloatingEffect(
      float speed,
      Vector2 objPos,
      Liquid liquidObj,
      bool hasHoover,
      int hooverValue)
    {
      this._speed = speed;
      this._auxObjectPosition = objPos;
      this._liquidObj = liquidObj;
      this._hasHoover = hasHoover;
      this._hooverValue = hooverValue;
      this._amplitude = BrainGame.Rand.Next(1, 3);
      this._interval = BrainGame.Rand.Next(1, 3);
      this._signal = BrainGame.Rand.Next(1, 2);
      if (this._signal == 2)
        this._signal = -1;
      this._angle = (float) BrainGame.Rand.Next(0, 360);
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      float num = (float) (this._signal * this._amplitude) * (float) Math.Sin((double) this._interval * (double) MathHelper.ToRadians(this._angle));
      this._angle += (float) (1.0 * gameTime.ElapsedGameTime.TotalMilliseconds) * this._speed;
      if ((double) this._angle > 360.0)
        this._angle = 1f;
      this.Position = new Vector3(this.Position.X + num * (float) gameTime.ElapsedGameTime.TotalMilliseconds * this._speed, this.Position.Y - (float) (1.0 * gameTime.ElapsedGameTime.TotalMilliseconds) * this._speed, 0.0f) * this._speed;
      this._auxObjectPosition += this.PositionV2;
      Vector2 vector2 = this._auxObjectPosition + this.PositionV2;
      if (this.FloatArea.Contains((int) vector2.X, (int) vector2.Y))
        return;
      this.Ended = true;
    }
  }
}
