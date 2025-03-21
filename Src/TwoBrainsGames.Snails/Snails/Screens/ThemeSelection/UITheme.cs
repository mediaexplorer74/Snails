
// Type: TwoBrainsGames.Snails.Screens.ThemeSelection.UITheme
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Effects;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.Screens.ThemeSelection
{
  public class UITheme : UIControl
  {
    protected static Vector2 DEFAULT_SCALE = new Vector2(0.85f, 0.85f);
    public UIControl.UIEvent OnMoveToEnded;
    public UIControl.UIEvent OnUnselectEnded;
    protected UIImage _imgBoard;
    protected UILocker _lockerImage;
    protected UIImage _imgSmallLocker;
    protected UIImage _imgMedal;
    protected UISnailsThemeIcon _imgTheme;
    protected bool _locked;
    protected Vector2 _originalPosition;
    protected AlignModes _saveAlignment;
    protected UIPanel _pnlContainer;
    protected UICaption _lblToUnlock;
    protected UICaption _lblLockedInTrial;
    protected UICaption _lblGardenNeeded;
    protected UICaption _lblEgyptNeeded;
    protected UICaption _lblFactoryNeeded;
    protected UICaption _lblStagesUnlocked;
    protected UICaption _lblGoldMedalsEarned;
    protected Sample _focusSound;

    public ThemeType ThemeId { get; private set; }

    public bool FocusEffectEnabled { get; set; }

    public override BoundingSquare BoundingBox
    {
      get
      {
        return new BoundingSquare(this._pnlContainer.AbsolutePositionInPixels + new Vector2(this._imgBoard.Sprite.BoundingBoxes[0].Left, this._imgBoard.Sprite.BoundingBoxes[0].Top), this._imgBoard.Sprite.BoundingBoxes[0].Width * this._pnlContainer.Scale.X, this._imgBoard.Sprite.BoundingBoxes[0].Height * this._pnlContainer.Scale.Y);
      }
    }

    public bool LockedInDemo { get; set; }

    public bool Locked
    {
      get => this._locked;
      private set => this._locked = value;
    }

    public override bool Visible
    {
      get => base.Visible;
      set
      {
        base.Visible = value;
        if (this._pnlContainer == null)
          return;
        this._pnlContainer.Visible = value;
        this._pnlContainer.Scale = UITheme.DEFAULT_SCALE;
      }
    }

    private bool WithThemeSelectionAnimations { get; set; }

    public UITheme(UIScreen screenOwner, ThemeType themeId)
      : base(screenOwner)
    {
      this.ThemeId = themeId;
      this.Name = "theme_" + this.ThemeId.ToString();
      this.OnMoveToEnded = (UIControl.UIEvent) null;
      this.OnEnter += new UIControl.UIEvent(this.Theme_OnEnter);
      this.OnLeave += new UIControl.UIEvent(this.Theme_OnLeave);
      this.OnInitializeFromContent += new UIControl.UIEvent(this.UITheme_OnInitializeFromContent);
      this._pnlContainer = new UIPanel(screenOwner);
      this._pnlContainer.Scale = UITheme.DEFAULT_SCALE;
      this._pnlContainer.ParentAlignment = AlignModes.HorizontalyVertically;
      this._pnlContainer.ShowEffect = (TransformEffectBase) new SquashEffect(0.7f, 3.2f, 0.08f, this.BlendColor, this._pnlContainer.Scale);
      this._pnlContainer.HideEffect = (TransformEffectBase) new PopOutEffect(new Vector2(1.2f, 1.2f), 6f);
      this._pnlContainer.OnAccept += new UIControl.UIEvent(this._pnlContainer_OnAccept);
      this._pnlContainer.OnShow += new UIControl.UIEvent(this._pnlContainer_OnShow);
      this.Controls.Add((UIControl) this._pnlContainer);
      this._imgBoard = new UIImage(screenOwner, "spriteset/boards/DarkWoodMedium");
      this._imgBoard.Name = "icon_" + this.ThemeId.ToString();
      this._imgBoard.ParentAlignment = AlignModes.HorizontalyVertically;
      this._pnlContainer.Controls.Add((UIControl) this._imgBoard);
      this._imgTheme = new UISnailsThemeIcon(screenOwner);
      this._imgTheme.Name = "title_" + this.ThemeId.ToString();
      this._imgTheme.Position = new Vector2(300f, 450f);
      this._pnlContainer.Controls.Add((UIControl) this._imgTheme);
      this._imgSmallLocker = new UIImage(screenOwner, "spriteset/common-elements-1/LockerSmallOpen", "__STATIC__");
      this._imgSmallLocker.Position = new Vector2(2600f, 600f);
      this._pnlContainer.Controls.Add((UIControl) this._imgSmallLocker);
      this._lblStagesUnlocked = new UICaption(screenOwner, "", Colors.ThemeStageStats, UICaption.CaptionStyle.ThemeStats);
      this._lblStagesUnlocked.Position = this._imgSmallLocker.Position + new Vector2(600f, 300f);
      this._pnlContainer.Controls.Add((UIControl) this._lblStagesUnlocked);
      this._imgMedal = new UIImage(screenOwner, "spriteset/menu-elements-1/GoldMedal", "__TEMPORARY__");
      this._imgMedal.Position = new Vector2(2650f, 1750f);
      this._pnlContainer.Controls.Add((UIControl) this._imgMedal);
      this._lblGoldMedalsEarned = new UICaption(screenOwner, "", Colors.ThemeStageStats, UICaption.CaptionStyle.ThemeStats);
      this._lblGoldMedalsEarned.Position = this._imgMedal.Position + new Vector2(550f, 300f);
      this._pnlContainer.Controls.Add((UIControl) this._lblGoldMedalsEarned);
      this._lockerImage = new UILocker(screenOwner);
      this._lockerImage.Position = new Vector2(1950f, 780f);
      this._pnlContainer.Controls.Add((UIControl) this._lockerImage);
      this._lblToUnlock = new UICaption(screenOwner, "", Colors.ThemeSelectionNeeded, UICaption.CaptionStyle.ThemeUnlockInfoTitle);
      this._lblToUnlock.ParentAlignment = AlignModes.Horizontaly;
      this._lblToUnlock.TextResourceId = "LBL_NEEDED_UNLOCK";
      this._lblToUnlock.Position = new Vector2(0.0f, 1630f);
      this._lblToUnlock.BlendColorWithParent = false;
      this._pnlContainer.Controls.Add((UIControl) this._lblToUnlock);
      this._lblLockedInTrial = new UICaption(screenOwner, "", Colors.ThemeSelectionNeeded, UICaption.CaptionStyle.ThemeUnlockInfoTitle);
      this._lblLockedInTrial.ParentAlignment = AlignModes.Horizontaly;
      this._lblLockedInTrial.TextResourceId = "LBL_LOCKED_IN_TRIAL";
      this._lblLockedInTrial.Position = this.NativeResolution(new Vector2(0.0f, 1000f));
      this._lblLockedInTrial.BlendColorWithParent = false;
      this._pnlContainer.Controls.Add((UIControl) this._lblLockedInTrial);
      this._lblGardenNeeded = new UICaption(screenOwner, "", Colors.ThemeSelectionNeededFromThemes, UICaption.CaptionStyle.ThemeUnlockInfo);
      this._lblGardenNeeded.ParentAlignment = AlignModes.Horizontaly;
      this._lblGardenNeeded.Position = this._lblToUnlock.Position + new Vector2(0.0f, 420f);
      this._lblGardenNeeded.BlendColorWithParent = false;
      this._pnlContainer.Controls.Add((UIControl) this._lblGardenNeeded);
      this._lblEgyptNeeded = new UICaption(screenOwner, "", Colors.ThemeSelectionNeededFromThemes, UICaption.CaptionStyle.ThemeUnlockInfo);
      this._lblEgyptNeeded.ParentAlignment = AlignModes.Horizontaly;
      this._lblEgyptNeeded.Position = this._lblGardenNeeded.Position + new Vector2(0.0f, 350f);
      this._lblEgyptNeeded.BlendColorWithParent = false;
      this._pnlContainer.Controls.Add((UIControl) this._lblEgyptNeeded);
      this._lblFactoryNeeded = new UICaption(screenOwner, "", Colors.ThemeSelectionNeededFromThemes, UICaption.CaptionStyle.ThemeUnlockInfo);
      this._lblFactoryNeeded.ParentAlignment = AlignModes.Horizontaly;
      this._lblFactoryNeeded.Position = this._lblEgyptNeeded.Position + new Vector2(0.0f, 350f);
      this._lblFactoryNeeded.BlendColorWithParent = false;
      this._pnlContainer.Controls.Add((UIControl) this._lblFactoryNeeded);
      this._imgTheme.Theme = this.ThemeId;
      this._pnlContainer.Size = this._imgBoard.Size;
      this.Size = this._imgBoard.Size;
      this._focusSound = BrainGame.ResourceManager.GetSampleStatic("sfx/menu-item-focus");
      this.AcceptControllerInput = true;
    }

    public override void InitializeFromContent()
    {
      base.InitializeFromContent();
      this.InitializeFromContent(nameof (UITheme));
    }

    private void UITheme_OnInitializeFromContent(IUIControl sender)
    {
      this.FocusEffectEnabled = this.GetContentPropertyValue<bool>("withFocusEffect", this.FocusEffectEnabled);
      this.WithThemeSelectionAnimations = this.GetContentPropertyValue<bool>("withThemeSelectionAnimations", this.FocusEffectEnabled);
    }

    public void Initialize() => this.Refresh();

    private string FormatStagesNeededText(ThemeType theme, int stagesNeeded)
    {
      return Game1.ProfilesManager.CurrentProfile.PlayerStats.IsThemeUnlocked(theme) ? string.Format(LanguageManager.GetString("LBL_STAGES_NEEDED_UNLOCK_THEME"), (object) Formater.GetThemeName(theme), (object) stagesNeeded) : string.Format(LanguageManager.GetString("LBL_STAGES_NEEDED_UNLOCK_THEME"), (object) LanguageManager.GetString("LBL_LOCKED_THEME"), (object) stagesNeeded);
    }

    public void UpdateUnlockGoal()
    {
      int unlockTheme1 = Game1.ProfilesManager.CurrentProfile.PlayerStats.StagesNeededToUnlockTheme(this.ThemeId, ThemeType.ThemeA);
      this._lblGardenNeeded.Text = this.FormatStagesNeededText(ThemeType.ThemeA, unlockTheme1);
      this._lblGardenNeeded.Visible = unlockTheme1 > 0 && !this.LockedInDemo;
      int unlockTheme2 = Game1.ProfilesManager.CurrentProfile.PlayerStats.StagesNeededToUnlockTheme(this.ThemeId, ThemeType.ThemeB);
      this._lblEgyptNeeded.Text = this.FormatStagesNeededText(ThemeType.ThemeB, unlockTheme2);
      this._lblEgyptNeeded.Visible = unlockTheme2 > 0 && !this.LockedInDemo;
      int unlockTheme3 = Game1.ProfilesManager.CurrentProfile.PlayerStats.StagesNeededToUnlockTheme(this.ThemeId, ThemeType.ThemeC);
      this._lblFactoryNeeded.Text = this.FormatStagesNeededText(ThemeType.ThemeC, unlockTheme3);
      this._lblFactoryNeeded.Visible = unlockTheme3 > 0 && !this.LockedInDemo;
    }

    private void _pnlContainer_OnShow(IUIControl sender) => this.InvokeOnShow();

    private void _pnlContainer_OnAccept(IUIControl sender)
    {
      if (this.Locked)
        return;
      this.InvokeOnAccept();
    }

    public override void Show()
    {
      this._pnlContainer.Visible = true;
      base.Show();
    }

    public void Select(Vector2 position)
    {
      if (this.Locked)
        return;
      this.MoveTo(position);
    }

    public void SelectWithoutAnimations(Vector2 position)
    {
      this._pnlContainer.Scale = new Vector2(1f, 1f);
      this._originalPosition = this.PositionInPixels;
      this.Position = position;
      this._saveAlignment = this.ParentAlignment;
      this.ParentAlignment = AlignModes.None;
      this.FocusEffectEnabled = false;
    }

    private void MoveTo(Vector2 position)
    {
      this.FocusEffectEnabled = false;
      this._saveAlignment = this.ParentAlignment;
      this.ParentAlignment = AlignModes.None;
      this._originalPosition = this.PositionInPixels;
      PathFollowEffect moveEffect = this.CreateMoveEffect(position);
      moveEffect.OnEnd = new TransformEffectBase.OnEndEvent(this.MoveToEffect_OnEnd);
      this.EffectsBlender.Add((ITransformEffect) moveEffect, 1);
    }

    public void Unselect()
    {
      if (this.WithThemeSelectionAnimations)
      {
        PathFollowEffect moveEffect = this.CreateMoveEffect(this._originalPosition);
        moveEffect.OnEnd = new TransformEffectBase.OnEndEvent(this.UnselectMoveToEffect_OnEnd);
        this.EffectsBlender.Add((ITransformEffect) moveEffect, 1);
      }
      else
        this.UnselectMoveToEffect_OnEnd((object) null);
    }

    private PathFollowEffect CreateMoveEffect(Vector2 destination)
    {
      float speed = (this.PositionInPixels - destination).Length() / 20f;
      return new PathFollowEffect(this.PositionInPixels, destination, speed, false);
    }

    private void Theme_OnEnter(IUIControl sender)
    {
      if (!this.FocusEffectEnabled || this.Locked || (double) this._pnlContainer.Scale.X == 1.0 && (double) this._pnlContainer.Scale.Y == 1.0)
        return;
      this._focusSound.Play();
      this._pnlContainer.Effect = (ITransformEffect) new ScaleEffect(UITheme.DEFAULT_SCALE, 2f, new Vector2(1f, 1f), false);
    }

    private void Theme_OnLeave(IUIControl sender)
    {
      if (!this.FocusEffectEnabled)
        return;
      this._pnlContainer.Effect = (ITransformEffect) null;
      this._pnlContainer.Scale = UITheme.DEFAULT_SCALE;
    }

    public void ClearFocusEffect()
    {
      if (!this.FocusEffectEnabled)
        return;
      this._pnlContainer.Effect = (ITransformEffect) null;
      this._pnlContainer.Scale = UITheme.DEFAULT_SCALE;
    }

    private void MoveToEffect_OnEnd(object param)
    {
      this.EffectsBlender.Clear();
      if (this.OnMoveToEnded == null)
        return;
      this.OnMoveToEnded((IUIControl) this);
    }

    private void UnselectMoveToEffect_OnEnd(object param)
    {
      this.EffectsBlender.Clear();
      this.Enabled = true;
      this.ParentAlignment = this._saveAlignment;
      if (this.OnUnselectEnded == null)
        return;
      this.OnUnselectEnded((IUIControl) this);
    }

    public void Refresh()
    {
      this.Locked = !Game1.ProfilesManager.CurrentProfile.PlayerStats.IsThemeUnlocked(this.ThemeId);
      this.LockedInDemo = Levels._instance.IsLockedInDemo(this.ThemeId)/* && BrainGame.IsTrial*/;
      this._lblStagesUnlocked.Text = string.Format("{0}/{1}", (object) Game1.ProfilesManager.CurrentProfile.PlayerStats.GetUnlockedStagesForTheme(this.ThemeId), (object) 21);
      this._lblGoldMedalsEarned.Text = string.Format("{0}/{1}", (object) Game1.ProfilesManager.CurrentProfile.PlayerStats.GetTotalMedalsForTheme(MedalType.Gold, this.ThemeId), (object) 21);
      this._lblToUnlock.Visible = this.Locked && !this.LockedInDemo;
      this._lblLockedInTrial.Visible = this.Locked && this.LockedInDemo;
      this.UpdateUnlockGoal();
      this.BlendColor = this.Locked ? Color.Black : Color.White;
      this._lockerImage.Visible = this._locked;
      this.Enabled = !this._locked;
      if (!this.Locked)
        return;
      this._lockerImage.LockerType = this.LockedInDemo ? UILocker.LockerImageType.Demo : UILocker.LockerImageType.Normal;
      this._lockerImage.Position = this.NativeResolution(new Vector2(1950f, 780f));
      if (!this.LockedInDemo)
        return;
      this._lblFactoryNeeded.Visible = false;
      this._lblEgyptNeeded.Visible = false;
      this._lblGardenNeeded.Visible = false;
      this._lockerImage.Position = this.NativeResolution(new Vector2(1950f, 2000f));
    }
  }
}
