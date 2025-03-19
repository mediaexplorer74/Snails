
// Type: TwoBrainsGames.Snails.Stages.StageData
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using System;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.Snails.ToolObjects;


namespace TwoBrainsGames.Snails.Stages
{
  public class StageData : ISnailsDataFileSerializable, IDataFileSerializable
  {
    private Dictionary<string, StageObject> _objects;
    private Dictionary<string, ToolObject> _tools;
    private Dictionary<string, Tile> _tiles;
    private Dictionary<string, SnailsBackgroundLayer> _layers;
    private Dictionary<string, LightSource> _lightSources;
    private TileFragments _tileFragments;

    public Dictionary<string, StageObject> Objects => this._objects;

    public Tile[,] TilesByStyleId { get; set; }

    public Dictionary<string, Tile> Tiles => this._tiles;

    public Dictionary<string, ToolObject> Tools => this._tools;

    public Dictionary<string, LightSource> LightSources => this._lightSources;

    public TileFragments TileFragments => this._tileFragments;

    public bool ContainsTile(string id) => !string.IsNullOrEmpty(id) && this._tiles.ContainsKey(id);

    public Tile GetTile(string id)
    {
      return this.ContainsTile(id) ? this._tiles[id] : throw new SnailsException("Tile cannot be retrieved. Tile with id [" + id + "] does not exist.");
    }

    public StageObject GetObjectNoInitialize(string id) => this._objects[id].Clone();

    public StageObject GetObject(string id)
    {
      StageObject stageObject = this._objects[id].Clone();
      stageObject.LoadContent();
      stageObject.Initialize();
      return stageObject;
    }

    public ToolObject GetTool(string id) => this._tools[id];

    public LightSource GetLightSource(string id)
    {
      LightSource lightSource = this._lightSources[id].Clone();
      lightSource.LoadContent();
      return lightSource;
    }

    public Sprite GetFragmentSprite(int styleGroupId)
    {
      return styleGroupId < 0 || styleGroupId >= this._tileFragments.FragmentsSprites.Length ? (Sprite) null : this._tileFragments.FragmentsSprites[styleGroupId];
    }

    public static StageData FromDataFileRecord(DataFileRecord record)
    {
      StageData stageData = new StageData();
      stageData.InitFromDataFileRecord(record);
      return stageData;
    }

    private void InitTileByStyleArray(int styleCount)
    {
      this.TilesByStyleId = new Tile[styleCount, 256];
      foreach (Tile tile in this.Tiles.Values)
        this.TilesByStyleId[tile.StyleGroupId, (int) tile._walkFlags] = tile;
    }

    public Tile GetTileByStyle(int styleGroupId, WalkFlags walkFlags)
    {
      return this.TilesByStyleId[styleGroupId, (int) walkFlags];
    }

    public void LoadContent(ThemeType theme)
    {
      foreach (Tile tile in this.Tiles.Values)
      {
        if (tile.ValidThemes == ThemeType.All || tile.ValidThemes == theme)
        {
          if (tile.ResourceId.Contains("%THEME%"))
          {
            tile.ResourceId = tile.ResourceId.Replace("%THEME%", theme.ToString());
            tile._contentManagerId = "STAGE_THEME_RESOURCES";
          }
          tile.LoadContent();
        }
      }
      foreach (StageObject stageObject in this.Objects.Values)
      {
        if (stageObject.ResourceId.Contains("%THEME%") || stageObject.ResourceId.Contains(ThemeType.ThemeA.ToString()) || stageObject.ResourceId.Contains(ThemeType.ThemeB.ToString()) || stageObject.ResourceId.Contains(ThemeType.ThemeC.ToString()) || stageObject.ResourceId.Contains(ThemeType.ThemeD.ToString()))
        {
          stageObject.ResourceId = stageObject.ResourceId.Replace("%THEME%", theme.ToString());
          stageObject._contentManagerId = "STAGE_THEME_RESOURCES";
        }
        if (stageObject.PreLoad)
          stageObject.LoadContent();
      }
      foreach (ToolObject toolObject in this.Tools.Values)
        toolObject.LoadContent();
      this._tileFragments.LoadContent();
    }

    public SnailsBackgroundLayer GetLayer(string id) => this._layers[id].Clone();

    public List<SnailsBackgroundLayer> GetLayers()
    {
      List<SnailsBackgroundLayer> layers = new List<SnailsBackgroundLayer>();
      foreach (SnailsBackgroundLayer other in this._layers.Values)
        layers.Add(new SnailsBackgroundLayer(other));
      return layers;
    }

