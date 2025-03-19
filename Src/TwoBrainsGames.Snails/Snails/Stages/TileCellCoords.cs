
// Type: TwoBrainsGames.Snails.Stages.TileCellCoords
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer


namespace TwoBrainsGames.Snails.Stages
{
  public struct TileCellCoords(int ColIndex, int rowIndex)
  {
    private int _RowIndex = rowIndex;
    private int _ColIndex = ColIndex;

    public int RowIndex
    {
      get => this._RowIndex;
      set => this._RowIndex = value;
    }

    public int ColIndex
    {
      get => this._ColIndex;
      set => this._ColIndex = value;
    }

    public override string ToString()
    {
      return string.Format("{0},{1}", (object) this.RowIndex, (object) this.ColIndex);
    }
  }
}
