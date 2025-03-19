
// Type: TwoBrainsGames.Snails.Screens.OverscanScreen
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.Snails.Screens.Transitions;


namespace TwoBrainsGames.Snails.Screens
{
  internal class OverscanScreen(ScreenNavigator owner) : SnailsScreen(owner, ScreenType.Overscan)
  {
    private const int OVERSCAN_INCREMENT = 2;
    private const int LINE_SPACING = 500;
    private UISnailsButton _btnContinue;
    private Rectangle _viewportArea;
    private Rectangle _viewportSafeArea;
    private float _viewportWidth;
    private float _viewportHeight;
    private float _viewportX;
    private float _viewportY;
    private int _screenWidth;
    private int _screenHeight;
    private ScreenType _caller;
    private Viewport _vp = new Viewport();
    private UISnailsBoard _board;
    private UITimer _tmrAccept;
    private UIImage _imgStick;
    private List<UISnail> _snails;
    private UICaption[] _capInstructions;
    private UICaption _capConfigLater;

    private OverscanScreen.ScreenState State { get; set; }

    public override void OnLoad()
    {
      base.OnLoad();
      this.BackgroundImageBlendColor = Colors.OverscanScrBkColor1;
      this._board = new UISnailsBoard((UIScreen) this, UISnailsBoard.BoardType.LightWoodMediumLong);
      this._board.ParentAlignment = AlignModes.HorizontalyVertically;
      this._board.OnShow += new UIControl.UIEvent(this._board_OnShow);
      this.Controls.Add((UIControl) this._board);
      this.SetupInstructionMessage();
      this._tmrAccept = new UITimer((UIScreen) this, 500.0, false);
      this._tmrAccept.OnTimer += new UIControl.UIEvent(this._tmrAccept_OnAccept);
      this.Controls.Add((UIControl) this._tmrAccept);
      this._imgStick = new UIImage((UIScreen) this, "spriteset/main-menu-objects2/LeftStick", "__TEMPORARY__");
      this._imgStick.ParentAlignment = AlignModes.Horizontaly;
      this._imgStick.Position = new Vector2(0.0f, 1900f);
      this._board.Controls.Add((UIControl) this._imgStick);
      this._capConfigLater = new UICaption((UIScreen) this, "", new Color(230, 230, 230), UICaption.CaptionStyle.NormalTextSmall);
      this._capConfigLater.TextResourceId = "LBL_OVERSCAN_CONFIG_LATER";
      this._capConfigLater.ParentAlignment = AlignModes.Horizontaly | AlignModes.Bottom;
      this._capConfigLater.Margins.Bottom = 400f;
      this._board.Controls.Add((UIControl) this._capConfigLater);
      this._btnContinue = new UISnailsButton((UIScreen) this, "BTN_ACCEPT", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.Accept, new UIControl.UIEvent(this.btnContinue_OnClick), false);
      this._btnContinue.ParentAlignment = AlignModes.Horizontaly | AlignModes.Bottom;
      this._btnContinue.Margins.Bottom = -1200f;
      this._board.Controls.Add((UIControl) this._btnContinue);
      this.OnOpenTransitionEnded += new UIControl.UIEvent(this.OverscanScreen_OnOpenTransitionEnded);
      this.OnLanguageChanged += new UIControl.UIEvent(this.OverscanScreen_OnLanguageChanged);
      this._viewportSafeArea = Game1.SafeArea;
      this._viewportArea = this._viewportSafeArea;
      this._screenWidth = BrainGame.ScreenWidth;
      this._screenHeight = BrainGame.ScreenHeight;
    }

