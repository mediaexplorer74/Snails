
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UITutorialTopic
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Tutorials;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UITutorialTopic : UIControl
  {
    private const float FADE_SPEED = 0.1f;
    private TutorialTopic _topic;
    private UILocker _locker;
    private UICaption _capTopicCounter;

    private bool Locked { get; set; }

    public TutorialTopic Topic
    {
      get => this._topic;
      set
      {
        this._topic = value;
        if (this._topic == null)
          return;
        this._topic.LoadContent();
        this._topic.Position = this.CenterInPixels;
        this._topic.UpdatePositions();
        this.Locked = !Game1.ProfilesManager.CurrentProfile.IsTutorialTopicRead(this._topic.TopicId);
        if (this._topic.AlwaysUnlockedInHelp)
          this.Locked = false;
        Color color = this.Locked ? Color.Black : Color.White;
        this.ShowEffect = (TransformEffectBase) new ColorEffect(new Color(0, 0, 0, 0), color, 0.1f, false);
        this.HideEffect = (TransformEffectBase) new ColorEffect(color, new Color(0, 0, 0, 0), 0.1f, false);
        this.Size = new Size(this.PixelsToScreenUnits(this._topic.Size));
      }
    }

    public UITutorialTopic(UIScreen screenOwner)
      : base(screenOwner)
    {
      this._locker = new UILocker(screenOwner);
      this._locker.Position = this.NativeResolution(new Vector2(1600f, 1600f));
      this._locker.Visible = false;
      this.Controls.Add((UIControl) this._locker);
      this._capTopicCounter = new UICaption(screenOwner, "", Color.Black, UICaption.CaptionStyle.Notebook);
      this._capTopicCounter.ParentAlignment = AlignModes.Right | AlignModes.Bottom;
      this._capTopicCounter.Margins.Right = this.NativeResolutionX(100f);
      this._capTopicCounter.Margins.Bottom = this.NativeResolutionY(150f);
      this._capTopicCounter.BlendColorWithParent = false;
      this.Controls.Add((UIControl) this._capTopicCounter);
      this.OnShow += new UIControl.UIEvent(this.UITutorialTopic_OnShow);
      this.OnHideBegin += new UIControl.UIEvent(this.UITutorialTopic_OnHideBegin);
    }

    public override void BeginDraw()
    {
      base.BeginDraw();
      if (this.Topic == null)
        return;
      if (!this.Locked)
        this.Topic.DrawTopic(this.SpriteBatch, this.BlendColor);
      else
        this.Topic.DrawTopicBallon(this.SpriteBatch, this.BlendColor);
    }

    private void UITutorialTopic_OnShow(IUIControl sender)
    {
      this._locker.Visible = this.Locked;
      this._capTopicCounter.Visible = true;
      this._capTopicCounter.BlendColor = this.Locked ? Color.White : Color.Black;
    }

    private void UITutorialTopic_OnHideBegin(IUIControl sender)
    {
      this._locker.Visible = false;
      this._capTopicCounter.Visible = false;
    }

    public void SetCounter(int topicNum, int totalTopics)
    {
      this._capTopicCounter.Text = string.Format("{0}/{1}", (object) topicNum, (object) totalTopics);
    }
  }
}
