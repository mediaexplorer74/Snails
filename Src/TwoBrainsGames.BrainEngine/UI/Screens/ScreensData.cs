
// Type: TwoBrainsGames.BrainEngine.UI.Screens.ScreensData
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using System.Collections.Generic;
using TwoBrainsGames.BrainEngine.Data.DataFiles;


namespace TwoBrainsGames.BrainEngine.UI.Screens
{
  public class ScreensData : IDataFileSerializable
  {
    public string _startupGroup;
    public string _startupScreenId;
    public Dictionary<string, ScreensData.ScreenGroupData> _groupsData;

    public ScreensData()
    {
      this._groupsData = new Dictionary<string, ScreensData.ScreenGroupData>();
    }

    public static ScreensData FromDataFileRecord(DataFileRecord record)
    {
      ScreensData screensData = new ScreensData();
      screensData.InitFromDataFileRecord(record);
      return screensData;
    }

    public void InitFromDataFileRecord(DataFileRecord record)
    {
      this._startupGroup = record.GetFieldValue<string>("StartupGroup");
      this._startupScreenId = record.GetFieldValue<string>("StartupScreen");
      this._groupsData = new Dictionary<string, ScreensData.ScreenGroupData>();
      foreach (DataFileRecord selectRecord1 in record.SelectRecords("ScreenGroup"))
      {
        ScreensData.ScreenGroupData screenGroupData = new ScreensData.ScreenGroupData();
        screenGroupData.GroupId = selectRecord1.GetFieldValue<string>("Id");
        screenGroupData.ScreensData = new List<ScreensData.ScreenData>();
        foreach (DataFileRecord selectRecord2 in selectRecord1.SelectRecords("Screen"))
          screenGroupData.ScreensData.Add(new ScreensData.ScreenData()
          {
            ScreenId = selectRecord2.GetFieldValue<string>("Type"),
            SkipTime = selectRecord2.GetFieldValue<float>("SkipTime")
          });
        this._groupsData.Add(screenGroupData.GroupId, screenGroupData);
      }
    }

    public DataFileRecord ToDataFileRecord()
    {
      DataFile dataFile = new DataFile();
      dataFile.RootRecord.Name = nameof (ScreensData);
      dataFile.RootRecord.AddField("StartupGroup", (object) this._startupGroup);
      dataFile.RootRecord.AddField("StartupScreen", (object) this._startupScreenId);
      foreach (ScreensData.ScreenGroupData screenGroupData in this._groupsData.Values)
      {
        DataFileRecord record1 = new DataFileRecord("ScreenGroup");
        record1.AddField("Id", (object) screenGroupData.GroupId);
        foreach (ScreensData.ScreenData screenData in screenGroupData.ScreensData)
        {
          DataFileRecord record2 = new DataFileRecord("Screen");
          record2.AddField("Type", (object) screenData.ScreenId);
          record2.AddField("SkipTime", (object) screenData.SkipTime);
          record1.AddRecord(record2);
        }
        dataFile.RootRecord.AddRecord(record1);
      }
      return dataFile.RootRecord;
    }

    public struct ScreenData
    {
      public string ScreenId;
      public float SkipTime;
    }

    public struct ScreenGroupData
    {
      public string GroupId;
      public List<ScreensData.ScreenData> ScreensData;

      public override string ToString() => this.GetType().ToString();
    }
  }
}
