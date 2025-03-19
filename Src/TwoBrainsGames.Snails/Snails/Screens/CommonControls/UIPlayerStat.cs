
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UIPlayerStat
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UIPlayerStat : UIPanel
  {
    private UIValuedCaption _caption;
    private UISnailsButton _btnReset;
    private UIPlayerStat.PlayerStatType _statType;

    public event UIPlayerStat.UIPlayerStatEvent OnReset;

    public object Value
    {
      get => this._caption.Value;
      set => this._caption.Value = value;
    }

    public UIValuedCaption.CaptionMode CaptionFormat
    {
      get => this._caption.Mode;
      set => this._caption.Mode = value;
    }

    public UIPlayerStat(
      UIScreen screenOwner,
      string textResource,
      Vector2 pos,
      UIPlayerStat.PlayerStatType statType)
      : base(screenOwner)
    {
      this._caption = new UIValuedCaption(screenOwner, textResource, (object) 0, Color.LightBlue, Color.White, UICaption.CaptionStyle.PlayerStats, 5000f, false);
      this._caption.ParentAlignment = AlignModes.Vertically | AlignModes.Left;
      this._caption.Margins.Left = 200f;
      this._caption.AnimateValue = false;
      this.Controls.Add((UIControl) this._caption);
      this._btnReset = new UISnailsButton(screenOwner, "BTN_RESET", UISnailsButton.ButtonSizeType.Small, InputBase.InputActions.None, new UIControl.UIEvent(this.btnReset_OnPress), false);
      this._btnReset.ParentAlignment = AlignModes.Vertically | AlignModes.Right;
      this._btnReset.Scale = this.FromNativeResolution(this._btnReset.Scale);
      this.Controls.Add((UIControl) this._btnReset);
      this._statType = statType;
      this.OnScreenStart += new UIControl.UIEvent(this.UIPlayerStat_OnScreenStart);
      this.Position = pos;
      this.BackgroundColor = new Color(0, 0, 0, 50);
    }

    private void UIPlayerStat_OnScreenStart(IUIControl sender)
    {
      this.Size = new Size(this.Parent.Size.Width - 600f, 900f);
    }

    private void btnReset_OnPress(IUIControl sender)
    {
      if (this.OnReset == null)
        return;
      this.OnReset((IUIControl) this, this._statType);
    }

    public enum PlayerStatType
    {
      PlayingTime,
      RunningTime,
      TotalSnailsSafe,
      TotalSnailsKingSafe,
      TotalSnailsDeadByFire,
      TotalSnailsDeadBySpikes,
      TotalSnailsDeadByDynamite,
      TotalSnailsDeadByCrate,
      TotalSnailsDeadByWater,
      TotalSnailsDeadByLaser,
      TotalSnailsDeadBySacrifice,
      TotalGoldMedals,
      TotalSilverMedals,
      TotalBronzeMedals,
      TotalBoots,
      TotalSnailsDeadByCrateExplosion,
      TotalSnailsDeadByAcid,
      TotalSnailsDeadByOutOfStage,
      TotalSnailsDeadByEvilSnail,
      SnailsDeadInDifferentWays,
    }

    public delegate void UIPlayerStatEvent(IUIControl sender, UIPlayerStat.PlayerStatType stat);
  }
}
