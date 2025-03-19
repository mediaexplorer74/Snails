
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UIAchievement
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Configuration;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UIAchievement : UIPanel
  {
    private const int MAX_CHARS_PER_LINE = 38;
    private BrainAchievement _achievement;
    private UIImage _image;
    private UICaption _capDescriptionLine1;
    private UICaption _capDescriptionLine2;
    private UICaption _capCompletion;

    private Color CaptionColorWon { get; set; }

    private Color CaptionColorNotWon { get; set; }

    public float CompletionPercentage { get; set; }

    public bool AllowToggle => false;

    public BrainAchievement Achievement
    {
      get => this._achievement;
      set
      {
        this._achievement = value;
        this.Refresh();
      }
    }

    public UIAchievement(UIScreen screenOwner, BrainAchievement achievement)
      : base(screenOwner)
    {
      this.CaptionColorWon = Color.LightBlue;
      this.CaptionColorNotWon = Color.Gray;
      this._image = new UIImage(screenOwner);
      this._image.Position = new Vector2(50f, 0.0f);
      if (Game1.GameSettings.PresentationMode == GameSettings.PresentationType.LD)
        this._image.Scale = new Vector2(0.85f, 0.85f);
      this.Controls.Add((UIControl) this._image);
      this._capDescriptionLine1 = new UICaption(screenOwner, "", Color.White, UICaption.CaptionStyle.AwardCaption);
      this._capDescriptionLine1.Position = new Vector2(650f, 200f);
      this.Controls.Add((UIControl) this._capDescriptionLine1);
      this._capDescriptionLine2 = new UICaption(screenOwner, "", Color.White, UICaption.CaptionStyle.AwardCaption);
      this._capDescriptionLine2.Position = new Vector2(650f, 500f);
      this.Controls.Add((UIControl) this._capDescriptionLine2);
      this._capCompletion = new UICaption(screenOwner, "", Color.White, UICaption.CaptionStyle.AwardCaption);
      this._capCompletion.ParentAlignment = AlignModes.Vertically | AlignModes.Right;
      this.Controls.Add((UIControl) this._capCompletion);
      this.Achievement = achievement;
      this.UpdateSize();
      this.BackgroundColor = new Color(0, 0, 0, 50);
    }

    private void UpdateSize()
    {
      if (!this.AllowToggle)
      {
        this.Size = new Size(6400f, 900f);
        this._capCompletion.Margins.Right = 0.0f;
      }
      else
        this.Size = new Size(6400f, 1100f);
    }

    public float GetCompletionPercentage(BrainAchievement achievement)
    {
      if (Game1.ProfilesManager.CurrentProfile.IsAchievementEarned(achievement.EventType))
        return 100f;
      int quantity = achievement.Quantity;
      if (quantity <= 0)
        return 0.0f;
      int num = BrainGame.AchievementsManager.Verify(achievement.EventType);
      return num == -1 ? 0.0f : (float) ((double) num / (double) quantity * 100.0);
    }

    public void Refresh()
    {
      if (this.Achievement == null)
        return;
      this._image.Sprite = this.Achievement.Trophy;
      string str1 = this.Achievement.Description;
      string str2 = (string) null;
      if (Game1.GameSettings.PresentationMode == GameSettings.PresentationType.LD && this.Achievement.Description.Length > 38)
      {
        string str3 = str1.Substring(0, 38);
        int num = str3.Length - 1;
        while (num >= 0 && str3[num] != ' ')
          --num;
        str1 = str3.Substring(0, num).Trim();
        str2 = this.Achievement.Description.Substring(str1.Length).Trim();
      }
      this._capDescriptionLine1.Text = str1;
      this._capDescriptionLine2.Text = str2;
      if (str2 == null)
      {
        this._capDescriptionLine1.ParentAlignment = AlignModes.Vertically;
        this._capDescriptionLine2.Visible = false;
      }
      else
      {
        this._capDescriptionLine1.ParentAlignment = AlignModes.Top;
        this._capDescriptionLine2.Visible = true;
        this._capDescriptionLine2.ParentAlignment = AlignModes.Bottom;
      }
      this._capCompletion.Text = string.Format("{0:##0}%", (object) this.GetCompletionPercentage(this.Achievement));
      if (Game1.ProfilesManager.CurrentProfile.IsAchievementEarned(this.Achievement.EventType))
      {
        this._image.BlendColor = Color.White;
        this._capDescriptionLine1.BlendColor = this.CaptionColorWon;
        this._capDescriptionLine2.BlendColor = this.CaptionColorWon;
      }
      else
      {
        this._image.BlendColor = new Color(0, 0, 0, (int) byte.MaxValue);
        this._capDescriptionLine1.BlendColor = this.CaptionColorNotWon;
        this._capDescriptionLine2.BlendColor = this.CaptionColorNotWon;
      }
    }
  }
}
