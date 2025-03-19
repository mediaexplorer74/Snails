
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UISnailsWindow
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
  internal class UISnailsWindow : UIControl
  {
    private UISnailsBoard _board;
    private UISnailsMenuTitle _title;
    private UISnailsButton _btnContinue;
    private UICloseButton _btnClose;
    private UISnailsWindow.ButtonType _buttonType;

    public event UIControl.UIEvent OnDismiss;

    public event UIControl.UIEvent OnDismissBegin;

    public event UIControl.UIEvent OnDismissPressed;

    public event UIControl.UIEvent OnDismissButtonShown;

    protected UISnailsBoard Board => this._board;

    protected UISnailsButton ContinueButton => this._btnContinue;

    protected UICloseButton CloseButton => this._btnClose;

    public string TitleResourceId
    {
      set => this._title.TextResourceId = value;
    }

    public UISnailsBoard.BoardType BoardType
    {
      get => this._board.Type;
      set
      {
        this._board.Type = value;
        this._board.Size = new Size(this._board.Size.Width, this._board.Size.Height + this.NativeResolutionY(1200f));
        this.Size = this._board.Size;
      }
    }

    public bool DismissButtonVisible
    {
      get => this._btnContinue.Visible;
      set => this._btnContinue.Visible = value;
    }

    public UISnailsWindow.ButtonType ButtonCaptionType
    {
      get => this._buttonType;
      set
      {
        this._buttonType = value;
        switch (this._buttonType)
        {
          case UISnailsWindow.ButtonType.Start:
            this._btnContinue.TextResourceId = "BTN_START";
            this._btnContinue.ButtonAction = UISnailsButton.ButtonActionType.Start;
            break;
          case UISnailsWindow.ButtonType.Dismiss:
            this._btnContinue.TextResourceId = "BTN_DISMISS";
            this._btnContinue.ButtonAction = UISnailsButton.ButtonActionType.Back;
            break;
        }
      }
    }

    public UISnailsWindow(UIScreen screenOwner)
      : base(screenOwner)
    {
      this._board = new UISnailsBoard(screenOwner, UISnailsBoard.BoardType.LightWoodMediumLong);
      this._board.ParentAlignment = AlignModes.HorizontalyVertically;
      this._board.OnHide += new UIControl.UIEvent(this._board_OnHide);
      this._board.OnShow += new UIControl.UIEvent(this._board_OnShow);
      this.Controls.Add((UIControl) this._board);
      this._title = new UISnailsMenuTitle(screenOwner);
      this._title.TextResourceId = "TITLE_HOW_TO_PLAY";
      this._title.BoardSize = UISnailsMenuTitle.TitleSize.Big;
      this._btnContinue = new UISnailsButton(screenOwner, "BTN_DISMISS", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.Back, new UIControl.UIEvent(this.btnContinue_OnClick), false);
      this._btnContinue.OnClickBegin += new UIControl.UIEvent(this.ContinueButton_OnClickBegin);
      this._btnContinue.ParentAlignment = AlignModes.Horizontaly | AlignModes.Bottom;
      this._btnContinue.OnShow += new UIControl.UIEvent(this._btnContinue_OnShow);
      this._btnContinue.Name = "Continue Button";
      this._btnContinue.ButtonAction = UISnailsButton.ButtonActionType.Back;
      this._board.Controls.Add((UIControl) this._btnContinue);
      this._btnClose = new UICloseButton(screenOwner);
      this._btnClose.ParentAlignment = AlignModes.Right | AlignModes.Top;
      this._btnClose.Margins.Top = -400f;
      this._btnClose.OnPress += new UIControl.UIEvent(this._btnClose_OnPress);
      this._btnClose.OnAcceptBegin += new UIControl.UIEvent(this.ContinueButton_OnClickBegin);
      this.Controls.Add((UIControl) this._btnClose);
      this.ButtonCaptionType = UISnailsWindow.ButtonType.Dismiss;
      this.DismissButtonVisible = true;
      this.BoardType = UISnailsBoard.BoardType.LightWoodMediumLong;
      this.OnAfterInitializeFromContent += new UIControl.UIEvent(this.UISnailsWindow_OnAfterInitializeFromContent);
    }

    public override void InitializeFromContent()
    {
      base.InitializeFromContent();
      this.InitializeFromContent(nameof (UISnailsWindow));
    }

    private void UISnailsWindow_OnAfterInitializeFromContent(IUIControl sender)
    {
      this._title.Position = this.GetContentPropertyValue<Vector2>("titlePosition", this._title.Position);
    }

    private void _board_OnShow(IUIControl sender) => this.InvokeOnShow();

    private void _board_OnHide(IUIControl sender)
    {
      this.Visible = false;
      this.InvokeOnDismiss();
    }

    public override void Show()
    {
      base.Show();
      this._board.Show();
    }

    private void InvokeOnDismiss()
    {
      if (this.OnDismiss == null)
        return;
      this.OnDismiss((IUIControl) this);
    }

    private void btnContinue_OnClick(IUIControl sender)
    {
      if (this.OnDismissPressed != null)
        this.OnDismissPressed((IUIControl) this);
      this.Dismiss();
    }

    private void ContinueButton_OnClickBegin(IUIControl sender)
    {
    }

    public void ShowDismissButton() => this._btnContinue.Show();

    private void _btnContinue_OnShow(IUIControl sender)
    {
      if (this.OnDismissButtonShown == null)
        return;
      this.OnDismissButtonShown((IUIControl) this);
    }

    public override void Focus() => this._btnContinue.Focus();

    protected void Dismiss()
    {
      if (this.OnDismissBegin != null)
        this.OnDismissBegin((IUIControl) this);
      ((SnailsScreen) this.ScreenOwner).DisableInput();
      this.Hide();
    }

    private void _btnClose_OnPress(IUIControl sender) => this.Dismiss();

    public enum ButtonType
    {
      Start,
      Dismiss,
    }
  }
}
