
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UISnailsStageGoalIcon
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UISnailsStageGoalIcon : UIImage
  {
    private GoalType _goal;
    private UISnailsStageGoalIcon.GoalIconSize _iconSize;

    public GoalType Goal
    {
      get => this._goal;
      set
      {
        this._goal = value;
        this.CurrentFrame = (int) this._goal;
      }
    }

    public UISnailsStageGoalIcon.GoalIconSize IconSize
    {
      get => this._iconSize;
      set
      {
        this._iconSize = value;
        switch (this._iconSize)
        {
          case UISnailsStageGoalIcon.GoalIconSize.Small:
            this.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/common-elements-1/GoalSmall");
            break;
          case UISnailsStageGoalIcon.GoalIconSize.Big:
            this.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/common-elements-1/Goal");
            break;
        }
      }
    }

    public UISnailsStageGoalIcon(UIScreen screenOwner)
      : base(screenOwner)
    {
      this.Animate = false;
      this.IconSize = UISnailsStageGoalIcon.GoalIconSize.Big;
    }

    public enum GoalIconSize
    {
      Small,
      Big,
    }
  }
}
