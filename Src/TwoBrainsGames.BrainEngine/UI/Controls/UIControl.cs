
// Type: TwoBrainsGames.BrainEngine.UI.Controls.UIControl
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.BrainEngine.UI.Controls
{
  public class UIControl : IUIControl
  {
    private const int ONACCEPT_EFFECT_IDX = 0;
    private const int ONFOCUS_EFFECT_IDX = 1;
    private Size _size;
    private UnitsType _unitType;
    private bool _visible;
    private Color _blendColor;
    private Vector2 _scale;
    private bool _acceptControllerInput;
    private bool _cursorDownOnControl;
    private Rectangle _clientRect;
    private Rectangle _clientRectInPixels;
    private IUIControl _parent;
    private string _textResourceId;
    private Vector2 _position;
    private AlignModes _parentAlignment;

    public event UIControl.UIEvent OnSizeChanged;

    public event UIControl.UIEvent OnHide;

    public event UIControl.UIEvent OnHideBegin;

    public event UIControl.UIEvent OnShow;

    public event UIControl.UIEvent OnShowBegin;

    public event UIControl.UIEvent OnEnter;

    public event UIControl.UIEvent OnLeave;

    public event UIControl.UIEvent OnAccept;

    public event UIControl.UIControlAddedEvent OnBeforeControlAdded;

    public event UIControl.UIEvent OnAcceptBegin;

    public event UIControl.UIEvent OnDoubleTapAccept;

    public event UIControl.UIEvent OnDoubleTapAcceptBegin;

    public event UIControl.UIEvent OnBack;

    public event UIControl.UIEvent OnCancel;

    public event UIControl.UIEvent OnControllerAction;

    public event UIControl.UIEvent OnFocus;

    public event UIControl.UIEvent OnLostFocus;

    public event UIControl.UIEvent OnMotionPointerDown;

    public event UIControl.UIEvent OnMotionPointerUp;

    public event UIControl.UIEvent OnLanguageChanged;

    public event UIControl.UIEvent OnScreenStart;

    public event UIControl.UIEvent OnInitializeFromContent;

    public event UIControl.UIEvent OnAfterInitializeFromContent;

    public event UIControl.UIEvent OnGameplayModeChanged;

    public Sample ShowSoundEffect { get; set; }

    public string Name { get; set; }

    public Vector2 Position
    {
      get => this._position;
      set
      {
        if (!(this._position != value))
          return;
        this._position = value;
        this.NeedUpdateLayout = true;
      }
    }

    public Rectangle ClientRect
    {
      get => this._clientRect;
      private set
      {
        this._clientRect = value;
        Vector2 vector1 = new Vector2((float) this._clientRect.X, (float) this._clientRect.Y);
        Vector2 vector2 = new Vector2((float) this._clientRect.Width, (float) this._clientRect.Height);
        Vector2 pixels1 = this.ScreenUnitToPixels(vector1);
        Vector2 pixels2 = this.ScreenUnitToPixels(vector2);
        this._clientRectInPixels = new Rectangle((int) pixels1.X, (int) pixels1.Y, (int) pixels2.X, (int) pixels2.Y);
      }
    }

    public Rectangle ClientRectInPixels => this._clientRectInPixels;

    public float Rotation { get; set; }

    public Color BackgroundColor { get; set; }

    public bool UseBlendColorOnBackground { get; set; }

    public virtual bool Enabled { get; set; }

    public bool CursorStop { get; set; }

    public bool Hidden { get; set; }

    public bool AcceptControllerInput
    {
      get
      {
        return this.Parent != null && !this.Parent.AcceptControllerInput ? this.Parent.AcceptControllerInput : this._acceptControllerInput;
      }
      set => this._acceptControllerInput = value;
    }

    public bool CanFocus
    {
      get
      {
        return (!this.ScreenOwner.IsCursorCaptured || this.ScreenOwner.CursorCaptureControl == this) && this.Visible && this.Enabled && this.AcceptControllerInput && this.Active;
      }
    }

    public bool HasFocus => this.ScreenOwner != null && this.ScreenOwner.FocusControl == this;

    public TransformBlender EffectsBlender { get; private set; }

    private TransformBlender EventsEffectsBlender { get; set; }

    public IUIControl Parent
    {
      get => this._parent;
      set
      {
        if (this._parent != value)
        {
          this._parent = value;
          this.ParentChanged();
        }
        else
          this._parent = value;
      }
    }

    protected bool Busy { get; set; }

    private bool Active => this.Enabled && !this.Busy;

    private bool NeedUpdateLayout { get; set; }

    private UIControl.EffectContext CurrentEffectContext { get; set; }

    protected TransformEffectBase CurrentEffect { get; set; }

    public TransformEffectBase ShowEffect { get; set; }

    public TransformEffectBase HideEffect { get; set; }

    public TransformEffectBase OnFocusEffect { get; set; }

    public TransformEffectBase OnAcceptEffect { get; set; }

    public bool CursorInside { get; set; }

    public bool BlendColorWithParent { get; set; }

    public bool BlendScaleWithParent { get; set; }

    public bool CanBeScaled { get; set; }

    public int ControllerActionCode { get; set; }

    public ControlCollection Controls { get; private set; }

    public AlignModes ParentAlignment
    {
      get => this._parentAlignment;
      set
      {
        if (this._parentAlignment == value)
          return;
        this._parentAlignment = value;
        this.NeedUpdateLayout = true;
      }
    }

    public Vector2 ParentAlignmentOffset { get; set; }

    public Margin Margins { get; private set; }

    public UIScreen ScreenOwner { get; set; }

    public bool DropShadow { get; set; }

    public Color ShadowColor { get; set; }

    public Vector2 ShadowDistance { get; set; }

    public bool InvokeOnAcceptOnMotionUp { get; set; }

    public Color BlendColor
    {
      get
      {
        return this.Parent != null && this.BlendColorWithParent ? new Color(this._blendColor.ToVector4() * this.Parent.BlendColor.ToVector4()) : this._blendColor;
      }
      set => this._blendColor = value;
    }

    public virtual Vector2 Scale
    {
      get
      {
        return this.Parent != null && this.BlendScaleWithParent ? this._scale * this.Parent.Scale : this._scale;
      }
      set
      {
        if (!(this._scale != value))
          return;
        this._scale = value;
        this.NeedUpdateLayout = true;
        this.InvalidateChildsLayout();
      }
    }

    public virtual bool Visible
    {
      get => this._visible;
      set
      {
        this._visible = value;
        if (this._visible == value)
          return;
        this._visible = value;
        if (this.OnHide == null)
          return;
        this.OnHide((IUIControl) this);
      }
    }

    public ITransformEffect Effect
    {
      set
      {
        this.EffectsBlender.Clear();
        if (value == null)
          return;
        this.EffectsBlender.Add(value);
      }
      get => this.EffectsBlender.Count == 0 ? (ITransformEffect) null : this.EffectsBlender[0];
    }

    public UnitsType UnitType
    {
      get => this.ScreenOwner == null ? this._unitType : this.ScreenOwner.UnitType;
      set => this._unitType = value;
    }

    public virtual Size SizeScaled
    {
      get => new Size(this._size.Width * this.Scale.X, this._size.Height * this.Scale.Y);
      set => this.Size = new Size(value.Width * this.Scale.X, value.Height * this.Scale.Y);
    }

    public virtual Size Size
    {
      get => this._size;
      set
      {
        if ((double) this._size.Height == (double) value.Height && (double) this._size.Width == (double) value.Width)
          return;
        this._size = value;
        this.NeedUpdateLayout = true;
        this.InvalidateChildsLayout();
        if (this.OnSizeChanged == null || !this.LaunchOnSizeChanged)
          return;
        this.OnSizeChanged((IUIControl) this);
      }
    }

    public bool LaunchOnSizeChanged { get; set; }

    public Size SizeInPixels => this.ScreenUnitToPixels(this.Size);

    public Size SizeInPixelsScaled => this.ScreenUnitToPixels(this.SizeScaled);

    public Vector2 PositionInPixels
    {
      get => this.ScreenUnitToPixels(this.Position);
      set => this.Position = this.PixelsToScreenUnits(value);
    }

    public Vector2 CenterInPixels
    {
      get
      {
        return this.AbsolutePositionInPixels + new Vector2(this.SizeInPixelsScaled.Width / 2f, this.SizeInPixelsScaled.Height / 2f);
      }
    }

    public Vector2 Center
    {
      get
      {
        return this.AbsolutePosition + new Vector2(this.SizeScaled.Width / 2f, this.SizeScaled.Height / 2f);
      }
    }

    protected SpriteBatch SpriteBatch => this.ScreenOwner.SpriteBatch;

    public virtual BoundingSquare BoundingBox
    {
      get
      {
        return new BoundingSquare(this.AbsolutePositionInPixels, this.SizeInPixelsScaled.Width, this.SizeInPixelsScaled.Height);
      }
    }

    public virtual Vector2 AbsolutePositionInPixels
    {
      get => this.ScreenUnitToPixels(this.AbsolutePosition);
    }

    public virtual Vector2 AbsolutePosition
    {
      get
      {
        Vector2 zero = Vector2.Zero;
        Vector2 absolutePosition;
        if (this.Parent != null)
        {
          switch (this.ParentAlignment)
          {
            case AlignModes.None:
              absolutePosition = this.Parent.AbsolutePosition + this.Position * this.Parent.Scale;
              break;
            case AlignModes.Horizontaly:
            case AlignModes.Right:
            case AlignModes.Left:
              absolutePosition = this.Parent.AbsolutePosition + this.Position * new Vector2(1f, this.Parent.Scale.Y);
              break;
            case AlignModes.Vertically:
            case AlignModes.Bottom:
            case AlignModes.Top:
              absolutePosition = this.Parent.AbsolutePosition + this.Position * new Vector2(this.Parent.Scale.X, 1f);
              break;
            default:
              absolutePosition = this.Parent.AbsolutePosition + this.Position;
              break;
          }
        }
        else
          absolutePosition = this.Position;
        return absolutePosition;
      }
    }

    public bool WithFocus
    {
      get
      {
        return this.ScreenOwner.CursorMode == CursorModes.Free ? this.CursorInside : this.ScreenOwner.FocusControl == this;
      }
    }

    public float Top => this.Position.Y;

    public float Left => this.Position.X;

    public float Right => this.Position.X + this.Size.Width;

    public float Bottom => this.Position.Y + this.Size.Height;

    public Vector2 PivotPosition { get; set; }

    public float Width
    {
      get => this.Size.Width;
      set => this.Size = new Size(value, this.Size.Height);
    }

    public float Height
    {
      get => this.Size.Height;
      set => this.Size = new Size(this.Size.Width, value);
    }

    public virtual string Text { get; set; }

    public virtual string TextResourceId
    {
      get => this._textResourceId;
      set
      {
        this._textResourceId = value;
        this.Text = LanguageManager.GetString(value);
      }
    }

    private DataFileRecord ControlContentRootRecord { get; set; }

    public UIControl(UIScreen screenOwner)
    {
      this.ScreenOwner = screenOwner;
      this.Controls = new ControlCollection((IUIControl) this);
      this.Size = new Size(1000f, 1000f);
      this.BackgroundColor = Color.Transparent;
      this.Visible = true;
      this.Enabled = true;
      this.EffectsBlender = new TransformBlender();
      this.EventsEffectsBlender = new TransformBlender();
      this.BlendColor = Color.White;
      this.BlendColorWithParent = true;
      this.BlendScaleWithParent = true;
      this.CanBeScaled = true;
      this.AcceptControllerInput = true;
      this.Margins = new Margin(this);
      this.Scale = Vector2.One;
      this.ShadowDistance = new Vector2(10f, 10f);
      this.ShadowColor = new Color(0.0f, 0.0f, 0.0f, 0.3f);
      this.CursorStop = true;
      this.LaunchOnSizeChanged = true;
      this.NeedUpdateLayout = true;
    }

    internal void InternalLoad()
    {
      this.UpdateLayout();
      foreach (UIControl control in this.Controls)
        control.InternalLoad();
      this.Load();
    }

    protected void InitializeFromContent(string contentName)
    {
      this.ControlContentRootRecord = BrainGame.ResourceManager.Load<DataFileRecord>(
          Path.Combine(BrainGame.Settings.NavigatorControlContentFolder, contentName), 
          ResourceManager.ResourceManagerCacheType.Static);
      this.InitializeFromDataFileRecord();
    }

    public virtual void InitializeFromContent()
    {
      if (this.ScreenOwner.ControlsContentRootRecord != null && !string.IsNullOrEmpty(this.Name))
      {
        this.ControlContentRootRecord = 
                    this.ScreenOwner.ControlsContentRootRecord.SelectRecordByField(
                        "Control", "name", (object) this.Name);

        if (this.ControlContentRootRecord != null)
          this.InitializeFromDataFileRecord();
      }
      if (this.OnAfterInitializeFromContent != null)
        this.OnAfterInitializeFromContent((IUIControl) this);
      foreach (UIControl control in this.Controls)
        control.InitializeFromContent();
    }

    internal void InternalAfterInitializeFromContent()
    {
      if (this.OnAfterInitializeFromContent != null)
        this.OnAfterInitializeFromContent((IUIControl) this);
      foreach (UIControl control in this.Controls)
        control.InternalAfterInitializeFromContent();
    }

    private void InitializeFromDataFileRecord()
    {
      this.BlendColor = this.GetContentPropertyValue<Color>("blendColor", this.BlendColor);
      this.Position = this.GetContentPropertyValue<Vector2>("position", this.Position);
      this.ParentAlignment = (AlignModes) Enum.Parse(typeof (AlignModes), 
          this.GetContentPropertyValue<string>("parentAlignment", this.ParentAlignment.ToString()), true);
      this.Scale = this.GetContentPropertyValue<Vector2>("scale", this.Scale);
      this.Size = this.GetContentPropertyValue<Size>("size", this.Size);
      this.BackgroundColor = this.GetContentPropertyValue<Color>("backColor", this.BackgroundColor);
      this.Size = new Size(this.GetContentPropertyValue<float>("width", this.Size.Width), this.Size.Height);
      this.Size = new Size(this.Size.Width, this.GetContentPropertyValue<float>("height", this.Size.Height));
      this.Margins.Bottom = this.GetContentPropertyValue<float>("bottomMargin", this.Margins.Bottom);
      if (this.OnInitializeFromContent == null)
        return;
      this.OnInitializeFromContent((IUIControl) this);
    }

    protected T GetContentPropertyValue<T>(string propName, T defaultValue)
    {
      T contentPropertyValue = defaultValue;
      if (this.ControlContentRootRecord == null)
        return contentPropertyValue;
      DataFileRecord parentRecord1 = this.ControlContentRootRecord.SelectRecordByField(
          "Properties", "presentationMode", (object) "");

      if (parentRecord1 != null)
        contentPropertyValue = this.GetPropertyValue<T>(parentRecord1, propName, defaultValue);

      DataFileRecord parentRecord2 = this.ControlContentRootRecord.SelectRecordByField(
          "Properties", "presentationMode", (object) BrainGame.Settings.PresentationModeString);

      if (parentRecord2 != null)
        contentPropertyValue = this.GetPropertyValue<T>(parentRecord2, propName, defaultValue);

      return contentPropertyValue;
    }

    private T GetPropertyValue<T>(DataFileRecord parentRecord, string propName, T defaultValue)
    {
      return parentRecord.GetFieldValue<T>(propName, defaultValue);
    }

    internal void InternalUpdate(BrainGameTime gameTime)
    {
      if (this.ScreenOwner.Closed)
        return;
      this.UpdateLayout();
      if (this.Visible)
      {
        this.UpdateEffects(gameTime);
        if (this.UseBlendColorOnBackground)
          this.BackgroundColor = this.BlendColor;
        this.Update(gameTime);
        this.UpdateLayout();
        foreach (UIControl control in this.Controls)
          control.InternalUpdate(gameTime);
      }
      if (!this.Enabled || !this.Visible)
      {
        this.CursorInside = false;
      }
      else
      {
        if (this.ScreenOwner != null)
        {
          if (this.CanFocus && this.CheckCursorInside())
          {
            if (!this.CursorInside)
            {
              this.CursorInside = true;
              if (this.OnEnter != null)
                this.OnEnter((IUIControl) this);
            }
          }
          else if (this.CursorInside)
          {
            this.CursorInside = false;
            if (this.OnLeave != null)
              this.OnLeave((IUIControl) this);
          }
        }
        if (!this.Active)
          return;
        if (this.AcceptControllerInput)
          this.ProcessController();
        this.UpdateLayout();
      }
    }

    internal void InternalDraw()
    {
      if (!this.Visible || this.Hidden)
        return;
      this.UpdateLayout();
      this.BeginDraw();
      if (this.BackgroundColor != Color.Transparent)
        this.SpriteBatch.Draw(UIScreen.ClearTexture, this.BoundingBox.ToRect(), this.BackgroundColor);
      foreach (UIControl control in this.Controls)
        control.InternalDraw();
      this.Draw();
      this.EndDraw();
    }

    internal void InternalLanguageChanged()
    {
      if (!string.IsNullOrEmpty(this.TextResourceId))
        this.Text = LanguageManager.GetString(this.TextResourceId);
      if (this.OnLanguageChanged != null)
        this.OnLanguageChanged((IUIControl) this);
      foreach (UIControl control in this.Controls)
        control.InternalLanguageChanged();
    }

    internal void InternalGameplayModeChanged()
    {
      if (this.OnGameplayModeChanged != null)
        this.OnGameplayModeChanged((IUIControl) this);
      foreach (UIControl control in this.Controls)
        control.InternalGameplayModeChanged();
    }

    internal void InvalidateChildsLayout()
    {
      foreach (UIControl control in this.Controls)
      {
        if (control.ParentAlignment != AlignModes.None)
        {
          control.NeedUpdateLayout = true;
          control.InvalidateChildsLayout();
        }
      }
    }

    public virtual void Load()
    {
    }

    public virtual void BeginDraw()
    {
    }

    public virtual void Draw()
    {
    }

    public virtual void EndDraw()
    {
    }

    public virtual void OnResize()
    {
    }

    public virtual void Update(BrainGameTime gameTime)
    {
    }

    internal bool AnyChildAllowsFocus()
    {
      foreach (UIControl control in this.Controls)
      {
        if (control.AcceptControllerInput || control.AnyChildAllowsFocus())
          return true;
      }
      return false;
    }

    public void UpdateLayout()
    {
      if (!this.NeedUpdateLayout)
        return;
      this.ClientRect = new Rectangle((int) this.Position.X, (int) this.Position.Y, (int) this.Size.Width, (int) this.Size.Height);
      if (this.ParentAlignment == AlignModes.None)
        return;
      float num1 = this.Position.X;
      float num2 = this.Position.Y;
      if ((this.ParentAlignment & AlignModes.Horizontaly) == AlignModes.Horizontaly)
        num1 = (float) ((double) this.Parent.SizeScaled.Width / 2.0 - (double) this.SizeScaled.Width / 2.0);
      if ((this.ParentAlignment & AlignModes.Vertically) == AlignModes.Vertically)
        num2 = (float) ((double) this.Parent.SizeScaled.Height / 2.0 - (double) this.SizeScaled.Height / 2.0);
      if ((this.ParentAlignment & AlignModes.Right) == AlignModes.Right)
        num1 = this.Parent.SizeScaled.Width - this.SizeScaled.Width - (float) (int) this.Margins.Right;
      if ((this.ParentAlignment & AlignModes.Bottom) == AlignModes.Bottom)
        num2 = this.Parent.SizeScaled.Height - this.SizeScaled.Height - (float) (int) this.Margins.Bottom;
      if ((this.ParentAlignment & AlignModes.Left) == AlignModes.Left)
        num1 = (float) BrainGame.ScreenRectangle.X + (float) (int) this.Margins.Left;
      if ((this.ParentAlignment & AlignModes.Top) == AlignModes.Top)
        num2 = (float) BrainGame.ScreenRectangle.Y + (float) (int) this.Margins.Top;
      float x = num1 + this.ParentAlignmentOffset.X;
      float y = num2 + this.ParentAlignmentOffset.Y;
      this.Position = new Vector2(x, y);
      this.ClientRect = new Rectangle((int) x, (int) y, (int) this.Size.Width, (int) this.Size.Height);
      this.NeedUpdateLayout = false;
      this.InvalidateChildsLayout();
    }

    private void ProcessController()
    {
      if (this.CursorInside && this.WithFocus && (this.ScreenOwner.InputController.ActionAccept || !this.ScreenOwner.InputController.IsMotionPointerDown && this.ScreenOwner.InputController.WasMotionPointerDown && this.InvokeOnAcceptOnMotionUp) || ((InputBase.InputActions) this.ControllerActionCode & this.ScreenOwner.InputController.Actions) != InputBase.InputActions.None)
      {
        this.InvokeOnAcceptBegin();
        if (this.OnAcceptEffect != null)
        {
          this.EventsEffectsBlender.DeleteEffects(0);
          this.OnAcceptEffect.Reset();
          this.OnAcceptEffect.OnEnd = new TransformEffectBase.OnEndEvent(this.OnAcceptEffect_Ended);
          this.EventsEffectsBlender.Add((ITransformEffect) this.OnAcceptEffect, 0);
        }
        else
          this.InvokeOnAccept();
      }
      if (this.CursorInside && this.WithFocus && this.ScreenOwner.InputController.ActionAcceptDown)
      {
        this._cursorDownOnControl = true;
        if (this.OnMotionPointerDown != null)
        {
          this._cursorDownOnControl = true;
          this.OnMotionPointerDown((IUIControl) this);
        }
      }
      if (this._cursorDownOnControl && this.ScreenOwner.InputController.ActionAcceptUp && this.OnMotionPointerUp != null)
      {
        this._cursorDownOnControl = false;
        this.OnMotionPointerUp((IUIControl) this);
      }
      if (this.CursorInside && this.ScreenOwner.InputController.ActionBack && this.OnBack != null)
        this.OnBack((IUIControl) this);
      if (this.CursorInside && this.ScreenOwner.InputController.ActionCancel && this.OnCancel != null)
        this.OnCancel((IUIControl) this);
      if (this.ControllerActionCode == 0 || ((InputBase.InputActions) this.ControllerActionCode & this.ScreenOwner.InputController.Actions) == InputBase.InputActions.None || this.OnControllerAction == null)
        return;
      this.OnControllerAction((IUIControl) this);
    }

    private void HideEffectEnded()
    {
      this.CurrentEffect = (TransformEffectBase) null;
      this.Busy = false;
      this.CurrentEffectContext = UIControl.EffectContext.None;
      this._visible = false;
      if (this.OnHide == null)
        return;
      this.OnHide((IUIControl) this);
    }

    private void ShowEffectEnded()
    {
      this.CurrentEffect = (TransformEffectBase) null;
      this.Busy = false;
      this.CurrentEffectContext = UIControl.EffectContext.None;
      if (this.OnShow == null)
        return;
      this.OnShow((IUIControl) this);
    }

    private void OnAcceptEffect_Ended(object param)
    {
      if (this.OnAccept == null)
        return;
      this.OnAccept((IUIControl) this);
    }

    public virtual bool CheckCursorInside()
    {
      return this.ScreenOwner.FocusControl == this && !this.BoundingBox.Contains(BrainGame.GameCursor.Position) ? this.BoundingBox.Contains(BrainGame.GameCursor.Position) : this.BoundingBox.Contains(BrainGame.GameCursor.Position);
    }

    protected void UpdateCurrentEffect(BrainGameTime gameTime)
    {
      if (this.CurrentEffect == null)
        return;
      this.CurrentEffect.Update(gameTime);
      this.ApplySingleEffect(this.CurrentEffect);
      if (!this.CurrentEffect.Ended)
        return;
      switch (this.CurrentEffectContext)
      {
        case UIControl.EffectContext.Hidding:
          this.HideEffectEnded();
          break;
        case UIControl.EffectContext.Showing:
          this.ShowEffectEnded();
          break;
      }
    }

    protected void UpdateEffects(BrainGameTime gameTime)
    {
      if (this.CurrentEffect != null)
      {
        this.UpdateCurrentEffect(gameTime);
      }
      else
      {
        if (this.EffectsBlender.WithActiveEffects)
        {
          this.EffectsBlender.Update(gameTime);
          this.BlendColor = this.EffectsBlender.Color;
          this.PositionInPixels += this.EffectsBlender.PositionV2;
          this.Scale = this._scale + this.EffectsBlender._scale;
        }
        if (!this.EventsEffectsBlender.WithActiveEffects)
          return;
        this.EventsEffectsBlender.Update(gameTime);
        this.BlendColor = this.EventsEffectsBlender.Color;
        this.PositionInPixels += this.EventsEffectsBlender.PositionV2;
        this.Scale = this._scale + this.EventsEffectsBlender._scale;
      }
    }

    internal virtual void UnFocus()
    {
      this.EventsEffectsBlender.DeleteEffects(1);
      this.InvokeOnLostFocus();
    }

    internal virtual void ParentChanged()
    {
    }

    public virtual void Focus()
    {
      if (!this.CanFocus)
        return;
      this.ScreenOwner.FocusControl = this;
      if (this.ScreenOwner != null && this.ScreenOwner.CursorMode == CursorModes.SnapToControl)
        BrainGame.GameCursor.Position = this.CenterInPixels;
      if (this.OnFocusEffect != null)
      {
        this.OnFocusEffect.Reset();
        this.EventsEffectsBlender.DeleteEffects(1);
        this.EventsEffectsBlender.Add((ITransformEffect) this.OnFocusEffect, 1);
      }
      if (this.OnFocus == null)
        return;
      this.OnFocus((IUIControl) this);
    }

    public void ApplySingleEffect(TransformEffectBase effect)
    {
      if (effect == null)
        return;
      this.BlendColor = effect.Color;
      this.Scale = effect.Scale;
    }

    public void ApplyEffects(TransformBlender effectsBlender)
    {
      this.BlendColor = effectsBlender.Color;
    }

    public void SendToBack()
    {
      if (this.Parent == null || !this.Parent.Controls.Contains(this))
        return;
      IUIControl parent = this.Parent;
      this.Parent.Controls.Remove(this);
      parent.Controls.InsertAt(0, this);
    }

    public void BringToFront()
    {
      if (this.Parent == null || !this.Parent.Controls.Contains(this))
        return;
      IUIControl parent = this.Parent;
      this.Parent.Controls.Remove(this);
      parent.Controls.InsertAt(parent.Controls.Count, this);
    }

    public void ScaleChilds(Vector2 scaleFactor)
    {
      foreach (UIControl control in this.Controls)
      {
        if (control.CanBeScaled)
          control.Scale *= scaleFactor;
      }
    }

    public virtual void Show()
    {
      if (this.ShowSoundEffect != null)
        this.ShowSoundEffect.Play();
      this.Visible = true;
      if (this.OnShowBegin != null)
        this.OnShowBegin((IUIControl) this);
      if (this.ShowEffect != null)
      {
        this.ShowEffect.Reset();
        this.CurrentEffect = this.ShowEffect;
        this.Busy = true;
        this.CurrentEffectContext = UIControl.EffectContext.Showing;
      }
      foreach (UIControl control in this.Controls)
      {
        if (control.Visible)
          control.Show();
      }
    }

    public virtual void Hide()
    {
      if (!this.Visible)
        return;
      if (this.OnHideBegin != null)
        this.OnHideBegin((IUIControl) this);
      if (this.HideEffect != null)
      {
        this.HideEffect.Reset();
        this.CurrentEffect = this.HideEffect;
        this.Busy = true;
        this.CurrentEffectContext = UIControl.EffectContext.Hidding;
      }
      else
      {
        this.Visible = true;
        foreach (UIControl control in this.Controls)
        {
          if (control.Visible)
            control.Hide();
        }
      }
    }

    public override string ToString() => this.Name == null ? base.ToString() : this.Name;

    internal void InternalOnScreenStart()
    {
      this.InvokeOnScreenStart();
      foreach (UIControl control in this.Controls)
        control.InternalOnScreenStart();
    }

    internal void MarginChanged() => this.NeedUpdateLayout = true;

    protected void InvokeOnScreenStart()
    {
      if (this.OnScreenStart == null)
        return;
      this.OnScreenStart((IUIControl) this);
    }

    protected void InvokeOnAcceptBegin()
    {
      if (this.OnAcceptBegin == null)
        return;
      this.OnAcceptBegin((IUIControl) this);
    }

    protected void InvokeOnAccept()
    {
      if (this.OnAccept == null)
        return;
      this.OnAccept((IUIControl) this);
    }

    internal bool InvokeOnBeforeControlAdded(UIControl control)
    {
      return this.OnBeforeControlAdded == null || this.OnBeforeControlAdded((IUIControl) this, control);
    }

    protected void InvokeOnDoubleTapAcceptBegin()
    {
      if (this.OnDoubleTapAcceptBegin == null)
        return;
      this.OnDoubleTapAcceptBegin((IUIControl) this);
    }

    protected void InvokeOnDoubleTapAccept()
    {
      if (this.OnDoubleTapAccept == null)
        return;
      this.OnDoubleTapAccept((IUIControl) this);
    }

    protected void InvokeOnShow()
    {
      if (this.OnShow == null)
        return;
      this.OnShow((IUIControl) this);
    }

    public void InvokeOnFocus()
    {
      if (this.OnFocus == null)
        return;
      this.OnFocus((IUIControl) this);
    }

    public void InvokeOnLostFocus()
    {
      if (this.OnLostFocus == null)
        return;
      this.OnLostFocus((IUIControl) this);
    }

    protected void InvokeOnHide()
    {
      if (this.OnHide == null)
        return;
      this.OnHide((IUIControl) this);
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

    protected Vector2 NativeResolution(Vector2 vector)
    {
      return new Vector2(vector.X * (float) BrainGame.NativeScreenWidth / (float) BrainGame.ScreenWidth, vector.Y * (float) BrainGame.NativeScreenHeight / (float) BrainGame.ScreenHeight);
    }

    protected Size NativeResolution(Size size)
    {
      return new Size(size.Width * (float) BrainGame.NativeScreenWidth / (float) BrainGame.ScreenWidth, size.Height * (float) BrainGame.NativeScreenHeight / (float) BrainGame.ScreenHeight);
    }

    protected BoundingSquare NativeResolution(BoundingSquare bs)
    {
      return new BoundingSquare(this.NativeResolution(bs.UpperLeft), this.NativeResolution(new Vector2(bs.Width, bs.Height)));
    }

    protected float NativeResolutionY(float val)
    {
      return val * (float) BrainGame.NativeScreenHeight / (float) BrainGame.ScreenHeight;
    }

    protected float NativeResolutionX(float val)
    {
      return val * (float) BrainGame.NativeScreenWidth / (float) BrainGame.ScreenWidth;
    }

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

    private enum EffectContext
    {
      None,
      Hidding,
      Showing,
      Focus,
    }

    public delegate void UIEvent(IUIControl sender);

    public delegate bool UIControlAddedEvent(IUIControl sender, UIControl control);
  }
}
