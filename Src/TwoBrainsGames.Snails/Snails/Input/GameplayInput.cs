
// Type: TwoBrainsGames.Snails.Input.GameplayInput
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Input;


namespace TwoBrainsGames.Snails.Input
{
  public class GameplayInput : InputBase
  {
    public GameplayInput.GamePlayButtons GameButtons;

    public event GameplayInput.InputEvent OnAfterUpdate;

    public int ScrollValue { get; protected set; }

    public Vector2 CameraMotionPosition { get; protected set; }

    public float PinchScale { get; protected set; }

    public Vector2 PinchCenterPosition { get; protected set; }

    public bool IsGameButtonSet(GameplayInput.GamePlayButtons button)
    {
      return (this.GameButtons & button) == button;
    }

    public bool IsActionClicked
    {
      get
      {
        return (this.GameButtons & GameplayInput.GamePlayButtons.ActionClicked) == GameplayInput.GamePlayButtons.ActionClicked;
      }
    }

    public bool IsStageStartSelected
    {
      get
      {
        return (this.GameButtons & GameplayInput.GamePlayButtons.StageStart) == GameplayInput.GamePlayButtons.StageStart;
      }
    }

    public bool IsActionDown
    {
      get
      {
        return (this.GameButtons & GameplayInput.GamePlayButtons.ActionDown) == GameplayInput.GamePlayButtons.ActionDown;
      }
    }

    public bool IsActionReleased
    {
      get
      {
        return (this.GameButtons & GameplayInput.GamePlayButtons.ActionReleased) == GameplayInput.GamePlayButtons.ActionReleased;
      }
    }

    public bool IsToolUpClicked
    {
      get
      {
        return (this.GameButtons & GameplayInput.GamePlayButtons.ToolUp) == GameplayInput.GamePlayButtons.ToolUp;
      }
    }

    public bool IsToolDownClicked
    {
      get
      {
        return (this.GameButtons & GameplayInput.GamePlayButtons.ToolDown) == GameplayInput.GamePlayButtons.ToolDown;
      }
    }

    public bool IsToolReleasedClicked
    {
      get
      {
        return (this.GameButtons & GameplayInput.GamePlayButtons.ToolReleased) == GameplayInput.GamePlayButtons.ToolReleased;
      }
    }

    public bool TimeWarpSelected
    {
      get
      {
        return (this.GameButtons & GameplayInput.GamePlayButtons.TimeWarp) == GameplayInput.GamePlayButtons.TimeWarp;
      }
    }

    public bool IsCameraUpPressed
    {
      get
      {
        return (this.GameButtons & GameplayInput.GamePlayButtons.CameraUp) == GameplayInput.GamePlayButtons.CameraUp;
      }
    }

    public bool IsCameraDownPressed
    {
      get
      {
        return (this.GameButtons & GameplayInput.GamePlayButtons.CameraDown) == GameplayInput.GamePlayButtons.CameraDown;
      }
    }

    public bool IsCameraLeftPressed
    {
      get
      {
        return (this.GameButtons & GameplayInput.GamePlayButtons.CameraLeft) == GameplayInput.GamePlayButtons.CameraLeft;
      }
    }

    public bool IsCameraRightPressed
    {
      get
      {
        return (this.GameButtons & GameplayInput.GamePlayButtons.CameraRight) == GameplayInput.GamePlayButtons.CameraRight;
      }
    }

    public bool CursorMotion
    {
      get
      {
        return (this.GameButtons & GameplayInput.GamePlayButtons.CursorMotion) == GameplayInput.GamePlayButtons.CursorMotion;
      }
    }

    public bool IsCursorUpPressed
    {
      get
      {
        return (this.GameButtons & GameplayInput.GamePlayButtons.CursorUp) == GameplayInput.GamePlayButtons.CursorUp;
      }
    }

    public bool IsCursorDownPressed
    {
      get
      {
        return (this.GameButtons & GameplayInput.GamePlayButtons.CursorDown) == GameplayInput.GamePlayButtons.CursorDown;
      }
    }

    public bool IsCursorLeftPressed
    {
      get
      {
        return (this.GameButtons & GameplayInput.GamePlayButtons.CursorLeft) == GameplayInput.GamePlayButtons.CursorLeft;
      }
    }

    public bool IsCursorRightPressed
    {
      get
      {
        return (this.GameButtons & GameplayInput.GamePlayButtons.CursorRight) == GameplayInput.GamePlayButtons.CursorRight;
      }
    }