    public override void OnStart()
    {
      this._viewportArea = Game1.ProfilesManager.CurrentProfile.Viewport.Bounds;
      this._viewportWidth = (float) this._viewportArea.Width;
      this._viewportHeight = (float) this._viewportArea.Height;
      this._viewportX = (float) this._viewportArea.X;
      this._viewportY = (float) this._viewportArea.Y;
      this.State = OverscanScreen.ScreenState.Starting;
      this.CenterSafeFrame();
      this.ValidateOverscanBounds();
      this.UpdateViewport();
      this.DisableInput();
      this._board.Visible = false;
      BrainGame.GameCursor.Visible = false;
      BrainGame.ClearColor = Colors.BBOverscanScreen;
      this._snails = new List<UISnail>();
      for (int index = 0; index < 20; ++index)
      {
        UISnail control = new UISnail((UIScreen) this);
        this._snails.Add(control);
        this.Controls.Add((UIControl) control);
      }
      this.BackgroundImageBlendColor = Colors.OverscanScrBkColor1;
      this._capConfigLater.Visible = true;
      this._caller = this.Navigator.GlobalCache.Get<ScreenType>("OVERSCAN_CALLER_SCREEN", ScreenType.None);
      if (this._caller != ScreenType.Options)
        return;
      this.BackgroundImageBlendColor = Colors.OverscanScrBkColor2;
      this._capConfigLater.Visible = false;
    }

    public override void OnUpdate(BrainGameTime gameTime)
    {
      switch (this.State)
      {
        case OverscanScreen.ScreenState.Active:
          this.UpdateStateActive(gameTime);
          break;
        case OverscanScreen.ScreenState.Accepted:
          this.UpdateStateAccepted();
          break;
      }
    }

    private void OverscanScreen_OnOpenTransitionEnded(IUIControl sender) => this._board.Show();

    private void OverscanScreen_OnLanguageChanged(IUIControl sender)
    {
      this.SetupInstructionMessage();
    }

    private void UpdateStateAccepted()
    {
      if (this._snails.Count > 0)
      {
        for (int index = 0; index < this._snails.Count; ++index)
        {
          if ((double) this._snails[index].Position.Y > (double) this.PixelsToScreenUnits(new Vector2(0.0f, (float) BrainGame.ScreenHeight)).Y + (double) this._snails[index].Size.Height)
          {
            this.Controls.Remove((UIControl) this._snails[index]);
            this._snails.Remove(this._snails[index]);
            --index;
          }
        }
        if (this._snails.Count == 0)
          this._tmrAccept.Enabled = true;
      }
      BrainGame.ClearColor = Colors.BBDefaultColor;
    }

    private void UpdateStateActive(BrainGameTime gameTime)
    {
      if (this._inputController.ActionUp)
      {
        this._viewportHeight += 2f;
        --this._viewportY;
      }
      if (this._inputController.ActionDown)
      {
        this._viewportHeight -= 2f;
        ++this._viewportY;
      }
      if (this._inputController.ActionRight)
      {
        this._viewportWidth -= 2f;
        ++this._viewportX;
      }
      if (this._inputController.ActionLeft)
      {
        this._viewportWidth += 2f;
        --this._viewportX;
      }
      if (Game1.GameSettings.UseGamepad)
      {
        if ((double) this._inputController.MotionPosition.X != 0.0)
        {
          this._viewportX += this._inputController.MotionPosition.X * 1f;
          this._viewportWidth -= this._inputController.MotionPosition.X * 2f;
        }
        if ((double) this._inputController.MotionPosition.Y != 0.0)
        {
          this._viewportY -= this._inputController.MotionPosition.Y * 1f;
          this._viewportHeight += this._inputController.MotionPosition.Y * 2f;
        }
      }
      this.ValidateOverscanBounds();
      this._viewportArea.X = (int) this._viewportX;
      this._viewportArea.Width = (int) this._viewportWidth;
      this._viewportArea.Y = (int) this._viewportY;
      this._viewportArea.Height = (int) this._viewportHeight;
      this.UpdateViewport();
    }

    private void UpdateViewport()
    {
      this._vp.X = this._viewportArea.X;
      this._vp.Y = this._viewportArea.Y;
      this._vp.Width = this._viewportArea.Width;
      this._vp.Height = this._viewportArea.Height;
      BrainGame.SetViewport(this._vp);
    }

