
// Type: TwoBrainsGames.Snails.Screens.ThemeSelection.UIThemeLD
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.Snails.Screens.ThemeSelection
{
  public class UIThemeLD : UITheme
  {
    public UIThemeLD(UIScreen screenOwner, ThemeType themeId)
      : base(screenOwner, themeId)
    {
      this.FocusEffectEnabled = false;
      this._imgTheme.Position = new Vector2(500f, 800f);
      this._imgSmallLocker.Position = new Vector2(4250f, 950f);
      this._lblStagesUnlocked.Position = this._imgSmallLocker.Position + new Vector2(950f, 400f);
      this._imgMedal.Position = new Vector2(4250f, 2550f);
      this._lblGoldMedalsEarned.Position = this._imgMedal.Position + new Vector2(900f, 500f);
      this._lockerImage.Position = new Vector2(3050f, 1400f);
      this._lblToUnlock.Position = new Vector2(0.0f, 2430f);
      this._lblGardenNeeded.Position = this._lblToUnlock.Position + new Vector2(0.0f, 620f);
      this._lblEgyptNeeded.Position = this._lblGardenNeeded.Position + new Vector2(0.0f, 550f);
      this._lblFactoryNeeded.Position = this._lblEgyptNeeded.Position + new Vector2(0.0f, 550f);
    }

    public override void Update(BrainGameTime gameTime) => base.Update(gameTime);
  }
}
