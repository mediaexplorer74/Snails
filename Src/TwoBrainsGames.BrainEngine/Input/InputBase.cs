
// Type: TwoBrainsGames.BrainEngine.Input.InputBase
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;


namespace TwoBrainsGames.BrainEngine.Input
{
  public class InputBase
  {
    public InputBase.InputActions Actions;
    public static InputBase _instance;
    protected static MouseInput _mouse;
    protected static KeyboardInput _keyboard;
    protected bool _checkForController;
    protected static GamePadInput _gamepad;
    public static TouchInput _touch;

    public static InputBase Current => InputBase._instance;

    public MouseInput Mouse => InputBase._mouse;

    public KeyboardInput Keyboard => InputBase._keyboard;

    public GamePadInput GamePad => InputBase._gamepad;

    public Vector2 PrevMotionPosition { get; protected set; }

    public Vector2 MotionPosition { get; protected set; }

    public Vector2 AuxiliaryMotionPosition { get; protected set; }

    public Vector2 FlickDelta { get; protected set; }

    public TouchInput Touch => InputBase._touch;

    public bool ActionAccept
    {
      get => (this.Actions & InputBase.InputActions.Accept) == InputBase.InputActions.Accept;
    }

    public bool ActionAcceptDown
    {
      get
      {
        return (this.Actions & InputBase.InputActions.AcceptDown) == InputBase.InputActions.AcceptDown;
      }
    }

    public bool ActionAcceptUp
    {
      get => (this.Actions & InputBase.InputActions.AcceptUp) == InputBase.InputActions.AcceptUp;
    }

    public bool ActionBack
    {
      get => (this.Actions & InputBase.InputActions.Back) == InputBase.InputActions.Back;
    }

    public bool ActionUp
    {
      get => (this.Actions & InputBase.InputActions.CursorUp) == InputBase.InputActions.CursorUp;
    }

    public bool ActionDown
    {
      get
      {
        return (this.Actions & InputBase.InputActions.CursorDown) == InputBase.InputActions.CursorDown;
      }
    }

    public bool ActionLeft
    {
      get
      {
        return (this.Actions & InputBase.InputActions.CursorLeft) == InputBase.InputActions.CursorLeft;
      }
    }

    public bool ActionRight
    {
      get
      {
        return (this.Actions & InputBase.InputActions.CursorRight) == InputBase.InputActions.CursorRight;
      }
    }

    public bool ActionCancel
    {
      get => (this.Actions & InputBase.InputActions.Cancel) == InputBase.InputActions.Cancel;
    }

    public bool ActionUpClicked
    {
      get
      {
        return (this.Actions & InputBase.InputActions.CursorUpClicked) == InputBase.InputActions.CursorUpClicked;
      }
    }

    public bool ActionDownClicked
    {
      get
      {
        return (this.Actions & InputBase.InputActions.CursorDownClicked) == InputBase.InputActions.CursorDownClicked;
      }
    }

    public bool ActionLeftClicked
    {
      get
      {
        return (this.Actions & InputBase.InputActions.CursorLeftClicked) == InputBase.InputActions.CursorLeftClicked;
      }
    }

    public bool ActionRightClicked
    {
      get
      {
        return (this.Actions & InputBase.InputActions.CursorRightClicked) == InputBase.InputActions.CursorRightClicked;
      }
    }

    public bool ActionLeftReleased
    {
      get
      {
        return (this.Actions & InputBase.InputActions.CursorLeftReleased) == InputBase.InputActions.CursorLeftReleased;
      }
    }

    public bool ActionRightReleased
    {
      get
      {
        return (this.Actions & InputBase.InputActions.CursorRightReleased) == InputBase.InputActions.CursorRightReleased;
      }
    }

    public bool ActionStart
    {
      get => (this.Actions & InputBase.InputActions.Start) == InputBase.InputActions.Start;
    }

    public bool ActionFreeDrag
    {
      get => (this.Actions & InputBase.InputActions.FreeDrag) == InputBase.InputActions.FreeDrag;
    }

