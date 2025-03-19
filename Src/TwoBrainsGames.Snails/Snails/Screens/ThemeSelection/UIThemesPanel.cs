
// Type: TwoBrainsGames.Snails.Screens.ThemeSelection.UIThemesPanel
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.Snails.Screens.ThemeSelection
{
  internal class UIThemesPanel : UIControl
  {
    private UITheme[] _themesControls;
    private Sample _selectedSample;
    private Sample _shownSample;
    private UITimer _tmrShowThemes;
    private UITimer _tmrThemeSelected;

    public event UIControl.UIEvent OnThemeSelectedStarted;

    public event UIControl.UIEvent OnThemeSelected;

    public event UIControl.UIEvent OnCancelEnded;

    private UITheme TopLeftTheme => this._themesControls[0];

    private UITheme BottomRightTheme => this._themesControls[3];

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
        if (this._themesControls == null || value)
          return;
        for (int index = 0; index < this._themesControls.Length; ++index)
          this._themesControls[index].Visible = value;
      }
    }

    private bool FocusEffectEnabled { get; set; }

    public UIThemesPanel(UIScreen owner)
      : base(owner)
    {
      this.OnInitializeFromContent += new UIControl.UIEvent(this.UIThemesPanel_OnInitializeFromContent);
      this._themesControls = new UITheme[4];
      this._themesControls[0] = this.CreateThemeControl(ThemeType.ThemeA, new Vector2(0.0f, 0.0f));
      this._themesControls[0].ParentAlignment = AlignModes.Top | AlignModes.Left;
      this._themesControls[0].OnShow += new UIControl.UIEvent(this.GardenTheme_OnShow);
      this._themesControls[0].OnShowBegin += new UIControl.UIEvent(this.UIThemesPanel_OnShowBegin);
      this._themesControls[1] = this.CreateThemeControl(ThemeType.ThemeB, new Vector2(3700f, 0.0f));
      this._themesControls[1].ParentAlignment = AlignModes.Right | AlignModes.Top;
      this._themesControls[2] = this.CreateThemeControl(ThemeType.ThemeC, new Vector2(0.0f, 4600f));
      this._themesControls[2].ParentAlignment = AlignModes.Bottom | AlignModes.Left;
      this._themesControls[3] = this.CreateThemeControl(ThemeType.ThemeD, new Vector2(3700f, 4600f));
      this._themesControls[3].ParentAlignment = AlignModes.Right | AlignModes.Bottom;
      this.Size = new Size(7900f, 7500f);
      this.SelectedTheme = (UITheme) null;
      this._tmrShowThemes = new UITimer(owner, 150.0, true);
      this._tmrShowThemes.OnTimer += new UIControl.UIEvent(this._tmrShowThemes_OnTimer);
      this.Controls.Add((UIControl) this._tmrShowThemes);
      this._tmrThemeSelected = new UITimer(owner, 350.0, false);
      this._tmrThemeSelected.OnTimer += new UIControl.UIEvent(this._tmrThemeSelected_OnTimer);
      this.Controls.Add((UIControl) this._tmrThemeSelected);
      this._selectedSample = BrainGame.ResourceManager.GetSampleStatic("sfx/menu-item-selected");
      this._shownSample = BrainGame.ResourceManager.GetSampleStatic("sfx/menu-item-shown");
    }

    private void UIThemesPanel_OnInitializeFromContent(IUIControl sender)
    {
      this.FocusEffectEnabled = this.GetContentPropertyValue<bool>("withFocusEffect", this.FocusEffectEnabled);
    }

    public override void InitializeFromContent()
    {
      base.InitializeFromContent();
      this.InitializeFromContent(nameof (UIThemesPanel));
    }

    private void UIThemesPanel_OnShowBegin(IUIControl sender) => this._shownSample.Play();

    public void Initialize()
    {
      foreach (UITheme themesControl in this._themesControls)
      {
        themesControl.Initialize();
        themesControl.FocusEffectEnabled = this.FocusEffectEnabled;
      }
    }

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
      base.Focus();
      if (this.SelectedTheme == null)
        this.SelectedTheme = this._themesControls[0];
      this.SelectedTheme.Focus();
    }

    private UITheme CreateThemeControl(ThemeType themeId, Vector2 position)
    {
      UITheme control = new UITheme(this.ScreenOwner, themeId);
      control.Position = position;
      control.OnAccept += new UIControl.UIEvent(this.Theme_OnAccept);
      control.OnUnselectEnded = new UIControl.UIEvent(this.Theme_OnUnselectEnded);
      this.Controls.Add((UIControl) control);
      return control;
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
      this.SelectedTheme.SelectWithoutAnimations(this.TopLeftTheme.PositionInPixels);
    }

    private void Theme_OnAccept(IUIControl sender)
    {
      this.AcceptControllerInput = false;
      UITheme uiTheme = (UITheme) sender;
      this.SelectedTheme = uiTheme;
      this.LastSelectedTheme = uiTheme;
      this.ShowHideUnselectedThemes(false);
      this.SelectedTheme.Select(this.TopLeftTheme.PositionInPixels);
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

    private void Theme_OnMoveToEnded(IUIControl sender)
    {
      if (this.OnThemeSelected == null)
        return;
      this.OnThemeSelected(sender);
    }

    private void Theme_OnUnselectEnded(IUIControl sender)
    {
      this.SelectedTheme.FocusEffectEnabled = this.FocusEffectEnabled;
      this.SelectedTheme.Show();
      this.ShowHideUnselectedThemes(true);
      this.SelectedTheme = (UITheme) null;
      if (this.OnCancelEnded == null)
        return;
      this.OnCancelEnded((IUIControl) this);
    }

    private void ShowHideUnselectedThemes(bool show)
    {
      for (int index = 0; index < this._themesControls.Length; ++index)
      {
        if (this.SelectedTheme != this._themesControls[index])
        {
          if (show)
            this._themesControls[index].Show();
          else
            this._themesControls[index].Hide();
        }
      }
    }

    public void Refresh()
    {
      for (int index = 0; index < this._themesControls.Length; ++index)
        this._themesControls[index].Refresh();
    }
  }
}
