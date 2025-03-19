
// Type: TwoBrainsGames.BrainEngine.Input.InputRecorder
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;


namespace TwoBrainsGames.BrainEngine.Input
{
  public class InputRecorder
  {
    private InputRecorderStream _currentStream;
    private double _ellapsedTime;
    private InputRecorder.RecorderState _state;
    private int _currentPosition;

    public InputRecorderStream InputStream => this._currentStream;

    public InputRecorderStream.StreamItem CurrentItem
    {
      get
      {
        return this._currentStream.ItemsCount <= this._currentPosition ? (InputRecorderStream.StreamItem) null : this._currentStream[this._currentPosition];
      }
    }

    public bool IsActive => this._state != InputRecorder.RecorderState.Stopped;

    public bool IsRecording => this._state == InputRecorder.RecorderState.Recording;

    public bool IsPlaying => this._state == InputRecorder.RecorderState.Playing;

    public InputRecorder()
    {
      this._state = InputRecorder.RecorderState.Stopped;
      this._currentStream = new InputRecorderStream();
    }

    public void StartRecording()
    {
      this._state = InputRecorder.RecorderState.Recording;
      this._currentStream.Clear();
    }

    public void Stop() => this._state = InputRecorder.RecorderState.Stopped;

    public void PlayBack()
    {
      this._state = InputRecorder.RecorderState.Playing;
      this.Rewind();
    }

    public void Rewind()
    {
      this._currentPosition = 0;
      this._ellapsedTime = 0.0;
    }

    public void Update(BrainGameTime gameTime, ulong action, Vector2 position)
    {
      if (!this.IsActive)
        return;
      switch (this._state)
      {
        case InputRecorder.RecorderState.Recording:
          this._ellapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
          this._currentStream.Append(this._ellapsedTime, action, position);
          break;
        case InputRecorder.RecorderState.Playing:
          if (this._currentPosition < this._currentStream.ItemsCount)
          {
            this._ellapsedTime += gameTime.ElapsedGameTime.TotalMilliseconds;
            while (this._currentStream[this._currentPosition]._milliseconds < this._ellapsedTime)
            {
              ++this._currentPosition;
              if (this._currentPosition >= this._currentStream.ItemsCount)
                break;
            }
          }
          if (this._currentPosition < this._currentStream.ItemsCount)
            break;
          this.Stop();
          break;
      }
    }

    private enum RecorderState
    {
      Stopped,
      Recording,
      Playing,
    }
  }
}
