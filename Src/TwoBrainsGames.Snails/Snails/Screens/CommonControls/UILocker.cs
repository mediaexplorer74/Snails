
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UILocker
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  public class UILocker : UIImage
  {
    private bool _locked;
    private UILocker.LockerImageType _lockerType;
    private Sprite _openSprite;
    private Sprite _closeSprite;

    public UILocker.LockerImageType LockerType
    {
      get => this._lockerType;
      set
      {
        this._lockerType = value;
        switch (this._lockerType)
        {
          case UILocker.LockerImageType.Normal:
            this._closeSprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/common-elements-1", "Locker");
            break;
          case UILocker.LockerImageType.Demo:
            this._closeSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/menu-elements-1", "LockedInDemo");
            break;
        }
        this.UpdateSprite();
      }
    }

    public bool Locked
    {
      get => this._locked;
      set
      {
        this._locked = value;
        this.UpdateSprite();
      }
    }

    public UILocker(UIScreen screenOwner)
      : base(screenOwner)
    {
      this._openSprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/common-elements-1", "LockerOpen");
      this._closeSprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/common-elements-1", "Locker");
      this.Locked = true;
      this.BlendColorWithParent = false;
      this.Effect = (ITransformEffect) new ScaleEffect(new Vector2(1f, 1f), 0.1f, new Vector2(0.95f, 0.95f), true);
    }

    private void UpdateSprite()
    {
      if (this._locked)
        this.Sprite = this._closeSprite;
      else
        this.Sprite = this._openSprite;
    }

    public enum LockerImageType
    {
      Normal,
      Small,
      Demo,
    }
  }
}
