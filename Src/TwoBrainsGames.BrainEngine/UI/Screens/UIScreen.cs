
// Type: TwoBrainsGames.BrainEngine.UI.Screens.UIScreen
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.UI.Controls;


namespace TwoBrainsGames.BrainEngine.UI.Screens
{
  public class UIScreen : Screen, IUIControl
  {
    public const int MAX_SCREEN_WITDH_IN_POINTS = 10000;
    public const int MAX_SCREEN_HEIGHT_IN_POINTS = 10000;
    public UIControl.UIEvent OnCancel;
    private UITimer _tmrCursorSnap;
    internal ContainerControl _container;
    internal ControlCollection _controls;
    private static Texture2D _clearTexture;
    private Color _blendColor;
    internal UIControl _focusControl;

    public event UIControl.UIEvent OnAccept;

    public event UIControl.UIEvent OnBack;

    public event UIControl.UIEvent OnBeforeControlsDraw;

    public event UIControl.UIEvent OnAfterInitializeFromContent;

    internal static Texture2D ClearTexture
    {
      get
      {
        if (UIScreen._clearTexture == null)
        {
          UIScreen._clearTexture = new Texture2D(BrainGame.Graphics, 1, 1);
          UIScreen._clearTexture.SetData<Color>(new Color[1]
          {
            Color.White
          });
        }
        return UIScreen._clearTexture;
      }
    }

    public bool CanFocus => true;

    internal UIControl FocusControl
    {
      get => this._focusControl;
      set
      {
        if (this._focusControl == value)
          return;
        if (this._focusControl != null && (value == null || value != null && this._focusControl != value.Parent))
          this._focusControl.UnFocus();
        UIControl focusControl = this._focusControl;
        this._focusControl = value;
        if (this._focusControl == null || focusControl != null && (focusControl == null || this._focusControl == focusControl.Parent))
          return;
        this._focusControl.Focus();
      }
    }

    public bool Enabled { get; set; }

    public UnitsType UnitType { get; set; }

    public Color BackgroundColor { get; set; }

    public Color BackgroundImageBlendColor { get; set; }

    public ScreenBackgroudImageMode BackgroundImageMode { get; set; }

    public Sprite BackgroundImage { get; set; }

    public AlignModes ParentAlignment { get; set; }

    public Size Size { get; set; }

    public virtual Size SizeScaled
    {
      get => new Size(this.Size.Width * this.Scale.X, this.Size.Height * this.Scale.Y);
      set => this.Size = new Size(value.Width * this.Scale.X, value.Height * this.Scale.Y);
    }

    public Vector2 Position { get; set; }

    public CursorModes CursorMode { get; set; }

    public bool CursorEnabled { get; set; }

    public bool IgnoreControlFocus { get; set; }

    public Vector2 Scale { get; set; }

    public bool AcceptControllerInput { get; set; }

    public bool Visible { get; set; }

    public RenderMask Mask { get; set; }

    public bool CursorInside
    {
      get
      {
        return new BoundingSquare(this.PositionInPixels, this.SizeInPixels.Width, this.SizeInPixels.Height).Contains(BrainGame.GameCursor.Position);
      }
    }

    public bool HasFocus
    {
      get => this.Navigator.ActiveScreen != null && this.Navigator.ActiveScreen == this;
    }

    public virtual bool CheckCursorInside() => true;

    public Size SizeInPixels => this.ScreenUnitToPixels(this.Size);

    public Vector2 PositionInPixels => this.ScreenUnitToPixels(this.Position);

    public Vector2 CenterInPixels
    {
      get
      {
        return this.PositionInPixels + new Vector2(this.SizeInPixels.Width / 2f, this.SizeInPixels.Height / 2f);
      }
    }

    public ControlCollection Controls => this._controls;

    public bool IsFullscreen
    {
      get
      {
        return ((double) this.PositionInPixels.X == 0.0 || (double) this.PositionInPixels.Y == 0.0) && ((double) this.SizeInPixels.Width == (double) BrainGame.ScreenWidth || (double) this.SizeInPixels.Height == (double) BrainGame.ScreenHeight);
      }
    }

