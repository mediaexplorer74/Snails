
// Type: TwoBrainsGames.BrainEngine.Graphics.SpriteAnimation
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Effects;


namespace TwoBrainsGames.BrainEngine.Graphics
{
  public class SpriteAnimation
  {
    public SpriteEffects _spriteEffect;
    public Sprite Sprite;
    public int CurrentFrame;
    public int ElapsedFrameTime;
    private bool _paused;
    public ColorEffect _fadeEffect;

    public event SpriteAnimation.LastFrameHandler OnLastFrame;

    public double TotalPlayTime => this.Sprite.TotalTime;

    public int FrameCount => this.Sprite != null ? this.Sprite.FrameCount : 0;

    public float LayerDepth
    {
      get => this.Sprite.LayerDepth;
      set => this.Sprite.LayerDepth = value;
    }

    public bool Visible { get; set; }

    public Vector2 Position { get; set; }

    public bool Autohide { get; set; }

    public TransformBlender EffectsBlender { get; set; }

    public Color BlendColor { get; set; }

    public Vector2 Scale { get; set; }

    public int PauseAtEndInMsecs { get; set; }

    public int Height => this.Sprite.Frames[0].Height;

    public SpriteAnimation(Sprite sprite)
    {
      this.Sprite = sprite;
      this.EffectsBlender = new TransformBlender();
      this.BlendColor = Color.White;
      this._paused = false;
      this.Scale = new Vector2(1f, 1f);
    }

    public SpriteAnimation(string resourceId, string resourceManagerId)
      : this(BrainGame.ResourceManager.GetSprite(resourceId, resourceManagerId))
    {
    }

    public SpriteAnimation Clone()
    {
      return new SpriteAnimation(this.Sprite)
      {
        Visible = this.Visible,
        Position = this.Position,
        Autohide = this.Autohide,
        BlendColor = this.BlendColor,
        _paused = this._paused,
        Scale = this.Scale
      };
    }

    public void SetSprite(Sprite sprite)
    {
      this.Sprite = sprite;
      this.Reset();
    }

    public void Reset()
    {
      this.CurrentFrame = 0;
      this._paused = false;
    }

    public void Update(BrainGameTime gameTime) => this.Update(gameTime, false);

    private int GetCurrentFrameTime()
    {
      if (this.Sprite.Frames[this.CurrentFrame].PlayTime != 0)
        return this.Sprite.Frames[this.CurrentFrame].PlayTime;
      return this.Sprite.Fps == 0 ? 0 : 1000 / this.Sprite.Fps;
    }

    public void Update(BrainGameTime gameTime, bool useRealTime)
    {
      if (this._paused)
        return;
      if (this.EffectsBlender.Count > 0)
      {
        this.EffectsBlender.Update(gameTime);
        this.Position += this.EffectsBlender.PositionV2;
        this.BlendColor = this.EffectsBlender.Color;
      }
      if (this.Sprite.FrameCount < 1)
        return;
      if (!useRealTime)
        this.ElapsedFrameTime += gameTime.ElapsedGameTime.Milliseconds;
      else
        this.ElapsedFrameTime += gameTime.ElapsedRealTime.Milliseconds;
      int currentFrameTime = this.GetCurrentFrameTime();
      do
      {
        if (this.ElapsedFrameTime > currentFrameTime)
        {
          ++this.CurrentFrame;
          if (this.CurrentFrame >= this.Sprite.FrameCount)
          {
            this.CurrentFrame = 0;
            if (this.OnLastFrame != null)
              this.OnLastFrame();
            this.Visible = !this.Autohide;
          }
          this.ElapsedFrameTime -= currentFrameTime;
          currentFrameTime = this.GetCurrentFrameTime();
        }
      }
      while (this.ElapsedFrameTime > currentFrameTime && currentFrameTime != 0);
    }

    public void Draw(SpriteBatch spriteBatch) => this.Draw(this.Position, spriteBatch);

