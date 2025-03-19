
// Type: TwoBrainsGames.BrainEngine.Resources.Resource
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Content;
using System;
using TwoBrainsGames.BrainEngine.Data.DataFiles;


namespace TwoBrainsGames.BrainEngine.Resources
{
  public class Resource : IDataFileSerializable
  {
    private string _id;
    private string _gid;
    private string _desc;
    private string _path;
    private string _asset;
    private bool _isLoaded;
    private bool _preLoad;
    private Resource.ResourceType _type;

    public string Id
    {
      get => this._id;
      set => this._id = value;
    }

    public string GroupId
    {
      get => this._gid;
      set => this._gid = value;
    }

    public string Description
    {
      get => this._desc;
      set => this._desc = value;
    }

    public string Path
    {
      get => this._path;
      set => this._path = value;
    }

    public string Asset
    {
      get => this._asset;
      set => this._asset = value;
    }

    public bool IsLoaded
    {
      get => this._isLoaded;
      set => this._isLoaded = value;
    }

    public bool Preload
    {
      get => this._preLoad;
      set => this._preLoad = value;
    }

    public Resource.ResourceType Type
    {
      get => this._type;
      set => this._type = value;
    }

    public Resource()
    {
      this._isLoaded = false;
      this._preLoad = false;
      this._type = Resource.ResourceType.None;
      this._id = string.Empty;
      this._path = string.Empty;
      this._asset = string.Empty;
      this._desc = string.Empty;
    }

    public virtual void LoadContent(ContentManager contentManager)
    {
      throw new NotImplementedException();
    }

    public virtual bool Load(ContentManager contentManager) => throw new NotImplementedException();

    public virtual bool Release(ContentManager contentManager)
    {
      throw new NotImplementedException();
    }

    public static Resource CreateResourceByType(Resource.ResourceType type)
    {
      Resource resourceByType;
      switch (type)
      {
        case Resource.ResourceType.Image:
          resourceByType = (Resource) new Image();
          break;
        case Resource.ResourceType.Sprite:
          resourceByType = (Resource) new SpriteSet();
          break;
        case Resource.ResourceType.Font:
          resourceByType = (Resource) new TextFont();
          break;
        case Resource.ResourceType.Sample:
          resourceByType = (Resource) new Sample();
          break;
        case Resource.ResourceType.Music:
          resourceByType = (Resource) new Music();
          break;
        default:
          resourceByType = new Resource();
          break;
      }
      return resourceByType;
    }

    public static System.Type GetResourceObjectType(Resource.ResourceType type)
    {
      System.Type resourceObjectType;
      switch (type)
      {
        case Resource.ResourceType.Image:
          resourceObjectType = typeof (Image);
          break;
        case Resource.ResourceType.Sprite:
          resourceObjectType = typeof (SpriteSet);
          break;
        case Resource.ResourceType.Font:
          resourceObjectType = typeof (TextFont);
          break;
        case Resource.ResourceType.Sample:
          resourceObjectType = typeof (Sample);
          break;
        case Resource.ResourceType.Music:
          resourceObjectType = typeof (Music);
          break;
        default:
          resourceObjectType = typeof (Resource);
          break;
      }
      return resourceObjectType;
    }

    public static Resource FromDataFileRecordBase(DataFileRecord record)
    {
      Resource.ResourceType type = (Resource.ResourceType) Enum.Parse(typeof (Resource.ResourceType), record.GetFieldValue<string>("type"), false);
      Resource resourceByType = Resource.CreateResourceByType(type);
      resourceByType.Type = type;
      resourceByType.InitFromDataFileRecordBase(record);
      return resourceByType;
    }

    public void InitFromDataFileRecordBase(DataFileRecord record)
    {
      this._id = record.GetFieldValue<string>("id");
      this._desc = record.GetFieldValue<string>("desc");
      this._path = record.GetFieldValue<string>("path");
      this._asset = record.GetFieldValue<string>("asset");
      this._preLoad = record.GetFieldValue<bool>("preload");
    }

    public void InitFromResource(Resource res)
    {
      this._type = res.Type;
      this._id = res.Id;
      this._desc = res.Description;
      this._path = res.Path;
      this._asset = res.Asset;
      this._preLoad = res.Preload;
    }

    public virtual void InitFromDataFileRecord(DataFileRecord record)
    {
      throw new NotImplementedException();
    }

    public virtual DataFileRecord ToDataFileRecord() => throw new NotImplementedException();

    public enum ResourceType
    {
      None,
      Image,
      Sprite,
      Font,
      Sample,
      Music,
    }
  }
}
