
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UIXBoxControls
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UIXBoxControls : UISnailsWindow
  {
    private UIImage _imgController;
    private UIXBoxControls.ButtonCaption[] _captions = new UIXBoxControls.ButtonCaption[9]
    {
      new UIXBoxControls.ButtonCaption("LBL_XBOX_PAUSE", new Vector2(400f, 800f), new Size(1400f, 400f), HorizontalTextAligment.Right),
      new UIXBoxControls.ButtonCaption("LBL_XBOX_MOVE_CURSOR", new Vector2(400f, 1300f), new Size(1400f, 400f), HorizontalTextAligment.Right),
      new UIXBoxControls.ButtonCaption("LBL_XBOX_CURSOR_SNAP", new Vector2(400f, 1800f), new Size(1400f, 800f), HorizontalTextAligment.Right),
      new UIXBoxControls.ButtonCaption("LBL_XBOX_QUICK_SEL_TOOL", new Vector2(4600f, 250f), new Size(1400f, 400f), HorizontalTextAligment.Left),
      new UIXBoxControls.ButtonCaption("LBL_XBOX_TIME_WARP", new Vector2(4600f, 1000f), new Size(1400f, 400f), HorizontalTextAligment.Left),
      new UIXBoxControls.ButtonCaption("LBL_XBOX_DISMISS_TUTORIAL", new Vector2(4600f, 1400f), new Size(1400f, 400f), HorizontalTextAligment.Left),
      new UIXBoxControls.ButtonCaption("LBL_XBOX_ACTION", new Vector2(4600f, 1930f), new Size(1400f, 400f), HorizontalTextAligment.Left),
      new UIXBoxControls.ButtonCaption("LBL_XBOX_MAP_PAN", new Vector2(4600f, 2550f), new Size(1400f, 400f), HorizontalTextAligment.Left),
      new UIXBoxControls.ButtonCaption("LBL_XBOX_RESTART", new Vector2(1950f, 3200f), new Size(2500f, 400f), HorizontalTextAligment.Center)
    };

    public UIXBoxControls(UIScreen screenOwner)
      : base(screenOwner)
    {
      this._imgController = new UIImage(screenOwner, "spriteset/common-elements-1/XBoxController", "__STATIC__");
      this._imgController.ParentAlignment = AlignModes.Horizontaly;
      this._imgController.Position = new Vector2(0.0f, 250f);
      this.Board.Controls.Add((UIControl) this._imgController);
      foreach (UIXBoxControls.ButtonCaption caption in this._captions)
      {
        UICaption control = new UICaption(screenOwner, "", Colors.ControllerHelp, UICaption.CaptionStyle.ControllerHelp);
        control.TextResourceId = caption._textResourceId;
        control.Autosize = false;
        control.HorizontalAligment = caption._alignment;
        control.VerticalAligment = VerticalTextAligment.Center;
        control.Position = caption._position;
        control.Size = caption._size;
        this.Board.Controls.Add((UIControl) control);
      }
      this.TitleResourceId = "TITLE_GAME_CONTROLS";
    }

    public override void Update(BrainGameTime gameTime) => base.Update(gameTime);

    private struct ButtonCaption(
      string textResId,
      Vector2 pos,
      Size size,
      HorizontalTextAligment alignment)
    {
      public Size _size = size;
      public Vector2 _position = pos;
      public string _textResourceId = textResId;
      public HorizontalTextAligment _alignment = alignment;
    }
  }
}
