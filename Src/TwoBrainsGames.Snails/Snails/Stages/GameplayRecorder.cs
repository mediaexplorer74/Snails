
// Type: TwoBrainsGames.Snails.Stages.GameplayRecorder
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.Snails.Effects;
using TwoBrainsGames.Snails.Input;


namespace TwoBrainsGames.Snails.Stages
{
  public class GameplayRecorder : Object2D
  {
    public const string DEFAULT_EXTENSION = "SGD";
    public const string FILE_DESCRIPTION = "Snails Gameplay";
    private const string STAGE_GAMEPLAY_FILE_STAMP = "SNAILS_GAMEPLAY";
    private const string STAGE_GAMEPLAY_FILE_STAMP2 = "SNAILS_GAMEPLV2";
    private const string STAGE_GAMEPLAY_FILE_VERSION_1_1 = "V1.1";
    private const string STAGE_GAMEPLAY_FILE_VERSION_1_2 = "V1.2";
    private Stage _stage;
    private BlinkEffect _blink;
    private GameplayInput.GamePlayButtons _actionsToIgnore;
    private GameplayInput.GamePlayButtons _acceptedActionsWhilePlaying;

    private GameplayRecorder.RecorderState StateBeforePause { get; set; }

    private InputRecorder Recorder { get; set; }

    public GameplayRecorder.RecorderState State { get; private set; }

    public bool Enabled { get; set; }

    public bool IsRecording => this.State == GameplayRecorder.RecorderState.Recording;

    public bool IsPlaying => this.State == GameplayRecorder.RecorderState.Playing;

    public bool IsPaused => this.State == GameplayRecorder.RecorderState.Paused;

    public GameplayRecorder(Stage stage)
    {
      this._stage = stage;
      this._stage.Input.OnAfterUpdate += new GameplayInput.InputEvent(this.Input_OnAfterUpdate);
    }

    public void Initialize()
    {
      this.SpriteAnimationActive = false;
      this._position = new Vector2(50f, 50f);
      this._blink = new BlinkEffect(500.0, 500.0);
      this._blink.UseRealTime = true;
      this.Recorder = new InputRecorder();
      this._actionsToIgnore = GameplayInput.GamePlayButtons.Pause;
      this._acceptedActionsWhilePlaying = GameplayInput.GamePlayButtons.TimeWarp | GameplayInput.GamePlayButtons.Pause;
    }

    public void LoadContent()
    {
      this.Sprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", nameof (GameplayRecorder));
    }

    private void Input_OnAfterUpdate(
      GameplayInput gameplayInput,
      BrainGameTime gameTime,
      out Vector2 newMotionPosition)
    {
      newMotionPosition = gameplayInput.MotionPosition;
      if (!this.Enabled)
        return;
      this.Recorder.Update(gameTime, (ulong) (gameplayInput.GameButtons & ~this._actionsToIgnore), gameplayInput.MotionPosition);
      if (this.State != GameplayRecorder.RecorderState.Playing || this.Recorder.CurrentItem == null)
        return;
      GameplayInput.GamePlayButtons gamePlayButtons = gameplayInput.GameButtons & this._acceptedActionsWhilePlaying;
      gameplayInput.GameButtons = (GameplayInput.GamePlayButtons) this.Recorder.CurrentItem._inputAction | gamePlayButtons;
      newMotionPosition = this.Recorder.CurrentItem._position;
    }

    public override void Update(BrainGameTime gameTime)
    {
      if (!this.Enabled)
        return;
      base.Update(gameTime);
      this._blink.Update(gameTime);
      this.CurrentFrame = (int) this.State;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      if (!this.Enabled || !this._blink.Visible)
        return;
      base.Draw(spriteBatch);
    }

    public void StartRecording()
    {
      this.Recorder.StartRecording();
      this.State = GameplayRecorder.RecorderState.Recording;
    }

    public void Play()
    {
      this.Recorder.PlayBack();
      this.State = GameplayRecorder.RecorderState.Playing;
    }

    public void Stop()
    {
      this.Recorder.Stop();
      this.State = GameplayRecorder.RecorderState.Stopped;
    }

    public void Pause()
    {
      this.StateBeforePause = this.State;
      this.State = GameplayRecorder.RecorderState.Paused;
    }

