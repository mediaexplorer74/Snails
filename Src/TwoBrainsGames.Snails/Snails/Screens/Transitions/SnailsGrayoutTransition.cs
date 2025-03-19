
// Type: TwoBrainsGames.Snails.Screens.Transitions.SnailsGrayoutTransition
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Graphics;
using System.Runtime.CompilerServices;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.Screens.Transitions
{
  internal class SnailsGrayoutTransition(UIScreen screen, bool isGray) : 
    GrayoutTransition(screen, isGray, 6, 10.0),
    ISnailsPauseTransition
  {
    private const int BLUR_ITERATIONS = 6;
    private const double BLUR_ITERATION_TIME = 10.0;

    public override void Initialize()
    {
      base.Initialize();
      if (Stage.CurrentStage == null)
        return;
      Stage.CurrentStage.DrawToRenderTarget();
    }

    public override void Reset()
    {
      base.Reset();
      Stage currentStage = Stage.CurrentStage;
    }

    public void RestoreBlur()
    {
      this.TextureToBlur = (Texture2D) null;
      Stage.CurrentStage.DrawToRenderTarget();
      this.DrawStageToTexture();
      this.PerformBlur();
    }

    public override void Update(BrainGameTime gameTime) => base.Update(gameTime);

    private void DrawStageToTexture()
    {
      this.TextureToBlur = (Texture2D) Stage.CurrentStage.RenderTarget;
    }

    public override void Draw()
    {
      this.ScreenOwner.SuspendDraw();
      if (this.TextureToBlur == null)
      {
        if (!this.Ended)
          this.DrawStageToTexture();
        else
          this.RestoreBlur();
      }
      else if (this.Ended && this.RenderTarget != null && this.RenderTarget.IsContentLost && BrainGame.IsGameActive)
        this.RestoreBlur();
      base.Draw();
      this.ScreenOwner.ResumeDraw();
    }

    //[SpecialName]
    bool ISnailsPauseTransition.IsTransitionOut => this.IsTransitionOut;
  }
}
