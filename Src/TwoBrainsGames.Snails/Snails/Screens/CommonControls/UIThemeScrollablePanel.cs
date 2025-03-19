
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UIThemeScrollablePanel
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Screens.ThemeSelection;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UIThemeScrollablePanel : UISnailsScrollablePanel
  {
    private UITheme[] _themesControls;
    private Sample _selectedSample;
    private Sample _shownSample;
    private UITimer _tmrShowThemes;
    private UITimer _tmrThemeSelected;

    public event UIControl.UIEvent OnThemeSelectedStarted;

    public event UIControl.UIEvent OnThemeSelected;

    public event UIControl.UIEvent OnCancelEnded;

    public UITheme GardenTheme => this._themesControls[0];

    public UITheme AncientEgyptTheme => this._themesControls[1];

    public UITheme BotFactoryTheme => this._themesControls[2];

    public UITheme OuterSpaceTheme => this._themesControls[3];

    public UITheme SelectedTheme { get; private set; }

    public UITheme LastSelectedTheme { get; private set; }

    public override bool Visible
    {
      get => base.Visible;
      set
      {
        base.Visible = value;
        if (this._themesControls != null)
        {
          for (int index = 0; index < this._themesControls.Length; ++index)
            this._themesControls[index].Visible = value;
        }
        this.StopFlick();
      }
    }

    public UIThemeScrollablePanel(UIScreen screenOwner)
      : this(screenOwner, Vector2.Zero)
    {
    }

    public UIThemeScrollablePanel(UIScreen screenOwner, Vector2 position)
      : base(screenOwner, UIScrollablePanel.PanelOrientation.Horizontal, false, 18600f)
    {
      this._themesControls = new UITheme[4];
      this.Orientation = UIScrollablePanel.PanelOrientation.Horizontal;
      this._themesControls[0] = this.CreateThemeControl(ThemeType.ThemeA, new Vector2(0.0f, 0.0f));
      this._themesControls[0].OnShow += new UIControl.UIEvent(this.GardenTheme_OnShow);
      this._themesControls[0].OnShowBegin += new UIControl.UIEvent(this.UIThemesPanel_OnShowBegin);
      this._themesControls[1] = this.CreateThemeControl(ThemeType.ThemeB, new Vector2(4500f, 0.0f));
      this._themesControls[2] = this.CreateThemeControl(ThemeType.ThemeC, new Vector2(9000f, 0.0f));
      this._themesControls[3] = this.CreateThemeControl(ThemeType.ThemeD, new Vector2(13500f, 0.0f));
      this.Size = new Size(9500f, 5000f);
      this.SelectedTheme = (UITheme) null;
      this._tmrShowThemes = new UITimer(screenOwner, 150.0, true);
      this._tmrShowThemes.OnTimer += new UIControl.UIEvent(this._tmrShowThemes_OnTimer);
      this.Controls.Add((UIControl) this._tmrShowThemes);
      this._tmrThemeSelected = new UITimer(screenOwner, 350.0, false);
      this._tmrThemeSelected.OnTimer += new UIControl.UIEvent(this._tmrThemeSelected_OnTimer);
      this.Controls.Add((UIControl) this._tmrThemeSelected);
      this._selectedSample = BrainGame.ResourceManager.GetSampleStatic("sfx/menu-item-selected");
      this._shownSample = BrainGame.ResourceManager.GetSampleStatic("sfx/menu-item-shown");
    }

    private UITheme CreateThemeControl(ThemeType themeId, Vector2 position)
    {
      UITheme control = (UITheme) new UIThemeLD(this.ScreenOwner, themeId);
      control.ParentAlignment = AlignModes.Top;
      control.Margins.Top = 125f;
      control.Scale = new Vector2(0.8f, 0.8f);
      control.Position = position;
      control.UpdateLayout();
      control.OnAccept += new UIControl.UIEvent(this.Theme_OnAccept);
      control.OnUnselectEnded = new UIControl.UIEvent(this.Theme_OnUnselectEnded);
      this.Controls.Add((UIControl) control);
      return control;
    }

    public void Initialize()
    {
      foreach (UITheme themesControl in this._themesControls)
        themesControl.Initialize();
    }

    private void UIThemesPanel_OnShowBegin(IUIControl sender) => this._shownSample.Play();

    private void GardenTheme_OnShow(IUIControl sender) => this.InvokeOnShow();

    private void _tmrShowThemes_OnTimer(IUIControl sender)
    {
      for (int index = 0; index < this._themesControls.Length; ++index)
        this._themesControls[index].Show();
      this._tmrShowThemes.Enabled = false;
    }

    public override void Show()
    {
      this._tmrShowThemes.Enabled = true;
      this.Visible = true;
    }

    public override void Focus()
    {
      if (this.State != UIScrollablePanel.PanState.None)
        return;
      base.Focus();
      if (this.SelectedTheme == null)
        this.SelectedTheme = this._themesControls[0];
      this.SelectedTheme.Focus();
    }

    private void Theme_OnAccept(IUIControl sender)
    {
      UITheme uiTheme = (UITheme) sender;
      if (uiTheme.Locked)
        return;
      this.SelectedTheme = uiTheme;
      this.LastSelectedTheme = uiTheme;
      this.ShowHideUnselectedThemes(false);
      this._tmrThemeSelected.Reset();
      this._tmrThemeSelected.Enabled = true;
      this._selectedSample.Play();
      if (this.OnThemeSelectedStarted == null)
        return;
      this.OnThemeSelectedStarted(sender);
    }

    private void _tmrThemeSelected_OnTimer(IUIControl sender)
    {
      this._tmrThemeSelected.Enabled = false;
      if (this.OnThemeSelected == null)
        return;
      this.OnThemeSelected(sender);
    }

    private void Theme_OnUnselectEnded(IUIControl sender)
    {
      this.SelectedTheme.FocusEffectEnabled = false;
      this.SelectedTheme.Show();
      this.ShowHideUnselectedThemes(true);
      this.SelectedTheme = (UITheme) null;
      if (this.OnCancelEnded == null)
        return;
      this.OnCancelEnded((IUIControl) this);
    }

    public void CancelSelection()
    {
      if (this.SelectedTheme == null)
        return;
      this.SelectedTheme.Unselect();
    }

    public void SelectThemeWithoutAnimations(ThemeType theme)
    {
      this.SelectedTheme = this._themesControls[(int) theme];
      for (int index = 0; index < this._themesControls.Length; ++index)
        this._themesControls[index].Visible = this._themesControls[index].ThemeId == theme;
    }

    private void ShowHideUnselectedThemes(bool show)
    {
      for (int index = 0; index < this._themesControls.Length; ++index)
      {
        if (show)
          this._themesControls[index].Show();
        else
          this._themesControls[index].Hide();
      }
    }
  }
}
