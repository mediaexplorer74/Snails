
// Type: TwoBrainsGames.BrainEngine.UI.Controls.UIMenuItemCollection
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using System.Collections;
using System.Collections.Generic;


namespace TwoBrainsGames.BrainEngine.UI.Controls
{
  public class UIMenuItemCollection : IEnumerable
  {
    private UIMenu OwnerMenu;

    public int Count => this.Items.Count;

    private List<UIMenuItem> Items { get; set; }

    private UIMenuItemCollectionEnumerator Enumerator { get; set; }

    public UIMenuItem this[int i] => this.Items[i];

    internal UIMenuItemCollection(UIMenu owner)
    {
      this.Items = new List<UIMenuItem>();
      this.Enumerator = new UIMenuItemCollectionEnumerator(this.Items);
      this.OwnerMenu = owner;
    }

    public void Add(UIMenuItem menuItem)
    {
      if (menuItem == null)
        return;
      this.Items.Add(menuItem);
      menuItem.Parent = (IUIControl) this.OwnerMenu;
      menuItem.Parent.Controls.Add((UIControl) menuItem);
      this.OwnerMenu.RepositionItems();
      menuItem.OnFocus += new UIControl.UIEvent(this.OwnerMenu.OnItemEnter);
      menuItem.OnLostFocus += new UIControl.UIEvent(this.OwnerMenu.OnItemLeave);
    }

    public void Remove(UIMenuItem menuItem)
    {
      if (menuItem == null)
        return;
      this.Items.Remove(menuItem);
      menuItem.Parent.Controls.Remove((UIControl) menuItem);
      this.OwnerMenu.RepositionItems();
      menuItem.Parent = (IUIControl) null;
    }

    public void Clear()
    {
      foreach (UIMenuItem control in this.Items)
      {
        control.Parent.Controls.Remove((UIControl) control);
        control.Parent = (IUIControl) null;
      }
      this.Items.Clear();
      this.OwnerMenu.RepositionItems();
    }

    public override string ToString() => string.Format("Count: {0}", (object) this.Items.Count);

    public IEnumerator GetEnumerator()
    {
      return (IEnumerator) new UIMenuItemCollectionEnumerator(this.Items);
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.Enumerator;
  }
}
