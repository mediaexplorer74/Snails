
// Type: TwoBrainsGames.BrainEngine.UI.Cursor
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Reflection;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.BrainEngine.UI
{
  public class Cursor
  {
    public Vector2 Position { get; set; }

    public virtual bool Visible { get; set; }

    public virtual void SetCursor(int id)
    {
    }

    public virtual void LoadCursor(string resourceName, int id)
    {
    }

    public virtual void LoadCursor(Assembly assembly, string resourceName, int id)
    {
    }

    public virtual void Update(BrainGameTime gameTime)
    {
      if (BrainGame.Settings.UseTouch)
        this.Position = BrainGame.ScreenNavigator.InputController.MotionPosition;
      if (!BrainGame.Settings.UseMouse || !(BrainGame.ScreenNavigator.ActiveScreen is UIScreen) || ((UIScreen) BrainGame.ScreenNavigator.ActiveScreen).CursorMode == CursorModes.SnapToControl)
        return;
      this.Position = BrainGame.ScreenNavigator.InputController.MotionPosition;
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
    }
  }
}
