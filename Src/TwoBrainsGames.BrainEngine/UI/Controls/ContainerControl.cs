
// Type: TwoBrainsGames.BrainEngine.UI.Controls.ContainerControl
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using System;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.BrainEngine.UI.Controls
{
  internal class ContainerControl : IControlContainer
  {
    private ControlCollection _controls;

    public ContainerControl(ControlCollection controls) => this._controls = controls;

    public void DrawControls()
    {
      foreach (UIControl control in this._controls)
      {
        if (control is IControlContainer)
          ((IControlContainer) control).DrawControls();
        control.Draw();
      }
    }

    public Screen ParentScreen => throw new NotImplementedException();
  }
}
