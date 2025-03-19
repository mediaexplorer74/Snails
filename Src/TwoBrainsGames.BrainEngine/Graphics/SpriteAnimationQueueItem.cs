
// Type: TwoBrainsGames.BrainEngine.Graphics.SpriteAnimationQueueItem
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Resources;


namespace TwoBrainsGames.BrainEngine.Graphics
{
  public class SpriteAnimationQueueItem
  {
    public SpriteAnimation _animation;
    public Vector2 _position;
    public bool _wait;
    public bool _ended;

    public SpriteAnimationQueueItem.LastFrameCallBackDelegate LastFrameCallback { get; set; }

    public bool DrawFirstFrame { get; set; }

    public bool IsActive { get; set; }

    public Sample Sound { get; set; }

    public SpriteAnimationQueueItem(
      SpriteAnimation animationSprite,
      Vector2 position,
      bool drawFirstFrame,
      bool wait)
      : this(animationSprite, position, drawFirstFrame, wait, (SpriteAnimationQueueItem.LastFrameCallBackDelegate) null)
    {
    }

    public SpriteAnimationQueueItem(Sprite sprite, Vector2 position, bool wait)
      : this(new SpriteAnimation(sprite), position, false, wait)
    {
    }

    public SpriteAnimationQueueItem(
      SpriteAnimation animationSprite,
      Vector2 position,
      bool drawFirstFrame,
      bool wait,
      SpriteAnimationQueueItem.LastFrameCallBackDelegate lastFrameCallback)
    {
      this._position = position;
      this._wait = wait;
      this._animation = animationSprite;
      this._animation.OnLastFrame += new SpriteAnimation.LastFrameHandler(this.OnLastFrame);
      this.LastFrameCallback = lastFrameCallback;
      this.DrawFirstFrame = drawFirstFrame;
    }

    public void Update(BrainGameTime gameTime) => this._animation.Update(gameTime);

    public void Draw(Vector2 position, Color blendColor, SpriteBatch spriteBatch)
    {
      this._animation.Draw(position + this._position, blendColor, spriteBatch);
    }

    private void OnLastFrame()
    {
      this._ended = true;
      if (this.LastFrameCallback == null)
        return;
      this.LastFrameCallback();
    }

    public delegate void LastFrameCallBackDelegate();
  }
}
