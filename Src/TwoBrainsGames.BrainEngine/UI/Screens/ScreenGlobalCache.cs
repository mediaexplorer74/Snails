
// Type: TwoBrainsGames.BrainEngine.UI.Screens.ScreenGlobalCache
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using System.Collections.Generic;


namespace TwoBrainsGames.BrainEngine.UI.Screens
{
  public class ScreenGlobalCache
  {
    private Dictionary<string, object> Items { get; set; }

    public object this[string key] => this.Items.ContainsKey(key) ? this.Items[key] : (object) null;

    internal ScreenGlobalCache() => this.Items = new Dictionary<string, object>();

    public void Set(string key, object data)
    {
      if (this.Items.ContainsKey(key))
        this.Items[key] = data;
      else
        this.Items.Add(key, data);
    }

    public void Remove(string key)
    {
      if (!this.Items.ContainsKey(key))
        return;
      this.Items.Remove(key);
    }

    public void Clear() => this.Items.Clear();

    public T Get<T>(string key, T defaultVal)
    {
      return this.Items.ContainsKey(key) ? (T) this.Items[key] : defaultVal;
    }

    public T Get<T>(string key) => this.Items.ContainsKey(key) ? (T) this.Items[key] : default (T);

    public bool Contains(string key) => this.Items.ContainsKey(key);
  }
}
