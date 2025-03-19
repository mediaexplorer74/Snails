
// Type: TwoBrainsGames.BrainEngine.UI.Controls.UIMenu
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.BrainEngine.UI.Controls
{
  public class UIMenu : UIControl
  {
    private MenuItemPlacement _itemPlacement;

    public UIMenuItemCollection Items { get; private set; }

    public TransformEffectBase ItemOnFocusEffect { get; set; }

    public UIMenuItem SelectedItem { get; private set; }

    public bool ShowDisabledItems { get; set; }

    public float ItemSpacing { get; set; }

    public int Columns { get; set; }

    public MenuItemPlacement ItemPlacement
    {
      get => this._itemPlacement;
      set
      {
        this._itemPlacement = value;
        this.RepositionItems();
      }
    }

    public UIMenu(UIScreen screenOwner)
      : base(screenOwner)
    {
      this.Columns = 1;
      this.Items = new UIMenuItemCollection(this);
      this.ItemPlacement = MenuItemPlacement.Vertical;
      this.ShowDisabledItems = true;
    }

    public override void Load() => this.RepositionItems();

    public override void Update(BrainGameTime gameTime)
    {
      foreach (UIMenuItem uiMenuItem in this.Items)
      {
        uiMenuItem.DropShadow = this.DropShadow;
        uiMenuItem.OnFocusEffect = this.ItemOnFocusEffect;
      }
    }

    internal void OnItemEnter(IUIControl sender)
    {
      this.SelectedItem = (UIMenuItem) sender;
      this.SelectedItem.BringToFront();
    }

    internal void OnItemLeave(IUIControl sender) => this.SelectedItem = (UIMenuItem) null;

    public void Refresh() => this.RepositionItems();

    public void RepositionItems()
    {
      if (this.ItemPlacement == MenuItemPlacement.Free)
        return;
      float x = 0.0f;
      float y = 0.0f;
      float num1 = 0.0f;
      float num2 = 0.0f;
      int num3 = this.Items.Count / this.Columns;
      int num4 = 0;
      int num5 = 1;
      for (int i = 0; i < this.Items.Count; ++i)
      {
        if (this.Items[i].Enabled || this.ShowDisabledItems)
        {
          ++num4;
          this.Items[i].Position = new Vector2(x, y);
          if ((double) this.Items[i].Position.X + (double) this.Items[i].Size.Width > (double) num1)
            num1 = this.Items[i].Position.X + this.Items[i].Size.Width;
          if ((double) this.Items[i].Position.Y + (double) this.Items[i].Size.Height > (double) num2)
            num2 = this.Items[i].Position.Y + this.Items[i].Size.Height;
          if (this.Columns > 1)
          {
            if (num5 == 1)
              this.Items[i].ParentAlignmentOffset = new Vector2((float) (-(double) this.Items[i].Size.Width / 2.0), 0.0f);
            else
              this.Items[i].ParentAlignmentOffset = new Vector2(this.Items[i].Size.Width / 2f, 0.0f);
          }
          bool flag = num4 == num3;
          switch (this.ItemPlacement)
          {
            case MenuItemPlacement.Horizontal:
              x += this.Items[i].Size.Width;
              continue;
            case MenuItemPlacement.Vertical:
              y += this.Items[i].Size.Height + this.ItemSpacing;
              if (flag)
              {
                y = 0.0f;
                x += this.Items[i].Size.Width;
                num4 = 0;
                ++num5;
                continue;
              }
              continue;
            default:
              continue;
          }
        }
      }
      this.AdjustSizeToFitItems();
    }

    public void AdjustSizeToFitItems()
    {
      if (this.ItemPlacement == MenuItemPlacement.Free || this.Items.Count == 0)
        return;
      float num1 = 999999f;
      float num2 = 999999f;
      float num3 = 0.0f;
      float num4 = 0.0f;
      for (int i = 0; i < this.Items.Count; ++i)
      {
        if ((double) this.Items[i].Position.X < (double) num1)
          num1 = this.Items[i].Position.X;
        if ((double) this.Items[i].Position.Y < (double) num2)
          num2 = this.Items[i].Position.Y;
        if ((double) this.Items[i].Position.X + (double) this.Items[i].Size.Width > (double) num3)
          num3 = this.Items[i].Position.X + this.Items[i].Size.Width;
        if ((double) this.Items[i].Position.Y + (double) this.Items[i].Size.Height > (double) num4)
          num4 = this.Items[i].Position.Y + this.Items[i].Size.Height;
      }
      this.Size = new Size(num3 - num1, num4 - num2);
    }
  }
}