    public List<int> GetStyleGroupIdList()
    {
      List<int> styleGroupIdList = new List<int>();
      foreach (Tile tile in this.Tiles.Values)
      {
        bool flag = false;
        foreach (int num in styleGroupIdList)
        {
          if (num == tile.StyleGroupId)
          {
            flag = true;
            break;
          }
        }
        if (!flag)
          styleGroupIdList.Add(tile.StyleGroupId);
      }
      return styleGroupIdList;
    }

    public Tile GetTileMatchingWalkFlags(int styleGroupId, WalkFlags flags)
    {
      return this.TilesByStyleId[styleGroupId, (int) flags] ?? this.TilesByStyleId[styleGroupId, 15];
    }

    public void InitFromDataFileRecord(DataFileRecord record)
    {
      this._objects = new Dictionary<string, StageObject>();
      this._tiles = new Dictionary<string, Tile>();
      this._tools = new Dictionary<string, ToolObject>();
      this._layers = new Dictionary<string, SnailsBackgroundLayer>();
      this._lightSources = new Dictionary<string, LightSource>();
      this._tileFragments = new TileFragments();
      foreach (DataFileRecord selectRecord in record.SelectRecords("Layers\\Layer"))
      {
        SnailsBackgroundLayer snailsBackgroundLayer = new SnailsBackgroundLayer();
        snailsBackgroundLayer.InitFromDataFileRecord(selectRecord);
        this._layers.Add(snailsBackgroundLayer.Id, snailsBackgroundLayer);
      }
      DataFileRecordList dataFileRecordList = record.SelectRecords("Tiles\\Tile");
      int num = 0;
      foreach (DataFileRecord record1 in dataFileRecordList)
      {
        Tile tile = new Tile();
        tile.InitFromDataFileRecord(record1);
        this._tiles[tile.Id] = tile;
        if (tile.StyleGroupId > num)
          num = tile.StyleGroupId;
      }
      foreach (DataFileRecord selectRecord in record.SelectRecords("Objects\\Object"))
      {
        StageObject stageObject = StageObjectFactory.Create((StageObjectType) Enum.Parse(typeof (StageObjectType), selectRecord.GetFieldValue<string>("type"), false));
        stageObject.InitFromDataFileRecord(selectRecord);
        this._objects[stageObject.Id] = stageObject;
      }
      foreach (DataFileRecord selectRecord in record.SelectRecords("Tools\\Tool"))
      {
        ToolObject toolObject = ToolObject.Create((ToolObjectType) Enum.Parse(typeof (ToolObjectType), selectRecord.GetFieldValue<string>("type"), false));
        toolObject.InitFromDataFileRecord(selectRecord);
        this._tools[toolObject.Id] = toolObject;
      }
      foreach (DataFileRecord selectRecord in record.SelectRecords("LightSources\\LightSource"))
      {
        LightSource lightSource = LightSource.Create((LightSource.LightSourceType) Enum.Parse(typeof (LightSource.LightSourceType), selectRecord.GetFieldValue<string>("type", LightSource.LightSourceType.Baselight.ToString()), false));
        lightSource.InitFromDataFileRecord(selectRecord);
        this._lightSources[lightSource.Id] = lightSource;
      }
      this._tileFragments.InitFromDataFileRecord(record.SelectRecord("TileFragments"));
      this.InitTileByStyleArray(num + 1);
    }

    public virtual DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFile dataFile = new DataFile();
      dataFile.RootRecord.Name = nameof (StageData);
      DataFileRecord dataFileRecord1 = dataFile.RootRecord.AddRecord("Layers");
      foreach (SnailsBackgroundLayer snailsBackgroundLayer in this._layers.Values)
        dataFileRecord1.AddRecord(snailsBackgroundLayer.ToDataFileRecord());
      DataFileRecord dataFileRecord2 = dataFile.RootRecord.AddRecord("Tiles");
      foreach (Tile tile in this._tiles.Values)
        dataFileRecord2.AddRecord(tile.ToDataFileRecord());
      DataFileRecord dataFileRecord3 = dataFile.RootRecord.AddRecord("Objects");
      foreach (StageObject stageObject in this._objects.Values)
        dataFileRecord3.AddRecord(stageObject.ToDataFileRecord());
      DataFileRecord dataFileRecord4 = dataFile.RootRecord.AddRecord("Tools");
      foreach (ToolObject toolObject in this._tools.Values)
        dataFileRecord4.AddRecord(toolObject.ToDataFileRecord());
      DataFileRecord dataFileRecord5 = dataFile.RootRecord.AddRecord("LightSources");
      foreach (LightSource lightSource in this._lightSources.Values)
        dataFileRecord5.AddRecord(lightSource.ToDataFileRecord());
      dataFile.RootRecord.AddRecord(this._tileFragments.ToDataFileRecord(context));
      return dataFile.RootRecord;
    }
  }
}
