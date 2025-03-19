
// Type: TwoBrainsGames.BrainEngine.Graphics.SpriteAnimationQueue
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;


namespace TwoBrainsGames.BrainEngine.Graphics
{
  public class SpriteAnimationQueue
  {
    private bool _loop;
    private int _currentItemIndex;
    private List<SpriteAnimationQueueItem> _items;
    private List<SpriteAnimationQueueItem> _activeItems;

    public event SpriteAnimationQueue.AnimationEndedHandler OnAnimationEnded;

    public bool Active { get; private set; }

    public SpriteAnimationQueue(bool loop, bool active)
    {
      this._items = new List<SpriteAnimationQueueItem>();
      this._activeItems = new List<SpriteAnimationQueueItem>();
      this._loop = loop;
      this.Active = false;
    }

    public void AddItem(SpriteAnimationQueueItem item) => this._items.Add(item);

    public SpriteAnimationQueueItem AddItem(
      SpriteAnimation animation,
      Vector2 position,
      bool drawFirstFrame,
      bool wait)
    {
      return this.AddItem(animation, position, drawFirstFrame, wait, (string) null, (Object2D) null);
    }

    public SpriteAnimationQueueItem AddItem(
      SpriteAnimation animation,
      Vector2 position,
      bool drawFirstFrame,
      bool wait,
      string soundResource,
      Object2D autidoEmmiter)
    {
      return this.AddItem(animation, position, drawFirstFrame, wait, soundResource, autidoEmmiter, (SpriteAnimationQueueItem.LastFrameCallBackDelegate) null);
    }

    public SpriteAnimationQueueItem AddItem(
      SpriteAnimation animation,
      Vector2 position,
      bool drawFirstFrame,
      bool wait,
      SpriteAnimationQueueItem.LastFrameCallBackDelegate lastFrameCallback)
    {
      return this.AddItem(animation, position, drawFirstFrame, wait, (string) null, (Object2D) null, lastFrameCallback);
    }

    public SpriteAnimationQueueItem AddItem(
      SpriteAnimation animation,
      Vector2 position,
      bool drawFirstFrame,
      bool wait,
      string soundResource,
      Object2D autidoEmmiter,
      SpriteAnimationQueueItem.LastFrameCallBackDelegate lastFrameCallback)
    {
      SpriteAnimationQueueItem animationQueueItem = new SpriteAnimationQueueItem(animation, position, drawFirstFrame, wait, lastFrameCallback);
      if (!string.IsNullOrEmpty(soundResource))
        animationQueueItem.Sound = BrainGame.ResourceManager.GetSampleTemporary(soundResource, autidoEmmiter);
      this.AddItem(animationQueueItem);
      return animationQueueItem;
    }

    public void Reset()
    {
      this._activeItems.Clear();
      this._currentItemIndex = -1;
      if (!this.Active)
        return;
      this.ActivateNextItems();
    }

    public void Update(BrainGameTime gameTime)
    {
      if (!this.Active || this._activeItems.Count == 0)
        return;
      for (int index = 0; index < this._activeItems.Count; ++index)
      {
        this._activeItems[index].Update(gameTime);
        if (this._activeItems[index]._ended)
        {
          this._activeItems[index].IsActive = false;
          this._activeItems[index]._animation.CurrentFrame = 0;
          this._activeItems.RemoveAt(index);
          --index;
        }
      }
      if (this._activeItems.Count != 0)
        return;
      if (this._currentItemIndex >= this._items.Count - 1)
      {
        if (this.OnAnimationEnded != null)
          this.OnAnimationEnded();
        if (!this._loop)
        {
          this.Active = false;
          this.Reset();
          return;
        }
        this._currentItemIndex = -1;
      }
      this.ActivateNextItems();
    }

    public void Draw(Vector2 position, Color blendColor, SpriteBatch spriteBatch)
    {
      foreach (SpriteAnimationQueueItem animationQueueItem in this._items)
      {
        if (animationQueueItem.IsActive || animationQueueItem.DrawFirstFrame)
          animationQueueItem.Draw(position, blendColor, spriteBatch);
      }
    }

    private void ActivateNextItems()
    {
      ++this._currentItemIndex;
      int currentItemIndex = this._currentItemIndex;
      while (currentItemIndex < this._items.Count)
      {
        this._items[currentItemIndex]._ended = false;
        this._items[currentItemIndex].IsActive = true;
        if (this._items[currentItemIndex].DrawFirstFrame)
          this._items[currentItemIndex]._animation.CurrentFrame = 1;
        this._activeItems.Add(this._items[currentItemIndex]);
        if (this._items[currentItemIndex].Sound != null)
          this._items[currentItemIndex].Sound.Play();
        if (this._items[currentItemIndex]._wait)
          break;
        ++currentItemIndex;
        ++this._currentItemIndex;
      }
    }

    public void Activate()
    {
      this.Active = true;
      this.ActivateNextItems();
    }

    public delegate void AnimationEndedHandler();
  }
}
