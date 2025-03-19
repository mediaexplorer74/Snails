
// Type: TwoBrainsGames.Snails.StageObjects.DynamiteBoxTriggered
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class DynamiteBoxTriggered : DynamiteBox
  {
    public new const string ID = "DYNAMITE_BOX_TRIGGERED";
    public new const int EXPLOSION_TIME = 4000;

    public DynamiteBoxTriggered()
      : base(StageObjectType.DynamiteBoxTriggered)
    {
      this._counterVisible = false;
      this._isActive = false;
    }

    protected override void SnailCollided(Snail snail)
    {
      this._status = DynamiteBox.DynamiteBoxStatus.Counting;
      this._counterVisible = true;
    }

    protected override void BoxDeployed(bool addTile, bool addPaths, bool checkCollisions)
    {
      base.BoxDeployed(addTile, addPaths, checkCollisions);
      this._status = DynamiteBox.DynamiteBoxStatus.Deployed;
    }

    public override void Update(BrainGameTime gameTime)
    {
      if (this._status == DynamiteBox.DynamiteBoxStatus.Deployed)
        this.DoQuadtreeCollisions(0);
      base.Update(gameTime);
    }
  }
}
