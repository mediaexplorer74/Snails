
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UISnailsBoard
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Effects;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UISnailsBoard : UIControl
  {
    private UISnailsBoard.BoardType _type;
    private Sample _hideSample;
    private Sample _showSample;
    private UIImage _imgBackground;

    public UISnailsBoard.BoardType Type
    {
      get => this._type;
      set
      {
        this._type = value;
        switch (this._type)
        {
          case UISnailsBoard.BoardType.LightWoodMedium:
            this._imgBackground.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/boards/LightWoodMedium");
            break;
          case UISnailsBoard.BoardType.LightWoodLongNarrow:
            this._imgBackground.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/boards/LightWoodLongNarrow");
            break;
          case UISnailsBoard.BoardType.LeafsMedium:
            this._imgBackground.Sprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/ingame-elements-1/LeafsMedium");
            break;
          case UISnailsBoard.BoardType.LightWoodMediumLong:
            this._imgBackground.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/boards/LightWoodMediumLong");
            break;
        }
        this.Size = this._imgBackground.Size;
      }
    }

    public UIImage ImgBackground
    {
      get => this._imgBackground;
      set => this._imgBackground = value;
    }

    public UISnailsBoard(UIScreen screenOwner, UISnailsBoard.BoardType type)
      : base(screenOwner)
    {
      this._imgBackground = new UIImage(screenOwner);
      this._imgBackground.ParentAlignment = AlignModes.Horizontaly;
      this.Controls.Add((UIControl) this._imgBackground);
      this.ShowEffect = (TransformEffectBase) new SquashEffect(0.85f, 4f, 0.03f, this.BlendColor, this.Scale);
      this.HideEffect = (TransformEffectBase) new PopOutEffect(new Vector2(1.2f, 1.2f), 6f);
      this.OnHideBegin += new UIControl.UIEvent(this.UISnailsBoard_OnHideBegin);
      this.OnShowBegin += new UIControl.UIEvent(this.UISnailsBoard_OnShowBegin);
      this.OnInitializeFromContent += new UIControl.UIEvent(this.UISnailsBoard_OnInitializeFromContent);
      this.Type = type;
      this._hideSample = BrainGame.ResourceManager.GetSampleStatic("sfx/menu-closed");
      this._showSample = BrainGame.ResourceManager.GetSampleStatic("sfx/menu-item-shown");
    }

    private void UISnailsBoard_OnInitializeFromContent(IUIControl sender)
    {
      this._imgBackground.Scale = this.GetContentPropertyValue<Vector2>("imgBackgroundScale", this._imgBackground.Scale);
    }

    private void UISnailsBoard_OnShowBegin(IUIControl sender) => this._showSample.Play();

    private void UISnailsBoard_OnHideBegin(IUIControl sender) => this._hideSample.Play();

    public enum BoardType
    {
      LightWoodMedium,
      LightWoodLongNarrow,
      LeafsMedium,
      LightWoodMediumLong,
    }
  }
}
