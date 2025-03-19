
// Type: TwoBrainsGames.BrainEngine.Input.InputRecorderStream
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System.Collections.Generic;


namespace TwoBrainsGames.BrainEngine.Input
{
  public class InputRecorderStream
  {
    private List<InputRecorderStream.StreamItem> _items;

    public int ItemsCount => this._items.Count;

    public InputRecorderStream.StreamItem this[int key] => this._items[key];

    public InputRecorderStream() => this._items = new List<InputRecorderStream.StreamItem>();

    public void Append(double milliseconds, ulong action, Vector2 position)
    {
      if (this._items.Count > 0 && (long) this._items[this._items.Count - 1]._inputAction == (long) action && this._items[this._items.Count - 1]._position == position)
        return;
      this._items.Add(new InputRecorderStream.StreamItem(milliseconds, action, position));
    }

    public void Clear() => this._items.Clear();

    public class StreamItem
    {
      public double _milliseconds;
      public ulong _inputAction;
      public Vector2 _position;

      public StreamItem()
      {
      }

      public StreamItem(double milliseconds, ulong inputAction, Vector2 position)
      {
        this._milliseconds = milliseconds;
        this._inputAction = inputAction;
        this._position = position;
      }
    }
  }
}
