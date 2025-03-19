
// Type: TwoBrainsGames.BrainEngine.Input.ControllerInput
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;


namespace TwoBrainsGames.BrainEngine.Input
{
  public class ControllerInput
  {
    private bool _allowVibration;
    private PlayerIndex _playerIndex;
    private KeyboardState _keyboardState;
    private KeyboardState _lastKeyboardState;
    private Dictionary<Buttons, Keys> _keyboardMap;
    private GamePadState _gamePadState;
    private GamePadState _lastGamePadState;

    public bool AllowVibration
    {
      get => this._allowVibration;
      set => this._allowVibration = value;
    }

    public ControllerInput(PlayerIndex playerIndex)
      : this(playerIndex, (Dictionary<Buttons, Keys>) null)
    {
    }

    public ControllerInput(PlayerIndex playerIndex, Dictionary<Buttons, Keys> keyboardMap)
    {
      this._playerIndex = playerIndex;
      this._keyboardMap = keyboardMap;
      this._allowVibration = true;
    }

    public void Update()
    {
      this._lastKeyboardState = this._keyboardState;
      this._keyboardState = Keyboard.GetState(this._playerIndex);
      this._lastGamePadState = this._gamePadState;
      this._gamePadState = GamePad.GetState(this._playerIndex);
    }

    public bool IsKeyPressed(Buttons button)
    {
      bool flag = false;
      if (this._gamePadState.IsConnected)
        flag = this._gamePadState.IsButtonDown(button) && this._lastGamePadState.IsButtonUp(button);
      else if (this._keyboardMap != null)
      {
        Keys keyboard = this._keyboardMap[button];
        flag = this._keyboardState.IsKeyDown(keyboard) && this._lastKeyboardState.IsKeyUp(keyboard);
      }
      return flag;
    }

    public bool TestKeyPressed(Buttons button)
    {
      bool flag = false;
      if (this._gamePadState.IsConnected)
        flag = this._gamePadState.IsButtonDown(button);
      else if (this._keyboardMap != null)
        flag = this._keyboardState.IsKeyDown(this._keyboardMap[button]);
      return flag;
    }

    public bool IsKeyReleased(Buttons button)
    {
      bool flag = false;
      if (this._gamePadState.IsConnected)
        flag = this._gamePadState.IsButtonUp(button) && this._lastGamePadState.IsButtonDown(button);
      else if (this._keyboardMap != null)
      {
        Keys keyboard = this._keyboardMap[button];
        flag = this._keyboardState.IsKeyUp(keyboard) && this._lastKeyboardState.IsKeyDown(keyboard);
      }
      return flag;
    }

    public Vector2 GetLeftThumbStickPosition()
    {
      Vector2 thumbStickPosition = Vector2.Zero;
      if (this._gamePadState.IsConnected)
        thumbStickPosition = this._gamePadState.ThumbSticks.Left;
      else if (this._keyboardMap != null)
      {
        if (this._keyboardState.IsKeyDown(this._keyboardMap[Buttons.LeftThumbstickUp]))
          thumbStickPosition.Y = 1f;
        else if (this._keyboardState.IsKeyDown(this._keyboardMap[Buttons.LeftThumbstickDown]))
          thumbStickPosition.Y = -1f;
        if (this._keyboardState.IsKeyDown(this._keyboardMap[Buttons.LeftThumbstickRight]))
          thumbStickPosition.X = 1f;
        else if (this._keyboardState.IsKeyDown(this._keyboardMap[Buttons.LeftThumbstickLeft]))
          thumbStickPosition.X = -1f;
      }
      return thumbStickPosition;
    }

    public Vector2 GetRightThumbStickPosition()
    {
      Vector2 thumbStickPosition = Vector2.Zero;
      if (this._gamePadState.IsConnected)
        thumbStickPosition = this._gamePadState.ThumbSticks.Right;
      else if (this._keyboardMap != null)
      {
        if (this._keyboardState.IsKeyDown(this._keyboardMap[Buttons.RightThumbstickUp]))
          thumbStickPosition.Y = 1f;
        else if (this._keyboardState.IsKeyDown(this._keyboardMap[Buttons.RightThumbstickDown]))
          thumbStickPosition.Y = -1f;
        if (this._keyboardState.IsKeyDown(this._keyboardMap[Buttons.RightThumbstickRight]))
          thumbStickPosition.X = 1f;
        else if (this._keyboardState.IsKeyDown(this._keyboardMap[Buttons.RightThumbstickLeft]))
          thumbStickPosition.X = -1f;
      }
      return thumbStickPosition;
    }

    public bool StopVibration() => this.SetVibration(0.0f, 0.0f);

    public bool SetVibration(float leftMotor, float rightMotor)
    {
      return this._allowVibration && this._gamePadState.IsConnected && GamePad.SetVibration(this._playerIndex, leftMotor, rightMotor);
    }
  }
}
