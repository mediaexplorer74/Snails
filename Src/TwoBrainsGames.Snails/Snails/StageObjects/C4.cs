
// Type: TwoBrainsGames.Snails.StageObjects.C4
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.Effects;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  internal class C4 : MovingObject, ISwitchable
  {
    private const int LIGHT_POS_BB_IDX = 1;
    private const int RED_LIGHT_FRAME = 1;
    private const int GREEN_LIGHT_FRAME = 0;
    private Sprite _c4LightSprite;
    private Vector2 _lightPosition;
    private int _lightFrame;
    private BlinkEffect _lightEffect;
    private Sample _beepSound;

    public C4()
      : base(StageObjectType.C4)
    {
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this._c4LightSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/stage-objects", "C4Light");
      this._beepSound = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/C4-beep", (Object2D) this);
    }

    public override void Initialize()
    {
      base.Initialize();
      this._lightPosition = this.Sprite.BoundingBoxes[1].Center;
      this._lightEffect = new BlinkEffect(250.0, 2000.0, this._beepSound, 0.0, 0.0);
      this._lightFrame = 1;
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      if (this.IsDead || this.IsDisposed)
        return;
      if (Stage.CurrentStage.Board.GetTileAt(this.BoardY, this.BoardX) == null)
      {
        this.DynamicFlags = StageObjectDynamicFlags.IsVisible;
        this.StaticFlags = StageObjectStaticFlags.CanFall | StageObjectStaticFlags.CanDieWithExplosions | StageObjectStaticFlags.CanDieWithAnyTypeOfExplosion;
        this.DettachFromPath();
      }
      this._lightEffect.Update(gameTime);
    }

    public override void Draw(bool shadow)
    {
      base.Draw(shadow);
      if (!this._lightEffect.Visible)
        return;
      this._c4LightSprite.Draw(this.Position + this._lightPosition, this._lightFrame, Stage.CurrentStage.SpriteBatch);
    }

    public override void KillByExplosion(Explosion exp) => this.Explode();

    private void Explode()
    {
      this.Explode(Explosion.ExplosionSize.Small, Explosion.ExplosionSize.Medium, Explosion.ExplosionRadiusType.Circle, Explosion.ObjectTypeAffected.All, this.Position, true, (Sprite) null, (Sprite) null, true, false);
    }

    public void SwitchOn() => this.Explode();

    public void SwitchOff()
    {
    }

    public bool IsOn => false;

    private enum C4State
    {
      Idle,
      Beeping,
      Exploded,
    }
  }
}
