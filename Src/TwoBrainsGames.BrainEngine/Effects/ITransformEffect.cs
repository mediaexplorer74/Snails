
// Type: TwoBrainsGames.BrainEngine.Effects.ITransformEffect
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;


namespace TwoBrainsGames.BrainEngine.Effects
{
  public interface ITransformEffect
  {
    TransformEffectBase.OnEndEvent OnEnd { get; set; }

    bool Active { get; set; }

    Vector2 PositionV2 { get; }

    Vector3 Position { get; }

    Vector2 VirtualPositionV2 { get; }

    Vector3 VirtualPosition { get; }

    Vector4 ColorVector { get; }

    Color Color { get; }

    Vector2 Scale { get; }

    Vector2 LastScale { get; set; }

    bool AutoDeleteOnEnd { get; set; }

    float Rotation { get; }

    float VirtualRotation { get; }

    bool Ended { get; set; }

    void Update(BrainGameTime gameTime);

    void InternalUpdate(BrainGameTime gameTime);

    void Reset();
  }
}
