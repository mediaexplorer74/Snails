
// Type: TwoBrainsGames.Snails.Stages.HUD.ToolsMenu
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.ToolObjects;


namespace TwoBrainsGames.Snails.Stages.HUD
{
  public class ToolsMenu
  {
    private int _selected = -1;
    private int _previousSelected = -1;
    private Vector2 _toolsAreaMargins;
    private List<ToolObject> _tools = new List<ToolObject>();
    private Vector2 _toolSelHelpPosition;
    public BoundingSquare _ToolsArea;
    private Sprite _sprite;
    private Vector2 _position;
    private int _toolsAreaSpriteBBIdx;
    private float _toolBottomMargin;

    public bool HasToolsToSelect => this.IsThereToolsToSelect();

    public List<ToolObject> Tools => this._tools;

    public int Width => this._sprite.Width;

    public virtual void ControllerEvents()
    {
      if (!Game1.GameSettings.WithToolSelectionShortcutKeys)
        return;
      for (int index = 0; index < this._tools.Count; ++index)
      {
        if (this._tools[index].IsSelectable && this._tools[index].IsToolShortcutPressed())
        {
          this._selected = index;
          this.SelectTool();
          break;
        }
      }
    }

    public void LoadContent()
    {
      this.InitFromContent();
      this._sprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "Menu");
      foreach (ToolObject tool in this._tools)
        tool.LoadContent();
      this._position = new Vector2((float) (BrainGame.ScreenWidth - this._sprite.Width - BrainGame.ScreenRectangle.X), 0.0f);
      this._ToolsArea = this._sprite.BoundingBoxes[this._toolsAreaSpriteBBIdx].Transform(this._position);
      if (Game1.GameSettings.UseGamepad)
      {
        Rectangle rect = StageHUD.SpriteXBoxHelp.Frames[0].Rect;
        this._toolsAreaMargins += new Vector2(0.0f, (float) rect.Height);
        this._toolSelHelpPosition = new Vector2(this._ToolsArea.Left + this._ToolsArea.Width / 2f - (float) (rect.Width / 2), this._ToolsArea.Top + this._toolsAreaMargins.Y - (float) rect.Height);
      }
      this.ComputeToolPositions();
      this.UpdateToolsIndex();
    }

    private void InitFromContent()
    {
      DataFileRecord dataFileRecord = BrainGame.ResourceManager.Load<DataFileRecord>("screens\\Gameplay", ResourceManager.ResourceManagerCacheType.Temporary).SelectRecordByField("ToolsMenu\\Settings", "presentation", (object) Game1.GameSettings.PresentationModeString);
      this._toolsAreaSpriteBBIdx = dataFileRecord.GetFieldValue<int>("toolsAreaSpriteBBIdx");
      this._toolsAreaMargins = dataFileRecord.GetFieldValue<Vector2>("toolsAreaMargins");
      this._toolBottomMargin = dataFileRecord.GetFieldValue<float>("toolBottomMargin");
    }

    private void UpdateToolsIndex()
    {
      for (int index = 0; index < this._tools.Count; ++index)
        this._tools[index]._toolboxIndex = index;
    }

    private void ComputeToolPositions()
    {
      float x = this._ToolsArea.Left + this._toolsAreaMargins.X;
      float y = this._ToolsArea.Top + this._toolsAreaMargins.Y;
      for (int index = 0; index < this._tools.Count; ++index)
      {
        this._tools[index].SetPosition(new Vector2(x, y));
        this._tools[index].UpdateBoundingBox();
        y += this._tools[index].Height + this._toolBottomMargin;
      }
    }

    public void IncrementSelection() => this.SetIncrementSelection(1);

    public void DecrementSelection() => this.SetIncrementSelection(-1);

    public void SetIncrementSelection(int value)
    {
      if (!this.HasToolsToSelect)
        return;
      this._selected += value;
      if (this._selected >= this._tools.Count)
        this._selected = 0;
      if (this._selected < 0)
        this._selected = this._tools.Count - 1;
      if (this._tools[this._selected].IsSelectable)
      {
        this.SelectTool();
      }
      else
      {
        if (!this.HasToolsToSelect)
          return;
        this.SetIncrementSelection(value);
      }
    }

