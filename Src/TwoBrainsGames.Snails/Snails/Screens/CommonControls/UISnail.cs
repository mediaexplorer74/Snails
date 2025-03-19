
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UISnail
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UISnail : UIControl
  {
    private SpriteAnimation _snailAnim;
    private UISnail.SnailWalkEdge _walkEdge;
    private UISnail.SnailDirection _direction;
    private Sprite _spriteWalk;
    private Sprite _spriteTurn;
    private UISnail.SnailState _state;
    private float _snailWidth;
    private float _snailHeight;

    private UISnail.SnailState State
    {
      get => this._state;
      set
      {
        this._state = value;
        switch (this._state)
        {
          case UISnail.SnailState.Walking:
            this._snailAnim.Sprite = this._spriteWalk;
            break;
          case UISnail.SnailState.Turning:
            this._snailAnim.Sprite = this._spriteTurn;
            break;
          case UISnail.SnailState.Falling:
            this._snailAnim.Sprite = this._spriteWalk;
            this.Effect = (ITransformEffect) new GravityEffect(Game1.GameSettings.Gravity, 0.0f);
            this.Rotation = 0.0f;
            break;
        }
        this._snailAnim.CurrentFrame = 0;
      }
    }

    public float Speed { get; set; }

    private UISnail.SnailDirection Direction
    {
      get => this._direction;
      set
      {
        this._direction = value;
        this._snailAnim._spriteEffect = this._direction == UISnail.SnailDirection.Clowckwise ? SpriteEffects.None : SpriteEffects.FlipHorizontally;
      }
    }

    private UISnail.SnailWalkEdge WalkEdge
    {
      get => this._walkEdge;
      set
      {
        this._walkEdge = value;
        switch (this._walkEdge)
        {
          case UISnail.SnailWalkEdge.Bottom:
            this.Rotation = 0.0f;
            this.Size = new Size(this.PixelsToScreenUnits(new Vector2(this._snailWidth, this._snailHeight)));
            this.Position = new Vector2(this.Position.X, 10000f - this.Size.Height);
            break;
          case UISnail.SnailWalkEdge.Right:
            this.Rotation = 270f;
            this.Size = new Size(this.PixelsToScreenUnits(new Vector2(this._snailHeight, this._snailWidth)));
            this.Position = new Vector2(10000f - this.Size.Width, this.Position.Y);
            break;
          case UISnail.SnailWalkEdge.Top:
            this.Rotation = 180f;
            this.Size = new Size(this.PixelsToScreenUnits(new Vector2(this._snailWidth, this._snailHeight)));
            this.Position = new Vector2(this.Position.X, 0.0f);
            break;
          case UISnail.SnailWalkEdge.Left:
            this.Rotation = 90f;
            this.Size = new Size(this.PixelsToScreenUnits(new Vector2(this._snailHeight, this._snailWidth)));
            this.Position = new Vector2(0.0f, this.Position.Y);
            break;
        }
      }
    }

    public UISnail(UIScreen ownerScreen)
      : base(ownerScreen)
    {
      this._spriteWalk = BrainGame.ResourceManager.GetSpriteStatic("spriteset/anim-snails", "SnailWalk");
      this._spriteTurn = BrainGame.ResourceManager.GetSpriteStatic("spriteset/anim-snails", "SnailTurnUp");
      this._snailAnim = new SpriteAnimation(this._spriteWalk);
      this._snailWidth = (float) this._spriteWalk.Frames[0].Width;
      this._snailHeight = (float) this._spriteWalk.Frames[0].Height;
      this.State = UISnail.SnailState.Walking;
      this.Speed = (float) ((BrainGame.Rand.Next(5) + 10) * 10);
      this.Direction = BrainGame.Rand.Next(2) == 1 ? UISnail.SnailDirection.Clowckwise : UISnail.SnailDirection.CounterClockwise;
      this.WalkEdge = (UISnail.SnailWalkEdge) BrainGame.Rand.Next(4);
      switch (this.WalkEdge)
      {
        case UISnail.SnailWalkEdge.Bottom:
        case UISnail.SnailWalkEdge.Top:
          this.Position = new Vector2((float) BrainGame.Rand.Next(10000), this.Position.Y);
          break;
        case UISnail.SnailWalkEdge.Right:
        case UISnail.SnailWalkEdge.Left:
          this.Position = new Vector2(this.Position.X, (float) BrainGame.Rand.Next(10000));
          break;
      }
      this.DropShadow = true;
      this.ShadowDistance = new Vector2(3f, 3f);
      this.AcceptControllerInput = false;
    }

    public override void Update(BrainGameTime gameTime)
    {
      this._snailAnim.Update(gameTime);
      switch (this.State)
      {
        case UISnail.SnailState.Walking:
          this.UpdateWalk(gameTime);
          break;
        case UISnail.SnailState.Turning:
          this.UpdateTurn(gameTime);
          break;
      }
    }

    public void Kill() => this.State = UISnail.SnailState.Falling;

    private void UpdateTurn(BrainGameTime gameTime)
    {
      if (this._snailAnim.CurrentFrame != this._snailAnim.Sprite.FrameCount - 1)
        return;
      if (this._direction == UISnail.SnailDirection.Clowckwise)
      {
        int num = (int) (this.WalkEdge + 1);
        if (num > 3)
          num = 0;
        this.WalkEdge = (UISnail.SnailWalkEdge) num;
        this.State = UISnail.SnailState.Walking;
        switch (this.WalkEdge)
        {
          case UISnail.SnailWalkEdge.Bottom:
            this.Position = new Vector2(0.0f, 10000f - this.Size.Height);
            break;
          case UISnail.SnailWalkEdge.Right:
            this.Position = new Vector2(10000f - this.Size.Width, 10000f - this.Size.Height);
            break;
          case UISnail.SnailWalkEdge.Top:
            this.Position = new Vector2(10000f - this.Size.Width, 0.0f);
            break;
          case UISnail.SnailWalkEdge.Left:
            this.Position = new Vector2(0.0f, 0.0f);
            break;
        }
      }
      else
      {
        int num = (int) (this.WalkEdge - 1);
        if (num < 0)
          num = 3;
        this.WalkEdge = (UISnail.SnailWalkEdge) num;
        this.State = UISnail.SnailState.Walking;
        switch (this.WalkEdge)
        {
          case UISnail.SnailWalkEdge.Bottom:
            this.Position = new Vector2(10000f - this.Size.Width, 10000f - this.Size.Height);
            break;
          case UISnail.SnailWalkEdge.Right:
            this.Position = new Vector2(10000f - this.Size.Width, 0.0f);
            break;
          case UISnail.SnailWalkEdge.Top:
            this.Position = new Vector2(0.0f, 0.0f);
            break;
          case UISnail.SnailWalkEdge.Left:
            this.Position = new Vector2(0.0f, 10000f - this.Size.Height);
            break;
        }
      }
    }

    private void UpdateWalk(BrainGameTime gameTime)
    {
      float num = (float) ((double) this.Speed * gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0) * (float) this.Direction;
      switch (this.WalkEdge)
      {
        case UISnail.SnailWalkEdge.Bottom:
          this.Position = new Vector2(this.Position.X + num, this.Position.Y);
          break;
        case UISnail.SnailWalkEdge.Right:
          this.Position = new Vector2(this.Position.X, this.Position.Y - num);
          break;
        case UISnail.SnailWalkEdge.Top:
          this.Position = new Vector2(this.Position.X - num, this.Position.Y);
          break;
        case UISnail.SnailWalkEdge.Left:
          this.Position = new Vector2(this.Position.X, this.Position.Y + num);
          break;
      }
      if ((double) this.Position.Y > (double) (10000f - this.Size.Height))
      {
        this.Position = new Vector2(this.Position.X, 10000f - this.Size.Height);
        this.State = UISnail.SnailState.Turning;
      }
      if ((double) this.Position.Y < 0.0)
      {
        this.Position = new Vector2(this.Position.X, 0.0f);
        this.State = UISnail.SnailState.Turning;
      }
      if ((double) this.Position.X < 0.0)
      {
        this.Position = new Vector2(0.0f, this.Position.Y);
        this.State = UISnail.SnailState.Turning;
      }
      if ((double) this.Position.X <= (double) (10000f - this.Size.Width))
        return;
      this.Position = new Vector2(10000f - this.Size.Width, this.Position.Y);
      this.State = UISnail.SnailState.Turning;
    }

    public override void Draw()
    {
      if (this.DropShadow)
        this._snailAnim.Draw(this.AbsolutePositionInPixels + this.GetDrawOffset() + this.ShadowDistance, this.Rotation, this.ShadowColor, this.ScreenOwner.SpriteBatch);
      this._snailAnim.Draw(this.AbsolutePositionInPixels + this.GetDrawOffset(), this.Rotation, this.ScreenOwner.SpriteBatch);
    }

    private Vector2 GetDrawOffset()
    {
      if (this.State == UISnail.SnailState.Walking)
      {
        switch (this.WalkEdge)
        {
          case UISnail.SnailWalkEdge.Bottom:
            return new Vector2(this._snailAnim.Sprite.Offset.X, this._snailAnim.Sprite.Offset.Y);
          case UISnail.SnailWalkEdge.Right:
            return new Vector2(this._snailAnim.Sprite.Offset.Y, this._snailAnim.Sprite.Offset.X);
          case UISnail.SnailWalkEdge.Top:
            return new Vector2(this._snailAnim.Sprite.Offset.X, 0.0f);
          case UISnail.SnailWalkEdge.Left:
            return new Vector2(0.0f, this._snailAnim.Sprite.Offset.X);
        }
      }
      else if (this.Direction == UISnail.SnailDirection.Clowckwise)
      {
        switch (this.WalkEdge)
        {
          case UISnail.SnailWalkEdge.Bottom:
            return new Vector2(this._snailAnim.Sprite.Offset.Y, this._snailAnim.Sprite.Offset.X);
          case UISnail.SnailWalkEdge.Right:
            return new Vector2(this._snailAnim.Sprite.Offset.X, 0.0f);
          case UISnail.SnailWalkEdge.Top:
            return new Vector2(0.0f, 0.0f);
          case UISnail.SnailWalkEdge.Left:
            return new Vector2(0.0f, this._snailAnim.Sprite.Offset.X);
        }
      }
      else
      {
        switch (this.WalkEdge)
        {
          case UISnail.SnailWalkEdge.Bottom:
            return new Vector2(0.0f, this._snailAnim.Sprite.Offset.X);
          case UISnail.SnailWalkEdge.Right:
            return new Vector2(this._snailAnim.Sprite.Offset.X, this._snailAnim.Sprite.Offset.Y);
          case UISnail.SnailWalkEdge.Top:
            return new Vector2(this._snailAnim.Sprite.Offset.X, 0.0f);
          case UISnail.SnailWalkEdge.Left:
            return new Vector2(0.0f, 0.0f);
        }
      }
      return Vector2.Zero;
    }

    private enum SnailDirection
    {
      CounterClockwise = -1, // 0xFFFFFFFF
      Clowckwise = 1,
    }

    private enum SnailWalkEdge
    {
      Bottom,
      Right,
      Top,
      Left,
    }

    private enum SnailState
    {
      Walking,
      Turning,
      Falling,
    }
  }
}
