
// Type: TwoBrainsGames.Snails.StageObjects.DynamiteBoxCounted
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Effects;


namespace TwoBrainsGames.Snails.StageObjects
{
  internal class DynamiteBoxCounted : DynamiteBox
  {
    private const int EXPLOSTION_DELAY_TIME = 3500;
    private Color _saveCrateColor;
    private List<Snail> _currentCollidingSnails;
    private List<Snail> _collidingSnails;
    private BlinkEffect _counterBlinkEffect;
    private UITimer _flashTimer;

    public int SnailsAllowed { get; set; }

    public DynamiteBoxCounted()
      : base(StageObjectType.DynamiteBoxCounted)
    {
    }

    public override void Copy(StageObject other)
    {
      base.Copy(other);
      this.SnailsAllowed = ((DynamiteBoxCounted) other).SnailsAllowed;
    }

    public override void Initialize()
    {
      base.Initialize();
      this._counterVisible = true;
      this._currentSecond = this.SnailsAllowed;
      this._currentCollidingSnails = new List<Snail>();
      this._collidingSnails = new List<Snail>();
      this._counterBlinkEffect = new BlinkEffect(500.0, 100.0, (Sample) null, 3500.0, 150.0);
      this._counterBlinkEffect.UseRealTime = false;
      this._counterBlinkEffect.MinBLink = 50f;
      this._flashTimer = new UITimer((UIScreen) null, 100.0, false);
      this._flashTimer.OnTimer += new UIControl.UIEvent(this._flashTimer_OnTimer);
      this._saveCrateColor = this.BlendColor;
    }

    private void _flashTimer_OnTimer(IUIControl sender) => this.BlendColor = this._saveCrateColor;

    protected override void BoxDeployed(bool addTile, bool addPaths, bool checkCollisions)
    {
      base.BoxDeployed(addTile, addPaths, checkCollisions);
      this._status = DynamiteBox.DynamiteBoxStatus.Idle;
      this.SetSpriteWhenIdle();
      this.PreComputeCounterData(this.SnailsAllowed);
      this._deployStatus = Box.BoxDeployStatus.Deployed;
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      if (this._flashTimer.Enabled)
        this._flashTimer.Update(gameTime);
      this._currentCollidingSnails.Clear();
      if (this.SnailsAllowed > 0)
        this.DoQuadtreeCollisions(0);
      for (int index = 0; index < this._collidingSnails.Count; ++index)
      {
        if (!this._currentCollidingSnails.Contains(this._collidingSnails[index]))
        {
          this._collidingSnails.Remove(this._collidingSnails[index]);
          --index;
        }
      }
      if (this._status != DynamiteBox.DynamiteBoxStatus.ExplosionDelay)
        return;
      this._counterBlinkEffect.Update(gameTime);
      this._counterVisible = this._counterBlinkEffect.Visible;
      this._elapsedTimeToExplode += gameTime.ElapsedGameTime.TotalMilliseconds;
      if (this._elapsedTimeToExplode <= 3500.0)
        return;
      this.Explode();
    }

    protected override void SnailCollided(Snail snail)
    {
      if (!this._currentCollidingSnails.Contains(snail))
        this._currentCollidingSnails.Add(snail);
      if (this._collidingSnails.Contains(snail))
        return;
      this._tickSample.Play();
      --this.SnailsAllowed;
      if (this.SnailsAllowed < 0)
        return;
      --this._currentSecond;
      this._collidingSnails.Add(snail);
      this.PreComputeCounterData(this.SnailsAllowed);
      if (!this._flashTimer.Enabled)
      {
        this.BlendColor = new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 128);
        this._flashTimer.Enabled = true;
      }
      if (this._currentSecond != 0)
        return;
      this._status = DynamiteBox.DynamiteBoxStatus.ExplosionDelay;
      this._counterBlinkEffect.Reset();
      this._counterBlinkEffect.Active = true;
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this.SnailsAllowed = record.GetFieldValue<int>("snailsAllowed", this.SnailsAllowed);
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord(context);
      if (context == ToDataFileRecordContext.StageSave)
        dataFileRecord.AddField("snailsAllowed", (object) this.SnailsAllowed);
      return dataFileRecord;
    }
  }
}
