
// Type: TwoBrainsGames.Snails.StageObjects.SnailTriggerSwitch
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class SnailTriggerSwitch : Switch
  {
    public const string ID = "TRIGGER_SWITCH";
    private const int BB_IDX_HANDLE = 1;
    private const int BB_IDX_LIGHT = 2;
    private const float HANDLE_ANGLE = 45f;
    private const float HANDLE_SPEED = 30f;
    private Sprite _handleSprite;
    private Sprite _lightSprite;
    private Sample _leverMovingSound;
    private Sample _leverAcivatingSound;
    private Vector2 _handlePosition;
    private Vector2 _lightPosition;
    private SnailTriggerSwitch.SnailTriggerSwitchState _switchState;
    private float _handleRotation;
    private int _handleDirection;
    private List<Snail> _snailsInsideSwitch;

    private MovingObject.WalkDirection SwitchActivationDirection
    {
      get
      {
        return this.IsHorizontallyFlipped ? (!this.IsOn ? MovingObject.WalkDirection.CounterClockwise : MovingObject.WalkDirection.Clockwise) : (!this.IsOff ? MovingObject.WalkDirection.CounterClockwise : MovingObject.WalkDirection.Clockwise);
      }
    }

    public SnailTriggerSwitch()
      : this(StageObjectType.TriggerSwitch)
    {
    }

    public SnailTriggerSwitch(StageObjectType type)
      : base(type)
    {
      this._snailsInsideSwitch = new List<Snail>();
    }

    public override void Copy(StageObject other) => base.Copy(other);

    public override void LoadContent()
    {
      base.LoadContent();
      this._handleSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/simple-switch/Handle");
      this._lightSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/simple-switch/Lights");
      this._leverMovingSound = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/switch-lever-moving", (Object2D) this);
      this._leverAcivatingSound = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/switch-lever-activating", (Object2D) this);
    }

    public override void Initialize()
    {
      base.Initialize();
      this._crateCollisionBB = this.GetCurrentFrameRectTransformed();
      this._handlePosition = this.TransformSpriteFrameBB(1).ToBoundingSquare().UpperLeft;
      this._lightPosition = this.TransformSpriteFrameBB(2).ToBoundingSquare().UpperLeft;
      if (this.IsOff)
      {
        this._handleRotation = -45f;
        this._handleDirection = 1;
      }
      else
      {
        this._handleRotation = 45f;
        this._handleDirection = -1;
      }
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      this.DoQuadtreeCollisions(0);
      if (this._switchState != SnailTriggerSwitch.SnailTriggerSwitchState.Idle)
      {
        this._handleRotation += (float) (gameTime.ElapsedGameTime.TotalMilliseconds / 100.0 * (double) this._handleDirection * 30.0);
        if ((double) Math.Abs(this._handleRotation) > 45.0)
        {
          this._handleRotation = 45f * (float) this._handleDirection;
          this._handleDirection *= -1;
          if (this._switchState == SnailTriggerSwitch.SnailTriggerSwitchState.SwitchingOff)
            this.SwitchOff();
          else
            this.SwitchOn();
          this._leverAcivatingSound.Play();
          this._switchState = SnailTriggerSwitch.SnailTriggerSwitchState.Idle;
        }
      }
      this.UpdateSnailsInsideSwitchList();
    }

    private void UpdateSnailsInsideSwitchList()
    {
      for (int index = 0; index < this._snailsInsideSwitch.Count; ++index)
      {
        if (!this.CollidesWithSnail(this._snailsInsideSwitch[index]))
        {
          this._snailsInsideSwitch.Remove(this._snailsInsideSwitch[index]);
          --index;
        }
      }
    }

    private bool CollidesWithSnail(Snail snail)
    {
      return snail.CheckCollisionWithHead(this.SnailCollisionBB);
    }

    public override void Draw(bool shadow)
    {
      float handleRotation = this._handleRotation;
      if (this.IsHorizontallyFlipped)
        handleRotation *= -1f;
      Vector2 zero = Vector2.Zero;
      Color opacity = this.BlendColor;
      if (shadow)
      {
        zero += GenericConsts.ShadowDepth;
        opacity = this.ShadowColor;
      }
      this._handleSprite.Draw(this._handlePosition + zero, 0, handleRotation + this.Rotation, this.SpriteEffect, opacity, 1f, Stage.CurrentStage.SpriteBatch);
      base.Draw(shadow);
      this._lightSprite.Draw(this._lightPosition, (int) this.State, Stage.CurrentStage.SpriteBatch);
    }

    protected override void OnSnailCollided(Snail snail)
    {
      if (this._switchState == SnailTriggerSwitch.SnailTriggerSwitchState.Idle && snail.CanActivateSwitch && snail.Direction == this.SwitchActivationDirection && this.CollidesWithSnail(snail) && !this._snailsInsideSwitch.Contains(snail))
      {
        this._leverMovingSound.Play();
        this._switchState = !this.IsOff ? SnailTriggerSwitch.SnailTriggerSwitchState.SwitchingOff : SnailTriggerSwitch.SnailTriggerSwitchState.SwitchingOn;
      }
      if (this._switchState == SnailTriggerSwitch.SnailTriggerSwitchState.Idle || this._snailsInsideSwitch.Contains(snail))
        return;
      this._snailsInsideSwitch.Add(snail);
    }

    public override bool CrateToolIsValid(BoundingSquare crateBs)
    {
      return !crateBs.Collides(this._crateCollisionBB);
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this.State = (Switch.SwitchState) Enum.Parse(typeof (Switch.SwitchState), record.GetFieldValue<string>("state", Switch.SwitchState.On.ToString()), true);
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord(context);
      dataFileRecord.AddField("state", (object) this.State.ToString());
      return dataFileRecord;
    }

    private enum SnailTriggerSwitchState
    {
      Idle,
      SwitchingOn,
      SwitchingOff,
    }
  }
}
