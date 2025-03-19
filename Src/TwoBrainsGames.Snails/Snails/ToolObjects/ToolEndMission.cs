
// Type: TwoBrainsGames.Snails.ToolObjects.ToolEndMission
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.Snails.Stages.HUD;


namespace TwoBrainsGames.Snails.ToolObjects
{
  internal class ToolEndMission : ToolObject
  {
    private ToolEndMission.EndMissionState _state;
    private Sprite _stateSprite;
    private string[] _textLines;
    private Vector2 _controllerHelpPosition;
    private Vector2 _stateIconPosition;

    public ToolEndMission.EndMissionState State
    {
      set
      {
        this._state = value;
        this._textLines = (string[]) null;
        switch (this._state)
        {
          case ToolEndMission.EndMissionState.Restart:
            this._textLines = LanguageManager.GetMultiString("LBL_ICON_RESTART");
            break;
          case ToolEndMission.EndMissionState.EndMission:
            this._textLines = LanguageManager.GetMultiString("LBL_ICON_END_MISSION");
            break;
        }
        this.ComputeTextPosition(this._textLines);
      }
    }

    public ToolEndMission()
      : base(ToolObjectType.EndMission)
    {
      this._withQuantity = false;
      this._state = ToolEndMission.EndMissionState.Restart;
    }

    public override void LoadContent()
    {
      this._spriteWhenUnselected = BrainGame.ResourceManager.GetSpriteStatic("spriteset/StageHUD", "ToolIcon");
      this._spriteWhenSelected = BrainGame.ResourceManager.GetSpriteStatic("spriteset/StageHUD", "ToolIconSelected");
      this.Sprite = this._spriteWhenUnselected;
      this._stateSprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/StageHUD", "MissionToolStates");
      this._spriteShortcurKeys = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD/ShortcutKeys");
      this.Font = BrainGame.ResourceManager.Load<TextFont>("fonts/notebook", ResourceManager.ResourceManagerCacheType.Static);
      this.State = ToolEndMission.EndMissionState.Start;
    }

    public override void SetPosition(Vector2 pos)
    {
      base.SetPosition(pos);
      this.ComputeTextPosition(this._textLines);
      this._controllerHelpPosition = pos - new Vector2(5f, 5f);
      this._stateIconPosition = pos + new Vector2(30f, 10f);
    }

    public override void Draw()
    {
      this.Sprite.Draw(this.Position, this.SpriteBatch);
      this._stateSprite.Draw(this._stateIconPosition, (int) this._state, this.SpriteBatch);
      if (!Game1.GameSettings.UseGamepad)
        return;
      StageHUD.SpriteXBoxHelp.Draw(this._controllerHelpPosition, 3, this.SpriteBatch);
    }

    public void Select() => this.Action(Vector2.Zero);

    public override void Action(Vector2 position)
    {
      switch (this._state)
      {
        case ToolEndMission.EndMissionState.Start:
          Stage.CurrentStage.StartupEnded();
          break;
        case ToolEndMission.EndMissionState.Restart:
          Stage.CurrentStage.RestartMission();
          break;
        case ToolEndMission.EndMissionState.EndMission:
          Stage.CurrentStage.EndMission();
          break;
      }
    }

    private void ComputeTextPosition(string[] text)
    {
    }

    public enum EndMissionState
    {
      Start,
      Restart,
      EndMission,
    }
  }
}
