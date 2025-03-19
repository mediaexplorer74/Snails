
// Type: TwoBrainsGames.Snails.Stages.StageCursor
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.Snails.ToolObjects;


namespace TwoBrainsGames.Snails.Stages
{
  public class StageCursor : Object2D
  {
    public const int PLAYER_CURSOR_BLICK_INTERVAL_START = 600;
    public const int PLAYER_CURSOR_BLICK_INTERVAL_END = 1200;
    private StageCursor.CursorState _state;
    private StageCursor.CursorType _cursorType;
    private bool _toolActionIsValid;
    private bool IsOutOfStock;
    private StageCursorActionStatus ActionStatus;
    private Sprite _spriteDropping;
    private Sprite _spriteDefault;
    private Sprite _spritePan;
    private Sprite _spriteBeforePan;
    private SpriteAnimation _toolInvalidAnim;
    private ColorEffect _invalidToolEffect;
    private double Timer;
    private Vector2 _panLastPosition;
    private Vector2 _panLastOffset;
    private float _panDistance;
    private double _panEllapsedTime;
    private Sample _toolInvalidSample;
    private Color TOOL_OPACITY = new Color(180, 180, 180, 150);
    private ICursorInteractable _interactingObject;
    private ToolObject Tool;
    private StageCursor.CursorType _cursorBeforePanning;
    private bool _showInvalidActionSign;
    private float _startPinchScale;
    private double _pinchTime;

    public Vector2 ScreenPosition
    {
      get => BrainGame.GameCursor.Position;
      set => BrainGame.GameCursor.Position = value;
    }

    public bool IsInteractingWithObject
    {
      get => this._state == StageCursor.CursorState.InteractingWithObject;
    }

    public bool CanUseTools => Stage.CurrentStage._state == Stage.StageState.Playing;

    public bool CanInteractWithObjects => Stage.CurrentStage._state == Stage.StageState.Playing;

    public bool EndPanEaseOutEnabled { get; set; }

    public StageCursor()
    {
    }

    public StageCursor(StageCursor other)
      : base((Object2D) other)
    {
      this.Copy((Object2D) other);
      this.EndPanEaseOutEnabled = true;
    }

    public override void Copy(Object2D other) => base.Copy(other);

    public virtual void Initialize()
    {
      this._toolActionIsValid = false;
      if (Game1.GameSettings.UseGamepad)
        this.SetPositionInScreenMiddle();
      this.SetCursor(StageCursor.CursorType.Select);
      this._showInvalidActionSign = Game1.GameSettings.UseTouch;
    }

