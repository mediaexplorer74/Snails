
// Type: TwoBrainsGames.Snails.ToolObjects.ToolCrate
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.ToolObjects
{
  public class ToolCrate : ToolObject
  {
    private string _objectId;

    public ToolCrate(ToolObjectType objType)
      : base(objType)
    {
      switch (objType)
      {
        case ToolObjectType.Box:
          this._objectId = "BOX";
          break;
        case ToolObjectType.Copper:
          this._objectId = "COPPER";
          break;
        case ToolObjectType.DynamiteBox:
          this._objectId = "DYNAMITE_BOX";
          break;
        case ToolObjectType.DynamiteBoxTriggered:
          this._objectId = "DYNAMITE_BOX_TRIGGERED";
          break;
        case ToolObjectType.DirectionalBoxCW:
          this._objectId = "DIRECTIONAL_BOX_CW";
          break;
        case ToolObjectType.DirectionalBoxCCW:
          this._objectId = "DIRECTIONAL_BOX_CCW";
          break;
        default:
          throw new SnailsException("Invalid object type for ToolBox [" + objType.ToString() + "]");
      }
    }

    public override void Copy(ToolObject other) => base.Copy(other);

    public override void LoadContent() => base.LoadContent();

    public override void Action(Vector2 position)
    {
      base.Action(position);
      StageObject stageObject = Stage.CurrentStage.StageData.GetObject(this._objectId);
      stageObject.Position = position;
      stageObject.SnapIt();
      stageObject.UpdateBoundingBox();
      Stage.CurrentStage.AddObjectInRuntime(stageObject);
      ((Box) stageObject).BoxDeployed();
    }

    public override bool IsValidAtPosition(Vector2 position)
    {
      if (!base.IsValidAtPosition(position))
        return false;
      TileCellCoords coordsFromPosition = Stage.CurrentStage.Board.GetCoordsFromPosition(position);
      int colIndex = coordsFromPosition.ColIndex;
      int rowIndex = coordsFromPosition.RowIndex;
      BoundingSquare crateBs = new BoundingSquare(new Vector2((float) (colIndex * 60 + 2), (float) (rowIndex * 60 + 2)), 56f, 56f);
      foreach (StageObject stageObject in Stage.CurrentStage.Objects)
      {
        if (!stageObject.CrateToolIsValid(crateBs))
          return false;
      }
      return Stage.CurrentStage.Board.Tiles[rowIndex, colIndex] == null && (colIndex > 0 && Stage.CurrentStage.Board.Tiles[rowIndex, colIndex - 1] != null || colIndex + 1 < Stage.CurrentStage.Board.Columns && Stage.CurrentStage.Board.Tiles[rowIndex, colIndex + 1] != null || rowIndex + 1 < Stage.CurrentStage.Board.Rows && Stage.CurrentStage.Board.Tiles[rowIndex + 1, colIndex] != null || rowIndex > 0 && Stage.CurrentStage.Board.Tiles[rowIndex - 1, colIndex] != null);
    }
  }
}
