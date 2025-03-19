
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UIValuedCaption
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UIValuedCaption : UIControl
  {
    private const int TOTAL_VALUE_COUNTER_TIME = 1000;
    private const int COUNTER_TIMER = 50;
    private UICaption _capText;
    private UICaption _capValue;
    private UITimer _tmrTimer;
    private object _value;
    private Sample _pointIncSample;
    private UIValuedCaption.ValueAlignmentMode _captionAlignment;
    private float _captionSpacing;

    public Color ValueColor
    {
      get => this._capValue.BlendColor;
      set
      {
        this._capValue.BlendColor = value;
        if (!(this._capValue.ShowEffect is SquashEffect))
          return;
        ((SquashEffect) this._capValue.ShowEffect).BlendColor = value;
      }
    }

    public Color CaptionColor
    {
      get => this._capText.BlendColor;
      set
      {
        this._capText.BlendColor = value;
        if (!(this._capText.ShowEffect is SquashEffect))
          return;
        ((SquashEffect) this._capText.ShowEffect).BlendColor = value;
      }
    }

    public object Value
    {
      get => this._value;
      set
      {
        this._value = value;
        switch (value)
        {
          case int _:
            this.ValType = UIValuedCaption.ValueType.Int;
            this.ValueCounter = (object) 0;
            if (!this.AutoComputeIncrement)
              break;
            this.Increment = (int) Math.Ceiling((double) (int) this.Value * 50.0 / 1000.0);
            if (this.Increment > 0)
              break;
            this.Increment = 1;
            break;
          case TimeSpan _:
            this.ValType = UIValuedCaption.ValueType.Time;
            this.ValueCounter = (object) new TimeSpan(0, 0, 0);
            if (!this.AutoComputeIncrement)
              break;
            this.Increment = (int) Math.Ceiling(((TimeSpan) this.Value).TotalSeconds * 50.0 / 1000.0);
            if (this.Increment > 0)
              break;
            this.Increment = 1;
            break;
          case string _:
            this.ValType = UIValuedCaption.ValueType.String;
            this.ValueCounter = value;
            break;
        }
      }
    }

    public object FractionValue { get; set; }

    public int MultiplierValue { get; set; }

    private object ValueCounter { get; set; }

    private UIValuedCaption.ValueType ValType { get; set; }

    public UIValuedCaption.CaptionMode Mode { get; set; }

    public int Increment { get; set; }

    public bool AnimateValue { get; set; }

    public bool AutoComputeIncrement { get; set; }

    public override string Text
    {
      get => this._capText.Text;
      set => this._capText.Text = value;
    }

    public Size CaptionSize => this._capText.Size;

    public Vector2 ValuePosition
    {
      get => this.Position + this._capValue.Position;
      set => this._capValue.Position = value;
    }

    public UIValuedCaption.ValueAlignmentMode CaptionAlignment
    {
      get => this._captionAlignment;
      set
      {
        this._captionAlignment = value;
        this.Refresh();
      }
    }

    public float CaptionSpacing
    {
      get => this._captionSpacing;
      set
      {
        this._captionSpacing = value;
        this.Refresh();
      }
    }

    public UIValuedCaption(
      UIScreen screenOwner,
      string text,
      object value,
      Color color,
      Color valColor,
      UICaption.CaptionStyle style,
      float width,
      bool playShowSound)
      : base(screenOwner)
    {
      this.AnimateValue = true;
      this._tmrTimer = new UITimer(screenOwner, 50.0, true);
      this._tmrTimer.OnTimer += new UIControl.UIEvent(this._tmrTimer_OnTimer);
      this.Controls.Add((UIControl) this._tmrTimer);
      this.Value = value;
      this._capText = new UICaption(screenOwner, "", color, style);
      this._capText.TextResourceId = text;
      this._capText.OnShow += new UIControl.UIEvent(this._capText_OnShow);
      this._capText.ShowEffect = (TransformEffectBase) new SquashEffect(0.9f, 4f, 0.04f, this._capText.BlendColor, this._capText.Scale);
      this.Controls.Add((UIControl) this._capText);
      this._capValue = new UICaption(screenOwner, this.Value != null ? this.Value.ToString() : "", valColor, style);
      this._capValue.ParentAlignment = AlignModes.Right;
      this._capValue.ShowEffect = (TransformEffectBase) new SquashEffect(0.9f, 4f, 0.04f, this._capValue.BlendColor, this._capValue.Scale);
      this.Controls.Add((UIControl) this._capValue);
      this.Size = new Size(width, this._capText.Size.Height);
      this.Increment = 1;
      this.AcceptControllerInput = false;
      this.AutoComputeIncrement = true;
      this.Refresh();
      this._pointIncSample = BrainGame.ResourceManager.GetSampleStatic("sfx/point-increment");
      if (!playShowSound)
        return;
      this.ShowSoundEffect = BrainGame.ResourceManager.GetSampleStatic("sfx/caption-show");
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      if (!this.AnimateValue)
        this.ValueCounter = this.Value;
      switch (this.ValType)
      {
        case UIValuedCaption.ValueType.Int:
          switch (this.Mode)
          {
            case UIValuedCaption.CaptionMode.Simple:
              this._capValue.Text = string.Format("{0}", this.ValueCounter);
              return;
            case UIValuedCaption.CaptionMode.Fraction:
              this._capValue.Text = string.Format("{0}/{1}", this.ValueCounter, this.FractionValue);
              return;
            case UIValuedCaption.CaptionMode.Multiplier:
              this._capValue.Text = string.Format("{0}X{1}", this.ValueCounter, (object) this.MultiplierValue);
              return;
            default:
              return;
          }
        case UIValuedCaption.ValueType.Time:
          TimeSpan valueCounter = (TimeSpan) this.ValueCounter;
          switch (this.Mode)
          {
            case UIValuedCaption.CaptionMode.Simple:
              string str = string.Format("{0:00}:{1:00}", (object) (valueCounter.Hours * 60 + valueCounter.Minutes), (object) valueCounter.Seconds);
              if (valueCounter.Hours > 0)
                str = "60:00";
              this._capValue.Text = str;
              return;
            case UIValuedCaption.CaptionMode.Fraction:
              throw new SnailsException("Not implemented");
            case UIValuedCaption.CaptionMode.Multiplier:
              this._capValue.Text = string.Format("{0}X{1}", (object) (int) valueCounter.TotalSeconds, (object) this.MultiplierValue);
              return;
            case UIValuedCaption.CaptionMode.Fulltime:
              this._capValue.Text = string.Format("{0:00}:{1:00},{2:000}", (object) valueCounter.Minutes, (object) valueCounter.Seconds, (object) valueCounter.Milliseconds);
              return;
            case UIValuedCaption.CaptionMode.FulltimeHours:
              this._capValue.Text = string.Format("{0:00}:{1:00}:{2:00},{3:000}", (object) valueCounter.Hours, (object) valueCounter.Minutes, (object) valueCounter.Seconds, (object) valueCounter.Milliseconds);
              return;
            default:
              return;
          }
        case UIValuedCaption.ValueType.String:
          this._capValue.Text = (string) this.Value;
          break;
      }
    }

    public void Reset() => this._tmrTimer.Enabled = false;

    public void Refresh()
    {
      switch (this.CaptionAlignment)
      {
        case UIValuedCaption.ValueAlignmentMode.Right:
          this._capValue.ParentAlignment = AlignModes.Right;
          break;
        case UIValuedCaption.ValueAlignmentMode.Left:
          this._capValue.ParentAlignment = AlignModes.None;
          this._capValue.Position = this._capText.Position + new Vector2(this._capText.Width + this._captionSpacing, 0.0f);
          break;
      }
    }

    public void QuickShow()
    {
      this.ValueCounter = this.Value;
      this.Show();
    }

    private void _capText_OnShow(IUIControl sender)
    {
      if (!this.AnimateValue)
        return;
      this._tmrTimer.Enabled = true;
    }

    private void _tmrTimer_OnTimer(IUIControl sender)
    {
      bool flag = false;
      switch (this.ValType)
      {
        case UIValuedCaption.ValueType.Int:
          int num = (int) this.ValueCounter + this.Increment;
          if (num > (int) this.Value)
          {
            num = (int) this.Value;
            flag = true;
          }
          this.ValueCounter = (object) num;
          break;
        case UIValuedCaption.ValueType.Time:
          TimeSpan timeSpan = (TimeSpan) this.ValueCounter;
          timeSpan = timeSpan.Add(new TimeSpan(0, 0, this.Increment));
          if (timeSpan > (TimeSpan) this.Value)
          {
            timeSpan = (TimeSpan) this.Value;
            flag = true;
          }
          this.ValueCounter = (object) timeSpan;
          break;
        default:
          flag = true;
          break;
      }
      if (flag)
      {
        this._tmrTimer.Enabled = false;
        this.InvokeOnShow();
        this._pointIncSample.Stop();
      }
      else
        this._pointIncSample.Play(true);
    }

    private enum ValueType
    {
      None,
      Int,
      Time,
      String,
    }

    public enum CaptionMode
    {
      Simple,
      Fraction,
      Multiplier,
      Fulltime,
      FulltimeHours,
    }

    public enum ValueAlignmentMode
    {
      Right,
      Left,
    }
  }
}
