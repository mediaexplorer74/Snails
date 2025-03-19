
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UILanguageMenu
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Configuration;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UILanguageMenu : UIControl
  {
    private UISnailsMenu _menu;

    public event UIControl.UIEvent OnLanguageSelected;

    public override bool Visible
    {
      get => base.Visible;
      set => base.Visible = value;
    }

    public UILanguageMenu(UIScreen screenOwner)
      : base(screenOwner)
    {
      this._menu = new UISnailsMenu(screenOwner);
      this._menu.Size = new Size(3000f, 3800f);
      this._menu.Visible = false;
      this._menu.TextResourceId = "MNU_LANGUAGE";
      this._menu.DefaultItemIndex = 1;
      this._menu.OnMenuShown += new UIControl.UIEvent(this._menu_OnMenuShown);
      this._menu.WithBackButton = true;
      this._menu.OnBackPressed += new UIControl.UIEvent(this.MenuItem_OnBack);
      this._menu.OnSizeChanged += new UIControl.UIEvent(this._menu_OnSizeChanged);
      this.Controls.Add((UIControl) this._menu);
      if ((Game1.GameSettings.Languages & LanguageType.English) == LanguageType.English)
        this._menu.AddMenuItem("MNU_ITEM_ENGLISH", new UIControl.UIEvent(this.MenuItem_OnEnglish), InputBase.InputActions.None);
      if ((Game1.GameSettings.Languages & LanguageType.Portuguese) == LanguageType.Portuguese)
        this._menu.AddMenuItem("MNU_ITEM_PORTUGUESE", new UIControl.UIEvent(this.MenuItem_OnPortuguese), InputBase.InputActions.None);
      if ((Game1.GameSettings.Languages & LanguageType.French) == LanguageType.French)
        this._menu.AddMenuItem("MNU_ITEM_FRENCH", new UIControl.UIEvent(this.MenuItem_OnFrench), InputBase.InputActions.None);
      if ((Game1.GameSettings.Languages & LanguageType.Spanish) == LanguageType.Spanish)
        this._menu.AddMenuItem("MNU_ITEM_SPANISH", new UIControl.UIEvent(this.MenuItem_OnSpanish), InputBase.InputActions.None);
      if ((Game1.GameSettings.Languages & LanguageType.Italian) == LanguageType.Italian)
        this._menu.AddMenuItem("MNU_ITEM_ITALIAN", new UIControl.UIEvent(this.MenuItem_OnItalian), InputBase.InputActions.None);
      if ((Game1.GameSettings.Languages & LanguageType.German) == LanguageType.German)
        this._menu.AddMenuItem("MNU_ITEM_GERMAN", new UIControl.UIEvent(this.MenuItem_OnGerman), InputBase.InputActions.None);
      this.Size = this._menu.Size;
      this.Position = new Vector2(0.0f, 4500f);
      this.ParentAlignment = AlignModes.Horizontaly;
      this._menu.Columns = 1;
      if (Game1.GameSettings.PresentationMode == GameSettings.PresentationType.LD && this._menu.ItemCount > 3)
        this._menu.Columns = 2;
      this._menu.Columns = Game1.GameSettings.PresentationMode == GameSettings.PresentationType.LD ? 2 : 1;
    }

    private void _menu_OnSizeChanged(IUIControl sender) => this.Size = this._menu.Size;

    private void _menu_OnMenuShown(IUIControl sender) => this.InvokeOnShow();

    private void MenuItem_OnEnglish(IUIControl sender) => this.SetLanguage(LanguageCode.en);

    private void MenuItem_OnPortuguese(IUIControl sender) => this.SetLanguage(LanguageCode.pt);

    private void MenuItem_OnFrench(IUIControl sender) => this.SetLanguage(LanguageCode.fr);

    private void MenuItem_OnSpanish(IUIControl sender) => this.SetLanguage(LanguageCode.es);

    private void MenuItem_OnItalian(IUIControl sender) => this.SetLanguage(LanguageCode.it);

    private void MenuItem_OnGerman(IUIControl sender) => this.SetLanguage(LanguageCode.de);

    private void SetLanguage(LanguageCode languageCode)
    {
      BrainGame.CurrentLanguage = languageCode;
      if (this.OnLanguageSelected == null)
        return;
      this.OnLanguageSelected((IUIControl) this);
    }

    private void MenuItem_OnBack(IUIControl sender) => this.Close();

    public override void Show()
    {
      this.Visible = true;
      this._menu.Show();
    }

    private void Close()
    {
      this.Visible = false;
      this.InvokeOnHide();
    }

    public void SetFocus(int itemIdx) => this._menu.SetFocus(itemIdx);
  }
}
