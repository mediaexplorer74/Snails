
// Type: TwoBrainsGames.BrainEngine.Input.KeyboardInput
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace TwoBrainsGames.BrainEngine.Input
{
  public class KeyboardInput : GameComponent
  {
    private KeyboardState PreviousState { get; set; }

    private KeyboardState CurrentState { get; set; }

    public bool IsControlDown
    {
      get
      {
        return this.CurrentState.IsKeyDown(Keys.LeftControl) || this.CurrentState.IsKeyDown(Keys.RightControl);
      }
    }

    public bool IsShiftDown
    {
      get
      {
        return this.CurrentState.IsKeyDown(Keys.LeftShift) || this.CurrentState.IsKeyDown(Keys.RightShift);
      }
    }

    public KeyboardInput()
      : base((Game) BrainGame.Instance)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.CurrentState = Keyboard.GetState();
    }

    public override void Update(GameTime gameTime)
    {
      this.PreviousState = this.CurrentState;
      this.CurrentState = Keyboard.GetState();
    }

    public Keys[] GetPressedKeys() => this.CurrentState.GetPressedKeys();

    public bool IsKeyDown(Keys key) => this.CurrentState.IsKeyDown(key);

    public bool IsKeyPressed(Keys key)
    {
      return this.PreviousState.IsKeyUp(key) && this.CurrentState.IsKeyDown(key);
    }

    public bool IsKeyReleased(Keys key)
    {
      return this.PreviousState.IsKeyDown(key) && this.CurrentState.IsKeyUp(key);
    }
  }
}
