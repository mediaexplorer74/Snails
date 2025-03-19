
// Type: BrainAchievement
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.BrainEngine.Resources;


public class BrainAchievement : Object2D, IDataFileSerializable
{
  public bool ShowOnAppStore;
  public bool CanBeDisplayed = true;
  public int EventType;
  public int Difficulty;
  private Dictionary<LanguageCode, string> _description;
  public int Quantity;
  private string _difficultyRes;
  private string _ballonRes;
  private string _textFontRes;
  private string _playSoundRes;
  private Sprite _spriteDifficulty;
  private Sprite _spriteBallon;
  private TextFont _font;
  private Sample _bubbleOut;
  private string _achievementWonText;
  private Rectangle _rcBackground;
  private Vector2 _descPosition;
  private Vector2 _achievWonPosition;

  public string Description => this._description[BrainGame.CurrentLanguage];

  public Sprite Trophy => this._spriteDifficulty;

  public BrainAchievement(
    string difficultyRes,
    string ballonRes,
    string textFontRes,
    string playSoundRes)
  {
    this._difficultyRes = difficultyRes;
    this._ballonRes = ballonRes;
    this._textFontRes = textFontRes;
    this._playSoundRes = playSoundRes;
  }

  private string GetDifficultySprite()
  {
    string difficultySprite = string.Empty;
    switch (this.Difficulty)
    {
      case 1:
        difficultySprite = "AwardBronze";
        break;
      case 2:
        difficultySprite = "AwardSilver";
        break;
      case 3:
        difficultySprite = "AwardGold";
        break;
    }
    return difficultySprite;
  }

  internal void LoadContent()
  {
    string spriteResourceName = this._difficultyRes + "/" + this.GetDifficultySprite();
    this._bubbleOut = BrainGame.ResourceManager.GetSampleStatic(this._playSoundRes, (Object2D) this);
    this._font = BrainGame.ResourceManager.Load<TextFont>(this._textFontRes, ResourceManager.ResourceManagerCacheType.Static);
    this._spriteDifficulty = BrainGame.ResourceManager.GetSpriteStatic(spriteResourceName);
    if (this._ballonRes == null)
      return;
    this._spriteBallon = BrainGame.ResourceManager.GetSpriteStatic(this._ballonRes);
  }

  public override void InitFromDataFileRecord(DataFileRecord record)
  {
    this.EventType = record.GetFieldValue<int>("EventType");
    this.Difficulty = record.GetFieldValue<int>("Difficulty");
    this.Quantity = record.GetFieldValue<int>("Quantity", 0);
    this.ShowOnAppStore = record.GetFieldValue<bool>("ShowOnAppStore", false);
    DataFileRecordList dataFileRecordList = record.SelectRecords("Description");
    this._description = new Dictionary<LanguageCode, string>();
    foreach (DataFileRecord dataFileRecord in dataFileRecordList)
      this._description.Add((LanguageCode) Enum.Parse(typeof (LanguageCode), dataFileRecord.GetFieldValue<string>("language"), true), dataFileRecord.GetFieldValue<string>("text").Replace("%Quantity%", this.Quantity.ToString()));
  }

  public override DataFileRecord ToDataFileRecord()
  {
    DataFileRecord dataFileRecord = new DataFileRecord("Achievement");
    dataFileRecord.AddField("EventType", (object) this.EventType);
    dataFileRecord.AddField("Difficulty", (object) this.Difficulty);
    dataFileRecord.AddField("Quantity", (object) this.Quantity);
    dataFileRecord.AddField("Description", (object) this.Description);
    dataFileRecord.AddField("ShowOnAppStore", (object) this.ShowOnAppStore);
    foreach (KeyValuePair<LanguageCode, string> keyValuePair in this._description)
    {
      DataFileRecord record = new DataFileRecord("Description");
      record.AddField("language", (object) keyValuePair.Key.ToString());
      record.AddField("text", (object) keyValuePair.Value.ToString());
      dataFileRecord.AddRecord(record);
    }
    return dataFileRecord;
  }

  public override void Draw(SpriteBatch spriteBatch)
  {
    BrainGame.DrawRectangleFilled(spriteBatch, this._rcBackground, new Color(60, 60, 60, (int) byte.MaxValue));
    if (this._spriteBallon != null)
      this._spriteBallon.Draw(this.Position, spriteBatch);
    this._spriteDifficulty.Draw(this.Position + new Vector2(10f, 0.0f), spriteBatch);
    this._font.DrawString(spriteBatch, this._achievementWonText, this.Position + this._achievWonPosition, Vector2.One, Color.LightGreen);
    this._font.DrawString(spriteBatch, this.Description, this.Position + this._descPosition, Vector2.One, Color.White);
  }

  internal void Show()
  {
    this._bubbleOut.Play();
    this._achievementWonText = LanguageManager.GetString("MSG_ACHIEVEMENT_WON");
    this._rcBackground = new Rectangle((int) this.Position.X + 50, (int) this.Position.Y + 5, (int) Math.Max(this._font.MeasureString(this._achievementWonText), this._font.MeasureString(this.Description)) + 40, 60);
    this._achievWonPosition = new Vector2((float) this._spriteDifficulty.Width + 20f, 10f);
    this._descPosition = new Vector2((float) this._spriteDifficulty.Width + 20f, 30f);
  }

  internal void Hide() => this.CanBeDisplayed = false;
}
