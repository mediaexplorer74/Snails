
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UISnailsScrollablePanel
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Effects;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UISnailsScrollablePanel : UIPanel
  {
    private UIPanel _pnlContainer;
    private UIScrollablePanel _panel;
    private UISlider _slider;
    private UIImage _imgPanelTop;
    private UIImage _imgPanelBottom;
    private UIArrow _arrowBeginning;
    private UIArrow _arrowEnd;
    private HooverEffect _arrowEffect;

    private bool WithSlider { get; set; }

    public bool ShowScrollIndicators { get; set; }

    public float Length
    {
      get
      {
        return this._panel.Orientation == UIScrollablePanel.PanelOrientation.Vertical ? this._panel.Height : this._panel.Width;
      }
      set
      {
        if (this._panel.Orientation == UIScrollablePanel.PanelOrientation.Vertical)
          this._panel.Height = value;
        else
          this._panel.Width = value;
        this.Refresh();
      }
    }

    private float Distance
    {
      get
      {
        return this._panel.Orientation == UIScrollablePanel.PanelOrientation.Vertical ? this._panel.Width : this._panel.Height;
      }
      set
      {
        if (this._panel.Orientation == UIScrollablePanel.PanelOrientation.Vertical)
          this._panel.Width = value;
        else
          this._panel.Height = value;
      }
    }

    public UIScrollablePanel.PanState State => this._panel.State;

    public UIScrollablePanel.PanState PreviousState => this._panel.PreviousState;

    public UIScrollablePanel.PanelOrientation Orientation
    {
      get => this._panel.Orientation;
      set => this._panel.Orientation = value;
    }

    public UISnailsScrollablePanel(
      UIScreen screenOwner,
      UIScrollablePanel.PanelOrientation orientation,
      bool withSlider,
      float sheetLength)
      : base(screenOwner)
    {
      this._pnlContainer = new UIPanel(screenOwner);
      this._pnlContainer.ParentAlignment = AlignModes.HorizontalyVertically;
      this._pnlContainer.ShowEffect = (TransformEffectBase) new SquashEffect(0.85f, 4f, 0.03f, Color.White, Vector2.One);
      this._pnlContainer.HideEffect = (TransformEffectBase) new PopOutEffect(new Vector2(1.2f, 1.2f), 6f);
      this._pnlContainer.BackgroundColor = new Color(0, 0, 0, 100);
      this._pnlContainer.OnHide += new UIControl.UIEvent(this._pnlContainer_OnHide);
      this._pnlContainer.OnShow += new UIControl.UIEvent(this._pnlContainer_OnShow);
      this.Controls.Add((UIControl) this._pnlContainer);
      this._panel = new UIScrollablePanel(screenOwner, orientation);
      this._panel.ClipToParent = true;
      this._panel.Size = new Size(sheetLength, sheetLength);
      this._panel.OnScroll += new UIControl.UIEvent(this._panel_OnScroll);
      this._pnlContainer.Controls.Add((UIControl) this._panel);
      this._slider = new UISlider(screenOwner, (string) null, "spriteset/boards/ScrollManipulator");
      this._slider.Orientation = orientation == UIScrollablePanel.PanelOrientation.Vertical ? SliderOrientation.Vertical : SliderOrientation.Horizontal;
      this._slider.BackgroundColor = new Color(0, 0, 0, 200);
      this._slider.AutoSetSliderSlot = true;
      this._slider.MinValue = 0.0f;
      this._slider.MaxValue = this._panel.Height;
      this._slider.Visible = withSlider;
      this._pnlContainer.Controls.Add((UIControl) this._slider);
      this._imgPanelTop = new UIImage(screenOwner);
      this._imgPanelTop.Scale = this.FromNativeResolution(Vector2.One);
      this._pnlContainer.Controls.Add((UIControl) this._imgPanelTop);
      this._imgPanelBottom = new UIImage(screenOwner);
      this._imgPanelBottom.Scale = this.FromNativeResolution(Vector2.One);
      this._pnlContainer.Controls.Add((UIControl) this._imgPanelBottom);
      this._arrowBeginning = new UIArrow(this.ScreenOwner, UIArrow.ArrowType.Up, UIArrow.ArrowSize.Small, this._arrowEffect);
      this._arrowBeginning.Visible = true;
      this._pnlContainer.Controls.Add((UIControl) this._arrowBeginning);
      this._arrowEnd = new UIArrow(this.ScreenOwner, UIArrow.ArrowType.Down, UIArrow.ArrowSize.Small, this._arrowEffect);
      this._arrowEnd.Visible = true;
      this._pnlContainer.Controls.Add((UIControl) this._arrowEnd);
      if (withSlider)
        this._panel.ConnectSlider(this._slider);
      this.WithSlider = withSlider;
      this.ShowScrollIndicators = true;
      this.OnBeforeControlAdded += new UIControl.UIControlAddedEvent(this.UISnailsScrollablePanel_OnBeforeControlAdded);
      this.OnSizeChanged += new UIControl.UIEvent(this.UISnailsScrollablePanel_OnSizeChanged);
      this.Refresh();
    }

    private void _pnlContainer_OnShow(IUIControl sender) => this.InvokeOnShow();

    private void _pnlContainer_OnHide(IUIControl sender)
    {
      this.InvokeOnHide();
      this.Visible = false;
    }

    private void UISnailsScrollablePanel_OnSizeChanged(IUIControl sender) => this.Refresh();

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      this._arrowEffect.Update(gameTime);
      if (this._arrowBeginning.Visible)
        this._arrowBeginning.DoHoover(this._arrowEffect.PositionV2);
      if (!this._arrowEnd.Visible)
        return;
      this._arrowEnd.DoHoover(this._arrowEffect.PositionV2);
    }

    private void Refresh()
    {
      this._pnlContainer.Size = this.Size;
      switch (this._panel.Orientation)
      {
        case UIScrollablePanel.PanelOrientation.Horizontal:
          if (this.WithSlider)
          {
            this._slider.Size = new Size(this.Width, 100f);
            this._slider.Position = new Vector2(0.0f, this.Height);
            this._slider.SliderMargin = this._imgPanelTop.Width / 2f;
            this._slider.MaxValue = this._panel.Width - this.Width;
          }
          this._panel.Size = new Size(this._panel.Width, this.Height);
          break;
        case UIScrollablePanel.PanelOrientation.Vertical:
          if (this.WithSlider)
          {
            this._slider.Size = new Size(100f, this.Height);
            this._slider.Position = new Vector2(this.Width, 0.0f);
            this._slider.SliderMargin = this._imgPanelTop.Height / 2f;
            this._slider.MaxValue = this._panel.Height - this.Height;
          }
          this._panel.Size = new Size(this.Width, this._panel.Height);
          break;
      }
      this._imgPanelTop.Sprite = this.Orientation != UIScrollablePanel.PanelOrientation.Vertical ? BrainGame.ResourceManager.GetSpriteTemporary("spriteset/main-menu-objects2/LogSmallV") : ((double) this.Distance <= 5000.0 ? BrainGame.ResourceManager.GetSpriteTemporary("spriteset/main-menu-objects2/LogSmall") : BrainGame.ResourceManager.GetSpriteTemporary("spriteset/main-menu-objects2/LogLong"));
      this._imgPanelBottom.Sprite = this._imgPanelTop.Sprite;
      switch (this._panel.Orientation)
      {
        case UIScrollablePanel.PanelOrientation.Horizontal:
          this._imgPanelTop.Margins.Top = 0.0f;
          this._imgPanelTop.Margins.Left = (float) (-(double) this.FromNativeResolutionX(this._imgPanelTop.Width) / 2.0);
          this._imgPanelTop.ParentAlignment = AlignModes.Vertically | AlignModes.Left;
          this._imgPanelBottom.Margins.Right = (float) (-(double) this.FromNativeResolutionX(this._imgPanelBottom.Width) / 2.0);
          this._imgPanelBottom.Margins.Bottom = 0.0f;
          this._imgPanelBottom.ParentAlignment = AlignModes.Vertically | AlignModes.Right;
          this._arrowBeginning.ParentAlignment = AlignModes.Bottom | AlignModes.Left;
          this._arrowBeginning.Margins.Clear();
          this._arrowBeginning.Margins.Left = this.FromNativeResolutionX(300f);
          this._arrowBeginning.Orientation = UIArrow.ArrowType.Left;
          this._arrowEnd.ParentAlignment = AlignModes.Right | AlignModes.Bottom;
          this._arrowEnd.Margins.Clear();
          this._arrowEnd.Margins.Right = this.FromNativeResolutionX(300f);
          this._arrowEnd.Orientation = UIArrow.ArrowType.Right;
          this._arrowEffect = new HooverEffect(1f, 3f, 90f);
          break;
        case UIScrollablePanel.PanelOrientation.Vertical:
          this._imgPanelTop.Margins.Left = 0.0f;
          this._imgPanelTop.Margins.Top = (float) (-(double) this.FromNativeResolutionY(this._imgPanelTop.Height) / 2.0);
          this._imgPanelTop.ParentAlignment = AlignModes.Horizontaly | AlignModes.Top;
          this._imgPanelBottom.Margins.Bottom = (float) (-(double) this.FromNativeResolutionY(this._imgPanelBottom.Height) / 2.0);
          this._imgPanelBottom.Margins.Right = 0.0f;
          this._imgPanelBottom.ParentAlignment = AlignModes.Horizontaly | AlignModes.Bottom;
          this._arrowBeginning.ParentAlignment = AlignModes.Right | AlignModes.Top;
          this._arrowBeginning.Margins.Clear();
          this._arrowBeginning.Margins.Top = this.FromNativeResolutionY(500f);
          this._arrowBeginning.Orientation = UIArrow.ArrowType.Up;
          this._arrowEnd.ParentAlignment = AlignModes.Right | AlignModes.Bottom;
          this._arrowEnd.Margins.Clear();
          this._arrowEnd.Margins.Bottom = this.FromNativeResolutionY(500f);
          this._arrowEnd.Orientation = UIArrow.ArrowType.Down;
          this._arrowEffect = new HooverEffect(1f, 3f, 0.0f);
          break;
      }
      this.RefreshScrollIndicators();
    }

    private bool UISnailsScrollablePanel_OnBeforeControlAdded(IUIControl sender, UIControl control)
    {
      this._panel.Controls.Add(control);
      return false;
    }

    public void Clear() => this._panel.Controls.Clear();

    public bool AtLeftBound() => this._panel.AtLeftBound();

    public bool AtRightBound() => this._panel.AtRightBound();

    public bool AtUpBound() => this._panel.AtUpBound();

    public bool AtDownBound() => this._panel.AtDownBound();

    public override void Hide() => this._pnlContainer.Hide();

    public override void Show()
    {
      this.Visible = true;
      this._pnlContainer.Show();
    }

    public void ScrollToTop() => this._panel.SetScroll(0.0f);

    private void _panel_OnScroll(IUIControl sender) => this.RefreshScrollIndicators();

    private void RefreshScrollIndicators()
    {
      this._arrowBeginning.Visible = !this._panel.AtBeginning() && !this.WithSlider && this.ShowScrollIndicators;
      this._arrowBeginning.DoHoover(this._arrowEffect.PositionV2);
      this._arrowEnd.Visible = !this._panel.AtEnd() && !this.WithSlider && this.ShowScrollIndicators;
      this._arrowEnd.DoHoover(this._arrowEffect.PositionV2);
    }

    public void Reset() => this.ScrollToTop();

    public void StopFlick()
    {
      if (this._panel == null)
        return;
      this._panel.StopFlick();
    }
  }
}
