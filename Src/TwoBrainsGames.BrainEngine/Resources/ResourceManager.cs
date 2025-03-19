
// Type: TwoBrainsGames.BrainEngine.Resources.ResourceManager
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using TwoBrainsGames.BrainEngine.Graphics;


namespace TwoBrainsGames.BrainEngine.Resources
{
  public class ResourceManager
  {
    public const string RES_MANAGER_ID_STATIC = "__STATIC__";
    public const string RES_MANAGER_ID_TEMPORARY = "__TEMPORARY__";

    private BrainContentManager StaticResManager => this.ResourceManagers["__STATIC__"];

    private BrainContentManager TemporaryResManager => this.ResourceManagers["__TEMPORARY__"];

    private Dictionary<string, BrainContentManager> ResourceManagers { get; set; }

    private IServiceProvider ServiceProvider { get; set; }

    private string RootDirectory { get; set; }

    public ResourceManager(IServiceProvider serviceProvider, string rootDirectory)
    {
      this.ServiceProvider = serviceProvider;
      this.RootDirectory = rootDirectory;
      this.ResourceManagers = new Dictionary<string, BrainContentManager>();
      this.ResourceManagers.Add("__STATIC__", new BrainContentManager(serviceProvider, rootDirectory, ResourceManager.ResourceManagerCacheType.Static, "__STATIC__"));
      this.ResourceManagers.Add("__TEMPORARY__", new BrainContentManager(serviceProvider, rootDirectory, ResourceManager.ResourceManagerCacheType.Temporary, "__TEMPORARY__"));
    }

    public void CreateUserDefinedResourceManager(string resManagerId)
    {
      if (this.ResourceManagers.ContainsKey(resManagerId))
        return;
      this.ResourceManagers.Add(resManagerId, new BrainContentManager(this.ServiceProvider, this.RootDirectory, ResourceManager.ResourceManagerCacheType.UserDefined, resManagerId));
    }

    public T Load<T>(string assetName)
    {
      return this.Load<T>(assetName, ResourceManager.ResourceManagerCacheType.Temporary);
    }

    public T Load<T>(string assetName, string resManagerId)
    {
      if (!this.ResourceManagers.ContainsKey(resManagerId))
        throw new BrainException("Resource Manager with id [" + resManagerId + "] does not exist. Use ResourceManager.CreateUserDefinedResourceManager() to create one.");
      return this.ResourceManagers[resManagerId].Load<T>(assetName);
    }

    public T Load<T>(string assetName, ResourceManager.ResourceManagerCacheType type)
    {
      T obj = default (T);
      switch (type)
      {
        case ResourceManager.ResourceManagerCacheType.Temporary:
            try
            {
                obj = this.TemporaryResManager.Load<T>(assetName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] ResourceManager - " + ex.Message);
            }
        break;
        case ResourceManager.ResourceManagerCacheType.Static:
            try
            {
                obj = this.StaticResManager.Load<T>(assetName);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[ex] ResourceManager - " + ex.Message);
            }
        break;
        case ResourceManager.ResourceManagerCacheType.UserDefined:
          throw new BrainException("Resource ID must be specified for UserDefined resource managers.");
      }
      return obj;
    }

    public void UnloadTemporary() => this.Unload(this.TemporaryResManager);

    public void UnloadStatic() => this.Unload(this.StaticResManager);

    public void Unload()
    {
      foreach (KeyValuePair<string, BrainContentManager> resourceManager in this.ResourceManagers)
        this.Unload(resourceManager.Value);
    }

    public void Unload(string resManagerId)
    {
      if (!this.ResourceManagers.ContainsKey(resManagerId))
        return;
      this.Unload(this.ResourceManagers[resManagerId]);
      GC.Collect();
    }

    private void Unload(BrainContentManager resManager)
    {
      resManager.Unload();
      GC.Collect();
    }

    public Sprite GetSpriteTemporary(string resourceName)
    {
      return this.GetSpriteTemporary(BrainPath.GetDirectoryName(resourceName), BrainPath.GetFileName(resourceName));
    }

    public Sprite GetSprite(string resourceName, string resManagerId)
    {
      string directoryName = BrainPath.GetDirectoryName(resourceName);
      string fileName = BrainPath.GetFileName(resourceName);
      return this.Load<SpriteSet>(directoryName, resManagerId)[fileName];
    }

    public Sprite GetSpriteTemporary(string assetName, string spriteName)
    {
      return this.Load<SpriteSet>(assetName, ResourceManager.ResourceManagerCacheType.Temporary)[spriteName];
    }

    public Sprite GetSpriteStatic(string assetName, string spriteName)
    {
      return this.Load<SpriteSet>(assetName, ResourceManager.ResourceManagerCacheType.Static)[spriteName];
    }

    public Sprite GetSpriteStatic(string spriteResourceName)
    {
      return this.GetSpriteStatic(BrainPath.GetDirectoryName(spriteResourceName), BrainPath.GetFileName(spriteResourceName));
    }

    public Sprite GetSprite(
      string assetName,
      string spriteName,
      ResourceManager.ResourceManagerCacheType type)
    {
      return this.Load<SpriteSet>(assetName, type)[spriteName];
    }

