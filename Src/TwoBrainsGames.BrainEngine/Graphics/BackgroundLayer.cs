
// Type: TwoBrainsGames.BrainEngine.Graphics.BackgroundLayer
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Data.DataFiles;


namespace TwoBrainsGames.BrainEngine.Graphics
{
  public class BackgroundLayer : IDataFileSerializable
  {
    private Vector2 _size;
    private Rectangle _drawingRect;
    private SamplerState _samplerState;
    public string _contentManagerId;
    public string ResId;
    public string SpriteId;
    public float OffsetX;
    public float OffsetY;
    public float LayerDepth;
    public Sprite Sprite;
    public Vector2 _position;

    public Vector2 Size
    {
      get => this._size;
      set
      {
        this._size = value;
        this._drawingRect = new Rectangle(0, 0, (int) this._size.X, (int) this._size.Y);
      }
    }

    public BackgroundLayer() => this._contentManagerId = "__TEMPORARY__";

    public BackgroundLayer(BackgroundLayer other) => this.Copy(other);

    public virtual void Copy(BackgroundLayer other)
    {
      this.ResId = other.ResId;
      this.SpriteId = other.SpriteId;
      this.OffsetX = other.OffsetX;
      this.OffsetY = other.OffsetY;
      this.LayerDepth = other.LayerDepth;
      this.Sprite = other.Sprite;
      this._drawingRect = other._drawingRect;
      this._position = other._position;
      this._samplerState = other._samplerState;
      this._size = other._size;
      this._contentManagerId = other._contentManagerId;
    }

    public virtual void LoadContent()
    {
      this.Sprite = BrainGame.ResourceManager.GetSprite(this.ResId + "/" + this.SpriteId, this._contentManagerId);
      this.Sprite.LayerDepth = this.LayerDepth;
      this.Sprite.HasShadows = false;
      this.Size = new Vector2((float) this.Sprite.Width, (float) this.Sprite.Height);
      this._samplerState = new SamplerState();
      this._samplerState.AddressU = TextureAddressMode.Wrap;
      this._samplerState.AddressV = TextureAddressMode.Wrap;
    }

    public virtual void Update(BrainGameTime gameTime)
    {
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
      SamplerState samplerState = BrainGame.Graphics.SamplerStates[0];
      BrainGame.Graphics.SamplerStates[0] = this._samplerState;
      this.Sprite.Draw(this._position, this._drawingRect, spriteBatch);
      BrainGame.Graphics.SamplerStates[0] = samplerState;
    }

    public virtual void InitFromDataFileRecord(DataFileRecord record)
    {
      this.ResId = record.GetFieldValue<string>("res", this.ResId);
      this.SpriteId = record.GetFieldValue<string>("sprite", this.SpriteId);
      this.OffsetX = record.GetFieldValue<float>("offsetX", this.OffsetX);
      this.OffsetY = record.GetFieldValue<float>("offsetY", this.OffsetY);
      this.LayerDepth = record.GetFieldValue<float>("layerDepth", this.LayerDepth);
      this._position = new Vector2(record.GetFieldValue<float>("x", this._position.X), record.GetFieldValue<float>("y", this._position.Y));
    }

    public virtual DataFileRecord ToDataFileRecord()
    {
      DataFileRecord dataFileRecord = new DataFileRecord("Layer");
      dataFileRecord.AddField("res", (object) this.ResId);
      dataFileRecord.AddField("sprite", (object) this.SpriteId);
      dataFileRecord.AddField("offsetX", (object) this.OffsetX);
      dataFileRecord.AddField("offsetY", (object) this.OffsetY);
      dataFileRecord.AddField("layerDepth", (object) this.LayerDepth);
      dataFileRecord.AddField("x", (object) this._position.X);
      dataFileRecord.AddField("y", (object) this._position.Y);
      return dataFileRecord;
    }
  }
}
