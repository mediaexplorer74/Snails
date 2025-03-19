
// Type: TwoBrainsGames.BrainEngine.Input.GamePadInput
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;


namespace TwoBrainsGames.BrainEngine.Input
{
  public class GamePadInput : GameComponent
  {
    private GamePadState PreviousState { get; set; }

    private GamePadState CurrentState { get; set; }

    public PlayerIndex PlayerIndex { get; set; }

    public bool AllowVibration { get; set; }

    public bool IsConnected => this.CurrentState.IsConnected;

    public bool IsDisconnected => !this.IsConnected;

    public GamePadInput()
      : base((Game) BrainGame.Instance)
    {
      this.PlayerIndex = PlayerIndex.One;
    }

    public GamePadInput(PlayerIndex _playerIndex)
      : base((Game) BrainGame.Instance)
    {
      this.PlayerIndex = _playerIndex;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.CurrentState = GamePad.GetState(this.PlayerIndex);
    }

    public override void Update(GameTime gameTime)
    {
      this.PreviousState = this.CurrentState;
      this.CurrentState = GamePad.GetState(this.PlayerIndex);
    }

    public bool IsButtonClicked(Buttons button)
    {
      bool flag = false;
      if (this.CurrentState.IsConnected)
        flag = this.CurrentState.IsButtonDown(button) && this.PreviousState.IsButtonUp(button);
      return flag;
    }

    public bool IsButtonPressed(Buttons button)
    {
      bool flag = false;
      if (this.CurrentState.IsConnected)
        flag = this.CurrentState.IsButtonDown(button);
      return flag;
    }

    public bool IsButtonReleased(Buttons button)
    {
      bool flag = false;
      if (this.CurrentState.IsConnected)
        flag = this.CurrentState.IsButtonUp(button) && this.PreviousState.IsButtonDown(button);
      return flag;
    }

    public Vector2 GetLeftThumbStickPosition()
    {
      Vector2 thumbStickPosition = Vector2.Zero;
      if (this.CurrentState.IsConnected)
        thumbStickPosition = this.CurrentState.ThumbSticks.Left;
      return thumbStickPosition;
    }

    public Vector2 GetRightThumbStickPosition()
    {
      Vector2 thumbStickPosition = Vector2.Zero;
      if (this.CurrentState.IsConnected)
        thumbStickPosition = this.CurrentState.ThumbSticks.Right;
      return thumbStickPosition;
    }

    public bool StopVibration() => this.SetVibration(0.0f, 0.0f);

    public bool SetVibration(float leftMotor, float rightMotor)
    {
      return this.AllowVibration && this.CurrentState.IsConnected && GamePad.SetVibration(this.PlayerIndex, leftMotor, rightMotor);
    }

    public int CheckActionStartPressed()
    {
      for (PlayerIndex playerIndex = PlayerIndex.One; playerIndex <= PlayerIndex.Four; ++playerIndex)
      {
        if (GamePad.GetState(playerIndex).Buttons.Start == ButtonState.Pressed)
          return (int) playerIndex;
      }
      return -1;
    }

    public bool SkipAny()
    {
      for (PlayerIndex playerIndex = PlayerIndex.One; playerIndex <= PlayerIndex.Four; ++playerIndex)
      {
        if (GamePad.GetState(playerIndex).Buttons.A == ButtonState.Pressed)
          return true;
      }
      return false;
    }

    public int IsControllerConnected(PlayerIndex playerIndex)
    {
      if (GamePad.GetState(playerIndex).IsConnected)
        return (int) playerIndex;
      for (PlayerIndex playerIndex1 = PlayerIndex.One; playerIndex1 <= PlayerIndex.Four; ++playerIndex1)
      {
        if (GamePad.GetState(playerIndex1).IsConnected)
          return (int) playerIndex1;
      }
      return -1;
    }
  }
}
