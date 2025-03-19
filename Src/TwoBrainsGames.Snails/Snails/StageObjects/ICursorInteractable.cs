
// Type: TwoBrainsGames.Snails.StageObjects.ICursorInteractable
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  public interface ICursorInteractable
  {
    StageCursor.CursorType QueryCursor();

    bool QueryInterating();

    void CursorActionPressed(Vector2 cursorPos);

    void CursorActionReleased();

    void CursorActionSelected();

    bool CanAcceptCursorInteraction { get; }
  }
}
