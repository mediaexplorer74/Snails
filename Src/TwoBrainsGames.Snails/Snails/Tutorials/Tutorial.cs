
// Type: TwoBrainsGames.Snails.Tutorials.Tutorial
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.Tutorials
{
  public class Tutorial : 
    IBrainComponent,
    ISnailsDataFileSerializable,
    IDataFileSerializable,
    IAsyncOperation
  {
    private List<TutorialTopic> _topicQueue;
    private List<TutorialTopic> _topics;
    private TutorialTopic _currentTopic;
    public bool _loaded;

    public List<TutorialTopic> Topics => this._topics;

    public bool TopicVisible => this._currentTopic != null && this._currentTopic.IsOpen;

    public Tutorial() => this._topicQueue = new List<TutorialTopic>();

    public static Tutorial FromDataFileRecord(DataFileRecord record)
    {
      Tutorial tutorial = new Tutorial();
      tutorial.InitFromDataFileRecord(record);
      return tutorial;
    }

    public List<TutorialTopic> GetTopics(int[] topicsIds)
    {
      List<TutorialTopic> topics = new List<TutorialTopic>();
      foreach (int topicsId in topicsIds)
        topics.Add(this.GetTopic(topicsId));
      return topics;
    }

    public TutorialTopic GetTopic(int topicId)
    {
      return this._topics.Find<TutorialTopic>((Func<TutorialTopic, bool>) (t => t.TopicId == topicId)) ?? throw new SnailsException("Topic with id [" + topicId.ToString() + "] not found.");
    }

    public void ShowTopic(int topicId, bool showIfAlreadyViewed)
    {
      if (Game1.ProfilesManager.CurrentProfile != null && Game1.ProfilesManager.CurrentProfile.IsTutorialTopicRead(topicId) && !showIfAlreadyViewed)
        return;
      TutorialTopic topic = this.GetTopic(topicId);
      if (!string.IsNullOrEmpty(topic._stageId) && topic._stageId != Stage.CurrentStage.LevelStage.StageId)
        return;
      if (this._currentTopic == null)
      {
        this.ShowTopic(topic);
      }
      else
      {
        if (topic._inQueue)
          return;
        this._topicQueue.Add(topic);
        topic._inQueue = true;
      }
    }

    private void ShowTopic(TutorialTopic topic)
    {
      this._currentTopic = topic;
      this._currentTopic.Show();
    }

    public SpriteBatch SpriteBatch => throw new NotImplementedException();

    public void Initialize()
    {
      this._currentTopic = (TutorialTopic) null;
      this._topicQueue.Clear();
    }

    public void LoadContent() => this._loaded = true;

    public void Update(BrainGameTime gameTime)
    {
      if (this._currentTopic == null)
        return;
      this._currentTopic.Update(gameTime);
      if (!this._currentTopic.IsClosed)
        return;
      this._currentTopic = (TutorialTopic) null;
      if (this._topicQueue.Count <= 0)
        return;
      this.ShowTopic(this._topicQueue[0]);
      this._topicQueue.RemoveAt(0);
    }

    public void Draw()
    {
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      if (this._currentTopic == null)
        return;
      this._currentTopic.Draw(spriteBatch);
    }

    public void UnloadContent()
    {
    }

    public void InitFromDataFileRecord(DataFileRecord record)
    {
      this._topics = new List<TutorialTopic>();
      foreach (DataFileRecord selectRecord in record.SelectRecords("TutorialTopic"))
      {
        TutorialTopic tutorialTopic = TutorialTopic.FromDataFileRecord(selectRecord, this);
        if (Game1.Instance == null || BrainGame.Settings == null || (tutorialTopic.Platforms & BrainGame.Settings.Platform) == BrainGame.Settings.Platform)
          this._topics.Add(tutorialTopic);
      }
    }

    public DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = new DataFileRecord(nameof (Tutorial));
      foreach (TutorialTopic topic in this._topics)
        dataFileRecord.AddRecord(topic.ToDataFileRecord());
      return dataFileRecord;
    }

    public void BeginLoad() => this.LoadContent();

    public object AsyncLoadingParams
    {
      set
      {
      }
    }
  }
}
