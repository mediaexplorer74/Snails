
// Type: TwoBrainsGames.Snails.Screens.ThemeSelection.UIStagesPanelLD
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Player;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.Screens.ThemeSelection
{
  internal class UIStagesPanelLD : UISnailsScrollablePanel
  {
    private const float STAGE_ITEM_WIDTH = 1300f;
    private const float STAGE_ITEM_HEIGHT = 1800f;
    public UIControl.UIEvent OnStageSelected;
    public UIControl.UIEvent OnStageDoubleSelected;
    private UIStage[] _stageControls;
    private UITimer _timer;
    private Sample _shownSample;
    private UIArrow _arrowUp;
    private UIArrow _arrowDown;
    private HooverEffect _arrowEffect;
    protected Vector2 _arrowUpAbsPos;
    protected Vector2 _arrowDownAbsPos;
    protected bool? _atBoundUpChanged = new bool?(false);
    protected bool? _atBoundDownChanged = new bool?(false);
    private UIStage _selectedStage;

    public event UIControl.UIEvent OnStageEnter;

    public event UIControl.UIEvent OnStageLeave;

    private UIStage SelectedStage
    {
      get => this._selectedStage;
      set
      {
        if (this._selectedStage != null)
          this._selectedStage.Selected = false;
        this._selectedStage = value;
        if (this._selectedStage == null)
          return;
        this._selectedStage.Selected = true;
      }
    }

    public bool RaiseStageLeaveEvent { get; set; }

    public override bool Visible
    {
      get => base.Visible;
      set
      {
        base.Visible = value;
        if (this._stageControls == null)
          return;
        for (int index = 0; index < this._stageControls.Length; ++index)
          this._stageControls[index].Visible = value;
      }
    }

    public bool FocusEffectEnabled { get; set; }

    public override bool Enabled
    {
      get => base.Enabled;
      set => base.Enabled = value;
    }

    public UIStagesPanelLD(UIScreen owner)
      : base(owner, UIScrollablePanel.PanelOrientation.Vertical, false, 13700f)
    {
      this._timer = new UITimer(this.ScreenOwner, 500.0, true);
      this._timer.Enabled = false;
      this.Controls.Add((UIControl) this._timer);
      this._shownSample = BrainGame.ResourceManager.GetSampleStatic("sfx/menu-item-shown");
      this._arrowEffect = new HooverEffect(1f, 1.5f, 180f);
      this._arrowUpAbsPos = new Vector2(5050f, 5700f);
      this._arrowUp = new UIArrow(this.ScreenOwner, UIArrow.ArrowType.Up, UIArrow.ArrowSize.Small, this._arrowEffect);
      this._arrowUp.Position = this._arrowUpAbsPos;
      this._arrowUp.Visible = false;
      this._arrowDownAbsPos = new Vector2(5050f, 6700f);
      this._arrowDown = new UIArrow(this.ScreenOwner, UIArrow.ArrowType.Down, UIArrow.ArrowSize.Small, this._arrowEffect);
      this._arrowDown.Position = this._arrowDownAbsPos;
      this.ShowScrollIndicators = true;
      this.CreateStageControls();
    }

    public void SelectStage(LevelStage levelStage)
    {
      foreach (UIStage stageControl in this._stageControls)
      {
        if (stageControl.LevelStageInfo.StageKey == levelStage.StageKey)
          this.SelectedStage = stageControl;
      }
    }

    public void ShowArrows()
    {
      if (!this.AtUpBound())
        this._arrowUp.Show();
      if (this.AtDownBound())
        return;
      this._arrowDown.Show();
    }

    public void HideArrows()
    {
      if (!this.AtUpBound())
        this._arrowUp.Hide();
      if (this.AtDownBound())
        return;
      this._arrowDown.Hide();
    }

    public void UpdateArrows()
    {
    }

    private void CreateStageControls()
    {
      this._stageControls = new UIStage[21];
      float x = 100f;
      float y = 300f;
      float width = 0.0f;
      float num = 0.0f;
      for (int index = 0; index < this._stageControls.Length; ++index)
      {
        this._stageControls[index] = new UIStage(this.ScreenOwner, index + 1);
        this._stageControls[index].Position = new Vector2(x, y);
        x += 1330f;
        if ((index + 1) % 3 == 0)
        {
          x = 100f;
          y += 1800f;
        }
        if ((double) x > (double) width)
          width = x;
        if ((double) y > (double) num)
          num = y;
        this._stageControls[index].AcceptOnEnterEvents = false;
        this._stageControls[index].OnPress += new UIControl.UIEvent(this.UIStage_OnAccept);
        this._stageControls[index].OnDoublePress += new UIControl.UIEvent(this.UIStage_OnDoublePress);
        this.Controls.Add((UIControl) this._stageControls[index]);
      }
      this._stageControls[0].OnShow += new UIControl.UIEvent(this.FirstStageControl_OnShow);
      this._stageControls[0].OnHide += new UIControl.UIEvent(this.FirstStageControl_OnHide);
      this.Size = new Size((float) (int) width, (float) (int) num + this._stageControls[0].Size.Height);
    }

    public void SetTheme(Levels levels, ThemeType themeId)
    {
      for (int index = 0; index < levels.GetStagesCountForTheme(themeId); ++index)
      {
        this._stageControls[index].LevelStageInfo = levels.FindStageInfo(themeId, index + 1);
        this._stageControls[index].ThemeId = themeId;
        if (Game1.ProfilesManager.CurrentProfile != null)
        {
          this._stageControls[index].Locked = !Game1.ProfilesManager.CurrentProfile.PlayerStats.IsStageUnlocked(this._stageControls[index].LevelStageInfo.StageId);
          PlayerStageStats stageStats = Game1.ProfilesManager.CurrentProfile.PlayerStats.GetStageStats(this._stageControls[index].LevelStageInfo.StageId);
          this._stageControls[index].Medal = stageStats == null ? MedalType.None : stageStats.Medal;
        }
      }
    }

    public void Show(Levels levels, ThemeType themeId)
    {
      this.Enabled = true;
      this.SelectedStage = (UIStage) null;
      this._shownSample.Play();
      this.Show();
      this.ResetPanel();
      this.SetTheme(levels, themeId);
      for (int index = 0; index < this._stageControls.Length; ++index)
        this._stageControls[index].Show();
      this.ShowArrows();
    }

    public void ResetPanel()
    {
      this.RaiseStageLeaveEvent = true;
      for (int index = 0; index < this._stageControls.Length; ++index)
        this._stageControls[index].Reset();
    }

    private void EnableFocusEffect(bool enabled)
    {
      this.FocusEffectEnabled = enabled;
      for (int index = 0; index < this._stageControls.Length; ++index)
        this._stageControls[index].AcceptControllerInput = enabled;
    }

    public void OnProcessControllerEnd(IUIControl sender)
    {
      if (this.State == UIScrollablePanel.PanState.None && this.PreviousState == UIScrollablePanel.PanState.None)
        this.EnableFocusEffect(true);
      else
        this.EnableFocusEffect(false);
    }

    public void Focus(int stageNr)
    {
      if (stageNr <= 0 || stageNr > this._stageControls.Length)
        stageNr = 1;
      this._stageControls[stageNr - 1].Focus();
    }

    public void FocusOnLastUnlocked()
    {
      int lastUnlockedStage = this.GetLastUnlockedStage();
      if (lastUnlockedStage <= 0)
        return;
      this.Focus(lastUnlockedStage);
    }

    public void Cancel()
    {
      this.ScreenOwner.IgnoreControlFocus = true;
      this._timer.OnTimer += new UIControl.UIEvent(this.Timer_OnHideStageTimer);
      this._timer.Enabled = true;
      this._timer.Parameter = (object) (this._stageControls.Length - 1);
    }

    private void Timer_OnHideStageTimer(IUIControl sender)
    {
      int parameter = (int) this._timer.Parameter;
      this._stageControls[(int) this._timer.Parameter].Visible = false;
      if ((int) (this._timer.Parameter = (object) (parameter - 1)) > 0)
        return;
      this._timer.Enabled = false;
      this.Visible = false;
    }

    public int GetLastUnlockedStage()
    {
      for (int index = this._stageControls.Length - 1; index >= 0; --index)
      {
        if (!this._stageControls[index].Locked)
          return index + 1;
      }
      return 0;
    }

    private void UIStage_OnAccept(IUIControl sender)
    {
      this.SelectedStage = (UIStage) sender;
      if (this.OnStageSelected == null)
        return;
      this.OnStageSelected(sender);
    }

    private void UIStage_OnDoublePress(IUIControl sender)
    {
      if (this.OnStageDoubleSelected == null)
        return;
      this.OnStageDoubleSelected(sender);
    }

    private void FirstStageControl_OnHide(IUIControl sender)
    {
      this.Visible = false;
      this.InvokeOnHide();
    }

    private void FirstStageControl_OnShow(IUIControl sender) => this.InvokeOnShow();

    private void UIStage_OnEnter(IUIControl sender)
    {
      if (this.OnStageEnter == null)
        return;
      this.OnStageEnter(sender);
    }

    private void UIStage_OnLeave(IUIControl sender)
    {
      if (!this.RaiseStageLeaveEvent || this.OnStageLeave == null)
        return;
      this.OnStageLeave(sender);
    }
  }
}
