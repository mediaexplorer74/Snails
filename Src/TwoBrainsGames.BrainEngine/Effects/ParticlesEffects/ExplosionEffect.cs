
// Type: TwoBrainsGames.BrainEngine.Effects.ParticlesEffects.ExplosionEffect
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Graphics;


namespace TwoBrainsGames.BrainEngine.Effects.ParticlesEffects
{
  public class ExplosionEffect : ParticlesEffect
  {
    public const int DEFAULT_EXPLOSION_PARTICLES = 6;
    public const float DEFAULT_MIN_SPEED = 15f;
    public const float DEFAULT_MAX_SPEED = 20f;
    private const int EXPLOSION_TIME = 10000;
    private float _x0;
    private float _y0;
    private float _minSpeed;
    private float _maxSpeed;
    private float t;
    private List<ExplosionEffectEntity> _particles;
    private int _explosionTime;
    private Sprite _fragSprite;
    private float _layerDepth;
    private Color _color;
    private int _minAngle;
    private int _maxAngle;

    private Sprite FragmentsSprite { get; set; }

    public float MinSpeed
    {
      get => this._minSpeed;
      set => this._minSpeed = value;
    }

    public float MaxSpeed
    {
      get => this._maxSpeed;
      set => this._maxSpeed = value;
    }

    public int MaxAngle
    {
      get => this._maxAngle;
      set => this._maxAngle = value;
    }

    public int MinAngle
    {
      get => this._minAngle;
      set => this._minAngle = value;
    }

    public Color Color
    {
      get => this._color;
      set => this._color = value;
    }

    public ExplosionEffect(
      float x,
      float y,
      Sprite fragSprite,
      float layerDepth,
      float gravityForce)
    {
      this._color = Color.White;
      this._x0 = x;
      this._y0 = y;
      this._fragSprite = fragSprite;
      this._maxSpeed = BrainGame.Settings.ExplosionMaxVelocity;
      this._minSpeed = BrainGame.Settings.ExplosionMinVelocity;
      this.t = 0.0f;
      this._numParticles = BrainGame.Settings.ExplosionParticles;
      this._explosionTime = 10000;
      this._ended = false;
      this._minAngle = 0;
      this._maxAngle = 356;
      this._layerDepth = layerDepth;
      this.FragmentsSprite = fragSprite;
      this.ComputeParticles(gravityForce);
    }

    public void ComputeParticles(float gravityForce)
    {
      this._particles = new List<ExplosionEffectEntity>(this._numParticles);
      for (int index = 0; index < this._numParticles; ++index)
      {
        ExplosionEffectEntity explosionEffectEntity = new ExplosionEffectEntity();
        explosionEffectEntity.Sprite = this._fragSprite;
        explosionEffectEntity.SpriteFrame = BrainGame.Rand.Next(0, this.FragmentsSprite.FrameCount - 1);
        float num = (float) BrainGame.Rand.Next((int) this._minSpeed * 100, (int) this._maxSpeed * 100) / 100f;
        explosionEffectEntity._EffectsBlender = new TransformBlender();
        float degrees = (float) BrainGame.Rand.Next(this._minAngle, this._maxAngle);
        Vector2 vector2 = new Vector2((float) Math.Cos((double) MathHelper.ToRadians(degrees)), -(float) Math.Sin((double) MathHelper.ToRadians(degrees)));
        explosionEffectEntity._EffectsBlender.Add((ITransformEffect) new MotionEffect(gravityForce, vector2 * num));
        explosionEffectEntity._EffectsBlender.Add((ITransformEffect) new RotationEffect((float) BrainGame.Rand.Next(-15, 15)));
        explosionEffectEntity.Position = new Vector2(this._x0, this._y0);
        this._particles.Add(explosionEffectEntity);
      }
    }

    public override void Update(BrainGameTime gameTime)
    {
      if (this._explosionTime <= 0)
      {
        this._ended = true;
      }
      else
      {
        foreach (ExplosionEffectEntity particle in this._particles)
        {
          particle._EffectsBlender.Update(gameTime);
          particle.Position += particle._EffectsBlender.PositionV2;
          particle.Rotation += particle._EffectsBlender.Rotation;
        }
        this.t += (float) gameTime.ElapsedGameTime.Milliseconds / 1000f;
        this._explosionTime -= gameTime.ElapsedGameTime.Milliseconds;
      }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      if (this._ended)
        return;
      foreach (ExplosionEffectEntity particle in this._particles)
        particle.Sprite.Draw(particle.Position, particle.SpriteFrame, particle.Rotation, SpriteEffects.None, this._layerDepth, this._color, 1f, spriteBatch);
    }
  }
}
