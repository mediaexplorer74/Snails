
// Type: TwoBrainsGames.Snails.StageObjects.Lamp
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  internal class Lamp : SingleLightEmitter, ISwitchable
  {
    private const float MAX_SHAKE_ANGLE = 10f;
    private const float MAX_SHAKE_DISTANCE = 800f;
    private Lamp.LampState _lampState;
    private int _switchingOnFlashingTimes;
    private double _ellapsedFlashingTime;
    private int _flickerCounter;
    private bool _shaking;
    private int _shakeDirection;
    private float _maxShakingAngle;
    private float _shakeAngle;
    private bool _shakesWithExplosions;

    public Lamp()
      : base(StageObjectType.Lamp)
    {
    }

    public override void Copy(StageObject other)
    {
      base.Copy(other);
      this._lampState = this.State == LightSource.LightState.On ? Lamp.LampState.On : Lamp.LampState.Off;
      this._shakesWithExplosions = ((Lamp) other)._shakesWithExplosions;
    }

    public override StageObject Clone()
    {
      StageObject stageObject = StageObjectFactory.Create(this.Type);
      stageObject.Copy((StageObject) this);
      return stageObject;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.LightSource.SetState(this.State);
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      switch (this._lampState)
      {
        case Lamp.LampState.SwitchingOn:
          if (this._ellapsedFlashingTime == 0.0)
            this._ellapsedFlashingTime = (double) (30 + BrainGame.Rand.Next(80));
          this._ellapsedFlashingTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
          if (this._ellapsedFlashingTime <= 0.0)
          {
            if (this.LightSource.IsOff)
              this.LightSource.SwitchOn();
            else
              this.LightSource.SwitchOff();
            --this._switchingOnFlashingTimes;
            if (this._switchingOnFlashingTimes <= 0)
            {
              this.LightSource.SwitchOn();
              this._lampState = Lamp.LampState.On;
            }
            this._ellapsedFlashingTime = 0.0;
            break;
          }
          break;
        case Lamp.LampState.On:
          if (this._ellapsedFlashingTime == 0.0)
            this._ellapsedFlashingTime = (double) (2000 + BrainGame.Rand.Next(3000));
          this._ellapsedFlashingTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
          if (this._ellapsedFlashingTime < 0.0)
          {
            this._lampState = Lamp.LampState.Flicker;
            this._ellapsedFlashingTime = 0.0;
            this._flickerCounter = 8;
            break;
          }
          break;
        case Lamp.LampState.Flicker:
          if (this._ellapsedFlashingTime == 0.0)
            this._ellapsedFlashingTime = 80.0;
          this._ellapsedFlashingTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
          if (this._ellapsedFlashingTime < 0.0)
          {
            this._ellapsedFlashingTime = 0.0;
            --this._flickerCounter;
            if ((double) this.LightSource.Power == 1.0)
              this.LightSource.Power = (float) (180 + BrainGame.Rand.Next(50)) / 256f;
            else
              this.LightSource.Power = 1f;
            if (this._flickerCounter < 0)
            {
              this._lampState = Lamp.LampState.On;
              this.LightSource.Power = 1f;
              this._ellapsedFlashingTime = 0.0;
              break;
            }
            break;
          }
          break;
      }
      if (!this._shaking)
        return;
      this._shakeAngle += (float) ((double) (this._maxShakingAngle - Math.Abs(this._shakeAngle * 0.5f)) * (double) this._shakeDirection * gameTime.ElapsedGameTime.TotalMilliseconds / 100.0);
      if ((double) Math.Abs(this._shakeAngle) > (double) this._maxShakingAngle)
      {
        this._maxShakingAngle *= 0.8f;
        this._shakeAngle = this._maxShakingAngle * (float) this._shakeDirection;
        this._shakeDirection *= -1;
        if ((double) this._maxShakingAngle < 0.5)
        {
          this._shakeAngle = 0.0f;
          this._shaking = false;
        }
      }
      this.Rotation = this.LightSource.Rotation = this._shakeAngle;
    }

    public override void OnExplosion(Explosion explosion)
    {
      if (!this._shakesWithExplosions)
        return;
      float num = (explosion.Position - this.Position).Length();
      if ((double) num > 800.0)
        return;
      this.Shake((float) (1.0 - (double) num / 800.0));
    }

    public void Shake(float power)
    {
      if (!this._shaking)
      {
        this._shakeAngle = 0.0f;
        this._shakeDirection = BrainGame.Rand.Next(2);
        if (this._shakeDirection == 0)
          this._shakeDirection = -1;
      }
      this._shaking = true;
      if (10.0 * (double) power <= (double) this._maxShakingAngle)
        return;
      this._maxShakingAngle = 10f * power;
    }

    public void SwitchOn()
    {
      this._lampState = Lamp.LampState.SwitchingOn;
      this.LightSource.Power = 1f;
      this._switchingOnFlashingTimes = 3 + BrainGame.Rand.Next(4);
      this._ellapsedFlashingTime = 0.0;
      this.LightSource.SwitchOn();
    }

    public void SwitchOff()
    {
      this.LightSource.SwitchOff();
      this._lampState = Lamp.LampState.Off;
    }

    public new bool IsOn => this._lampState == Lamp.LampState.On;

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this._shakesWithExplosions = record.GetFieldValue<bool>("shakesWithExplosions", this._shakesWithExplosions);
    }

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord(context);
      if (context == ToDataFileRecordContext.StageDataSave)
        dataFileRecord.AddField("shakesWithExplosions", (object) this._shakesWithExplosions);
      return dataFileRecord;
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    private enum LampState
    {
      Off,
      SwitchingOn,
      On,
      Flicker,
    }
  }
}