    public bool ActionOpenDebugOptions
    {
      get
      {
        return (this.GameButtons & GameplayInput.GamePlayButtons.DebugInfo) == GameplayInput.GamePlayButtons.DebugInfo;
      }
    }

    public bool ActionStageEditor
    {
      get
      {
        return (this.GameButtons & GameplayInput.GamePlayButtons.StageEditor) == GameplayInput.GamePlayButtons.StageEditor;
      }
    }

    public bool ActionLoadCustomStage
    {
      get
      {
        return (this.GameButtons & GameplayInput.GamePlayButtons.LoadCustomStage) == GameplayInput.GamePlayButtons.LoadCustomStage;
      }
    }

    public bool ActionPause
    {
      get
      {
        return (this.GameButtons & GameplayInput.GamePlayButtons.Pause) == GameplayInput.GamePlayButtons.Pause;
      }
    }

    public bool CameraMotion
    {
      get
      {
        return (this.GameButtons & GameplayInput.GamePlayButtons.CameraMotion) == GameplayInput.GamePlayButtons.CameraMotion;
      }
    }

    public bool MapPanStarted
    {
      get
      {
        return (this.GameButtons & GameplayInput.GamePlayButtons.PanMapStarted) == GameplayInput.GamePlayButtons.PanMapStarted;
      }
    }

    public bool MapPanEnded
    {
      get
      {
        return (this.GameButtons & GameplayInput.GamePlayButtons.PanMapEnded) == GameplayInput.GamePlayButtons.PanMapEnded;
      }
    }

    public bool CloseTutorialSelected
    {
      get
      {
        return (this.GameButtons & GameplayInput.GamePlayButtons.CloseTutorial) == GameplayInput.GamePlayButtons.CloseTutorial;
      }
    }

    public bool RestartSelected
    {
      get
      {
        return (this.GameButtons & GameplayInput.GamePlayButtons.RestartStage) == GameplayInput.GamePlayButtons.RestartStage;
      }
    }

    public bool MapPinchStarted
    {
      get
      {
        return (this.GameButtons & GameplayInput.GamePlayButtons.PinchMapStarted) == GameplayInput.GamePlayButtons.PinchMapStarted;
      }
    }

    public bool MapPinchEnded
    {
      get
      {
        return (this.GameButtons & GameplayInput.GamePlayButtons.PinchMapEnded) == GameplayInput.GamePlayButtons.PinchMapEnded;
      }
    }

    public bool IsActionPressed
    {
      get
      {
        return (this.GameButtons & GameplayInput.GamePlayButtons.ActionPressed) == GameplayInput.GamePlayButtons.ActionPressed;
      }
    }

