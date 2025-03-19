
// Type: TwoBrainsGames.BrainEngine.Debugging.BrainPerformanceCounter
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;


namespace TwoBrainsGames.BrainEngine.Debugging
{
  public class BrainPerformanceCounter(Game game) : GameComponent(game)
  {
    private double _AlertThreshold;
    private double _Counter;
    private BrainPerformanceCounter.AlertConditions _AlertMode;

    public bool AlarmOn { get; private set; }

    private double AverageAcumulator { get; set; }

    public double Average { get; private set; }

    public bool AverageOn { get; private set; }

    public int AverageCounter { get; private set; }

    public double AlertThreshold
    {
      get => this._AlertThreshold;
      set
      {
        if (this._AlertThreshold == value)
          return;
        this._AlertThreshold = value;
        this.AlertThresholdChanged();
      }
    }

    public double Counter
    {
      get => this._Counter;
      set
      {
        if (this._Counter == value)
          return;
        this._Counter = value;
        this.CounterThresholdChanged();
      }
    }

    public BrainPerformanceCounter.AlertConditions AlertMode
    {
      get => this._AlertMode;
      set
      {
        if (this._AlertMode == value)
          return;
        this._AlertMode = value;
        this.AlertModeThresholdChanged();
      }
    }

    public override void Update(GameTime gameTime)
    {
      if (!this.AverageOn)
        return;
      ++this.AverageCounter;
      this.AverageAcumulator += this.Counter;
      this.Average = this.AverageAcumulator / (double) this.AverageCounter;
    }

    public virtual void Reset() => this.Counter = 0.0;

    public void Set(float val) => this.Counter = (double) val;

    public void Set(int val) => this.Counter = (double) val;

    public void SetAlarmThreshold(float threshold, BrainPerformanceCounter.AlertConditions modes)
    {
      this.AlertMode = modes;
      this.AlertThreshold = (double) threshold;
    }

    public void BeginAverage()
    {
      this.Average = 0.0;
      this.AverageAcumulator = 0.0;
      this.AverageCounter = 0;
      this.AverageOn = true;
    }

    public void EndAverage() => this.AverageOn = false;

    private void UpdateAlarmStatus()
    {
      if (this.AlertMode == BrainPerformanceCounter.AlertConditions.None)
        return;
      this.AlarmOn = false;
      if ((this.AlertMode & BrainPerformanceCounter.AlertConditions.Equal) == BrainPerformanceCounter.AlertConditions.Equal && this.Counter == this.AlertThreshold)
        this.AlarmOn = true;
      if ((this.AlertMode & BrainPerformanceCounter.AlertConditions.Greater) == BrainPerformanceCounter.AlertConditions.Greater && this.Counter > this.AlertThreshold)
        this.AlarmOn = true;
      if ((this.AlertMode & BrainPerformanceCounter.AlertConditions.Smaller) != BrainPerformanceCounter.AlertConditions.Smaller || this.Counter >= this.AlertThreshold)
        return;
      this.AlarmOn = true;
    }

    private void CounterThresholdChanged() => this.UpdateAlarmStatus();

    private void AlertThresholdChanged() => this.UpdateAlarmStatus();

    private void AlertModeThresholdChanged() => this.UpdateAlarmStatus();

    [Flags]
    public enum AlertConditions
    {
      None = 0,
      Smaller = 1,
      Equal = 2,
      Greater = 4,
    }
  }
}