    public bool ActionHorizontalDrag
    {
      get
      {
        return (this.Actions & InputBase.InputActions.HorizontalDrag) == InputBase.InputActions.HorizontalDrag;
      }
    }

    public bool ActionVerticalDrag
    {
      get
      {
        return (this.Actions & InputBase.InputActions.VerticalDrag) == InputBase.InputActions.VerticalDrag;
      }
    }

    public bool ActionDragComplete
    {
      get
      {
        return (this.Actions & InputBase.InputActions.DragComplete) == InputBase.InputActions.DragComplete;
      }
    }

    public bool ActionFlick
    {
      get => (this.Actions & InputBase.InputActions.Flick) == InputBase.InputActions.Flick;
    }

    public bool ActionNext
    {
      get => (this.Actions & InputBase.InputActions.Next) == InputBase.InputActions.Next;
    }

    public bool ActionPrev
    {
      get => (this.Actions & InputBase.InputActions.Prev) == InputBase.InputActions.Prev;
    }

    public bool ActionDoubleTap
    {
      get => (this.Actions & InputBase.InputActions.DoubleTap) == InputBase.InputActions.DoubleTap;
    }

    public bool ActionNone => this.Actions == InputBase.InputActions.None;

    public double TimeIdleMsecs { get; private set; }

    private bool PreviousMotionPointerDown { get; set; }

    private bool MotionPointerDown { get; set; }

    public bool IsMotionPointerDown => this.MotionPointerDown;

    public bool WasMotionPointerDown => this.PreviousMotionPointerDown;

    public PlayerIndex ControllerIndex
    {
      get => this.GamePad != null ? this.GamePad.PlayerIndex : PlayerIndex.One;
    }

    public bool WithMouse => InputBase._mouse != null;

    public InputBase() => InputBase._instance = this;

    public void Initialize()
    {
      this.MotionPointerDown = true;
      this.PreviousMotionPointerDown = true;

      if (true)//(BrainGame.Settings.UseMouse && InputBase._mouse == null)
      {
                BrainGame.Settings.UseMouse = true; // RnD (TEMP)
        InputBase._mouse = new MouseInput();
        BrainGame.AddComponent((GameComponent) InputBase._mouse);
      }

      if (true)// (BrainGame.Settings.UseKeyboard && InputBase._keyboard == null)
      {
        InputBase._keyboard = new KeyboardInput();
        BrainGame.AddComponent((GameComponent) InputBase._keyboard);
      }

      if (BrainGame.Settings.UseGamepad && InputBase._gamepad == null)
      {
        InputBase._gamepad = new GamePadInput(BrainGame.CurrentControllerIndex);
        BrainGame.AddComponent((GameComponent) InputBase._gamepad);
      }

      if (!BrainGame.Settings.UseTouch || InputBase._touch != null)
        return;

      InputBase._touch = new TouchInput();
      BrainGame.AddComponent((GameComponent) InputBase._touch);
      this.MotionPointerDown = false;
      this.PreviousMotionPointerDown = true;
    }

    public virtual void Reset() => this.Actions = InputBase.InputActions.None;

    public virtual void ResetTimeIdle() => this.TimeIdleMsecs = 0.0;

