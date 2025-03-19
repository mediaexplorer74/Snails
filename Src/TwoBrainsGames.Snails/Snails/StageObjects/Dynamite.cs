
// Type: TwoBrainsGames.Snails.StageObjects.Dynamite
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class Dynamite : MovingObject
  {
    public const string ID = "DYNAMITE";
    public const int EXPLOSION_TIME = 1300;
    protected Sample _fuseSample;
    protected Sample _beepSample;
    private Sprite _spriteExplosionRadius;
    private Sprite _fuseSprite;
    private Sprite _fuseFlameSprite;
    private SpriteAnimation _fuseAnimation;
    private SpriteAnimation _fuseFlameAnimation;
    private double _blinkTime;
    private double _blinkCurrentTime;
    private bool _radiusVisible;
    private Dynamite.DynamiteState _state;

    public Dynamite()
      : base(StageObjectType.Dynamite)
    {
      this.FramesPerTime = 1300;
    }

    public Dynamite(StageObjectType type)
      : base(type)
    {
      this.FramesPerTime = 1300;
    }

    public Dynamite(Dynamite other)
      : base((StageObject) other)
    {
      this.Copy((StageObject) other);
    }

    public override void Copy(StageObject other)
    {
      base.Copy(other);
      this._fuseSample = (other as Dynamite)._fuseSample;
      this._beepSample = (other as Dynamite)._beepSample;
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this._fuseSample = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/fuse", (Object2D) this);
      this._beepSample = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/dynamite-beep", (Object2D) this);
      this._spriteExplosionRadius = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/dynamite", "DynamiteRadius");
      this._fuseSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/dynamite", "Fuse");
      this._fuseFlameSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/dynamite", "Flame");
    }

    public override void Initialize()
    {
      base.Initialize();
      this._blinkTime = 300.0;
      this._blinkCurrentTime = 0.0;
      this._radiusVisible = true;
      this._fuseAnimation = new SpriteAnimation(this._fuseSprite);
      this._fuseAnimation.OnLastFrame += new SpriteAnimation.LastFrameHandler(this._fuseAnimation_OnLastFrame);
      this._fuseFlameAnimation = new SpriteAnimation(this._fuseFlameSprite);
    }

    private void _fuseAnimation_OnLastFrame() => this.Explode();

    public override void OnAddedToStage() => this._fuseSample.Play(true);

    public void Explode()
    {
      this._fuseSample.Stop();
      this.Explode(Explosion.ExplosionSize.Medium, Explosion.ExplosionSize.Medium, Explosion.ExplosionRadiusType.Circle, Explosion.ObjectTypeAffected.All, this.Position, false, (Sprite) null, (Sprite) null, true, false);
    }

    public override void KillByFire() => this.Explode();

    public override void StopSamples()
    {
      this._fuseSample.Stop();
      base.StopSamples();
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      if (this._state != Dynamite.DynamiteState.Burning)
        return;
      this._fuseAnimation.Update(gameTime);
      this._fuseFlameAnimation.Update(gameTime);
      this._blinkCurrentTime += gameTime.ElapsedGameTime.TotalMilliseconds;
      if (this._blinkCurrentTime <= this._blinkTime)
        return;
      this._radiusVisible = !this._radiusVisible;
      this._blinkCurrentTime -= this._blinkTime;
      this._blinkTime *= 0.92000001668930054;
      if (this._radiusVisible)
        this._beepSample.Play();
      if (this._blinkTime >= 20.0)
        return;
      this._blinkTime = 20.0;
    }

    public override void Draw(bool shadow)
    {
      base.Draw(shadow);
      this._fuseAnimation.Draw(this.Position, Stage.CurrentStage.SpriteBatch);
      if (this._state != Dynamite.DynamiteState.Burning)
        return;
      this._fuseFlameAnimation.Draw(this.Position, Stage.CurrentStage.SpriteBatch);
    }

    public override void ForegroundDraw()
    {
      base.ForegroundDraw();
      if (!this._radiusVisible)
        return;
      this._spriteExplosionRadius.Draw(this.Position, Stage.CurrentStage.SpriteBatch);
    }

    public override void OnEnterLiquid(Liquid liquid)
    {
      switch (liquid)
      {
        case Water _:
          base.OnEnterLiquid(liquid);
          this._fuseSample.Stop();
          this.SpriteAnimationActive = false;
          this.Extinguish();
          break;
        case Acid _:
          this.Explode();
          break;
      }
    }

    public void Extinguish()
    {
      this._radiusVisible = false;
      this._blinkCurrentTime = 0.0;
      this._fuseFlameAnimation.Visible = false;
      this._fuseAnimation.PausePlay();
      this._state = Dynamite.DynamiteState.Extinguished;
    }

    private enum DynamiteState
    {
      Burning,
      Extinguished,
    }
  }
}
