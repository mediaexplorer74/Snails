
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UISnailStamp
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UISnailStamp : UIControl
  {
    protected UIImage _imgImage;

    public bool IgnoreShowEffect { get; set; }

    public Sample ShowSample { get; set; }

    public UISnailStamp(UIScreen screenOwner)
      : this(screenOwner, (string) null, (string) null)
    {
    }

    public UISnailStamp(UIScreen screenOwner, string imageResource, string resourceManagerId)
      : base(screenOwner)
    {
      this._imgImage = new UIImage(screenOwner);
      if (imageResource != null)
        this._imgImage.Sprite = BrainGame.ResourceManager.GetSprite(imageResource, resourceManagerId);
      this._imgImage.OnShow += new UIControl.UIEvent(this._imgImage_OnShow);
      this._imgImage.ShowEffect = (TransformEffectBase) new ScaleEffect(new Vector2(10f, 10f), 30f, new Vector2(1f, 1f), false);
      this._imgImage.BlendColorWithParent = true;
      this.Controls.Add((UIControl) this._imgImage);
      this.AcceptControllerInput = false;
      this.ShowSample = BrainGame.ResourceManager.GetSampleTemporary("sfx/fail-stamp");
      this.Reset();
    }

    public override void Show()
    {
      if (!this.IgnoreShowEffect)
      {
        base.Show();
        if (this.ShowSample == null)
          return;
        this.ShowSample.Play();
      }
      else
        this.Visible = true;
    }

    private void _imgImage_OnShow(IUIControl sender)
    {
      this.Effect = (ITransformEffect) new SquashEffect(0.8f, 4f, 0.04f, this.BlendColor, this.Scale);
      this.Effect.OnEnd = new TransformEffectBase.OnEndEvent(this.EffectEnded);
    }

    private void EffectEnded(object param) => this.InvokeOnShow();

    public void Reset() => this.Scale = new Vector2(1f, 1f);
  }
}
