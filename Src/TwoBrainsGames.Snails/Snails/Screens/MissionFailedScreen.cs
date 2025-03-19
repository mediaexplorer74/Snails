
// Type: TwoBrainsGames.Snails.Screens.MissionFailedScreen
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.Snails.Screens.Transitions;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.Screens
{
  internal class MissionFailedScreen(ScreenNavigator owner) : SnailsScreen(owner, ScreenType.MissionFailed)
  {
    private const int REASON_LINES_COUNT = 3;
    protected UISnailsBoard _leafsBoard;
    protected UISnailsBoard _board;
    protected UISnailsMenuTitle _title;
    protected UICaption[] _capReasonLines;
    protected UISnailStamp _failStamp;
    protected UISnailsButton _btnQuit;
    protected UISnailsButton _btnAgain;
    private Stage.MissionFailedReasonType _missionFailedReason;
    private Sample _failedSample;

    private float LineSpacing { get; set; }

    private Vector2 MessagePosition { get; set; }

    public override void OnLoad()
    {
      base.OnLoad();
      this.Name = "MissionFailed";
      this.OnInitializeFromContent += new UIControl.UIEvent(this.MissionFailedScreen_OnInitializeFromContent);
      this.OnAfterInitializeFromContent += new UIControl.UIEvent(this.MissionFailedScreen_OnAfterInitializeFromContent);
      this._leafsBoard = new UISnailsBoard((UIScreen) this, UISnailsBoard.BoardType.LeafsMedium);
      this._leafsBoard.Name = "_leafsBoard";
      this._leafsBoard.ParentAlignment = AlignModes.Horizontaly;
      this._leafsBoard.Size = new Size(this._leafsBoard.Size.Width, this._leafsBoard.Size.Height + 900f);
      this.Controls.Add((UIControl) this._leafsBoard);
      this._board = new UISnailsBoard((UIScreen) this, UISnailsBoard.BoardType.LightWoodMedium);
      this._board.Name = "_board";
      this._board.ParentAlignment = AlignModes.Horizontaly;
      this._board.OnShow += new UIControl.UIEvent(this._board_OnShow);
      this._board.Size = new Size(this._board.Size.Width, this._board.Size.Height + 1500f);
      this._leafsBoard.Controls.Add((UIControl) this._board);
      this._title = new UISnailsMenuTitle((UIScreen) this);
      this._title.Name = "_title";
      this._title.TextResourceId = "TITLE_MISSION_FAILED";
      this._title.BoardSize = UISnailsMenuTitle.TitleSize.Big;
      this._board.Controls.Add((UIControl) this._title);
      this._failStamp = new UISnailStamp((UIScreen) this, "spriteset/common-elements-1/Fail", "__STATIC__");
      this._failStamp.Name = "_failStamp";
      this._failStamp.Position = new Vector2(800f, 3000f);
      this._failStamp.OnShow += new UIControl.UIEvent(this._failStamp_OnShow);
      this._failStamp.BlendColor = new Color(180, 180, 180, 180);
      this._board.Controls.Add((UIControl) this._failStamp);
      this._capReasonLines = new UICaption[3];
      for (int index = 0; index < this._capReasonLines.Length; ++index)
      {
        this._capReasonLines[index] = new UICaption((UIScreen) this, "", Colors.MissionFailedCaptions, UICaption.CaptionStyle.MissionFailedCaptions);
        this._capReasonLines[index].ParentAlignment = AlignModes.Horizontaly;
        this._board.Controls.Add((UIControl) this._capReasonLines[index]);
      }
      this._btnQuit = new UISnailsButton((UIScreen) this, "BTN_STAGE_SELECTION", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.Back, new UIControl.UIEvent(this.btnBack_OnAccept), true);
      this._btnQuit.Name = "_btnQuit";
      this._btnQuit.ParentAlignment = AlignModes.Bottom;
      this._btnQuit.ButtonAction = UISnailsButton.ButtonActionType.StageSelection;
      this._board.Controls.Add((UIControl) this._btnQuit);
      this._btnAgain = new UISnailsButton((UIScreen) this, "BTN_RETRY", UISnailsButton.ButtonSizeType.Medium, InputBase.InputActions.None, new UIControl.UIEvent(this.btnAgain_OnAccept), true);
      this._btnAgain.Name = "_btnAgain";
      this._btnAgain.ParentAlignment = AlignModes.Bottom;
      this._btnAgain.ButtonAction = UISnailsButton.ButtonActionType.Retry;
      this._board.Controls.Add((UIControl) this._btnAgain);
      this._failedSample = BrainGame.ResourceManager.GetSampleStatic("sfx/mission_failed");
      this.WithBlurEffect = true;
      this.OnBlurEffectEnded += new EventHandler(this.MissionFailedScreen_OnBlurEffectEnded);
    }

    private void MissionFailedScreen_OnInitializeFromContent(IUIControl sender)
    {
      this.LineSpacing = this.GetContentPropertyValue<float>("lineSpacing", this.LineSpacing);
      this.MessagePosition = this.GetContentPropertyValue<Vector2>("messagePosition", this.MessagePosition);
    }

    private void MissionFailedScreen_OnAfterInitializeFromContent(IUIControl sender)
    {
      Vector2 messagePosition = this.MessagePosition;
      for (int index = 0; index < this._capReasonLines.Length; ++index)
      {
        this._capReasonLines[index].Position = messagePosition;
        messagePosition += new Vector2(0.0f, this.LineSpacing);
      }
    }

    private void _failStamp_OnShow(IUIControl sender)
    {
      this.EnableInput();
      this.InstructionBar.HideAllLabels();
      this.InstructionBar.ShowLabel(UIInstructionLabel.LabelActionTypes.Quit);
      this.InstructionBar.ShowLabel(UIInstructionLabel.LabelActionTypes.Retry);
      this._btnAgain.Focus();
    }

    public override void OnStart()
    {
      base.OnStart();
      BrainGame.MusicManager.FadeMusic(0.0f, 500);
      this.DisableInput();
      this._failedSample.Play();
      this._missionFailedReason = this.Navigator.GlobalCache.Get<Stage.MissionFailedReasonType>("MISSION_FAILED_REASON");
      string[] strArray = new string[0];
      switch (this._missionFailedReason)
      {
        case Stage.MissionFailedReasonType.Incomplete:
          strArray = LanguageManager.GetMultiString("MISSION_FAILED_INCOMPLETE");
          break;
        case Stage.MissionFailedReasonType.NotEnoughSnails:
          strArray = LanguageManager.GetMultiString("MISSION_FAILED_NOT_ENOUGH_SNAILS");
          break;
        case Stage.MissionFailedReasonType.TimeExpired:
          strArray = LanguageManager.GetMultiString("MISSION_FAILED_TIME_EXPIRED");
          break;
        case Stage.MissionFailedReasonType.KingIsDead:
          strArray = LanguageManager.GetMultiString("MISSION_FAILED_KING_DEAD");
          break;
      }
      for (int index = 0; index < strArray.Length && index < this._capReasonLines.Length; ++index)
        this._capReasonLines[index].Text = strArray[index];
      this._leafsBoard.Visible = false;
      this._failStamp.Visible = false;
    }

    private void btnBack_OnAccept(IUIControl sender)
    {
      this.DisableInput();
      this.Navigator.GlobalCache.Set("AUTO_SELECT_STAGE", (object) true);
      this.NavigateTo("MainMenu", ScreenType.ThemeSelection.ToString(), (Transition) ScreenTransitions.LeafsClosing, (Transition) ScreenTransitions.LeafsOpening);
    }

    private void btnAgain_OnAccept(IUIControl sender)
    {
      this.DisableInput();
      this.Navigator.GlobalCache.Set("STAGE_START_SHOW_STAGE_INFO", (object) false);
      this.Navigator.GlobalCache.Set("STAGE_START_SHOW_XBOX_HELP", (object) false);
      this.NavigateTo(ScreenType.StageStart.ToString(), (Transition) ScreenTransitions.LeafsClosing, (Transition) null);
    }

    public override void OnUpdate(BrainGameTime gameTime) => base.OnUpdate(gameTime);

    private void MissionFailedScreen_OnBlurEffectEnded(object sender, EventArgs e)
    {
      this._leafsBoard.Show();
    }

    private void _board_OnShow(IUIControl sender) => this._failStamp.Show();
  }
}
