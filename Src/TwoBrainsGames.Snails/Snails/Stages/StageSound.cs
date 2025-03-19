
// Type: TwoBrainsGames.Snails.Stages.StageSound
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Resources;


namespace TwoBrainsGames.Snails.Stages
{
  public class StageSound : IBrainComponent, ISnailsDataFileSerializable, IDataFileSerializable
  {
    private const int DEFAULT_SURROUND_TIME = 3000;
    private const int DEFAULT_SURROUND_TIME_RND = 15000;
    private int _maxRandomDelay;
    private int _minDelay;
    private double _elapsedTime;
    private double _nextSurroundTime;
    private int _surroundPlayed;
    private int _surroundPlayedMaxBit;
    private bool _musicIsPlaying;
    private TwoBrainsGames.BrainEngine.Resources.Music _music;
    private Sample _ambienceSample;
    private List<Sample> _surroundingsSamples = new List<Sample>();
    public StageSoundSource Music = new StageSoundSource();
    public StageSoundSource AmbienceSetting = new StageSoundSource();
    public List<StageSoundSource> AmbienceSurroundings = new List<StageSoundSource>();

    public SpriteBatch SpriteBatch => Levels.CurrentLevel.SpriteBatch;

    public void Initialize()
    {
      this._musicIsPlaying = false;
      this._surroundPlayed = 0;
      this._elapsedTime = 0.0;
      this._nextSurroundTime = (double) (this._minDelay + BrainGame.Rand.Next(this._maxRandomDelay));
    }

    public void LoadContent()
    {
      if (!string.IsNullOrEmpty(this.Music.Res))
        this._music = BrainGame.ResourceManager.GetMusic(this.Music.Res, "STAGE_THEME_RESOURCES");
      if (!string.IsNullOrEmpty(this.AmbienceSetting.Res))
        this._ambienceSample = BrainGame.ResourceManager.GetSample(this.AmbienceSetting.Res, "STAGE_THEME_RESOURCES");
      for (int index = 0; index < this.AmbienceSurroundings.Count; ++index)
      {
        if (!string.IsNullOrEmpty(this.AmbienceSurroundings[index].Res))
          this._surroundingsSamples.Add(BrainGame.ResourceManager.GetSample(this.AmbienceSurroundings[index].Res, "STAGE_THEME_RESOURCES"));
      }
    }

    public void Update(BrainGameTime gameTime)
    {
      if (this.AmbienceSurroundings.Count == 0)
        return;
      this._elapsedTime += gameTime.ElapsedRealTime.TotalMilliseconds;
      if (this._elapsedTime < this._nextSurroundTime)
        return;
      int index = BrainGame.Rand.Next(this.AmbienceSurroundings.Count);
      if ((this._surroundPlayed & 1 << index) != 0)
        return;
      this._surroundPlayed |= 1 << index;
      if (this._surroundPlayed == this._surroundPlayedMaxBit)
        this._surroundPlayed = 0;
      StageSoundSource ambienceSurrounding = this.AmbienceSurroundings[index];
      this._surroundingsSamples[index].Play(ambienceSurrounding.Volume, ambienceSurrounding.Loop);
      this._elapsedTime = 0.0;
      this._nextSurroundTime = (double) (this._minDelay + BrainGame.Rand.Next(this._maxRandomDelay));
    }

    public void Draw()
    {
    }

    public void UnloadContent()
    {
    }

    public void PlayMusic()
    {
      this._music.CancelFade();
      if (this._music == null)
        return;
      this._music.Play(true);
    }

    public void PlayMusicIfNotPlaying()
    {
      if (this._music == null || this._musicIsPlaying)
        return;
      this._music.Play(true);
      this._musicIsPlaying = true;
    }

    public void StopMusic()
    {
      this._musicIsPlaying = false;
      if (this._music == null)
        return;
      this._music.Fade(0.0f, new TimeSpan(0, 0, 0, 0, 500));
    }

    public void PlayAmbience()
    {
      if (this._ambienceSample == null)
        return;
      this._ambienceSample.Play(this.AmbienceSetting.Volume, true);
    }

    public void PauseAmbience()
    {
      if (this._ambienceSample == null)
        return;
      this._ambienceSample.Pause();
    }

    public void ResumeAmbience()
    {
      if (this._ambienceSample == null)
        return;
      this._ambienceSample.Resume();
    }

    public void InitFromDataFileRecord(DataFileRecord record)
    {
      this.Music.InitFromDataFileRecord(record.SelectRecord("Music"));
      this.AmbienceSetting.InitFromDataFileRecord(record.SelectRecord("Ambience\\Setting"));
      DataFileRecord dataFileRecord = record.SelectRecord("Ambience\\Surroundings");
      if (dataFileRecord == null)
        return;
      this._minDelay = dataFileRecord.GetFieldValue<int>("minDelay", 3000);
      this._maxRandomDelay = dataFileRecord.GetFieldValue<int>("maxRandomDelay", 15000);
      DataFileRecordList dataFileRecordList = dataFileRecord.SelectRecords("Surround");
      if (dataFileRecordList == null)
        return;
      this._surroundPlayedMaxBit = (int) Math.Pow(2.0, (double) dataFileRecordList.Count) - 1;
      this.AmbienceSurroundings = new List<StageSoundSource>(dataFileRecordList.Count);
      foreach (DataFileRecord record1 in dataFileRecordList)
      {
        StageSoundSource stageSoundSource = new StageSoundSource();
        stageSoundSource.InitFromDataFileRecord(record1);
        this.AmbienceSurroundings.Add(stageSoundSource);
      }
    }

    public DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = new DataFileRecord("Sound");
      dataFileRecord.AddRecord(this.Music.ToDataFileRecord("Music"));
      DataFileRecord record1 = new DataFileRecord("Ambience");
      record1.AddRecord(this.AmbienceSetting.ToDataFileRecord("Setting"));
      DataFileRecord record2 = new DataFileRecord("Surroundings");
      record2.AddField("minDelay", (object) this._minDelay);
      record2.AddField("maxRandomDelay", (object) this._maxRandomDelay);
      foreach (StageSoundSource ambienceSurrounding in this.AmbienceSurroundings)
        record2.AddRecord(ambienceSurrounding.ToDataFileRecord("Surround"));
      record1.AddRecord(record2);
      dataFileRecord.AddRecord(record1);
      return dataFileRecord;
    }
  }
}