    public Sprite GetSprite(string resName, ResourceManager.ResourceManagerCacheType type)
    {
      string directoryName = BrainPath.GetDirectoryName(resName);
      string fileName = BrainPath.GetFileName(resName);
      return this.Load<SpriteSet>(directoryName, type)[fileName];
    }

    public Sample GetSampleTemporary(string assetName)
    {
      return this.GetSampleTemporary(assetName, (Object2D) null);
    }

    public Sample GetSampleStatic(string assetName)
    {
      return this.GetSampleStatic(assetName, (Object2D) null);
    }

    public Sample GetSampleTemporary(string assetName, Object2D objEmitter)
    {
      return new Sample(this.Load<SoundEffect>(assetName, ResourceManager.ResourceManagerCacheType.Temporary), objEmitter);
    }

    public Sample GetSampleStatic(string assetName, Object2D objEmitter)
    {
      return new Sample(this.Load<SoundEffect>(assetName, ResourceManager.ResourceManagerCacheType.Static), objEmitter);
    }

    public Sample GetSample(string assetName, ResourceManager.ResourceManagerCacheType type)
    {
      return new Sample(this.Load<SoundEffect>(assetName, type));
    }

    public Sample GetSample(string assetName, string resourceManagerId)
    {
      return new Sample(this.Load<SoundEffect>(assetName, resourceManagerId));
    }

    public Sample GetSample(string assetName, string resourceManagerId, Object2D objEmitter)
    {
      return new Sample(this.Load<SoundEffect>(assetName, resourceManagerId), objEmitter);
    }

    public Music GetMusicTemporary(string assetName)
    {
      return new Music(this.Load<Song>(assetName, ResourceManager.ResourceManagerCacheType.Temporary));
    }

    public Music GetMusicStatic(string assetName)
    {
      return new Music(this.Load<Song>(assetName, ResourceManager.ResourceManagerCacheType.Static));
    }

    public Music GetMusic(string assetName, ResourceManager.ResourceManagerCacheType type)
    {
      return new Music(this.Load<Song>(assetName, type));
    }

    public Music GetMusic(string assetName, string resourceManagerId)
    {
      return new Music(this.Load<Song>(assetName, resourceManagerId));
    }

    public bool ContainsSprite(string assetName, string spriteName)
    {
      assetName = this.StaticResManager.NormalizeAssetName(assetName);
      if (this.StaticResManager.ContainsAssetNormalized(assetName))
        return ((SpriteSet) this.StaticResManager[assetName]).ContainsSprite(spriteName);
      return this.TemporaryResManager.ContainsAssetNormalized(assetName) && ((SpriteSet) this.TemporaryResManager[assetName]).ContainsSprite(spriteName);
    }

    public void Lock() => Monitor.Enter((object) this);

    public bool TryLock(int timeout) => Monitor.TryEnter((object) this, timeout);

    public void Unlock() => Monitor.Exit((object) this);

    public bool IsLocked()
    {
      if (!Monitor.TryEnter((object) this))
        return true;
      Monitor.Exit((object) this);
      return false;
    }

    public void TraceLoadedResources(SpriteBatch spriteBatch, SpriteFont font)
    {
      BrainGame.DrawRectangleFilled(spriteBatch, new Rectangle(BrainGame.Viewport.X, BrainGame.Viewport.Y, BrainGame.Viewport.Width, BrainGame.Viewport.Height), new Color(0, 0, 0, 120));
      Vector2 position = new Vector2(20f, 60f);
      foreach (KeyValuePair<string, BrainContentManager> resourceManager in this.ResourceManagers)
        position = this.TraceLoadedResources(resourceManager.Value, spriteBatch, font, position);
    }

    private Vector2 TraceLoadedResources(
      BrainContentManager manager,
      SpriteBatch spriteBatch,
      SpriteFont font,
      Vector2 position)
    {
      spriteBatch.DrawString(font, manager.Id, position, Color.Cyan);
      position += new Vector2(0.0f, font.MeasureString(manager.Id).Y);
      foreach (KeyValuePair<string, object> resource in manager.Resources)
      {
        string text = string.Format("{0}", (object) resource.Key.PadRight(40));
        Color color = Color.Yellow;
        if (resource.Value is Texture2D)
          color = Color.Orange;
        if (this.IsResourceDoubleUsed(manager, resource.Key))
          color = Color.Red;
        spriteBatch.DrawString(font, text, position, color);
        position += new Vector2(0.0f, font.MeasureString(text).Y);
        if ((double) position.Y + (double) font.MeasureString(text).Y > (double) BrainGame.ScreenHeight)
          position = new Vector2(position.X + 250f, 60f);
      }
      return position;
    }

    private bool IsResourceDoubleUsed(BrainContentManager ignoreManager, string resourceId)
    {
      foreach (KeyValuePair<string, BrainContentManager> resourceManager in this.ResourceManagers)
      {
        if (resourceManager.Value != ignoreManager && resourceManager.Value.Resources.ContainsKey(resourceId))
          return true;
      }
      return false;
    }

    public enum ResourceManagerCacheType
    {
      Temporary,
      Static,
      UserDefined,
    }
  }
}
