
// Type: TwoBrainsGames.Snails.Stages.CustomStage
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using System.IO;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Data.DataFiles.BinaryDataFile;


namespace TwoBrainsGames.Snails.Stages
{
  public class CustomStage : Stage, IDataFileSerializable
  {
    public ThemeType Theme { get; set; }

    public void Save()
    {
      this.LevelStage._snailsToRelease = this.GetTotalSnailsToRelease();
      this.IncrementBuildNr();
      new BinaryDataFileWriter().Write(this.LevelStage.CustomStageFilename, new DataFile()
      {
        RootRecord = this.ToDataFileRecord(ToDataFileRecordContext.StageSave)
      });
    }

    public static CustomStage FromFile(string filename)
    {
      CustomStage customStage = new CustomStage();
      DataFile dataFile = new BinaryDataFileReader().Read(filename);
      customStage.InitFromDataFileRecord(dataFile.RootRecord);
      customStage.LevelStage.CustomStageFilename = filename;
      return customStage;
    }

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      return base.ToDataFileRecord(context);
    }

    public static string BuildFilename(string stageId)
    {
      return Path.Combine(Game1.GameSettings.CustomStagesFolder, stageId + ".customStage");
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this.LevelStage.IsCustomStage = true;
    }

    public override DataFileRecord ToDataFileRecord() => base.ToDataFileRecord();
  }
}
