
// Type: TwoBrainsGames.Snails.Stages.HUD.StageHUD
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.ToolObjects;


namespace TwoBrainsGames.Snails.Stages.HUD
{
  public class StageHUD : IBrainComponent
  {
    public const int XBOX_HELP_TOOL_SEL_FRAME_NR = 0;
    public const int XBOX_HELP_TIME_WARP_FRAME_NR = 1;
    public const int XBOX_HELP_CLOSE_TUT_FRAME_NR = 2;
    public const int XBOX_HELP_END_MISSION_FRAME_NR = 3;
    public const int WIN_HELP_TIME_WARP_FRAME_NR = 4;
    private List<HUDItem> _infoItems;
    private HUDMinimap _minimap;
    private IncomingMessage _incomingMessage;
    private Sprite _spriteXBoxHelp;
    private HUDItemTimer _itemTimer;
    private HUDItemControls _itemControls;
    private FastForwardStrips _ffStrips;
    public ToolsMenu _toolsMenu;
    public BoundingSquare _stageArea;
    public Vector2 HudCenter;

    public bool IsInteractingWithCursor { get; set; }

    public bool ControlButtonsVisible => this._itemControls._visible;

    public static Sprite SpriteXBoxHelp => Stage.CurrentStage.StageHUD._spriteXBoxHelp;

    public ToolObject Tool => this._toolsMenu.GetSelectedTool();

    public SpriteBatch SpriteBatch
    {
      get => throw new SnailsException("Deprecated. Draw now recieves the SpriteBatch.");
    }

    public StageHUD()
    {
      this._toolsMenu = new ToolsMenu();
      this._infoItems = new List<HUDItem>();
    }

    public void Initialize()
    {
      this._infoItems.Clear();
      if (Stage.CurrentStage.LevelStage._goal == GoalType.SnailDelivery || Stage.CurrentStage.LevelStage._goal == GoalType.TimeAttack)
        this._infoItems.Add((HUDItem) new HUDItemSnailsDelivered());
      else if (Stage.CurrentStage.LevelStage._goal == GoalType.SnailKiller)
        this._infoItems.Add((HUDItem) new HUDItemSnailsCounter());
      this._itemTimer = new HUDItemTimer();
      this._infoItems.Add((HUDItem) this._itemTimer);
      this._infoItems.Add((HUDItem) new HUDItemGoal());
      this._infoItems.Add((HUDItem) new HUDItemMissionStatus());
      Vector2 position = new Vector2(10f + (float) BrainGame.ScreenRectangle.X, 10f + (float) BrainGame.ScreenRectangle.Y);
      foreach (HUDItem infoItem in this._infoItems)
      {
        infoItem.Initialize(position);
        if (infoItem._autoPosition)
          position += new Vector2(infoItem._width, 0.0f);
      }
      this._itemControls = new HUDItemControls();
      this._itemControls.Initialize(Vector2.Zero);
      this._itemControls._visible = false;
      if (Game1.GameSettings.MinimapVisible)
      {
        this._minimap = new HUDMinimap();
        this._minimap.Initialize(new Vector2(5f, 614f));
      }
      this._ffStrips = new FastForwardStrips();
      this._ffStrips.Initialize();
    }

    public void LoadContent()
    {
      this._spriteXBoxHelp = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "ControllsHelp");
      foreach (HUDItem infoItem in this._infoItems)
        infoItem.LoadContent();
      this._toolsMenu.LoadContent();
      this._stageArea = new BoundingSquare(new Vector2(0.0f), this._toolsMenu._ToolsArea.Left, (float) BrainGame.ScreenHeight);
      if (Game1.GameSettings.MinimapVisible)
        this._minimap.SetCameraProjections();
      this._incomingMessage = new IncomingMessage(this._stageArea);
      this._incomingMessage.LoadContent();
      this._itemControls.LoadContent();
      this._ffStrips.LoadContent();
      this._itemControls.StageAreaChanged(this._stageArea);
      this.HudCenter = new Vector2((float) (int) ((double) this._stageArea.Left + (double) this._stageArea.Width / 2.0), (float) (int) ((double) this._stageArea.Top + (double) this._stageArea.Height / 2.0));
    }

    public void Update(BrainGameTime gameTime)
    {
      foreach (HUDItem infoItem in this._infoItems)
      {
        infoItem.HandleInput(gameTime);
        infoItem.Update(gameTime);
      }
      this._toolsMenu.Update(gameTime);
      this.UpdateIncomingMessage(gameTime);
      if (Game1.GameSettings.MinimapVisible)
        this._minimap.Update(gameTime);
      if (this._itemControls._visible)
        this._itemControls.Update(gameTime);
      this._ffStrips.Update(gameTime);
    }

    public void UpdateIncomingMessage(BrainGameTime gameTime)
    {
      if (!this._incomingMessage.IsActive)
        return;
      this._incomingMessage.Update(gameTime);
    }

    public void Draw() => throw new SnailsException("Deprecated. Use Draw(spriteBatch) instead.");

    public void Draw(SpriteBatch spriteBatch)
    {
      if (this._incomingMessage.IsActive)
        this._incomingMessage.Draw(spriteBatch);
      this._toolsMenu.Draw();
      foreach (HUDItem infoItem in this._infoItems)
        infoItem.Draw(spriteBatch);
      if (Game1.GameSettings.MinimapVisible)
        this._minimap.Draw(spriteBatch);
      if (this._itemControls._visible)
        this._itemControls.Draw(spriteBatch);
      Game1.Tutorial.Draw(spriteBatch);
      this._ffStrips.Draw(spriteBatch);
    }

    public void StopTimer() => this._itemTimer.StopTimer();

    public void SnailsStageStatsChanged()
    {
      foreach (HUDItem infoItem in this._infoItems)
        infoItem.SnailsStageStatsChanged();
    }

    public void UnloadContent()
    {
    }

    public void MissionStateChanged()
    {
      this._toolsMenu.MissionStateChanged();
      foreach (HUDItem infoItem in this._infoItems)
        infoItem.MissionStateChanged();
      this._itemControls.MissionStateChanged();
    }

    public void ShowIncomingMessage(IncomingMessage.MessageType message)
    {
      if (this._incomingMessage.IsActive)
        return;
      this._incomingMessage.Show(message);
    }

    public void OnStageStarted()
    {
      this._incomingMessage.Hide();
      this._itemControls._visible = true;
    }

    public void TimeWarpChanged()
    {
      foreach (HUDItem infoItem in this._infoItems)
        infoItem.TimeWarpChanged();
    }
  }
}
