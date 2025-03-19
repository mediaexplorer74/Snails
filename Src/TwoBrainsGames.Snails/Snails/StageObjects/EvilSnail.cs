
// Type: TwoBrainsGames.Snails.StageObjects.EvilSnail
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.SpacePartitioning;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class EvilSnail : Snail
  {
    public new const string ID = "EVIL_SNAIL";

    public bool CanKillSnail => !this.IsEating && !this.IsHidding;

    public EvilSnail()
      : this(StageObjectType.EvilSnail)
    {
    }

    protected EvilSnail(StageObjectType type)
      : base(type)
    {
    }

    public override void Initialize() => base.Initialize();

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      this.DoQuadtreeCollisions(0);
    }

    public override void Draw(bool shadow) => base.Draw(shadow);

    public override void OnCollide(IQuadtreeContainable obj, int listIdx)
    {
      Snail snail = (Snail) obj;
      if (!snail.CanBeKilledByEvilSnail || !this.CanKillSnail)
        return;
      snail.KillByEvilSnail();
    }
  }
}
