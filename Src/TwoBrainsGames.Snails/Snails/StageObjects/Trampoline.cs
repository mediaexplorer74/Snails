
// Type: TwoBrainsGames.Snails.StageObjects.Trampoline
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class Trampoline : MovingObject, ICursorInteractable
  {
    public const string ID = "TRAMPOLINE";
    public const string SPRITE_BASE = "Base";
    public const string SPRITE_POWER = "Power";
    private const string SPRITE_SHOOT = "TriggerShoot";
    private const string SPRITE_HOT_SPOT = "HotSpot";
    private const string SPRITE_DOTS = "Dots";
    private const string SPRITE_TRAJECT_DOTS = "TrajectoryDot";
    private const float MAX_SPEED = 90f;
    private const float MAX_ANGLE = 75f;
    private const float MIN_ANGLE = -75f;
    private const float MIN_SPEED_PERCENT = 30f;
    private const int CRATE_TOOL_VALID_BB_IDX = 1;
    private const float HOT_SPOT_OFFSET_LENGTH = 0.0f;
    private const int NUM_TRAJECTORY_DOTS = 20;
    private const int TRAJECTORY_DOTS_START_FADE_TIME = 1000;
    private const int BS_CONTROLLER_IDX = 0;
    private const int BS_CONTROLLER_WP_IDX = 1;
    private float _angle;
    private float _jumpSpeed;
    private float _jumpSpeedPercent;
    private Sprite _powerSprite;
    private Sprite _dotsSprite;
    private Sprite _baseSprite;
    private Sprite _shootSprite;
    private Sprite _idleSprite;
    private Sprite _trajDotsSprite;
    private Trampoline.TrampolineState _state;
    private BoundingCircle _bsControllerTransf;
    private bool _cursorDown;
    private float _powerSpriteScale;
    private Vector2 _powerBaseVector;
    private float _powerBaseVectorLen;
    private Sprite _spriteHotSpot;
    private float _hotSpotRotation;
    private Sample _jumpSample;
    private Sample _throwSample;
    private float _hotSpotOffsetLength;
    private BoundingCircle _bsController;
    private Vector2 _dotsPosition;
    private Vector2[] _preCalcTrajDots = new Vector2[20];
    private int _trajDotFrameIdx;
    private float _trajDotFrameElapsed;
    private int _lastDot;
    private Trampoline.TrajectoryDotsFadeState _trajectDotsFadeState;
    private Color _trajDotColor = Color.White;
    private float _trajDotFadeElapsed;

    private Vector2 HotSpotPosition => this._bsControllerTransf._center;

    public float Angle => this._angle;

    public float JumpSpeed => this._jumpSpeed;

    public Trampoline()
      : base(StageObjectType.Trampoline)
    {
      this.DrawInForeground = true;
      this._SpritePlaybackMode = Object2D.AnimtionPlaybackModes.PlayOnce;
      this._jumpSpeedPercent = 50f;
    }

    public Trampoline(Trampoline other)
      : base((StageObject) other)
    {
      this.Copy((StageObject) other);
    }

    public Trampoline(string resourceId)
      : this()
    {
      this.ResourceId = resourceId;
    }

    public override void Copy(StageObject other)
    {
      base.Copy(other);
      Trampoline trampoline = other as Trampoline;
      this._angle = trampoline._angle;
      this._jumpSpeed = trampoline._jumpSpeed;
      this._jumpSpeedPercent = trampoline._jumpSpeedPercent;
      this._baseSprite = trampoline._baseSprite;
      this._powerSprite = trampoline._powerSprite;
      this._dotsSprite = trampoline._dotsSprite;
      this._shootSprite = trampoline._shootSprite;
      this._spriteHotSpot = trampoline._spriteHotSpot;
      this._trajDotsSprite = trampoline._trajDotsSprite;
      this._bsController = trampoline._bsController;
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this._powerSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "Power");
      this._baseSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "Base");
      this._shootSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "TriggerShoot");
      this._spriteHotSpot = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "HotSpot");
      this._dotsSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "Dots");
      this._trajDotsSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "TrajectoryDot");
      this._idleSprite = this.Sprite;
      this._jumpSample = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/trampoline", (Object2D) this);
      this._throwSample = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/snail-throw", (Object2D) this);
    }

    public override void Initialize()
    {
      base.Initialize();
      this._hotSpotOffsetLength = 0.0f;
      this._bsController = this._powerSprite._boundingSpheres[0];
      if (Game1.GameSettings.UseTouch)
      {
        this._hotSpotOffsetLength = 0.0f;
        this._bsController = this._powerSprite._boundingSpheres[1];
      }
      this._powerBaseVector = this._bsController._center;
      this._powerBaseVectorLen = this._powerBaseVector.Length();
      this._powerBaseVector.Y *= -1f;
      this.ComputeJumpSpeed();
      this.UpdateControllerBB();
      this._hotSpotRotation = 0.0f;
      this._jumpSpeedPercent = 50f;
    }

    public override void UpdateBoundingBox()
    {
      base.UpdateBoundingBox();
      this.UpdateControllerBB();
    }

    private void UpdateControllerBB()
    {
      float num = (float) ((double) this._jumpSpeedPercent * (double) this._powerBaseVectorLen / 100.0);
      Vector2 powerBaseVector = this._powerBaseVector;
      powerBaseVector.Normalize();
      Vector2 vector2 = Vector2.Transform(powerBaseVector * (num + this._hotSpotOffsetLength), Matrix.CreateRotationZ(MathHelper.ToRadians(this._angle)));
      if (this._bsController != null)
        this._bsControllerTransf = new BoundingCircle(this.Position - vector2, this._bsController._radius);
      vector2.Normalize();
      this._dotsPosition = this.Position - vector2 * num;
    }

    private void ComputeJumpSpeed()
    {
      this._jumpSpeed = (float) ((double) this._jumpSpeedPercent * 90.0 / 100.0);
      this._powerSpriteScale = this._jumpSpeedPercent / 100f;
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      if (this._state != Trampoline.TrampolineState.Shooting)
        this.DoQuadtreeCollisions(0);
      if (this.QueryCursorInsideInteractingZone() && !Stage.CurrentStage.Cursor.IsInteractingWithObject)
        Stage.CurrentStage.Cursor.SetInteractingObject((ICursorInteractable) this);
      this._hotSpotRotation += (float) (0.029999999329447746 * gameTime.ElapsedGameTime.TotalMilliseconds);
      this._trajDotFrameElapsed += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
      if ((double) this._trajDotFrameElapsed > 25.0)
      {
        this._trajDotFrameElapsed = 0.0f;
        ++this._trajDotFrameIdx;
        if (this._trajDotFrameIdx >= this._lastDot)
          this._trajDotFrameIdx = 0;
      }
      if (this.QueryInterating() || this._trajectDotsFadeState == Trampoline.TrajectoryDotsFadeState.StartFading)
      {
        this.ComputeTrajectoryDots();
      }
      else
      {
        if (this._trajectDotsFadeState == Trampoline.TrajectoryDotsFadeState.Computing)
          this._trajectDotsFadeState = Trampoline.TrajectoryDotsFadeState.StartFading;
        if (this._trajectDotsFadeState == Trampoline.TrajectoryDotsFadeState.StartFading)
        {
          this._trajDotFadeElapsed += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
          if ((double) this._trajDotFadeElapsed > 1000.0)
          {
            this._trajDotFadeElapsed = 0.0f;
            this._trajectDotsFadeState = Trampoline.TrajectoryDotsFadeState.Fading;
          }
        }
      }
      if (this._trajectDotsFadeState != Trampoline.TrajectoryDotsFadeState.Fading)
        return;
      Vector4 vector4 = this._trajDotColor.ToVector4();
      vector4.W -= 0.025f;
      vector4.X -= 0.025f;
      vector4.Y -= 0.025f;
      vector4.Z -= 0.025f;
      if ((double) vector4.W < 0.0)
      {
        vector4.W = 0.0f;
        this._trajectDotsFadeState = Trampoline.TrajectoryDotsFadeState.Idle;
      }
      this._trajDotColor = new Color(vector4);
    }

    private bool QueryCursorInsideInteractingZone()
    {
      return !(Stage.CurrentStage.Cursor.ScreenPosition == Vector2.Zero) && this._bsControllerTransf.Contains(Stage.CurrentStage.Cursor.Position);
    }

    public override void OnLastFrame()
    {
      if (this._state != Trampoline.TrampolineState.Shooting)
        return;
      this._state = Trampoline.TrampolineState.Idle;
      this.Sprite = this._idleSprite;
      this.CurrentFrame = 0;
    }

    public override void OnCollide(IQuadtreeContainable obj, int listIdx)
    {
      if (this._state == Trampoline.TrampolineState.Shooting)
        return;
      Snail snail = obj as Snail;
      if (!snail.CanJumpTranpoline)
        return;
      BoundingSquare boundingSquare = this.TransformSpriteFrameBB(1).ToBoundingSquare();
      if (!snail.CheckCollisionWithTrampolim(boundingSquare))
        return;
      this._jumpSample.Play();
      this._throwSample.Play();
      snail.JumpOnTrampoline(this);
      this._state = Trampoline.TrampolineState.Shooting;
      this.Sprite = this._shootSprite;
      this.CurrentFrame = 0;
    }

    public override void Draw(bool shadow)
    {
      Vector2 zero = Vector2.Zero;
      Color opacity = Color.White;
      if (shadow)
      {
        zero += GenericConsts.ShadowDepth;
        opacity = this.ShadowColor;
      }
      this._powerSprite.Draw(this.Position + zero, 0, this._angle, this.Sprite.Offset, 1f, this._powerSpriteScale, opacity, Stage.CurrentStage.SpriteBatch);
      this._baseSprite.Draw(this.Position + zero, 0, 0.0f, SpriteEffects.None, 1f, opacity, 1f, Stage.CurrentStage.SpriteBatch);
      base.Draw(shadow);
      this._spriteHotSpot.Draw(this.HotSpotPosition, 0, this._hotSpotRotation, SpriteEffects.None, Levels.CurrentLevel.SpriteBatch);
      if ((double) this._hotSpotOffsetLength == 0.0)
        return;
      this._dotsSprite.Draw(this._dotsPosition, 0, this._angle, SpriteEffects.None, Levels.CurrentLevel.SpriteBatch);
    }

    private void DrawTrajectoryDots()
    {
      int num = 0;
      for (int index = 0; index <= this._lastDot; ++index)
      {
        Vector2 preCalcTrajDot = this._preCalcTrajDots[index];
        int frame = num == this._trajDotFrameIdx ? 1 : 0;
        this._trajDotsSprite.Draw(preCalcTrajDot, frame, 0.0f, SpriteEffects.None, this._trajDotColor, 1f, Levels.CurrentLevel.SpriteBatch);
        ++num;
        if (BrainGame.Settings.ShowBoundingBoxes)
        {
          BoundingSquare boundingBox = this._trajDotsSprite.BoundingBox;
          boundingBox.TransformInPlace(preCalcTrajDot);
          boundingBox.Draw(Color.Red, Stage.CurrentStage.Camera.Position);
        }
      }
    }

    public override void ForegroundDraw()
    {
      base.ForegroundDraw();
      if (this.QueryInterating() || this._trajectDotsFadeState == Trampoline.TrajectoryDotsFadeState.StartFading || this._trajectDotsFadeState == Trampoline.TrajectoryDotsFadeState.Computing)
      {
        this._trajDotColor = Color.White;
        this.DrawTrajectoryDots();
      }
      else
      {
        if (this._trajectDotsFadeState != Trampoline.TrajectoryDotsFadeState.Fading)
          return;
        this.DrawTrajectoryDots();
      }
    }

    private void ComputeTrajectoryDots()
    {
      this._trajectDotsFadeState = Trampoline.TrajectoryDotsFadeState.Computing;
      float num1 = (float) Math.Sin((double) MathHelper.ToRadians(90f - this._angle));
      Vector2 vector2 = new Vector2((float) Math.Cos((double) MathHelper.ToRadians(90f - this._angle)), -num1) * this._jumpSpeed;
      double num2 = 1.0;
      this._lastDot = 19;
      for (int index = 0; index < 20; ++index)
      {
        num2 += 0.75;
        Vector2 p1 = new Vector2(this.X + vector2.X * (float) num2, this.Y + (float) ((double) vector2.Y * num2 + 0.5 * (double) Game1.GameSettings.Gravity * (num2 * num2)));
        Vector2 p0 = index > 0 ? this._preCalcTrajDots[index - 1] : Vector2.Zero;
        if (index == 0)
          this._preCalcTrajDots[index] = p1;
        else if (Stage.CurrentStage.Board.PathCollidesWithVector(p0, p1) == null)
        {
          this._preCalcTrajDots[index] = p1;
        }
        else
        {
          this._lastDot = index - 1;
          break;
        }
      }
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this._angle = record.GetFieldValue<float>("angle", this._angle);
    }

    public override DataFileRecord ToDataFileRecord()
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord();
      dataFileRecord.AddField("angle", (object) (int) this._angle);
      return dataFileRecord;
    }

    public StageCursor.CursorType QueryCursor() => StageCursor.CursorType.Select;

    public bool QueryInterating() => this._cursorDown || this.QueryCursorInsideInteractingZone();

    public void CursorActionPressed(Vector2 cursorPos)
    {
      if (!this._cursorDown)
      {
        Stage.CurrentStage.Cursor.SetPosition(this._bsControllerTransf._center - Stage.CurrentStage.Camera.UpperLeftScreenCorner);
        this._cursorDown = true;
      }
      else
      {
        if ((double) (this._bsControllerTransf._center - cursorPos).Length() < 1.5)
          return;
        Vector2 v1 = this.Position - cursorPos;
        float num = v1.Length() - this._hotSpotOffsetLength;
        if ((double) num > (double) this._powerBaseVectorLen)
          num = this._powerBaseVectorLen;
        this._jumpSpeedPercent = num * 100f / this._powerBaseVectorLen;
        if ((double) this._jumpSpeedPercent > 100.0)
          this._jumpSpeedPercent = 100f;
        if ((double) this._jumpSpeedPercent < 30.0)
          this._jumpSpeedPercent = 30f;
        v1.Normalize();
        Vector2 powerBaseVector = this._powerBaseVector;
        powerBaseVector.Normalize();
        this._angle = Mathematics.AngleBetweenNormalizedVectors(v1, powerBaseVector);
        if ((double) this._angle < -75.0)
          this._angle = -75f;
        if ((double) this._angle > 75.0)
          this._angle = 75f;
        this.ComputeJumpSpeed();
        this.UpdateControllerBB();
        Stage.CurrentStage.Cursor.SetPosition(this._bsControllerTransf._center - Stage.CurrentStage.Camera.UpperLeftScreenCorner);
      }
    }

    public void CursorActionReleased() => this._cursorDown = false;

    public void CursorActionSelected()
    {
    }

    private enum TrampolineState
    {
      Idle,
      Shooting,
    }

    private enum TrajectoryDotsFadeState
    {
      Idle,
      Computing,
      StartFading,
      Fading,
    }
  }
}
