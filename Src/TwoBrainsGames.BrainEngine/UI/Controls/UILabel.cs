
// Type: TwoBrainsGames.BrainEngine.UI.Controls.UILabel
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.BrainEngine.UI.Controls
{
  public class UILabel : UIControl
  {
    public const char LINE_SEPARATOR = '|';
    protected bool _autosize;
    private HorizontalTextAligment _horizontalAligment;
    private VerticalTextAligment _verticalAligment;

    public override string Text
    {
      get => base.Text;
      set
      {
        base.Text = value;
        this.TextLines = new string[0];
        if (value != null)
          this.TextLines = value.Split('|');
        if (base.Text == null)
          base.Text = "";
        this.CalculateSize();
      }
    }

    public string[] TextLines { get; private set; }

    public bool Autosize
    {
      get => this._autosize;
      set
      {
        if (this._autosize == value)
          return;
        this._autosize = value;
        this.CalculateSize();
      }
    }

    public HorizontalTextAligment HorizontalAligment
    {
      get => this._horizontalAligment;
      set
      {
        if (this._horizontalAligment == value)
          return;
        this._horizontalAligment = value;
        this.CalculateSize();
      }
    }

    public VerticalTextAligment VerticalAligment
    {
      get => this._verticalAligment;
      set
      {
        if (this._verticalAligment == value)
          return;
        this._verticalAligment = value;
        this.CalculateSize();
      }
    }

    public int LineCount => this.TextLines != null ? this.TextLines.Length : 0;

    public UILabel(UIScreen screenOwner)
      : base(screenOwner)
    {
      this.AcceptControllerInput = false;
      this.Autosize = true;
      this.HorizontalAligment = HorizontalTextAligment.Left;
      this.VerticalAligment = VerticalTextAligment.Top;
      this.Text = "";
    }

    protected virtual void CalculateSize()
    {
    }
  }
}
