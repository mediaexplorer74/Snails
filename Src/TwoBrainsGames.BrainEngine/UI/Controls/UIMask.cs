
// Type: TwoBrainsGames.BrainEngine.UI.Controls.UIMask
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.BrainEngine.UI.Controls
{
  public class UIMask : UIControl
  {
    private RenderMask _mask;

    public UIMask.MaskAction Action { get; set; }

    public UIMask(UIScreen screenOwner)
      : this(screenOwner, (string) null)
    {
    }

    public UIMask(UIScreen screenOwner, string spriteName)
      : base(screenOwner)
    {
      this.Action = UIMask.MaskAction.Apply;
      if (string.IsNullOrEmpty(spriteName))
        return;
      this._mask = RenderMask.CreateFromSprite(spriteName);
    }

    public void Inicialize()
    {
    }

    public override void Update(BrainGameTime gameTime)
    {
      if (this._mask == null || this.Action != UIMask.MaskAction.Apply)
        return;
      this._mask.Position = this.AbsolutePositionInPixels;
    }

    public override void Draw()
    {
      if (this.Action == UIMask.MaskAction.Apply)
      {
        this.ScreenOwner.EndDraw();
        this._mask.Render();
        this.ScreenOwner.Mask = this._mask;
        this.ScreenOwner.BeginDraw(BlendState.AlphaBlend);
      }
      else
      {
        this.ScreenOwner.Mask = (RenderMask) null;
        this.ScreenOwner.EndDraw();
        this.ScreenOwner.BeginDraw(BlendState.AlphaBlend);
      }
    }

    public enum MaskAction
    {
      Apply,
      Remove,
    }
  }
}
