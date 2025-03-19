
// Type: TwoBrainsGames.Snails.Tutorials.TutorialImage
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;


namespace TwoBrainsGames.Snails.Tutorials
{
  internal class TutorialImage : TutorialItem
  {
    private string _imageResource;
    private Sprite _sprite;

    public TutorialImage(string imageResource) => this._imageResource = imageResource;

    public override void LoadContent()
    {
      this._sprite = BrainGame.ResourceManager.GetSpriteTemporary(this._imageResource);
      this._displayTime = 1000f;
    }

    public override void Draw(Vector2 topicTopLefPosition, Color color, SpriteBatch spriteBatch)
    {
      this._sprite.Draw(topicTopLefPosition + this.Position, 0, 0.0f, Vector2.Zero, this._parentTopic._scale.X, this._parentTopic._scale.Y, color, spriteBatch);
    }

    public override float GetWidth() => (float) this._sprite.Width;
  }
}
