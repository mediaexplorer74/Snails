
// Type: TwoBrainsGames.BrainEngine.UI.SoftwareCursor
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine.Graphics;


namespace TwoBrainsGames.BrainEngine.UI
{
  public class SoftwareCursor : Cursor
  {
    protected Sprite _cursorSprite;
    protected Dictionary<int, Sprite> _cursorSprites;

    public SoftwareCursor() => this._cursorSprites = new Dictionary<int, Sprite>();

    public override void SetCursor(int id) => this._cursorSprite = this._cursorSprites[id];

    public override void LoadCursor(string resourceName, int id)
    {
      if (this.IsCursorLoaded(id))
        return;
      string directoryName = BrainPath.GetDirectoryName(resourceName);
      string fileName = BrainPath.GetFileName(resourceName);
      this._cursorSprites.Add(id, BrainGame.ResourceManager.GetSpriteStatic(directoryName, fileName));
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      if (!this.Visible || this._cursorSprite == null)
        return;
      spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, (DepthStencilState) null, (RasterizerState) null, (Effect) BrainGame.RenderEffect);
      this._cursorSprite.Draw(this.Position, 0, spriteBatch);
      spriteBatch.End();
    }

    protected bool IsCursorLoaded(int name) => this._cursorSprites.ContainsKey(name);
  }
}
