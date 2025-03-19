
// Type: TwoBrainsGames.Snails.ToolObjects.ToolObject
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.Snails.Input;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.ToolObjects
{
  public class ToolObject : 
    Object2D,
    IBrainComponent,
    ISnailsDataFileSerializable,
    IDataFileSerializable
  {
    private const float ICON_LAYER_DEPTH = 0.82f;
    private const float SELECTED_ICON_LAYER_DEPTH = 0.8f;
    private const float COUNTER_LAYER_DEPTH = 0.92f;
    public const string RES_PLAYER_CURSOR_SELECTION = "spriteset/tools-menu-selected";
    private string _quantityString;
    private int _quantity;
    private Vector2 _iconPosition;
    private Vector2 _quantityPosition;
    protected int _shortcutFrameIdx;
    protected GameplayInput.GamePlayButtons _shortcutKey;
    private Vector2 _shortcutKeyPosition;
    private string _onUseSoundRes;
    public ToolObjectType Type;
    public TextFont Font;
    private bool _selected;
    protected bool _allowOnPaths;
    private Sample _toolSelection;
    private Sample _toolUseSound;
    public Color CursorDrawBlendColor;
    public int _toolboxIndex;
    public bool SnapIt;
    public string ObjectId;
    protected Sprite _spriteWhenSelected;
    protected Sprite _spriteWhenUnselected;
    protected Sprite _spriteOutOfStock;
    protected Sprite _spriteOutOfStockCross;
    protected Sprite _spriteShortcurKeys;
    public Sprite _iconSprite;
    public Sprite ObjectSprite;
    protected Vector2 CounterPos;
    public BoundingSquare SelectionFrame;
    protected bool _withQuantity;

    public int Quantity
    {
      get => this._quantity;
      set
      {
        this._quantity = value;
        this._quantityString = this._quantity.ToString();
        if (this._quantity <= 0 && this._withQuantity)
          this.Sprite = this._spriteOutOfStock;
        else
          this.Sprite = this._selected ? this._spriteWhenSelected : this._spriteWhenUnselected;
      }
    }

    public bool Selected
    {
      get => this._selected;
      set
      {
        this._selected = value;
        if (this._quantity == 0 && this._withQuantity)
          this.Sprite = this._spriteOutOfStock;
        else
          this.Sprite = this._selected ? this._spriteWhenSelected : this._spriteWhenUnselected;
      }
    }

    public bool WithEnoughQuantity
    {
      get => this._quantity > 0 && this._withQuantity || !this._withQuantity;
    }

    public float Width => (float) this.Sprite.Frames[0].Width;

    public float Height => (float) this.Sprite.Frames[0].Height;

    public bool IsSelectable
    {
      get
      {
        if (this._quantity > 0)
          return true;
        return this._quantity == 0 && !this._withQuantity;
      }
    }

    public ToolObject()
    {
      this.LayerDepth = 0.82f;
      this.SnapIt = false;
      this._withQuantity = true;
    }

    public ToolObject(ToolObjectType type)
    {
      this.Type = type;
      this.CounterPos = new Vector2(5f, 30f);
      this.SnapIt = false;
      this._withQuantity = true;
      this._shortcutFrameIdx = (int) type;
      this._shortcutKey = GameplayInput.GamePlayButtons.None;
    }

    public ToolObject(ToolObjectType type, int quantity)
      : this(type)
    {
      this.Quantity = quantity;
    }

    public virtual void Copy(ToolObject other)
    {
      this.Copy((Object2D) other);
      this._spriteOutOfStock = other._spriteOutOfStock;
      this._spriteOutOfStockCross = other._spriteOutOfStockCross;
      this._spriteWhenSelected = other._spriteWhenSelected;
      this._spriteWhenUnselected = other._spriteWhenUnselected;
      this._iconSprite = other._iconSprite;
      this.Sprite = other.Sprite;
      this.Type = other.Type;
      this.ObjectId = other.ObjectId;
      this.Quantity = other.Quantity;
      this.Font = other.Font;
      this.SnapIt = other.SnapIt;
      this.ObjectSprite = other.ObjectSprite;
      this._spriteShortcurKeys = other._spriteShortcurKeys;
      this._toolSelection = other._toolSelection;
      this._allowOnPaths = other._allowOnPaths;
      this._shortcutKey = other._shortcutKey;
      this.BlendColor = other.BlendColor;
      this._onUseSoundRes = other._onUseSoundRes;
      this._toolUseSound = other._toolUseSound;
    }

    public static ToolObject Create(ToolObjectType type)
    {
      switch (type)
      {
        case ToolObjectType.Box:
          return (ToolObject) new ToolBox();
        case ToolObjectType.Salt:
          return (ToolObject) new ToolSalt();
        case ToolObjectType.Vitamin:
          return (ToolObject) new ToolVitamin();
        case ToolObjectType.Trampoline:
          return (ToolObject) new ToolTrampoline();
        case ToolObjectType.Copper:
          return (ToolObject) new ToolCopper();
        case ToolObjectType.Dynamite:
          return (ToolObject) new ToolDynamite();
        case ToolObjectType.Apple:
          return (ToolObject) new ToolApple();
        case ToolObjectType.DynamiteBox:
          return (ToolObject) new ToolDynamiteBox();
        case ToolObjectType.DynamiteBoxTriggered:
          return (ToolObject) new ToolDynamiteBoxTriggered();
        case ToolObjectType.DirectionalBoxCW:
          return (ToolObject) new ToolDirectionalBox(ToolObjectType.DirectionalBoxCW);
        case ToolObjectType.DirectionalBoxCCW:
          return (ToolObject) new ToolDirectionalBox(ToolObjectType.DirectionalBoxCCW);
        default:
          throw new BrainException("Invalid ToolObjectType [" + type.ToString() + "]");
      }
    }

    public ToolObject Clone()
    {
      ToolObject toolObject = ToolObject.Create(this.Type);
      toolObject.Copy(this);
      return toolObject;
    }

    public virtual void OnSelect()
    {
      if (this._toolSelection == null || this._toolSelection.IsPlaying)
        return;
      this._toolSelection.Play();
    }

    public SpriteBatch SpriteBatch => Levels.CurrentLevel.SpriteBatch;

    public void Initialize()
    {
    }

    public virtual void LoadContent()
    {
      this._iconSprite = BrainGame.ResourceManager.GetSpriteTemporary(this);
      StageObject stageObject = Levels.CurrentLevel.StageData.GetObject(this.ObjectId);
      this.BlendColor = stageObject.BlendColor;
      this.ObjectSprite = BrainGame.ResourceManager.GetSpriteTemporary(stageObject);
      this._spriteWhenUnselected = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "ToolIcon");
      this._spriteOutOfStock = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "ToolIconOutOfStock");
      this._spriteOutOfStockCross = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "ToolIconOutOfStockCross");
      this._spriteWhenSelected = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "ToolIconSelected");
      this._spriteShortcurKeys = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "ShortcutKeys");
      this.Font = BrainGame.ResourceManager.Load<TextFont>("fonts/notebook-medium", ResourceManager.ResourceManagerCacheType.Static);
      this._toolSelection = BrainGame.ResourceManager.GetSampleTemporary("sfx/tool_selection");
      if (this._onUseSoundRes == null)
        return;
      this._toolUseSound = BrainGame.ResourceManager.GetSampleTemporary(this._onUseSoundRes);
    }

    public virtual void Draw()
    {
      this.Sprite.Draw(this.Position, this.SpriteBatch);
      this._iconSprite.Draw(this._iconPosition, this.SpriteBatch);
      this.Font.DrawString(this.SpriteBatch, this._quantityString, this._quantityPosition, new Vector2(1f, 1f), Color.Blue);
      if (this._quantity <= 0)
      {
        this._spriteOutOfStockCross.Draw(this.Position, this.SpriteBatch);
      }
      else
      {
        if (!Game1.GameSettings.WithToolSelectionShortcutKeys)
          return;
        this._spriteShortcurKeys.Draw(this._shortcutKeyPosition, this._shortcutFrameIdx, this.SpriteBatch);
      }
    }

    public void UnloadContent()
    {
    }

    public virtual void SetPosition(Vector2 pos)
    {
      this.Position = pos;
      this._iconPosition = pos + new Vector2(45f, 9f);
      this._quantityPosition = pos + new Vector2(20f, 15f);
      if (Game1.GameSettings.WithToolSelectionShortcutKeys)
        this._shortcutKeyPosition = pos + new Vector2(0.0f, (float) (this._spriteWhenSelected.Height - this._spriteShortcurKeys.Height));
      this.SelectionFrame = new BoundingSquare(this.Position, this.Width, this.Height);
    }

    protected BoundingSquare TransformBB(Vector2 position)
    {
      return this.ObjectSprite.BoundingBox.Transform(position);
    }

    public virtual bool IsValidAtPosition(Vector2 position)
    {
      if (this._quantity <= 0 && this._withQuantity || (double) position.X < 0.0 || (double) position.Y < 0.0 || (double) position.X >= (double) Stage.CurrentStage.Board.Width || (double) position.Y >= (double) Stage.CurrentStage.Board.Height)
        return false;
      if (this.SnapIt)
        return true;
      BoundingSquare bs = this.TransformBB(position);
      if (!Stage.CurrentStage.Board.BoundingBox.Contains(bs))
        return false;
      Board board = Stage.CurrentStage.Board;
      return board.GetTileAt(bs.UpperLeft) == null && board.GetTileAt(bs.UpperRight) == null && board.GetTileAt(bs.LowerLeft) == null && board.GetTileAt(bs.LowerRight) == null || this._allowOnPaths && Stage.CurrentStage.Board.Quadtree.GetCollidingObjects(bs, 2).Count > 0;
    }

    public virtual void Action(Vector2 position)
    {
      if (this._toolUseSound != null)
        this._toolUseSound.Play();
      --this.Quantity;
      if (this.Quantity > 0)
        return;
      this.Quantity = 0;
      this.Selected = false;
    }

    public virtual void DrawCursor(Vector2 position, bool enabled)
    {
      this.ObjectSprite.Draw(position, 0, Color.White, Levels.CurrentLevel.SpriteBatch);
    }

    public virtual void SetCursorOnBoard(bool enabled)
    {
      if (enabled)
        BrainGame.GameCursor.SetCursor(0);
      else
        BrainGame.GameCursor.SetCursor(2);
    }

    public static string ToolTypeToString(ToolObjectType type)
    {
      switch (type)
      {
        case ToolObjectType.Box:
          return "TOOL_BOX";
        case ToolObjectType.Salt:
          return "TOOL_SALT";
        case ToolObjectType.Vitamin:
          return "TOOL_VITAMIN";
        case ToolObjectType.Trampoline:
          return "TOOL_TRAMPOLINE";
        case ToolObjectType.Copper:
          return "TOOL_COPPER";
        case ToolObjectType.Dynamite:
          return "TOOL_DYNAMITE";
        case ToolObjectType.Apple:
          return "TOOL_APPLE";
        case ToolObjectType.DynamiteBox:
          return "TOOL_DYNAMITE_BOX";
        case ToolObjectType.DynamiteBoxTriggered:
          return "TOOL_DYNAMITE_BOX_TRIGGERED";
        case ToolObjectType.DirectionalBoxCW:
          return "TOOL_DIRECTIONAL_BOX_CW";
        case ToolObjectType.DirectionalBoxCCW:
          return "TOOL_DIRECTIONAL_BOX_CCW";
        default:
          throw new SnailsException("Unexpected ToolObjectType [" + type.ToString() + "]");
      }
    }

    public bool IsToolShortcutPressed()
    {
      return this._shortcutKey != GameplayInput.GamePlayButtons.None && Stage.CurrentStage.Input.QueryActionDown(this._shortcutKey);
    }

    protected BoardPathNode GetNearestClickedPath(BoundingSquare bs, Vector2 clickPosition)
    {
      List<IQuadtreeContainable> collidingObjects = Stage.CurrentStage.Board.Quadtree.GetCollidingObjects(bs, 2);
      if (collidingObjects.Count == 0)
        return (BoardPathNode) null;
      BoardPathNode nearestClickedPath = (BoardPathNode) collidingObjects[0];
      if (collidingObjects.Count <= 1)
        return nearestClickedPath;
      float num1 = 0.0f;
      Vector2 vector2 = clickPosition;
      foreach (BoardPathNode boardPathNode in collidingObjects)
      {
        float num2 = Vector2.DistanceSquared(vector2, boardPathNode.Value.P0);
        float num3 = Vector2.DistanceSquared(vector2, boardPathNode.Value.P1);
        float num4 = (double) num2 < (double) num3 ? num3 : num2;
        if ((double) num1 == 0.0 || (double) num1 > (double) num4)
        {
          num1 = num4;
          nearestClickedPath = boardPathNode;
        }
      }
      return nearestClickedPath;
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this.Type = (ToolObjectType) Enum.Parse(typeof (ToolObjectType), record.GetFieldValue<string>("type", this.Type.ToString()), true);
      this.ObjectId = record.GetFieldValue<string>("objectId", this.ObjectId);
      this.Quantity = record.GetFieldValue<int>("quantity", this.Quantity);
      this.SnapIt = record.GetFieldValue<bool>("snapIt", this.SnapIt);
      this._allowOnPaths = record.GetFieldValue<bool>("allowOnPaths", this._allowOnPaths);
      this._shortcutKey = (GameplayInput.GamePlayButtons) Enum.Parse(typeof (GameplayInput.GamePlayButtons), record.GetFieldValue<string>("gamePlayButtonShortkey", this._shortcutKey.ToString()), true);
      this._onUseSoundRes = record.GetFieldValue<string>("onUseSoundRes", (string) null);
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord();
      dataFileRecord.Name = "Tool";
      switch (context)
      {
        case ToDataFileRecordContext.StageDataSave:
          dataFileRecord.AddField("type", (object) this.Type.ToString());
          dataFileRecord.AddField("objectId", (object) this.ObjectId);
          dataFileRecord.AddField("snapIt", (object) this.SnapIt);
          dataFileRecord.AddField("allowOnPaths", (object) this._allowOnPaths);
          dataFileRecord.AddField("onUseSoundRes", (object) this._onUseSoundRes);
          break;
        case ToDataFileRecordContext.StageSave:
          dataFileRecord.RemoveField("res");
          dataFileRecord.RemoveField("sprite");
          break;
      }
      dataFileRecord.AddField("quantity", (object) this.Quantity);
      dataFileRecord.AddField("gamePlayButtonShortkey", (object) this._shortcutKey.ToString());
      return dataFileRecord;
    }
  }
}