    public override void Update(BrainGameTime gameTime)
    {
      this.GameButtons = GameplayInput.GamePlayButtons.None;
      if (InputBase._mouse != null)
      {
        if (InputBase._mouse.ButtonClicked(TwoBrainsGames.BrainEngine.Input.MouseButtons.LeftButton))
        {
          this.GameButtons |= GameplayInput.GamePlayButtons.ActionClicked;
          this.GameButtons |= GameplayInput.GamePlayButtons.StageStart;
        }
        if (InputBase._mouse.ButtonPressed(TwoBrainsGames.BrainEngine.Input.MouseButtons.LeftButton))
          this.GameButtons |= GameplayInput.GamePlayButtons.ActionPressed;
        if (InputBase._mouse.ButtonDown(TwoBrainsGames.BrainEngine.Input.MouseButtons.LeftButton))
          this.GameButtons |= GameplayInput.GamePlayButtons.ActionDown;
        if (InputBase._mouse.ButtonClicked(TwoBrainsGames.BrainEngine.Input.MouseButtons.LeftButton))
          this.GameButtons |= GameplayInput.GamePlayButtons.ActionReleased;
        if (InputBase._mouse.ButtonDown(TwoBrainsGames.BrainEngine.Input.MouseButtons.RightButton))
          this.GameButtons |= GameplayInput.GamePlayButtons.PanMapStarted;
        if (InputBase._mouse.ButtonReleased(TwoBrainsGames.BrainEngine.Input.MouseButtons.RightButton))
          this.GameButtons |= GameplayInput.GamePlayButtons.PanMapEnded;
        this.ScrollValue = InputBase._mouse.GetScrollWheelValueTransformed();
        if (this.ScrollValue != 0)
        {
          if (this.ScrollValue == -1)
            this.GameButtons |= GameplayInput.GamePlayButtons.ToolUp;
          else if (this.ScrollValue == 1)
            this.GameButtons |= GameplayInput.GamePlayButtons.ToolDown;
        }
        this.GameButtons |= GameplayInput.GamePlayButtons.CursorMotion;
        Vector2 position = InputBase._mouse.GetPosition();
        this.MotionPosition = new Vector2(position.X / BrainGame.ViewportRatioX, position.Y / BrainGame.ViewportRatioY);
      }
      if (InputBase._gamepad != null)
      {
        if (InputBase._gamepad.IsButtonClicked(Buttons.A))
          this.GameButtons |= GameplayInput.GamePlayButtons.ActionClicked;
        if (InputBase._gamepad.IsButtonPressed(Buttons.A))
        {
          this.GameButtons |= GameplayInput.GamePlayButtons.ActionDown;
          this.GameButtons |= GameplayInput.GamePlayButtons.ActionPressed;
        }
        if (InputBase._gamepad.IsButtonReleased(Buttons.A))
          this.GameButtons |= GameplayInput.GamePlayButtons.ActionReleased;
        if (InputBase._gamepad.IsButtonClicked(Buttons.B))
          this.GameButtons |= GameplayInput.GamePlayButtons.CloseTutorial;
        if (InputBase._gamepad.IsButtonClicked(Buttons.LeftShoulder) || InputBase._gamepad.IsButtonClicked(Buttons.LeftTrigger))
          this.GameButtons |= GameplayInput.GamePlayButtons.ToolUp;
        if (InputBase._gamepad.IsButtonClicked(Buttons.RightShoulder) || InputBase._gamepad.IsButtonClicked(Buttons.RightTrigger))
          this.GameButtons |= GameplayInput.GamePlayButtons.ToolDown;
        if (InputBase._gamepad.IsButtonClicked(Buttons.X))
          this.GameButtons |= GameplayInput.GamePlayButtons.ToolReleased;
        if (InputBase._gamepad.IsButtonClicked(Buttons.Y))
          this.GameButtons |= GameplayInput.GamePlayButtons.TimeWarp;
        if (InputBase._gamepad.IsButtonClicked(Buttons.Back) || InputBase._gamepad.IsButtonClicked(Buttons.BigButton))
          this.GameButtons |= GameplayInput.GamePlayButtons.Pause;
        this.GameButtons |= GameplayInput.GamePlayButtons.CursorMotion;
        this.MotionPosition = InputBase._gamepad.GetLeftThumbStickPosition();
        if (this.MotionPosition != Vector2.Zero && (double) this.MotionPosition.Length() < 0.89999997615814209)
        {
          GameplayInput gameplayInput = this;
          gameplayInput.MotionPosition = gameplayInput.MotionPosition * 0.3f;
        }
        if (InputBase._gamepad.IsButtonClicked(Buttons.DPadUp))
        {
          this.GameButtons |= GameplayInput.GamePlayButtons.CursorUp;
          this.GameButtons &= ~GameplayInput.GamePlayButtons.CursorMotion;
        }
        if (InputBase._gamepad.IsButtonClicked(Buttons.DPadDown))
        {
          this.GameButtons |= GameplayInput.GamePlayButtons.CursorDown;
          this.GameButtons &= ~GameplayInput.GamePlayButtons.CursorMotion;
        }
        if (InputBase._gamepad.IsButtonClicked(Buttons.DPadLeft))
        {
          this.GameButtons |= GameplayInput.GamePlayButtons.CursorLeft;
          this.GameButtons &= ~GameplayInput.GamePlayButtons.CursorMotion;
        }
        if (InputBase._gamepad.IsButtonClicked(Buttons.DPadRight))
        {
          this.GameButtons |= GameplayInput.GamePlayButtons.CursorRight;
          this.GameButtons &= ~GameplayInput.GamePlayButtons.CursorMotion;
        }
        if (InputBase._gamepad.IsButtonClicked(Buttons.Start))
          this.GameButtons |= GameplayInput.GamePlayButtons.RestartStage;
        this.GameButtons |= GameplayInput.GamePlayButtons.CameraMotion;
        this.CameraMotionPosition = InputBase._gamepad.GetRightThumbStickPosition();
      }
      if (InputBase._keyboard != null)
      {
        if (InputBase._keyboard.IsKeyPressed(Keys.C))
          this.GameButtons |= GameplayInput.GamePlayButtons.ToolBox;
        if (InputBase._keyboard.IsKeyPressed(Keys.L))
          this.GameButtons |= GameplayInput.GamePlayButtons.ToolSalt;
        if (InputBase._keyboard.IsKeyPressed(Keys.R))
          this.GameButtons |= GameplayInput.GamePlayButtons.ToolVitamin;
        if (InputBase._keyboard.IsKeyPressed(Keys.T))
          this.GameButtons |= GameplayInput.GamePlayButtons.ToolTrampoline;
        if (InputBase._keyboard.IsKeyPressed(Keys.M))
          this.GameButtons |= GameplayInput.GamePlayButtons.ToolCopperBox;
        if (InputBase._keyboard.IsKeyPressed(Keys.B))
          this.GameButtons |= GameplayInput.GamePlayButtons.ToolDynamite;
        if (InputBase._keyboard.IsKeyPressed(Keys.P))
          this.GameButtons |= GameplayInput.GamePlayButtons.ToolApple;
        if (InputBase._keyboard.IsKeyPressed(Keys.K))
          this.GameButtons |= GameplayInput.GamePlayButtons.ToolTimedBox;
        if (InputBase._keyboard.IsKeyPressed(Keys.G))
          this.GameButtons |= GameplayInput.GamePlayButtons.ToolTriggerBox;
        if (InputBase._keyboard.IsKeyPressed(Keys.F9))
          this.GameButtons |= GameplayInput.GamePlayButtons.DebugInfo;
        if (InputBase._keyboard.IsKeyPressed(Keys.E))
          this.GameButtons |= GameplayInput.GamePlayButtons.StageEditor;
        if (InputBase._keyboard.IsKeyPressed(Keys.F3))
          this.GameButtons |= GameplayInput.GamePlayButtons.LoadCustomStage;
        if (InputBase._keyboard.IsKeyPressed(Keys.Space))
        {
          this.GameButtons |= GameplayInput.GamePlayButtons.TimeWarp;
          this.GameButtons |= GameplayInput.GamePlayButtons.CloseTutorial;
          this.GameButtons |= GameplayInput.GamePlayButtons.StageStart;
        }
        if (InputBase._keyboard.IsKeyPressed(Keys.Enter))
          this.GameButtons |= GameplayInput.GamePlayButtons.StageStart;
        if (InputBase._keyboard.IsKeyDown(Keys.Up) || InputBase._keyboard.IsKeyDown(Keys.W))
          this.GameButtons |= GameplayInput.GamePlayButtons.CameraUp;
        if (InputBase._keyboard.IsKeyDown(Keys.Down) || InputBase._keyboard.IsKeyDown(Keys.S))
          this.GameButtons |= GameplayInput.GamePlayButtons.CameraDown;
        if (InputBase._keyboard.IsKeyDown(Keys.Left) || InputBase._keyboard.IsKeyDown(Keys.A))
          this.GameButtons |= GameplayInput.GamePlayButtons.CameraLeft;
        if (InputBase._keyboard.IsKeyDown(Keys.Right) || InputBase._keyboard.IsKeyDown(Keys.D))
          this.GameButtons |= GameplayInput.GamePlayButtons.CameraRight;
        if (InputBase._keyboard.IsKeyDown(Keys.Escape))
          this.GameButtons |= GameplayInput.GamePlayButtons.Pause;
        if (InputBase._keyboard.IsKeyDown(Keys.Back))
          this.GameButtons |= GameplayInput.GamePlayButtons.RestartStage;
      }
      if (InputBase._touch != null)
      {
        this.MotionPosition = Vector2.Zero;
        if (InputBase._touch.CanGetPosition)
        {
          this.MotionPosition = InputBase._touch.GetTouchPosition();
          if (this.MotionPosition != Vector2.Zero)
            this.GameButtons |= GameplayInput.GamePlayButtons.CursorMotion;
        }
        if (InputBase._touch.IsTapping)
        {
          this.GameButtons |= GameplayInput.GamePlayButtons.ActionClicked;
          this.GameButtons |= GameplayInput.GamePlayButtons.ActionReleased;
          this.GameButtons |= GameplayInput.GamePlayButtons.StageStart;
        }
        if (InputBase._touch.IsDoubleTapping)
          this.GameButtons |= GameplayInput.GamePlayButtons.CloseUpMap;
        if (InputBase._touch.IsDown)
          this.GameButtons |= GameplayInput.GamePlayButtons.ActionDown;
        if (InputBase._touch.WasPressed)
          this.GameButtons |= GameplayInput.GamePlayButtons.ActionPressed;
        if (InputBase._touch.WasReleased)
        {
          this.GameButtons |= GameplayInput.GamePlayButtons.ActionReleased;
          this.GameButtons |= GameplayInput.GamePlayButtons.PanMapEnded;
        }
        if (InputBase._touch.StartDragging)
          this.GameButtons |= GameplayInput.GamePlayButtons.PanMapStarted;
        if (InputBase._touch.EndDragging)
          this.GameButtons |= GameplayInput.GamePlayButtons.PanMapEnded;
        this.PinchScale = 1f;
        if (InputBase._touch.StartPinching)
        {
          this.GameButtons |= GameplayInput.GamePlayButtons.PinchMapStarted;
          this.PinchScale = InputBase._touch.PinchDistance * 0.005f;
          this.PinchScale = InputBase._touch.PinchDistance / InputBase._touch.PinchPrevDistance;
          this.PinchCenterPosition = InputBase._touch.PinchCenterPosition;
        }
        if (InputBase._touch.EndPinching)
          this.GameButtons |= GameplayInput.GamePlayButtons.PinchMapEnded;
        if (Microsoft.Xna.Framework.Input.GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
          this.GameButtons |= GameplayInput.GamePlayButtons.Pause;
      }
      if (InputBase._mouse != null && InputBase._keyboard != null)
      {
        if (InputBase._mouse.ButtonDown(TwoBrainsGames.BrainEngine.Input.MouseButtons.LeftButton) && (InputBase._keyboard.IsKeyDown(Keys.LeftControl) || InputBase._keyboard.IsKeyDown(Keys.RightControl)))
          this.GameButtons |= GameplayInput.GamePlayButtons.PanMapStarted;
        if (InputBase._mouse.ButtonReleased(TwoBrainsGames.BrainEngine.Input.MouseButtons.LeftButton) && (InputBase._keyboard.IsKeyDown(Keys.LeftControl) || InputBase._keyboard.IsKeyDown(Keys.RightControl)))
          this.GameButtons |= GameplayInput.GamePlayButtons.PanMapEnded;
      }
      if (this.OnAfterUpdate == null)
        return;
      Vector2 newMotionPosition;
      this.OnAfterUpdate(this, gameTime, out newMotionPosition);
      this.MotionPosition = newMotionPosition;
    }

    public bool QueryActionDown(GameplayInput.GamePlayButtons action)
    {
      return (this.GameButtons & action) == action;
    }

    public override void Reset() => this.GameButtons = GameplayInput.GamePlayButtons.None;

    [Flags]
    public enum GamePlayButtons : ulong
    {
      None = 0,
      ActionClicked = 1,
      ActionDown = 2,
      ToolUp = 4,
      ToolDown = 8,
      ToolReleased = 16, // 0x0000000000000010
      TimeWarp = 64, // 0x0000000000000040
      CameraUp = 128, // 0x0000000000000080
      CameraDown = 256, // 0x0000000000000100
      CameraLeft = 512, // 0x0000000000000200
      CameraRight = 1024, // 0x0000000000000400
      CursorMotion = 2048, // 0x0000000000000800
      CursorUp = 4096, // 0x0000000000001000
      CursorDown = 8192, // 0x0000000000002000
      CursorLeft = 16384, // 0x0000000000004000
      CursorRight = 32768, // 0x0000000000008000
      DebugInfo = 65536, // 0x0000000000010000
      StageEditor = 131072, // 0x0000000000020000
      Pause = 262144, // 0x0000000000040000
      CameraMotion = 524288, // 0x0000000000080000
      ActionReleased = 1048576, // 0x0000000000100000
      ToolBox = 2097152, // 0x0000000000200000
      ToolSalt = 4194304, // 0x0000000000400000
      ToolVitamin = 8388608, // 0x0000000000800000
      ToolTrampoline = 16777216, // 0x0000000001000000
      ToolCopperBox = 33554432, // 0x0000000002000000
      ToolDynamite = 67108864, // 0x0000000004000000
      ToolApple = 134217728, // 0x0000000008000000
      ToolTimedBox = 268435456, // 0x0000000010000000
      PanMapStarted = 536870912, // 0x0000000020000000
      PanMapEnded = 1073741824, // 0x0000000040000000
      CloseTutorial = 2147483648, // 0x0000000080000000
      ToolTriggerBox = 4294967296, // 0x0000000100000000
      LoadCustomStage = 8589934592, // 0x0000000200000000
      RestartStage = 17179869184, // 0x0000000400000000
      PinchMapStarted = 68719476736, // 0x0000001000000000
      PinchMapEnded = 137438953472, // 0x0000002000000000
      CloseUpMap = 274877906944, // 0x0000004000000000
      ActionPressed = 549755813888, // 0x0000008000000000
      StageStart = 1099511627776, // 0x0000010000000000
    }

    public delegate void InputEvent(
      GameplayInput sender,
      BrainGameTime gameTime,
      out Vector2 newMotionPosition);
  }
}
