
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UISnailsMedal
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UISnailsMedal : UISnailStamp
  {
    private MedalType _face;
    private UISnailsMedal.MedalSizeType _medalSize;

    public UISnailsMedal.MedalSizeType MedalSize
    {
      get => this._medalSize;
      set
      {
        this._medalSize = value;
        this.UpdateSprite();
      }
    }

    public MedalType Face
    {
      get => this._face;
      set
      {
        this._face = value;
        this.UpdateSprite();
      }
    }

    public UISnailsMedal(UIScreen screenOwner, MedalType face)
      : base(screenOwner)
    {
      this.Face = face;
      this.ShowSample = BrainGame.ResourceManager.GetSampleTemporary("sfx/medal-hitting-board");
    }

    private void UpdateSprite()
    {
      switch (this.Face)
      {
        case MedalType.Bronze:
          switch (this.MedalSize)
          {
            case UISnailsMedal.MedalSizeType.Normal:
              this.SetSprite("BronzeMedal");
              return;
            case UISnailsMedal.MedalSizeType.Small:
              this.SetSprite("BronzeMedalSmall");
              return;
            case UISnailsMedal.MedalSizeType.Tiny:
              this.SetSprite("BronzeMedalTiny");
              return;
            default:
              return;
          }
        case MedalType.Silver:
          switch (this.MedalSize)
          {
            case UISnailsMedal.MedalSizeType.Normal:
              this.SetSprite("SilverMedal");
              return;
            case UISnailsMedal.MedalSizeType.Small:
              this.SetSprite("SilverMedalSmall");
              return;
            case UISnailsMedal.MedalSizeType.Tiny:
              this.SetSprite("SilverMedalTiny");
              return;
            default:
              return;
          }
        case MedalType.Gold:
          switch (this.MedalSize)
          {
            case UISnailsMedal.MedalSizeType.Normal:
              this.SetSprite("GoldMedal");
              return;
            case UISnailsMedal.MedalSizeType.Small:
              this.SetSprite("GoldMedalSmall");
              return;
            case UISnailsMedal.MedalSizeType.Tiny:
              this.SetSprite("GoldMedalTiny");
              return;
            default:
              return;
          }
      }
    }

    private void SetSprite(string spriteName)
    {
      this._imgImage.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/common-elements-1", spriteName);
    }

    public enum MedalSizeType
    {
      Normal,
      Small,
      Tiny,
    }
  }
}
