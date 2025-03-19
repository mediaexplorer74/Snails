
// Type: TwoBrainsGames.Snails.Screens.Transitions.LeafTransition
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;
using TwoBrainsGames.Snails.Configuration;


namespace TwoBrainsGames.Snails.Screens.Transitions
{
  internal class LeafTransition : Transition
  {
    private const float LEAF1_OPENED_ANGLE = -45f;
    private const float LEAF2_OPENED_ANGLE = 55f;
    private const float LEAF3_OPENED_ANGLE = -80f;
    private const float LEAF1_CLOSED_ANGLE = 55f;
    private const float LEAF2_CLOSED_ANGLE = -45f;
    private const float LEAF3_CLOSED_ANGLE = 20f;
    private const float MIN_SPEED = 0.1f;
    private const float MAX_SPEED = 20f;
    private Sprite _leafSprite;
    private Vector2 _leaf1Position;
    private Vector2 _leaf2Position;
    private Vector2 _leaf3Position;
    private float _rotation1;
    private float _rotation2;
    private float _rotation3;
    private float _speed;
    private Color _leaf1Color;
    private Color _leaf2Color;
    private Color _leaf3Color;
    private LeafTransition.State _state;
    private bool _leaf1Ended;
    private bool _leaf2Ended;
    private bool _leaf3Ended;
    private bool _endOnNextLoop;
    private Sample _leafsOpenSample;
    private Sample _leafsCloseSample;

    public bool StopSounds { get; set; }

    public SpriteBatch SpriteBatch => BrainGame.SpriteBatch;

    public LeafTransition(LeafTransition.State state)
    {
      this._state = state;
      this.StopSounds = true;
    }

    public override void LoadContent()
    {
      this._leafSprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/leaf", "Leaf");
      this._leafsOpenSample = BrainGame.ResourceManager.GetSampleStatic("sfx/leafs-open");
      this._leafsCloseSample = BrainGame.ResourceManager.GetSampleStatic("sfx/leafs-close");
    }

    public override void Initialize()
    {
      base.Initialize();
      this._leaf1Position = new Vector2(-220f, 0.0f);
      this._leaf2Position = new Vector2((float) BrainGame.ScreenWidth, -200f);
      this._leaf3Position = new Vector2((float) BrainGame.ScreenWidth + 230f, (float) BrainGame.ScreenHeight);
      switch (this._state)
      {
        case LeafTransition.State.Closing:
          this._rotation1 = -45f;
          this._rotation2 = 55f;
          this._rotation3 = -80f;
          this._speed = 20f;
          break;
        case LeafTransition.State.Opening:
          this._rotation1 = 55f;
          this._rotation2 = -45f;
          this._rotation3 = 20f;
          this._speed = 0.01f;
          break;
        case LeafTransition.State.ClosedStopped:
          this._rotation1 = 55f;
          this._rotation2 = -45f;
          this._rotation3 = 20f;
          this._speed = 0.0f;
          break;
      }
      this._leaf1Color = new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
      this._leaf2Color = new Color((int) byte.MaxValue, (int) byte.MaxValue, 130, (int) byte.MaxValue);
      this._leaf3Color = new Color((int) byte.MaxValue, (int) byte.MaxValue, 200, (int) byte.MaxValue);
      this._leaf1Ended = false;
      this._leaf2Ended = false;
      this._leaf3Ended = false;
      this._endOnNextLoop = false;
    }

    public override void Update(BrainGameTime gameTime)
    {
      if (this._endOnNextLoop)
      {
        this._ended = true;
      }
      else
      {
        switch (this._state)
        {
          case LeafTransition.State.Closing:
            this.UpdateClosing(gameTime);
            break;
          case LeafTransition.State.Opening:
            this.UpdateOpening(gameTime);
            break;
          case LeafTransition.State.ClosedStopped:
            this._endOnNextLoop = true;
            break;
        }
      }
    }

    public override void OnStart()
    {
      if (this._state == LeafTransition.State.Opening)
        this._leafsOpenSample.Play();
      else
        this._leafsCloseSample.Play();
    }

    private void UpdateClosing(BrainGameTime gameTime)
    {
      float num = (float) ((double) this._speed * gameTime.ElapsedRealTime.TotalMilliseconds / 100.0);
      if ((double) this._rotation1 > 35.0)
      {
        this._speed = (float) (22.0 - (double) this._rotation1 * 20.0 / 55.0);
        if ((double) this._speed < 0.10000000149011612)
          this._speed = 0.1f;
      }
      this._rotation1 += num;
      this._rotation2 -= num;
      this._rotation3 += num;
      if ((double) this._rotation1 > 55.0)
      {
        this._rotation1 = 55f;
        this._leaf1Ended = true;
      }
      if ((double) this._rotation2 < -45.0)
      {
        this._rotation2 = -45f;
        this._leaf2Ended = true;
      }
      if ((double) this._rotation3 > 20.0)
      {
        this._rotation3 = 20f;
        this._leaf3Ended = true;
      }
      if (!this._leaf1Ended || !this._leaf2Ended || !this._leaf3Ended)
        return;
      this._endOnNextLoop = true;
      if (!this.StopSounds)
        return;
      BrainGame.SampleManager.StopAll();
    }

    private void UpdateOpening(BrainGameTime gameTime)
    {
      float num = (float) ((double) this._speed * gameTime.ElapsedRealTime.TotalMilliseconds / 100.0);
      if ((double) this._rotation1 > 35.0)
      {
        this._speed = (float) (22.0 - (double) this._rotation1 * 20.0 / 55.0);
        if ((double) this._speed > 20.0)
          this._speed = 20f;
      }
      else
        this._speed = 20f;
      this._rotation1 -= num;
      this._rotation2 += num;
      this._rotation3 -= num;
      if ((double) this._rotation1 < -45.0)
      {
        this._rotation1 = -45f;
        this._leaf1Ended = true;
      }
      if ((double) this._rotation2 > 55.0)
      {
        this._rotation2 = 55f;
        this._leaf2Ended = true;
      }
      if ((double) this._rotation3 < -80.0)
      {
        this._rotation3 = -80f;
        this._leaf3Ended = true;
      }
      if (!this._leaf1Ended || !this._leaf2Ended || !this._leaf3Ended)
        return;
      this._endOnNextLoop = true;
    }

    public override void Draw()
    {
      this.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, (DepthStencilState) null, (RasterizerState) null, (Effect) BrainGame.RenderEffect);
      this.Draw(this.SpriteBatch);
      this.SpriteBatch.End();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      float scale = 2.5f;
      if (Game1.GameSettings.PresentationMode == GameSettings.PresentationType.LD)
        scale = 2f;
      this._leafSprite.Draw(this._leaf3Position, 0, this._rotation3, SpriteEffects.FlipHorizontally, this._leaf3Color, scale, spriteBatch);
      this._leafSprite.Draw(this._leaf2Position, 0, this._rotation2, SpriteEffects.FlipHorizontally, this._leaf2Color, scale, spriteBatch);
      this._leafSprite.Draw(this._leaf1Position, 0, this._rotation1, SpriteEffects.None, this._leaf1Color, scale, spriteBatch);
    }

    public enum State
    {
      Closing,
      Opening,
      ClosedStopped,
    }
  }
}
