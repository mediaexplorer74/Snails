
// Type: TwoBrainsGames.BrainEngine.UI.Controls.ControlCollectionEnumerator
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using System;
using System.Collections;
using System.Collections.Generic;


namespace TwoBrainsGames.BrainEngine.UI.Controls
{
  public class ControlCollectionEnumerator : IEnumerator
  {
    public List<UIControl> _controls;
    private int _position = -1;

    public ControlCollectionEnumerator(List<UIControl> controls) => this._controls = controls;

    public bool MoveNext()
    {
      ++this._position;
      return this._position < this._controls.Count;
    }

    public void Reset() => this._position = -1;

    object IEnumerator.Current => (object) this.Current;

    public UIControl Current
    {
      get
      {
        try
        {
          return this._controls[this._position];
        }
        catch (IndexOutOfRangeException ex)
        {
          throw new InvalidOperationException();
        }
      }
    }
  }
}
