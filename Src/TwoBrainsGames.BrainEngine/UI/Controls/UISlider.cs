
// Type: TwoBrainsGames.BrainEngine.UI.Controls.UISlider
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.BrainEngine.UI.Controls
{
  public class UISlider : UIControl
  {
    private UIImage _imgBackground;
    private UIImage _imgSlider;
    private float _value;
    private float _minValue;
    private float _maxValue;
    private bool _dragStarted;
    private SliderOrientation _orientation;
    private bool _needSliderUpdate;
    private float _sliderLength;
    private float _sliderMargin;

    public event UIControl.UIEvent OnValueChanged;

    public event UIControl.UIEvent OnValueChangedEnded;

    private Vector2 GrabOffset { get; set; }

    public float SliderMargin
    {
      get => this._sliderMargin;
      set
      {
        this._sliderMargin = value;
        this._needSliderUpdate = true;
      }
    }

    private bool DragStarted
    {
      get => this._dragStarted;
      set
      {
        this._dragStarted = value;
        if (this._dragStarted)
          this.ScreenOwner.CaptureCursor((UIControl) this);
        else
          this.ScreenOwner.ReleaseCursor();
      }
    }

    public Vector2 SliderStartPosition { get; set; }

    public float SliderLength
    {
      get => this._sliderLength;
      set
      {
        this._sliderLength = value;
        this._needSliderUpdate = true;
      }
    }

    public SliderOrientation Orientation
    {
      get => this._orientation;
      set
      {
        this._orientation = value;
        this._needSliderUpdate = true;
      }
    }

    public float Step { get; set; }

    public UIImage Image => this._imgBackground;

    public bool AutoSetSliderSlot { get; set; }

    public float Value
    {
      get => this._value;
      set
      {
        if ((double) this._value == (double) value)
          return;
        this._value = value;
        if ((double) this._value < (double) this.MinValue)
          this._value = this.MinValue;
        if ((double) this._value > (double) this.MaxValue)
          this._value = this.MaxValue;
        this.SetSliderPosition();
        if (this.OnValueChanged == null)
          return;
        this.OnValueChanged((IUIControl) this);
      }
    }

    public float MinValue
    {
      get => this._minValue;
      set => this._minValue = value;
    }

    public float MaxValue
    {
      get => this._maxValue;
      set => this._maxValue = value;
    }

    public new bool DropShadow
    {
      get => base.DropShadow;
      set
      {
        base.DropShadow = value;
        this._imgSlider.DropShadow = value;
        this._imgBackground.DropShadow = value;
      }
    }

    public new Vector2 ShadowDistance
    {
      get => base.ShadowDistance;
      set
      {
        base.ShadowDistance = value;
        this._imgSlider.ShadowDistance = value;
        this._imgBackground.ShadowDistance = value;
      }
    }

    public new Color ShadowColor
    {
      get => base.ShadowColor;
      set
      {
        base.ShadowColor = value;
        this._imgSlider.ShadowColor = value;
        this._imgBackground.ShadowColor = value;
      }
    }

    public override Size Size
    {
      get => base.Size;
      set
      {
        base.Size = value;
        this._needSliderUpdate = true;
      }
    }

    public UISlider(
      UIScreen screenOwner,
      string backgroundImgResourceName,
      string sliderImgResourceName)
      : base(screenOwner)
    {
      if (backgroundImgResourceName != null)
      {
        this._imgBackground = new UIImage(screenOwner, backgroundImgResourceName);
        this.Controls.Add((UIControl) this._imgBackground);
      }
      this.CursorStop = false;
      this.OnFocus += new UIControl.UIEvent(this.UISlider_OnFocus);
      this._imgSlider = new UIImage(screenOwner, sliderImgResourceName);
      this._imgSlider.OnMotionPointerDown += new UIControl.UIEvent(this._imgSlider_OnMotionPointerDown);
      this._imgSlider.OnMotionPointerUp += new UIControl.UIEvent(this._imgSlider_OnMotionPointerUp);
      this._imgSlider.AcceptControllerInput = true;
      this._imgSlider.Name = "_Slider";
      this._imgSlider.OnFocus += new UIControl.UIEvent(this._imgSlider_OnFocus);
      this.Controls.Add((UIControl) this._imgSlider);
      this.MinValue = 0.0f;
      this.MaxValue = 100f;
      this.Step = 5f;
      if (this._imgBackground != null)
        this.Size = this._imgBackground.Size;
      this.Orientation = SliderOrientation.Horizontal;
      this.Value = 0.0f;
      this.CursorStop = false;
    }

    private void UISlider_OnFocus(IUIControl sender) => this._imgSlider.Focus();

    private void _imgSlider_OnFocus(IUIControl sender)
    {
      if (!(this.Parent is UIControl))
        return;
      ((UIControl) this.Parent).InvokeOnFocus();
    }

    private void SetSliderPosition()
    {
      float num = 0.0f;
      if ((double) this.MaxValue != 0.0)
        num = this.SliderLength * this.Value / this.MaxValue;
      switch (this.Orientation)
      {
        case SliderOrientation.Horizontal:
          this._imgSlider.Position = this.SliderStartPosition + new Vector2(num, 0.0f);
          break;
        case SliderOrientation.Vertical:
          this._imgSlider.Position = this.SliderStartPosition + new Vector2(0.0f, num);
          break;
      }
    }

    private void _imgSlider_OnMotionPointerDown(IUIControl sender)
    {
      this.DragStarted = true;
      this.Focus();
      this.GrabOffset = this.PixelsToScreenUnits(BrainGame.GameCursor.Position - this._imgSlider.AbsolutePositionInPixels);
    }

    private void _imgSlider_OnMotionPointerUp(IUIControl sender)
    {
      this.DragStarted = false;
      if (this.OnValueChangedEnded == null)
        return;
      this.OnValueChangedEnded((IUIControl) this);
    }

    public override void Update(BrainGameTime gameTime)
    {
      if (this._needSliderUpdate && this.AutoSetSliderSlot)
      {
        this.SetSliderToControlSize();
        this.SetSliderPosition();
        this._needSliderUpdate = false;
      }
      if (this.DragStarted)
      {
        switch (this.Orientation)
        {
          case SliderOrientation.Horizontal:
            float num1 = this.PixelsToScreenUnits(BrainGame.GameCursor.Position).X - this.AbsolutePosition.X - this.GrabOffset.X;
            if ((double) num1 < (double) this.SliderStartPosition.X - (double) this.PixelsToScreenUnitsX((float) this._imgSlider.Sprite.Width))
              num1 = this.SliderStartPosition.X - this.PixelsToScreenUnitsX((float) this._imgSlider.Sprite.Width);
            if ((double) num1 > (double) this.SliderStartPosition.X + (double) this.SliderLength)
              num1 = this.SliderStartPosition.X + this.SliderLength;
            this._imgSlider.Position = new Vector2(num1 + this.PixelsToScreenUnitsX(this._imgSlider.Sprite.Offset.X), this._imgSlider.Position.Y);
            this.Value = this.MaxValue * (this._imgSlider.Position.X - this.SliderStartPosition.X) / this.SliderLength;
            break;
          case SliderOrientation.Vertical:
            float num2 = this.PixelsToScreenUnits(BrainGame.GameCursor.Position).Y - this.AbsolutePosition.Y - this.GrabOffset.Y;
            if ((double) num2 < (double) this.SliderStartPosition.Y - (double) this.PixelsToScreenUnitsY((float) this._imgSlider.Sprite.Height))
              num2 = this.SliderStartPosition.Y - this.PixelsToScreenUnitsY((float) this._imgSlider.Sprite.Height);
            if ((double) num2 > (double) this.SliderStartPosition.Y + (double) this.SliderLength)
              num2 = this.SliderStartPosition.Y + this.SliderLength;
            this._imgSlider.Position = new Vector2(this._imgSlider.Position.X, num2 + this.PixelsToScreenUnitsY(this._imgSlider.Sprite.Offset.Y));
            this.Value = this.MaxValue * (this._imgSlider.Position.Y - this.SliderStartPosition.Y) / this.SliderLength;
            break;
        }
      }
      if (this.CursorInside && this.ScreenOwner.CursorMode == CursorModes.SnapToControl)
      {
        if (this.ScreenOwner.InputController.ActionLeft)
          this.Value -= (float) ((double) this.Step * gameTime.ElapsedGameTime.TotalMilliseconds / 100.0);
        if (this.ScreenOwner.InputController.ActionRight)
          this.Value += (float) ((double) this.Step * gameTime.ElapsedGameTime.TotalMilliseconds / 100.0);
        if ((this.ScreenOwner.InputController.ActionLeftReleased || this.ScreenOwner.InputController.ActionRightReleased) && this.OnValueChangedEnded != null)
          this.OnValueChangedEnded((IUIControl) this);
      }
      if (this.ScreenOwner.CursorMode != CursorModes.SnapToControl || !this._imgSlider.HasFocus)
        return;
      BrainGame.GameCursor.Position = this._imgSlider.AbsolutePositionInPixels + this._imgSlider.Sprite.Offset;
    }

    private void SetSliderToControlSize()
    {
      switch (this.Orientation)
      {
        case SliderOrientation.Horizontal:
          this.SliderLength = this.Size.Width - this.SliderMargin * 2f;
          this.SliderStartPosition = new Vector2(this.SliderMargin, this.Size.Height / 2f);
          break;
        case SliderOrientation.Vertical:
          this.SliderLength = this.Size.Height - this.SliderMargin * 2f;
          this.SliderStartPosition = new Vector2(this.Size.Width / 2f, this.SliderMargin);
          break;
      }
    }

    public void SetSliderSlotToSpriteBB(bool centerInSlot)
    {
      if (this._imgBackground == null || this._imgBackground.Sprite == null)
        return;
      BoundingSquare screenUnits = this.PixelsToScreenUnits(this._imgBackground.Sprite.BoundingBox);
      this.SliderStartPosition = screenUnits.UpperLeft;
      if (centerInSlot)
      {
        switch (this.Orientation)
        {
          case SliderOrientation.Horizontal:
            this.SliderStartPosition = new Vector2(screenUnits.UpperLeft.X, screenUnits.UpperLeft.Y + screenUnits.Height / 2f);
            break;
          case SliderOrientation.Vertical:
            this.SliderStartPosition = new Vector2(screenUnits.UpperLeft.X + screenUnits.Width / 2f, screenUnits.UpperLeft.Y);
            break;
        }
      }
      this.SliderLength = screenUnits.Width;
      this.SetSliderPosition();
    }

    public override void Hide() => base.Hide();
  }
}
