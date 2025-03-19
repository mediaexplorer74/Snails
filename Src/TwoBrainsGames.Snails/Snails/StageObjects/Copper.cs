
// Type: TwoBrainsGames.Snails.StageObjects.Copper
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class Copper : Box
  {
    public new const string ID = "COPPER";

    public Copper()
      : base(StageObjectType.Copper)
    {
    }

    public Copper(Copper other)
      : base((Box) other)
    {
      this.Copy((StageObject) other);
    }

    public override void Copy(StageObject other) => base.Copy(other);

    public override void OnLastFrame()
    {
      Stage.CurrentStage.Board.SetTileAt(this._tile, this.BoardX, this.BoardY);
      this.Quadtree.DoCollisions((IQuadtreeContainable) this, 0);
      this.Quadtree.DoCollisions((IQuadtreeContainable) this, 1);
      Stage.CurrentStage.RemoveObject((StageObject) this);
      this.SwitchObjects();
    }
  }
}
