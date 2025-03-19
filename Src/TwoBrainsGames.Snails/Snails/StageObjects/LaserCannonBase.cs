
// Type: TwoBrainsGames.Snails.StageObjects.LaserCannonBase
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class LaserCannonBase(StageObjectType cannonType) : 
    MovingObject(cannonType),
    ISwitchable,
    ISnailsDataFileSerializable,
    IDataFileSerializable
  {
    protected LaserBeam _laserBeam;
    protected LaserCannonBase.LaserBeamSourceState _state;
    protected double _turningOnTime;
    protected TwoBrainsGames.Snails.Effects.BlinkEffect _blinkEffect;
    protected Color _laserXnaColor;
    protected Sample _laserSound;

    public double BlinkTimeOn { get; set; }

    public double BlinkTimeOff { get; set; }

    public bool WithBlink { get; set; }

    public bool TurnedOn { get; set; }

    public LaserBeam.LaserBeamColor LaserColor { get; set; }

    public override void LoadContent()
    {
      base.LoadContent();
      this._laserSound = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/laser-beam", (Object2D) this);
    }

    public override void Initialize()
    {
      base.Initialize();
      this._laserXnaColor = LaserBeam.GetXnaColor(this.LaserColor);
      this._blinkEffect = new TwoBrainsGames.Snails.Effects.BlinkEffect(this.BlinkTimeOn, this.BlinkTimeOn);
      this._blinkEffect.Active = false;
      this._blinkEffect.AutoDeleteOnEnd = false;
      this.EffectsBlender.Add((ITransformEffect) this._blinkEffect);
      if (this._laserBeam == null)
      {
        this._laserBeam = LaserBeam.Create(this);
        this._laserBeam.Activate();
      }
      if (this.TurnedOn)
        this.SetTurningOnState(false);
      else
        this._laserBeam.Hide();
    }

    public override void StageStartupPhaseEnded()
    {
      base.StageStartupPhaseEnded();
      int state = (int) this._state;
    }

    public int CountBeamMirrorCollisions(LaserBeamMirror mirror)
    {
      int num = 0;
      for (LaserBeam laserBeam = this._laserBeam; laserBeam != null && laserBeam.IsVisible; laserBeam = laserBeam.NextBeam)
      {
        if (laserBeam.ReflectingMirror == mirror)
          ++num;
      }
      return num;
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      if (this._state != LaserCannonBase.LaserBeamSourceState.On || !this.WithBlink)
        return;
      this._laserBeam.Visible = this._blinkEffect.Visible;
      this._blinkEffect.Update(gameTime);
    }

    protected virtual void SetTurningOnState(bool playSound)
    {
      this._state = LaserCannonBase.LaserBeamSourceState.On;
      this._laserBeam.Visible = true;
    }

    public void SwitchOn()
    {
      if (this.TurnedOn)
        return;
      this.SetTurningOnState(true);
    }

    public void SwitchOff()
    {
      this._laserBeam.Visible = false;
      this.TurnedOn = false;
      this._blinkEffect.Active = false;
      this._state = LaserCannonBase.LaserBeamSourceState.Off;
    }

    public bool IsOn => this._state != LaserCannonBase.LaserBeamSourceState.Off;

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord(context);
      dataFileRecord.AddField("turnedOn", (object) this.TurnedOn);
      dataFileRecord.AddField("blinkTimeOn", (object) this.BlinkTimeOn);
      dataFileRecord.AddField("blinkTimeOff", (object) this.BlinkTimeOff);
      dataFileRecord.AddField("withBlink", (object) this.WithBlink);
      dataFileRecord.AddField("colorType", (object) this.LaserColor.ToString());
      return dataFileRecord;
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this.TurnedOn = record.GetFieldValue<bool>("turnedOn", this.TurnedOn);
      this._state = this.TurnedOn ? LaserCannonBase.LaserBeamSourceState.TurningOn : LaserCannonBase.LaserBeamSourceState.Off;
      this.BlinkTimeOn = record.GetFieldValue<double>("blinkTimeOn", this.BlinkTimeOn);
      this.BlinkTimeOff = record.GetFieldValue<double>("blinkTimeOff", this.BlinkTimeOff);
      this.WithBlink = record.GetFieldValue<bool>("withBlink", this.WithBlink);
      this.LaserColor = (LaserBeam.LaserBeamColor) Enum.Parse(typeof (LaserBeam.LaserBeamColor), record.GetFieldValue<string>("colorType", this.LaserColor.ToString()), true);
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    protected enum LaserBeamSourceState
    {
      TurningOn,
      On,
      Off,
    }
  }
}
