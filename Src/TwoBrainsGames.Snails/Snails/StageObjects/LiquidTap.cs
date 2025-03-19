
// Type: TwoBrainsGames.Snails.StageObjects.LiquidTap
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class LiquidTap : LiquidSwitch, ISnailsDataFileSerializable, IDataFileSerializable
  {
    private const float ROTATION_SPEED = 0.1f;
    private const double ROTATION_TIME = 3000.0;
    private const int SNAIL_COLLISION_BB_IDX = 0;
    private const int HANDLE_POS_BB_IDX = 1;
    private const int SIGN_POS_BB_IDX = 2;
    private const double TAP_PUMPING_TIME = 1000.0;
    private int _direction;
    private float _tapRotation;
    private Sprite _handleSprite;
    private Vector2 _handlePosition;
    private bool _pumpingActive;
    private double _ellapsedTime;
    private Sprite _pumpInSignSprite;
    private Sprite _pumpOutSignSprite;
    private Sprite _signSprite;
    private Vector2 _signPosition;
    private SpriteEffects _signSpriteEffect;
    private Sample _tapSound;

    public LiquidTap.TapOpenDirection OpenDirection { get; set; }

    public LiquidTap.TapSignToShow SignToShow { get; set; }

    public LiquidTap()
      : base(StageObjectType.LiquidTap)
    {
    }

    public override void Copy(StageObject other)
    {
      base.Copy(other);
      this.OpenDirection = ((LiquidTap) other).OpenDirection;
      this.SignToShow = ((LiquidTap) other).SignToShow;
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this._handleSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/water", "TapHandle");
      this._pumpInSignSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/water", "TapSignPumpIn");
      this._pumpOutSignSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/water", "TapSignPumpOut");
      this._tapSound = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/water-tap", (Object2D) this);
    }

    public override void Initialize()
    {
      base.Initialize();
      this._handlePosition = this.TransformSpriteFrameBB(1).GetCenter();
      this._signSprite = this.SignToShow != LiquidTap.TapSignToShow.PumpIn ? this._pumpOutSignSprite : this._pumpInSignSprite;
      this._signPosition = this.TransformSpriteFrameBB(2).GetCenter();
      if (this.OpenDirection != LiquidTap.TapOpenDirection.CounterClockwise)
        return;
      this._signSpriteEffect = SpriteEffects.FlipHorizontally;
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      if (this._pumpingActive)
      {
        double num = gameTime.ElapsedGameTime.TotalMilliseconds;
        if (this._ellapsedTime + num > 1000.0)
        {
          num = 1000.0 - this._ellapsedTime;
          this._ellapsedTime = 0.0;
          this._pumpingActive = false;
          this._tapSound.Stop();
        }
        else
          this._ellapsedTime += num;
        if (num <= 0.0)
          return;
        this._tapRotation += (float) (0.10000000149011612 * num * (double) this._direction * (this.OpenDirection == LiquidTap.TapOpenDirection.CounterClockwise ? -1.0 : 1.0));
        foreach (Liquid linkedObject in this.LinkedObjects)
          linkedObject.PumpLiquid((float) ((double) this.PumpSpeed * 0.029999999329447746 * num / 1000.0) * (float) this._direction);
      }
      else
        this.DoQuadtreeCollisions(0);
    }

    public override void Draw(bool shadow)
    {
      if (!shadow)
        this._signSprite.Draw(this._signPosition, 0, 0.0f, this._signSpriteEffect, Stage.CurrentStage.SpriteBatch);
      else
        this._signSprite.Draw(this._signPosition + GenericConsts.ShadowDepth, 0, 0.0f, this._signSpriteEffect, this.ShadowColor, 1f, Stage.CurrentStage.SpriteBatch);
      base.Draw(shadow);
      this._handleSprite.Draw(this._handlePosition + new Vector2(3f, 3f), 0, this._tapRotation, SpriteEffects.None, this.ShadowColor, 1f, Stage.CurrentStage.SpriteBatch);
      this._handleSprite.Draw(this._handlePosition, 0, this._tapRotation, SpriteEffects.None, Stage.CurrentStage.SpriteBatch);
    }

    protected override void OnSnailCollided(Snail snail)
    {
      if (this._pumpingActive || !snail.CollidedWithLiquidTap(this))
        return;
      this._pumpingActive = true;
      this._ellapsedTime = 0.0;
      this._direction = snail.Direction == MovingObject.WalkDirection.Clockwise ? -1 : 1;
      if (this.OpenDirection == LiquidTap.TapOpenDirection.CounterClockwise)
        this._direction *= -1;
      if (this._direction == 1)
      {
        bool flag = true;
        foreach (Liquid linkedObject in this.LinkedObjects)
        {
          if (!linkedObject.IsFull)
          {
            flag = false;
            break;
          }
        }
        if (flag)
        {
          this._pumpingActive = false;
          return;
        }
      }
      else
      {
        bool flag = true;
        foreach (Liquid linkedObject in this.LinkedObjects)
        {
          if (!linkedObject.IsEmpty)
          {
            flag = false;
            break;
          }
        }
        if (flag)
        {
          this._pumpingActive = false;
          return;
        }
      }
      this._tapSound.Play();
    }

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord(context);
      dataFileRecord.AddField("openDirection", (object) this.OpenDirection.ToString());
      dataFileRecord.AddField("signToShow", (object) this.SignToShow.ToString());
      return dataFileRecord;
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this.OpenDirection = (LiquidTap.TapOpenDirection) Enum.Parse(typeof (LiquidTap.TapOpenDirection), record.GetFieldValue<string>("openDirection", LiquidTap.TapOpenDirection.Clockwise.ToString()), true);
      this.SignToShow = (LiquidTap.TapSignToShow) Enum.Parse(typeof (LiquidTap.TapSignToShow), record.GetFieldValue<string>("signToShow", LiquidTap.TapSignToShow.PumpIn.ToString()), true);
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public enum TapOpenDirection
    {
      Clockwise,
      CounterClockwise,
    }

    public enum TapSignToShow
    {
      PumpIn,
      PumpOut,
    }
  }
}
