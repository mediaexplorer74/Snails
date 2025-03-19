
// Type: TwoBrainsGames.BrainEngine.UI.Controls.UITimer
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.BrainEngine.UI.Controls
{
  public class UITimer : UIControl
  {
    public event UIControl.UIEvent OnTimer;

    private double EllapsedTime { get; set; }

    public double Time { get; set; }

    public bool Snooze { get; set; }

    public object Parameter { get; set; }

    public bool RaisesOnTimerWhenStarts { get; set; }

    public UITimer(UIScreen screenOwner)
      : this(screenOwner, 0.0, false)
    {
    }

    public UITimer(UIScreen screenOwner, double time, bool snooze)
      : base(screenOwner)
    {
      this.AcceptControllerInput = false;
      this.Time = time;
      this.EllapsedTime = 0.0;
      this.Snooze = snooze;
      this.Enabled = false;
    }

    public override void Update(BrainGameTime gameTime)
    {
      if (!this.Enabled)
        return;
      this.EllapsedTime += gameTime.ElapsedRealTime.TotalMilliseconds;
      if (this.EllapsedTime <= this.Time)
        return;
      if (this.OnTimer != null)
        this.OnTimer((IUIControl) this);
      if (this.Snooze)
      {
        this.EllapsedTime -= this.Time;
      }
      else
      {
        this.Enabled = false;
        this.Reset();
      }
    }

    public void Reset() => this.EllapsedTime = this.RaisesOnTimerWhenStarts ? this.Time : 0.0;
  }
}
