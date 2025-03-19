
// Type: TwoBrainsGames.BrainEngine.Resources.Image
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


namespace TwoBrainsGames.BrainEngine.Resources
{
  public class Image : Resource
  {
    private Texture2D _texture;

    public Texture2D Texture
    {
      get => this._texture;
      set => this._texture = value;
    }

    public override bool Load(ContentManager contentManager)
    {
      this.IsLoaded = false;
      this._texture = contentManager.Load<Texture2D>(this.Path);
      if (this._texture != null)
        this.IsLoaded = true;
      return this.IsLoaded;
    }

    public override bool Release(ContentManager contentManager)
    {
      if (this._texture != null)
        this._texture.Dispose();
      this.IsLoaded = false;
      return true;
    }
  }
}
