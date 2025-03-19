
// Type: TwoBrainsGames.BrainEngine.Effects.TransformEffectBase
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;


namespace TwoBrainsGames.BrainEngine.Effects
{
  public class TransformEffectBase : ITransformEffect
  {
    private bool _ended;
    public double _expirationTime;
    public double _ellapsedExpirationTime;
    public object _onEndEventParam;
    private Vector4 _ColorVector;

    public TransformEffectBase.OnEndEvent OnEnd { get; set; }

    public bool Active { get; set; }

    public bool UseRealTime { get; set; }

    private Matrix Transform { get; set; }

    public virtual bool Ended
    {
      get => this._ended;
      set
      {
        if (this._ended == value)
          return;
        this._ended = value;
        if (!this._ended || this.OnEnd == null)
          return;
        this.OnEnd(this._onEndEventParam);
      }
    }

    public Vector3 Position { get; set; }

    public Vector2 PositionV2
    {
      get => new Vector2(this.Position.X, this.Position.Y);
      set => this.Position = new Vector3(value.X, value.Y, 0.0f);
    }

    public Vector3 VirtualPosition { get; set; }

    public Vector2 VirtualPositionV2
    {
      get => new Vector2(this.VirtualPosition.X, this.VirtualPosition.Y);
      set => this.VirtualPosition = new Vector3(value.X, value.Y, 0.0f);
    }

    public float Rotation { get; set; }

    public float VirtualRotation { get; set; }

    public Vector4 ColorVector
    {
      get => this._ColorVector;
      set => this._ColorVector = value;
    }

    public Color Color => new Color(this.ColorVector);

    public Vector2 Scale { get; set; }

    public Vector2 LastScale { get; set; }

    public bool AutoDeleteOnEnd { get; set; }

    public TransformEffectBase()
      : this(true)
    {
      this.Reset();
      this.Active = true;
      this.AutoDeleteOnEnd = true;
    }

    public TransformEffectBase(bool persistentEffect) => this.Transform = Matrix.Identity;

    public virtual void Update(BrainGameTime gameTime)
    {
    }

    public virtual void InternalUpdate(BrainGameTime gameTime)
    {
      if (this._expirationTime > 0.0)
      {
        this._ellapsedExpirationTime += gameTime.ElapsedGameTime.TotalMilliseconds;
        if (this._ellapsedExpirationTime >= this._expirationTime)
        {
          this.Ended = true;
          return;
        }
      }
      this.Update(gameTime);
    }

    public virtual void Reset()
    {
      this.Transform = Matrix.Identity;
      this.Ended = false;
      this.Position = new Vector3();
      this.Rotation = 0.0f;
      this.VirtualPosition = new Vector3();
      this.VirtualRotation = 0.0f;
      this.ColorVector = new Vector4(1f, 1f, 1f, 1f);
      this._ellapsedExpirationTime = 0.0;
      this.Scale = new Vector2(1f, 1f);
      this.LastScale = this.Scale;
    }

    public delegate void OnEndEvent(object param);
  }
}
