
// Type: TwoBrainsGames.Snails.StageObjects.SnailShell
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.Effects;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class SnailShell : MovingObject
  {
    public const string ID = "SNAIL_SHELL";
    private const int HOOVER_UNDWATER = 5;
    private const float ROTATION_LIMIT = 180f;
    private FloatingEffect _floatEffect;
    private RotationEffect _rotationEffect;
    private SnailShell.ShellStatus _status;
    private Sprite _hiddingInShellSprite;
    private Sprite _shellSprite;
    private float _previousPositionY;

    public SnailShell()
      : this(StageObjectType.SnailShell)
    {
    }

    protected SnailShell(StageObjectType type)
      : base(type)
    {
    }

    public SnailShell(StageObject other)
      : base(other)
    {
      this.Copy(other);
    }

    public override void Copy(StageObject other) => base.Copy(other);

    public override void LoadContent()
    {
      base.LoadContent();
      this._shellSprite = this.Sprite;
      this._hiddingInShellSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/anim-snails", "HiddingInShell");
    }

    public void FloatDeath(Liquid inWaterObj, float speed, bool withHideInShellAnimation)
    {
      this._inLiquidRef = inWaterObj;
      this._floatEffect = new FloatingEffect(speed, this.Position, inWaterObj, true, 5);
      this._floatEffect.Amplitude = 1;
      this._floatEffect.Interval = 2;
      this._floatEffect.Active = false;
      this.EffectsBlender.Add((ITransformEffect) this._floatEffect);
      int num = 1;
      if ((double) this.Rotation != 0.0 && (double) this.Rotation - 180.0 > 0.0)
        num = -1;
      this._rotationEffect = new RotationEffect((float) (6 * num));
      this.EffectsBlender.Add((ITransformEffect) this._rotationEffect, 4);
      this._rotationEffect.Active = false;
      if (withHideInShellAnimation)
        this.SetHidingInShellStatus();
      else
        this.SetFloatingShellStatus();
      if (inWaterObj != null)
        return;
      this.KillWithProjection();
    }

    private void SetHidingInShellStatus()
    {
      this._status = SnailShell.ShellStatus.Hidding;
      this.Sprite = this._hiddingInShellSprite;
      this.CurrentFrame = 0;
    }

    private void SetFloatingShellStatus()
    {
      this._status = SnailShell.ShellStatus.Floating;
      this.Sprite = this._shellSprite;
      this.CurrentFrame = 0;
      this._floatEffect.Active = true;
      this._rotationEffect.Active = true;
    }

    public override void OnLastFrame()
    {
      base.OnLastFrame();
      if (this._status != SnailShell.ShellStatus.Hidding)
        return;
      this.SetFloatingShellStatus();
    }

    public override void Update(BrainGameTime gameTime)
    {
      if (this.EffectsBlender.Count > 0 && (double) this._inLiquidRef.LiquidLevel == 0.0)
      {
        this.EffectsBlender.DeleteEffects(4);
        this.EffectsBlender.DeleteEffects(1);
      }
      base.Update(gameTime);
      if (this._status == SnailShell.ShellStatus.Hidding)
        return;
      if (this._status == SnailShell.ShellStatus.Floating && this._floatEffect.Ended || !this.EffectsBlender.Contains(1) && (double) this._inLiquidRef.LiquidLevel != 0.0)
      {
        this._status = SnailShell.ShellStatus.Hoovering;
        this.EffectsBlender.Add((ITransformEffect) new HooverEffect(0.06f, 0.3f, 0.0f), 1);
        this._previousPositionY = this._inLiquidRef.QuadtreeCollisionBB.Top;
      }
      if ((double) this._rotationEffect.Speed < 0.0 && (double) this.Rotation < 180.0 || (double) this._rotationEffect.Speed > 0.0 && (double) this.Rotation > 180.0)
      {
        this._rotationEffect.Ended = true;
        this.Rotation = 180f;
        this.EffectsBlender.DeleteEffects(4);
      }
      if (this._status == SnailShell.ShellStatus.Hoovering && (double) this._previousPositionY != (double) this._inLiquidRef.QuadtreeCollisionBB.Top)
        this.Y += this._inLiquidRef.QuadtreeCollisionBB.Top - this._previousPositionY;
      this._previousPositionY = this._inLiquidRef.QuadtreeCollisionBB.Top;
    }

    private enum ShellStatus
    {
      None,
      Hidding,
      Floating,
      Hoovering,
    }
  }
}