    public void Resume() => this.State = this.StateBeforePause;

    public void Save(string filename, string description)
    {
      BinaryWriter writeBinary = new BinaryWriter((Stream) new FileStream(filename, FileMode.Create));
      try
      {
        writeBinary.Write("SNAILS_GAMEPLV2");
        writeBinary.Write("V1.2");
        writeBinary.Write(this._stage.LevelStage.StageKey);
        writeBinary.Write(this._stage.BuildNr);
        writeBinary.Write(description);
        GameplayRecorder.Write(writeBinary, DateTime.Now);
        GameplayRecorder.Write(writeBinary, this._stage.StartupCameraPOI);
        GameplayRecorder.Write(writeBinary, this._stage.Stats.TimeTaken);
        writeBinary.Write((int) this._stage.Stats.MedalWon);
        writeBinary.Write(this._stage.Stats.TotalScore);
        writeBinary.Write(this.Recorder.InputStream.ItemsCount);
        for (int key = 0; key < this.Recorder.InputStream.ItemsCount; ++key)
        {
          writeBinary.Write(this.Recorder.InputStream[key]._inputAction);
          writeBinary.Write(this.Recorder.InputStream[key]._milliseconds);
          writeBinary.Write((double) this.Recorder.InputStream[key]._position.X);
          writeBinary.Write((double) this.Recorder.InputStream[key]._position.Y);
        }
      }
      finally
      {
         writeBinary?.Flush();
         writeBinary?.Dispose();
      }
    }

    public void Load(string filename)
    {
      FileStream input = new FileStream(filename, FileMode.Open);
      BinaryReader binaryReader = new BinaryReader((Stream) input);
      try
      {
        GameplayRecorder.SnailsGameplayRecord snailsGameplayRecord = GameplayRecorder.ReadHeaderFromStream(binaryReader);
        if (snailsGameplayRecord.StageKey != this._stage.LevelStage.StageKey)
          throw new SnailsException("Gameplay storage file is not from the current stage.");
        int num = binaryReader.ReadInt32();
        for (int index = 0; index < num; ++index)
        {
          ulong action = binaryReader.ReadUInt64();
          double milliseconds = binaryReader.ReadDouble();
          Vector2 position = new Vector2((float) binaryReader.ReadDouble(), (float) binaryReader.ReadDouble());
          this.Recorder.InputStream.Append(milliseconds, action, position);
        }
        if (!(snailsGameplayRecord.StageStartupPosition != Vector2.Zero))
          return;
        this._stage.Camera.MoveTo(snailsGameplayRecord.StageStartupPosition);
      }
      finally
      {
        input?.Flush();
        input?.Dispose();
        binaryReader?.Dispose();
      }
    }

    private static GameplayRecorder.SnailsGameplayRecord ReadHeaderFromStream_V1_1(
      BinaryReader binaryReader)
    {
      return new GameplayRecorder.SnailsGameplayRecord()
      {
        Version = "V1.1",
        StageKey = binaryReader.ReadString(),
        BuildNr = binaryReader.ReadInt32(),
        Description = binaryReader.ReadString(),
        TimeTaken = GameplayRecorder.ReadTimeSpan(binaryReader)
      };
    }

    private static GameplayRecorder.SnailsGameplayRecord ReadHeaderFromStream_V1_2(
      BinaryReader binaryReader)
    {
      return new GameplayRecorder.SnailsGameplayRecord()
      {
        Version = "V1.2",
        StageKey = binaryReader.ReadString(),
        BuildNr = binaryReader.ReadInt32(),
        Description = binaryReader.ReadString(),
        FileDate = GameplayRecorder.ReadDateTime(binaryReader),
        StageStartupPosition = GameplayRecorder.ReadVector2(binaryReader),
        TimeTaken = GameplayRecorder.ReadTimeSpan(binaryReader),
        MedalWon = (MedalType) binaryReader.ReadInt32(),
        TotalScore = binaryReader.ReadInt32()
      };
    }

