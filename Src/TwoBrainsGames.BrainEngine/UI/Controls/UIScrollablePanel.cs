
// Type: TwoBrainsGames.BrainEngine.UI.Controls.UIScrollablePanel
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.BrainEngine.UI.Controls
{
  public class UIScrollablePanel : UIPanel
  {
    protected Vector2 PanStartPosition;
    protected Vector2 PreviousPosition;
    protected Vector2 LastOffset;
    public UIScrollablePanel.PanState State;
    public UIScrollablePanel.PanState PreviousState;
    public UIScrollablePanel.PanelOrientation Orientation;
    public Rectangle PageRect;
    public Rectangle ClipRect;
    public float PanDistance;
    public double PanEllapsedTime;
    private FlickEffect _flickEffect;
    public UISlider _connectedSlider;

    public event UIControl.UIEvent OnScroll;

    public Rectangle PageRectInUnits
    {
      get
      {
        return new Rectangle((int) this.ScreenOwner.PixelsToScreenUnitsX((float) this.PageRect.X), (int) this.ScreenOwner.PixelsToScreenUnitsY((float) this.PageRect.Y), (int) this.ScreenOwner.PixelsToScreenUnitsX((float) this.PageRect.Width), (int) this.ScreenOwner.PixelsToScreenUnitsY((float) this.PageRect.Height));
      }
      set
      {
        this.PageRect = new Rectangle((int) this.ScreenUnitToPixelsX((float) value.X), (int) this.ScreenUnitToPixelsY((float) value.Y), (int) this.ScreenUnitToPixelsX((float) value.Width), (int) this.ScreenUnitToPixelsY((float) value.Height));
      }
    }

    public Rectangle ClipRectInUnits
    {
      get
      {
        return new Rectangle((int) this.ScreenOwner.PixelsToScreenUnitsX((float) this.ClipRect.X), (int) this.ScreenOwner.PixelsToScreenUnitsY((float) this.ClipRect.Y), (int) this.ScreenOwner.PixelsToScreenUnitsX((float) this.ClipRect.Width), (int) this.ScreenOwner.PixelsToScreenUnitsY((float) this.ClipRect.Height));
      }
      set
      {
        this.ClipRect = new Rectangle((int) this.ScreenUnitToPixelsX((float) value.X), (int) this.ScreenUnitToPixelsY((float) value.Y), (int) this.ScreenUnitToPixelsX((float) value.Width), (int) this.ScreenUnitToPixelsY((float) value.Height));
      }
    }

    public bool ClipToParent { get; set; }

    public UIScrollablePanel(UIScreen screenOwner, UIScrollablePanel.PanelOrientation orientation)
      : this(screenOwner, Vector2.Zero, orientation)
    {
    }

    public UIScrollablePanel(
      UIScreen screenOwner,
      Vector2 position,
      UIScrollablePanel.PanelOrientation orientation)
      : base(screenOwner)
    {
      this.Position = position;
      this.Orientation = orientation;
      this.PageRect = new Rectangle(0, 0, BrainGame.ScreenWidth, BrainGame.ScreenHeight);
      this.ClipRect = new Rectangle(0, 0, BrainGame.ScreenWidth, BrainGame.ScreenHeight);
      this._flickEffect = new FlickEffect(Vector2.Zero);
      this._flickEffect.Active = false;
      this._flickEffect.AutoDeleteOnEnd = false;
      this.EffectsBlender.Add((ITransformEffect) this._flickEffect);
      this.OnScreenStart += new UIControl.UIEvent(this.UIScrollablePanel_OnScreenStart);
    }

    private void UIScrollablePanel_OnScreenStart(IUIControl sender) => this.StopFlick();

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      if (this.ClipToParent)
      {
        this.ClipRect = this.Parent.BoundingBox.ToRect();
        if (this.ClipRect.X + this.ClipRect.Width > BrainGame.ScreenWidth)
          this.ClipRect = new Rectangle(this.ClipRect.X, this.ClipRect.Y, BrainGame.ScreenWidth - this.ClipRect.X, this.ClipRect.Height);
        if (this.ClipRect.Y + this.ClipRect.Height > BrainGame.ScreenHeight)
          this.ClipRect = new Rectangle(this.ClipRect.X, this.ClipRect.Y, this.ClipRect.Width, BrainGame.ScreenHeight - this.ClipRect.Y);
        this.PageRect = this.ClipRect;
      }
      this.PreviousState = this.State;
      if (this.State == UIScrollablePanel.PanState.Flicking && !this._flickEffect.Active || this.State != UIScrollablePanel.PanState.Flicking && this.ScreenOwner.InputController.ActionNone)
        this.State = UIScrollablePanel.PanState.None;
      if ((this.ClipToParent && this.Parent.CheckCursorInside() || !this.ClipToParent) && (this.Orientation == UIScrollablePanel.PanelOrientation.Horizontal && this.ScreenOwner.InputController.ActionHorizontalDrag || this.Orientation == UIScrollablePanel.PanelOrientation.Vertical && this.ScreenOwner.InputController.ActionVerticalDrag) && (this.State == UIScrollablePanel.PanState.None || this.State == UIScrollablePanel.PanState.Flicking))
        this.State = UIScrollablePanel.PanState.DragStart;
      if (this.State != UIScrollablePanel.PanState.None && this.State != UIScrollablePanel.PanState.Flicking && this.ScreenOwner.InputController.ActionDragComplete)
        this.State = UIScrollablePanel.PanState.DragEnd;
      if (this.State == UIScrollablePanel.PanState.DragStart)
      {
        this.PanEllapsedTime = 0.0;
        this.PanDistance = 0.0f;
        this.PanStartPosition = this.ScreenOwner.InputController.MotionPosition;
        this.PreviousPosition = this.Position;
        this.State = UIScrollablePanel.PanState.Dragging;
      }
      if (this.State == UIScrollablePanel.PanState.Dragging)
      {
        this.PanEllapsedTime += gameTime.ElapsedRealTime.TotalMilliseconds;
        this.LastOffset = this.ScreenOwner.InputController.MotionPosition - this.PanStartPosition;
        if (this.LastOffset != Vector2.Zero)
        {
          this.PanDistance += this.LastOffset.Length();
          if (this.Orientation == UIScrollablePanel.PanelOrientation.Horizontal)
          {
            float x = this.PositionInPixels.X + this.LastOffset.X;
            if ((double) x > 0.0)
            {
              x = 0.0f;
              this.StopFlick();
            }
            float num = Math.Abs(this.SizeInPixels.Width - this.Parent.SizeInPixels.Width) * -1f;
            if ((double) x < (double) num)
            {
              x = num;
              this.StopFlick();
            }
            this.PositionInPixels = new Vector2(x, this.PositionInPixels.Y);
            this.PanStartPosition = this.ScreenOwner.InputController.MotionPosition;
            this.UpdateLayout();
            this.InvokeOnScroll();
          }
          if (this.Orientation == UIScrollablePanel.PanelOrientation.Vertical)
          {
            float y = this.PositionInPixels.Y + this.LastOffset.Y;
            if ((double) y > 0.0)
            {
              y = 0.0f;
              this.StopFlick();
            }
            float num = Math.Abs(this.SizeInPixels.Height - (float) this.PageRect.Height) * -1f;
            if ((double) y < (double) num)
            {
              y = num;
              this.StopFlick();
            }
            this.PositionInPixels = new Vector2(this.PositionInPixels.X, y);
            this.PanStartPosition = this.ScreenOwner.InputController.MotionPosition;
            this.UpdateLayout();
            this.InvokeOnScroll();
          }
        }
      }
      if (this.State == UIScrollablePanel.PanState.DragEnd)
      {
        if ((double) this.PanDistance > 20.0)
        {
          this.State = UIScrollablePanel.PanState.Flicking;
          float num1 = this.PanDistance / (float) this.PanEllapsedTime;
          float num2 = this.LastOffset.Length() / (float) gameTime.ElapsedRealTime.TotalMilliseconds;
          Vector2 lastOffset = this.LastOffset;
          if (lastOffset != Vector2.Zero)
          {
            lastOffset.Normalize();
            Vector2 speed = lastOffset * num2 * 1000f;
            if (this.Orientation == UIScrollablePanel.PanelOrientation.Horizontal)
              speed = new Vector2(speed.X, 0.0f);
            if (this.Orientation == UIScrollablePanel.PanelOrientation.Vertical)
              speed = new Vector2(0.0f, speed.Y);
            this._flickEffect.Reset(speed);
            this._flickEffect.Active = true;
          }
        }
        else
          this.State = UIScrollablePanel.PanState.None;
      }
      if (this.State == UIScrollablePanel.PanState.Flicking)
      {
        if (this.Orientation == UIScrollablePanel.PanelOrientation.Horizontal)
        {
          float x = this.PositionInPixels.X;
          float num = Math.Abs(this.SizeInPixels.Width - this.Parent.SizeInPixels.Width) * -1f;
          if ((double) x > 0.0)
          {
            x = 0.0f;
            this.State = UIScrollablePanel.PanState.None;
          }
          if ((double) x < (double) num)
          {
            x = num;
            this.State = UIScrollablePanel.PanState.None;
          }
          if (this.State == UIScrollablePanel.PanState.None)
          {
            this.StopFlick();
            this.PositionInPixels = new Vector2(x, this.PositionInPixels.Y);
            this.PanStartPosition = this.ScreenOwner.InputController.MotionPosition;
            this.UpdateLayout();
          }
          this.InvokeOnScroll();
        }
        if (this.Orientation == UIScrollablePanel.PanelOrientation.Vertical)
        {
          float y = this.PositionInPixels.Y;
          float num = Math.Abs(this.SizeInPixels.Height - (float) this.PageRect.Height) * -1f;
          if ((double) y > 0.0)
          {
            y = 0.0f;
            this.State = UIScrollablePanel.PanState.None;
          }
          if ((double) y < (double) num)
          {
            y = num;
            this.State = UIScrollablePanel.PanState.None;
          }
          if (this.State == UIScrollablePanel.PanState.None)
          {
            this.StopFlick();
            this.PositionInPixels = new Vector2(this.PositionInPixels.X, y);
            this.PanStartPosition = this.ScreenOwner.InputController.MotionPosition;
            this.UpdateLayout();
          }
          this.InvokeOnScroll();
        }
      }
      if (this.Orientation == UIScrollablePanel.PanelOrientation.Horizontal)
      {
        if ((double) this.Position.X > 0.0)
        {
          this.Position = new Vector2(0.0f, this.Position.Y);
          this.StopFlick();
          this.InvokeOnScroll();
        }
        if ((double) Math.Abs(this.Position.X) > (double) Math.Abs(this.Width - this.Parent.Size.Width))
        {
          this.Position = new Vector2(this.Width - this.Parent.Size.Width, 0.0f);
          this.StopFlick();
          this.InvokeOnScroll();
        }
      }
      else
      {
        if ((double) this.Position.Y > 0.0)
        {
          this.Position = new Vector2(this.Position.X, 0.0f);
          this.StopFlick();
          this.InvokeOnScroll();
        }
        if ((double) Math.Abs(this.Position.Y) > (double) Math.Abs(this.Height - this.Parent.Size.Height))
        {
          this.Position = new Vector2(0.0f, this.Parent.Size.Height - this.Height);
          this.StopFlick();
          this.InvokeOnScroll();
        }
      }
      if (this._connectedSlider == null)
        return;
      this._connectedSlider.Value = -this.Position.Y;
    }

    public void StopFlick()
    {
      this._flickEffect.Active = false;
      this.State = UIScrollablePanel.PanState.None;
    }

    public bool AtBeginning()
    {
      return this.Orientation == UIScrollablePanel.PanelOrientation.Horizontal ? (double) this.Position.X == 0.0 : (double) this.Position.Y == 0.0;
    }

    public bool AtEnd()
    {
      return this.Orientation == UIScrollablePanel.PanelOrientation.Horizontal ? (double) Math.Abs(this.Position.X) == (double) Math.Abs(this.Width - this.Parent.Size.Width) : (double) Math.Abs(this.Position.Y) == (double) Math.Abs(this.Height - this.Parent.Size.Height);
    }

    protected bool AtBound(UIScrollablePanel.BoundType bound)
    {
      float num1 = 0.01f;
      bool flag = false;
      float num2 = Math.Abs(this.PositionInPixels.X);
      float num3 = Math.Abs(this.PositionInPixels.Y);
      float num4 = Math.Abs(this.SizeInPixels.Width - this.ScreenOwner.SizeInPixels.Width);
      float num5 = Math.Abs(this.SizeInPixels.Height - (float) this.PageRect.Height);
      if (this.Orientation == UIScrollablePanel.PanelOrientation.Horizontal)
      {
        switch (bound)
        {
          case UIScrollablePanel.BoundType.Left:
            flag = (double) num2 < (double) num1;
            break;
          case UIScrollablePanel.BoundType.Right:
            flag = (double) num2 > (double) num4 - (double) num1;
            break;
        }
      }
      else if (this.Orientation == UIScrollablePanel.PanelOrientation.Vertical)
      {
        switch (bound)
        {
          case UIScrollablePanel.BoundType.Up:
            flag = (double) num3 < (double) num1;
            break;
          case UIScrollablePanel.BoundType.Down:
            flag = (double) num3 > (double) num5 - (double) num1;
            break;
        }
      }
      return flag;
    }

    public bool AtLeftBound() => this.AtBound(UIScrollablePanel.BoundType.Left);

    public bool AtRightBound() => this.AtBound(UIScrollablePanel.BoundType.Right);

    public bool AtUpBound() => this.AtBound(UIScrollablePanel.BoundType.Up);

    public bool AtDownBound() => this.AtBound(UIScrollablePanel.BoundType.Down);

    public override void BeginDraw()
    {
      base.BeginDraw();
      if (!this.ClipToParent)
        return;
      this.ScreenOwner.SuspendDraw();
      RasterizerState rasterizerState = new RasterizerState();
      rasterizerState.ScissorTestEnable = true;
      BrainGame.GraphicsManager.GraphicsDevice.RasterizerState = rasterizerState;
      BrainGame.GraphicsManager.GraphicsDevice.ScissorRectangle = this.ClipRect;
      this.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, (DepthStencilState) null, rasterizerState);
    }

    public override void EndDraw()
    {
      base.EndDraw();
      if (!this.ClipToParent)
        return;
      this.SpriteBatch.End();
      this.ScreenOwner.ResumeDraw();
    }

    public void SetScroll(float value)
    {
      switch (this.Orientation)
      {
        case UIScrollablePanel.PanelOrientation.Horizontal:
          this.Position = new Vector2(-value, this.Position.Y);
          if (this._connectedSlider != null)
          {
            this._connectedSlider.Value = -this.Position.X;
            break;
          }
          break;
        case UIScrollablePanel.PanelOrientation.Vertical:
          this.Position = new Vector2(this.Position.X, -value);
          if (this._connectedSlider != null)
          {
            this._connectedSlider.Value = -this.Position.Y;
            break;
          }
          break;
      }
      this.InvokeOnScroll();
    }

    public void ConnectSlider(UISlider slider)
    {
      if (this._connectedSlider != null)
        this._connectedSlider.OnValueChanged -= new UIControl.UIEvent(this._connectedSlider_OnValueChanged);
      this._connectedSlider = slider;
      this._connectedSlider.OnValueChanged += new UIControl.UIEvent(this._connectedSlider_OnValueChanged);
      this._connectedSlider.MinValue = 0.0f;
      this._connectedSlider.MaxValue = this.Height - this.Parent.Size.Height;
    }

    private void _connectedSlider_OnValueChanged(IUIControl sender)
    {
      this.SetScroll(this._connectedSlider.Value);
    }

    private void InvokeOnScroll()
    {
      if (this.OnScroll == null)
        return;
      this.OnScroll((IUIControl) this);
    }

    public enum PanelOrientation
    {
      Horizontal,
      Vertical,
    }

    public enum PanState
    {
      None,
      DragStart,
      Dragging,
      DragEnd,
      Flicking,
    }

    public enum BoundType
    {
      Up,
      Down,
      Left,
      Right,
    }
  }
}
