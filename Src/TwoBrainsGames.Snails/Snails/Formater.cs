
// Type: TwoBrainsGames.Snails.Formater
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using System;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails
{
  internal class Formater
  {
    public static string FormatModeName(GoalType goal)
    {
      switch (goal)
      {
        case GoalType.SnailDelivery:
          return LanguageManager.GetString("GAME_MODE_ESCORT");
        case GoalType.SnailKiller:
          return LanguageManager.GetString("GAME_MODE_KILLER");
        case GoalType.SnailKing:
          return LanguageManager.GetString("GAME_MODE_KING");
        case GoalType.TimeAttack:
          return LanguageManager.GetString("GAME_MODE_TIME");
        default:
          throw new SnailsException("Invalid goal type [" + goal.ToString() + "]");
      }
    }

    public static string FormatGoalDescription(LevelStage levelStage)
    {
      return Formater.FormatGoalDescription(levelStage._goal, levelStage._snailsToSave, levelStage._targetTime, false);
    }

    public static string FormatGoalDescription(LevelStage levelStage, bool formatForHud)
    {
      return Formater.FormatGoalDescription(levelStage._goal, levelStage._snailsToSave, levelStage._targetTime, formatForHud);
    }

    public static string FormatGoalDescription(
      GoalType goal,
      int snailsToDeliver,
      TimeSpan targetTime,
      bool formatForHud)
    {
      string str = "";
      switch (goal)
      {
        case GoalType.SnailDelivery:
          str = snailsToDeliver != 1 ? string.Format(LanguageManager.GetString("GAME_MODE_ESCORT_GOAL"), (object) snailsToDeliver) : string.Format(LanguageManager.GetString("GAME_MODE_ESCORT_GOAL_SINGLE"));
          break;
        case GoalType.SnailKiller:
          str = LanguageManager.GetString("GAME_MODE_KILLER_GOAL");
          break;
        case GoalType.SnailKing:
          str = LanguageManager.GetString("GAME_MODE_KING_GOAL");
          break;
        case GoalType.TimeAttack:
          str = formatForHud ? string.Format(LanguageManager.GetString("GAME_MODE_TIME_GOAL_HUD"), (object) snailsToDeliver, (object) targetTime.Minutes, (object) targetTime.Seconds) : string.Format(LanguageManager.GetString("GAME_MODE_TIME_GOAL"), (object) snailsToDeliver, (object) targetTime.Minutes, (object) targetTime.Seconds);
          break;
      }
      return str;
    }

    public static string GetThemeName(ThemeType theme)
    {
      switch (theme)
      {
        case ThemeType.ThemeA:
          return LanguageManager.GetString("THEME_A_NAME");
        case ThemeType.ThemeB:
          return LanguageManager.GetString("THEME_B_NAME");
        case ThemeType.ThemeC:
          return LanguageManager.GetString("THEME_C_NAME");
        case ThemeType.ThemeD:
          return LanguageManager.GetString("THEME_D_NAME");
        default:
          return "";
      }
    }

    public static string FormatLevelTime(TimeSpan ts)
    {
      return string.Format("{0:00}:{1:00}", (object) (ts.Hours * 60 + ts.Minutes), (object) ts.Seconds);
    }

    public static string FormatLevelScore(int score) => string.Format("{0} pts", (object) score);
  }
}
