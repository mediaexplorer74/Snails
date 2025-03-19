
// Type: TwoBrainsGames.BrainEngine.Player.AchievementsManager
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Resources;


namespace TwoBrainsGames.BrainEngine.Player
{
  public class AchievementsManager
  {
    private const int DISPLAY_TIME = 5000;
    private int posX = -1;
    private int posY = -1;
    private string difficultyRes;
    private string ballonRes;
    private string textFontRes;
    private string playSoundRes;
    private double _ellapsedDisplayTime;
    private int queueIdx = -1;
    private Dictionary<int, BrainAchievement> _listAchievementsType = new Dictionary<int, BrainAchievement>();
    private Dictionary<int, Delegate> _eventTable = new Dictionary<int, Delegate>();
    private List<BrainAchievement> _queueAchivements = new List<BrainAchievement>();

    public Dictionary<int, BrainAchievement> Achievements => this._listAchievementsType;

    public bool HasAchievementsInQueue => this._queueAchivements.Count > 0;

    public void Update(BrainGameTime gameTime)
    {
      if (!this.HasAchievementsInQueue)
        return;
      if (this.queueIdx == -1)
      {
        this.queueIdx = 0;
        this._queueAchivements[this.queueIdx].Show();
      }
      this._ellapsedDisplayTime += gameTime.ElapsedRealTime.TotalMilliseconds;
      if (this._ellapsedDisplayTime > 5000.0)
      {
        this._ellapsedDisplayTime = 0.0;
        this._queueAchivements[this.queueIdx].Hide();
        if (this.queueIdx < this._queueAchivements.Count - 1)
        {
          ++this.queueIdx;
          this._queueAchivements[this.queueIdx].Show();
        }
        else
          this.queueIdx = -1;
      }
      if (this.queueIdx != -1)
        return;
      for (int index = 0; index < this._queueAchivements.Count; ++index)
      {
        if (!this._queueAchivements[index].CanBeDisplayed)
        {
          this._queueAchivements.RemoveAt(index);
          --index;
        }
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      if (!this.HasAchievementsInQueue || this.queueIdx == -1)
        return;
      this._queueAchivements[this.queueIdx].Draw(spriteBatch);
    }

    public void QueueAchievement(int eventType)
    {
      if (!this._listAchievementsType.ContainsKey(eventType))
        return;
      BrainAchievement brainAchievement = this._listAchievementsType[eventType];
      this._queueAchivements.Add(brainAchievement);
      this.Unregister(brainAchievement.EventType);
    }

    public BrainAchievement GetAchievement(int eventType)
    {
      return this._listAchievementsType.ContainsKey(eventType) ? this._listAchievementsType[eventType] : (BrainAchievement) null;
    }

    public void Register(int eventType, Callback handler)
    {
      if (!this._eventTable.ContainsKey(eventType))
        this._eventTable.Add(eventType, (Delegate) null);
      this._eventTable[eventType] = Delegate.Combine(this._eventTable[eventType], (Delegate) handler);
    }

    public void Unregister(int eventType)
    {
      if (!this._eventTable.ContainsKey(eventType))
        return;
      this._eventTable[eventType] = (Delegate) null;
      this._eventTable.Remove(eventType);
    }

    public void Notify(int eventType)
    {
      Delegate @delegate;
      if (!this._eventTable.TryGetValue(eventType, out @delegate))
        return;
      Callback callback = (Callback) @delegate;
      if (callback == null)
        return;
      int num = callback();
    }

    public int Verify(int eventType)
    {
      Delegate @delegate;
      if (this._eventTable.TryGetValue(eventType, out @delegate))
      {
        Callback callback = (Callback) @delegate;
        if (callback != null)
          return callback();
      }
      return -1;
    }

    public void InitFromDataFileRecord(DataFileRecord record)
    {
      this.posX = record.GetFieldValue<int>("posX");
      this.posY = record.GetFieldValue<int>("posY");
      this.difficultyRes = record.GetFieldValue<string>("difficultyRes");
      this.ballonRes = record.GetFieldValue<string>("ballonRes");
      this.textFontRes = record.GetFieldValue<string>("textFontRes");
      this.playSoundRes = record.GetFieldValue<string>("playSoundRes");
      foreach (DataFileRecord selectRecord in record.SelectRecords("Achievements\\Achievement"))
      {
        BrainAchievement brainAchievement = new BrainAchievement(this.difficultyRes, this.ballonRes, this.textFontRes, this.playSoundRes);
        brainAchievement.InitFromDataFileRecord(selectRecord);
        brainAchievement.LoadContent();
        brainAchievement.Position = new Vector2((float) this.posX, (float) this.posY);
        this._listAchievementsType.Add(brainAchievement.EventType, brainAchievement);
      }
    }

    internal void Load(string asset)
    {
      this.InitFromDataFileRecord(BrainGame.ResourceManager.Load<DataFileRecord>(asset, ResourceManager.ResourceManagerCacheType.Static));
    }
  }
}
