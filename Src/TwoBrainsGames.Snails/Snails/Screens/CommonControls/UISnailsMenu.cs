
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UISnailsMenu
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UISnailsMenu : UIControl
  {
    private const float SELECTED_ITEM_SCALE = 1f;
    private const float MENU_ITEMS_SPACING = -130f;
    private UIMenu _menu;
    private UITimer _tmrShow;
    private UITimer _hideTimer;
    private int _idxLastItemShowned;
    private UISnailsMenuTitle _title;
    private UISnailsMenu.MenuItemSize _itemSize;
    private Sample _menuClosedSample;
    private Sample _menuShownSound;
    private UIBackButton _btnBack;

    public event UIControl.UIEvent OnMenuShownBegin;

    public event UIControl.UIEvent OnMenuShown;

    public event UIControl.UIEvent OnItemSelectedBegin;

    public event UIControl.UIEvent OnMenuHideBegin;

    public event UIControl.UIEvent OnBackPressed;

    public TextFont MenuItemsFont { get; private set; }

    public TextFont MenuItemsFontUnselected { get; private set; }

    public int IdxLastItemSelected { get; set; }

    public UISnailsMenuItem ItemSelected { get; private set; }

    public int DefaultItemIndex { get; set; }

    public int Columns
    {
      get => this._menu.Columns;
      set => this._menu.Columns = value;
    }

    public override bool Visible
    {
      get => base.Visible;
      set
      {
        base.Visible = value;
        if (this.Visible)
          return;
        if (this._title != null)
          this._title.Visible = value;
        if (this._menu == null)
          return;
        foreach (UIControl uiControl in this._menu.Items)
          uiControl.Visible = value;
      }
    }

    public UISnailsMenuTitle.TitleSize TitleSize
    {
      get => this._title.BoardSize;
      set => this._title.BoardSize = value;
    }

    public UISnailsMenu.MenuItemSize ItemSize
    {
      get => this._itemSize;
      set => this._itemSize = value;
    }

    public override string TextResourceId
    {
      get => this._title.TextResourceId;
      set => this._title.TextResourceId = value;
    }

    private float TitleMenuSpacing { get; set; }

    private bool BackPressed { get; set; }

    public bool WithBackButton
    {
      get => this._btnBack.Visible;
      set => this._btnBack.Visible = value;
    }

    public int ItemCount => this._menu.Items.Count;

    public UISnailsMenu(UIScreen ownerScreen)
      : base(ownerScreen)
    {
      this.IdxLastItemSelected = 0;
      this.DefaultItemIndex = -1;
      this.DropShadow = false;
      this.ItemSize = UISnailsMenu.MenuItemSize.Medium;
      this.OnScreenStart += new UIControl.UIEvent(this.UISnailsMenu_OnScreenStart);
      this.OnInitializeFromContent += new UIControl.UIEvent(this.UISnailsMenu_OnInitializeFromContent);
      this._menuClosedSample = BrainGame.ResourceManager.GetSampleStatic("sfx/menu-closed");
      this._menuShownSound = BrainGame.ResourceManager.GetSampleStatic("sfx/menu-shown");
      this.ParentAlignment = AlignModes.Horizontaly;
      this.DropShadow = false;
      this._idxLastItemShowned = -1;
      this._tmrShow = new UITimer(ownerScreen, 10.0, true);
      this._tmrShow.Enabled = false;
      this._tmrShow.OnTimer += new UIControl.UIEvent(this.ShowTimer_OnTimer);
      this.Controls.Add((UIControl) this._tmrShow);
      this._hideTimer = new UITimer(ownerScreen, 400.0, false);
      this._hideTimer.Enabled = false;
      this._hideTimer.OnTimer += new UIControl.UIEvent(this._hideTimer_OnTimer);
      this.Controls.Add((UIControl) this._hideTimer);
      this.MenuItemsFont = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-medium", ResourceManager.ResourceManagerCacheType.Static);
      this.MenuItemsFontUnselected = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-medium-2", ResourceManager.ResourceManagerCacheType.Static);
      this._title = new UISnailsMenuTitle(ownerScreen);
      this._title.AcceptControllerInput = true;
      this.Controls.Add((UIControl) this._title);
      this._btnBack = new UIBackButton(ownerScreen);
      this._btnBack.Effect = (ITransformEffect) new HooverEffect(0.2f, 0.5f, -90f);
      this._btnBack.Position = new Vector2(0.0f, 150f);
      this._btnBack.OnAccept += new UIControl.UIEvent(this._btnBack_OnAccept);
      this._btnBack.AcceptControllerInput = true;
      this._btnBack.OnAcceptEffect = (TransformEffectBase) new ColorEffect(Color.White, Color.Gray, 0.4f, true, Color.White, 130.0);
      this._btnBack.ControllerActionCode = 2;
      this._title.Controls.Add((UIControl) this._btnBack);
      this._menu = new UIMenu(ownerScreen);
      this._menu.ShowDisabledItems = false;
      this._menu.ParentAlignment = AlignModes.Horizontaly;
      this._menu.DropShadow = false;
      this._menu.ItemSpacing = -130f;
      this.Controls.Add((UIControl) this._menu);
      this.WithBackButton = false;
    }

    private void _btnBack_OnAccept(IUIControl sender)
    {
      ((SnailsScreen) this.ScreenOwner).DisableInput();
      this.BackPressed = true;
      this.Hide();
    }

    public override void InitializeFromContent()
    {
      base.InitializeFromContent();
      this.InitializeFromContent(nameof (UISnailsMenu));
    }

    private void UISnailsMenu_OnInitializeFromContent(IUIControl sender)
    {
      this.TitleMenuSpacing = this.GetContentPropertyValue<float>("titleMenuSpacing", this.TitleMenuSpacing);
    }

    private void UISnailsMenu_OnScreenStart(IUIControl sender) => this.UpdateSize();

    public void ShowWithoutEffects()
    {
      this.BackPressed = false;
      this.Visible = true;
      this._title.Visible = true;
      this.ItemSelected = (UISnailsMenuItem) null;
      this._idxLastItemShowned = -1;
      for (int i = 0; i < this._menu.Items.Count; ++i)
      {
        ((UISnailsMenuItem) this._menu.Items[i]).Initialize();
        if (this._menu.Items[i].Enabled)
          this._menu.Items[i].Visible = true;
      }
      this._menu.Refresh();
    }

    public override void Show()
    {
      this.BackPressed = false;
      if (this.Visible)
        return;
      if (this.OnMenuShownBegin != null)
        this.OnMenuShownBegin((IUIControl) this);
      this.ItemSelected = (UISnailsMenuItem) null;
      this.Visible = true;
      this._tmrShow.Reset();
      this._idxLastItemShowned = -1;
      this._title.Show();
      this._tmrShow.Enabled = true;
      if (this._menu.Items.Count > 0)
      {
        this._menu.Items[this._menu.Items.Count - 1].OnShow -= new UIControl.UIEvent(this.UISnailsMenu_OnShow);
        this._menu.Items[this._menu.Items.Count - 1].OnShow += new UIControl.UIEvent(this.UISnailsMenu_OnShow);
      }
      for (int i = 0; i < this._menu.Items.Count; ++i)
      {
        if (this._menu.Items[i].Enabled)
          ((UISnailsMenuItem) this._menu.Items[i]).Initialize();
      }
      this._menu.Refresh();
      this._menuShownSound.Play();
    }

    public override void Hide()
    {
      this._menuClosedSample.Play();
      this.AcceptControllerInput = false;
      foreach (UIControl uiControl in this._menu.Items)
        uiControl.Hide();
      this._title.Hide();
      this._hideTimer.Enabled = true;
      if (this.OnMenuHideBegin == null)
        return;
      this.OnMenuHideBegin((IUIControl) this);
    }

    public UISnailsMenuItem AddMenuItem(
      string textResourceId,
      UIControl.UIEvent acceptEvent,
      InputBase.InputActions controllerActionCode)
    {
      return this.AddMenuItem(textResourceId, acceptEvent, controllerActionCode, true);
    }

    public UISnailsMenuItem AddMenuItem(
      string textResourceId,
      UIControl.UIEvent acceptEvent,
      InputBase.InputActions controllerActionCode,
      bool autohideMenu,
      bool shouldCreate)
    {
      if (!shouldCreate)
        return (UISnailsMenuItem) null;
      UISnailsMenuItem uiSnailsMenuItem = new UISnailsMenuItem(this.ScreenOwner, textResourceId, this.MenuItemsFont, this, true);
      uiSnailsMenuItem.AutoHideMenu = autohideMenu;
      this.InitMenuItem(uiSnailsMenuItem, acceptEvent, controllerActionCode, true);
      return uiSnailsMenuItem;
    }

    public UISnailsMenuItem AddMenuItem(
      string textResourceId,
      UIControl.UIEvent acceptEvent,
      InputBase.InputActions controllerActionCode,
      bool autohideMenu)
    {
      return this.AddMenuItem(textResourceId, acceptEvent, controllerActionCode, autohideMenu, true);
    }

    public UISnailsSliderMenuItem AddSliderItem(string textResource)
    {
      UISnailsSliderMenuItem snailsSliderMenuItem = new UISnailsSliderMenuItem(this.ScreenOwner, this);
      this.InitMenuItem((UISnailsMenuItem) snailsSliderMenuItem, (UIControl.UIEvent) null, InputBase.InputActions.None, false);
      snailsSliderMenuItem.TextResourceId = textResource;
      return snailsSliderMenuItem;
    }

    private void InitMenuItem(
      UISnailsMenuItem item,
      UIControl.UIEvent acceptEvent,
      InputBase.InputActions controllerActionCode,
      bool withAccept)
    {
      item.Visible = this.Visible;
      item.OnFocus += new UIControl.UIEvent(this.MenuItem_OnFocus);
      item.OnLostFocus += new UIControl.UIEvent(this.MenuItem_OnLostFocus);
      if (withAccept)
      {
        item.OnAcceptBegin += new UIControl.UIEvent(this.MenuItem_OnAcceptBegin);
        item.OnAcceptEffect = (TransformEffectBase) new ColorEffect(Color.White, Color.Gray, 0.4f, true, Color.White, 100.0);
        item.OnAccept += new UIControl.UIEvent(this.MenuItem_OnAccept);
      }
      item.OnSelect += acceptEvent;
      item.ControllerActionCode = (int) controllerActionCode;
      if (this.Columns > 1)
        item.ParentAlignment = AlignModes.None;
      this._menu.Items.Add((UIMenuItem) item);
      this.UpdateSize();
    }

    public void UpdateSize()
    {
      this._menu.Position = new Vector2(0.0f, this._title.Position.Y + this.TitleMenuSpacing + this._title.Size.Height);
      this._menu.AdjustSizeToFitItems();
      this.Size = new Size(Math.Max(this._title.Size.Width, this._menu.Width), this._title.Size.Height + this.TitleMenuSpacing + this._menu.Size.Height);
    }

    public void SetFocus(int itemIdx)
    {
      if (this.ScreenOwner.CursorMode != CursorModes.SnapToControl)
        return;
      this._menu.Items[itemIdx].Focus();
    }

    public void SetFocus(UISnailsMenuItem item)
    {
      if (this.ScreenOwner.CursorMode != CursorModes.SnapToControl)
        return;
      for (int index = 0; index < this._menu.Items.Count; ++index)
      {
        if (item == this._menu.Items[index])
        {
          this.SetFocus(index);
          break;
        }
      }
    }

    public void SetFocusOnLastSelectedItem()
    {
      if (this.ScreenOwner.CursorMode != CursorModes.SnapToControl || this.IdxLastItemSelected < 0 || this.IdxLastItemSelected >= this._menu.Items.Count)
        return;
      this._menu.Items[this.IdxLastItemSelected].Focus();
    }

    private void ShowTimer_OnTimer(IUIControl sender)
    {
      ++this._idxLastItemShowned;
      if (this._idxLastItemShowned >= this._menu.Items.Count || !this._menu.Items[this._idxLastItemShowned].Enabled)
        return;
      this._menu.Items[this._idxLastItemShowned].Show();
    }

    private void UISnailsMenu_OnShow(IUIControl sender)
    {
      this._tmrShow.Enabled = false;
      this.AcceptControllerInput = true;
      if (this.DefaultItemIndex >= 0 && this.DefaultItemIndex < this._menu.Items.Count)
      {
        this.Focus();
        this.SetFocus(this.DefaultItemIndex);
      }
      if (this.OnMenuShown == null)
        return;
      this.OnMenuShown((IUIControl) null);
    }

    private void MenuItem_OnLostFocus(IUIControl sender)
    {
      UISnailsMenuItem uiSnailsMenuItem = (UISnailsMenuItem) sender;
      if (this.ItemSelected == uiSnailsMenuItem)
        return;
      uiSnailsMenuItem.Reset();
    }

    private void MenuItem_OnFocus(IUIControl sender)
    {
      if (((UISnailsMenuItem) sender).WithFocus)
        return;
      this.ResetAll();
      ((UISnailsMenuItem) sender).GotFocus();
    }

    private void ResetAll()
    {
      foreach (UISnailsMenuItem uiSnailsMenuItem in this._menu.Items)
        uiSnailsMenuItem.Reset();
    }

    private void MenuItem_OnAccept(IUIControl sender)
    {
      UISnailsMenuItem uiSnailsMenuItem = (UISnailsMenuItem) sender;
      if (uiSnailsMenuItem.AutoHideMenu)
      {
        this.Hide();
      }
      else
      {
        uiSnailsMenuItem.Reset();
        this.InvokeItemSelected();
        this.ItemSelected = (UISnailsMenuItem) null;
      }
      this.IdxLastItemSelected = this.GetItemIndex((UISnailsMenuItem) sender);
    }

    private void MenuItem_OnAcceptBegin(IUIControl sender)
    {
      ((SnailsScreen) this.ScreenOwner).DisableInput();
      UISnailsMenuItem sender1 = (UISnailsMenuItem) sender;
      this.ItemSelected = sender1;
      if (!this.ItemSelected.WithFocus)
        this.ItemSelected.Focus();
      if (this.OnItemSelectedBegin == null)
        return;
      this.OnItemSelectedBegin((IUIControl) sender1);
    }

    private void _hideTimer_OnTimer(IUIControl sender)
    {
      this.Visible = false;
      if (this.BackPressed)
      {
        if (this.OnBackPressed == null)
          return;
        this.OnBackPressed((IUIControl) this);
      }
      else
        this.InvokeItemSelected();
    }

    private void InvokeItemSelected()
    {
      if (this.ItemSelected == null)
        return;
      this.ItemSelected.InvokeOnSelect();
    }

    private int GetItemIndex(UISnailsMenuItem toFind)
    {
      for (int i = 0; i < this._menu.Items.Count; ++i)
      {
        if (this._menu.Items[i] == toFind)
          return i;
      }
      return -1;
    }

    public enum MenuItemSize
    {
      Medium,
      Big,
    }
  }
}