    public void Update(BrainGameTime gameTime)
    {
      this.ControllerEvents();
      foreach (Object2D tool in this._tools)
        tool.Update(gameTime);
    }

    public void Draw()
    {
      this._sprite.Draw(this._position, Stage.CurrentStage.SpriteBatch);
      if (Game1.GameSettings.UseGamepad)
        StageHUD.SpriteXBoxHelp.Draw(this._toolSelHelpPosition, 0, Stage.CurrentStage.SpriteBatch);
      foreach (ToolObject tool in this._tools)
        tool.Draw();
    }

    public void AddTool(ToolObject tool)
    {
      this._tools.Insert(this._tools.Count, tool);
      this.ComputeToolPositions();
      this.UpdateToolsIndex();
    }

    private bool IsThereToolsToSelect()
    {
      for (int index = 0; index < this._tools.Count; ++index)
      {
        if (this._tools[index].IsSelectable)
          return true;
      }
      return false;
    }

    public void RemoveTool(ToolObject tool)
    {
      tool.Selected = false;
      this._selected = -1;
      this._previousSelected = -1;
      this.Tools.Remove(tool);
      this.ComputeToolPositions();
      this.UpdateToolsIndex();
      Stage.CurrentStage.Cursor.SetSelectedTool(this.GetSelectedTool());
    }

    public ToolObject GetSelectedTool()
    {
      return this._selected < 0 || this._selected >= this.Tools.Count ? (ToolObject) null : this.Tools[this._selected];
    }

    private void SelectTool()
    {
      if (this._selected == this._previousSelected)
        return;
      this._previousSelected = this._selected;
      if (this._selected == -1)
        return;
      if (Stage.CurrentStage._state == Stage.StageState.Startup)
        Stage.CurrentStage.StartupEnded();
      this.Tools[this._selected].OnSelect();
      for (int index = 0; index < this.Tools.Count; ++index)
      {
        this.Tools[index].Selected = index == this._selected;
        if (this.Tools[index].Selected)
          Stage.CurrentStage.Cursor.SetSelectedTool(this.Tools[index]);
      }
    }

    public bool IsOver(Vector2 pos) => this._ToolsArea.Contains(pos);

    public void ClickOverTool(Vector2 pos)
    {
      for (int index = 0; index < this.Tools.Count; ++index)
      {
        if (!this.Tools[index].Selected && this.Tools[index].IsSelectable && this.Tools[index].SelectionFrame.Contains(pos))
        {
          this._selected = index;
          this.SelectTool();
        }
      }
    }

    public bool AllowToolPickUp(ToolObjectType toolType)
    {
      ToolObject toolObject = this._tools.Find<ToolObject>((Func<ToolObject, bool>) (match => match.Type == toolType));
      return this._tools.Count < Game1.GameSettings.MaxTools || toolObject != null;
    }

    public void AddToolQuantity(ToolObjectType type, int quantity)
    {
      ToolObject tool = this._tools.Find<ToolObject>((Func<ToolObject, bool>) (match => match.Type == type));
      if (tool == null)
      {
        tool = Stage.CurrentStage.StageData.GetTool(ToolObject.ToolTypeToString(type)).Clone();
        tool.LoadContent();
        this.AddTool(tool);
      }
      tool.Quantity += quantity;
    }

    public bool AllowAddToolQuantity(ToolObjectType type, int quantity)
    {
      ToolObject tool = this._tools.Find<ToolObject>((Func<ToolObject, bool>) (match => match.Type == type));
      bool flag = this._tools.Count < Game1.GameSettings.MaxTools || tool != null;
      if (flag)
      {
        if (tool == null)
        {
          tool = Stage.CurrentStage.StageData.GetTool(ToolObject.ToolTypeToString(type)).Clone();
          tool.LoadContent();
          this.AddTool(tool);
        }
        tool.Quantity += quantity;
      }
      return flag;
    }

    public int GetTotalTools()
    {
      int totalTools = 0;
      for (int index = 0; index < this._tools.Count; ++index)
        totalTools += this._tools[index].Quantity;
      return totalTools;
    }

    public void MissionStateChanged()
    {
    }

    public void SelectEndMission()
    {
    }
  }
}