    public virtual void Update(BrainGameTime gameTime)
    {
      this.Reset();
      if (InputBase._mouse != null)
      {
        this.Actions &= ~InputBase.InputActions.AcceptUp;
        if (InputBase._mouse.ButtonClicked(MouseButtons.LeftButton))
          this.Actions |= InputBase.InputActions.Accept;
        if (InputBase._mouse.ButtonDown(MouseButtons.LeftButton))
        {
          this.Actions |= InputBase.InputActions.AcceptDown;
          this.Actions |= InputBase.InputActions.Start;
        }
        if (InputBase._mouse.ButtonReleased(MouseButtons.LeftButton))
        {
          this.Actions &= ~InputBase.InputActions.AcceptDown;
          this.Actions |= InputBase.InputActions.AcceptUp;
        }
        if (InputBase._mouse.ButtonDown(MouseButtons.RightButton))
          this.Actions |= InputBase.InputActions.Start;
        Vector2 position = InputBase._mouse.GetPosition();
        this.MotionPosition = this.AuxiliaryMotionPosition = new Vector2(position.X / BrainGame.ViewportRatioX, position.Y / BrainGame.ViewportRatioY);
        if (this.MotionPosition != this.PrevMotionPosition)
        {
          this.Actions |= InputBase.InputActions.CursorMotion;
          this.PrevMotionPosition = this.MotionPosition;
        }
      }
      if (InputBase._gamepad != null)
      {
        if (InputBase._gamepad.IsButtonClicked(Buttons.A))
          this.Actions |= InputBase.InputActions.Accept;
        if (InputBase._gamepad.IsButtonClicked(Buttons.X))
          this.Actions |= InputBase.InputActions.Cancel;
        if (InputBase._gamepad.IsButtonClicked(Buttons.Y))
          this.Actions |= InputBase.InputActions.Cancel;
        if (InputBase._gamepad.IsButtonClicked(Buttons.B) || InputBase._gamepad.IsButtonClicked(Buttons.Back))
          this.Actions |= InputBase.InputActions.Back;
        this.MotionPosition = InputBase._gamepad.GetLeftThumbStickPosition();
        this.AuxiliaryMotionPosition = InputBase._gamepad.GetRightThumbStickPosition();
        if (this.MotionPosition != Vector2.Zero || this.AuxiliaryMotionPosition != Vector2.Zero)
          this.Actions |= InputBase.InputActions.CursorMotion;
        if (InputBase._gamepad.IsButtonPressed(Buttons.DPadUp))
        {
          this.Actions |= InputBase.InputActions.CursorUp;
          this.Actions &= ~InputBase.InputActions.CursorMotion;
        }
        if (InputBase._gamepad.IsButtonPressed(Buttons.DPadDown))
        {
          this.Actions |= InputBase.InputActions.CursorDown;
          this.Actions &= ~InputBase.InputActions.CursorMotion;
        }
        if (InputBase._gamepad.IsButtonPressed(Buttons.DPadLeft))
        {
          this.Actions |= InputBase.InputActions.CursorLeft;
          this.Actions &= ~InputBase.InputActions.CursorMotion;
        }
        if (InputBase._gamepad.IsButtonPressed(Buttons.DPadRight))
        {
          this.Actions |= InputBase.InputActions.CursorRight;
          this.Actions &= ~InputBase.InputActions.CursorMotion;
        }
        if (InputBase._gamepad.IsButtonClicked(Buttons.DPadUp))
        {
          this.Actions |= InputBase.InputActions.CursorUpClicked;
          this.Actions &= ~InputBase.InputActions.CursorMotion;
        }
        if (InputBase._gamepad.IsButtonClicked(Buttons.DPadDown))
        {
          this.Actions |= InputBase.InputActions.CursorDownClicked;
          this.Actions &= ~InputBase.InputActions.CursorMotion;
        }
        if (InputBase._gamepad.IsButtonClicked(Buttons.DPadLeft))
        {
          this.Actions |= InputBase.InputActions.CursorLeftClicked;
          this.Actions &= ~InputBase.InputActions.CursorMotion;
        }
        if (InputBase._gamepad.IsButtonClicked(Buttons.DPadRight))
        {
          this.Actions |= InputBase.InputActions.CursorRightClicked;
          this.Actions &= ~InputBase.InputActions.CursorMotion;
        }
        if (InputBase._gamepad.IsButtonReleased(Buttons.DPadLeft))
        {
          this.Actions |= InputBase.InputActions.CursorLeftReleased;
          this.Actions &= ~InputBase.InputActions.CursorMotion;
        }
        if (InputBase._gamepad.IsButtonReleased(Buttons.DPadRight))
        {
          this.Actions |= InputBase.InputActions.CursorRightReleased;
          this.Actions &= ~InputBase.InputActions.CursorMotion;
        }
        if (InputBase._gamepad.IsButtonPressed(Buttons.Start))
          this.Actions |= InputBase.InputActions.Start;
        if (InputBase._gamepad.IsButtonReleased(Buttons.LeftShoulder) || InputBase._gamepad.IsButtonReleased(Buttons.LeftTrigger))
          this.Actions |= InputBase.InputActions.Prev;
        if (InputBase._gamepad.IsButtonReleased(Buttons.RightShoulder) || InputBase._gamepad.IsButtonReleased(Buttons.RightTrigger))
          this.Actions |= InputBase.InputActions.Next;
        if (InputBase._gamepad.IsDisconnected && !this._checkForController)
          this._checkForController = true;
        if (this._checkForController)
        {
          int num = this.GamePad.IsControllerConnected(BrainGame.CurrentControllerIndex);
          if (num != -1)
          {
            this.GamePad.PlayerIndex = (PlayerIndex) num;
            BrainGame.CurrentControllerIndex = this.GamePad.PlayerIndex;
            this._checkForController = false;
          }
        }
      }
      if (InputBase._keyboard != null)
      {
        if (InputBase._keyboard.IsKeyPressed(Keys.Enter))
          this.Actions |= InputBase.InputActions.Accept;
        if (InputBase._keyboard.IsKeyPressed(Keys.Up))
          this.Actions |= InputBase.InputActions.CursorUpClicked;
        if (InputBase._keyboard.IsKeyPressed(Keys.Down))
          this.Actions |= InputBase.InputActions.CursorDownClicked;
        if (InputBase._keyboard.IsKeyPressed(Keys.Left))
          this.Actions |= InputBase.InputActions.CursorLeftClicked;
        if (InputBase._keyboard.IsKeyPressed(Keys.Right))
          this.Actions |= InputBase.InputActions.CursorRightClicked;
        if (InputBase._keyboard.IsKeyPressed(Keys.Back))
          this.Actions |= InputBase.InputActions.Back;
        if (InputBase._keyboard.IsKeyPressed(Keys.Escape))
        {
          this.Actions |= InputBase.InputActions.Cancel;
          this.Actions |= InputBase.InputActions.Back;
        }
        if (InputBase._keyboard.IsKeyDown(Keys.Up))
          this.Actions |= InputBase.InputActions.CursorUp;
        if (InputBase._keyboard.IsKeyDown(Keys.Down))
          this.Actions |= InputBase.InputActions.CursorDown;
        if (InputBase._keyboard.IsKeyDown(Keys.Left))
          this.Actions |= InputBase.InputActions.CursorLeft;
        if (InputBase._keyboard.IsKeyDown(Keys.Right))
          this.Actions |= InputBase.InputActions.CursorRight;
        if (InputBase._keyboard.IsKeyReleased(Keys.Left))
          this.Actions |= InputBase.InputActions.CursorLeftReleased;
        if (InputBase._keyboard.IsKeyReleased(Keys.Right))
          this.Actions |= InputBase.InputActions.CursorRightReleased;
      }
      if (InputBase._touch != null)
      {
        this.PreviousMotionPointerDown = this.MotionPointerDown;
        this.FlickDelta = Vector2.Zero;
        this.MotionPosition = Vector2.Zero;
        if (InputBase._touch.CanGetPosition)
        {
          this.MotionPosition = InputBase._touch.GetTouchPosition();
          if (this.MotionPosition != Vector2.Zero)
            this.Actions |= InputBase.InputActions.CursorMotion;
        }
        if (InputBase._touch.WasPressed || InputBase._touch.HasMoved)
        {
          this.MotionPointerDown = true;
          this.Actions |= InputBase.InputActions.AcceptDown;
        }
        if (InputBase._touch.WasReleased)
        {
          this.MotionPointerDown = false;
          this.Actions &= ~InputBase.InputActions.AcceptDown;
          this.Actions |= InputBase.InputActions.AcceptUp;
          this.Actions |= InputBase.InputActions.DragComplete;
        }
        if (InputBase._touch.IsTapping)
        {
          this.Actions |= InputBase.InputActions.Accept;
          this.Actions |= InputBase.InputActions.AcceptUp;
        }
        if (InputBase._touch.IsDoubleTapping)
        {
          this.Actions |= InputBase.InputActions.DoubleTap;
          this.Actions |= InputBase.InputActions.Start;
        }
        if (Microsoft.Xna.Framework.Input.GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
        {
          this.Actions |= InputBase.InputActions.Cancel;
          this.Actions |= InputBase.InputActions.Back;
        }
        if (InputBase._touch.StartDragging)
        {
          this.Actions |= InputBase.InputActions.FreeDrag;
          if (InputBase._touch.IsHorizontalDrag)
            this.Actions |= InputBase.InputActions.HorizontalDrag;
          if (InputBase._touch.IsVerticalDrag)
            this.Actions |= InputBase.InputActions.VerticalDrag;
        }
        if (InputBase._touch.EndDragging)
          this.Actions |= InputBase.InputActions.DragComplete;
        if (InputBase._touch.HasFlick)
        {
          this.Actions |= InputBase.InputActions.Flick;
          this.FlickDelta = InputBase._touch.FlickDelta;
        }
      }
      if (this.Actions == InputBase.InputActions.None)
        this.TimeIdleMsecs += gameTime.ElapsedRealTime.TotalMilliseconds;
      else
        this.TimeIdleMsecs = 0.0;
    }

    public void SetMotionPosition(Vector2 pos)
    {
      if (!BrainGame.Instance.IsActive || this.Mouse == null)
        return;
      this.Mouse.SetPosition((int) pos.X, (int) pos.Y);
      this.MotionPosition = pos;
    }

    public Vector2 GetMotionPosition()
    {
      return this.Mouse != null ? InputBase._mouse.GetUpdatedPosition() : new Vector2(0.0f, 0.0f);
    }

    public bool CheckActionStartPressed()
    {
      if (this.GamePad != null)
      {
        int num = this.GamePad.CheckActionStartPressed();
        if (num != -1)
        {
          this.GamePad.PlayerIndex = (PlayerIndex) num;
          BrainGame.CurrentControllerIndex = this.GamePad.PlayerIndex;
        }
        return num != -1;
      }
      return this.ActionStart || this.ActionAccept;
    }

    public bool SkipAny() => this.GamePad != null && this.GamePad.SkipAny();

    [Flags]
    public enum InputActions
    {
      None = 0,
      Accept = 1,
      Back = 2,
      Cancel = 64, // 0x00000040
      CursorMotion = 128, // 0x00000080
      CursorUp = 256, // 0x00000100
      CursorDown = 512, // 0x00000200
      CursorLeft = 1024, // 0x00000400
      CursorRight = 2048, // 0x00000800
      CursorUpClicked = 4096, // 0x00001000
      CursorDownClicked = 8192, // 0x00002000
      CursorLeftClicked = 16384, // 0x00004000
      CursorRightClicked = 32768, // 0x00008000
      AcceptDown = 65536, // 0x00010000
      AcceptUp = 131072, // 0x00020000
      CursorLeftReleased = 262144, // 0x00040000
      CursorRightReleased = 524288, // 0x00080000
      Start = 1048576, // 0x00100000
      FreeDrag = 2097152, // 0x00200000
      HorizontalDrag = 4194304, // 0x00400000
      VerticalDrag = 8388608, // 0x00800000
      DragComplete = 16777216, // 0x01000000
      Flick = 33554432, // 0x02000000
      Next = 67108864, // 0x04000000
      Prev = 134217728, // 0x08000000
      DoubleTap = 268435456, // 0x10000000
    }
  }
}
