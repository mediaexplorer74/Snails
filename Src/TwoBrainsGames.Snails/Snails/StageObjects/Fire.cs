
// Type: TwoBrainsGames.Snails.StageObjects.Fire
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.SpacePartitioning;


namespace TwoBrainsGames.Snails.StageObjects
{
  internal class Fire : StageObject
  {
    private static Sample _fire;

    private Sample FireSample
    {
      get => Fire._fire;
      set => Fire._fire = value;
    }

    public Fire()
      : base(StageObjectType.Fire)
    {
    }

    public override void LoadContent()
    {
      base.LoadContent();
      Sample fireSample = this.FireSample;
    }

    public override void Initialize()
    {
      base.Initialize();
      this._crateCollisionBB = this.GetCurrentFrameRectTransformed();
    }

    public override void StageStartupPhaseEnded() => base.StageStartupPhaseEnded();

    public override void OnCollide(IQuadtreeContainable obj, int listIdx)
    {
      StageObject stageObject = obj as StageObject;
      if (!stageObject.CanDieWithFire)
        return;
      stageObject.KillByFire();
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      this.DoQuadtreeCollisions(0);
      this.DoQuadtreeCollisions(1);
    }

    public override bool CrateToolIsValid(BoundingSquare crateBs)
    {
      return !crateBs.Collides(this._crateCollisionBB);
    }
  }
}
