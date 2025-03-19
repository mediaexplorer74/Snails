
// Type: TwoBrainsGames.BrainEngine.Graphics.Object2D
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Effects;


namespace TwoBrainsGames.BrainEngine.Graphics
{
  public class Object2D : IDataFileSerializable
  {
    public const float DEFAULT_LAYER_DEPTH = 0.6f;
    protected Vector2 _position;
    protected float _rotation;
    protected float _rotationInRad;
    private Sprite _Sprite;
    public bool SpriteAnimationActive;
    public Object2D.AnimtionPlaybackModes _SpritePlaybackMode;
    public SpriteEffects SpriteEffect;
    public string Id;
    public string ResourceId;
    public string SpriteId;
    public int CurrentFrame;
    public TransformBlender EffectsBlender;
    public TransformBlender SpriteEffectsBlender;
    public OOBoundingBox BoundingBox;
    public BoundingSquare AABoundingBox;
    public BoundingCircle BoundingCircle;
    public bool BoundingBoxChanged;
    public bool DrawQuadtree;
    protected float _frameUpdateMultiplier;
    protected float LayerDepth;
    protected int FramesPerTime;
    protected Vector2 SpriteDrawOffset;
    protected double ElapsedGameTime;
    private Color _color;
    protected bool _colorChanged;

    public string UniqueId { get; set; }

    public Vector2 Position
    {
      get => this._position;
      set => this._position = value;
    }

    public Color BlendColor
    {
      get => this._color;
      set
      {
        if (!(this._color != value))
          return;
        this._colorChanged = true;
        this._color = value;
      }
    }

    public float X
    {
      get => this._position.X;
      set => this._position.X = value;
    }

    public float Y
    {
      get => this._position.Y;
      set => this._position.Y = value;
    }

    public virtual float Rotation
    {
      get => this._rotation;
      set
      {
        if ((double) value == (double) this._rotation)
          return;
        this._rotation = value;
        if ((double) this._rotation < 0.0)
          this._rotation += 360f;
        if ((double) this._rotation >= 360.0)
          this._rotation -= 360f;
        this._rotationInRad = MathHelper.ToRadians(this._rotation);
      }
    }

    public virtual Sprite Sprite
    {
      get => this._Sprite;
      set
      {
        if (this._Sprite == value)
          return;
        this._Sprite = value;
        if (this.Sprite == null)
          return;
        this.UpdateBoundingBox();
      }
    }

    public float CurrentFrameWidth => (float) this.Sprite.Frames[this.CurrentFrame].Width;

    public float CurrentFrameHeight => (float) this.Sprite.Frames[this.CurrentFrame].Height;

    public int MiddleFrameNumber => this.Sprite.FrameCount / 2;

    public Vector2 Scale { get; set; }

    public bool IsHorizontallyFlipped
    {
      get => (this.SpriteEffect & SpriteEffects.FlipHorizontally) == SpriteEffects.FlipHorizontally;
    }

    public virtual BoundingSquare SoundEmmiterBoundingBox => new BoundingSquare();

    public Object2D()
    {
      this.SpriteEffect = SpriteEffects.None;
      this.LayerDepth = 0.6f;
      this.FramesPerTime = 1000;
      this.EffectsBlender = new TransformBlender();
      this.SpriteEffectsBlender = new TransformBlender();
      this.SpriteAnimationActive = true;
      this._frameUpdateMultiplier = 1f;
      this.Scale = new Vector2(1f, 1f);
    }

    public Object2D(Object2D other) => this.Copy(other);

    public virtual void Copy(Object2D other)
    {
      this._position = other._position;
      this._rotation = other._rotation;
      this.Id = other.Id;
      this.ResourceId = other.ResourceId;
      this.SpriteId = other.SpriteId;
      this.Sprite = other.Sprite;
      this.CurrentFrame = other.CurrentFrame;
      this.LayerDepth = other.LayerDepth;
      this.SpriteEffect = other.SpriteEffect;
      this.FramesPerTime = other.FramesPerTime;
      this.BoundingBox = new OOBoundingBox();
      this.SpriteAnimationActive = other.SpriteAnimationActive;
      this.Scale = other.Scale;
    }