    private static GameplayRecorder.SnailsGameplayRecord ReadHeaderFromStream(
      BinaryReader binaryReader)
    {
      GameplayRecorder.SnailsGameplayRecord snailsGameplayRecord = new GameplayRecorder.SnailsGameplayRecord();
      string str = binaryReader.ReadString();
      if (str != "SNAILS_GAMEPLAY" && str != "SNAILS_GAMEPLV2")
        throw new SnailsException("Gameplay storage file is invalid.");
      if (str == "SNAILS_GAMEPLV2")
      {
        switch (binaryReader.ReadString())
        {
          case "V1.1":
            return GameplayRecorder.ReadHeaderFromStream_V1_1(binaryReader);
          case "V1.2":
            return GameplayRecorder.ReadHeaderFromStream_V1_2(binaryReader);
        }
      }
      snailsGameplayRecord.StageKey = binaryReader.ReadString();
      snailsGameplayRecord.Description = binaryReader.ReadString();
      binaryReader.ReadInt32();
      binaryReader.ReadInt32();
      binaryReader.ReadInt32();
      binaryReader.ReadInt32();
      return snailsGameplayRecord;
    }

    private static GameplayRecorder.SnailsGameplayRecord ReadHeader(string filename)
    {
      FileStream input = new FileStream(filename, FileMode.Open);
      BinaryReader binaryReader = new BinaryReader((Stream) input);
      try
      {
        GameplayRecorder.SnailsGameplayRecord snailsGameplayRecord = GameplayRecorder.ReadHeaderFromStream(binaryReader);
        snailsGameplayRecord.Filename = filename;
        return snailsGameplayRecord;
      }
      finally
      {
        input?.Flush();
        input?.Dispose();
        binaryReader?.Dispose();
      }
    }

    public static List<GameplayRecorder.SnailsGameplayRecord> EnumerateFiles(
      string path,
      string stageKey)
    {
      List<GameplayRecorder.SnailsGameplayRecord> snailsGameplayRecordList = new List<GameplayRecorder.SnailsGameplayRecord>();
      if (Directory.Exists(path))
      {
        foreach (string file in Directory.GetFiles(path, "*.SGD"))
        {
          GameplayRecorder.SnailsGameplayRecord snailsGameplayRecord = GameplayRecorder.ReadHeader(file);
          if (snailsGameplayRecord.StageKey == stageKey)
            snailsGameplayRecordList.Add(snailsGameplayRecord);
        }
      }
      return snailsGameplayRecordList;
    }

    private static void Write(BinaryWriter writeBinary, DateTime dateTime)
    {
      writeBinary.Write(dateTime.Year);
      writeBinary.Write(dateTime.Month);
      writeBinary.Write(dateTime.Day);
      writeBinary.Write(dateTime.Hour);
      writeBinary.Write(dateTime.Minute);
      writeBinary.Write(dateTime.Second);
      writeBinary.Write(dateTime.Millisecond);
    }

    private static void Write(BinaryWriter writeBinary, TimeSpan time)
    {
      writeBinary.Write(time.Hours);
      writeBinary.Write(time.Minutes);
      writeBinary.Write(time.Seconds);
      writeBinary.Write(time.Milliseconds);
    }

    private static void Write(BinaryWriter writeBinary, Vector2 pos)
    {
      writeBinary.Write((double) pos.X);
      writeBinary.Write((double) pos.Y);
    }

    private static TimeSpan ReadTimeSpan(BinaryReader reader)
    {
      return new TimeSpan(0, reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
    }

    private static DateTime ReadDateTime(BinaryReader reader)
    {
      return new DateTime(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
    }

    private static Vector2 ReadVector2(BinaryReader reader)
    {
      return new Vector2((float) reader.ReadDouble(), (float) reader.ReadDouble());
    }

    public enum RecorderState
    {
      Stopped,
      Recording,
      Playing,
      Paused,
    }

    public class SnailsGameplayRecord
    {
      public string Version { get; set; }

      public string Filename { get; set; }

      public string Description { get; set; }

      public int BuildNr { get; set; }

      public string StageKey { get; set; }

      public TimeSpan TimeTaken { get; set; }

      public int TotalScore { get; set; }

      public MedalType MedalWon { get; set; }

      public DateTime FileDate { get; set; }

      public Vector2 StageStartupPosition { get; set; }

      public override string ToString()
      {
        return this.Description != null ? string.Format("{0} [Build:{1}]", (object) this.Description, (object) this.BuildNr) : "NO DESCRIPTION";
      }
    }
  }
}
