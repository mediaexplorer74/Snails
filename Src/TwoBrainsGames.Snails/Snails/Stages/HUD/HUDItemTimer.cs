
// Type: TwoBrainsGames.Snails.Stages.HUD.HUDItemTimer
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;


namespace TwoBrainsGames.Snails.Stages.HUD
{
  internal class HUDItemTimer : HUDItem
  {
    private SpriteAnimation _spriteTimer;
    private Sprite _spriteTimeWarp;
    private Vector2 _timerPosition;
    private Vector2 _timeWarpPosition;
    private Vector2 _stringPosition;
    private Color _timerColor;
    private string _timerString;
    private bool _timerStopped;
    private float _timeWarpRotation;
    private double _timerEllapsed;
    private Sample _tickSample;
    private Sample _timerUpSample;

    public override void Initialize(Vector2 position)
    {
      base.Initialize(position);
      this._timerPosition = position + new Vector2(0.0f, 10f);
      this._stringPosition = position + new Vector2(40f, 10f);
      this._timeWarpPosition = position + new Vector2(60f, 20f);
      this.UpdateString();
      this._width = 125f;
      this._timerColor = Colors.StageHUDInfoColor;
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this._spriteTimer = new SpriteAnimation(BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "Timer"));
      this._spriteTimeWarp = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "TimeWarp");
      this._tickSample = BrainGame.ResourceManager.GetSampleTemporary("sfx/timer-tick");
      this._timerUpSample = BrainGame.ResourceManager.GetSampleTemporary("sfx/time-up");
    }

    public override void HandleInput(BrainGameTime gameTime)
    {
    }

    public override void Update(BrainGameTime gameTime)
    {
      if (Stage.CurrentStage.InTimeWarp)
      {
        this._timeWarpRotation -= (float) (0.5 * gameTime.ElapsedRealTime.TotalMilliseconds);
        if ((double) this._timeWarpRotation < 0.0)
          this._timeWarpRotation += 360f;
      }
      if (!this._timerStopped)
      {
        if (Stage.CurrentStage.LevelStage._goal == GoalType.TimeAttack)
        {
          if (Stage.CurrentStage.Stats.Timer.TotalSeconds > 0.0)
          {
            Stage.CurrentStage.Stats.Timer = Stage.CurrentStage.Stats.Timer.Subtract(gameTime.ElapsedGameTime);
            if (Stage.CurrentStage.Stats.Timer.TotalSeconds <= 0.0)
            {
              this.StopTimer();
              Stage.CurrentStage.Stats.Timer = new TimeSpan(0L);
              this._timerUpSample.Play();
            }
          }
        }
        else
          Stage.CurrentStage.Stats.Timer = Stage.CurrentStage.Stats.Timer.Add(gameTime.ElapsedGameTime);
      }
      this._timerEllapsed -= gameTime.ElapsedGameTime.TotalMilliseconds;
      if (this._timerEllapsed >= 0.0)
        return;
      double num = Stage.CurrentStage.Stats.Timer.TotalMilliseconds / 1000.0;
      if (!this._timerStopped)
      {
        if (Stage.CurrentStage.LevelStage._goal == GoalType.TimeAttack)
        {
          if (num < 10.0)
          {
            this._timerColor = Colors.StageHUDTimerLowColor;
            if (Stage.CurrentStage.Stats.Timer.TotalSeconds > 0.0)
              this._tickSample.Play();
          }
          this._spriteTimer.DecrementFrame();
        }
        else
          this._spriteTimer.IncrementFrame();
        this._timerEllapsed = 1000.0 + this._timerEllapsed;
      }
      this.UpdateString();
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      if (Stage.CurrentStage.InTimeWarp)
        this._spriteTimeWarp.Draw(this._timeWarpPosition, 0, this._timeWarpRotation, SpriteEffects.None, spriteBatch);
      this._spriteTimer.Draw(this._timerPosition, spriteBatch);
      this._font.DrawString(spriteBatch, this._timerString, this._stringPosition, new Vector2(1f, 1f), this._timerColor);
    }

    public void StopTimer() => this._timerStopped = true;

    private void UpdateString()
    {
      TimeSpan timer = Stage.CurrentStage.Stats.Timer;
      this._timerString = string.Format("{0:00}:{1:00}", (object) timer.Minutes, (object) timer.Seconds);
      if (timer.Hours <= 0)
        return;
      this._timerString = "60:00";
    }
  }
}
