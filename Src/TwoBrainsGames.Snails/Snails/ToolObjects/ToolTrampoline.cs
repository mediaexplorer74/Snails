
// Type: TwoBrainsGames.Snails.ToolObjects.ToolTrampoline
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.ToolObjects
{
  public class ToolTrampoline : ToolObject
  {
    public const string ID = "TOOL_TRAMPOLINE";
    private Trampoline _trampoline;

    public ToolTrampoline()
      : base(ToolObjectType.Trampoline)
    {
    }

    public override void LoadContent() => base.LoadContent();

    public override void OnSelect()
    {
      base.OnSelect();
      this._trampoline = (Trampoline) Stage.CurrentStage.StageData.GetObject("TRAMPOLINE");
    }

    public override void Action(Vector2 position)
    {
      if (this.Quantity <= 0)
        return;
      base.Action(position);
      StageObject stageObject = Stage.CurrentStage.StageData.GetObject("TRAMPOLINE");
      stageObject.Position = position;
      stageObject.UpdateBoundingBox();
      stageObject.Initialize();
      Stage.CurrentStage.AddObjectInRuntime(stageObject);
    }

    public override void DrawCursor(Vector2 position, bool enabled)
    {
      this._trampoline.Position = position;
      this._trampoline.Draw(false);
    }
  }
}
