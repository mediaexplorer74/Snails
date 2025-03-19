
// Type: TwoBrainsGames.Snails.StageObjects.StageProp
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using System;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Resources;


namespace TwoBrainsGames.Snails.StageObjects
{
  internal class StageProp : StageObject, ISnailsDataFileSerializable, IDataFileSerializable
  {
    public const string ID = "STAGE_PROP";
    private StageProp.StagePropItem _properties;
    private double _ellapsedHideTime;
    private Sample _sound;

    public static Dictionary<string, StageProp.StagePropItem> StagePropsItems { get; set; }

    public string PropId { get; set; }

    public ThemeType Theme { get; set; }

    public StageProp()
      : base(StageObjectType.StageProp)
    {
    }

    public StageProp(StageProp other)
      : base((StageObject) other)
    {
      this.Copy((StageObject) other);
    }

    public override void Copy(StageObject other)
    {
      base.Copy(other);
      this.PropId = (other as StageProp).PropId;
    }

    public override void LoadContent()
    {
      if (StageProp.StagePropsItems != null && this.PropId != null)
      {
        string spriteResource = StageProp.StagePropsItems[this.PropId]._spriteResource;
        this.ResourceId = BrainPath.GetDirectoryName(spriteResource);
        this.SpriteId = BrainPath.GetFileName(spriteResource);
        if (!string.IsNullOrEmpty(StageProp.StagePropsItems[this.PropId]._soundRes))
          this._sound = BrainGame.ResourceManager.GetSampleTemporary(StageProp.StagePropsItems[this.PropId]._soundRes);
      }
      base.LoadContent();
    }

    public override void Initialize()
    {
      base.Initialize();
      this.DrawInForeground = StageProp.StagePropsItems[this.PropId]._foreground;
      this.Theme = StageProp.StagePropsItems[this.PropId]._theme;
      this._properties = StageProp.StagePropsItems[this.PropId];
      this._ellapsedHideTime = 0.0;
      if (this._properties._showDelay > 0)
      {
        this.Hide();
        this.SetupShowTimer();
      }
      if (!this._properties._randomizeFirstFrame)
        return;
      this.CurrentFrame = BrainGame.Rand.Next(this.Sprite.FrameCount);
    }

    public override void OnLastFrame()
    {
      if (!this._properties._autoHideOnAnimationEnd)
        return;
      this.Hide();
      if (!this._properties._redisplay)
        return;
      this.SetupShowTimer();
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      if (this.IsVisible || !this._properties._redisplay)
        return;
      this._ellapsedHideTime -= gameTime.ElapsedGameTime.TotalMilliseconds;
      if (this._ellapsedHideTime >= 0.0)
        return;
      this.Show();
    }

    public override void Show()
    {
      if (this._sound == null)
        return;
      this._sound.Play();
    }

    private void SetupShowTimer()
    {
      this._ellapsedHideTime = (double) this._properties._showDelay;
      if (!this._properties._randomizeShowDelay)
        return;
      this._ellapsedHideTime = (double) BrainGame.Rand.Next(this._properties._showDelay);
    }

    public override string ToString() => this.PropId == null ? base.ToString() : this.PropId;

