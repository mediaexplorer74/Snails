
// Type: TwoBrainsGames.Snails.StageObjects.ControllableLaserCannon
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  internal class ControllableLaserCannon : 
    LaserCannonBase,
    ICursorInteractable,
    IRotationControllable
  {
    public const float MAX_ROTATION = 40f;
    public const float MIN_ROTATION = -40f;
    private const int CANNON_POSITION_BS_IDX = 0;
    private const int ON_OFF_BUTTON_IDX = 1;
    private const int CONTROLLER_IDX = 2;
    private const int BEAM_ORIGIN_BS_IDX = 0;
    private const int GLOW_BS_IDX = 1;
    private const double TURNING_ON_TIME = 1000.0;
    private float _cannonRotation;
    private Vector2 _cannonPosition;
    private Vector2 _onOffButtonPosition;
    private Vector2 _glowPosition;
    private Sprite _cannonSprite;
    private Sprite _onOffButtonSprite;
    private Sprite _sphereSprite;
    private SpriteAnimation _laserStartAnimation;
    private SpriteAnimation _laserGlowAnimation;
    private RotationController _controller;
    private Sample _laserStartUpSound;

    public float CannonRotation
    {
      get => this._cannonRotation;
      set => this._cannonRotation = value;
    }

    public ControllableLaserCannon()
      : base(StageObjectType.ControllableLaserCannon)
    {
      this._controller = new RotationController((IRotationControllable) this);
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this._cannonSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/laser-beam", "LaserBeamCannon");
      this._onOffButtonSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/laser-beam", "CannonState");
      this._sphereSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/laser-beam", "CannonSphere");
      this._laserStartAnimation = new SpriteAnimation(BrainGame.ResourceManager.GetSpriteTemporary("spriteset/laser-beam", "LaserBeamStart"));
      this._laserGlowAnimation = new SpriteAnimation(BrainGame.ResourceManager.GetSpriteTemporary("spriteset/laser-beam", "CannonGlow"));
      this._controller.LoadContent();
      this._laserStartUpSound = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/laser-beam-power-up", (Object2D) this);
    }

    public override void Initialize()
    {
      base.Initialize();
      this._laserStartAnimation.BlendColor = this._laserXnaColor;
      this._laserGlowAnimation.BlendColor = this._laserXnaColor;
      this.UpdateCannon();
      this._controller.Initialize();
    }

    private void UpdateCannon()
    {
      if ((double) this._cannonRotation > 40.0)
        this._cannonRotation = 40f;
      if ((double) this._cannonRotation < -40.0)
        this._cannonRotation = -40f;
      int num = this.IsHorizontallyFlipped ? -1 : 1;
      this._onOffButtonPosition = Mathematics.TransformVector(new Vector2(this.Sprite.BoundingBoxes[1].Center.X * (float) num, this.Sprite.BoundingBoxes[1].Center.Y), this.Rotation, this.Position);
      this._cannonPosition = this.TransformSpriteFrameBB(0).GetCenter();
      Vector2 toTransform = Mathematics.TransformVector(new Vector2(this._cannonSprite.BoundingBoxes[0].Center.X * (float) num, this._cannonSprite.BoundingBoxes[0].Center.Y), this.CannonRotation, new Vector2(this.Sprite.BoundingBoxes[0].Center.X * (float) num, this.Sprite.BoundingBoxes[0].Center.Y));
      this._laserBeam.Position = Mathematics.TransformVector(toTransform, this.Rotation, this.Position);
      this._laserBeam.SpriteEffect = this.SpriteEffect;
      this._laserBeam.BeamRotation = (float) ((double) this.CannonRotation + (double) this.Rotation + 90.0 + (this.IsHorizontallyFlipped ? 180.0 : 0.0));
      toTransform = new Vector2(this._cannonSprite.BoundingBoxes[1].Center.X * (float) num, this._cannonSprite.BoundingBoxes[1].Center.Y);
      this._glowPosition = Mathematics.TransformVector(Mathematics.TransformVector(toTransform, this.CannonRotation, new Vector2(this.Sprite.BoundingBoxes[0].Center.X * (float) num, this.Sprite.BoundingBoxes[0].Center.Y)), this.Rotation, this.Position);
      this._controller.SetPosition(this.Sprite.BoundingBoxes[2].UpperLeft);
    }

    public override void StageStartupPhaseEnded()
    {
      base.StageStartupPhaseEnded();
      if (this._state != LaserCannonBase.LaserBeamSourceState.TurningOn)
        return;
      this._laserStartUpSound.Play();
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      if (this._state == LaserCannonBase.LaserBeamSourceState.TurningOn)
      {
        this._laserBeam.Visible = false;
        this._laserStartAnimation.Visible = true;
        this._laserStartAnimation.Update(gameTime);
        this._turningOnTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
        if (this._turningOnTime <= 0.0)
        {
          this._state = LaserCannonBase.LaserBeamSourceState.On;
          this._laserBeam.Visible = true;
        }
      }
      if (this._controller.Contains(Stage.CurrentStage.Cursor.Position) && !Stage.CurrentStage.Cursor.IsInteractingWithObject)
        Stage.CurrentStage.Cursor.SetInteractingObject((ICursorInteractable) this);
      this._controller.Update(gameTime);
    }

    public override void Draw(bool shadow)
    {
      base.Draw(shadow);
      this._onOffButtonSprite.Draw(this._onOffButtonPosition, (int) this._state, this.Rotation, this.SpriteEffect, Stage.CurrentStage.SpriteBatch);
      if (this._state == LaserCannonBase.LaserBeamSourceState.TurningOn)
        this._laserStartAnimation.Draw(this._laserBeam.Position, this.CannonRotation + this.Rotation, this.SpriteEffect, Stage.CurrentStage.SpriteBatch);
      if (!shadow)
      {
        this._sphereSprite.Draw(this._glowPosition, 0, this.CannonRotation + this.Rotation, this.SpriteEffect, this._laserXnaColor, 1f, Stage.CurrentStage.SpriteBatch);
        this._cannonSprite.Draw(this._cannonPosition, 0, this.CannonRotation + this.Rotation, this.SpriteEffect, Stage.CurrentStage.SpriteBatch);
      }
      else
      {
        this._sphereSprite.Draw(this._glowPosition + GenericConsts.ShadowDepth, 0, this.CannonRotation + this.Rotation, this.SpriteEffect, this.ShadowColor, 1f, Stage.CurrentStage.SpriteBatch);
        this._cannonSprite.Draw(this._cannonPosition + GenericConsts.ShadowDepth, 0, this.CannonRotation + this.Rotation, this.SpriteEffect, this.ShadowColor, 1f, Stage.CurrentStage.SpriteBatch);
      }
      this._controller.Draw();
      if (!this.TurnedOn)
        return;
      this._laserGlowAnimation.Draw(this._glowPosition, 0.0f, this.SpriteEffect, Stage.CurrentStage.SpriteBatch);
    }

    public override void ForegroundDraw()
    {
      base.ForegroundDraw();
      this._controller.DrawForeground();
    }

    protected override void SetTurningOnState(bool playSound)
    {
      this.TurnedOn = true;
      this._laserStartAnimation.Visible = false;
      this._turningOnTime = 1000.0;
      this._blinkEffect.Active = false;
      this._state = LaserCannonBase.LaserBeamSourceState.TurningOn;
      if (!playSound)
        return;
      this._laserStartUpSound.Play();
    }

    public StageCursor.CursorType QueryCursor() => this._controller.QueryCursor();

    public bool QueryInterating() => this._controller.QueryInterating();

    public void CursorActionPressed(Vector2 cursorPos)
    {
      this._controller.CursorActionPressed(cursorPos);
    }

    public void CursorActionReleased() => this._controller.CursorActionReleased();

    public void CursorActionSelected()
    {
    }

    public StageObject ControlledObject => (StageObject) this;

    public bool ControllerValueChanged(float value)
    {
      float cannonRotation = this._cannonRotation;
      this._cannonRotation -= value;
      this.UpdateCannon();
      return (double) cannonRotation != (double) this._cannonRotation;
    }

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord(context);
      dataFileRecord.AddField("cannonRotation", (object) this.CannonRotation);
      return dataFileRecord;
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this.CannonRotation = record.GetFieldValue<float>("cannonRotation", this.CannonRotation);
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }
  }
}
