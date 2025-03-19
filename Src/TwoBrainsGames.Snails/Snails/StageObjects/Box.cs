
// Type: TwoBrainsGames.Snails.StageObjects.Box
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class Box : TileObject
  {
    public const string ID = "BOX";
    protected Box.BoxDeployStatus _deployStatus;
    private List<ISwitchable> _objectsToSwitch;
    private bool _autodeployOnLastFrame;
    private Sprite _idleSprite;
    private string _idleSpriteResource;
    protected TileCell _tileCell;
    private int _countSnailsDeadByBox;
    private Sample _sampleBoxDrop;

    public Box()
      : this(StageObjectType.Box)
    {
    }

    public Box(StageObjectType type)
      : base(type)
    {
      this._autodeployOnLastFrame = true;
    }

    public Box(Box other)
      : base((TileObject) other)
    {
      this.Copy((StageObject) other);
    }

    public static Box CreateAndDeploy(Box.BoxType type, Vector2 position)
    {
      string id;
      switch (type)
      {
        case Box.BoxType.WoodBox:
          id = "BOX";
          break;
        case Box.BoxType.MetalBox:
          id = "COPPER";
          break;
        case Box.BoxType.DynamiteBox:
          id = "DYNAMITE_BOX";
          break;
        case Box.BoxType.DynamiteBoxTriggered:
          id = "DYNAMITE_BOX_TRIGGERED";
          break;
        default:
          throw new SnailsException("Invalid box type " + type.ToString());
      }
      StageObject andDeploy = Stage.CurrentStage.StageData.GetObject(id);
      andDeploy.Position = position;
      andDeploy.SnapIt();
      andDeploy.UpdateBoundingBox();
      Stage.CurrentStage.AddObjectInRuntime(andDeploy);
      Stage.CurrentStage.Board.SetTileAt(Tile.Empty(), andDeploy.BoardX, andDeploy.BoardY);
      ((Box) andDeploy).BoxDeployed();
      return (Box) andDeploy;
    }

    public void BoxDeployed() => this._sampleBoxDrop.Play();

    public override void Copy(StageObject other)
    {
      base.Copy(other);
      Box box = (Box) other;
      this._autodeployOnLastFrame = box._autodeployOnLastFrame;
      this._idleSpriteResource = box._idleSpriteResource;
    }

    public override void LoadContent()
    {
      base.LoadContent();
      if (!string.IsNullOrEmpty(this._idleSpriteResource))
        this._idleSprite = BrainGame.ResourceManager.GetSpriteTemporary(this._idleSpriteResource);
      this._sampleBoxDrop = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/box-drop");
    }

    public override void Initialize()
    {
      base.Initialize();
      this._deployStatus = Box.BoxDeployStatus.Deploying;
      this._objectsToSwitch = new List<ISwitchable>();
    }

    protected virtual void BoxDeployed(bool addTile, bool addPaths, bool checkCollisions)
    {
      if (addTile)
      {
        Tile tile = this._tile;
        this._tileCell = Stage.CurrentStage.Board.SetTileAtWithoutPaths(tile, this.BoardX, this.BoardY);
        if (addPaths)
        {
          List<PathSegment> addSegments = new List<PathSegment>();
          addSegments.AddRange((IEnumerable<PathSegment>) this._tileCell.Segments);
          Stage.CurrentStage.Board.AddPathSegments(addSegments);
          Stage.CurrentStage.Board.RemoveCoincidentPathSegments();
        }
      }
      if (checkCollisions)
      {
        this.Quadtree.DoCollisions((IQuadtreeContainable) this, 0);
        this.Quadtree.DoCollisions((IQuadtreeContainable) this, 1);
      }
      this._deployStatus = Box.BoxDeployStatus.Deployed;
    }

    public override void OnCollide(IQuadtreeContainable obj, int listIdx)
    {
      if (listIdx == 0)
      {
        Snail snail = obj as Snail;
        snail.KillByCrate();
        if (!(snail is EvilSnail))
          ++this._countSnailsDeadByBox;
        if (this._countSnailsDeadByBox < 20)
          return;
        BrainGame.AchievementsManager.Notify(13);
      }
      else
      {
        if (!(obj is MovingObject))
          return;
        MovingObject movingObject = (MovingObject) obj;
        if (!movingObject.CanDieWithCrates)
          return;
        movingObject.KillByCrate();
      }
    }

    public override void OnLastFrame()
    {
      if (this._deployStatus != Box.BoxDeployStatus.Deploying || !this._autodeployOnLastFrame)
        return;
      Tile tile = this._tile;
      this._tileCell = Stage.CurrentStage.Board.SetTileAtWithoutPaths(tile, this.BoardX, this.BoardY);
      List<PathSegment> addSegments = new List<PathSegment>();
      addSegments.AddRange((IEnumerable<PathSegment>) this._tileCell.Segments);
      Stage.CurrentStage.Board.AddPathSegments(addSegments);
      Stage.CurrentStage.Board.RemoveCoincidentPathSegments();
      this.Quadtree.DoCollisions((IQuadtreeContainable) this, 0);
      this.Quadtree.DoCollisions((IQuadtreeContainable) this, 1);
      Stage.CurrentStage.RemoveObject((StageObject) this);
      this._deployStatus = Box.BoxDeployStatus.Deployed;
      this.SwitchObjects();
    }

    protected void SwitchObjects()
    {
      foreach (ISwitchable switchable in this._objectsToSwitch)
        switchable.SwitchOn();
    }

    public void AddSwitchableObject(ISwitchable switchable)
    {
      this._objectsToSwitch.Add(switchable);
    }

    protected void SetSpriteWhenIdle()
    {
      this.Sprite = this._idleSprite;
      this.CurrentFrame = 0;
    }

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord(context);
      if (context == ToDataFileRecordContext.StageDataSave)
      {
        dataFileRecord.AddField("autodeployOnLastFrame", (object) this._autodeployOnLastFrame);
        dataFileRecord.AddField("idleSpriteRes", (object) this._idleSpriteResource);
      }
      return dataFileRecord;
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this._autodeployOnLastFrame = record.GetFieldValue<bool>("autodeployOnLastFrame", this._autodeployOnLastFrame);
      this._idleSpriteResource = record.GetFieldValue<string>("idleSpriteRes", this._idleSpriteResource);
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public enum BoxType
    {
      WoodBox,
      MetalBox,
      DynamiteBox,
      DynamiteBoxTriggered,
    }

    protected enum BoxDeployStatus
    {
      Deploying,
      Deployed,
    }
  }
}
