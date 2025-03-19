
// Type: TwoBrainsGames.Snails.Stages.SnailsBackgroundLayer
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;


namespace TwoBrainsGames.Snails.Stages
{
  public class SnailsBackgroundLayer : 
    BackgroundLayer,
    ISnailsDataFileSerializable,
    IDataFileSerializable
  {
    private Vector2 _basePosition;
    public float _distance;
    private Vector2 _moveOffset;
    private LayerSizeMode _sizeMode;
    private int _frameNr;
    public float _speed;
    public LayerType _layerType;
    private BasicEffect _backgroundEffect;
    private float _scale;

    public string Id { get; set; }

    public BasicEffect BackgroundEffect => this._backgroundEffect;

    public Color BlendColor
    {
      get => new Color(this._backgroundEffect.DiffuseColor);
      set => this._backgroundEffect.DiffuseColor = value.ToVector3();
    }

    public SnailsBackgroundLayer() => this._contentManagerId = "STAGE_THEME_RESOURCES";

    public SnailsBackgroundLayer(SnailsBackgroundLayer other) => this.Copy((BackgroundLayer) other);

    public override void Copy(BackgroundLayer other)
    {
      base.Copy(other);
      SnailsBackgroundLayer snailsBackgroundLayer = (SnailsBackgroundLayer) other;
      this.Id = snailsBackgroundLayer.Id;
      this._basePosition = snailsBackgroundLayer._basePosition;
      this._layerType = snailsBackgroundLayer._layerType;
      this._distance = snailsBackgroundLayer._distance;
      this._speed = snailsBackgroundLayer._speed;
      this._frameNr = snailsBackgroundLayer._frameNr;
      this._sizeMode = snailsBackgroundLayer._sizeMode;
      this._scale = snailsBackgroundLayer._scale;
    }

    public SnailsBackgroundLayer Clone()
    {
      SnailsBackgroundLayer snailsBackgroundLayer = new SnailsBackgroundLayer();
      snailsBackgroundLayer.Copy((BackgroundLayer) this);
      return snailsBackgroundLayer;
    }

    public override void LoadContent()
    {
      base.LoadContent();
      switch (this._sizeMode)
      {
        case LayerSizeMode.FillBoard:
          this.Size = new Vector2((float) Stage.CurrentStage.Board.Width * (1f + this._distance), (float) Stage.CurrentStage.Board.Height * (1f + this._distance));
          break;
        case LayerSizeMode.Sprite:
          this.Size = new Vector2((float) this.Sprite.Frames[this._frameNr].Width, (float) this.Sprite.Frames[this._frameNr].Height);
          break;
      }
    }

    public void Initialize()
    {
      this._backgroundEffect = new BasicEffect(BrainGame.Graphics);
      this._backgroundEffect.TextureEnabled = true;
      this._backgroundEffect.VertexColorEnabled = true;
      this._backgroundEffect.World = Matrix.Identity;
      this._backgroundEffect.View = Matrix.Identity;
      this._backgroundEffect.Projection = BrainGame.RenderEffect.Projection;
    }

    public void Update(BrainGameTime gameTime, bool allowMove)
    {
      Vector2 origin = Stage.CurrentStage.Camera.Origin;
      Vector2 vector2 = new Vector2(Stage.CurrentStage.Camera.Position.X / this._distance, Stage.CurrentStage.Camera.Position.Y / this._distance) + new Vector2(origin.X - origin.X / this._distance, origin.Y - origin.Y / this._distance) / Stage.CurrentStage.Camera.MaxZoomOut + Stage.CurrentStage._backgroundLayersOffset / this._scale;
      this._backgroundEffect.World = Matrix.CreateTranslation(-vector2.X / this._scale, -vector2.Y / this._scale, 0.0f) * Matrix.CreateScale(new Vector3(this._scale * Stage.CurrentStage.Camera.Scale.X, this._scale * Stage.CurrentStage.Camera.Scale.Y, 0.0f)) * Matrix.CreateTranslation(Stage.CurrentStage.Camera.Origin.X, Stage.CurrentStage.Camera.Origin.Y, 0.0f);
      this._position = (this._moveOffset + this._basePosition) / this._scale;
      if ((double) this._speed == 0.0 || !allowMove)
        return;
      this._moveOffset += new Vector2(this._speed * (float) (gameTime.ElapsedGameTime.TotalMilliseconds / 100.0), 0.0f);
      if ((double) this._position.X <= (double) Stage.CurrentStage.Board.Width / (double) this._scale)
        return;
      this._basePosition.X = -this.Size.X * this._scale;
      this._moveOffset = Vector2.Zero;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      switch (this._sizeMode)
      {
        case LayerSizeMode.FillBoard:
          base.Draw(spriteBatch);
          break;
        case LayerSizeMode.Sprite:
          this.Sprite.DrawNoRound(this._position, this._frameNr, spriteBatch);
          break;
      }
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this.Id = record.GetFieldValue<string>("id", this.Id);
      this._layerType = (LayerType) record.GetFieldValue<int>("type", (int) this._layerType);
      this._distance = record.GetFieldValue<float>("distance", this._distance);
      this._speed = record.GetFieldValue<float>("speed", this._speed);
      this._frameNr = record.GetFieldValue<int>("frame", this._frameNr);
      this._sizeMode = (LayerSizeMode) record.GetFieldValue<int>("sizeMode", (int) this._sizeMode);
      this._scale = record.GetFieldValue<float>("scale", this._scale);
      this._basePosition = this._position;
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord;
      if (context == ToDataFileRecordContext.StageDataSave)
      {
        dataFileRecord = base.ToDataFileRecord();
        dataFileRecord.AddField("distance", (object) this._distance);
        dataFileRecord.AddField("speed", (object) this._speed);
        dataFileRecord.AddField("frame", (object) this._frameNr);
        dataFileRecord.AddField("sizeMode", (object) (int) this._sizeMode);
        dataFileRecord.AddField("scale", (object) this._scale);
      }
      else
        dataFileRecord = new DataFileRecord("Layer");
      dataFileRecord.AddField("id", (object) this.Id);
      dataFileRecord.AddField("type", (object) (int) this._layerType);
      return dataFileRecord;
    }

    public override string ToString() => this.Id == null ? "" : this.Id;
  }
}
