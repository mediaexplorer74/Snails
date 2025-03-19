
// Type: TwoBrainsGames.BrainEngine.Effects.TransformEffects.MotionEffect
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;


namespace TwoBrainsGames.BrainEngine.Effects.TransformEffects
{
  public class MotionEffect : TransformEffectBase, ITransformEffect
  {
    private float _gravity;
    private float _t;
    private Vector2 _initialSpeed;
    private Vector2 _prev;

    public float Gravity
    {
      get => this._gravity;
      set => this._gravity = value;
    }

    public Vector2 InitialSpeed
    {
      get => this._initialSpeed;
      set => this._initialSpeed = value;
    }

    public Vector2 CurrentSpeed { get; set; }

    public MotionEffect(float gravity, Vector2 initialSpeed)
    {
      this._gravity = gravity;
      this.Reset(initialSpeed);
    }

    public override void Update(BrainGameTime gameTime)
    {
      this._t += (float) (gameTime.ElapsedGameTime.TotalMilliseconds / 120.0);
      float y = (float) ((double) this._initialSpeed.Y * (double) this._t + 0.5 * (double) this._gravity * ((double) this._t * (double) this._t));
      float x = this._initialSpeed.X * this._t;
      this.Position = new Vector3(x - this._prev.X, y - this._prev.Y, 0.0f);
      this._prev = new Vector2(x, y);
      this.CurrentSpeed = new Vector2(this._initialSpeed.X, this._initialSpeed.Y + this._gravity * this._t);
    }

    public void Reset(Vector2 initialSpeed)
    {
      this.Reset();
      this._initialSpeed = initialSpeed;
      this._t = 0.0f;
      this._prev = Vector2.Zero;
    }
  }
}
