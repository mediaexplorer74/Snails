
// Type: TwoBrainsGames.Snails.StageObjects.Apple
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.SpacePartitioning;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class Apple : MovingObject
  {
    public const string ID = "APPLE";
    private string RES_APPLE = "spriteset/stage-objects";
    private string SPRITE_APPLE_OXIDIZED_ANIM = "AppleOxidizedAnim";
    private int _bites;
    private bool _singleSnailEat = true;
    private Sprite _SpriteOxidizedAnim;
    private Snail _snail;
    private Apple.AppleState _state;
    private Sample _biteSample;

    public bool CanBeEaten => this._bites > 0 && this.IsAttachedToPath;

    public Apple()
      : base(StageObjectType.Apple)
    {
      this.SpriteAnimationActive = false;
    }

    public Apple(Apple other)
      : base((StageObject) other)
    {
      this.Copy((StageObject) other);
    }

    public override void Copy(StageObject other)
    {
      base.Copy(other);
      this._bites = (other as Apple)._bites;
      this._singleSnailEat = (other as Apple)._singleSnailEat;
      this._SpriteOxidizedAnim = (other as Apple)._SpriteOxidizedAnim;
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this._SpriteOxidizedAnim = BrainGame.ResourceManager.GetSpriteTemporary(this.RES_APPLE, this.SPRITE_APPLE_OXIDIZED_ANIM);
      this._biteSample = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/apple_bite", (Object2D) this);
    }

    public override void Initialize()
    {
      base.Initialize();
      this._bites = this.Sprite.FrameCount - 1;
      this._state = Apple.AppleState.Good;
    }

    public override void OnLastFrame()
    {
      base.OnLastFrame();
      if (this._state != Apple.AppleState.Rotting)
        return;
      this.SpriteAnimationActive = false;
      this.CurrentFrame = this.Sprite.FrameCount - 1;
      this.FadeOut(1f);
    }

    public override void OnCollide(IQuadtreeContainable obj, int listIdx)
    {
      if (!(obj is Snail snail) || !snail.CanEatApple || this._singleSnailEat && this._snail != null && this._snail != snail || !snail.CheckCollisionWithHead(this.AABoundingBox))
        return;
      if (this._singleSnailEat && this._snail == null)
        this._snail = snail;
      this.RemoveHooverEffect();
      snail.SetEatingApple(this);
    }

    public void SnailBite()
    {
      if (this._biteSample != null && !this._biteSample.IsPlaying)
        this._biteSample.Play();
      --this._bites;
      ++this.CurrentFrame;
      if (this._bites != 0)
        return;
      this.CurrentFrame = 0;
      this.Sprite = this._SpriteOxidizedAnim;
      this.SpriteAnimationActive = true;
      this.StaticFlags &= ~StageObjectStaticFlags.CanHoover;
      this._state = Apple.AppleState.Rotting;
    }

    public void SnailStoppedEating() => this._snail = (Snail) null;

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      if (this._snail != null && this._snail.IsDead)
        this._snail = (Snail) null;
      if (this.IsDead || this.IsDisposed || this._bites <= 0 || !this.IsAttachedToPath)
        return;
      this.DoQuadtreeCollisions(0);
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
    }

    public override DataFileRecord ToDataFileRecord() => base.ToDataFileRecord();

    private enum AppleState
    {
      Good,
      Rotting,
    }
  }
}
