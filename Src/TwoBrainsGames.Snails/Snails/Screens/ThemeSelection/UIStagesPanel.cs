
// Type: TwoBrainsGames.Snails.Screens.ThemeSelection.UIStagesPanel
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Player;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.Screens.ThemeSelection
{
  internal class UIStagesPanel : UIControl
  {
    private const float STAGE_ITEM_WIDTH = 1000f;
    private const float STAGE_ITEM_HEIGHT = 1200f;
    public UIControl.UIEvent OnStageSelected;
    private UIStage[] _stageControls;
    private UITimer _timer;
    private Sample _shownSample;
    private Sample _HiddenSample;

    public event UIControl.UIEvent OnStageEnter;

    public event UIControl.UIEvent OnStageLeave;

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

    public UIStagesPanel(UIScreen owner)
      : base(owner)
    {
      this.CreateStageControls();
      this._timer = new UITimer(this.ScreenOwner, 500.0, true);
      this._timer.Enabled = false;
      this.Controls.Add((UIControl) this._timer);
      this._shownSample = BrainGame.ResourceManager.GetSampleStatic("sfx/menu-item-shown");
      this._HiddenSample = BrainGame.ResourceManager.GetSampleStatic("sfx/menu-closed");
    }

    private void CreateStageControls()
    {
      this._stageControls = new UIStage[21];
      this.Position = new Vector2(0.0f, 5000f);
      float x = 0.0f;
      float y = 0.0f;
      float width = 0.0f;
      float num = 0.0f;
      for (int index = 0; index < this._stageControls.Length; ++index)
      {
        this._stageControls[index] = new UIStage(this.ScreenOwner, index + 1);
        this._stageControls[index].Position = new Vector2(x, y);
        x += 1050f;
        if (index == 6 || index == 13)
        {
          x = 0.0f;
          y += 1200f;
        }
        if ((double) x > (double) width)
          width = x;
        if ((double) y > (double) num)
          num = y;
        this._stageControls[index].OnEnter += new UIControl.UIEvent(this.UIStage_OnEnter);
        this._stageControls[index].OnLeave += new UIControl.UIEvent(this.UIStage_OnLeave);
        this._stageControls[index].OnPress += new UIControl.UIEvent(this.UIStage_OnAccept);
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
      this._shownSample.Play();
      this.Visible = true;
      this.Reset();
      this.SetTheme(levels, themeId);
      for (int index = 0; index < this._stageControls.Length; ++index)
        this._stageControls[index].Show();
    }

    public override void Hide()
    {
      this._HiddenSample.Play();
      for (int index = 0; index < this._stageControls.Length; ++index)
        this._stageControls[index].Hide();
      base.Hide();
    }

    public void Reset()
    {
      this.RaiseStageLeaveEvent = true;
      for (int index = 0; index < this._stageControls.Length; ++index)
        this._stageControls[index].Reset();
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
      if (this.OnStageSelected == null)
        return;
      this.OnStageSelected(sender);
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
