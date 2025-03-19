
// Type: TwoBrainsGames.Snails.Stages.HUD.HUDItemMissionStatus
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Localization;


namespace TwoBrainsGames.Snails.Stages.HUD
{
  internal class HUDItemMissionStatus : HUDItem
  {
    public const double MISSION_STATUS_BLINK_TIME = 400.0;
    private Vector2 _missionStatusPosition;
    private double _missionStatusBlink;
    private bool _blinkVisible;
    private string _statusText;
    private Color _color;
    private double _expirationTime;
    private bool _expires;

    public override void Initialize(Vector2 position)
    {
      base.Initialize(position);
      this._missionStatusPosition = position + new Vector2(0.0f, 10f);
      this._width = 300f;
    }

    public override void Update(BrainGameTime gameTime)
    {
      if (!this._visible)
        return;
      this._missionStatusBlink += gameTime.ElapsedRealTime.TotalMilliseconds;
      if (this._missionStatusBlink > 400.0)
      {
        this._missionStatusBlink -= 400.0;
        this._blinkVisible = !this._blinkVisible;
      }
      if (!this._expires)
        return;
      this._expirationTime -= gameTime.ElapsedRealTime.TotalMilliseconds;
      if (this._expirationTime > 0.0)
        return;
      this._visible = false;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      if (!this._blinkVisible || !this._visible)
        return;
      this._font.DrawString(spriteBatch, this._statusText, this._missionStatusPosition, new Vector2(1f, 1f), this._color);
    }

    public override void MissionStateChanged()
    {
      this._visible = false;
      this._missionStatusBlink = 0.0;
      this._blinkVisible = false;
      this._expirationTime = 0.0;
      this._expires = false;
      switch (Stage.CurrentStage.MissionState)
      {
        case Stage.MissionStateType.Starting:
        case Stage.MissionStateType.Running:
          this._visible = true;
          this._statusText = Formater.FormatGoalDescription(Stage.CurrentStage.LevelStage, true);
          this._color = Colors.HudItem_GoalDescription;
          this._expirationTime = 5000.0;
          this._expires = Stage.CurrentStage.MissionState == Stage.MissionStateType.Running;
          this._blinkVisible = true;
          break;
        case Stage.MissionStateType.Completed:
          this._visible = true;
          this._statusText = LanguageManager.GetString("MSG_MISSION_COMPLETED");
          this._color = Colors.HudItem_MsgStatusCompleted;
          break;
        case Stage.MissionStateType.Failed:
          this._visible = true;
          this._statusText = LanguageManager.GetString("MSG_MISSION_FAILED");
          this._color = Colors.HudItem_MsgStatusFailed;
          break;
      }
    }
  }
}
