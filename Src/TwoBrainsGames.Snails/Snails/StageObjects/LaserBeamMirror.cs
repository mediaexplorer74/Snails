
// Type: TwoBrainsGames.Snails.StageObjects.LaserBeamMirror
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class LaserBeamMirror : 
    MovingObject,
    ISnailsDataFileSerializable,
    IDataFileSerializable,
    ICursorInteractable,
    IRotationControllable
  {
    public const float MAX_ROTATION = 160f;
    public const float MIN_ROTATION = 20f;
    private const int MIRROR_BS_IDX = 0;
    private const int MIRROR_REFLECTOR_BS_IDX = 1;
    private const int CRATE_COLLISION_BB_IDX = 2;
    private const int MIRROR_POSITION_BS_IDX = 0;
    private const int CONTROLLER_IDX = 1;
    private OOBoundingBox _mirrorOOBB;
    private BoundingSquare _mirrorBB;
    private OOBoundingBox _mirrorReflectorBB;
    private float _mirrorRotation;
    private Vector2 _mirrorReflectorPosition;
    private Sprite _mirrorSprite;
    private RotationController _controller;

    public Vector2 MirrorVector => this._mirrorOOBB.P1 - this._mirrorOOBB.P0;

    public override BoundingSquare QuadtreeCollisionBB => this._mirrorBB;

    public Vector2 ReflectorCenter { get; set; }

    public float MirrorAbsoluteRotation { get; set; }

    public float MirrorRotation
    {
      get => this._mirrorRotation;
      set
      {
        this._mirrorRotation = value;
        if ((double) this._mirrorRotation > 180.0)
        {
          this._mirrorRotation = 180f;
        }
        else
        {
          if ((double) this._mirrorRotation >= -180.0)
            return;
          this._mirrorRotation = -180f;
        }
      }
    }

    public LaserBeamMirror()
      : base(StageObjectType.LaserBeamMirror)
    {
      this.MirrorRotation = 90f;
      this._controller = new RotationController((IRotationControllable) this);
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this._controller.LoadContent();
    }

    public override void Initialize()
    {
      base.Initialize();
      this._mirrorSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/laser-beam", "Mirror");
      this.UpdateMirror();
      this._controller.Initialize();
    }

    private void UpdateMirror()
    {
      if ((double) this._mirrorRotation > 160.0)
        this._mirrorRotation = 160f;
      if ((double) this._mirrorRotation < 20.0)
        this._mirrorRotation = 20f;
      this.MirrorAbsoluteRotation = this._mirrorRotation + Mathematics.ConvertAngleTo180(this.Rotation);
      this._mirrorReflectorPosition = Mathematics.TransformVector(this.Sprite.BoundingBoxes[0].Center, this.Rotation, this.Position);
      this._mirrorOOBB = this._mirrorSprite.BoundingBoxes[0].Transform(this.MirrorAbsoluteRotation, this._mirrorReflectorPosition);
      this._mirrorBB = this._mirrorOOBB.ToBoundingSquare();
      this._mirrorReflectorBB = this._mirrorSprite.BoundingBoxes[1].Transform(this.MirrorAbsoluteRotation, this._mirrorReflectorPosition);
      this.ReflectorCenter = this._mirrorReflectorBB.GetCenter();
      this._controller.SetPosition(this.Sprite.BoundingBoxes[1].UpperLeft);
    }

    public bool CollidesWithBeam(LaserBeam beam, out Vector2 p)
    {
      return this._mirrorOOBB.IntersectsLine(beam.BeamOrigin, beam.BeamEndPoint, out p) != OOBoundingBox.CollidingSegment.None;
    }

    public bool BeamCollidesWithReflector(LaserBeam beam, out Vector2 collidingPoint)
    {
      return this._mirrorReflectorBB.IntersectsLine(beam.BeamOrigin, beam.BeamEndPoint, out collidingPoint) == OOBoundingBox.CollidingSegment.P3P0;
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      if (this.QueryInterating())
        Stage.CurrentStage.Cursor.SetInteractingObject((ICursorInteractable) this);
      this._controller.Update(gameTime);
    }

    public override void Draw(bool shadow)
    {
      if (!shadow)
        this._mirrorSprite.Draw(this._mirrorReflectorPosition, 0, this.MirrorRotation + this.Rotation, SpriteEffects.None, Stage.CurrentStage.SpriteBatch);
      else
        this._mirrorSprite.Draw(this._mirrorReflectorPosition + GenericConsts.ShadowDepth, 0, this.MirrorRotation + this.Rotation, SpriteEffects.None, this.ShadowColor, 1f, Stage.CurrentStage.SpriteBatch);
      base.Draw(shadow);
      this._controller.Draw();
    }

    public override void ForegroundDraw()
    {
      base.ForegroundDraw();
      this._controller.DrawForeground();
    }

    public StageCursor.CursorType QueryCursor() => this._controller.QueryCursor();

    public bool QueryInterating() => this._controller.QueryInterating();

    public void CursorActionPressed(Vector2 cursorPos)
    {
      this._controller.CursorActionPressed(cursorPos);
    }

    public void CursorActionReleased() => this._controller.CursorActionReleased();

    public void CursorActionSelected()
    {
    }

    public StageObject ControlledObject => (StageObject) this;

    public bool ControllerValueChanged(float value)
    {
      float mirrorRotation = this.MirrorRotation;
      this.MirrorRotation -= value;
      this.UpdateMirror();
      return (double) mirrorRotation != (double) this.MirrorRotation;
    }

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord(context);
      dataFileRecord.AddField("mirrorRotation", (object) this.MirrorRotation);
      return dataFileRecord;
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this.MirrorRotation = record.GetFieldValue<float>("mirrorRotation", this.MirrorRotation);
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }
  }
}
