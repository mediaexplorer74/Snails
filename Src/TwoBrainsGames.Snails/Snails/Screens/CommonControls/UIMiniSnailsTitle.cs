
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UIMiniSnailsTitle
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UIMiniSnailsTitle : UIControl
  {
    private UIImage _imgTitle;

    public UIMiniSnailsTitle(UIScreen ScreenOwner)
      : base(ScreenOwner)
    {
      this.Position = new Vector2(0.0f, 400f);
      this.ShowEffect = (TransformEffectBase) new SquashEffect(0.7f, 4f, 0.04f);
      this._imgTitle = new UIImage(ScreenOwner, "spriteset/common-elements-1/SnailsLeafSmallTitle", "__STATIC__");
      this._imgTitle.ParentAlignment = AlignModes.HorizontalyVertically;
      this.Controls.Add((UIControl) this._imgTitle);
      this.Size = this._imgTitle.Size;
      this.AcceptControllerInput = false;
    }
  }
}
