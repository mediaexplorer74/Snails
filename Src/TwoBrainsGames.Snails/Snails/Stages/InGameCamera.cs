
// Type: TwoBrainsGames.Snails.Stages.InGameCamera
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.Effects;
using TwoBrainsGames.Snails.Stages.HUD;


namespace TwoBrainsGames.Snails.Stages
{
  public class InGameCamera : Camera2D
  {
    public const float MAX_ZOOM_IN = 1f;
    public const float MAX_ZOOM_OUT = 0.5f;
    private Stage _stage;
    private BasicEffect _stageRenderEffect;
    private BasicEffect _backgroundEffect;
    private FlickEffect _flickEffect;
    private StageStartCameraZoomEffect _stageStartEffect;
    private ShakeEffect _shakeEffect;
    private PinchEffect _pinchEffect;
    private Vector2 _pointOfInterest;

    private StageHUD Hud => this._stage.StageHUD;

    private Board Board => this._stage.Board;

    private BoundingSquare StageArea => this._stage.StageHUD._stageArea;

    public BasicEffect StageRenderEffect => this._stageRenderEffect;

    public BasicEffect BackgroundEffect => this._backgroundEffect;

    public float MaxZoomOut { get; private set; }

    public Vector2 UpperLeftScreenCorner => this.Position - this.Origin;

    public InGameCamera(Stage stage) => this._stage = stage;

    public override void Initialize()
    {
      base.Initialize();
      this._stageRenderEffect = new BasicEffect(BrainGame.Graphics);
      this._stageRenderEffect.TextureEnabled = true;
      this._stageRenderEffect.VertexColorEnabled = true;
      this._stageRenderEffect.World = Matrix.Identity;
      this._stageRenderEffect.View = Matrix.Identity;
      this._stageRenderEffect.Projection = BrainGame.RenderEffect.Projection;
      this._backgroundEffect = new BasicEffect(BrainGame.Graphics);
      this._backgroundEffect.TextureEnabled = true;
      this._backgroundEffect.VertexColorEnabled = true;
      this._backgroundEffect.World = Matrix.Identity;
      this._backgroundEffect.View = Matrix.Identity;
      this._backgroundEffect.Projection = BrainGame.RenderEffect.Projection;
      this.MaxZoomOut = Game1.GameSettings.MaxZoomOut;
      this._flickEffect = new FlickEffect(Vector2.Zero);
      this._flickEffect.Active = false;
      this._flickEffect.AutoDeleteOnEnd = false;
      Stage.CurrentStage.Camera.EffectsBlender.Add((ITransformEffect) this._flickEffect);
      this._stageStartEffect = new StageStartCameraZoomEffect(this);
      this._stageStartEffect.OnEnd = new TransformEffectBase.OnEndEvent(this.StageStartEffectEnded);
      this._stageStartEffect.Active = false;
      this._stageStartEffect.AutoDeleteOnEnd = false;
      Stage.CurrentStage.Camera.EffectsBlender.Add((ITransformEffect) this._stageStartEffect);
      this._shakeEffect = new ShakeEffect();
      this._shakeEffect.Active = false;
      this._shakeEffect.AutoDeleteOnEnd = false;
      Stage.CurrentStage.Camera.EffectsBlender.Add((ITransformEffect) this._shakeEffect);
      this._pinchEffect = new PinchEffect(this);
      this._pinchEffect.Active = false;
      this._pinchEffect.AutoDeleteOnEnd = false;
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      this.CheckCameraBounds();
      this._stageRenderEffect.World = this.Transform;
      BrainGame.SampleManager.SetAudioListenerPosition(this.Position);
    }

    public void SetOriginToHudCenter(StageHUD hud)
    {
      this.Origin = hud.HudCenter;
      this.UpdateTransform();
    }

    public void CenterInStage()
    {
      this.Position = new Vector2((float) (Stage.CurrentStage.Board.Width / 2), (float) (Stage.CurrentStage.Board.Height / 2));
      this.UpdateTransform();
    }

    private bool IsMaximumZoomOut()
    {
      return (double) this.Board.WidthInCameraWorld <= (double) this.StageArea.Width || (double) this.Board.HeightInCameraWorld <= (double) this.StageArea.Height;
    }

    private bool IsMaximumZoomIn() => this.Scale == Vector2.One;

    public void FullZoomOut()
    {
      this.Scale = Vector2.One;
      this.Zoom(this.MaxZoomOut);
    }

    public void StageStartupZoomIn(Vector2 pointOfInterest)
    {
      if (this.IsMaximumZoomIn())
        return;
      this._pointOfInterest = pointOfInterest;
      this._stageStartEffect.Reset(this.Scale, pointOfInterest);
      this._stageStartEffect.Active = true;
    }

    public override void Zoom(float scaleFactor)
    {
      if ((double) scaleFactor == 1.0 || (double) scaleFactor < 1.0 && this.IsMaximumZoomOut() || (double) scaleFactor > 1.0 && this.IsMaximumZoomIn())
        return;
      if ((double) scaleFactor * (double) this.Scale.X > 1.0)
        scaleFactor = 1f / this.Scale.X;
      if ((double) scaleFactor * (double) this.Scale.X < 0.5)
        scaleFactor = 0.5f / this.Scale.X;
      base.Zoom(scaleFactor);
      this.CheckCameraBounds();
    }

