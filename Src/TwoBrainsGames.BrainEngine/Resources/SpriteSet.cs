
// Type: TwoBrainsGames.BrainEngine.Resources.SpriteSet
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;


namespace TwoBrainsGames.BrainEngine.Resources
{
  public class SpriteSet : Image, IDataFileSerializable
  {
    private Dictionary<string, Sprite> _sprites;
    private string _name;
    private string _imageId;

    public Sprite this[string id]
    {
      get
      {
        Sprite sprite;
        if (!this._sprites.TryGetValue(id, out sprite))
          throw new BrainException("Sprite with id '" + id + "' not found in SpriteSet '" + this._name + "'.");
        return sprite;
      }
    }

    public override void LoadContent(ContentManager contentManager)
    {
      this.Texture = contentManager.Load<Texture2D>(this._imageId);
      foreach (string key in this._sprites.Keys)
        this._sprites[key].Texture = this.Texture;
    }

    public override bool Release(ContentManager contentManager) => base.Release(contentManager);

    public static SpriteSet FromDataFileRecord(DataFileRecord record)
    {
      SpriteSet spriteSet = new SpriteSet();
      spriteSet.InitFromDataFileRecord(record);
      return spriteSet;
    }

    public bool ContainsSprite(string spriteName) => this._sprites.ContainsKey(spriteName);

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      this._name = record.GetFieldValue<string>("Name");
      this._imageId = record.GetFieldValue<string>("ImageId");
      this._sprites = new Dictionary<string, Sprite>();
      foreach (DataFileRecord selectRecord in record.SelectRecords("Sprite"))
      {
        Sprite sprite = Sprite.FromDataFileRecord(selectRecord);
        this._sprites.Add(sprite.Id, sprite);
      }
    }

    public override DataFileRecord ToDataFileRecord()
    {
      DataFileRecord dataFileRecord = new DataFileRecord(nameof (SpriteSet));
      dataFileRecord.AddField("Name", (object) this._name);
      dataFileRecord.AddField("ImageId", (object) this._imageId);
      foreach (Sprite sprite in this._sprites.Values)
        dataFileRecord.AddRecord(sprite.ToDataFileRecord());
      return dataFileRecord;
    }
  }
}