    protected void UpdateEffects(BrainGameTime gameTime)
    {
      if (this.EffectsBlender.Count <= 0)
        return;
      this.EffectsBlender.Update(gameTime);
      this.Position += this.EffectsBlender.PositionV2;
      this.Rotation += this.EffectsBlender.Rotation;
      this.Scale += this.EffectsBlender._scale;
      this._color = this.EffectsBlender.Color;
      this.UpdateBoundingBox();
    }

    protected void UpdateSpriteEffects(BrainGameTime gameTime)
    {
      if (this.SpriteEffectsBlender.Count <= 0)
        return;
      this.SpriteEffectsBlender.Update(gameTime);
      this.SpriteDrawOffset += this.SpriteEffectsBlender.PositionV2;
    }

    public void GoToLastFrame() => this.CurrentFrame = this.Sprite.FrameCount - 1;

    private int GetCurrentFrameTime()
    {
      if (this.Sprite.Frames[this.CurrentFrame].PlayTime != 0)
        return this.Sprite.Frames[this.CurrentFrame].PlayTime;
      return this.Sprite.Fps == 0 ? 0 : this.FramesPerTime / this.Sprite.Fps;
    }

    public void UpdateCurrentFrame(BrainGameTime gameTime)
    {
      if (!this.SpriteAnimationActive)
        return;
      if (this.Sprite != null && this.Sprite.HasAnimations)
      {
        this.ElapsedGameTime += gameTime.ElapsedGameTime.TotalMilliseconds * (double) this._frameUpdateMultiplier;
        int currentFrameTime = this.GetCurrentFrameTime();
        do
        {
          if (this.ElapsedGameTime > (double) currentFrameTime)
          {
            if (this.CurrentFrame == 0)
              this.OnFirstFrame();
            this.OnMiddleFrames();
            ++this.CurrentFrame;
            if (this.CurrentFrame >= this.Sprite.FrameCount)
            {
              this.CurrentFrame = this._SpritePlaybackMode != Object2D.AnimtionPlaybackModes.Loop ? this.Sprite.FrameCount - 1 : 0;
              this.OnLastFrame();
            }
            this.ElapsedGameTime -= (double) currentFrameTime;
            currentFrameTime = this.GetCurrentFrameTime();
          }
        }
        while (this.ElapsedGameTime > (double) currentFrameTime && currentFrameTime != 0);
        if (!this.Sprite.Frames[this.CurrentFrame].WithCollisionBox)
          return;
        this.UpdateBoundingBox();
      }
      else
      {
        this.OnFirstFrame();
        this.OnMiddleFrames();
        this.OnLastFrame();
      }
    }

    public virtual void Update(BrainGameTime gameTime)
    {
      this.UpdateEffects(gameTime);
      this.UpdateSpriteEffects(gameTime);
      this.UpdateCurrentFrame(gameTime);
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
      this.Sprite.Draw(this._position, this.CurrentFrame, this._rotation, this.SpriteEffect, this.LayerDepth, Color.White, 1f, spriteBatch);
    }

    public virtual void Draw(SpriteBatch spriteBatch, Vector2 position, Color color)
    {
      this.Sprite.Draw(position, this.CurrentFrame, this._rotation, this.SpriteEffect, this.LayerDepth, color, this.Scale.X, spriteBatch);
    }

    public virtual void OnFirstFrame()
    {
    }

    public virtual void OnMiddleFrames()
    {
    }

    public virtual void OnLastFrame()
    {
    }

    public Vector2 GetAACenter()
    {
      return new Vector2(this.AABoundingBox.Left + this.AABoundingBox.Width / 2f, this.AABoundingBox.Top + this.AABoundingBox.Height / 2f);
    }

    public Vector2 GetCurrentFrameCenter()
    {
      Rectangle rect = this.Sprite.Frames[this.CurrentFrame].Rect;
      return this.Position + new Vector2((float) (rect.Width / 2), (float) (rect.Height / 2)) - this.Sprite.Offset;
    }

    public BoundingSquare TransformToObjectSpace(BoundingSquare bs) => bs.Transform(-this.Position);

