
// Type: TwoBrainsGames.Snails.StageObjects.Slime
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class Slime : StageObject
  {
    public const string ID = "SLIME";
    private Sample _slimeSound;

    public Slime()
      : base(StageObjectType.Slime)
    {
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this._slimeSound = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/snail-entering-stage", (Object2D) this);
    }

    public override void OnLastFrame()
    {
      this.SpriteAnimationActive = false;
      this.CurrentFrame = this.Sprite.FrameCount - 1;
      this.FadeOut(0.5f);
    }

    public override void OnAddedToStage()
    {
      base.OnAddedToStage();
      if (BrainGame.Rand.Next(5) != 0)
        return;
      this._slimeSound.Play();
    }

    public override void Update(BrainGameTime gameTime) => base.Update(gameTime);
  }
}
