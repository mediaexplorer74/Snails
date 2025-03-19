
// Type: TwoBrainsGames.Snails.Tutorials.TutorialTopic
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.Effects;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.Snails.ToolObjects;


namespace TwoBrainsGames.Snails.Tutorials
{
  public class TutorialTopic : Object2D, ISnailsDataFileSerializable, IDataFileSerializable
  {
    private const float LINE_SPACING = 22f;
    private const float DOTS_DISTANCE = 30f;
    private const float MINIMUM_DISPLAY_TIME = 2000f;
    private const int TEXT_PLACEMENT_BB_IDX = 1;
    public TextFont _font;
    private TutorialTopic.TopicState _state;
    private double _ellapsedShowDelayTime;
    private double _showDelay;
    private List<TutorialLine> _lines;
    private Rectangle _topicRect;
    private Tutorial _tutorialParent;
    private double _ellapsedDisplayTime;
    private double _displayTime;
    private ToolObjectType _pointingToolId;
    private TrembleEffect _trembleEffect;
    private HooverEffect _topicEffect;
    private SquashEffect _showEffect;
    private string _ballonSprite;
    public string _stageId;
    private int _id;
    public Dictionary<LanguageCode, List<TutorialLine>> _stCodeLines;
    public bool _inQueue;
    public Vector2 _scale;
    private string _continueString;
    private Vector2 _clickToContinuePosition;
    private Vector2 _xboxButtonPosition;
    private string _posStr;

    public bool IsClosed => this._state == TutorialTopic.TopicState.Closed;

    public bool IsOpen
    {
      get
      {
        return this._state == TutorialTopic.TopicState.Opening || this._state == TutorialTopic.TopicState.Open;
      }
    }

    private string FontName { get; set; }

    private string PictureResource { get; set; }

    private Sprite PictureSprite { get; set; }

    public bool AlwaysUnlockedInHelp { get; private set; }

    public BrainSettings.PlaformType Platforms { get; private set; }

    private bool IsStageCursorInside
    {
      get
      {
        Vector2 motionPosition = Stage.CurrentStage.Input.MotionPosition;
        return this._topicRect.Contains((int) motionPosition.X, (int) motionPosition.Y);
      }
    }

    public int TopicId => this._id;

    public Vector2 Size
    {
      get => new Vector2((float) this._topicRect.Width, (float) this._topicRect.Height);
    }

    public int Number
    {
      get
      {
        if (this._tutorialParent == null)
          return 0;
        for (int index = 0; index < this._tutorialParent.Topics.Count; ++index)
        {
          if (this._tutorialParent.Topics[index] == this)
            return index + 1;
        }
        return 0;
      }
    }

    public TutorialTopic(Tutorial tutorialParent)
    {
      this._tutorialParent = tutorialParent;
      this._state = TutorialTopic.TopicState.Closed;
      this._scale = new Vector2(1f, 1f);
    }

    public static TutorialTopic FromDataFileRecord(DataFileRecord record, Tutorial tutorialParent)
    {
      TutorialTopic tutorialTopic = new TutorialTopic(tutorialParent);
      tutorialTopic.InitFromDataFileRecord(record);
      return tutorialTopic;
    }

    public void LoadContent()
    {
      this._lines = new List<TutorialLine>();
      foreach (TutorialLine tutorialLine in this._stCodeLines[BrainGame.CurrentLanguage])
      {
        tutorialLine.ParseStCode();
        this._lines.Add(tutorialLine);
      }
      this._displayTime = 2000.0;
      this._font = BrainGame.ResourceManager.Load<TextFont>(this.FontName, ResourceManager.ResourceManagerCacheType.Static);
      this.Sprite = BrainGame.ResourceManager.GetSprite(this._ballonSprite, "TUTORIAL");
      foreach (TutorialLine line in this._lines)
      {
        foreach (TutorialItem tutorialItem in line.Items)
        {
          tutorialItem._parentTopic = this;
          tutorialItem.LoadContent();
        }
      }
      foreach (TutorialLine line in this._lines)
      {
        foreach (TutorialItem tutorialItem in line.Items)
          tutorialItem._parentTopic = this;
      }
      this._trembleEffect = new TrembleEffect();
      this._topicEffect = new HooverEffect(0.015f, 0.1f, 0.0f);
      this._continueString = LanguageManager.GetString("MSG_CLOSE_TUTORIAL");
      if (this.PictureResource == null)
        return;
      this.PictureSprite = BrainGame.ResourceManager.GetSprite(this.PictureResource, "TUTORIAL");
    }

