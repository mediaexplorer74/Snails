
// Type: TwoBrainsGames.Snails.StageObjects.Crystal
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.ParticlesEffects;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  internal class Crystal : SingleLightEmitter
  {
    private const int BB_IDX_LIGHT_SOURCE = 0;
    private const int BB_IDX_SHINE_FIRST = 1;
    private const int BB_IDX_SHINE_LAST = 5;
    private const int BB_IDX_EXPLOSION_COLLISION = 6;
    private const float BIG_CRYSTAL_LIGHT_SIZE = 6f;
    private const float MEDIUM_CRYSTAL_LIGHT_SIZE = 5f;
    private const float SMALL_CRYSTAL_LIGHT_SIZE = 3f;
    private float _maxLightScale;
    private Crystal.CrystalSizeType _cristalSize;
    private Crystal.CrystalColorType _cristalColor;
    private SpriteAnimation _shineAnimation;
    private double _shineShowTimer;
    private Sprite _bigCrystalSprite;
    private Sprite _mediumCrystalSprite;
    private Sprite _smallCrystalSprite;
    private Sprite _fragmentsSprite;
    private BoundingSquare _quadtreeCollisionBB;
    private List<Explosion> _explosionList;

    public Crystal.CrystalColorType CristalColor
    {
      get => this._cristalColor;
      set
      {
        this._cristalColor = value;
        switch (this._cristalColor)
        {
          case Crystal.CrystalColorType.Red:
            this.BlendColor = Color.Red;
            break;
          case Crystal.CrystalColorType.Green:
            this.BlendColor = Color.Green;
            break;
          case Crystal.CrystalColorType.Blue:
            this.BlendColor = Color.Blue;
            break;
          case Crystal.CrystalColorType.Yellow:
            this.BlendColor = Color.Yellow;
            break;
          case Crystal.CrystalColorType.Orange:
            this.BlendColor = Color.Orange;
            break;
        }
      }
    }

    public Crystal.CrystalSizeType CristalSize
    {
      get => this._cristalSize;
      set
      {
        this._cristalSize = value;
        this.Refresh();
      }
    }

    private bool IsShineVisible => this._shineAnimation.Visible;

    public override BoundingSquare QuadtreeCollisionBB => this._quadtreeCollisionBB;

    public Crystal()
      : base(StageObjectType.Crystal)
    {
      this.SpriteAnimationActive = false;
      this._explosionList = new List<Explosion>();
    }

    public override void Copy(StageObject other)
    {
      base.Copy(other);
      Crystal crystal = (Crystal) other;
      this.CristalColor = crystal.CristalColor;
      this.CristalSize = crystal.CristalSize;
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this._shineAnimation = new SpriteAnimation(BrainGame.ResourceManager.GetSpriteTemporary("spriteset/crystal/Shine"));
      this._shineAnimation.OnLastFrame += new SpriteAnimation.LastFrameHandler(this._shineAnimation_OnLastFrame);
      this._shineAnimation.Autohide = true;
      this._bigCrystalSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/crystal/CrystalBig");
      this._mediumCrystalSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/crystal/CrystalMedium");
      this._smallCrystalSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/crystal/CrystalSmall");
      this._fragmentsSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/crystal/Fragments");
    }

    private void _shineAnimation_OnLastFrame() => this.RandomizeShineShowTimer();

    public override void Initialize()
    {
      base.Initialize();
      this.LightSource.EffectsBlender.Add((ITransformEffect) new ColorEffect(this.BlendColor, new Color((float) this.BlendColor.R, (float) this.BlendColor.G, (float) this.BlendColor.B, 0.2f), 0.01f, true));
      this._shineAnimation.Visible = false;
      this.RandomizeShineShowTimer();
      this.Refresh();
      this.LightSource.Position = this.GetBoundingBoxTransformed(0).ToBoundingSquare().UpperLeft;
    }

    public override void KillByExplosion(Explosion exp)
    {
      if (this._explosionList.Contains(exp))
        return;
      this._explosionList.Add(exp);
      switch (this._cristalSize)
      {
        case Crystal.CrystalSizeType.Big:
          this.CristalSize = Crystal.CrystalSizeType.Medium;
          break;
        case Crystal.CrystalSizeType.Medium:
          this.CristalSize = Crystal.CrystalSizeType.Small;
          break;
        case Crystal.CrystalSizeType.Small:
          this.DisposeFromStage();
          break;
      }
      ExplosionEffect explosionEffect = new ExplosionEffect(this.Position.X, this.Position.Y, this._fragmentsSprite, 1f, Game1.GameSettings.Gravity);
      explosionEffect.MinSpeed = 30f;
      explosionEffect.MaxSpeed = 70f;
      explosionEffect.MinAngle = 30;
      explosionEffect.MaxAngle = 120;
      explosionEffect.Color = this.BlendColor;
      explosionEffect.ComputeParticles(Game1.GameSettings.Gravity);
      Stage.CurrentStage.Particles.Add((ParticlesEffect) explosionEffect);
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      float num = this._maxLightScale - (float) ((double) ((int) byte.MaxValue - (int) this.LightSource.Color.A) / 10.0 / 256.0);
      this.LightSource.Scale = new Vector2(num, num);
      if (!this.IsShineVisible)
      {
        this._shineShowTimer -= gameTime.ElapsedGameTime.TotalMilliseconds;
        if (this._shineShowTimer <= 0.0)
        {
          this._shineAnimation.Visible = true;
          this._shineAnimation.Reset();
          this._shineAnimation.Position = this.GetBoundingBoxTransformed(1 + BrainGame.Rand.Next(4)).ToBoundingSquare().UpperLeft;
        }
      }
      else
        this._shineAnimation.Update(gameTime);
      for (int index = 0; index < this._explosionList.Count; ++index)
      {
        if (this._explosionList[index].IsDisposed)
        {
          this._explosionList.RemoveAt(index);
          --index;
        }
      }
    }

    public override void Draw(bool shadow)
    {
      base.Draw(shadow);
      this.Sprite.Draw(this.Position, 1, this._rotation, SpriteEffects.None, Stage.CurrentStage.SpriteBatch);
      if (!this.IsShineVisible)
        return;
      this._shineAnimation.Draw(Stage.CurrentStage.SpriteBatch);
    }

    private void Refresh()
    {
      switch (this._cristalSize)
      {
        case Crystal.CrystalSizeType.Big:
          this.LightSource.Scale = new Vector2(6f, 6f);
          if (this._bigCrystalSprite != null)
          {
            this.Sprite = this._bigCrystalSprite;
            break;
          }
          break;
        case Crystal.CrystalSizeType.Medium:
          this.LightSource.Scale = new Vector2(5f, 5f);
          if (this._bigCrystalSprite != null)
          {
            this.Sprite = this._mediumCrystalSprite;
            break;
          }
          break;
        case Crystal.CrystalSizeType.Small:
          this.LightSource.Scale = new Vector2(3f, 3f);
          if (this._smallCrystalSprite != null)
          {
            this.Sprite = this._smallCrystalSprite;
            break;
          }
          break;
      }
      this._maxLightScale = this.LightSource.Scale.X;
      if (this.Sprite == null)
        return;
      this._quadtreeCollisionBB = this.GetBoundingBoxTransformed(6).ToBoundingSquare();
    }

    private void RandomizeShineShowTimer()
    {
      this._shineShowTimer = (double) BrainGame.Rand.Next(1000);
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this.CristalColor = (Crystal.CrystalColorType) Enum.Parse(typeof (Crystal.CrystalColorType), record.GetFieldValue<string>("colorType", this.CristalColor.ToString()), true);
      this.CristalSize = (Crystal.CrystalSizeType) Enum.Parse(typeof (Crystal.CrystalSizeType), record.GetFieldValue<string>("cristalSize", this.CristalSize.ToString()), true);
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord(context);
      dataFileRecord.AddField("colorType", (object) this.CristalColor.ToString());
      dataFileRecord.AddField("cristalSize", (object) this.CristalSize.ToString());
      return dataFileRecord;
    }

    public enum CrystalColorType
    {
      Red,
      Green,
      Blue,
      Yellow,
      Orange,
    }

    public enum CrystalSizeType
    {
      Big,
      Medium,
      Small,
    }
  }
}
