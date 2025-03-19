
// Type: TwoBrainsGames.BrainEngine.UI.Controls.IUIControl
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.BrainEngine.UI.Controls
{
  public interface IUIControl
  {
    string Name { get; }

    UIScreen ScreenOwner { get; }

    IUIControl Parent { get; set; }

    Size Size { get; set; }

    Size SizeScaled { get; set; }

    Vector2 Position { get; set; }

    Vector2 AbsolutePosition { get; }

    Size SizeInPixels { get; }

    Vector2 PositionInPixels { get; }

    Vector2 CenterInPixels { get; }

    Color BackgroundColor { get; set; }

    AlignModes ParentAlignment { get; set; }

    UnitsType UnitType { get; set; }

    ControlCollection Controls { get; }

    Color BlendColor { get; set; }

    Vector2 AbsolutePositionInPixels { get; }

    Vector2 Scale { get; set; }

    bool AcceptControllerInput { get; set; }

    bool CursorInside { get; }

    bool CanFocus { get; }

    bool HasFocus { get; }

    bool CheckCursorInside();

    BoundingSquare BoundingBox { get; }
  }
}
