
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UIGoldMedalInfoPanel
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UIGoldMedalInfoPanel : UIControl
  {
    private Color _labelColor = new Color(60, 240, 240);
    private UIPanel _pnlContainer;
    private UIImage _imgInfo;
    private UIValuedCaption _capLevel;
    private UIValuedCaption _capGold;

    public UIGoldMedalInfoPanel(UIScreen screenOwner)
      : base(screenOwner)
    {
      this._pnlContainer = new UIPanel(screenOwner);
      this._pnlContainer.Size = this.NativeResolution(new Size(3800f, 600f));
      this.Controls.Add((UIControl) this._pnlContainer);
      this._imgInfo = new UIImage(screenOwner, "spriteset/common-elements-1/InformationSign", "__STATIC__");
      this._imgInfo.Position = this.NativeResolution(new Vector2(0.0f, 300f));
      this._pnlContainer.Controls.Add((UIControl) this._imgInfo);
      this._capLevel = new UIValuedCaption(this.ScreenOwner, "LBL_LEVEL", (object) 0, this._labelColor, Color.White, UICaption.CaptionStyle.GoldMedalInfo, 900f, false);
      this._capLevel.Position = this.NativeResolution(new Vector2(300f, 100f));
      this._capLevel.CaptionAlignment = UIValuedCaption.ValueAlignmentMode.Left;
      this._capLevel.CaptionSpacing = 50f;
      this._capLevel.AnimateValue = false;
      this._pnlContainer.Controls.Add((UIControl) this._capLevel);
      this._capGold = new UIValuedCaption(this.ScreenOwner, "LBL_GOLD_MEDAL_SCORE", (object) 0, this._labelColor, Color.White, UICaption.CaptionStyle.GoldMedalInfo, 2300f, false);
      this._capGold.Position = this.NativeResolution(new Vector2(1100f, 100f));
      this._capGold.CaptionAlignment = UIValuedCaption.ValueAlignmentMode.Left;
      this._capGold.CaptionSpacing = 50f;
      this._capGold.AnimateValue = false;
      this._pnlContainer.Controls.Add((UIControl) this._capGold);
      this.OnScreenStart += new UIControl.UIEvent(this.UIGoldMedalInfoPanel_OnScreenStart);
      this.Size = this._pnlContainer.Size;
      this.BackgroundColor = new Color(0, 0, 0, 160);
      this.ShowEffect = (TransformEffectBase) new ColorEffect(Color.Transparent, Color.White, 0.1f, false);
      this.HideEffect = (TransformEffectBase) new ColorEffect(Color.White, Color.Transparent, 0.1f, false);
    }

    private void UIGoldMedalInfoPanel_OnScreenStart(IUIControl sender)
    {
      this._capLevel.Value = (object) Stage.CurrentStage.LevelStage.StageNr;
      this._capGold.Value = (object) string.Format("{0}, {1}", (object) Formater.FormatLevelTime(Stage.CurrentStage.LevelStage._goldMedalTime), (object) Formater.FormatLevelScore(Stage.CurrentStage.LevelStage._goldMedalScore));
    }
  }
}
