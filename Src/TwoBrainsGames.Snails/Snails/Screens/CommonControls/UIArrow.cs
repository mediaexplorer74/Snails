
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UIArrow
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UIArrow : UIControl
  {
    public const string SPRITE_PATH = "spriteset/menu-elements-1/";
    public const string SPRITE_UP = "spriteset/menu-elements-1/ArrowUp";
    public const string SPRITE_DOWN = "spriteset/menu-elements-1/ArrowDown";
    public const string SPRITE_LEFT = "spriteset/menu-elements-1/ArrowLeft";
    public const string SPRITE_RIGHT = "spriteset/menu-elements-1/ArrowRight";
    public const string SPRITE_SMALL = "Small";
    protected UIPanel _container;
    protected UIImage _arrow;
    protected UIArrow.ArrowType _type;
    protected UIArrow.ArrowSize _size;

    public UIArrow.ArrowType Orientation
    {
      get => this._type;
      set
      {
        this._type = value;
        string resourceName = string.Empty;
        switch (this._type)
        {
          case UIArrow.ArrowType.Up:
            resourceName = "spriteset/menu-elements-1/ArrowUp";
            break;
          case UIArrow.ArrowType.Down:
            resourceName = "spriteset/menu-elements-1/ArrowDown";
            break;
          case UIArrow.ArrowType.Left:
            resourceName = "spriteset/menu-elements-1/ArrowLeft";
            break;
          case UIArrow.ArrowType.Right:
            resourceName = "spriteset/menu-elements-1/ArrowRight";
            break;
        }
        if (this._size == UIArrow.ArrowSize.Small)
          resourceName += "Small";
        this._arrow.Sprite = BrainGame.ResourceManager.GetSpriteTemporary(resourceName);
        this._container.Size = this._arrow.Size;
        this.Size = this._arrow.Size;
      }
    }

    public override bool Visible
    {
      get => base.Visible;
      set
      {
        base.Visible = value;
        if (this._container != null)
          this._container.Visible = value;
        if (this._arrow == null)
          return;
        this._arrow.Visible = value;
      }
    }

    public UIArrow(UIScreen screenOwner, UIArrow.ArrowType type, UIArrow.ArrowSize size)
      : this(screenOwner, type, size, (HooverEffect) null)
    {
    }

    public UIArrow(
      UIScreen screenOwner,
      UIArrow.ArrowType type,
      UIArrow.ArrowSize size,
      HooverEffect effect)
      : base(screenOwner)
    {
      this.AcceptControllerInput = false;
      this._container = new UIPanel(screenOwner);
      this._container.ParentAlignment = AlignModes.HorizontalyVertically;
      this.Controls.Add((UIControl) this._container);
      this._arrow = new UIImage(screenOwner);
      this._container.Controls.Add((UIControl) this._arrow);
      this._container.Size = this._arrow.Size;
      this.Orientation = type;
    }

    public void DoHoover(Vector2 pos)
    {
      if (this.Orientation == UIArrow.ArrowType.Left || this.Orientation == UIArrow.ArrowType.Up)
        this._arrow.Offset = pos;
      else
        this._arrow.Offset = -pos;
    }

    public override void Draw() => base.Draw();

    public enum ArrowType
    {
      Up,
      Down,
      Left,
      Right,
    }

    public enum ArrowSize
    {
      Small,
      Medium,
    }
  }
}