    public void UpdatePositions()
    {
      this._topicRect = new Rectangle((int) ((double) this.Position.X + (double) this.Sprite.BoundingBox.Left), (int) ((double) this.Position.Y + (double) this.Sprite.BoundingBox.Top), (int) this.Sprite.BoundingBox.Width, (int) this.Sprite.BoundingBox.Height);
      Vector2 vector2 = new Vector2(0.0f, (float) ((double) this.Sprite.BoundingBoxes[1].Height / 2.0 - (double) this.ComputeTopicHeight() / 2.0));
      foreach (TutorialLine line in this._lines)
      {
        float num = 0.0f;
        foreach (TutorialItem tutorialItem in line.Items)
        {
          tutorialItem.LoadContent();
          tutorialItem.Position = new Vector2((float) (int) vector2.X, (float) (int) vector2.Y);
          vector2 += new Vector2(tutorialItem.GetWidth(), 0.0f);
          this._displayTime += (double) tutorialItem._displayTime;
        }
        float x = (float) (this._topicRect.Width / 2) - vector2.X / 2f;
        foreach (TutorialItem tutorialItem in line.Items)
          tutorialItem.Position += new Vector2(x, 0.0f);
        num = vector2.X;
        vector2 = new Vector2(0.0f, vector2.Y + 22f);
      }
      this._clickToContinuePosition = new Vector2((float) (this._topicRect.Width / 2) - this._font.MeasureString(this._continueString) / 2f, (float) this._topicRect.Height - 5f) - this.Sprite.Offset;
      this._xboxButtonPosition = new Vector2(15f, (float) this._topicRect.Height - 20f) - this.Sprite.Offset;
    }

    public void Show()
    {
      this.LoadContent();
      this.Position = new Vector2((float) (BrainGame.ScreenWidth / 2), (float) (BrainGame.ScreenHeight / 2));
      this.UpdatePositions();
      this._state = TutorialTopic.TopicState.Starting;
      this._scale = new Vector2(1f, 1f);
      this._showEffect = new SquashEffect(0.85f, 4f, 0.02f, Color.White, new Vector2(1f, 1f));
      this._trembleEffect.Reset();
      this.EffectsBlender.Clear();
    }

    public override void Update(BrainGameTime gameTime)
    {
      TimeSpan elapsedGameTime = gameTime.ElapsedGameTime;
      gameTime.ElapsedGameTime = gameTime.ElapsedRealTime;
      base.Update(gameTime);
      switch (this._state)
      {
        case TutorialTopic.TopicState.Starting:
          this._ellapsedShowDelayTime += gameTime.ElapsedRealTime.TotalMilliseconds;
          if (this._ellapsedShowDelayTime > this._showDelay)
          {
            this._state = TutorialTopic.TopicState.BeforeOpen;
            this.EffectsBlender.Add((ITransformEffect) this._showEffect);
            break;
          }
          break;
        case TutorialTopic.TopicState.Opening:
          this._scale = this._showEffect.Scale;
          if (this._showEffect.Ended)
          {
            this._state = TutorialTopic.TopicState.Open;
            this._scale = new Vector2(1f, 1f);
            this.EffectsBlender.Clear();
            if (this._topicEffect != null)
            {
              this.EffectsBlender.Add((ITransformEffect) this._topicEffect);
              break;
            }
            break;
          }
          break;
        case TutorialTopic.TopicState.Open:
          if (this._topicEffect != null)
          {
            this.Rotation = this._topicEffect.Rotation;
            TutorialTopic tutorialTopic = this;
            tutorialTopic.Position = tutorialTopic.Position + this._topicEffect.PositionV2;
          }
          if (!Game1.GameSettings.PauseInTutorial)
          {
            this._ellapsedDisplayTime += gameTime.ElapsedRealTime.TotalMilliseconds;
            if (this._ellapsedDisplayTime > this._displayTime)
            {
              this.Close();
              break;
            }
            break;
          }
          if (Stage.CurrentStage.Input.CloseTutorialSelected || Stage.CurrentStage.Input.IsActionClicked && this.IsStageCursorInside)
          {
            Stage.CurrentStage.Input.Reset();
            this.Close();
            break;
          }
          break;
        case TutorialTopic.TopicState.BeforeOpen:
          Stage.CurrentStage.TutorialTopicOpened();
          this._state = TutorialTopic.TopicState.Opening;
          break;
      }
      gameTime.ElapsedGameTime = elapsedGameTime;
    }

    public void DrawTopicBallon(SpriteBatch spriteBatch, Color color)
    {
      this.Sprite.Draw(this.Position, 0, this.Rotation, Vector2.Zero, this._scale.X, this._scale.Y, color, spriteBatch);
    }

