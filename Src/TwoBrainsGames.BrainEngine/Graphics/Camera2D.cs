
// Type: TwoBrainsGames.BrainEngine.Graphics.Camera2D
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects;


namespace TwoBrainsGames.BrainEngine.Graphics
{
  public class Camera2D : ICamera2D
  {
    private TransformBlender _EffectsBlender;

    public Vector2 Position { get; set; }

    private Vector2 PositionOffset { get; set; }

    private Vector2 CenterPinchOffset { get; set; }

    public float Rotation { get; set; }

    public float RotationOffset { get; set; }

    public Vector2 Origin { get; set; }

    public Vector2 Scale { get; set; }

    public Matrix Transform { get; set; }

    public float MoveSpeed { get; set; }

    public Vector2 TransformedPosition { get; set; }

    public TransformBlender EffectsBlender => this._EffectsBlender;

    public virtual void Initialize()
    {
      this.Origin = new Vector2((float) (BrainGame.ScreenWidth / 2), (float) (BrainGame.ScreenHeight / 2));
      this.Scale = Vector2.One;
      this._EffectsBlender = new TransformBlender();
      this.UpdateTransform();
    }

    public void UpdateTransform()
    {
      Vector2 vector2 = new Vector2((float) (int) (-(double) this.Position.X - (double) this.PositionOffset.X), (float) (int) (-(double) this.Position.Y - (double) this.PositionOffset.Y));
      this.Transform = Matrix.CreateTranslation(vector2.X, vector2.Y, 0.0f) * Matrix.CreateRotationZ(this.Rotation + this.RotationOffset) * Matrix.CreateScale(new Vector3(this.Scale.X, this.Scale.Y, 0.0f)) * Matrix.CreateTranslation(this.Origin.X, this.Origin.Y, 0.0f);
      this.TransformedPosition = vector2 + this.Origin;
    }

    public virtual void Update(BrainGameTime gameTime)
    {
      this._EffectsBlender.Update(gameTime);
      this.Position += this._EffectsBlender.PositionV2;
      this.Rotation += this._EffectsBlender.Rotation;
      this.PositionOffset = this._EffectsBlender.VirtualPositionV2;
      this.RotationOffset = this._EffectsBlender.VirtualRotation;
      this.Scale += this._EffectsBlender._scale;
      this.UpdateTransform();
    }

    public virtual void Zoom(float scale)
    {
      this.Scale = new Vector2(this.Scale.X * scale, this.Scale.Y * scale);
    }
  }
}
