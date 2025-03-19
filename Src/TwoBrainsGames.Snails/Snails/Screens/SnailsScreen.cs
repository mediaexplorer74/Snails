
// Type: TwoBrainsGames.Snails.Screens.SnailsScreen
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;
using TwoBrainsGames.Snails.Configuration;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.Snails.Screens.Transitions;


namespace TwoBrainsGames.Snails.Screens
{
  internal class SnailsScreen : UIScreen
  {
    private const string CURSOR_SPRITE = "DefaultCursor";
    protected const float CAPTIONS_GRADIENT_INCREMENT = 0.9f;
    protected UIInstructionBar _ibInstBar;
    protected UIAsync _asyncLoad;
    private bool _withBlurEffect;
    private Transition _pauseTransition;
    protected SnailsScreen.ScreenBackgroundType _backgroundType;
    private LeafTransition _leafs;
    protected ScreenType _screenType;
    private UIImage _imgTrial;

    public event EventHandler OnBlurEffectFadeEnded;

    public event EventHandler OnBlurEffectEnded;

    public UIInstructionBar InstructionBar => this._ibInstBar;

    protected UIFooterMessage FooterMessage { get; set; }

    protected bool WithBlurEffect
    {
      get => this._withBlurEffect;
      set => this._withBlurEffect = value;
    }

    protected SnailsScreen.ScreenBackgroundType BackgroundType
    {
      get => this._backgroundType;
      set
      {
        this._backgroundType = value;
        switch (this._backgroundType)
        {
          case SnailsScreen.ScreenBackgroundType.None:
            this.BackgroundImage = (Sprite) null;
            break;
          case SnailsScreen.ScreenBackgroundType.Image:
            this.BackgroundImage = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/menus-background", "MenusBackground");
            this.BackgroundImageMode = ScreenBackgroudImageMode.FitToScreen;
            break;
          case SnailsScreen.ScreenBackgroundType.Leafs:
            if (this._leafs == null)
            {
              this._leafs = new LeafTransition(LeafTransition.State.ClosedStopped);
              this._leafs.LoadContent();
            }
            this.BackgroundImage = (Sprite) null;
            break;
        }
      }
    }

    protected bool ShowTrialTag { get; set; }

    public SnailsScreen(ScreenNavigator owner, ScreenType screenType)
      : base(owner)
    {
      this._screenType = screenType;
    }

    public bool UseAudibleSoundsBoundingBox { get; set; }

    public override void OnLoad()
    {
      this.BackgroundType = SnailsScreen.ScreenBackgroundType.None;
      this.CursorMode = Game1.GameSettings.MenuCursorMode;
      this.OnGameplayModeChanged += new UIControl.UIEvent(this.SnailsScreen_OnGameplayModeChanged);
      this._ibInstBar = new UIInstructionBar((UIScreen) this);
      if (Game1.GameSettings.ShowTheInstructionBar)
        this.Controls.Add((UIControl) this._ibInstBar);
      this._asyncLoad = new UIAsync((UIScreen) this);
      this.Controls.Add((UIControl) this._asyncLoad);
      this.FooterMessage = new UIFooterMessage((UIScreen) this);
      this.FooterMessage.Visible = false;
      if (Game1.GameSettings.ShowFooterMessage)
        this.Controls.Add((UIControl) this.FooterMessage);
      this.OnBeforeControlsDraw += new UIControl.UIEvent(this.SnailsScreen_OnBeforeControlsDraw);
      this._imgTrial = new UIImage((UIScreen) this, "spriteset/common-elements-1/Trial", "__STATIC__");
      this._imgTrial.OnGameplayModeChanged += new UIControl.UIEvent(this._imgTrial_OnGameplayModeChanged);
      this._imgTrial.Position = new Vector2(9290f, 800f);
      if (Game1.GameSettings.PresentationMode == GameSettings.PresentationType.HD)
      {
        this._imgTrial.Effect = (ITransformEffect) new ScaleEffect(new Vector2(1f, 1f), 0.2f, new Vector2(0.95f, 0.95f), true);
      }
      else
      {
        this._imgTrial.Scale = new Vector2(0.7f, 0.7f);
        this._imgTrial.Effect = (ITransformEffect) new ScaleEffect(this._imgTrial.Scale, 0.2f, new Vector2(0.65f, 0.65f), true);
      }
      this._imgTrial.Rotation = 15f;
      this._imgTrial.AcceptControllerInput = true;
      this._imgTrial.OnAccept += new UIControl.UIEvent(this._imgTrial_OnAccept);
      this.ShowTrialTag = false;
      this.UseAudibleSoundsBoundingBox = false;
      this.Controls.Add((UIControl) this._imgTrial);
    }

