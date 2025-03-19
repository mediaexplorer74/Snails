
// Type: TwoBrainsGames.Snails.Tutorials.TutorialText
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Resources;


namespace TwoBrainsGames.Snails.Tutorials
{
  internal class TutorialText : TutorialItem
  {
    private string _text;
    public TextFont _font;
    public Color _color;

    public TutorialText(string text, Color color)
    {
      this._text = text;
      this._displayTime = (float) (text.Length * 40);
      this._color = color;
    }

    public override void LoadContent() => this._font = this._parentTopic._font;

    public override void Draw(Vector2 topicTopLefPosition, Color color, SpriteBatch spriteBatch)
    {
      color = !(color != Color.White) ? this._color : new Color(color.ToVector4() * this._color.ToVector4());
      this._font.DrawString(spriteBatch, this._text, topicTopLefPosition + this.Position, this._parentTopic._scale, color);
    }

    public override float GetWidth()
    {
      return this._font.MeasureString(this._text, this._parentTopic._scale);
    }

    public float GetHeight()
    {
      return this._font.MeasureStringHeight(this._text, this._parentTopic._scale);
    }
  }
}
