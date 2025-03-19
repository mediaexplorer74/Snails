
// Type: TwoBrainsGames.Snails.StageObjects.FadeInOutBox
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  internal class FadeInOutBox(StageObjectType type) : Box(type), ISwitchable, IDataFileSerializable
  {
    private const float FADING_SPEED = 3f;
    private float _alpha;
    private Sample _fadeInSound;
    private Sample _fadeOutSound;

    public FadeInOutBox.FadeInOutBoxState State { get; set; }

    public bool IsFadedIn => this.State == FadeInOutBox.FadeInOutBoxState.FadedIn;

    public bool IsFadedOut => this.State == FadeInOutBox.FadeInOutBoxState.FadedOut;

    public FadeInOutBox()
      : this(StageObjectType.FadeInOutBox)
    {
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this._fadeInSound = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/fade-box-in", (Object2D) this);
      this._fadeOutSound = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/fade-box-out", (Object2D) this);
    }

    public override void Initialize()
    {
      base.Initialize();
      this.CurrentFrame = BrainGame.Rand.Next(this.Sprite.FrameCount);
    }

    public override void StageInitialize()
    {
      base.StageInitialize();
      if (this.IsFadedIn)
      {
        this._alpha = 1f;
        this.BoxDeployed(Stage.LoadingContext == Stage.StageLoadingContext.Gameplay, false, false);
      }
      else
        this._alpha = 0.0f;
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      switch (this.State)
      {
        case FadeInOutBox.FadeInOutBoxState.FadingIn:
          this._alpha += (float) (3.0 * gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0);
          this.BlendColor = new Color(this._alpha, this._alpha, this._alpha, this._alpha);
          if ((double) this._alpha <= 1.0)
            break;
          this._alpha = 1f;
          this.State = FadeInOutBox.FadeInOutBoxState.FadedIn;
          break;
        case FadeInOutBox.FadeInOutBoxState.FadingOut:
          this._alpha -= (float) (3.0 * gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0);
          this.BlendColor = new Color(this._alpha, this._alpha, this._alpha, this._alpha);
          if ((double) this._alpha >= 0.0)
            break;
          this._alpha = 0.0f;
          this.State = FadeInOutBox.FadeInOutBoxState.FadedOut;
          Stage.CurrentStage.Board.RemoveTileAt(this.BoardX, this.BoardY);
          break;
      }
    }

    public override void Draw(bool shadow)
    {
      if (this.IsFadedOut)
        return;
      base.Draw(shadow);
    }

    private void SwitchChanged()
    {
      switch (this.State)
      {
        case FadeInOutBox.FadeInOutBoxState.FadedIn:
          this.State = FadeInOutBox.FadeInOutBoxState.FadingOut;
          this._fadeOutSound.Play();
          break;
        case FadeInOutBox.FadeInOutBoxState.FadedOut:
          if (Stage.CurrentStage.Board.GetTileAt(this.BoardY, this.BoardX) != null)
            break;
          this._fadeInSound.Play();
          this.State = FadeInOutBox.FadeInOutBoxState.FadingIn;
          this.BoxDeployed(true, true, true);
          break;
        case FadeInOutBox.FadeInOutBoxState.FadingIn:
          this.State = FadeInOutBox.FadeInOutBoxState.FadingIn;
          break;
      }
    }

    public void SwitchOn() => this.SwitchChanged();

    public void SwitchOff() => this.SwitchChanged();

    public bool IsOn => this.State == FadeInOutBox.FadeInOutBoxState.FadedIn;

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this.State = (FadeInOutBox.FadeInOutBoxState) Enum.Parse(typeof (FadeInOutBox.FadeInOutBoxState), record.GetFieldValue<string>("state", this.State.ToString()), true);
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord(context);
      dataFileRecord.AddField("state", (object) this.State.ToString());
      return dataFileRecord;
    }

    public enum FadeInOutBoxState
    {
      FadedIn,
      FadedOut,
      FadingIn,
      FadingOut,
    }
  }
}
