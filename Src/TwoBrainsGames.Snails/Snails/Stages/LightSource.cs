
// Type: TwoBrainsGames.Snails.Stages.LightSource
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Graphics;


namespace TwoBrainsGames.Snails.Stages
{
  public class LightSource : IDataFileSerializable
  {
    public const float FullPower = 1f;
    private Color _lightMapColor = Color.White;
    private float _power;

    private LightSource.LightSourceType Type { get; set; }

    public string Id { get; private set; }

    private string MaskSpriteResourceName { get; set; }

    private string TintSpriteResourceName { get; set; }

    public Sprite MaskSprite { get; set; }

    private Sprite TintSprite { get; set; }

    public Vector2 Scale { get; set; }

    public float Rotation { get; set; }

    public Color Color { get; set; }

    public Color ColorWithPower => new Color(this.Color.ToVector4() * this._power);

    public Vector2 Position { get; set; }

    public LightSource.LightState State { get; private set; }

    public bool IsOn => this.State == LightSource.LightState.On;

    public bool IsOff => this.State == LightSource.LightState.Off;

    public bool WithTint => this.TintSprite != null;

    public TransformBlender EffectsBlender { get; private set; }

    public float Power
    {
      get => this._power;
      set => this._power = value;
    }

    public LightSource()
    {
      this.Scale = new Vector2(1f, 1f);
      this.Rotation = 0.0f;
      this.Color = Color.White;
      this.State = LightSource.LightState.On;
      this.Power = 1f;
      this.EffectsBlender = new TransformBlender();
    }

    public static LightSource Create(LightSource.LightSourceType type)
    {
      if (type == LightSource.LightSourceType.Baselight)
        return new LightSource();
      throw new BrainException("Invalid light source type [" + type.ToString() + "]");
    }

    public virtual LightSource Clone()
    {
      LightSource lightSource = new LightSource();
      lightSource.Copy(this);
      return lightSource;
    }

    public virtual void Copy(LightSource from)
    {
      this.MaskSprite = from.MaskSprite;
      this.MaskSpriteResourceName = from.MaskSpriteResourceName;
      this.TintSpriteResourceName = from.TintSpriteResourceName;
      this.Color = from.Color;
      this.Id = from.Id;
      this.Position = from.Position;
      this.Rotation = from.Rotation;
      this.Scale = from.Scale;
      this.State = from.State;
      this.Power = from.Power;
    }

    public virtual void LoadContent()
    {
      this.MaskSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.MaskSpriteResourceName);
      if (string.IsNullOrEmpty(this.TintSpriteResourceName))
        return;
      this.TintSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.TintSpriteResourceName);
    }

    public virtual void Update(BrainGameTime gameTime)
    {
      if (this.EffectsBlender.Count <= 0)
        return;
      this.UpdateEffects(gameTime);
    }

    protected void UpdateEffects(BrainGameTime gameTime)
    {
      this.EffectsBlender.Update(gameTime);
      this.Position += this.EffectsBlender.PositionV2;
      this.Rotation += this.EffectsBlender.Rotation;
      this.Scale += this.EffectsBlender._scale;
      this.Color = this.EffectsBlender.Color;
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
      this.MaskSprite.Draw(this.Position, 0, this.Rotation, Vector2.Zero, this.Scale.X, this.Scale.Y, this._lightMapColor, spriteBatch);
    }

    public virtual void DrawTint(SpriteBatch spriteBatch)
    {
      if (this.TintSprite == null)
        return;
      this.TintSprite.Draw(this.Position, 0, this.Rotation, Vector2.Zero, this.Scale.X, this.Scale.Y, this.ColorWithPower, spriteBatch);
    }

    public void SwitchOn() => this.State = LightSource.LightState.On;

    public void SwitchOff() => this.State = LightSource.LightState.Off;

    public void SetState(LightSource.LightState state)
    {
      if (state == LightSource.LightState.On)
        this.SwitchOn();
      else
        this.SwitchOff();
    }

    public void InitFromDataFileRecord(DataFileRecord record)
    {
      this.Type = (LightSource.LightSourceType) Enum.Parse(typeof (LightSource.LightSourceType), record.GetFieldValue<string>("type", LightSource.LightSourceType.Baselight.ToString()), false);
      this.Id = record.GetFieldValue<string>("id");
      this.MaskSpriteResourceName = record.GetFieldValue<string>("maskSprite");
      this.TintSpriteResourceName = record.GetFieldValue<string>("tintSprite");
      this.Color = record.GetFieldValue<Color>("color");
    }

    public DataFileRecord ToDataFileRecord()
    {
      DataFileRecord dataFileRecord = new DataFileRecord(nameof (LightSource));
      dataFileRecord.AddField("type", (object) this.Type.ToString());
      dataFileRecord.AddField("id", (object) this.Id);
      dataFileRecord.AddField("maskSprite", (object) this.MaskSpriteResourceName);
      dataFileRecord.AddField("tintSprite", (object) this.TintSpriteResourceName);
      dataFileRecord.AddField("color", (object) this.Color);
      return dataFileRecord;
    }

    public enum LightSourceType
    {
      Baselight,
      Spotlight,
    }

    public enum LightState
    {
      On,
      Off,
    }
  }
}
