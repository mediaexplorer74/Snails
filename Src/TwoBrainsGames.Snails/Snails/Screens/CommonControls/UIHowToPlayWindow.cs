
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UIHowToPlayWindow
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Configuration;
using TwoBrainsGames.Snails.Tutorials;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UIHowToPlayWindow : UISnailsWindow
  {
    private UITutorialTopic _topicControl;
    private UISnailsButton _btnNext;
    private UISnailsButton _btnPrev;
    private UIPanel _topicContainer;
    private UIImage _infoSignImg;
    public List<TutorialTopic> _topics;

    public bool UseCloseHotKey
    {
      set => this.CloseButton.UseHotKey = value;
    }

    private int CurrentTutorialTopicIdx { get; set; }

    private int NextTopicToShowIdx { get; set; }

    public List<TutorialTopic> Topics
    {
      get => this._topics;
      set
      {
        this._topics = new List<TutorialTopic>();
        int index = 0;
        foreach (TutorialTopic tutorialTopic in value)
        {
          if (Game1.ProfilesManager.CurrentProfile.IsTutorialTopicRead(tutorialTopic.TopicId) || tutorialTopic.AlwaysUnlockedInHelp)
          {
            this._topics.Insert(index, tutorialTopic);
            ++index;
          }
          else
            this._topics.Insert(this._topics.Count, tutorialTopic);
        }
      }
    }

    private bool AllButtonsHidden
    {
      get => !this._btnNext.Visible && !this._btnPrev.Visible && !this.CloseButton.Visible;
    }

    public UIHowToPlayWindow(UIScreen screenOwner)
      : base(screenOwner)
    {
      this._topicContainer = new UIPanel(screenOwner);
      this._topicContainer.ParentAlignment = AlignModes.Horizontaly;
      this._topicContainer.Position = this.NativeResolution(new Vector2(0.0f, 200f));
      this._topicContainer.Size = this.NativeResolution(new Size(3500f, 4000f));
      this.Board.Controls.Add((UIControl) this._topicContainer);
      this._topicControl = new UITutorialTopic(screenOwner);
      this._topicControl.ParentAlignment = AlignModes.HorizontalyVertically;
      this._topicControl.Visible = false;
      this._topicControl.OnHide += new UIControl.UIEvent(this._topicControl_OnHide);
      this._topicControl.OnShow += new UIControl.UIEvent(this._topicControl_OnShow);
      this._topicContainer.Controls.Add((UIControl) this._topicControl);
      this._btnNext = new UISnailsButton(screenOwner, "BTN_NEXT", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.Next, (UIControl.UIEvent) null, false);
      this._btnNext.ParentAlignment = AlignModes.Bottom;
      this._btnNext.Position = this.NativeResolution(new Vector2(2600f, 0.0f));
      this._btnNext.OnClickBegin += new UIControl.UIEvent(this._btnNext_OnClickBegin);
      this._btnNext.ButtonAction = UISnailsButton.ButtonActionType.Next;
      this.Board.Controls.Add((UIControl) this._btnNext);
      this._btnPrev = new UISnailsButton(screenOwner, "BTN_PREV", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.Prev, (UIControl.UIEvent) null, false);
      this._btnPrev.ParentAlignment = AlignModes.Bottom;
      this._btnPrev.Position = this.NativeResolution(new Vector2(0.0f, 0.0f));
      this._btnPrev.OnClickBegin += new UIControl.UIEvent(this._btnPrev_OnClickBegin);
      this._btnPrev.ButtonAction = UISnailsButton.ButtonActionType.Previous;
      this.Board.Controls.Add((UIControl) this._btnPrev);
      this._infoSignImg = new UIImage(screenOwner);
      this._infoSignImg.Position = new Vector2(400f, 300f);
      this.Board.Controls.Add((UIControl) this._infoSignImg);
      this.OnDismissBegin += new UIControl.UIEvent(this.Window_OnDesmissBegin);
      this.TitleResourceId = "TITLE_HOW_TO_PLAY";
      this.BoardType = UISnailsBoard.BoardType.LightWoodMedium;
      this.OnShow += new UIControl.UIEvent(this.UIHowToPlayWindow_OnShow);
      this.OnShowBegin += new UIControl.UIEvent(this.UIHowToPlayWindow_OnShowBegin);
      this.ContinueButton.Visible = !Game1.GameSettings.UseButtonIcons;
      this.OnScreenStart += new UIControl.UIEvent(this.UIHowToPlayWindow_OnScreenStart);
    }

    private void UIHowToPlayWindow_OnScreenStart(IUIControl sender)
    {
      this._infoSignImg.Sprite = BrainGame.ResourceManager.GetSprite("spriteset/tutorial_with_images/InfoSign", "TUTORIAL");
    }

    private void UIHowToPlayWindow_OnShowBegin(IUIControl sender)
    {
      this.CurrentTutorialTopicIdx = 0;
      this.EnableButtons();
      this.CloseButton.Visible = false;
      if (!this.DismissButtonVisible && this.Topics.Count <= 1)
      {
        if (Game1.GameSettings.PresentationMode == GameSettings.PresentationType.HD)
          this.Position = new Vector2(0.0f, 3000f);
        else
          this.Position = new Vector2(0.0f, 1800f);
      }
      else if (Game1.GameSettings.PresentationMode == GameSettings.PresentationType.HD)
        this.Position = new Vector2(0.0f, 2600f);
      else
        this.Position = new Vector2(0.0f, 1200f);
    }

    private void _btnNext_OnClickBegin(IUIControl sender)
    {
      ((SnailsScreen) this.ScreenOwner).DisableInput();
      this.HideTopic();
      this.NextTopicToShowIdx = this.CurrentTutorialTopicIdx + 1;
      if (this.NextTopicToShowIdx + 1 <= this.Topics.Count)
        return;
      this.NextTopicToShowIdx = 0;
    }

    private void Window_OnDesmissBegin(IUIControl sender)
    {
      this.CloseButton.Visible = false;
      ((SnailsScreen) this.ScreenOwner).DisableInput();
      this.Close();
    }

    private void UIHowToPlayWindow_OnShow(IUIControl sender)
    {
      this.ShowTopic(0);
      this.CloseButton.Visible = Game1.GameSettings.UseButtonIcons;
    }

    private void _btnPrev_OnClickBegin(IUIControl sender)
    {
      ((SnailsScreen) this.ScreenOwner).DisableInput();
      this.HideTopic();
      this.NextTopicToShowIdx = this.CurrentTutorialTopicIdx - 1;
      if (this.NextTopicToShowIdx >= 0)
        return;
      this.NextTopicToShowIdx = this.Topics.Count - 1;
    }

    public void HideTopic() => this._topicControl.Hide();

    private void ShowTopic(int topicIdx)
    {
      this._topicControl.Topic = this.Topics[topicIdx];
      this._topicControl.Show();
      this._topicControl.SetCounter(topicIdx + 1, this.Topics.Count);
      this.CurrentTutorialTopicIdx = topicIdx;
      this.EnableButtons();
    }

    private void _topicControl_OnShow(IUIControl sender)
    {
      ((SnailsScreen) this.ScreenOwner).EnableInput();
    }

    private void _topicControl_OnHide(IUIControl sender)
    {
      if (this.NextTopicToShowIdx == -1)
        return;
      this.ShowTopic(this.NextTopicToShowIdx);
    }

    public void Close()
    {
      this.NextTopicToShowIdx = -1;
      this.HideTopic();
    }

    private void EnableButtons()
    {
      this._btnNext.Visible = this.CurrentTutorialTopicIdx < this.Topics.Count - 1;
      this._btnPrev.Visible = this.CurrentTutorialTopicIdx > 0;
    }
  }
}