    public static StageProp Create(string id)
    {
      StageProp.StagePropItem stagePropsItem = StageProp.StagePropsItems[id];
      StageProp stageProp = new StageProp();
      stageProp.Id = "STAGE_PROP";
      stageProp.PropId = id;
      stageProp.SpriteId = BrainPath.GetFileName(stagePropsItem._spriteResource);
      stageProp.ResourceId = BrainPath.GetDirectoryName(stagePropsItem._spriteResource);
      stageProp.DrawInForeground = stagePropsItem._foreground;
      stageProp.Theme = stagePropsItem._theme;
      stageProp._properties = stagePropsItem;
      stageProp.LoadContent();
      return stageProp;
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this.PropId = record.GetFieldValue<string>("propId");
      DataFileRecordList dataFileRecordList = record.SelectRecords("Props\\Prop");
      if (dataFileRecordList == null || dataFileRecordList.Count <= 0)
        return;
      StageProp.StagePropsItems = new Dictionary<string, StageProp.StagePropItem>();
      foreach (DataFileRecord dataFileRecord in dataFileRecordList)
      {
        string fieldValue1 = dataFileRecord.GetFieldValue<string>("id");
        string spriteRes = dataFileRecord.GetFieldValue<string>("res") + "/" + dataFileRecord.GetFieldValue<string>("sprite");
        bool fieldValue2 = dataFileRecord.GetFieldValue<bool>("foreground", false);
        ThemeType theme = (ThemeType) Enum.Parse(typeof (ThemeType), dataFileRecord.GetFieldValue<string>("theme"), true);
        bool fieldValue3 = dataFileRecord.GetFieldValue<bool>("autoHideOnAnimationEnd", false);
        bool fieldValue4 = dataFileRecord.GetFieldValue<bool>("redisplay", false);
        string fieldValue5 = dataFileRecord.GetFieldValue<string>("displayDelay", (string) null);
        string fieldValue6 = dataFileRecord.GetFieldValue<string>("soundRes", (string) null);
        bool randomizeShowDelay = false;
        int showDelay = 0;
        if (fieldValue5 != null)
        {
          if (fieldValue5.Contains("rand:"))
          {
            showDelay = Convert.ToInt32(fieldValue5.Substring(fieldValue5.IndexOf(":") + 1));
            randomizeShowDelay = true;
          }
          else
            showDelay = Convert.ToInt32(fieldValue5);
        }
        bool fieldValue7 = dataFileRecord.GetFieldValue<bool>("randomizeFristFrame", false);
        StageProp.StagePropsItems.Add(fieldValue1, new StageProp.StagePropItem(fieldValue1, spriteRes, fieldValue2, theme, fieldValue3, showDelay, randomizeShowDelay, fieldValue4, fieldValue5, fieldValue7, fieldValue6));
      }
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord1 = base.ToDataFileRecord(context);
      dataFileRecord1.AddField("propId", (object) this.PropId);
      if (context == ToDataFileRecordContext.StageDataSave)
      {
        DataFileRecord dataFileRecord2 = dataFileRecord1.AddRecord("Props");
        foreach (KeyValuePair<string, StageProp.StagePropItem> stagePropsItem in StageProp.StagePropsItems)
        {
          DataFileRecord record = new DataFileRecord("Prop");
          record.AddField("id", (object) stagePropsItem.Key);
          record.AddField("res", (object) BrainPath.GetDirectoryName(stagePropsItem.Value._spriteResource));
          record.AddField("sprite", (object) BrainPath.GetFileName(stagePropsItem.Value._spriteResource));
          record.AddField("foreground", (object) stagePropsItem.Value._foreground);
          record.AddField("theme", (object) stagePropsItem.Value._theme.ToString());
          record.AddField("autoHideOnAnimationEnd", (object) stagePropsItem.Value._autoHideOnAnimationEnd);
          record.AddField("redisplay", (object) stagePropsItem.Value._redisplay);
          record.AddField("displayDelay", (object) stagePropsItem.Value._displayDelayStr);
          record.AddField("randomizeFristFrame", (object) stagePropsItem.Value._randomizeFirstFrame);
          record.AddField("soundRes", (object) stagePropsItem.Value._soundRes);
          dataFileRecord2.AddRecord(record);
        }
      }
      return dataFileRecord1;
    }

    public struct StagePropItem(
      string id,
      string spriteRes,
      bool foreground,
      ThemeType theme,
      bool autoHideOnAnimationEnd,
      int showDelay,
      bool randomizeShowDelay,
      bool redisplay,
      string displayDelayStr,
      bool randomizeFirstFrame,
      string soundRes)
    {
      public string _id = id;
      public string _spriteResource = spriteRes;
      public bool _foreground = foreground;
      public ThemeType _theme = theme;
      public bool _autoHideOnAnimationEnd = autoHideOnAnimationEnd;
      public bool _redisplay = redisplay;
      public bool _randomizeShowDelay = randomizeShowDelay;
      public int _showDelay = showDelay;
      public string _displayDelayStr = displayDelayStr;
      public bool _randomizeFirstFrame = randomizeFirstFrame;
      public string _soundRes = soundRes;
    }
  }
}
