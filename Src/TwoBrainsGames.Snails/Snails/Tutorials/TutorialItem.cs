
// Type: TwoBrainsGames.Snails.Tutorials.TutorialItem
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace TwoBrainsGames.Snails.Tutorials
{
  public class TutorialItem
  {
    public float _displayTime;
    public TutorialTopic _parentTopic;

    public Vector2 Position { get; set; }

    public virtual void LoadContent()
    {
    }

    public virtual void Draw(Vector2 topicTopLefPosition, Color color, SpriteBatch spriteBatch)
    {
    }

    public virtual float GetWidth() => 0.0f;
  }
}
