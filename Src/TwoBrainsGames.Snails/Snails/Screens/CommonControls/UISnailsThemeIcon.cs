
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UISnailsThemeIcon
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  public class UISnailsThemeIcon(UIScreen screenOwner) : UIImage(screenOwner)
  {
    private ThemeType _theme;

    public ThemeType Theme
    {
      get => this._theme;
      set
      {
        this._theme = value;
        this.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/common-elements-1/" + this._theme.ToString() + "ThemeIcon");
      }
    }
  }
}