    public virtual void LoadContent()
    {
      this.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/player-cursor/DefaultCursor");
      this._spriteDefault = BrainGame.ResourceManager.GetSpriteStatic("spriteset/player-cursor/DefaultCursor");
      this._spriteDropping = BrainGame.ResourceManager.GetSpriteStatic("spriteset/player-cursor/DropCursor");
      this._spritePan = BrainGame.ResourceManager.GetSpriteStatic("spriteset/player-cursor/PanCursor");
      this._toolInvalidSample = BrainGame.ResourceManager.GetSampleTemporary("sfx/tool_invalid");
      this._toolInvalidAnim = new SpriteAnimation(BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD/ToolInvalid"));
      this._toolInvalidAnim.Autohide = true;
      this._invalidToolEffect = new ColorEffect(Color.White, new Color(0, 0, 0, 0), 0.03f, false);
      this._invalidToolEffect.UseRealTime = true;
    }

    public void ControllerEvents(BrainGameTime gameTime)
    {
      Vector2 vector2 = Vector2.Zero;
      if (Game1.GameSettings.UseMouse || Game1.GameSettings.UseTouch)
      {
        this.ScreenPosition = Stage.CurrentStage.Input.MotionPosition;
        vector2 = new Vector2(this.ScreenPosition.X, this.ScreenPosition.Y);
      }
      else if (Game1.GameSettings.UseGamepad)
      {
        vector2 = new Vector2(this.ScreenPosition.X, this.ScreenPosition.Y);
        if (Stage.CurrentStage.Input.CursorMotion)
        {
          vector2.X += Stage.CurrentStage.Input.MotionPosition.X * 7f;
          vector2.Y += Stage.CurrentStage.Input.MotionPosition.Y * -7f;
        }
        else
        {
          if (Stage.CurrentStage.Input.IsCursorUpPressed)
            vector2.Y -= (float) Stage.CurrentStage.Board.TileHeight;
          if (Stage.CurrentStage.Input.IsCursorDownPressed)
            vector2.Y += (float) Stage.CurrentStage.Board.TileHeight;
          if (Stage.CurrentStage.Input.IsCursorLeftPressed)
            vector2.X -= (float) Stage.CurrentStage.Board.TileWidth;
          if (Stage.CurrentStage.Input.IsCursorRightPressed)
            vector2.X += (float) Stage.CurrentStage.Board.TileWidth;
        }
      }
      Vector2 offset = Vector2.Zero;
      if ((double) vector2.X < 0.0)
      {
        offset = new Vector2(vector2.X, 0.0f);
        vector2.X = 0.0f;
      }
      else if ((double) vector2.X > (double) BrainGame.ScreenWidth)
      {
        offset = new Vector2(vector2.X - (float) BrainGame.ScreenWidth, 0.0f);
        vector2.X = (float) BrainGame.ScreenWidth;
      }
      if ((double) vector2.Y < 0.0)
      {
        offset = new Vector2(offset.X, vector2.Y);
        vector2.Y = 0.0f;
      }
      else if ((double) vector2.Y > (double) BrainGame.ScreenHeight)
      {
        offset = new Vector2(offset.X, vector2.Y - (float) BrainGame.ScreenHeight);
        vector2.Y = (float) BrainGame.ScreenHeight;
      }
      if (offset != Vector2.Zero && Game1.GameSettings.UseGamepad)
        Stage.CurrentStage.Camera.MoveByOffset(offset);
      this.ScreenPosition = vector2;
      this.Position = (vector2 - Stage.CurrentStage.Camera.Origin) / Stage.CurrentStage.Camera.Scale + Stage.CurrentStage.Camera.Position;
      this.UpdateBoundingBox();
      switch (this._state)
      {
        case StageCursor.CursorState.Normal:
          if (Stage.CurrentStage.Input.MapPinchStarted)
          {
            if (Stage.CurrentStage.StageHUD._stageArea.Contains(this.ScreenPosition))
            {
              this._state = StageCursor.CursorState.PichingMap;
              this._pinchTime = 0.0;
              this._startPinchScale = Stage.CurrentStage.Camera.Scale.X;
            }
          }
          else if (Stage.CurrentStage.Input.MapPanStarted)
          {
            if (!Stage.CurrentStage.StageHUD._stageArea.Contains(this.ScreenPosition))
              break;
            this._state = StageCursor.CursorState.PanningMap;
            this._panEllapsedTime = 0.0;
            this._panDistance = 0.0f;
            this._panLastPosition = this.ScreenPosition;
            this._spriteBeforePan = this.Sprite;
            this.Sprite = this._spritePan;
            this._cursorBeforePanning = this._cursorType;
            this.SetCursor(StageCursor.CursorType.PanningMap);
            break;
          }
          if (Stage.CurrentStage.Input.IsStageStartSelected && Stage.CurrentStage._state == Stage.StageState.Startup && Stage.CurrentStage.StageHUD._stageArea.Contains(this.ScreenPosition))
            Stage.CurrentStage.StartupEnded();
          if (Stage.CurrentStage.Input.IsToolUpClicked && Stage.CurrentStage.StageHUD._toolsMenu.HasToolsToSelect)
            Stage.CurrentStage.StageHUD._toolsMenu.DecrementSelection();
          if (Stage.CurrentStage.Input.IsToolDownClicked && Stage.CurrentStage.StageHUD._toolsMenu.HasToolsToSelect)
            Stage.CurrentStage.StageHUD._toolsMenu.IncrementSelection();
          if (Stage.CurrentStage.Input.IsActionPressed && Stage.CurrentStage.StageHUD._toolsMenu.IsOver(this.ScreenPosition))
          {
            BrainGame.GameCursor.SetCursor(0);
            Stage.CurrentStage.StageHUD._toolsMenu.ClickOverTool(this.ScreenPosition);
          }
          if (Stage.CurrentStage.StageHUD._toolsMenu.IsOver(this.ScreenPosition))
            break;
          this.CheckToolAction();
          break;
        case StageCursor.CursorState.PanningMap:
          this._panEllapsedTime += gameTime.ElapsedRealTime.TotalMilliseconds;
          if ((double) this.ScreenPosition.X < (double) Stage.CurrentStage.StageHUD._stageArea.Left)
            Stage.CurrentStage.Input.SetMotionPosition(new Vector2(Stage.CurrentStage.StageHUD._stageArea.Left, this.ScreenPosition.Y));
          else if ((double) this.ScreenPosition.X >= (double) Stage.CurrentStage.StageHUD._stageArea.Left + (double) Stage.CurrentStage.StageHUD._stageArea.Width)
            Stage.CurrentStage.Input.SetMotionPosition(new Vector2(Stage.CurrentStage.StageHUD._stageArea.Left + Stage.CurrentStage.StageHUD._stageArea.Width, this.ScreenPosition.Y));
          if ((double) this.ScreenPosition.Y < (double) Stage.CurrentStage.StageHUD._stageArea.Top)
            Stage.CurrentStage.Input.SetMotionPosition(new Vector2(this.ScreenPosition.X, Stage.CurrentStage.StageHUD._stageArea.Top));
          else if ((double) this.ScreenPosition.Y >= (double) Stage.CurrentStage.StageHUD._stageArea.Top + (double) Stage.CurrentStage.StageHUD._stageArea.Height)
            Stage.CurrentStage.Input.SetMotionPosition(new Vector2(this.ScreenPosition.X, Stage.CurrentStage.StageHUD._stageArea.Top + Stage.CurrentStage.StageHUD._stageArea.Height));
          if (!Stage.CurrentStage.Input.MapPanEnded)
            break;
          this.EndPan();
          break;
        case StageCursor.CursorState.InteractingWithObject:
          this.ProcessInteractingObjectInput();
          break;
        case StageCursor.CursorState.PichingMap:
          Stage.CurrentStage.Camera.Zoom(Stage.CurrentStage.Input.PinchScale);
          if (Stage.CurrentStage.Input.MapPinchEnded)
          {
            Stage.CurrentStage.Camera.EndPinch(this._startPinchScale, this._pinchTime);
            this._state = StageCursor.CursorState.Normal;
          }
          this._pinchTime += gameTime.ElapsedRealTime.TotalMilliseconds;
          break;
      }
    }

    private void ProcessInteractingObjectInput()
    {
      if (!this._interactingObject.CanAcceptCursorInteraction)
        return;
      if (Stage.CurrentStage.Input.IsActionDown)
        this._interactingObject.CursorActionPressed(this.Position);
      if (Stage.CurrentStage.Input.IsActionReleased)
        this._interactingObject.CursorActionReleased();
      if (!Stage.CurrentStage.Input.IsActionClicked)
        return;
      this._interactingObject.CursorActionSelected();
    }

    private void CheckToolAction()
    {
      if (!Stage.CurrentStage.Input.IsActionClicked || this.ActionStatus != StageCursorActionStatus.None || this.Tool == null || !this.Tool.WithEnoughQuantity || Stage.CurrentStage.StageHUD._toolsMenu.IsOver(this.ScreenPosition))
        return;
      if (this.CheckCurrentToolIsValid())
      {
        this.ActionStatus = StageCursorActionStatus.Action;
      }
      else
      {
        if (this._showInvalidActionSign)
        {
          this._toolInvalidAnim.Visible = true;
          this._toolInvalidAnim.Position = this.ScreenPosition;
          this._toolInvalidAnim.Reset();
          this._toolInvalidAnim.EffectsBlender.Clear();
          this._invalidToolEffect.Reset();
          this._toolInvalidAnim.EffectsBlender.Add((ITransformEffect) this._invalidToolEffect);
        }
        this._toolInvalidSample.Play();
      }
    }

    private bool CheckCurrentToolIsValid() => this.Tool.IsValidAtPosition(this.Position);

    public override void Update(BrainGameTime gameTime)
    {
      switch (this._state)
      {
        case StageCursor.CursorState.Normal:
          if (!Stage.CurrentStage.StageHUD._toolsMenu.IsOver(this.ScreenPosition))
          {
            switch (this.ActionStatus)
            {
              case StageCursorActionStatus.None:
                if (Game1.GameSettings.ShowCursor && this.Tool != null && !this.IsInteractingWithObject)
                {
                  this._toolActionIsValid = this.CheckCurrentToolIsValid();
                  this.IsOutOfStock = false;
                  this.SetCursor(StageCursor.CursorType.ToolCursor);
                  break;
                }
                break;
              case StageCursorActionStatus.Action:
                if (this.Tool.WithEnoughQuantity)
                {
                  this.Tool.Action(this.Position);
                  this.ActionStatus = StageCursorActionStatus.DroppingObject;
                  this.SetCursor(StageCursor.CursorType.Dropping);
                  this.Timer = 0.0;
                  if (this.Tool.Quantity <= 0)
                  {
                    Stage.CurrentStage.StageHUD._toolsMenu.RemoveTool(this.Tool);
                    this.SetCursor(StageCursor.CursorType.OutOfStock);
                    this.ActionStatus = StageCursorActionStatus.None;
                    this.IsOutOfStock = true;
                    break;
                  }
                  break;
                }
                break;
              case StageCursorActionStatus.DroppingObject:
                this.Timer += gameTime.ElapsedGameTime.TotalMilliseconds;
                if (this.Timer > 250.0)
                {
                  this.Timer = 0.0;
                  this.ActionStatus = StageCursorActionStatus.None;
                  this.SetCursor(StageCursor.CursorType.NoAction);
                  break;
                }
                break;
            }
          }
          else
          {
            if (this.IsOutOfStock)
            {
              this.IsOutOfStock = false;
              this.SetCursor(StageCursor.CursorType.NoAction);
              break;
            }
            this.SetCursor(StageCursor.CursorType.Select);
            break;
          }
          break;
        case StageCursor.CursorState.PanningMap:
          this._panLastOffset = this._panLastPosition - this.ScreenPosition;
          if (this._panLastOffset != Vector2.Zero)
            Stage.CurrentStage.Camera.MoveByOffset(this._panLastOffset);
          this._panDistance += this._panLastOffset.Length();
          this._panLastPosition = this.ScreenPosition;
          this.ScreenPosition = this._panLastPosition;
          break;
        case StageCursor.CursorState.InteractingWithObject:
          if (this._interactingObject.QueryInterating())
          {
            this.SetCursor(this._interactingObject.QueryCursor());
            return;
          }
          this._interactingObject = (ICursorInteractable) null;
          this._state = StageCursor.CursorState.Normal;
          break;
      }
      if (this.Tool != null)
        this.Tool.Update(gameTime);
      if (this._toolInvalidAnim.Visible)
        this._toolInvalidAnim.Update(gameTime, true);
      base.Update(gameTime);
    }

    public virtual void Draw()
    {
      if (Game1.Tutorial.TopicVisible)
        return;
      if (Game1.GameSettings.ShowCursor && this.IsCursorToolActive() && !Stage.CurrentStage.StageHUD._toolsMenu.IsOver(this.ScreenPosition) && this._state != StageCursor.CursorState.PanningMap)
      {
        if (this.Tool != null && this.Tool.SnapIt && this.ActionStatus == StageCursorActionStatus.None)
          this.DrawSelectedToolObjectsSnapped();
        else if (this.Tool != null && !this.Tool.SnapIt && this.ActionStatus == StageCursorActionStatus.None)
          this.DrawSelectedToolObjects();
      }
      if (!this._toolInvalidAnim.Visible)
        return;
      this._toolInvalidAnim.Draw(Stage.CurrentStage.SpriteBatch);
    }

    private bool IsCursorToolActive()
    {
      return !this.IsInteractingWithObject && !Stage.CurrentStage.StageHUD.IsInteractingWithCursor;
    }

    internal void DrawSelectedToolObjects()
    {
      this.Tool.DrawCursor(this.Position - Stage.CurrentStage.Camera.UpperLeftScreenCorner, this._toolActionIsValid);
    }

    internal void DrawSelectedToolObjectsSnapped()
    {
      this.Tool.ObjectSprite.Draw(new Vector2((float) (this.BoardX * Stage.CurrentStage.Board.TileWidth - (int) Stage.CurrentStage.Camera.UpperLeftScreenCorner.X), (float) (this.BoardY * Stage.CurrentStage.Board.TileHeight - (int) Stage.CurrentStage.Camera.UpperLeftScreenCorner.Y)), 0, this.Tool.CursorDrawBlendColor, Levels.CurrentLevel.SpriteBatch);
    }

    internal void DrawTileCellArea()
    {
      BrainGame.DrawRectangleFilled(Levels.CurrentLevel.SpriteBatch, new Rectangle(this.BoardX * Stage.CurrentStage.Board.TileWidth - (int) Stage.CurrentStage.Camera.UpperLeftScreenCorner.X, this.BoardY * Stage.CurrentStage.Board.TileHeight - (int) Stage.CurrentStage.Camera.UpperLeftScreenCorner.Y, Stage.CurrentStage.Board.TileWidth, Stage.CurrentStage.Board.TileHeight), new Color(0, 0, (int) byte.MaxValue, 50));
    }

    private void EndPan()
    {
      this._state = StageCursor.CursorState.Normal;
      if (this._spriteBeforePan != null)
        this.Sprite = this._spriteBeforePan;
      this.SetCursor(this._cursorBeforePanning);
      Vector2 panLastOffset = this._panLastOffset;
      if (!(this._panLastOffset != Vector2.Zero) || !this.EndPanEaseOutEnabled)
        return;
      float num = this._panDistance / (float) this._panEllapsedTime;
      panLastOffset.Normalize();
      Vector2 speedVector = panLastOffset * num;
      Stage.CurrentStage.Camera.DoFlick(speedVector);
    }

    public void GameLostFocus()
    {
      if (this._state == StageCursor.CursorState.PanningMap)
        this.EndPan();
      this._state = StageCursor.CursorState.Normal;
    }

    public void TutorialTopicOpened()
    {
      if (this._state == StageCursor.CursorState.PanningMap)
        this.EndPan();
      this.SetCursor(StageCursor.CursorType.Select);
      this._state = StageCursor.CursorState.TutorialOpened;
    }

    public void TutorialTopicClosed()
    {
      this.SetCursor(StageCursor.CursorType.Select);
      this._state = StageCursor.CursorState.Normal;
    }

    public void SetPositionInScreenMiddle()
    {
      float x = (float) (BrainGame.ScreenWidth / 2 / Stage.CurrentStage.Board.TileWidth);
      float y = (float) (BrainGame.ScreenWidth / 2 / Stage.CurrentStage.Board.TileHeight);
      this.ScreenPosition = new Vector2(x, y);
      this.Position = new Vector2(x, y);
    }

    public void SetInteractingObject(ICursorInteractable obj)
    {
      if (this._state == StageCursor.CursorState.PanningMap)
        return;
      this._state = StageCursor.CursorState.InteractingWithObject;
      this._interactingObject = obj;
      this.SetCursor(obj.QueryCursor());
      this.ProcessInteractingObjectInput();
    }

    public void SetPosition(Vector2 position)
    {
      this.ScreenPosition = position;
      this.Position = position;
      StageCursor stageCursor = this;
      stageCursor.Position = stageCursor.Position + Stage.CurrentStage.Camera.Position;
      Stage.CurrentStage.Input.SetMotionPosition(position);
    }

    private void SetCursor(StageCursor.CursorType cursor)
    {
      this._cursorType = cursor;
      switch (this._cursorType)
      {
        case StageCursor.CursorType.NoAction:
          BrainGame.GameCursor.SetCursor(2);
          break;
        case StageCursor.CursorType.Select:
          BrainGame.GameCursor.SetCursor(0);
          break;
        case StageCursor.CursorType.Dropping:
          this.Sprite = this._spriteDropping;
          break;
        case StageCursor.CursorType.OutOfStock:
          BrainGame.GameCursor.SetCursor(0);
          break;
        case StageCursor.CursorType.ToolCursor:
          if (this.Tool == null)
            break;
          this.Tool.SetCursorOnBoard(this._toolActionIsValid);
          break;
        case StageCursor.CursorType.PanningMap:
          BrainGame.GameCursor.SetCursor(6);
          break;
        default:
          this.Sprite = this._spriteDefault;
          break;
      }
    }

    public void SetSelectedTool(ToolObject tool)
    {
      this.Tool = tool;
      if (tool == null)
        return;
      this.Tool.CursorDrawBlendColor = new Color(this.Tool.BlendColor.ToVector4() * ((float) this.TOOL_OPACITY.A / (float) byte.MaxValue));
    }

    public int BoardX
    {
      get => (int) ((double) this.X / (double) Stage.CurrentStage.Board.TileWidth);
      set => this.X = (float) (value * Stage.CurrentStage.Board.TileWidth);
    }

    public int BoardY
    {
      get => (int) ((double) this.Y / (double) Stage.CurrentStage.Board.TileHeight);
      set => this.Y = (float) (value * Stage.CurrentStage.Board.TileHeight);
    }

    public enum CursorType
    {
      NoAction,
      Select,
      Dropping,
      OutOfStock,
      ToolCursor,
      PanningMap,
    }

    private enum CursorState
    {
      Normal,
      PanningMap,
      InteractingWithObject,
      TutorialOpened,
      PichingMap,
    }
  }
}
