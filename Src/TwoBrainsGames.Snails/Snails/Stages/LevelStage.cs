
// Type: TwoBrainsGames.Snails.Stages.LevelStage
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using System;


namespace TwoBrainsGames.Snails.Stages
{
  public class LevelStage
  {
    public string StageId;
    public string StageKey;
    public ThemeType ThemeId;
    public int StageNr;
    public int _snailsToSave;
    public int _snailsToRelease;
    public TimeSpan _targetTime;
    public TimeSpan _goldMedalTime;
    public int _goldMedalScore;
    public GoalType _goal;

    public string Theme => this.ThemeId.ToString();

    public bool AvailableInDemo { get; set; }

    public bool IsCustomStage { get; set; }

    public string CustomStageFilename { get; set; }

    public override string ToString()
    {
      return string.Format("Theme:{0},Id:{1}", (object) this.Theme, (object) this.StageId);
    }

    public static LevelStage CreateForCustomStage(ThemeType theme, string id)
    {
      return new LevelStage()
      {
        StageId = id,
        IsCustomStage = true,
        StageKey = (string) null,
        ThemeId = theme,
        AvailableInDemo = true
      };
    }
  }
}
