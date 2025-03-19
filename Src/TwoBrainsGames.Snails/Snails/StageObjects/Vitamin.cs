
// Type: TwoBrainsGames.Snails.StageObjects.Vitamin
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class Vitamin : MovingObject
  {
    public const string ID = "VITAMIN";
    private const int DEFAULT_VITAMINIZED_MILIS = 10000;
    private int _elapsedGameTime;
    private Snail _snail;
    private Sample _rocketSound;
    private Sample _underWaterRocketSound;

    public Vitamin()
      : base(StageObjectType.Vitamin)
    {
    }

    public Vitamin(Vitamin other)
      : base((StageObject) other)
    {
      this.Copy((StageObject) other);
    }

    public override void Copy(StageObject other) => base.Copy(other);

    public override void LoadContent()
    {
      base.LoadContent();
      this._rocketSound = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/rocket", (Object2D) this);
      this._underWaterRocketSound = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/rocket-underwater", (Object2D) this);
    }

    public override void OnCollide(IQuadtreeContainable obj, int listIdx)
    {
      Snail snail = obj as Snail;
      if (!snail.CanEatVitamins || !snail.CheckCollisionWithVitamin(this.AABoundingBox))
        return;
      this._snail = snail;
      this._snail.SetVitaminized(this);
      this.PlayRocketSound();
      this.Hide();
    }

    public override void Update(BrainGameTime gameTime)
    {
      if (this.IsDead || this.IsDisposed)
      {
        this.StopRocketSound();
        base.Update(gameTime);
      }
      else if (this._snail != null)
      {
        if (!this._snail.IsVitaminized)
          return;
        this._elapsedGameTime += gameTime.ElapsedGameTime.Milliseconds;
        if (this._elapsedGameTime <= 10000)
          return;
        this._snail.SetUnvitaminized();
        Stage.CurrentStage.RemoveObject((StageObject) this);
        this.StopRocketSound();
      }
      else
      {
        base.Update(gameTime);
        this.DoQuadtreeCollisions(0);
      }
    }

    public void SnailRemoved()
    {
      this.StopRocketSound();
      Stage.CurrentStage.RemoveObject((StageObject) this);
    }

    private void PlayRocketSound()
    {
      this.StopRocketSound();
      if (this._snail.IsUnderLiquid)
        this._underWaterRocketSound.Play(true);
      else
        this._rocketSound.Play(true);
    }

    private void StopRocketSound()
    {
      this._underWaterRocketSound.Stop();
      this._rocketSound.Stop();
    }

    public void OnSnailEnteredLiquid() => this.PlayRocketSound();

    public void OnSnailExitedLiquid() => this.PlayRocketSound();
  }
}