    public void MoveTo(Vector2 position)
    {
      this.Position = position;
      this.CheckCameraBounds();
    }

    public void MoveToOrigin()
    {
      this.Position = new Vector2(0.0f, 0.0f);
      this.CheckCameraBounds();
    }

        public void MoveByOffset(Vector2 offset)
        {
            this.Position += offset;
            this.Position = new Vector2((float)(int)this.Position.X, (float)(int)this.Position.Y);
            this.CheckCameraBounds();
        }

    private void CheckLeftCameraBound()
    {
      if ((double) this.Origin.X - (double) this.Position.X * (double) this.Scale.X <= (double) this.StageArea.UpperLeft.X)
        return;
      this.Position = new Vector2(this.StageArea.UpperLeft.X + this.Origin.X / this.Scale.X, this.Position.Y);
      this.EndFlickX();
      this.UpdateTransform();
    }

    private void CheckTopCameraBound()
    {
      if ((double) this.Origin.Y - (double) this.Position.Y * (double) this.Scale.Y <= (double) this.StageArea.UpperLeft.Y)
        return;
      this.Position = new Vector2(this.Position.X, this.StageArea.UpperLeft.Y + this.Origin.Y / this.Scale.Y);
      this.EndFlickY();
      this.UpdateTransform();
    }

    private void CheckCameraBounds()
    {
      if ((double) this.Board.WidthInCameraWorld < (double) this.StageArea.Width)
      {
        this.Scale = new Vector2(this.StageArea.Width / (float) this.Board.Width, this.Scale.Y);
        this.RemovePitchEffect();
      }
      if ((double) this.Board.HeightInCameraWorld < (double) this.StageArea.Height)
      {
        this.Scale = new Vector2(this.Scale.X, this.StageArea.Height / (float) this.Board.Height);
        this.RemovePitchEffect();
      }
      if ((double) this.Scale.X != (double) this.Scale.Y)
      {
        if ((double) this.Scale.X < (double) this.Scale.Y)
          this.Scale = new Vector2(this.Scale.Y, this.Scale.Y);
        if ((double) this.Scale.Y < (double) this.Scale.X)
          this.Scale = new Vector2(this.Scale.X, this.Scale.X);
      }
      if ((double) this.Scale.X > 1.0 || (double) this.Scale.Y > 1.0)
      {
        this.Scale = new Vector2(1f, 1f);
        this.RemovePitchEffect();
      }
      if ((double) this.Scale.X < 0.5 || (double) this.Scale.Y < 0.5)
      {
        this.Scale = new Vector2(0.5f, 0.5f);
        this.RemovePitchEffect();
      }
      this.CheckLeftCameraBound();
      if ((double) this.Position.X * (double) this.Scale.X - (double) this.Origin.X + (double) this.StageArea.UpperRight.X > (double) this.Board.Width * (double) this.Scale.X)
      {
        this.Position = new Vector2((float) ((double) this.Origin.X - (double) this.StageArea.UpperRight.X + (double) this.Board.Width * (double) this.Scale.X) / this.Scale.X, this.Position.Y);
        this.CheckLeftCameraBound();
        this.EndFlickX();
        this.UpdateTransform();
      }
      this.CheckTopCameraBound();
      if ((double) this.Position.Y * (double) this.Scale.Y - (double) this.Origin.Y + (double) this.StageArea.LowerRight.Y <= (double) this.Board.Height * (double) this.Scale.Y)
        return;
      this.Position = new Vector2(this.Position.X, (float) ((double) this.Origin.Y - (double) this.StageArea.LowerRight.Y + (double) this.Board.Height * (double) this.Scale.Y) / this.Scale.Y);
      this.CheckTopCameraBound();
      this.EndFlickY();
      this.UpdateTransform();
    }

    public void DoFlick(Vector2 speedVector)
    {
      speedVector *= 500f * this.Scale;
      this._flickEffect.Reset(speedVector);
      this._flickEffect.Active = true;
    }

    public void EndFlick() => this._flickEffect.Active = false;

    public void EndFlickX()
    {
      this._flickEffect.Speed = new Vector2(0.0f, this._flickEffect.Speed.Y);
      if (!(this._flickEffect.Speed == Vector2.Zero))
        return;
      this.EndFlick();
    }

    public void EndFlickY()
    {
      this._flickEffect.Speed = new Vector2(this._flickEffect.Speed.X, 0.0f);
      if (!(this._flickEffect.Speed == Vector2.Zero))
        return;
      this.EndFlick();
    }

    private void StageStartEffectEnded(object param) => this.Position = this._pointOfInterest;

    private void RemoveStageStartEffect() => this._stageStartEffect.Active = false;

    public void Shake(int shakeTime, int shakeStrength)
    {
      this._shakeEffect.Reset(shakeTime, shakeStrength);
      this._shakeEffect.Active = true;
    }

    public void EndPinch(float previousScale, double pinchTime)
    {
    }

    private void RemovePitchEffect() => this._pinchEffect.Active = false;
  }
}
