
// Type: TwoBrainsGames.BrainEngine.UI.Margin
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine.UI.Controls;


namespace TwoBrainsGames.BrainEngine.UI
{
  public class Margin
  {
    private float _left;
    private float _top;
    private float _right;
    private float _bottom;
    private UIControl _owner;

    public float Left
    {
      get => this._left;
      set
      {
        if ((double) this._left == (double) value)
          return;
        this._left = value;
        this._owner.MarginChanged();
      }
    }

    public float Top
    {
      get => this._top;
      set
      {
        if ((double) this._top == (double) value)
          return;
        this._top = value;
        this._owner.MarginChanged();
      }
    }

    public float Right
    {
      get => this._right;
      set
      {
        if ((double) this._right == (double) value)
          return;
        this._right = value;
        this._owner.MarginChanged();
      }
    }

    public float Bottom
    {
      get => this._bottom;
      set
      {
        if ((double) this._bottom == (double) value)
          return;
        this._bottom = value;
        this._owner.MarginChanged();
      }
    }

    public Margin(UIControl owner) => this._owner = owner;

    public Margin(float left, float top, float right, float bottom, UIControl owner)
    {
      this._left = left;
      this._top = top;
      this._right = right;
      this._bottom = bottom;
      this._owner = owner;
    }

    public void Clear() => this.Left = this.Top = this.Bottom = this.Right = 0.0f;

    public override string ToString()
    {
      return string.Format("{0},{1},{2},{3}", (object) this.Left, (object) this.Top, (object) this.Right, (object) this.Bottom);
    }
  }
}
