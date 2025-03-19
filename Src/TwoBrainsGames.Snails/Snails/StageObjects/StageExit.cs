
// Type: TwoBrainsGames.Snails.StageObjects.StageExit
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class StageExit : StageObject
  {
    public const string SPRITE_PUB_NAME = "ExitAnimation";
    private const int CRATE_TOOL_VALID_BB_IDX = 1;
    private int _snailsArrived;
    private SpriteAnimation _exitAnimation;

    public SnailCounter SnailCounter { get; set; }

    public Vector2 DoorPosition
    {
      get
      {
        return new Vector2(this.AABoundingBox.LowerLeft.X + this.AABoundingBox.Width / 2f, this.AABoundingBox.Bottom);
      }
    }

    public StageExit()
      : base(StageObjectType.StageExit)
    {
      this._snailsArrived = 0;
    }

    public StageExit(StageEntrance other)
      : base((StageObject) other)
    {
      this.Copy((StageObject) other);
    }

    public override void Copy(StageObject other)
    {
      base.Copy(other);
      this._snailsArrived = ((StageExit) other)._snailsArrived;
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this._exitAnimation = new SpriteAnimation(BrainGame.ResourceManager.GetSprite(this.ResourceId + "/ExitAnimation", this._contentManagerId));
      if (this.SnailCounter == null)
        return;
      this.SnailCounter.LoadContent();
    }

    public override void OnCollide(IQuadtreeContainable obj, int listIdx)
    {
      Snail snail = obj as Snail;
      if (!snail.CanEnterStage || !snail.CheckCollisionWithHead(this.AABoundingBox))
        return;
      ++Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsSafe;
      if (snail is SnailKing)
        ++Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsKingSafe;
      ++this._snailsArrived;
      if (this.SnailCounter != null)
        this.SnailCounter.SetCounter(this._snailsArrived);
      snail.OnEnteringStageExit(this);
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      this.DoQuadtreeCollisions(0);
      this._exitAnimation.Update(gameTime);
    }

    public override void Draw(bool shadow)
    {
      base.Draw(shadow);
      this._exitAnimation.Draw(this.Position, Stage.CurrentStage.SpriteBatch);
    }

    public override void AddLinkedObject(StageObject obj)
    {
      this._linkedObjects.Add(obj);
      if (!(obj is SnailCounter))
        return;
      this.SnailCounter = (SnailCounter) obj;
      this.SnailCounter.SetCounter(this._snailsArrived);
    }

    public void SetSnailCounter(SnailCounter counter)
    {
      this.SnailCounter = counter;
      this.LinkTo((StageObject) counter);
    }
  }
}
