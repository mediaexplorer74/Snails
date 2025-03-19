
// Type: TwoBrainsGames.Snails.Screens.AutoSaveScreen
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
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
  internal class AutoSaveScreen(ScreenNavigator owner) : SnailsScreen(owner, ScreenType.AutoSave)
  {
    private const float LINE_SPACING = 450f;
    private const double SCREEN_ALLOW_SKIP_TIME = 1000.0;
    private const double SHOW_BOARD_TIME = 200.0;
    private UISnailsBoard _board;
    private UITimer _tmrShowBoard;
    private UITimer _tmrSkip;
    private UISnailsButton _btnContinue;
    private UICaption[] _capMessages;

    public override void OnLoad()
    {
      base.OnLoad();
      this.BackgroundImageBlendColor = Colors.AutoSaveScrBkColor;
      this._board = new UISnailsBoard((UIScreen) this, UISnailsBoard.BoardType.LightWoodLongNarrow);
      this._board.ParentAlignment = AlignModes.HorizontalyVertically;
      this.Controls.Add((UIControl) this._board);
      this.SetupMessage();
      this._tmrShowBoard = new UITimer((UIScreen) this, 200.0, false);
      this._tmrShowBoard.OnTimer += new UIControl.UIEvent(this._tmrShowBoard_OnTimer);
      this.Controls.Add((UIControl) this._tmrShowBoard);
      this._tmrSkip = new UITimer((UIScreen) this, 1000.0, false);
      this._tmrSkip.OnTimer += new UIControl.UIEvent(this._tmrSkip_OnTimer);
      this.Controls.Add((UIControl) this._tmrSkip);
      this._btnContinue = new UISnailsButton((UIScreen) this, "BTN_CONTINUE", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.Accept, new UIControl.UIEvent(this.btnContinue_OnClick), false);
      this._btnContinue.ParentAlignment = AlignModes.Horizontaly | AlignModes.Bottom;
      this._btnContinue.Margins.Bottom = -1200f;
      this._btnContinue.OnShow += new UIControl.UIEvent(this._btnContinue_OnShow);
      this._board.Controls.Add((UIControl) this._btnContinue);
      this.OnLanguageChanged += new UIControl.UIEvent(this.AutoSaveScreen_OnLanguageChanged);
    }

    public override void OnStart()
    {
      base.OnStart();
      this.DisableInput();
      this._board.Visible = false;
      this._tmrShowBoard.Reset();
      this._tmrShowBoard.Enabled = true;
      this._tmrSkip.Enabled = false;
      this._tmrSkip.Reset();
      this._btnContinue.Visible = false;
    }

    private void _tmrShowBoard_OnTimer(IUIControl sender)
    {
      this._board.Show();
      this._tmrSkip.Enabled = true;
    }

    private void _tmrSkip_OnTimer(IUIControl sender) => this._btnContinue.Show();

    private void btnContinue_OnClick(IUIControl sender) => this.NavigateToMainMenu();

    private void _btnContinue_OnShow(IUIControl sender)
    {
      this.EnableInput();
      this.InstructionBar.HideAllLabels();
      this.InstructionBar.ShowLabel(UIInstructionLabel.LabelActionTypes.Continue);
      this._btnContinue.Focus();
    }

    private void NavigateToMainMenu()
    {
      this.DisableInput();
      this.Navigator.GlobalCache.Set("MAIN_SCREEN_STARTUP_MODE", (object) MainMenuScreen.StartupType.IntroPicture);
      this.NavigateTo("MainMenu", ScreenType.MainMenu.ToString(), (Transition) ScreenTransitions.LeafsClosing, (Transition) ScreenTransitions.LeafsOpening);
    }

    private void AutoSaveScreen_OnLanguageChanged(IUIControl sender) => this.SetupMessage();

    private void SetupMessage()
    {
      string[] multiString = LanguageManager.GetMultiString("MSG_AUTOSAVE_LINE");
      this._capMessages = new UICaption[multiString.Length];
      Vector2 vector2 = new Vector2(0.0f, 720f);
      for (int index = 0; index < multiString.Length; ++index)
      {
        this._capMessages[index] = new UICaption((UIScreen) this, multiString[index], Colors.AutoSaveWarningText, UICaption.CaptionStyle.AutoSaveMessage);
        this._capMessages[index].ParentAlignment = AlignModes.Horizontaly;
        this._capMessages[index].Position = vector2;
        this._board.Controls.Add((UIControl) this._capMessages[index]);
        vector2 += new Vector2(0.0f, 450f);
      }
    }
  }
}
