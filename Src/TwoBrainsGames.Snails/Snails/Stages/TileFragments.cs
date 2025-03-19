
// Type: TwoBrainsGames.Snails.Stages.TileFragments
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;


namespace TwoBrainsGames.Snails.Stages
{
  public class TileFragments : ISnailsDataFileSerializable, IDataFileSerializable
  {
    private List<TileFragments.FragmentData> _fragmentsData;
    private Sprite[] _fragmentsSprites;

    public Sprite[] FragmentsSprites => this._fragmentsSprites;

    public TileFragments() => this._fragmentsData = new List<TileFragments.FragmentData>();

    public void LoadContent()
    {
      int num = 0;
      foreach (TileFragments.FragmentData fragmentData in this._fragmentsData)
      {
        if (fragmentData._styleGroupId > num)
          num = fragmentData._styleGroupId;
      }
      this._fragmentsSprites = new Sprite[num + 1];
      foreach (TileFragments.FragmentData fragmentData in this._fragmentsData)
      {
        if (fragmentData._res.Contains("%THEME%"))
        {
          string str = fragmentData._res.Replace("%THEME%", Levels.CurrentTheme.ToString());
          this._fragmentsSprites[fragmentData._styleGroupId] = BrainGame.ResourceManager.GetSprite(str + "/" + fragmentData._sprite, "STAGE_THEME_RESOURCES");
        }
        else
          this._fragmentsSprites[fragmentData._styleGroupId] = BrainGame.ResourceManager.GetSpriteTemporary(fragmentData._res, fragmentData._sprite);
      }
    }

    public DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      return this.ToDataFileRecord();
    }

    public void InitFromDataFileRecord(DataFileRecord record)
    {
      this._fragmentsData.Clear();
      foreach (DataFileRecord selectRecord in record.SelectRecords("Fragment"))
        this._fragmentsData.Add(new TileFragments.FragmentData()
        {
          _res = selectRecord.GetFieldValue<string>("res"),
          _sprite = selectRecord.GetFieldValue<string>("sprite"),
          _styleGroupId = selectRecord.GetFieldValue<int>("styleGroupId")
        });
    }

    public DataFileRecord ToDataFileRecord()
    {
      DataFileRecord dataFileRecord = new DataFileRecord(nameof (TileFragments));
      foreach (TileFragments.FragmentData fragmentData in this._fragmentsData)
      {
        DataFileRecord record = new DataFileRecord("Fragment");
        record.AddField("styleGroupId", (object) fragmentData._styleGroupId);
        record.AddField("res", (object) fragmentData._res);
        record.AddField("sprite", (object) fragmentData._sprite);
        dataFileRecord.AddRecord(record);
      }
      return dataFileRecord;
    }

    private struct FragmentData(int styleGroupId, string res, string sprite)
    {
      public string _res = res;
      public string _sprite = sprite;
      public int _styleGroupId = styleGroupId;
    }
  }
}
