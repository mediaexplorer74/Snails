
// Type: TwoBrainsGames.Snails.Screens.Transitions.SnailsBlurTransition
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
   internal partial class SnailsBlurTransition(UIScreen screen, bool isGray) :
        BlurTransition(screen, isGray, 6, 10.0),
        ISnailsPauseTransition
    {
        private const int BLUR_ITERATIONS = 6;
        private const double BLUR_ITERATION_TIME = 10.0;
        bool ISnailsPauseTransition.IsTransitionOut => this.IsTransitionOut;

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
            this.TextureToBlur = (Texture2D)null;
            Stage currentStage = Stage.CurrentStage;
        }

        private void RestoreBlur()
        {
            this.TextureToBlur = (Texture2D)null;
            if (Stage.CurrentStage != null)
                Stage.CurrentStage.DrawToRenderTarget();
            this.DrawStageToTexture();
            this.PerformBlur();
        }

        public override void Update(BrainGameTime gameTime) => base.Update(gameTime);

        private void DrawStageToTexture()
        {
            if (Stage.CurrentStage == null)
                return;
            this.TextureToBlur = (Texture2D)Stage.CurrentStage.RenderTarget;
        }

        public override void Draw()
        {
            this.ScreenOwner.EndDraw();
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
            this.ScreenOwner.BeginDraw(BlendState.AlphaBlend);
        }

        //bool ISnailsPauseTransition.IsTransitionOut => this.IsTransitionOut;
    }
}