    private void _imgTrial_OnAccept(IUIControl sender)
    {
      if (!BrainGame.IsTrial || !Game1.GameSettings.WithAppStore)
        return;
      this.NavigateToPurchase();
    }

    private void SnailsScreen_OnGameplayModeChanged(IUIControl sender)
    {
      this._imgTrial.Visible = BrainGame.IsTrial && this.ShowTrialTag;
    }

    private void SnailsScreen_OnBeforeControlsDraw(IUIControl sender)
    {
      if (this.BackgroundType == SnailsScreen.ScreenBackgroundType.Leafs)
        this._leafs.Draw(this.SpriteBatch);
      if (this._pauseTransition == null)
        return;
      this._pauseTransition.Draw();
    }

    public override void OnStart()
    {
      base.OnStart();
      if (this._leafs != null)
        this._leafs.Initialize();
      if (this.WithBlurEffect)
      {
        this._pauseTransition = !Game1.GameSettings.SupportsShaderEffects ? (Transition) new SnailsGrayoutTransition((UIScreen) this, false) : (Transition) new SnailsBlurTransition((UIScreen) this, false);
        this._pauseTransition.LoadContent();
        this._pauseTransition.Initialize();
        this._pauseTransition.OnTransitionEnded += new EventHandler(this._pauseTransition_OnBlurEnded);
      }
      BrainGame.ClearColor = Colors.BBDefaultColor;
      BrainGame.DisplayHDDAccessIcon = true;
      this.FooterMessage.Initialize();
      if (this._pauseTransition != null)
        this._pauseTransition.Reset();
      this.Navigator.GlobalCache.Set("CURRENT_SCREEN", (object) this._screenType);
      this._imgTrial.Visible = BrainGame.IsTrial && this.ShowTrialTag;
      this._imgTrial.BringToFront();
      BrainGame.SampleManager.UseAudibleBoundingSquare = this.UseAudibleSoundsBoundingBox;
    }

    private void _imgTrial_OnGameplayModeChanged(IUIControl sender)
    {
      this._imgTrial.Visible = BrainGame.IsTrial && this.ShowTrialTag;
    }

    protected void FadeBlurOut()
    {
      if (this._pauseTransition == null)
        return;
      this._pauseTransition.TransitionOut();
    }

    public void DisableInput()
    {
      this.InstructionBar.HideAllLabels();
      this.AcceptControllerInput = false;
      BrainGame.GameCursor.Visible = this.CursorMode != CursorModes.SnapToControl;
      if (Game1.GameSettings.ShowCursor)
        return;
      BrainGame.GameCursor.Visible = false;
    }

    public virtual void EnableInput()
    {
      BrainGame.GameCursor.SetCursor(0);
      BrainGame.GameCursor.Visible = Game1.GameSettings.ShowCursor;
      this.InstructionBar.ShowLabel(UIInstructionLabel.LabelActionTypes.Accept);
      this.InstructionBar.ShowLabel(UIInstructionLabel.LabelActionTypes.Back);
      this.AcceptControllerInput = true;
    }

    protected virtual void EnableInput(bool hideCursor)
    {
      BrainGame.GameCursor.Visible = hideCursor;
      this.EnableInput();
    }

    public override void OnUpdate(BrainGameTime gameTime)
    {
      if (this._pauseTransition != null)
        this._pauseTransition.Update(gameTime);
      Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalRunningTime += gameTime.ElapsedGameTime;
    }

    private void _pauseTransition_OnBlurEnded(object sender, EventArgs e)
    {
      if (!((ISnailsPauseTransition) this._pauseTransition).IsTransitionOut)
      {
        if (this.OnBlurEffectEnded == null)
          return;
        this.OnBlurEffectEnded((object) this, e);
      }
      else
      {
        if (this.OnBlurEffectFadeEnded == null)
          return;
        this.OnBlurEffectFadeEnded((object) this, e);
      }
    }

    public void NavigateToPurchase()
    {
      this.NavigateTo("MainMenu", ScreenType.Purchase.ToString(), (Transition) ScreenTransitions.LeafsClosing, (Transition) null);
    }

    protected enum ScreenBackgroundType
    {
      None,
      Image,
      Leafs,
    }
  }
}
