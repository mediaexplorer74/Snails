
// Type: TwoBrainsGames.Snails.StageObjects.FlameLight
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  internal class FlameLight : SingleLightEmitter
  {
    private const int BB_IDX_FLAME_POSITION = 0;
    private string _flameSpriteRes;
    private SpriteAnimation _flameAnimation;
    private Vector2 _flamePosition;
    private Vector2 _defaultScale;
    private double _glowTime;

    public FlameLight()
      : base(StageObjectType.FlameLight)
    {
    }

    public override void Copy(StageObject other)
    {
      base.Copy(other);
      this._flameSpriteRes = ((FlameLight) other)._flameSpriteRes;
    }

    public override void LoadContent()
    {
      base.LoadContent();
      string resourceManagerId = "__TEMPORARY__";
      if (this._flameSpriteRes.Contains(ThemeType.ThemeA.ToString()) || this._flameSpriteRes.Contains(ThemeType.ThemeB.ToString()) || this._flameSpriteRes.Contains(ThemeType.ThemeC.ToString()) || this._flameSpriteRes.Contains(ThemeType.ThemeD.ToString()))
        resourceManagerId = "STAGE_THEME_RESOURCES";
      this._flameAnimation = new SpriteAnimation(this._flameSpriteRes, resourceManagerId);
    }

    public override void Initialize()
    {
      base.Initialize();
      this._flamePosition = this.TransformSpriteFrameBB(0).ToBoundingSquare().Center;
      this._defaultScale = this.LightSource.Scale;
      this._glowTime = 50.0;
      this.LightSource.Position = this._flamePosition;
      this.LightSource.Color = new Color(1f, 0.0f, 0.0f, 0.2f);
      this._flameAnimation.RandomizeFrame();
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      this._flameAnimation.Update(gameTime);
      this._glowTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
      if (this._glowTime >= 0.0)
        return;
      this._glowTime = 50.0;
      this.LightSource.Scale = this._defaultScale + new Vector2(BrainGame.RandomizeFloat(0.0f, 0.05f, 2), BrainGame.RandomizeFloat(0.0f, 0.05f, 2));
      this.LightSource.Color = new Color(BrainGame.RandomizeFloat(0.8f, 1f, 2), BrainGame.RandomizeFloat(0.25f, 0.3f, 2), 0.0f, 0.2f);
    }

    public override void Draw(bool shadow)
    {
      base.Draw(shadow);
      if (shadow)
        this._flameAnimation.Draw(this._flamePosition + GenericConsts.ShadowDepth, this.ShadowColor, Stage.CurrentStage.SpriteBatch);
      else
        this._flameAnimation.Draw(this._flamePosition, Stage.CurrentStage.SpriteBatch);
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this._flameSpriteRes = record.GetFieldValue<string>("flameSpriteRes", this._flameSpriteRes);
    }

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord(context);
      if (context == ToDataFileRecordContext.StageDataSave)
        dataFileRecord.AddField("flameSpriteRes", (object) this._flameSpriteRes);
      return dataFileRecord;
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }
  }
}
