
// Type: TwoBrainsGames.BrainEngine.Input.MouseInput
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace TwoBrainsGames.BrainEngine.Input
{
  public class MouseInput : GameComponent
  {
    private MouseState PreviousState { get; set; }

    private MouseState CurrentState { get; set; }

    public MouseInput()
      : base((Game) BrainGame.Instance)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.CurrentState = Mouse.GetState();
    }

    public override void Update(GameTime gameTime)
    {
      this.PreviousState = this.CurrentState;
      this.CurrentState = Mouse.GetState();
    }

    public int GetPositionX() => this.CurrentState.X;

    public int GetPositionY() => this.CurrentState.Y;

    public Vector2 GetPosition()
    {
      return new Vector2((float) this.CurrentState.X, (float) this.CurrentState.Y);
    }

    public Vector2 GetUpdatedPosition()
    {
      this.CurrentState = Mouse.GetState();
      return new Vector2((float) this.CurrentState.X, (float) this.CurrentState.Y);
    }

    public int GetScrollWheelValue() => this.CurrentState.ScrollWheelValue;

    public int GetScrollWheelValueTransformed()
    {
      return (this.CurrentState.ScrollWheelValue - this.PreviousState.ScrollWheelValue) / 120 * -1;
    }

    public bool ButtonClicked(MouseButtons button)
    {
      switch (button)
      {
        case MouseButtons.LeftButton:
          if (this.PreviousState.LeftButton == ButtonState.Pressed && this.CurrentState.LeftButton == ButtonState.Released)
            return true;
          break;
        case MouseButtons.MiddleButton:
          if (this.PreviousState.MiddleButton == ButtonState.Pressed && this.CurrentState.MiddleButton == ButtonState.Released)
            return true;
          break;
        case MouseButtons.RightButton:
          if (this.PreviousState.RightButton == ButtonState.Pressed && this.CurrentState.RightButton == ButtonState.Released)
            return true;
          break;
      }
      return false;
    }

    public bool ButtonDown(MouseButtons button)
    {
      switch (button)
      {
        case MouseButtons.LeftButton:
          if (this.CurrentState.LeftButton == ButtonState.Pressed)
            return true;
          break;
        case MouseButtons.MiddleButton:
          if (this.CurrentState.MiddleButton == ButtonState.Pressed)
            return true;
          break;
        case MouseButtons.RightButton:
          if (this.CurrentState.RightButton == ButtonState.Pressed)
            return true;
          break;
      }
      return false;
    }

    public bool ButtonPressed(MouseButtons button)
    {
      switch (button)
      {
        case MouseButtons.LeftButton:
          if (this.CurrentState.LeftButton == ButtonState.Pressed && this.PreviousState.LeftButton == ButtonState.Released)
            return true;
          break;
        case MouseButtons.MiddleButton:
          if (this.CurrentState.MiddleButton == ButtonState.Pressed && this.PreviousState.MiddleButton == ButtonState.Released)
            return true;
          break;
        case MouseButtons.RightButton:
          if (this.CurrentState.RightButton == ButtonState.Pressed && this.PreviousState.RightButton == ButtonState.Released)
            return true;
          break;
      }
      return false;
    }

    public bool ButtonReleased(MouseButtons button)
    {
      switch (button)
      {
        case MouseButtons.LeftButton:
          if (this.PreviousState.LeftButton == ButtonState.Pressed && this.CurrentState.LeftButton == ButtonState.Released)
            return true;
          break;
        case MouseButtons.MiddleButton:
          if (this.PreviousState.MiddleButton == ButtonState.Pressed && this.CurrentState.MiddleButton == ButtonState.Released)
            return true;
          break;
        case MouseButtons.RightButton:
          if (this.PreviousState.RightButton == ButtonState.Pressed && this.CurrentState.RightButton == ButtonState.Released)
            return true;
          break;
      }
      return false;
    }

    public void SetPosition(int x, int y) => Mouse.SetPosition(x, y);
  }
}