    public virtual void UpdateBoundingBox()
    {
      this.BoundingBox = this.GetBoundingBoxTransformed(0);
      this.UpdateAABoundingBox();
      this.BoundingBoxChanged = true;
    }

    public virtual void UpdateBoundingCircle()
    {
      this.BoundingCircle = this.Sprite._boundingSpheres[0].Transform(this.Position);
    }

    public OOBoundingBox TransformCurrentFrameBB()
    {
      return this.Sprite.Frames[this.CurrentFrame].WithCollisionBox ? this.TransformBoundingBox(this.Sprite.Frames[this.CurrentFrame].BoundingBox) : this.TransformBoundingBox(new BoundingSquare(new Rectangle((int) -(double) this.Sprite.Offset.X, (int) -(double) this.Sprite.Offset.Y, this.Sprite.Frames[this.CurrentFrame].Rect.Width, this.Sprite.Frames[this.CurrentFrame].Rect.Height)));
    }

    public OOBoundingBox TransformSpriteFrameBB(int idx)
    {
      return this.TransformBoundingBox(this.Sprite.BoundingBoxes[idx]);
    }

    public BoundingCircle TransformBoundingCircle(BoundingCircle bs)
    {
      return new BoundingCircle(this.Position + bs._center, bs._radius);
    }

    public Vector2 TransformVector(Vector2 v)
    {
      return Mathematics.TransformVector(v, this.Rotation, this.Position);
    }

    public OOBoundingBox TransformBoundingBox(BoundingSquare bs)
    {
      if (!this.IsHorizontallyFlipped)
        return bs.Transform(this.Rotation, this.Position);
      BoundingSquare boundingSquare = bs;
      return new BoundingSquare(new Vector2(-boundingSquare.LowerRight.X, boundingSquare.UpperLeft.Y), boundingSquare.Width, boundingSquare.Height).Transform(this.Rotation, this.Position);
    }

    public OOBoundingBox GetBoundingBoxTransformed(int idx)
    {
      return this.TransformBoundingBox(this.GetSpriteBoundingBox(idx));
    }

    private void UpdateAABoundingBox() => this.AABoundingBox = this.BoundingBox.ToBoundingSquare();

    public virtual void InitFromDataFileRecord(DataFileRecord record)
    {
      this.Id = record.GetFieldValue<string>("id");
      this.ResourceId = record.GetFieldValue<string>("res", this.ResourceId);
      this.SpriteId = record.GetFieldValue<string>("sprite", this.SpriteId);
      this.CurrentFrame = record.GetFieldValue<int>("frame", this.CurrentFrame);
      if (record.GetFieldValue<bool>("flipHorizontally", false))
        this.SpriteEffect = SpriteEffects.FlipHorizontally;
      if (record.GetFieldValue<bool>("flipVertically", false))
        this.SpriteEffect = SpriteEffects.FlipVertically;
      this.Rotation = record.GetFieldValue<float>("rotation", this.Rotation);
    }

    public virtual DataFileRecord ToDataFileRecord()
    {
      DataFileRecord dataFileRecord = new DataFileRecord("Object");
      dataFileRecord.AddField("id", (object) this.Id);
      dataFileRecord.AddField("res", (object) this.ResourceId);
      dataFileRecord.AddField("sprite", (object) this.SpriteId);
      if (this.CurrentFrame != 0)
        dataFileRecord.AddField("frame", (object) this.CurrentFrame);
      if (this.SpriteEffect == SpriteEffects.FlipHorizontally)
        dataFileRecord.AddField("flipHorizontally", (object) true);
      if (this.SpriteEffect == SpriteEffects.FlipVertically)
        dataFileRecord.AddField("flipVertically", (object) true);
      if ((double) this.Rotation != 0.0)
        dataFileRecord.AddField("rotation", (object) this.Rotation);
      return dataFileRecord;
    }

    public override string ToString()
    {
      return string.IsNullOrEmpty(this.UniqueId) ? this.GetType().Name : this.UniqueId;
    }

    public BoundingSquare GetSpriteBoundingBox(int idx) => this.Sprite.BoundingBoxes[idx];

    public enum AnimtionPlaybackModes
    {
      Loop,
      PlayOnce,
    }
  }
}
