
// Type: TwoBrainsGames.Snails.StageObjects.LiquidPump
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  internal class LiquidPump : 
    StageObject,
    ISwitchable,
    ISnailsDataFileSerializable,
    IDataFileSerializable
  {
    private const int WHEEL1_POS_BB_IDX = 0;
    private const int WHEEL2_POS_BB_IDX = 1;
    private const float WHEEL_ROTATION_SPEED = 150f;
    private Sprite _wheel1Sprite;
    private Sprite _wheel2Sprite;
    private SpriteAnimation _chainAnim;
    private Vector2 _wheel1Pos;
    private Vector2 _wheel2Pos;
    private float _wheelRotation;
    private LiquidPump.PumpState _state;
    private Sample _engineSound;

    public LiquidPump.PumpTypes PumpType { get; set; }

    public float PumpSpeed { get; set; }

    public LiquidPump()
      : base(StageObjectType.LiquidPump)
    {
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this._wheel1Sprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/liquid-pump", "Wheel1");
      this._wheel2Sprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/liquid-pump", "Wheel2");
      this._chainAnim = new SpriteAnimation(BrainGame.ResourceManager.GetSpriteTemporary("spriteset/liquid-pump", "Chain"));
      this._engineSound = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/water-pump", (Object2D) this);
    }

    public override void Initialize()
    {
      base.Initialize();
      this._wheel1Pos = this.TransformSpriteFrameBB(0).GetCenter();
      this._wheel2Pos = this.TransformSpriteFrameBB(1).GetCenter();
      this._chainAnim.Position = this._wheel1Pos;
      this._wheelRotation = 0.0f;
      this._state = LiquidPump.PumpState.Idle;
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      if (this._state != LiquidPump.PumpState.Working)
        return;
      this._chainAnim.Update(gameTime);
      this._wheelRotation += (float) (150.0 * gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0);
      if (this.PumpType == LiquidPump.PumpTypes.PumpIn)
      {
        bool flag = true;
        foreach (Liquid linkedObject in this.LinkedObjects)
        {
          if (!linkedObject.IsFull)
          {
            linkedObject.PumpLiquid((float) ((double) this.PumpSpeed * gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0));
            flag = false;
          }
        }
        if (!flag)
          return;
        this.StopPump();
      }
      else
      {
        bool flag = true;
        foreach (Liquid linkedObject in this.LinkedObjects)
        {
          if (!linkedObject.IsEmpty)
          {
            linkedObject.PumpLiquid((float) (-(double) this.PumpSpeed * 0.029999999329447746 * gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0));
            flag = false;
          }
        }
        if (!flag)
          return;
        this.StopPump();
      }
    }

    public override void Draw(bool shadow)
    {
      base.Draw(shadow);
      this._chainAnim.Draw(Stage.CurrentStage.SpriteBatch);
      this._wheel1Sprite.Draw(this._wheel1Pos, 0, this._wheelRotation, SpriteEffects.None, Stage.CurrentStage.SpriteBatch);
      this._wheel2Sprite.Draw(this._wheel2Pos, 0, this._wheelRotation, SpriteEffects.None, Stage.CurrentStage.SpriteBatch);
    }

    private void StopPump()
    {
      this._state = LiquidPump.PumpState.Idle;
      this._engineSound.Stop();
    }

    private void StartPump()
    {
      this._state = LiquidPump.PumpState.Working;
      this._engineSound.Play(true);
    }

    public override bool CrateToolIsValid(BoundingSquare crateBs)
    {
      return !crateBs.Collides(this._crateCollisionBB);
    }

    public void SwitchOn() => this.StartPump();

    public void SwitchOff() => this.StopPump();

    public bool IsOn => this._state == LiquidPump.PumpState.Working;

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord(context);
      if (context == ToDataFileRecordContext.StageSave)
      {
        dataFileRecord.AddField("pumpType", (object) this.PumpType.ToString());
        dataFileRecord.AddField("pumpSpeed", (object) this.PumpSpeed);
      }
      return dataFileRecord;
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this.PumpType = (LiquidPump.PumpTypes) Enum.Parse(typeof (LiquidPump.PumpTypes), record.GetFieldValue<string>("pumpType", this.PumpType.ToString()), true);
      this.PumpSpeed = record.GetFieldValue<float>("pumpSpeed", this.PumpSpeed);
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public enum PumpTypes
    {
      PumpIn,
      PumpOut,
    }

    private enum PumpState
    {
      Idle,
      Working,
    }
  }
}