    public UIScreen ScreenOwner
    {
      get
      {
        throw new BrainException("This property cannot be accessed. You probably are trying to access this property in Screen.OnLoad(). Use 'this' instead. This is a design issue in the infraestructure...)");
      }
    }

    public IUIControl Parent
    {
      get => (IUIControl) null;
      set
      {
      }
    }

    public InputBase InputController
    {
      get => this._inputController;
      set => this._inputController = value;
    }

    internal bool Loading { get; set; }

    private bool SnapInputTimeEllapsed { get; set; }

    public UIControl CursorCaptureControl { get; set; }

    public bool IsCursorCaptured => this.CursorCaptureControl != null;

    public SnapDirection CursorSnapDirections { get; set; }

    internal DataFileRecord ControlsContentRootRecord { get; set; }

    private BlendState LastBlendState { get; set; }

    public virtual BoundingSquare BoundingBox
    {
      get => new BoundingSquare(this.AbsolutePositionInPixels, this.Size.Width, this.Size.Height);
    }

    public UIScreen(ScreenNavigator owner)
      : base(owner)
    {
      this._controls = new ControlCollection((IUIControl) this);
      this._container = new ContainerControl(this._controls);
      this.UnitType = UnitsType.Point;
      this.Size = this.PixelsToScreenUnits(new Size((float) BrainGame.ScreenWidth, (float) BrainGame.ScreenHeight));
      this.Position = Vector2.Zero;
      this.BlendColor = Color.White;
      this.CursorEnabled = true;
      this.Scale = new Vector2(1f, 1f);
      this.AcceptControllerInput = true;
      this.BackgroundImageBlendColor = Color.White;
      this.BackgroundColor = Color.Transparent;
      this.CursorSnapDirections = SnapDirection.All;
      this._tmrCursorSnap = new UITimer(this, 225.0, false);
      this._tmrCursorSnap.OnTimer += new UIControl.UIEvent(this._tmrCursorSnap_OnTimer);
      this.Controls.Add((UIControl) this._tmrCursorSnap);
      this.SnapInputTimeEllapsed = true;
      this.CursorMode = BrainGame.Settings.MenuCursorMode;
    }

    internal override void Load()
    {
      this.Loading = true;
      this.OnLoad();
      foreach (UIControl control in this.Controls)
        control.InternalLoad();
      this.Loading = false;
    }

    internal override void InitializeFromContent()
    {
      base.InitializeFromContent();
      if (this.ScreenContentRootRecord != null)
      {
        this.InitializeFromDataFileRecord();
        this.ControlsContentRootRecord = this.ScreenContentRootRecord.SelectRecord("Controls");
      }
      foreach (UIControl control in this.Controls)
        control.InitializeFromContent();
      if (this.OnAfterInitializeFromContent != null)
        this.OnAfterInitializeFromContent((IUIControl) this);
      foreach (UIControl control in this.Controls)
        control.InternalAfterInitializeFromContent();
    }

    private void InitializeFromDataFileRecord()
    {
      this.BlendColor = this.GetContentPropertyValue<Color>("blendColor", this.BlendColor);
      this.InvokeInitializeFromContent();
    }

    protected T GetContentPropertyValue<T>(string propName, T defaultValue)
    {
      T contentPropertyValue = defaultValue;
      DataFileRecord parentRecord1 = this.ScreenContentRootRecord.SelectRecordByField("Properties", "presentationMode", (object) "");
      if (parentRecord1 != null)
        contentPropertyValue = this.GetPropertyValue<T>(parentRecord1, propName, defaultValue);
      DataFileRecord parentRecord2 = this.ScreenContentRootRecord.SelectRecordByField("Properties", "presentationMode", (object) BrainGame.Settings.PresentationModeString);
      if (parentRecord2 != null)
        contentPropertyValue = this.GetPropertyValue<T>(parentRecord2, propName, defaultValue);
      return contentPropertyValue;
    }