    public void Draw(Vector2 position, SpriteBatch spriteBatch)
    {
      this.Sprite.Draw(position, this.CurrentFrame, 0.0f, this._spriteEffect, this.BlendColor, this.Scale.X, spriteBatch);
    }

    public void Draw(Vector2 position, float rotation, SpriteBatch spriteBatch)
    {
      this.Sprite.Draw(position, this.CurrentFrame, rotation, this._spriteEffect, this.BlendColor, this.Scale.X, spriteBatch);
    }

    public void Draw(
      Vector2 position,
      float rotation,
      SpriteEffects effect,
      SpriteBatch spriteBatch)
    {
      this.Sprite.Draw(position, this.CurrentFrame, rotation, effect, this.BlendColor, this.Scale.X, spriteBatch);
    }

    public void Draw(Vector2 position, float rotation, Color color, SpriteBatch spriteBatch)
    {
      this.Sprite.Draw(position, this.CurrentFrame, rotation, this._spriteEffect, color, this.Scale.X, spriteBatch);
    }

    public void Draw(Vector2 position, Color opacity, SpriteBatch spriteBatch)
    {
      this.Sprite.Draw(position, this.CurrentFrame, 0.0f, this._spriteEffect, opacity, this.Scale.X, spriteBatch);
    }

    public void Draw(Vector2 position, Color opacity, Vector2 scale, SpriteBatch spriteBatch)
    {
      this.Sprite.Draw(position, this.CurrentFrame, 0.0f, this.Sprite.Offset, scale.X, scale.Y, opacity, spriteBatch);
    }

    public void Draw(
      Vector2 position,
      float rotation,
      Color opacity,
      Vector2 scale,
      Vector2 pivotPoint,
      SpriteBatch spriteBatch)
    {
      this.Sprite.Draw(position, this.CurrentFrame, rotation, pivotPoint, scale.X, scale.Y, opacity, spriteBatch);
    }

    public void Draw(Rectangle destRect, Color opacity, SpriteBatch spriteBatch)
    {
      this.Sprite.Draw(destRect, this.CurrentFrame, opacity, spriteBatch);
    }

    public void IncrementFrame()
    {
      ++this.CurrentFrame;
      if (this.CurrentFrame < this.Sprite.FrameCount)
        return;
      this.CurrentFrame = 0;
      if (this.OnLastFrame == null)
        return;
      this.OnLastFrame();
    }

    public void DecrementFrame()
    {
      --this.CurrentFrame;
      if (this.CurrentFrame >= 0)
        return;
      this.CurrentFrame = this.Sprite.FrameCount - 1;
      if (this.OnLastFrame == null)
        return;
      this.OnLastFrame();
    }

    public void PausePlay() => this._paused = true;

    public void ResumePlay() => this._paused = false;

    public void FadeIn(float speed)
    {
      if (this._fadeEffect == null)
        this.CreateFadeEffect(speed);
      this.Visible = true;
      this._fadeEffect.Active = true;
      this._fadeEffect.Reset(new Color(0, 0, 0, 0), Color.White);
    }

    public void FadeOut(float speed)
    {
      if (this._fadeEffect == null)
        this.CreateFadeEffect(speed);
      this._fadeEffect.Active = true;
      this._fadeEffect.Reset(Color.White, new Color(0, 0, 0, 0));
    }

    private void CreateFadeEffect(float speed)
    {
      this._fadeEffect = new ColorEffect(Color.White, new Color(0, 0, 0, 0), speed, false);
      this._fadeEffect.AutoDeleteOnEnd = false;
      this._fadeEffect.OnEnd = new TransformEffectBase.OnEndEvent(this._fadeEffect_OnEndEvt);
      this._fadeEffect.Active = false;
      this.EffectsBlender.Add((ITransformEffect) this._fadeEffect);
    }

    private void _fadeEffect_OnEndEvt(object param)
    {
      if (this._fadeEffect.Color.A != (byte) 0)
        return;
      this.Visible = false;
    }

    public void RandomizeFrame() => this.CurrentFrame = BrainGame.Rand.Next(this.FrameCount);

    public delegate void LastFrameHandler();
  }
}
