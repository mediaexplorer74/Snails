
// Type: TwoBrainsGames.BrainEngine.UI.Controls.ControlCollection
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using System.Collections;
using System.Collections.Generic;


namespace TwoBrainsGames.BrainEngine.UI.Controls
{
  public class ControlCollection : IEnumerable
  {
    private List<UIControl> _items;
    private ControlCollectionEnumerator _enumerator;
    private IUIControl _ownerControl;

    public int Count => this._items.Count;

    public UIControl this[int i] => this._items[i];

    internal ControlCollection(IUIControl owner)
    {
      this._items = new List<UIControl>();
      this._enumerator = new ControlCollectionEnumerator(this._items);
      this._ownerControl = owner;
    }

    public void Add(UIControl control)
    {
      if (control == null || this._ownerControl is UIControl && !((UIControl) this._ownerControl).InvokeOnBeforeControlAdded(control))
        return;
      this._items.Add(control);
      control.Parent = this._ownerControl;
    }

    public void Remove(UIControl control)
    {
      if (control == null)
        return;
      this._items.Remove(control);
      control.Parent = (IUIControl) null;
    }

    public void RemoveAt(int idx)
    {
      UIControl uiControl = this._items[idx];
      this._items.RemoveAt(idx);
      uiControl.Parent = (IUIControl) null;
    }

    public void Insert(int idx, UIControl control)
    {
      if (control == null)
        return;
      this._items.Insert(idx, control);
      control.Parent = this._ownerControl;
    }

    public void Clear()
    {
      foreach (UIControl uiControl in this._items)
        uiControl.Parent = (IUIControl) null;
      this._items.Clear();
    }

    public bool Contains(UIControl control)
    {
      if (control != null)
      {
        foreach (UIControl uiControl in this._items)
        {
          if (control == uiControl)
            return true;
        }
      }
      return false;
    }

    public void InsertAt(int index, UIControl control)
    {
      if (control == null)
        return;
      this._items.Insert(index, control);
      control.Parent = this._ownerControl;
    }

    public override string ToString() => string.Format("Count: {0}", (object) this._items.Count);

    public IEnumerator GetEnumerator()
    {
      return (IEnumerator) new ControlCollectionEnumerator(this._items);
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this._enumerator;
  }
}
