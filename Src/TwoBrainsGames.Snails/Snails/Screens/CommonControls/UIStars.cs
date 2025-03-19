
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UIStars
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UIStars : UIControl
  {
    private Sprite _starsSprite;

    private UIStars.Star[] Stars { get; set; }

    private int StarCount { get; set; }

    private int Duration { get; set; }

    private int EllapsedDuration { get; set; }

    private bool DurationExpired { get; set; }

    private int StarsDurationMaximum { get; set; }

    private int StarsDurationMinimum { get; set; }

    public UIStars(
      UIScreen screenOwner,
      int numStars,
      int duration,
      int starsDurationMinimum,
      int starsDurationMaximum)
      : base(screenOwner)
    {
      this._starsSprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/common-elements-1", "Star");
      this.StarCount = numStars;
      this.Duration = duration;
      this.StarsDurationMaximum = starsDurationMaximum;
      this.StarsDurationMinimum = starsDurationMinimum;
      this.AcceptControllerInput = false;
    }

    public void Initialize()
    {
      this.Stars = new UIStars.Star[this.StarCount];
      for (int index = 0; index < this.Stars.Length; ++index)
      {
        this.Stars[index] = new UIStars.Star(this);
        this.Stars[index].Randomize();
        this.Stars[index]._releaseTime = (double) BrainGame.Rand.Next(2000);
        this.Stars[index]._visible = false;
      }
      this.EllapsedDuration = 0;
      this.DurationExpired = false;
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      if (this.EllapsedDuration < this.Duration)
        this.EllapsedDuration += gameTime.ElapsedGameTime.Milliseconds;
      else
        this.DurationExpired = true;
      for (int index = 0; index < this.Stars.Length; ++index)
      {
        UIStars.Star star = this.Stars[index];
        if (star._visible)
        {
          star._position -= new Vector2(0.0f, star._speed * (float) gameTime.ElapsedGameTime.Milliseconds);
          star._curDecayTime += (double) gameTime.ElapsedGameTime.Milliseconds;
          if (star._curDecayTime > star._decayTime)
          {
            star._color = new Color((int) ((double) star._color.R * (double) star._colorAlpha), (int) ((double) star._color.G * (double) star._colorAlpha), (int) ((double) star._color.B * (double) star._colorAlpha), (int) ((double) star._color.A * (double) star._colorAlpha));
            star._colorAlpha -= (float) ((double) this.Stars[index]._fadeSpeed * (double) gameTime.ElapsedGameTime.Milliseconds / 1000.0);
            if (star._color.A == (byte) 0)
            {
              star.Randomize();
              star._visible = false;
            }
          }
        }
        else if (!this.DurationExpired)
        {
          star._curReleaseTime += (double) gameTime.ElapsedGameTime.Milliseconds;
          if (star._curReleaseTime >= star._releaseTime)
          {
            star._curReleaseTime = star._releaseTime;
            star._visible = true;
          }
        }
      }
    }

    public override void Draw()
    {
      base.Draw();
      for (int index = 0; index < this.Stars.Length; ++index)
      {
        if (this.Stars[index]._visible)
          this._starsSprite.Draw(this.ScreenUnitToPixels(this.Stars[index]._position) + this.AbsolutePositionInPixels, 0, 0.0f, Vector2.Zero, this.Stars[index]._scale, this.Stars[index]._scale, this.Stars[index]._color, this.ScreenOwner.SpriteBatch);
      }
    }

    private class Star
    {
      private UIStars _owner;
      public Vector2 _position;
      public float _speed;
      public float _scale;
      public Color _color;
      public float _fadeSpeed;
      public float _colorAlpha;
      public double _releaseTime;
      public double _curReleaseTime;
      public double _decayTime;
      public double _curDecayTime;
      public bool _visible;

      public Star(UIStars owner) => this._owner = owner;

      public void Randomize()
      {
        this._position = new Vector2((float) BrainGame.Rand.Next((int) this._owner.Size.Width), (float) BrainGame.Rand.Next((int) this._owner.Size.Height));
        this._scale = (float) (0.40000000596046448 + (double) BrainGame.Rand.Next(60) / 100.0);
        this._speed = (float) (0.800000011920929 + (double) BrainGame.Rand.Next(100) / 100.0);
        this._fadeSpeed = 0.04f;
        this._color = this.RandomizeColor();
        this._colorAlpha = 1f;
        this._decayTime = (double) (this._owner.StarsDurationMinimum + BrainGame.Rand.Next(this._owner.StarsDurationMaximum - this._owner.StarsDurationMinimum));
        this._curDecayTime = 0.0;
      }

      private Color RandomizeColor()
      {
        return new Color(128 + BrainGame.Rand.Next(128), 128 + BrainGame.Rand.Next(128), 128 + BrainGame.Rand.Next(128), (int) byte.MaxValue);
      }
    }
  }
}
