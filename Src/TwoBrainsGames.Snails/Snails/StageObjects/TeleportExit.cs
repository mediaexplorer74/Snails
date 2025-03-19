
// Type: TwoBrainsGames.Snails.StageObjects.TeleportExit
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class TeleportExit : StageObject
  {
    public TeleportExit()
      : base(StageObjectType.TeleportExit)
    {
    }

    public TeleportExit(TeleportEntrance other)
      : base((StageObject) other)
    {
      this.Copy((StageObject) other);
    }

    public override void Copy(StageObject other) => base.Copy(other);

    public override void Update(BrainGameTime gameTime) => base.Update(gameTime);

    public override void OnCollide(StageObject obj)
    {
      Snail snail = obj as Snail;
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
    }

    public override DataFileRecord ToDataFileRecord() => base.ToDataFileRecord();
  }
}
