
// Type: TwoBrainsGames.Snails.Stages.HUD.FastForwardStrips
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Graphics;


namespace TwoBrainsGames.Snails.Stages.HUD
{
  internal class FastForwardStrips : IBrainComponent
  {
    private const int VERTICAL_STRIP_COUNT = 5;
    private const float STRIPS_SPEED = 0.05f;
    private Sprite _ffSprite;
    private Rectangle[] VerticalStripRects;
    private float[] VerticalStripX;
    private ColorEffect _blink;

    public bool Visible { get; set; }

    public SpriteBatch SpriteBatch => throw new NotImplementedException();

    public void Initialize()
    {
    }

    public void LoadContent()
    {
      this._ffSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD/FastForward");
      this.VerticalStripRects = new Rectangle[5];
      this.VerticalStripX = new float[5];
      float num = 0.0f;
      for (int index = 0; index < this.VerticalStripRects.Length; ++index)
      {
        this.VerticalStripX[index] = num;
        this.VerticalStripRects[index] = new Rectangle((int) this.VerticalStripX[index], 0, this._ffSprite.Frames[0].Width, (int) Stage.CurrentStage.StageHUD._stageArea.Height);
        num += Stage.CurrentStage.StageHUD._stageArea.Width / 5f;
      }
      this._blink = new ColorEffect(new Color(1f, 1f, 1f, 0.4f), new Color(0.7f, 0.7f, 0.7f, 0.2f), 0.1f, true);
    }

    public void Update(BrainGameTime gameTime)
    {
      if (!this.Visible)
        return;
      this._blink.Update(gameTime);
      for (int index = 0; index < this.VerticalStripRects.Length; ++index)
      {
        this.VerticalStripX[index] += 0.05f * (float) gameTime.ElapsedRealTime.Milliseconds;
        this.VerticalStripRects[index].X = (int) this.VerticalStripX[index];
        if ((double) this.VerticalStripX[index] > (double) Stage.CurrentStage.StageHUD._stageArea.Right)
          this.VerticalStripX[index] = -10f;
      }
    }

    public void Draw()
    {
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      if (!this.Visible)
        return;
      for (int index = 0; index < this.VerticalStripRects.Length; ++index)
        this._ffSprite.Draw(this._ffSprite.Frames[0].Rect, this.VerticalStripRects[index], this._blink.Color, spriteBatch);
    }

    public void UnloadContent()
    {
    }
  }
}
