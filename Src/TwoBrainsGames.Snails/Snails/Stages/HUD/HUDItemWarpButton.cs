
// Type: TwoBrainsGames.Snails.Stages.HUD.HUDItemWarpButton
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.StageObjects;


namespace TwoBrainsGames.Snails.Stages.HUD
{
  internal class HUDItemWarpButton : HUDItem, ICursorInteractable
  {
    private const int BUTTON_FRAME_IDX = 0;
    private const int FAST_FWD_FRAME_IDX = 1;
    private const int NORMAL_SPEED_FRAME_IDX = 2;
    private const int KEYBOARD_HELP = 3;
    private Sprite _spriteWarp;
    private Vector2 _signPosition;
    private Vector2 _helpPosition;
    private int _signSpriteFrameIdx;
    private BoundingSquare _bsButton;
    private bool _buttonReleased;
    private bool _isInteracting;
    private int _keyHelpFrameIdx;
    private bool _showKeyHelp;

    public HUDItemWarpButton() => this._buttonReleased = true;

    public override void Initialize(Vector2 position)
    {
      base.Initialize(position);
      this._position = new Vector2((float) BrainGame.ScreenRectangle.X - 10f, (float) BrainGame.ScreenHeight - 70f);
      this._signPosition = this._position + new Vector2(15f, 20f);
      if (Game1.GameSettings.UseKeyboard)
      {
        this._helpPosition = this._position + new Vector2(18f, -7f);
        this._keyHelpFrameIdx = 4;
        this._showKeyHelp = true;
      }
      else if (Game1.GameSettings.UseGamepad)
      {
        this._helpPosition = this._position + new Vector2(72f, -20f);
        this._keyHelpFrameIdx = 1;
        this._showKeyHelp = true;
      }
      else if (Game1.GameSettings.UseTouch)
      {
        this._helpPosition = Vector2.Zero;
        this._keyHelpFrameIdx = 0;
        this._showKeyHelp = false;
      }
      this._signSpriteFrameIdx = 1;
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this._spriteWarp = new Sprite(BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "TimeWarpButton"));
      this._bsButton = this._spriteWarp.BoundingBox.Transform(this._position);
      this._size = new Vector2((float) this._spriteWarp.Frames[0].Rect.X, (float) this._spriteWarp.Frames[0].Rect.Y);
    }

    public override void Update(BrainGameTime gameTime)
    {
      if (Game1.GameSettings.UseTouch)
      {
        if (!this._isInteracting && this.QueryCursorInsideInteractingZone())
        {
          Stage.CurrentStage.Cursor.SetInteractingObject((ICursorInteractable) this);
          this._buttonReleased = true;
          this._isInteracting = true;
        }
        else
        {
          if (!this._buttonReleased)
            return;
          this._isInteracting = false;
        }
      }
      else if (this.QueryCursorInsideInteractingZone())
      {
        if (this._isInteracting)
          return;
        Stage.CurrentStage.Cursor.SetInteractingObject((ICursorInteractable) this);
        this._buttonReleased = true;
        this._isInteracting = true;
      }
      else
        this._isInteracting = false;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      this._spriteWarp.Draw(this._position, 0, spriteBatch);
      this._spriteWarp.Draw(this._signPosition, this._signSpriteFrameIdx, spriteBatch);
      if (!this._showKeyHelp)
        return;
      StageHUD.SpriteXBoxHelp.Draw(this._helpPosition, this._keyHelpFrameIdx, spriteBatch);
    }

    private bool QueryCursorInsideInteractingZone()
    {
      return this._bsButton.Contains(Stage.CurrentStage.Cursor.ScreenPosition);
    }

    public override void TimeWarpChanged()
    {
      if (Stage.CurrentStage.InTimeWarp)
        this._signSpriteFrameIdx = 2;
      else
        this._signSpriteFrameIdx = 1;
    }

    public StageCursor.CursorType QueryCursor() => StageCursor.CursorType.Select;

    public bool QueryInterating() => this.QueryCursorInsideInteractingZone();

    public void CursorActionPressed(Vector2 cursorPos)
    {
      if (!this._buttonReleased)
        return;
      Stage.CurrentStage.ToggleTimeWarp();
      this._buttonReleased = false;
    }

    public void CursorActionReleased() => this._buttonReleased = true;

    public void CursorActionSelected()
    {
    }

    public bool CanAcceptCursorInteraction => Stage.CurrentStage._state == Stage.StageState.Playing;
  }
}
