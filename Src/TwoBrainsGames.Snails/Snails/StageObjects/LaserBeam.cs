
// Type: TwoBrainsGames.Snails.StageObjects.LaserBeam
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class LaserBeam : MovingObject
  {
    private const int BEAM_THICKNESS = 5;
    public const string LASER_BEAM_ID = "LASER_BEAM";
    private OOBoundingBox _beamBs;
    private Rectangle _beamBs2;
    private BoundingSquare _quadtreeBeamBs;
    private Vector2 _beamOrigin;
    private Vector2 _beamEndPoint;
    private float _beamLength;
    private LaserBeam _nextBeam;
    private LaserBeamMirror _reflectingMirror;
    private LaserCannonBase _beamSource;
    private Sprite _laserSprite;
    private float _beamRotation;
    private ColorEffect _beamColorEffect;

    public Vector2 BeamOrigin => this._beamOrigin;

    public Vector2 BeamEndPoint => this._beamEndPoint;

    public LaserBeamMirror ReflectingMirror => this._reflectingMirror;

    public LaserBeam NextBeam => this._nextBeam;

    public LaserBeam.LaserBeamColor LaserColor { get; set; }

    public float BeamRotation
    {
      get => this._beamRotation;
      set
      {
        this._beamRotation = value;
        if ((double) this._beamRotation > 180.0)
        {
          this._beamRotation -= 360f;
        }
        else
        {
          if ((double) this._beamRotation >= -180.0)
            return;
          this._beamRotation += 360f;
        }
      }
    }

    public override BoundingSquare QuadtreeCollisionBB => this._quadtreeBeamBs;

    public bool Visible
    {
      get => this.IsVisible;
      set
      {
        if (value != this.Visible)
          this.VisibilityChanged = true;
        if (this.IsVisible == value)
          return;
        if (value)
        {
          this.DynamicFlags |= StageObjectDynamicFlags.IsVisible;
        }
        else
        {
          this.DynamicFlags &= ~StageObjectDynamicFlags.IsVisible;
          if (this._nextBeam == null)
            return;
          this._nextBeam.Visible = false;
        }
      }
    }

    private bool VisibilityChanged { get; set; }

    public LaserBeam()
      : base(StageObjectType.LaserBeam)
    {
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this._laserSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/laser-beam", "Laser");
    }

    public override void Initialize()
    {
      base.Initialize();
      this._beamEndPoint = this._beamOrigin = Vector2.Zero;
      this._beamColorEffect = new ColorEffect(Color.White, LaserBeam.GetXnaColor(this.LaserColor), 0.1f, true);
      this._beamColorEffect.Active = false;
      this.ComputeBeam();
    }

    public static LaserBeam Create(LaserCannonBase laserSource)
    {
      LaserBeam objectNoInitialize = (LaserBeam) Stage.CurrentStage.StageData.GetObjectNoInitialize("LASER_BEAM");
      objectNoInitialize._beamSource = laserSource;
      objectNoInitialize.LaserColor = laserSource.LaserColor;
      objectNoInitialize.LoadContent();
      objectNoInitialize.Initialize();
      return objectNoInitialize;
    }

    public void Activate() => Stage.CurrentStage.AddObjectInRuntime((StageObject) this);

    public override void Hide()
    {
      base.Hide();
      if (this._nextBeam == null)
        return;
      this._nextBeam.Hide();
    }

    private void ComputeBeam()
    {
      if (this._nextBeam != null)
        this._nextBeam.Hide();
      Vector2 position = this.Position;
      Vector2 v = new Vector2(0.0f, -999999f);
      Vector2 lineP1 = position + Mathematics.RotateVector(v, this.BeamRotation);
      Vector2 nearestColliddingPoint;
      if (Stage.CurrentStage.Board.BoundingBox.IntersectsLine(position, lineP1, out nearestColliddingPoint))
        lineP1 = nearestColliddingPoint;
      this._beamEndPoint = lineP1;
      this._beamOrigin = position;
      this._beamLength = (this._beamEndPoint - this._beamOrigin).Length();
    }

    private void ComputeBeamBB()
    {
      BoundingSquare bs = new BoundingSquare(Vector2.Zero, 5f, -this._beamLength);
      this._beamBs2 = new Rectangle((int) this.Position.X, (int) this.Position.Y, 5, (int) this._beamLength);
      this._beamBs = this.TransformBoundingBox(bs);
      this._quadtreeBeamBs = this.TransformBoundingBox(bs).ToBoundingSquare();
      this.RepositionObjectInQuadtree();
    }

    public override void OnCollide(IQuadtreeContainable obj, int listIdx)
    {
      switch (listIdx)
      {
        case 0:
          Snail snail = (Snail) obj;
          if (!snail.CheckCollisionWithLaserBeam(this))
            break;
          snail.KillByLaser();
          break;
        case 1:
          switch (obj)
          {
            case LaserBeamMirror _:
              this.CollidedWithMirror((LaserBeamMirror) obj);
              return;
            case LaserBeamSwitch _:
              this.CollidedWithSwitch((LaserBeamSwitch) obj);
              return;
            default:
              return;
          }
        case 2:
          BoardPathNode boardPathNode = (BoardPathNode) obj;
          Vector2 i;
          if (!Mathematics.LineLineIntersection(this._beamOrigin, this._beamEndPoint, boardPathNode.Value.P0, boardPathNode.Value.P1, out i))
            break;
          this.CropBeam(i);
          break;
      }
    }

    private void CropBeam(Vector2 cropPoint)
    {
      this._beamEndPoint = cropPoint;
      this._beamLength = (this._beamEndPoint - this._beamOrigin).Length();
    }

    private void CollidedWithMirror(LaserBeamMirror mirror)
    {
      Vector2 p;
      if (!mirror.CollidesWithBeam(this, out p) || this._reflectingMirror == mirror)
        return;
      Vector2 collidingPoint;
      if (!mirror.BeamCollidesWithReflector(this, out collidingPoint))
      {
        this.CropBeam(p);
      }
      else
      {
        this.CropBeam(collidingPoint);
        if (this._beamSource.CountBeamMirrorCollisions(mirror) > 0)
          return;
        float num = (double) mirror.MirrorAbsoluteRotation < 0.0 ? mirror.MirrorAbsoluteRotation + 180f : mirror.MirrorAbsoluteRotation;
        if ((double) Math.Abs(num - this.BeamRotation) <= 10.0)
          return;
        if (this._nextBeam == null)
        {
          this._nextBeam = LaserBeam.Create(this._beamSource);
          this._nextBeam.Activate();
        }
        this._nextBeam.BeamRotation = (float) (-(double) this.BeamRotation + (double) num * 2.0);
        this._nextBeam._reflectingMirror = mirror;
        this._nextBeam.Visible = true;
        this._nextBeam.Position = collidingPoint;
      }
    }

    private void CollidedWithSwitch(LaserBeamSwitch laserBeamSwitch)
    {
      Vector2 collidingPoint;
      if (!laserBeamSwitch.OnLaserBeamCollided(this, out collidingPoint))
        return;
      this.CropBeam(collidingPoint);
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      if (!this.IsVisible)
        return;
      this.Rotation = this.BeamRotation;
      this.ComputeBeam();
      this.ComputeBeamBB();
      this.DoQuadtreeCollisions(2);
      this.DoQuadtreeCollisions(1);
      this.DoQuadtreeCollisions(0);
      this.ComputeBeamBB();
      if (!this.IsVisible)
        return;
      this._beamColorEffect.Update(gameTime);
    }

    public override void Draw(bool shadow)
    {
      if (!this.IsVisible)
        return;
      this._laserSprite.Draw(this.Position, 0, this._beamBs2, this._beamColorEffect.Color, this.BeamRotation + 180f, Stage.CurrentStage.SpriteBatch);
    }

    public static Color GetXnaColor(LaserBeam.LaserBeamColor laserColor)
    {
      switch (laserColor)
      {
        case LaserBeam.LaserBeamColor.Cyan:
          return new Color(26, 201, (int) byte.MaxValue);
        case LaserBeam.LaserBeamColor.Magenta:
          return Color.Magenta;
        case LaserBeam.LaserBeamColor.Green:
          return new Color(95, (int) byte.MaxValue, 30);
        default:
          return Color.White;
      }
    }

    public enum LaserBeamColor
    {
      Cyan,
      Magenta,
      Green,
    }
  }
}
