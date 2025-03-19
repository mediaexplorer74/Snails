
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UIMainMenuBodyPanel
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UIMainMenuBodyPanel : UIPanel
  {
    public UIMainMenuBodyPanel(UIScreen screenOwner)
      : base(screenOwner)
    {
      this.ParentAlignment = AlignModes.Horizontaly;
      this.Size = new Size(10000f, new Vector2(0.0f, 7000f).Y);
    }

    public override void InitializeFromContent()
    {
      base.InitializeFromContent();
      this.InitializeFromContent(nameof (UIMainMenuBodyPanel));
    }
  }
}
