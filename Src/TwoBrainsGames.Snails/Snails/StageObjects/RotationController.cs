
// Type: TwoBrainsGames.Snails.StageObjects.RotationController
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.Configuration;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  internal class RotationController : ICursorInteractable
  {
    private const int CONTROLLER_SPRITE = 0;
    private const int CONTROLLER_HOTSPOT_IDX = 1;
    private const int CONTROLLER_HOTSPOT_IDX_TOUCH = 2;
    private const float SENSIVITY_HD = 0.04f;
    private const float SENSIVITY_LD = 0.06f;
    private const float MAX_OFFSET = 10f;
    private SpriteAnimation _arrowsAnimation;
    private Sprite _arrowsSprite;
    private Sprite _controllerSprite;
    private Vector2 _controllerPosition;
    private Vector2 _leftArrowPosition;
    private int _controllerCurFrame;
    private IRotationControllable _parentObject;
    private BoundingSquare _bbControllerHotSpot;
    private bool _cursorDown;
    private Vector2 _interactingInitialPos;
    private Vector2 _position;
    private ColorEffect _arrowFadeEffect;
    private float _sensivity;
    private int _controllerBBIdx;
    private Sample _tickSound;

    public RotationController(IRotationControllable parent) => this._parentObject = parent;

    public void SetPosition(Vector2 position) => this._position = position;

    public void LoadContent()
    {
      this._arrowsSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/laser-beam", "ControllerArrows");
      this._controllerSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/laser-beam", "LaserController");
      this._tickSound = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/rotation-controller-tick", (Object2D) this._parentObject);
    }

    public void Initialize()
    {
      this._arrowsAnimation = new SpriteAnimation(this._arrowsSprite);
      this._controllerCurFrame = 0;
      this._arrowFadeEffect = new ColorEffect(Color.White, new Color(0, 0, 0, 0), 0.05f, false);
      this._arrowFadeEffect.Active = false;
      this._sensivity = Game1.GameSettings.PresentationMode == GameSettings.PresentationType.HD ? 0.04f : 0.06f;
      this._controllerBBIdx = Game1.GameSettings.UseTouch ? 2 : 1;
      this.Refresh();
    }

    public void Update(BrainGameTime gameTime)
    {
      this._arrowsAnimation.Update(gameTime);
      if (!this._arrowFadeEffect.Active)
        return;
      this._arrowFadeEffect.Update(gameTime);
    }

    public void Draw()
    {
      this._controllerSprite.Draw(this._controllerPosition, this._controllerCurFrame, this._parentObject.ControlledObject.Rotation, this._parentObject.ControlledObject.SpriteEffect, Stage.CurrentStage.SpriteBatch);
    }

    public void DrawForeground()
    {
      this._arrowsAnimation.Draw(this._leftArrowPosition, this._parentObject.ControlledObject.Rotation, this._arrowFadeEffect.Color, Stage.CurrentStage.SpriteBatch);
    }

    public void Refresh()
    {
      OOBoundingBox ooBoundingBox = this._parentObject.ControlledObject.TransformBoundingBox(this._controllerSprite.BoundingBox.Transform(this._position));
      this._controllerPosition = this._parentObject.ControlledObject.IsHorizontallyFlipped ? ooBoundingBox.P1 : ooBoundingBox.P0;
      this._leftArrowPosition = ooBoundingBox.GetCenter();
      ooBoundingBox = this._parentObject.ControlledObject.TransformBoundingBox(this._controllerSprite.BoundingBoxes[this._controllerBBIdx].Transform(this._position));
      this._bbControllerHotSpot = ooBoundingBox.ToBoundingSquare();
    }

    public bool Contains(Vector2 pt) => this._bbControllerHotSpot.Contains(pt);

    public StageCursor.CursorType QueryCursor() => StageCursor.CursorType.Select;

    public bool QueryInterating()
    {
      if (Stage.CurrentStage.Cursor.ScreenPosition == Vector2.Zero)
        return false;
      return this._cursorDown || this.Contains(Stage.CurrentStage.Cursor.Position);
    }

    public void CursorActionPressed(Vector2 cursorPos)
    {
      if (!this._cursorDown)
      {
        this._interactingInitialPos = cursorPos;
        this._cursorDown = true;
        this._arrowFadeEffect.Active = true;
        this._arrowFadeEffect.StartColor = this._arrowFadeEffect.CurrentColor;
        this._arrowFadeEffect.EndColor = new Color(0, 0, 0, 0);
      }
      else
      {
        this._cursorDown = true;
        if (cursorPos == this._interactingInitialPos)
          return;
        int num1 = 0;
        switch ((int) this._parentObject.ControlledObject.Rotation)
        {
          case 0:
            num1 = (double) cursorPos.X > (double) this._interactingInitialPos.X ? 1 : -1;
            break;
          case 90:
            num1 = (double) cursorPos.Y > (double) this._interactingInitialPos.Y ? 1 : -1;
            break;
          case 180:
            num1 = (double) cursorPos.X > (double) this._interactingInitialPos.X ? -1 : 1;
            break;
          case 270:
            num1 = (double) cursorPos.Y > (double) this._interactingInitialPos.Y ? -1 : 1;
            break;
        }
        float num2 = (float) -((double) (this._interactingInitialPos - cursorPos).LengthSquared() * (double) this._sensivity * (double) num1);
        if ((double) num2 > 10.0)
          num2 = 10f;
        if (this._parentObject.ControllerValueChanged(num2))
        {
          ++this._controllerCurFrame;
          if (this._controllerCurFrame >= this._controllerSprite.FrameCount)
            this._controllerCurFrame = 0;
          this._tickSound.Play();
        }
        this._interactingInitialPos = cursorPos;
      }
    }

    public void CursorActionReleased()
    {
      this._cursorDown = false;
      this._arrowFadeEffect.StartColor = this._arrowFadeEffect.CurrentColor;
      this._arrowFadeEffect.EndColor = Color.White;
    }

    public void CursorActionSelected()
    {
    }

    public bool CanAcceptCursorInteraction => Stage.CurrentStage._state == Stage.StageState.Playing;
  }
}
