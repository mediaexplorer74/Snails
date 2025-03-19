
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UICaption
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Configuration;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  public class UICaption : UITextFontLabel
  {
    private TextFont NotebookFont;
    private TextFont NotebookFontMedium;
    private TextFont MainFontMedium;
    private TextFont MainFontBig;
    private TextFont MainFontSmall;
    private TextFont MainFontMediumSmall;
    private UICaption.CaptionStyle _style;

    private UICaption.CaptionStyle Style
    {
      get => this._style;
      set
      {
        this.Scale = new Vector2(1f, 1f);
        this._style = value;
        this.DropShadow = false;
        switch (this._style)
        {
          case UICaption.CaptionStyle.AutoSaveMessage:
          case UICaption.CaptionStyle.OverscanMessage:
            this.Font = this.MainFontMedium;
            break;
          case UICaption.CaptionStyle.NormalText:
          case UICaption.CaptionStyle.CreditsCategory:
          case UICaption.CaptionStyle.CreditsName:
          case UICaption.CaptionStyle.NormalTextMedium:
          case UICaption.CaptionStyle.ThemeUnlockInfoTitle:
            this.Font = this.MainFontMedium;
            break;
          case UICaption.CaptionStyle.NormalTextSmall:
          case UICaption.CaptionStyle.ControllerHelp:
          case UICaption.CaptionStyle.GoldMedalInfo:
          case UICaption.CaptionStyle.StageId:
            this.Font = this.MainFontSmall;
            break;
          case UICaption.CaptionStyle.Heading1:
          case UICaption.CaptionStyle.StageSelectionStageNr:
          case UICaption.CaptionStyle.IntroPressAnyKey:
            this.Font = this.MainFontBig;
            break;
          case UICaption.CaptionStyle.ThemeUnlockInfo:
            this.Font = this.MainFontMediumSmall;
            break;
          case UICaption.CaptionStyle.Notebook:
            this.Font = this.NotebookFont;
            break;
          case UICaption.CaptionStyle.StageStartBoardCaptions:
          case UICaption.CaptionStyle.StageCompletedCaptions:
          case UICaption.CaptionStyle.MissionFailedCaptions:
          case UICaption.CaptionStyle.ThemeStats:
            this.Font = this.MainFontMedium;
            break;
          case UICaption.CaptionStyle.StageCompletedTitle:
            this.Font = this.MainFontMedium;
            break;
          case UICaption.CaptionStyle.StageInfoHeader:
            this.Font = this.NotebookFontMedium;
            break;
          case UICaption.CaptionStyle.StageInfoDetail:
            this.Font = this.NotebookFont;
            break;
          case UICaption.CaptionStyle.AwardCaption:
          case UICaption.CaptionStyle.PlayerStats:
            if (Game1.GameSettings.PresentationMode == GameSettings.PresentationType.LD)
            {
              this.Font = this.MainFontSmall;
              break;
            }
            this.Font = this.MainFontMedium;
            break;
          default:
            this.Font = this.MainFontMedium;
            break;
        }
        this.CalculateSize();
      }
    }

    public UICaption(UIScreen screenOwner, string text, Color color, UICaption.CaptionStyle style)
      : base(screenOwner)
    {
      this.Text = text;
      this.NotebookFont = BrainGame.ResourceManager.Load<TextFont>("fonts/notebook", ResourceManager.ResourceManagerCacheType.Static);
      this.NotebookFontMedium = BrainGame.ResourceManager.Load<TextFont>("fonts/notebook-medium", ResourceManager.ResourceManagerCacheType.Static);
      this.MainFontBig = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-big", ResourceManager.ResourceManagerCacheType.Static);
      this.MainFontMedium = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-medium", ResourceManager.ResourceManagerCacheType.Static);
      this.MainFontSmall = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-small", ResourceManager.ResourceManagerCacheType.Static);
      this.MainFontMediumSmall = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-medium-2", ResourceManager.ResourceManagerCacheType.Static);
      this.BlendColor = color;
      this.Style = style;
    }

    public Vector2 MeasureString()
    {
      return new Vector2(this.Font.MeasureString(this.Text, this.Scale), this.Font.MeasureStringHeight(this.Text, this.Scale));
    }

    public enum CaptionStyle
    {
      AutoSaveMessage,
      OverscanMessage,
      NormalText,
      NormalTextSmall,
      Heading1,
      CreditsCategory,
      CreditsName,
      StageSelectionStageNr,
      NormalTextMedium,
      ThemeUnlockInfo,
      ThemeUnlockInfoTitle,
      Notebook,
      StageStartBoardCaptions,
      StageCompletedTitle,
      StageCompletedCaptions,
      MissionFailedCaptions,
      ThemeStats,
      StageInfoHeader,
      StageInfoDetail,
      IntroPressAnyKey,
      ControllerHelp,
      GoldMedalInfo,
      StageId,
      AwardCaption,
      PlayerStats,
    }
  }
}
