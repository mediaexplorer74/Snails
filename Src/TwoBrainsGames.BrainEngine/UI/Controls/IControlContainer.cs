
// Type: TwoBrainsGames.BrainEngine.UI.Controls.IControlContainer
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.BrainEngine.UI.Controls
{
  internal interface IControlContainer
  {
    Screen ParentScreen { get; }

    void DrawControls();
  }
}
