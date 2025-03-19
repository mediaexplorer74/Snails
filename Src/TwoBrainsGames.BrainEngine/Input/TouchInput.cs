
// Type: TwoBrainsGames.BrainEngine.Input.TouchInput
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input.Touch;


namespace TwoBrainsGames.BrainEngine.Input
{
  public class TouchInput : GameComponent
  {
    private GestureType _gestureType;
    private Vector2 _flickDelta;
    private float _pinchDistance;
    private float _pinchPrevDistance;
    private Vector2 _pinchCenterPosition;
    private TouchLocation _lastLocation;
    private TouchLocation _previousLocation;
    private TouchLocationState _lastLocationState;

    public TouchInput()
      : base((Game) BrainGame.Instance)
    {
    }

    public override void Initialize()
    {
      this.Game.IsMouseVisible = false;
      TouchPanel.EnabledGestures = GestureType.Tap | GestureType.DoubleTap | GestureType.Hold | GestureType.HorizontalDrag | GestureType.VerticalDrag | GestureType.FreeDrag | GestureType.Pinch | GestureType.Flick | GestureType.PinchComplete;
      base.Initialize();
    }

    public override void Update(GameTime gameTime)
    {
      this._lastLocationState = TouchLocationState.Invalid;
      this._gestureType = GestureType.None;
      GestureSample gestureSample1 = new GestureSample();
      GestureSample gestureSample2 = new GestureSample();
      int num = 0;
      TouchCollection state = TouchPanel.GetState();
      if (state.Count >= 1)
      {
        this._previousLocation = this._lastLocation;
        this._lastLocation = state[0];
        this._lastLocationState = this._lastLocation.State;
      }
      while (TouchPanel.IsGestureAvailable)
      {
        GestureSample gestureSample3 = TouchPanel.ReadGesture();
        this._gestureType |= gestureSample3.GestureType;
        if (gestureSample3.GestureType == GestureType.Flick)
          this._flickDelta = gestureSample3.Delta;
        else if (gestureSample3.GestureType == GestureType.Pinch)
        {
          ++num;
          if (num == 1)
            gestureSample1 = gestureSample2 = gestureSample3;
          else
            gestureSample2 = gestureSample3;
        }
      }
      if (num <= 0)
        return;
      Vector2 position1 = gestureSample2.Position;
      Vector2 position2 = gestureSample1.Position;
      Vector2 position2_1 = gestureSample2.Position2;
      Vector2 position2_2 = gestureSample1.Position2;
      this._pinchDistance = Vector2.Distance(position1, position2_1) - Vector2.Distance(position2, position2_2);
      this._pinchCenterPosition = (position1 + position2_1) / 2f;
      this._pinchPrevDistance = Vector2.Distance(position2, position2_2);
      this._pinchDistance = Vector2.Distance(position1, position2_1);
    }

    public bool CanGetPosition => this._lastLocationState != TouchLocationState.Invalid;

    public bool WasPressed
    {
      get
      {
        return this._lastLocationState == TouchLocationState.Pressed && this._previousLocation.State == TouchLocationState.Released;
      }
    }

    public bool IsDown
    {
      get
      {
        return this._lastLocationState == TouchLocationState.Pressed || this._lastLocationState == TouchLocationState.Moved;
      }
    }

    public bool WasReleased => this._lastLocationState == TouchLocationState.Released;

    public bool HasMoved => this._lastLocationState == TouchLocationState.Moved;

    public bool IsTapping => (this._gestureType & GestureType.Tap) == GestureType.Tap;

    public bool IsDoubleTapping
    {
      get => (this._gestureType & GestureType.DoubleTap) == GestureType.DoubleTap;
    }

    public bool StartDragging
    {
      get
      {
        return (this._gestureType & GestureType.FreeDrag) == GestureType.FreeDrag || (this._gestureType & GestureType.HorizontalDrag) == GestureType.HorizontalDrag || (this._gestureType & GestureType.VerticalDrag) == GestureType.VerticalDrag;
      }
    }

    public bool EndDragging
    {
      get => (this._gestureType & GestureType.DragComplete) == GestureType.DragComplete;
    }

    public bool IsHorizontalDrag
    {
      get => (this._gestureType & GestureType.HorizontalDrag) == GestureType.HorizontalDrag;
    }

    public bool IsVerticalDrag
    {
      get => (this._gestureType & GestureType.VerticalDrag) == GestureType.VerticalDrag;
    }

    public bool OnHold => (this._gestureType & GestureType.Hold) == GestureType.Hold;

    public bool IsNone => this._gestureType == GestureType.None;

    public bool HasFlick => (this._gestureType & GestureType.Flick) == GestureType.Flick;

    public bool StartPinching => (this._gestureType & GestureType.Pinch) == GestureType.Pinch;

    public bool EndPinching
    {
      get => (this._gestureType & GestureType.PinchComplete) == GestureType.PinchComplete;
    }

    public Vector2 FlickDelta => this._flickDelta;

    public float PinchDistance => this._pinchDistance;

    public float PinchPrevDistance => this._pinchPrevDistance;

    public Vector2 PinchCenterPosition => this._pinchCenterPosition;

    public Vector2 GetTouchPosition()
    {
      Vector2 touchPosition = Vector2.Zero;
      if (this.CanGetPosition)
        touchPosition = this._lastLocation.Position;
      return touchPosition;
    }

    public bool CheckActionStartPressed() => this.IsTapping || this.IsDoubleTapping;
  }
}
