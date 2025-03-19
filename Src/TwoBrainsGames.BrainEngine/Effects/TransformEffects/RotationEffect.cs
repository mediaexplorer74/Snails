
// Type: TwoBrainsGames.BrainEngine.Effects.TransformEffects.RotationEffect
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer


namespace TwoBrainsGames.BrainEngine.Effects.TransformEffects
{
  public class RotationEffect : TransformEffectBase, ITransformEffect
  {
    public float Speed;

    public float RotationSum { get; set; }

    public RotationEffect(float speed) => this.Speed = speed;

    public override void Update(BrainGameTime gameTime)
    {
      float num = (float) ((double) this.Speed * (double) gameTime.ElapsedGameTime.Milliseconds / 100.0);
      this.Rotation = num;
      this.RotationSum += num;
    }
  }
}
