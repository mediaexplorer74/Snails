
// Type: TwoBrainsGames.Snails.Colors
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;


namespace TwoBrainsGames.Snails
{
  public class Colors
  {
    public static Color HudItem_MsgStatusCompleted = new Color(50, 240, 30);
    public static Color HudItem_MsgStatusFailed = new Color((int) byte.MaxValue, 80, 0);
    public static Color HudItem_GoalDescription = new Color(50, 240, 30);
    public static Color ButtonsText = new Color((int) byte.MaxValue, 190, 70);
    public static Color ButtonsFocusText = new Color((int) byte.MaxValue, (int) byte.MaxValue, 0);
    public static Color ThemeSelectionNeeded = new Color(103, 153, (int) byte.MaxValue);
    public static Color ThemeSelectionNeededFromThemes = new Color(52, 245, 32);
    public static Color ThemeStageStats = Color.White;
    public static Color StageSelectionNumber = new Color((int) byte.MaxValue, 162, 0);
    public static Color StageSelectionNotebookText = new Color((int) byte.MaxValue, 80, 0);
    public static Color StageSelectionNotebookHighScoreText = new Color(0, 80, (int) byte.MaxValue);
    public static Color StageSelectionNotebookStageMode = new Color(0, 0, 0);
    public static Color[] ThemeCaptions = new Color[4]
    {
      new Color(160, 235, 80),
      new Color(235, 222, 80),
      new Color(240, 40, 50),
      new Color(160, 160, 160)
    };
    public static Color[] ThemeValues = new Color[4]
    {
      new Color(234, 234, 234),
      new Color(234, 234, 234),
      new Color(234, 234, 234),
      new Color(234, 234, 234)
    };
    public static Color MenuItem = new Color(220, 175, (int) byte.MaxValue);
    public static Color MenuItemSelected = new Color(78, (int) byte.MaxValue, 85);
    public static Color AutoSaveWarningText = new Color((int) byte.MaxValue, 155, 30);
    public static Color OverscanMessageText = new Color((int) byte.MaxValue, 155, 30);
    public static Color StageCompletedText = Color.Yellow;
    public static Color StageCompletedCaptions = new Color(150, 180, 250);
    public static Color StageCompletedBonusCaptions = new Color(240, 160, (int) byte.MaxValue);
    public static Color MissionFailedCaptions = new Color((int) byte.MaxValue, 150, 0);
    public static Color InstructionBarBackground = new Color(0, 0, 0, 150);
    public static Color TutorialTopicBackground = new Color(0, 50, 0, (int) byte.MaxValue);
    public static Color TutorialTextColor = Color.Black;
    public static Color TutorialEnphasizeTextColor = new Color((int) byte.MaxValue, 0, 0);
    public static Color IncomingMessageInfo = new Color(50, 240, 30);
    public static Color IncomingMessageError = new Color((int) byte.MaxValue, 80, 0);
    public static Color StageHUDInfoColor = new Color(230, 250, 30);
    public static Color StageHUDTimerLowColor = new Color((int) byte.MaxValue, 0, 0);
    public static Color IngameShadows = new Color(0, 0, 0, 180);
    public static Color MainMenuScrBkColor = new Color(60, 185, 230);
    public static Color OptionsScrBkColor = new Color(60, 185, 230);
    public static Color AutoSaveScrBkColor = new Color(110, 230, 42);
    public static Color OverscanScrBkColor1 = Colors.MainMenuScrBkColor;
    public static Color OverscanScrBkColor2 = new Color(110, 230, 42);
    public static Color ThemeSelectionScrBkColor = new Color(195, 110, 250);
    public static Color CreditsScrBkColor = new Color(240, 40, 40);
    public static Color AwardsScrBkColor = new Color(60, 185, 230);
    public static Color BBBrainsLogoScreen = new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue);
    public static Color BBOverscanScreen = new Color(200, 0, 0);
    public static Color BBDefaultColor = new Color(0, 0, 0);
    public static Color ControllerHelp = new Color(200, (int) byte.MaxValue, 112);
    public static Color ScrollablePanel = new Color(0, 0, 0, 50);
  }
}
