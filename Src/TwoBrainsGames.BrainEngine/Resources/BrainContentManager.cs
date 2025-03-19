
// Type: TwoBrainsGames.BrainEngine.Resources.BrainContentManager
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Content;
using System;
using System.Collections.Generic;


namespace TwoBrainsGames.BrainEngine.Resources
{
  public class BrainContentManager : ContentManager
  {
    private ResourceManager.ResourceManagerCacheType _cacheType;

    public string Id { get; private set; }

    public Dictionary<string, object> Resources { get; private set; }

    public object this[string key] => this.Resources[key];

    public BrainContentManager(
      IServiceProvider serviceProvider,
      string rootDirectory,
      ResourceManager.ResourceManagerCacheType cacheType,
      string id)
      : base(serviceProvider, rootDirectory)
    {
      this.Id = id;
      this._cacheType = cacheType;
      this.Resources = new Dictionary<string, object>();
    }

    public override T Load<T>(string assetName)
    {
      assetName = this.NormalizeAssetName(assetName);
      if (this.ContainsAsset(assetName))
        return (T) this.Resources[assetName];
      T obj = base.Load<T>(assetName);
      this.Resources.Add(assetName, (object) obj);
      return obj;
    }

    public override void Unload()
    {
      base.Unload();
      this.Resources.Clear();
    }

    public bool ContainsAsset(string assetName)
    {
      return this.Resources.ContainsKey(this.NormalizeAssetName(assetName));
    }

    public bool ContainsAssetNormalized(string normalizedAssetName)
    {
      return this.Resources.ContainsKey(normalizedAssetName);
    }

    public string NormalizeAssetName(string assetName) => assetName.Replace("\\", "/").ToLower();
  }
}
