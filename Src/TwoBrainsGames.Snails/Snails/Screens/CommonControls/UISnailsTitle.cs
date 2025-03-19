
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UISnailsTitle
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Effects;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UISnailsTitle : UIControl
  {
    private UIImage _imgLeaf;
    private UITimer _shakeTimer;
    private Vector2 _originalScale;
    public UISnailsTitle.TitleMode _mode;

    public UISnailsTitle.TitleMode Mode
    {
      get => this._mode;
      set
      {
        this._mode = value;
        switch (this._mode)
        {
          case UISnailsTitle.TitleMode.Leaf:
            this._imgLeaf.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/common-elements-1/SnailsLeafTitle");
            break;
          case UISnailsTitle.TitleMode.Log:
            this._imgLeaf.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/common-elements-1/SnailsLogTitle");
            break;
        }
      }
    }

    public bool WithShake { get; set; }

    public UISnailsTitle(UIScreen ScreenOwner)
      : base(ScreenOwner)
    {
      this.OnAfterInitializeFromContent += new UIControl.UIEvent(this.UISnailsTitle_OnAfterInitializeFromContent);
      this.ParentAlignment = AlignModes.Horizontaly;
      this.HideEffect = (TransformEffectBase) new PopOutEffect(new Vector2(1.2f, 1.2f), 6f);
      this._imgLeaf = new UIImage(ScreenOwner);
      this._imgLeaf.ParentAlignment = AlignModes.HorizontalyVertically;
      this._imgLeaf.DropShadow = false;
      this.Controls.Add((UIControl) this._imgLeaf);
      this.Mode = UISnailsTitle.TitleMode.Leaf;
      this.Size = this._imgLeaf.Size;
      this.AcceptControllerInput = false;
      this._shakeTimer = new UITimer(ScreenOwner);
      this._shakeTimer.Snooze = true;
      this._shakeTimer.OnTimer += new UIControl.UIEvent(this._shakeTimer_OnTimer);
      this.Controls.Add((UIControl) this._shakeTimer);
      this.OnScreenStart += new UIControl.UIEvent(this.UISnailsTitle_OnScreenStart);
      this.WithShake = true;
    }

    private void _shakeTimer_OnTimer(IUIControl sender)
    {
      this.Effect = (ITransformEffect) new SquashEffect(0.85f * this._originalScale.X, 1.2f * this._originalScale.X, 0.02f * this._originalScale.X, this.BlendColor, this._originalScale);
      this._shakeTimer.Time = (double) (5000 + BrainGame.Rand.Next(5000));
    }

    private void UISnailsTitle_OnScreenStart(IUIControl sender)
    {
      this.Scale = this._originalScale;
      this._shakeTimer.Time = (double) (5000 + BrainGame.Rand.Next(5000));
      this._shakeTimer.Reset();
      this._shakeTimer.Enabled = this.WithShake;
    }

    private void UISnailsTitle_OnAfterInitializeFromContent(IUIControl sender)
    {
      this._originalScale = this.Scale;
      this.ShowEffect = (TransformEffectBase) new SquashEffect(0.8f * this.Scale.X, 4f * this.Scale.X, 0.03f * this.Scale.X, this.BlendColor, this.Scale);
    }

    public override void Update(BrainGameTime gameTime) => base.Update(gameTime);

    public enum TitleMode
    {
      Leaf,
      Log,
    }
  }
}
