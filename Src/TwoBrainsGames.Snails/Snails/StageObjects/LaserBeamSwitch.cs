
// Type: TwoBrainsGames.Snails.StageObjects.LaserBeamSwitch
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  internal class LaserBeamSwitch : Switch
  {
    private const int RECEIVER_POS_BS_IDX = 0;
    private const int POINTER_POS_BB_IDX = 0;
    private const int RED_POS_BB_IDX = 1;
    private const int GREEN_POS_BB_IDX = 2;
    private const int ON_TEXT_POS_BB_IDX = 3;
    private const double ACTIVATION_TIME = 5000.0;
    private const float MAX_POINTER_ANGLE = 310f;
    private bool _laserBeamCollided;
    private Sprite _lightningSprite;
    private Sprite _lightsSprite;
    private Sprite _pointerSprite;
    private Sprite _sphereSprite;
    private SpriteAnimation _lightningAnimation;
    private BoundingCircle _bsReceiver;
    private BoundingSquare _switchBB;
    private float _charge;
    private Vector2 _pointerPosition;
    private float _pointerAngle;
    private Vector2 _lightPosition;
    private int _lightFrame;
    private ColorEffect _colorEffect;
    protected Sample _electricitySound;
    protected Sample _switchSound;

    public LaserBeam.LaserBeamColor LaserColor { get; set; }

    public override BoundingSquare QuadtreeCollisionBB => this._switchBB;

    public LaserBeamSwitch()
      : base(StageObjectType.LaserBeamSwitch)
    {
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this._lightningSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/laser-beam", "SwitchLightning");
      this._lightsSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/laser-beam", "SwitchLights");
      this._pointerSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/laser-beam", "SwitchPointer");
      this._sphereSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/laser-beam", "SwitchSphere");
      this._electricitySound = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/laser-switch-electricity", (Object2D) this);
      this._switchSound = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/switch-lever-activating", (Object2D) this);
    }

    public override void Initialize()
    {
      base.Initialize();
      this._lightningAnimation = new SpriteAnimation(this._lightningSprite);
      this._lightningAnimation.Visible = false;
      this._colorEffect = new ColorEffect(LaserBeam.GetXnaColor(this.LaserColor), new Color(180, 180, 180), 0.01f, false);
      this.Refresh();
    }

    private void Refresh()
    {
      this._bsReceiver = this.Sprite._boundingSpheres[0].Transform(this.Position);
      this._switchBB = this._bsReceiver.GetContainingSquare();
      this._lightningAnimation.Position = this._bsReceiver._center;
      this._pointerPosition = this.TransformSpriteFrameBB(0).GetCenter();
      this._lightPosition = this.TransformSpriteFrameBB(1).P0;
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      if (this.IsOn)
      {
        if (this._colorEffect.Ended)
          return;
        this._colorEffect.Update(gameTime);
      }
      else
      {
        if (!this._lightningAnimation.Visible)
          return;
        this._lightningAnimation.Update(gameTime);
      }
    }

    public override void AfterUpdate(BrainGameTime gameTime)
    {
      base.AfterUpdate(gameTime);
      if (this.IsOn)
        return;
      if (this._laserBeamCollided)
      {
        this._electricitySound.Play(true);
        this._charge += (float) (gameTime.ElapsedGameTime.TotalMilliseconds * 100.0 / 5000.0);
        if ((double) this._charge > 100.0)
        {
          this._charge = 100f;
          this._lightFrame = 1;
          this._lightPosition = this.TransformSpriteFrameBB(2).P0;
          this._pointerAngle = 310f;
          this._lightningAnimation.Visible = false;
          this.SwitchOn();
          this._electricitySound.Stop();
          this._switchSound.Play();
        }
        this._pointerAngle = (float) ((double) this._charge * 310.0 / 100.0);
      }
      else
      {
        this._electricitySound.Stop();
        this._charge -= (float) (gameTime.ElapsedGameTime.TotalMilliseconds * 100.0 / 5000.0 * 3.0);
        if ((double) this._charge < 0.0)
          this._charge = 0.0f;
        this._pointerAngle = (float) ((double) this._charge * 310.0 / 100.0);
        this._lightningAnimation.Visible = false;
      }
      this._laserBeamCollided = false;
    }

    public override void Draw(bool shadow)
    {
      if (shadow)
        this._sphereSprite.Draw(this._bsReceiver._center + GenericConsts.ShadowDepth, 0, this.ShadowColor, Stage.CurrentStage.SpriteBatch);
      else
        this._sphereSprite.Draw(this._bsReceiver._center, 0, this._colorEffect.Color, Stage.CurrentStage.SpriteBatch);
      base.Draw(shadow);
      if (this._lightningAnimation.Visible)
        this._lightningAnimation.Draw(Stage.CurrentStage.SpriteBatch);
      this._lightsSprite.Draw(this._lightPosition, this._lightFrame, Stage.CurrentStage.SpriteBatch);
      this._pointerSprite.Draw(this._pointerPosition, 0, this._pointerAngle, SpriteEffects.None, Stage.CurrentStage.SpriteBatch);
      if (!Game1.GameSettings.ShowBoundingBoxes)
        return;
      this._bsReceiver.Draw(Color.Red, Stage.CurrentStage.Camera.Position);
    }

    public bool OnLaserBeamCollided(LaserBeam beam, out Vector2 collidingPoint)
    {
      if (this.IsOff)
      {
        if (this._bsReceiver.IntersectsLine(beam.BeamOrigin, beam.BeamEndPoint, out collidingPoint))
        {
          if (beam.LaserColor != this.LaserColor || this.IsOn)
            return true;
          this._laserBeamCollided = true;
          this._lightningAnimation.Visible = true;
          return true;
        }
      }
      else
        collidingPoint = Vector2.Zero;
      return false;
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this.LaserColor = (LaserBeam.LaserBeamColor) Enum.Parse(typeof (LaserBeam.LaserBeamColor), record.GetFieldValue<string>("colorType", this.LaserColor.ToString()), true);
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord(context);
      dataFileRecord.AddField("colorType", (object) this.LaserColor.ToString());
      return dataFileRecord;
    }
  }
}
