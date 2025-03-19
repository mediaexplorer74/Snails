
// Type: TwoBrainsGames.Snails.StageObjects.ISwitchable
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer


namespace TwoBrainsGames.Snails.StageObjects
{
  public interface ISwitchable
  {
    void SwitchOn();

    void SwitchOff();

    bool IsOn { get; }
  }
}
