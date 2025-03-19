
// Type: TwoBrainsGames.Snails.Screens.XBoxHelpScreen
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Screens.CommonControls;


namespace TwoBrainsGames.Snails.Screens
{
  internal class XBoxHelpScreen : SnailsScreen
  {
    private UIXBoxControls _xboxController;

    public XBoxHelpScreen(ScreenNavigator owner)
      : base(owner, ScreenType.XBoxControllerHelp)
    {
      this.BackgroundColor = new Color(0, 0, 0, 200);
      this._xboxController = new UIXBoxControls((UIScreen) this);
      this._xboxController.Position = new Vector2(0.0f, 3500f);
      this._xboxController.ParentAlignment = AlignModes.Horizontaly;
      this._xboxController.OnDismiss += new UIControl.UIEvent(this._xboxController_OnDismiss);
      this._xboxController.OnShow += new UIControl.UIEvent(this._xboxController_OnShow);
      this.Controls.Add((UIControl) this._xboxController);
    }

    public override void OnLoad()
    {
      base.OnLoad();
      this.BackgroundImage = (Sprite) null;
    }

    public override void OnStart()
    {
      base.OnStart();
      this.DisableInput();
      this._xboxController.Visible = false;
      this._xboxController.Show();
    }

    private void _xboxController_OnShow(IUIControl sender)
    {
      this.EnableInput();
      this._xboxController.Focus();
    }

    private void _xboxController_OnDismiss(IUIControl sender) => this.Close();
  }
}