    public void DrawTopic(SpriteBatch spriteBatch, Color color)
    {
      this.DrawTopicBallon(spriteBatch, color);
      foreach (TutorialLine line in this._lines)
      {
        foreach (TutorialItem tutorialItem in line.Items)
          tutorialItem.Draw(new Vector2((float) (int) this.Position.X, (float) (int) this.Position.Y) + this.Sprite.BoundingBox.UpperLeft, color, spriteBatch);
      }
      if (this.PictureSprite == null)
        return;
      this.PictureSprite.Draw(this.Position + new Vector2(0.0f, 20f), 0, 0.0f, Vector2.Zero, this._scale.X, this._scale.Y, color, spriteBatch);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      if (this._state != TutorialTopic.TopicState.Open && this._state != TutorialTopic.TopicState.Opening)
        return;
      this.DrawTopic(spriteBatch, Color.White);
      if (!Game1.GameSettings.ShowCloseTutorialMessage)
        return;
      this._font.DrawString(spriteBatch, this._continueString, this._clickToContinuePosition + this.Position, this._scale, Color.Black);
    }

    private float ComputeTopicHeight() => (float) this._lines.Count * 23f;

    private void Close()
    {
      this._state = TutorialTopic.TopicState.Closed;
      Stage.CurrentStage.TutorialTopicClosed();
    }

    private Vector2 getPosition(string strPos)
    {
      Vector2 zero = Vector2.Zero;
      string[] strArray = strPos.Split(',');
      zero.X = (float) int.Parse(strArray[0]);
      zero.Y = (float) int.Parse(strArray[1]);
      return zero;
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      this._id = record.GetFieldValue<int>("id");
      this._showDelay = record.GetFieldValue<double>("showDelay");
      this._pointingToolId = (ToolObjectType) Enum.Parse(typeof (ToolObjectType), record.GetFieldValue<string>("pointingToolId", ToolObjectType.None.ToString()), true);
      this._stageId = record.GetFieldValue<string>("stageId");
      this._ballonSprite = record.GetFieldValue<string>("ballonSprite", "spriteset/Tutorial/BalloonMedium");
      this.FontName = record.GetFieldValue<string>("font", "fonts/notebook");
      this.AlwaysUnlockedInHelp = record.GetFieldValue<bool>("alwaysUnlockedInHelp", false);
      this.Platforms = (BrainSettings.PlaformType) Enum.Parse(typeof (BrainSettings.PlaformType), record.GetFieldValue<string>("platforms", BrainSettings.PlaformType.All.ToString()), false);
      DataFileRecordList dataFileRecordList1 = record.SelectRecords("language");
      this._stCodeLines = new Dictionary<LanguageCode, List<TutorialLine>>();
      foreach (DataFileRecord dataFileRecord in dataFileRecordList1)
      {
        LanguageCode key = (LanguageCode) Enum.Parse(typeof (LanguageCode), dataFileRecord.GetFieldValue<string>("id"), true);
        DataFileRecordList dataFileRecordList2 = dataFileRecord.SelectRecords("line");
        this._stCodeLines.Add(key, new List<TutorialLine>());
        foreach (DataFileRecord record1 in dataFileRecordList2)
          this._stCodeLines[key].Add(TutorialLine.CreateFromDataFileRecord(record1));
      }
      this._posStr = record.GetFieldValue<string>("position", (string) null);
      this.PictureResource = record.GetFieldValue<string>("picture", (string) null);
    }

    public DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = new DataFileRecord(nameof (TutorialTopic));
      dataFileRecord.AddField("id", (object) this._id);
      foreach (KeyValuePair<LanguageCode, List<TutorialLine>> stCodeLine in this._stCodeLines)
      {
        DataFileRecord record = new DataFileRecord("language");
        record.AddField("id", (object) stCodeLine.Key.ToString());
        foreach (TutorialLine tutorialLine in stCodeLine.Value)
          record.AddRecord(tutorialLine.ToDataFileRecord());
        dataFileRecord.AddRecord(record);
      }
      dataFileRecord.AddField("showDelay", (object) this._showDelay);
      dataFileRecord.AddField("pointingToolId", (object) this._pointingToolId.ToString());
      dataFileRecord.AddField("position", (object) this._posStr);
      dataFileRecord.AddField("stageId", (object) this._stageId);
      dataFileRecord.AddField("ballonSprite", (object) this._ballonSprite);
      dataFileRecord.AddField("font", (object) this.FontName);
      dataFileRecord.AddField("alwaysUnlockedInHelp", (object) this.AlwaysUnlockedInHelp);
      dataFileRecord.AddField("picture", (object) this.PictureResource);
      dataFileRecord.AddField("platforms", (object) this.Platforms.ToString());
      return dataFileRecord;
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    protected enum TopicState
    {
      Starting,
      Closed,
      Opening,
      Open,
      Closing,
      BeforeOpen,
    }
  }
}
