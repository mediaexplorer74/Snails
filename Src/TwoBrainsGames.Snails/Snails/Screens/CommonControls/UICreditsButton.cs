
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UICreditsButton
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UICreditsButton(UIScreen screenOwner, UIControl.UIEvent pressCallback) : 
    UISnailsButton(screenOwner, "MNU_ITEM_CREDITS", UISnailsButton.ButtonSizeType.Small, InputBase.InputActions.None, pressCallback, false)
  {
  }
}
