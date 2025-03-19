
// Type: TwoBrainsGames.Snails.Screens.QuitGameScreen
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.Snails.Screens
{
  internal class QuitGameScreen(ScreenNavigator owner) : UIScreen(owner)
  {
    public override void OnUpdate(BrainGameTime gameTime)
    {
      Game1.ProfilesManager.Save();
      BrainGame.QuitGame();
    }
  }
}
