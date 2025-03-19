
// Type: TwoBrainsGames.BrainEngine.Input.ControllerRumble
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer


namespace TwoBrainsGames.BrainEngine.Input
{
  public class ControllerRumble
  {
    private int _rumbleTimer;
    private bool _enableRumble;
    private float _leftMotor;
    private float _rightMotor;
    private GamePadInput _gamepad;

    public ControllerRumble(GamePadInput gamePad)
    {
      this._enableRumble = false;
      this._gamepad = gamePad;
    }

    public void Update(BrainGameTime gameTime)
    {
      if (!this._enableRumble)
        return;
      if (this._rumbleTimer > 0)
      {
        this._rumbleTimer -= gameTime.ElapsedGameTime.Milliseconds;
      }
      else
      {
        this._rumbleTimer = 5;
        this._leftMotor -= 0.1f;
        this._rightMotor -= 0.1f;
        if ((double) this._leftMotor < 0.0)
          this._leftMotor = 0.0f;
        if ((double) this._rightMotor < 0.0)
          this._rightMotor = 0.0f;
        if ((double) this._leftMotor == 0.0 && (double) this._rightMotor == 0.0)
        {
          this._enableRumble = false;
          this._rumbleTimer = 0;
        }
        this._gamepad.SetVibration(this._leftMotor, this._rightMotor);
      }
    }

    public void AddEffect(int duration, float leftMotor, float rightMotor)
    {
      this._enableRumble = true;
      this._leftMotor = leftMotor;
      this._rightMotor = rightMotor;
      this._rumbleTimer = duration;
      this._gamepad.SetVibration(this._leftMotor, this._rightMotor);
    }
  }
}
