
// Type: TwoBrainsGames.Snails.StageObjects.WaterBubble
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.Effects;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class WaterBubble : MovingObject
  {
    public const string ID = "WATER_BUBBLE";
    private FloatingEffect _floatEffect;
    private WaterBubble.BubbleSize _size;
    private Sprite _bubbleOutSprite;
    private WaterBubble.BubbleStatus _status;

    public WaterBubble.BubbleSize Size
    {
      get => this._size;
      set
      {
        this._size = value;
        if (value == WaterBubble.BubbleSize.Random)
        {
          float num = (float) (2 + BrainGame.Rand.Next(3)) / 10f;
          this.Scale = new Vector2(num, num);
        }
        else
        {
          WaterBubble waterBubble = this;
          waterBubble.Scale = waterBubble.Scale / (float) this._size;
        }
      }
    }

    public WaterBubble()
      : this(StageObjectType.WaterBubble)
    {
    }

    protected WaterBubble(StageObjectType type)
      : base(type)
    {
    }

    public WaterBubble(StageObject other)
      : base(other)
    {
      this.Copy(other);
    }

    public override void Copy(StageObject other) => base.Copy(other);

    public override void LoadContent()
    {
      base.LoadContent();
      this._bubbleOutSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "BubblePickUp");
    }

    public void FloatToSurface(Water inWaterObj, float speed)
    {
      this._status = WaterBubble.BubbleStatus.Floating;
      this._floatEffect = new FloatingEffect(speed, this.Position, (Liquid) inWaterObj);
      this._floatEffect.Amplitude = 2;
      this._floatEffect.Interval = 4;
      this.EffectsBlender.Add((ITransformEffect) this._floatEffect);
    }

    public override void OnLastFrame()
    {
      base.OnLastFrame();
      if (this._status != WaterBubble.BubbleStatus.BubbleOut)
        return;
      this.FadeOut(5f);
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      if (this._status != WaterBubble.BubbleStatus.Floating || !this._floatEffect.Ended)
        return;
      this._status = WaterBubble.BubbleStatus.BubbleOut;
      this.Sprite = this._bubbleOutSprite;
      this.CurrentFrame = 0;
    }

    public enum BubbleSize
    {
      Large = 1,
      Medium = 2,
      Small = 3,
      Random = 4,
    }

    private enum BubbleStatus
    {
      None,
      Floating,
      BubbleOut,
    }
  }
}