    private void ValidateOverscanBounds()
    {
      if ((double) this._viewportX > (double) this._viewportSafeArea.X)
        this._viewportX = (float) this._viewportSafeArea.X;
      if ((double) this._viewportY > (double) this._viewportSafeArea.Y)
        this._viewportY = (float) this._viewportSafeArea.Y;
      if ((double) this._viewportX < 0.0)
        this._viewportX = 0.0f;
      if ((double) this._viewportY < 0.0)
        this._viewportY = 0.0f;
      if ((double) this._viewportX + (double) this._viewportWidth < (double) (this._viewportSafeArea.X + this._viewportSafeArea.Width))
        this._viewportWidth = (float) (this._viewportSafeArea.X + this._viewportSafeArea.Width) - this._viewportX;
      if ((double) this._viewportY + (double) this._viewportHeight < (double) (this._viewportSafeArea.Y + this._viewportSafeArea.Height))
        this._viewportHeight = (float) (this._viewportSafeArea.Y + this._viewportSafeArea.Height) - this._viewportY;
      if ((double) this._viewportX + (double) this._viewportWidth > (double) this._screenWidth)
        this._viewportWidth = (float) this._screenWidth - this._viewportX;
      if ((double) this._viewportY + (double) this._viewportHeight <= (double) this._screenHeight)
        return;
      this._viewportHeight = (float) this._screenHeight - this._viewportY;
    }

    private void CenterSafeFrame()
    {
      this._viewportArea.X = (int) (((double) this._screenWidth - (double) this._viewportWidth) / 2.0);
      this._viewportArea.Width = (int) this._viewportWidth;
      this._viewportArea.Y = (int) (((double) this._screenHeight - (double) this._viewportHeight) / 2.0);
      this._viewportArea.Height = (int) this._viewportHeight;
    }

    private void AcceptSettings()
    {
      Viewport viewport = new Viewport();
      viewport.X = this._viewportArea.X;
      viewport.Y = this._viewportArea.Y;
      viewport.Width = this._viewportArea.Width;
      viewport.Height = this._viewportArea.Height;
      Game1.ProfilesManager.CurrentProfile.OverscanSet = true;
      Game1.ProfilesManager.CurrentProfile.Viewport = viewport;
      Game1.ProfilesManager.Save();
      BrainGame.SetViewport(viewport);
      foreach (UISnail snail in this._snails)
        snail.Kill();
      this.State = OverscanScreen.ScreenState.Accepted;
      this.DisableInput();
      this._board.Hide();
    }

    private void _tmrAccept_OnAccept(IUIControl sender)
    {
      this.DisableInput();
      if (this._caller == ScreenType.Options)
      {
        this.Navigator.GlobalCache.Set("OPTIONS_STARTUP_MODE", (object) OptionsScreen.StartupType.MenuVisible);
        this.NavigateTo(ScreenType.Options.ToString(), (Transition) ScreenTransitions.LeafsClosing, (Transition) ScreenTransitions.LeafsOpening);
      }
      else
      {
        this.Navigator.GlobalCache.Set("MAIN_SCREEN_STARTUP_MODE", (object) MainMenuScreen.StartupType.AllHidden);
        this.NavigateTo(ScreenType.MainMenu.ToString(), (Transition) null, (Transition) null);
      }
    }

    private void _board_OnShow(IUIControl sender)
    {
      this.EnableInput();
      this.InstructionBar.HideAllLabels();
      this.InstructionBar.ShowLabel(UIInstructionLabel.LabelActionTypes.Accept);
      this.InstructionBar.ShowLabel(UIInstructionLabel.LabelActionTypes.AdjustSize);
      this._btnContinue.Focus();
      this.State = OverscanScreen.ScreenState.Active;
    }

    private void btnContinue_OnClick(IUIControl sender) => this.AcceptSettings();

    private void SetupInstructionMessage()
    {
      string[] multiString = LanguageManager.GetMultiString("MSG_OVERSCAN");
      if (this._capInstructions != null)
      {
        foreach (UIControl capInstruction in this._capInstructions)
          this._board.Controls.Remove(capInstruction);
      }
      this._capInstructions = new UICaption[multiString.Length];
      Vector2 vector2 = new Vector2(0.0f, 650f);
      for (int index = 0; index < multiString.Length; ++index)
      {
        this._capInstructions[index] = new UICaption((UIScreen) this, multiString[index], Colors.OverscanMessageText, UICaption.CaptionStyle.OverscanMessage);
        this._capInstructions[index].ParentAlignment = AlignModes.Horizontaly;
        this._capInstructions[index].Position = vector2;
        vector2 += new Vector2(0.0f, 500f);
        this._board.Controls.Add((UIControl) this._capInstructions[index]);
      }
    }

    private enum ScreenState
    {
      Starting,
      Active,
      Accepted,
    }
  }
}
