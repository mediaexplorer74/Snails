
// Type: TwoBrainsGames.BrainEngine.UI.Controls.UIMenuItemCollectionEnumerator
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using System;
using System.Collections;
using System.Collections.Generic;


namespace TwoBrainsGames.BrainEngine.UI.Controls
{
  public class UIMenuItemCollectionEnumerator : IEnumerator
  {
    public List<UIMenuItem> _items;
    private int _position = -1;

    public UIMenuItemCollectionEnumerator(List<UIMenuItem> items) => this._items = items;

    public bool MoveNext()
    {
      ++this._position;
      return this._position < this._items.Count;
    }

    public void Reset() => this._position = -1;

    object IEnumerator.Current => (object) this.Current;

    public UIControl Current
    {
      get
      {
        try
        {
          return (UIControl) this._items[this._position];
        }
        catch (IndexOutOfRangeException ex)
        {
          throw new InvalidOperationException();
        }
      }
    }
  }
}