    private T GetPropertyValue<T>(DataFileRecord parentRecord, string propName, T defaultValue)
    {
      return parentRecord.GetFieldValue<T>(propName, defaultValue);
    }

    internal override void Start()
    {
      base.Start();
      foreach (UIControl control in this.Controls)
        control.InternalOnScreenStart();
    }

    internal override void Update(BrainGameTime gameTime)
    {
      if (this.Closed)
        return;
      if (this.CursorEnabled && this.AcceptControllerInput)
      {
        if (this.InputController.ActionAccept && this.OnAccept != null)
          this.OnAccept((IUIControl) this);
        if (this.InputController.ActionBack && this.OnBack != null)
          this.OnBack((IUIControl) this);
        if (this.InputController.ActionCancel && this.OnCancel != null)
          this.OnCancel((IUIControl) this);
        if (this.CursorMode == CursorModes.SnapToControl)
          this.CheckControlSnap();
      }
      foreach (UIControl control in this._controls)
      {
        control.InternalUpdate(gameTime);
        if (this.Closed)
          return;
      }
      this.FocusControl = this.GetFocusControl((IUIControl) this);
      this.OnUpdate(gameTime);
    }

    internal override void InternalLanguageChanged()
    {
      foreach (UIControl control in this._controls)
        control.InternalLanguageChanged();
      base.InternalLanguageChanged();
    }

    internal override void InternalGameplayModeChanged()
    {
      foreach (UIControl control in this._controls)
        control.InternalGameplayModeChanged();
      base.InternalGameplayModeChanged();
    }

    public void BeginDraw(BlendState blendState)
    {
      this.LastBlendState = blendState;
      if (this.Mask != null)
        this.SpriteBatch.Begin(SpriteSortMode.Immediate, blendState, BrainGame.CurrentSampler, this.Mask.State, (RasterizerState) null, (Effect) BrainGame.RenderEffect);
      else
        this.SpriteBatch.Begin(SpriteSortMode.Immediate, blendState, BrainGame.CurrentSampler, (DepthStencilState) null, (RasterizerState) null, (Effect) BrainGame.RenderEffect);
    }

    public void EndDraw() => this.SpriteBatch.End();

    public void SuspendDraw() => this.EndDraw();

    public void ResumeDraw() => this.BeginDraw(this.LastBlendState);

    internal override void Draw()
    {
      if (this.BackgroundColor != Color.Transparent || this.BackgroundImage != null)
      {
        if (this.BackgroundColor != Color.Transparent && this.BackgroundColor.A != byte.MaxValue)
          this.BeginDraw(BlendState.AlphaBlend);
        else
          this.BeginDraw(BlendState.Opaque);
        if (this.BackgroundColor != Color.Transparent)
          this.SpriteBatch.Draw(UIScreen.ClearTexture, new Rectangle((int) this.PositionInPixels.X, (int) this.PositionInPixels.Y, (int) this.SizeInPixels.Width, (int) this.SizeInPixels.Height), this.BackgroundColor);
        if (this.BackgroundImage != null)
        {
          switch (this.BackgroundImageMode)
          {
            case ScreenBackgroudImageMode.Normal:
              this.BackgroundImage.Draw(this.PositionInPixels, 0, this.BackgroundImageBlendColor, this.SpriteBatch);
              break;
            case ScreenBackgroudImageMode.FitToScreen:
              this.BackgroundImage.Draw(this.PositionInPixels, BrainGame.ScreenRectangle, this.BackgroundImageBlendColor, this.SpriteBatch);
              break;
          }
        }
        this.EndDraw();
      }
      this.BeginDraw(BlendState.AlphaBlend);
      if (this.OnBeforeControlsDraw != null)
        this.OnBeforeControlsDraw((IUIControl) this);
      this.DrawControls();
      this.OnDraw();
      this.EndDraw();
    }

    public void DrawControls()
    {
      foreach (UIControl control in this._controls)
        control.InternalDraw();
    }

