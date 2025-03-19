
// Type: TwoBrainsGames.Snails.StageObjects.InformationSign
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  internal class InformationSign : StageObject, ISnailsDataFileSerializable, IDataFileSerializable
  {
    public const string ID = "INFORMATION_SIGN";
    private const int BB_IDX_POOL_POSITION = 0;
    private const float MAX_ANGLE = 75f;
    private const float AMPLITUDE = 0.1f;
    private const float SPEED = 0.3f;
    private Sprite _poolSprite;
    private Sprite _signSprite;
    private float _poolRotation;
    private Vector2 _poolPosition;
    private int _direction;
    private float _angle;

    public static Dictionary<string, InformationSign.SignItem> SignItems { get; set; }

    public string SignId { get; set; }

    public InformationSign()
      : base(StageObjectType.InformationSign)
    {
    }

    public InformationSign(InformationSign other)
      : base((StageObject) other)
    {
      this.Copy((StageObject) other);
    }

    public override void Copy(StageObject other)
    {
      base.Copy(other);
      this.SignId = (other as InformationSign).SignId;
    }

    public override void LoadContent()
    {
      base.LoadContent();
      if (InformationSign.SignItems != null && this.SignId != null)
        this._signSprite = BrainGame.ResourceManager.GetSpriteTemporary(InformationSign.SignItems[this.SignId]._resource, InformationSign.SignItems[this.SignId]._sprite);
      this._poolSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/signs/Pool");
    }

    public override void Initialize()
    {
      base.Initialize();
      this._poolPosition = this.TransformSpriteFrameBB(0).ToBoundingSquare().Center;
      this._angle = (float) BrainGame.Rand.Next(150) - 75f;
      this._direction = 1;
    }

    public override string ToString() => this.SignId == null ? base.ToString() : this.SignId;

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      this._angle += (float) (Math.Cos((double) MathHelper.ToRadians(this._angle)) * (double) this._direction * 0.30000001192092896) * (float) gameTime.ElapsedGameTime.TotalMilliseconds;
      if ((double) Math.Abs(this._angle) >= 75.0)
      {
        this._angle = 75f * (float) this._direction;
        this._direction *= -1;
      }
      this._poolRotation = this._angle * 0.1f;
    }

    public override void Draw(bool shadow)
    {
      if (!shadow)
      {
        this._poolSprite.Draw(this._poolPosition, 0, this._poolRotation, SpriteEffects.None, Stage.CurrentStage.SpriteBatch);
        this._signSprite.Draw(this._poolPosition, 0, this._poolRotation, SpriteEffects.None, Stage.CurrentStage.SpriteBatch);
      }
      else
      {
        this._poolSprite.Draw(this._poolPosition + GenericConsts.ShadowDepth, 0, this._poolRotation, SpriteEffects.None, this.ShadowColor, 1f, Stage.CurrentStage.SpriteBatch);
        this._signSprite.Draw(this._poolPosition + GenericConsts.ShadowDepth, 0, this._poolRotation, SpriteEffects.None, this.ShadowColor, 1f, Stage.CurrentStage.SpriteBatch);
      }
      base.Draw(shadow);
    }

    public static InformationSign Create(StageData stageData, string signId)
    {
      InformationSign objectNoInitialize = (InformationSign) stageData.GetObjectNoInitialize("INFORMATION_SIGN");
      objectNoInitialize.SignId = signId;
      objectNoInitialize.LoadContent();
      objectNoInitialize.Initialize();
      return objectNoInitialize;
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this.SignId = record.GetFieldValue<string>("signId");
      DataFileRecordList dataFileRecordList = record.SelectRecords("Signs\\Sign");
      if (dataFileRecordList == null || dataFileRecordList.Count <= 0)
        return;
      InformationSign.SignItems = new Dictionary<string, InformationSign.SignItem>();
      foreach (DataFileRecord dataFileRecord in dataFileRecordList)
      {
        string fieldValue1 = dataFileRecord.GetFieldValue<string>("id");
        string fieldValue2 = dataFileRecord.GetFieldValue<string>("res");
        string fieldValue3 = dataFileRecord.GetFieldValue<string>("sprite");
        InformationSign.SignItems.Add(fieldValue1, new InformationSign.SignItem(fieldValue1, fieldValue2, fieldValue3));
      }
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord1 = base.ToDataFileRecord(context);
      dataFileRecord1.AddField("signId", (object) this.SignId);
      if (context == ToDataFileRecordContext.StageDataSave)
      {
        DataFileRecord dataFileRecord2 = dataFileRecord1.AddRecord("Signs");
        foreach (KeyValuePair<string, InformationSign.SignItem> signItem in InformationSign.SignItems)
        {
          DataFileRecord record = new DataFileRecord("Sign");
          record.AddField("id", (object) signItem.Key);
          record.AddField("res", (object) signItem.Value._resource);
          record.AddField("sprite", (object) signItem.Value._sprite);
          dataFileRecord2.AddRecord(record);
        }
      }
      return dataFileRecord1;
    }

    public struct SignItem(string id, string res, string sprite)
    {
      public string _id = id;
      public string _resource = res;
      public string _sprite = sprite;
    }
  }
}
