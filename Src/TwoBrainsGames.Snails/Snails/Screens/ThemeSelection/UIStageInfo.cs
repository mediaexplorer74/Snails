
// Type: TwoBrainsGames.Snails.Screens.ThemeSelection.UIStageInfo
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
using TwoBrainsGames.Snails.Player;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.Screens.ThemeSelection
{
  internal class UIStageInfo : UIControl
  {
    private UICaption _capModeName;
    private UIValuedCaption _capGoal;
    private UIValuedCaption _capSnailsReleased;
    private UIImage _imgBackground;
    private UISnailsStageGoalIcon _goalIcon;
    private UISnailsMedal _medal;
    private UIPanel _container;
    private UICaption _capTap;
    private UICaption _capGoldMedal;
    private UICaption _capGoldMedalTime;
    private UICaption _capGoldMedalScore;
    private UICaption _capYourBest;
    private UICaption _capYourBestTime;
    private UICaption _capYourBestScore;
    private UIImage _imgLevelThumbnail;

    public event UIControl.UIEvent OnInfoFilled;

    public event UIControl.UIEvent OnInfoCleared;

    public LevelStage StageInfo { get; private set; }

    private float LineSpacing { get; set; }

    private bool WithShowHideEffects { get; set; }

    public UIStageInfo(UIScreen screenOwner)
      : base(screenOwner)
    {
      this._container = new UIPanel(screenOwner);
      this._container.OnHide += new UIControl.UIEvent(this._container_OnHide);
      this.Controls.Add((UIControl) this._container);
      this._imgBackground = new UIImage(screenOwner, "spriteset/menu-elements-1/Notebook", "__TEMPORARY__");
      this.Size = this._imgBackground.Size;
      this._container.Size = this.Size;
      this._container.Controls.Add((UIControl) this._imgBackground);
      this._goalIcon = new UISnailsStageGoalIcon(screenOwner);
      this._goalIcon.Position = new Vector2(-100f, -100f);
      this._container.Controls.Add((UIControl) this._goalIcon);
      this._capModeName = new UICaption(screenOwner, "", Colors.StageSelectionNotebookStageMode, UICaption.CaptionStyle.StageInfoHeader);
      this._container.Controls.Add((UIControl) this._capModeName);
      this._capGoal = new UIValuedCaption(screenOwner, "LBL_STAGE_INFO_GOAL", (object) "", Color.Black, Colors.StageSelectionNotebookText, UICaption.CaptionStyle.StageInfoDetail, this.Size.Width - this.NativeResolutionX(500f), false);
      this._capGoal.AnimateValue = false;
      this._container.Controls.Add((UIControl) this._capGoal);
      this._capSnailsReleased = new UIValuedCaption(screenOwner, "LBL_STAGE_INFO_SNAIL_COUNT", (object) 0, Color.Black, Colors.StageSelectionNotebookText, UICaption.CaptionStyle.StageInfoDetail, this._capGoal.Size.Width, false);
      this._capSnailsReleased.AnimateValue = false;
      this._container.Controls.Add((UIControl) this._capSnailsReleased);
      this._medal = new UISnailsMedal(screenOwner, MedalType.None);
      this._medal.Position = new Vector2(this.Size.Width - 150f, 300f);
      this._medal.MedalSize = UISnailsMedal.MedalSizeType.Small;
      this._medal.IgnoreShowEffect = true;
      this._container.Controls.Add((UIControl) this._medal);
      this._capTap = new UICaption(screenOwner, "", Color.OrangeRed, UICaption.CaptionStyle.StageInfoDetail);
      this._capTap.TextResourceId = "LBL_STAGE_INFO_TAP";
      this._capTap.HorizontalAligment = HorizontalTextAligment.Center;
      this._capTap.ParentAlignment = AlignModes.Horizontaly | AlignModes.Top;
      this._capTap.Margins.Top = 2500f;
      if (BrainGame.Settings.UseTouch)
        this._container.Controls.Add((UIControl) this._capTap);
      this._capGoldMedal = new UICaption(screenOwner, "", Color.Black, UICaption.CaptionStyle.StageInfoDetail);
      this._capGoldMedal.TextResourceId = "LBL_STAGE_INFO_GOLD_MEDAL";
      this._container.Controls.Add((UIControl) this._capGoldMedal);
      this._capGoldMedalScore = new UICaption(screenOwner, "", Colors.StageSelectionNotebookHighScoreText, UICaption.CaptionStyle.StageInfoDetail);
      this._container.Controls.Add((UIControl) this._capGoldMedalScore);
      this._capGoldMedalTime = new UICaption(screenOwner, "", Colors.StageSelectionNotebookHighScoreText, UICaption.CaptionStyle.StageInfoDetail);
      this._container.Controls.Add((UIControl) this._capGoldMedalTime);
      this._capYourBest = new UICaption(screenOwner, "", Color.Black, UICaption.CaptionStyle.StageInfoDetail);
      this._capYourBest.TextResourceId = "LBL_STAGE_INFO_YOUR_BEST";
      this._container.Controls.Add((UIControl) this._capYourBest);
      this._capYourBestScore = new UICaption(screenOwner, "", Colors.StageSelectionNotebookHighScoreText, UICaption.CaptionStyle.StageInfoDetail);
      this._container.Controls.Add((UIControl) this._capYourBestScore);
      this._capYourBestTime = new UICaption(screenOwner, "", Colors.StageSelectionNotebookHighScoreText, UICaption.CaptionStyle.StageInfoDetail);
      this._container.Controls.Add((UIControl) this._capYourBestTime);
      this._imgLevelThumbnail = new UIImage(screenOwner);
      this._imgLevelThumbnail.Position = this.NativeResolution(new Vector2(1500f, 1650f));
      this._imgLevelThumbnail.Animate = false;
      this._container.Controls.Add((UIControl) this._imgLevelThumbnail);
      this.OnInitializeFromContent += new UIControl.UIEvent(this.UIStageInfo_OnInitializeFromContent);
      this.OnAfterInitializeFromContent += new UIControl.UIEvent(this.UIStageInfo_OnAfterInitializeFromContent);
      this.OnLanguageChanged += new UIControl.UIEvent(this.UIStageInfo_OnLanguageChanged);
    }

    private void _container_OnHide(IUIControl sender) => this.Visible = false;

    private void UIStageInfo_OnInitializeFromContent(IUIControl sender)
    {
      this._capGoal.Position = this.GetContentPropertyValue<Vector2>("goalPosition", this._capGoal.Position);
      this.LineSpacing = this.GetContentPropertyValue<float>("lineSpacing", this.LineSpacing);
      this.WithShowHideEffects = this.GetContentPropertyValue<bool>("withShowHideEffects", this.WithShowHideEffects);
      this._capModeName.Position = this.GetContentPropertyValue<Vector2>("modeNamePosition", this._capModeName.Position);
    }

    private void UIStageInfo_OnAfterInitializeFromContent(IUIControl sender)
    {
      this._capSnailsReleased.Position = this._capGoal.Position + new Vector2(0.0f, this.LineSpacing);
      if (this.WithShowHideEffects)
      {
        this._container.ParentAlignment = AlignModes.HorizontalyVertically;
        this._container.ShowEffect = (TransformEffectBase) new SquashEffect(0.85f, 4f, 0.03f, this.BlendColor, this.Scale);
        this._container.HideEffect = (TransformEffectBase) new PopOutEffect(new Vector2(1.2f, 1.2f), 6f);
      }
      this.SetLabelsPositions();
    }

    private void SetLabelsPositions()
    {
      this._capModeName.Position = new Vector2(600f, 300f);
      this.LineSpacing = 300f;
      this._capGoal.Position = new Vector2(250f, 850f);
      this._capSnailsReleased.Position = this._capGoal.Position + new Vector2(0.0f, this.LineSpacing);
      this._capGoldMedal.Position = this._capSnailsReleased.Position + new Vector2(0.0f, this.LineSpacing + 150f);
      this._capGoldMedalScore.Position = this._capGoldMedal.Position + new Vector2(150f, this.LineSpacing);
      this._capGoldMedalTime.Position = this._capGoldMedalScore.Position + new Vector2(0.0f, this.LineSpacing);
      this._capYourBest.Position = this._capGoldMedalTime.Position + new Vector2(-150f, this.LineSpacing);
      this._capYourBestScore.Position = this._capYourBest.Position + new Vector2(150f, this.LineSpacing);
      this._capYourBestTime.Position = this._capYourBestScore.Position + new Vector2(0.0f, this.LineSpacing);
      this._capModeName.Position = this.NativeResolution(this._capModeName.Position);
      this._capGoal.Position = this.NativeResolution(this._capGoal.Position);
      this._capSnailsReleased.Position = this.NativeResolution(this._capSnailsReleased.Position);
      this._capGoldMedal.Position = this.NativeResolution(this._capGoldMedal.Position);
      this._capGoldMedalScore.Position = this.NativeResolution(this._capGoldMedalScore.Position);
      this._capGoldMedalTime.Position = this.NativeResolution(this._capGoldMedalTime.Position);
      this._capYourBest.Position = this.NativeResolution(this._capYourBest.Position);
      this._capYourBestScore.Position = this.NativeResolution(this._capYourBestScore.Position);
      this._capYourBestTime.Position = this.NativeResolution(this._capYourBestTime.Position);
    }

    public override void InitializeFromContent()
    {
      base.InitializeFromContent();
      this.InitializeFromContent(nameof (UIStageInfo));
    }

    public void Initialize(LevelStage levelStage)
    {
      this.ShowHideTapMessage(false);
      this.StageInfo = levelStage;
      this.UpdateCaptionValues();
      if (this.OnInfoFilled == null)
        return;
      this.OnInfoFilled((IUIControl) this);
    }

    private void UIStageInfo_OnLanguageChanged(IUIControl sender) => this.UpdateCaptionValues();

    private void UpdateCaptionValues()
    {
      if (this.StageInfo == null)
        return;
      this._goalIcon.Goal = this.StageInfo._goal;
      this._capModeName.Text = this.StageInfo.StageNr.ToString() + " - " + Formater.FormatModeName(this.StageInfo._goal);
      this._capGoal.Value = (object) Formater.FormatGoalDescription(this.StageInfo);
      this._capSnailsReleased.Value = (object) this.StageInfo._snailsToRelease;
      this._capGoldMedalScore.Text = Formater.FormatLevelScore(this.StageInfo._goldMedalScore);
      this._capGoldMedalTime.Text = Formater.FormatLevelTime(this.StageInfo._goldMedalTime);
      this._capYourBestScore.Text = "-";
      this._capYourBestTime.Text = "-";
      this._medal.Visible = false;
      if (Game1.ProfilesManager.CurrentProfile != null)
      {
        PlayerStageStats stageStats = Game1.ProfilesManager.CurrentProfile.PlayerStats.GetStageStats(this.StageInfo.StageId);
        if (stageStats != null && stageStats.WasPlayed)
        {
          this._capYourBestScore.Text = Formater.FormatLevelScore(stageStats.Highscore);
          this._capYourBestTime.Text = Formater.FormatLevelTime(stageStats.CompletionTime);
          if (stageStats.Medal != MedalType.None)
          {
            this._medal.Visible = true;
            this._medal.Face = stageStats.Medal;
          }
        }
      }
      this._imgLevelThumbnail.Sprite = BrainGame.ResourceManager.GetSprite("spriteset/" + this.StageInfo.ThemeId.ToString() + "/thumbnails/" + this.StageInfo.StageKey, "STAGE_THUMBNAILS");
      this._imgLevelThumbnail.Visible = this._imgLevelThumbnail.FrameCount > 0;
    }

    public void ShowHideTapMessage(bool enable)
    {
      if (!BrainGame.Settings.UseTouch)
        return;
      this._capTap.Visible = enable;
      this._goalIcon.Visible = !enable;
      this._capModeName.Visible = !enable;
      this._capGoal.Visible = !enable;
      this._capSnailsReleased.Visible = !enable;
      this._medal.Visible = !enable;
      this._capGoldMedal.Visible = !enable;
      this._capGoldMedalScore.Visible = !enable;
      this._capGoldMedalTime.Visible = !enable;
      this._capYourBest.Visible = !enable;
      this._capYourBestScore.Visible = !enable;
      this._capYourBestTime.Visible = !enable;
      this._imgLevelThumbnail.Visible = !enable;
      if (this.OnInfoCleared == null)
        return;
      this.OnInfoCleared((IUIControl) this);
    }

    public override void Show()
    {
      this.Visible = true;
      this._container.Visible = true;
      base.Show();
      this.ShowHideTapMessage(true);
    }
  }
}