    private void CheckControlSnap()
    {
      if (!this.SnapInputTimeEllapsed)
        return;
      Vector2 directionalVector = Vector2.Zero;
      if (this._inputController.ActionRight && (this.CursorSnapDirections & SnapDirection.Right) == SnapDirection.Right)
        directionalVector = new Vector2(1f, 0.0f);
      else if (this._inputController.ActionDown && (this.CursorSnapDirections & SnapDirection.Down) == SnapDirection.Down)
        directionalVector = new Vector2(0.0f, -1f);
      else if (this._inputController.ActionLeft && (this.CursorSnapDirections & SnapDirection.Left) == SnapDirection.Left)
        directionalVector = new Vector2(-1f, 0.0f);
      else if (this._inputController.ActionUp && (this.CursorSnapDirections & SnapDirection.Up) == SnapDirection.Up)
        directionalVector = new Vector2(0.0f, 1f);
      else if (this._inputController.MotionPosition != Vector2.Zero && !this._inputController.WithMouse)
      {
        directionalVector = this._inputController.MotionPosition;
        if ((double) directionalVector.X > (double) directionalVector.Y && (double) directionalVector.X > 0.0 && (this.CursorSnapDirections & SnapDirection.Right) != SnapDirection.Right)
          directionalVector = Vector2.Zero;
        if ((double) directionalVector.X > (double) directionalVector.Y && (double) directionalVector.X < 0.0 && (this.CursorSnapDirections & SnapDirection.Left) != SnapDirection.Left)
          directionalVector = Vector2.Zero;
        if ((double) directionalVector.X < (double) directionalVector.Y && (double) directionalVector.Y > 0.0 && (this.CursorSnapDirections & SnapDirection.Down) != SnapDirection.Down)
          directionalVector = Vector2.Zero;
        if ((double) directionalVector.X < (double) directionalVector.Y && (double) directionalVector.Y < 0.0 && (this.CursorSnapDirections & SnapDirection.Up) != SnapDirection.Up)
          directionalVector = Vector2.Zero;
      }
      if (!(directionalVector != Vector2.Zero))
        return;
      UIControl controlToSnap = this.FindControlToSnap(directionalVector);
      if (controlToSnap == null)
        return;
      this.FocusControl = controlToSnap;
      this._tmrCursorSnap.Enabled = true;
      this.SnapInputTimeEllapsed = false;
    }

    private UIControl FindControlToSnap(Vector2 directionalVector)
    {
      float nearDistance = 999999f;
      return this.FindControlToSnap(BrainGame.GameCursor.Position, this.Controls, directionalVector, ref nearDistance);
    }

    private UIControl FindControlToSnap(
      Vector2 position,
      ControlCollection controls,
      Vector2 directionalVector,
      ref float nearDistance)
    {
      Vector2 rayEnd = position + new Vector2(directionalVector.Y, directionalVector.X);
      UIControl controlToSnap1 = (UIControl) null;
      foreach (UIControl control in controls)
      {
        if (control != this.FocusControl && control.CanFocus && !control.AnyChildAllowsFocus())
        {
          float num1 = Mathematics.ClassifyPointInRay(control.BoundingBox.UpperLeft, position, rayEnd);
          float num2 = Mathematics.ClassifyPointInRay(control.BoundingBox.UpperRight, position, rayEnd);
          float num3 = Mathematics.ClassifyPointInRay(control.BoundingBox.LowerLeft, position, rayEnd);
          float num4 = Mathematics.ClassifyPointInRay(control.BoundingBox.LowerRight, position, rayEnd);
          if ((double) num1 < 0.0 && (double) num2 < 0.0 && (double) num3 < 0.0 && (double) num4 < 0.0)
          {
            float num5 = (position - control.CenterInPixels).Length();
            if ((double) num5 < (double) nearDistance && control.CursorStop)
            {
              nearDistance = num5;
              controlToSnap1 = control;
            }
          }
        }
        if (control.CanFocus)
        {
          UIControl controlToSnap2 = this.FindControlToSnap(position, control.Controls, directionalVector, ref nearDistance);
          if (controlToSnap2 != null)
            controlToSnap1 = controlToSnap2;
        }
      }
      return controlToSnap1;
    }

