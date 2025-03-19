
// Type: TwoBrainsGames.Snails.Screens.StartupScreen
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;


namespace TwoBrainsGames.Snails.Screens
{
  internal class StartupScreen : UIScreen
  {
    public StartupScreen(ScreenNavigator owner)
      : base(owner)
    {
      this.BackgroundColor = Color.White;
    }

    public override void OnUpdate(BrainGameTime gameTime)
    {
      this.NavigateTo(ScreenType.BrainsLogo.ToString(), (Transition) null, (Transition) null);
    }
  }
}
