
// Type: TwoBrainsGames.Snails.Screens.ThemeSelection.UIStage
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Effects;
using TwoBrainsGames.Snails.Player;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.Screens.ThemeSelection
{
  internal class UIStage : UIControl
  {
    private const string IMG_LOCKER_RESOURCE = "spriteset/common-elements-1/LockerSmall";
    private static Vector2 DEFAULT_SCALE = new Vector2(0.8f, 0.8f);
    private bool _locked;
    private ThemeType _themeId;
    private UIButton _btnStage;
    private UIImage _imgBackground;
    private UIImage _imgLocker;
    private UIImage _imgLockerDemo;
    private UISnailsStageGoalIcon _goalIcon;
    private UICaption _capStageNr;
    private UIPanel _pnlContainer;
    private UISnailsMedal _medal;
    private Sample _focusSample;
    private Sample _selectedSample;
    private LevelStage _levelStageInfo;
    private bool _selected;
    private ColorEffect _selectedEffect;
    public bool AcceptOnEnterEvents = true;

    public event UIControl.UIEvent OnPress;

    public event UIControl.UIEvent OnDoublePress;

    public int StageNr { get; private set; }

    public LevelStage LevelStageInfo
    {
      get => this._levelStageInfo;
      set
      {
        this._levelStageInfo = value;
        this.Refresh();
      }
    }

    public override BoundingSquare BoundingBox
    {
      get
      {
        return new BoundingSquare(this._pnlContainer.AbsolutePositionInPixels + new Vector2(this._imgBackground.Sprite.BoundingBoxes[0].Left, this._imgBackground.Sprite.BoundingBoxes[0].Top), this._imgBackground.Sprite.BoundingBoxes[0].Width * this._pnlContainer.Scale.X, this._imgBackground.Sprite.BoundingBoxes[0].Height * this._pnlContainer.Scale.Y);
      }
    }

    public bool DoOnLeaveEffect { get; set; }

    public bool Locked
    {
      get
      {
        return /*BrainGame.IsTrial &&*/
                    this._levelStageInfo != null 
                    && !this._levelStageInfo.AvailableInDemo
                    && !Game1.GameSettings.AllStagesUnlocked 
                        || this._locked;
      }
      set
      {
        this._locked = value;
        this.Refresh();
      }
    }

    public ThemeType ThemeId
    {
      get => this._themeId;
      set
      {
        this._themeId = value;
        this._btnStage.Sprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/menu-elements-1/" + this._themeId.ToString() + "StageIcon");
      }
    }

    public MedalType Medal
    {
      get => this._medal.Face;
      set
      {
        this._medal.Face = value;
        if (this._medal.Face == MedalType.None)
          this._medal.Visible = false;
        else
          this._medal.Visible = true;
      }
    }

    private Vector2 LockerPosition { get; set; }

    private Vector2 LockerDemoPosition { get; set; }

    private Vector2 EnterEffectScale { get; set; }

    public bool Selected
    {
      get => this._selected;
      set
      {
        this._selected = value;
        if (this._selected)
        {
          this.Effect = (ITransformEffect) this._selectedEffect;
          this.Effect.Reset();
          this._pnlContainer.Scale = new Vector2(0.9f, 0.9f);
        }
        else
        {
          this.Effect = (ITransformEffect) null;
          this._pnlContainer.Scale = UIStage.DEFAULT_SCALE;
          this.BlendColor = Color.White;
        }
      }
    }

    public UIStage(UIScreen screenOwner, int stageNr)
      : base(screenOwner)
    {
      this.StageNr = stageNr;
      this.Name = "Stage_" + stageNr.ToString();
      this.OnEnter += new UIControl.UIEvent(this.Stage_OnEnter);
      this.OnLeave += new UIControl.UIEvent(this.Stage_OnLeave);
      this.OnInitializeFromContent += new UIControl.UIEvent(this.UIStage_OnInitializeFromContent);
      this.OnAfterInitializeFromContent += new UIControl.UIEvent(this.UIStage_OnAfterInitializeFromContent);
      this._pnlContainer = new UIPanel(screenOwner);
      this._pnlContainer.Name = nameof (_pnlContainer);
      this._pnlContainer.ParentAlignment = AlignModes.HorizontalyVertically;
      this._pnlContainer.ShowEffect = (TransformEffectBase) new SquashEffect(0.5f, 4f, 0.08f, this.BlendColor, UIStage.DEFAULT_SCALE);
      this._pnlContainer.HideEffect = (TransformEffectBase) new PopOutEffect(new Vector2(1.3f, 1.3f), 6f);
      this._pnlContainer.OnShow += new UIControl.UIEvent(this._pnlContainer_OnShow);
      this._pnlContainer.OnHide += new UIControl.UIEvent(this._pnlContainer_OnHide);
      this.Controls.Add((UIControl) this._pnlContainer);
      this._imgBackground = new UIImage(screenOwner, "spriteset/boards/LightWoodTiny");
      this._imgBackground.ParentAlignment = AlignModes.HorizontalyVertically;
      this._pnlContainer.Controls.Add((UIControl) this._imgBackground);
      this._btnStage = new UIButton(screenOwner);
      this._btnStage.OnDoublePress += new UIControl.UIEvent(this._btnStage_OnDoublePress);
      this._btnStage.OnBeforePress += new UIControl.UIEvent(this._btnStage_OnBeforePress);
      this._btnStage.Size = new Size(this._imgBackground.Size.Width - 140f, this._imgBackground.Size.Height - 240f);
      this._btnStage.SizeMode = ImageSizeMode.Center;
      this._btnStage.AnimateImage = false;
      this._pnlContainer.Controls.Add((UIControl) this._btnStage);
      this._capStageNr = new UICaption(screenOwner, "", Colors.StageSelectionNumber, UICaption.CaptionStyle.StageSelectionStageNr);
      this._capStageNr.Text = this.StageNr.ToString();
      this._capStageNr.ParentAlignment = AlignModes.Right | AlignModes.Bottom;
      this._capStageNr.Margins.Right = this._capStageNr.MeasureString().X + 50f;
      this._capStageNr.Margins.Bottom = this._capStageNr.MeasureString().Y + 20f;
      this._pnlContainer.Controls.Add((UIControl) this._capStageNr);
      this._goalIcon = new UISnailsStageGoalIcon(screenOwner);
      this._goalIcon.IconSize = UISnailsStageGoalIcon.GoalIconSize.Small;
      this._pnlContainer.Controls.Add((UIControl) this._goalIcon);
      this._medal = new UISnailsMedal(screenOwner, MedalType.None);
      this._medal.MedalSize = UISnailsMedal.MedalSizeType.Tiny;
      this._medal.IgnoreShowEffect = true;
      this._pnlContainer.Controls.Add((UIControl) this._medal);
      this._imgLocker = new UIImage(screenOwner, "spriteset/common-elements-1/LockerSmall", "__STATIC__");
      this._imgLocker.BlendColorWithParent = false;
      this._imgLocker.Effect = (ITransformEffect) new ScaleEffect(new Vector2(1f, 1f), 0.2f, new Vector2(0.95f, 0.95f), true);
      this._pnlContainer.Controls.Add((UIControl) this._imgLocker);
      this._imgLockerDemo = new UIImage(screenOwner, "spriteset/menu-elements-1/LockedInDemo", "__TEMPORARY__");
      this._imgLockerDemo.BlendColorWithParent = false;
      this._imgLockerDemo.Scale = new Vector2(0.5f, 0.5f);
      this._imgLockerDemo.Effect = (ITransformEffect) new ScaleEffect(new Vector2(0.45f, 0.45f), 0.05f, new Vector2(0.42f, 0.42f), true);
      this._pnlContainer.Controls.Add((UIControl) this._imgLockerDemo);
      this._pnlContainer.Scale = UIStage.DEFAULT_SCALE;
      this._focusSample = BrainGame.ResourceManager.GetSampleStatic("sfx/menu-item-focus");
      this._selectedSample = BrainGame.ResourceManager.GetSampleStatic("sfx/menu-item-selected");
      this.Size = this._imgBackground.Size;
      this._pnlContainer.Size = this.Size;
      this.Locked = true;
      this._selectedEffect = new ColorEffect(Color.White, Color.LightGray, 0.04f, true);
    }

    private void UIStage_OnInitializeFromContent(IUIControl sender)
    {
      this._goalIcon.Position = this.GetContentPropertyValue<Vector2>("goalIconPosition", this._goalIcon.Position);
      this._medal.Position = this.GetContentPropertyValue<Vector2>("medalPosition", this._medal.Position);
      this.LockerPosition = this.GetContentPropertyValue<Vector2>("lockerPosition", this.LockerPosition);
      this.LockerDemoPosition = this.GetContentPropertyValue<Vector2>("lockerDemoPosition", this.LockerDemoPosition);
      this.EnterEffectScale = this.GetContentPropertyValue<Vector2>("enterEffectScale", this.EnterEffectScale);
    }

    private void UIStage_OnAfterInitializeFromContent(IUIControl sender)
    {
      this._imgLocker.Position = this.LockerPosition;
      this._imgLockerDemo.Position = this.LockerDemoPosition;
    }

    public override void InitializeFromContent()
    {
      base.InitializeFromContent();
      this.InitializeFromContent(nameof (UIStage));
    }

    private void _btnStage_OnBeforePress(IUIControl sender)
    {
      this._selectedSample.Play();
      if (this.OnPress == null)
        return;
      this.OnPress((IUIControl) this);
    }

    private void _pnlContainer_OnHide(IUIControl sender) => this.InvokeOnHide();

    private void _pnlContainer_OnShow(IUIControl sender) => this.InvokeOnShow();

    private void _btnStage_OnPress(IUIControl sender)
    {
      if (this.OnPress == null)
        return;
      this.OnPress((IUIControl) this);
    }

    private void _btnStage_OnDoublePress(IUIControl sender)
    {
      if (this.OnDoublePress == null)
        return;
      this.OnDoublePress((IUIControl) this);
    }

    private void Refresh()
    {
      this.AcceptControllerInput = !this.Locked;
      this._imgLocker.Visible = this.Locked;
      this._imgLockerDemo.Visible = false;
      this.Enabled = !this.Locked;
      if (this.Locked && Game1.GameSettings.GameplayMode != BrainSettings.GameplayModeType.Retail)
      {
        this._imgLocker.Visible = true;
        this._imgLockerDemo.Visible = false;
        if (this._levelStageInfo != null && !this._levelStageInfo.AvailableInDemo /*&& BrainGame.IsTrial*/)
        {
          this._imgLocker.Visible = false;
          this._imgLockerDemo.Visible = true;
          this.AcceptControllerInput = true;
          this.Enabled = true;
        }
      }
      this._capStageNr.Visible = !this.Locked;
      this._btnStage.Enabled = this.Enabled;
      this.BlendColor = this.Locked ? Color.Black : Color.White;
      this.Medal = MedalType.None;
      if (this._levelStageInfo == null)
        return;
      this._goalIcon.Goal = this._levelStageInfo._goal;
      this._goalIcon.Visible = this._goalIcon.Goal != GoalType.SnailDelivery;
      PlayerStageStats stageStats = Game1.ProfilesManager.CurrentProfile.PlayerStats.GetStageStats(this._levelStageInfo.StageId);
      if (stageStats == null)
        return;
      this.Medal = stageStats.Medal;
    }

    public void Reset()
    {
      this.DoOnLeaveEffect = true;
      this._pnlContainer.Scale = UIStage.DEFAULT_SCALE;
      this._pnlContainer.Visible = true;
    }

    public void Stage_OnEnter(IUIControl sender)
    {
      if (this.Locked || !this.AcceptOnEnterEvents)
        return;
      this._focusSample.Play();
      this._pnlContainer.Effect = (ITransformEffect) new ScaleEffect(UIStage.DEFAULT_SCALE, 2f, this.EnterEffectScale, false);
    }

    public void Stage_OnLeave(IUIControl sender)
    {
      if (!this.DoOnLeaveEffect)
        return;
      this._pnlContainer.Effect = (ITransformEffect) null;
      if (this.Selected)
        return;
      this._pnlContainer.Scale = UIStage.DEFAULT_SCALE;
    }
  }
}