    private UIControl GetFocusControl(IUIControl control)
    {
      if (this.IsCursorCaptured)
        return this.FocusControl;
      if (!this.InputController.IsMotionPointerDown)
        return (UIControl) null;
      if (!control.CursorInside && !control.CanFocus)
        return (UIControl) null;
      for (int i = control.Controls.Count - 1; i >= 0; --i)
      {
        if (control.Controls[i].CheckCursorInside() && control.Controls[i].CanFocus)
          return this.GetFocusControl((IUIControl) control.Controls[i]) ?? control.Controls[i];
      }
      return control as UIControl;
    }

    private void _tmrCursorSnap_OnTimer(IUIControl sender) => this.SnapInputTimeEllapsed = true;

    public void CaptureCursor(UIControl control) => this.CursorCaptureControl = control;

    public void ReleaseCursor() => this.CursorCaptureControl = (UIControl) null;

    public float ScreenUnitToPixelsX(float val)
    {
      return (float) ((double) val * (double) BrainGame.ScreenWidth / 10000.0);
    }

    public float ScreenUnitToPixelsY(float val)
    {
      return (float) ((double) val * (double) BrainGame.ScreenHeight / 10000.0);
    }

    public Vector2 ScreenUnitToPixels(Vector2 vector)
    {
      return new Vector2(this.ScreenUnitToPixelsX(vector.X), this.ScreenUnitToPixelsY(vector.Y));
    }

    public Size ScreenUnitToPixels(Size size)
    {
      return new Size(this.ScreenUnitToPixelsX(size.Width), this.ScreenUnitToPixelsY(size.Height));
    }

    public float PixelsToScreenUnitsX(float val) => val * 10000f / (float) BrainGame.ScreenWidth;

    public float PixelsToScreenUnitsY(float val) => val * 10000f / (float) BrainGame.ScreenHeight;

    public Size PixelsToScreenUnits(Size size)
    {
      return new Size(this.PixelsToScreenUnitsX(size.Width), this.PixelsToScreenUnitsY(size.Height));
    }

    public BoundingSquare PixelsToScreenUnits(BoundingSquare bb)
    {
      return new BoundingSquare(this.PixelsToScreenUnits(bb.UpperLeft), this.PixelsToScreenUnits(bb.LowerRight));
    }

    public Vector2 PixelsToScreenUnits(Vector2 vector)
    {
      return new Vector2(this.PixelsToScreenUnitsX(vector.X), this.PixelsToScreenUnitsY(vector.Y));
    }

    public Color BlendColor
    {
      get => this._blendColor;
      set => this._blendColor = value;
    }

    public Vector2 AbsolutePositionInPixels => this.PositionInPixels;

    public Vector2 AbsolutePosition => this.Position;

    protected Vector2 NativeResolution(Vector2 vector)
    {
      return new Vector2(vector.X * (float) BrainGame.NativeScreenWidth / (float) BrainGame.ScreenWidth, vector.Y * (float) BrainGame.NativeScreenHeight / (float) BrainGame.ScreenHeight);
    }

    protected Size NativeResolution(Size size)
    {
      return new Size(size.Width * (float) BrainGame.NativeScreenWidth / (float) BrainGame.ScreenWidth, size.Height * (float) BrainGame.NativeScreenHeight / (float) BrainGame.ScreenHeight);
    }

    protected float FromNativeResolutionY(float val)
    {
      return val * (float) BrainGame.ScreenHeight / (float) BrainGame.NativeScreenHeight;
    }

    protected float FromNativeResolutionX(float val)
    {
      return val * (float) BrainGame.ScreenWidth / (float) BrainGame.NativeScreenWidth;
    }

    protected Vector2 FromNativeResolution(Vector2 vector)
    {
      return new Vector2(vector.X * (float) BrainGame.ScreenWidth / (float) BrainGame.NativeScreenWidth, vector.Y * (float) BrainGame.ScreenHeight / (float) BrainGame.NativeScreenHeight);
    }
  }
}
