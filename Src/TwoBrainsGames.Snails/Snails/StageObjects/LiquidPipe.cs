
// Type: TwoBrainsGames.Snails.StageObjects.LiquidPipe
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  internal class LiquidPipe : StageObject, ISnailsDataFileSerializable, IDataFileSerializable
  {
    private List<LiquidPipe.PipePartSprite> _pipeSprites;
    private List<LiquidPipe.PipePart> _pipeParts;

    public string PipeString { get; set; }

    public LiquidPipe.PipeLinkType Terminator { get; set; }

    public LiquidPipe.PipeLinkType PumpAttachment { get; set; }

    public LiquidPipe()
      : base(StageObjectType.LiquidPipe)
    {
    }

    public override void Copy(StageObject other)
    {
      base.Copy(other);
      LiquidPipe liquidPipe = (LiquidPipe) other;
      this._pipeParts = liquidPipe._pipeParts;
      this.PumpAttachment = liquidPipe.PumpAttachment;
      this.Terminator = liquidPipe.Terminator;
    }

    public override void Initialize()
    {
      base.Initialize();
      this.BuildPipe();
    }

    private void BuildPipe()
    {
      this._pipeSprites = new List<LiquidPipe.PipePartSprite>();
      if (string.IsNullOrEmpty(this.PipeString))
        return;
      Vector2 position = this.Position;
      List<LiquidPipe.PipeSection> pipeString = LiquidPipe.PipeSection.ParsePipeString(this.PipeString);
      LiquidPipe.PipeLinkType prevLink = LiquidPipe.PipeLinkType.None;
      for (int index1 = 0; index1 < pipeString.Count; ++index1)
      {
        for (int index2 = 0; index2 < pipeString[index1]._quantity; ++index2)
        {
          LiquidPipe.PipePartSprite pipePartSprite = new LiquidPipe.PipePartSprite();
          pipePartSprite._position = position;
          LiquidPipe.PipeLinkType nextLink = pipeString[index1]._direction;
          if (index2 + 1 == pipeString[index1]._quantity)
            nextLink = index1 + 1 >= pipeString.Count ? LiquidPipe.PipeLinkType.None : pipeString[index1 + 1]._direction;
          LiquidPipe.PipePart pipePartByLink = this.GetPipePartByLink(prevLink, nextLink);
          pipePartSprite._frame = pipePartByLink._frame;
          this._pipeSprites.Add(pipePartSprite);
          switch (nextLink)
          {
            case LiquidPipe.PipeLinkType.Up:
              position -= new Vector2(0.0f, (float) this.Sprite.Frames[pipePartSprite._frame].Rect.Height);
              break;
            case LiquidPipe.PipeLinkType.Down:
              position += new Vector2(0.0f, (float) this.Sprite.Frames[pipePartSprite._frame].Rect.Height);
              break;
            case LiquidPipe.PipeLinkType.Left:
              position -= new Vector2((float) this.Sprite.Frames[pipePartSprite._frame].Rect.Width, 0.0f);
              break;
            case LiquidPipe.PipeLinkType.Right:
              position += new Vector2((float) this.Sprite.Frames[pipePartSprite._frame].Rect.Width, 0.0f);
              break;
          }
          switch (nextLink)
          {
            case LiquidPipe.PipeLinkType.Up:
              prevLink = LiquidPipe.PipeLinkType.Down;
              break;
            case LiquidPipe.PipeLinkType.Down:
              prevLink = LiquidPipe.PipeLinkType.Up;
              break;
            case LiquidPipe.PipeLinkType.Left:
              prevLink = LiquidPipe.PipeLinkType.Right;
              break;
            case LiquidPipe.PipeLinkType.Right:
              prevLink = LiquidPipe.PipeLinkType.Left;
              break;
          }
        }
      }
    }

    private LiquidPipe.PipePart GetPipePartByLink(
      LiquidPipe.PipeLinkType prevLink,
      LiquidPipe.PipeLinkType nextLink)
    {
      LiquidPipe.PipePart pipePartByLink = (LiquidPipe.PipePart) null;
      foreach (LiquidPipe.PipePart pipePart in this._pipeParts)
      {
        if (pipePart._links == (prevLink | nextLink))
        {
          pipePartByLink = pipePart;
          if (pipePartByLink._pumpAttachment == this.PumpAttachment && pipePartByLink.IsPumpAttachment)
          {
            if (prevLink == LiquidPipe.PipeLinkType.None)
              break;
          }
          if (pipePartByLink._terminator == this.Terminator && pipePartByLink.IsTerminator)
          {
            if (nextLink == LiquidPipe.PipeLinkType.None)
              break;
          }
        }
      }
      return pipePartByLink;
    }

    public override void Draw(bool shadow)
    {
      foreach (LiquidPipe.PipePartSprite pipeSprite in this._pipeSprites)
      {
        if (!shadow)
          this.Sprite.Draw(pipeSprite._position, pipeSprite._frame, this.BlendColor, Stage.CurrentStage.SpriteBatch);
        else
          this.Sprite.Draw(pipeSprite._position + GenericConsts.ShadowDepth, pipeSprite._frame, this.ShadowColor, Stage.CurrentStage.SpriteBatch);
      }
    }

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord(context);
      dataFileRecord.AddField("terminator", (object) this.Terminator.ToString());
      dataFileRecord.AddField("pumpAttachment", (object) this.PumpAttachment.ToString());
      dataFileRecord.AddField("pipeString", (object) this.PipeString);
      if (context == ToDataFileRecordContext.StageDataSave)
      {
        foreach (LiquidPipe.PipePart pipePart in this._pipeParts)
        {
          DataFileRecord record = new DataFileRecord("PipePart");
          record.AddField("frame", (object) pipePart._frame);
          record.AddField("links", (object) pipePart._links.ToString());
          record.AddField("pumpAttachment", (object) pipePart._pumpAttachment.ToString());
          record.AddField("terminator", (object) pipePart._terminator.ToString());
          dataFileRecord.AddRecord(record);
        }
      }
      return dataFileRecord;
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this.PipeString = record.GetFieldValue<string>("pipeString", this.PipeString);
      this.Terminator = (LiquidPipe.PipeLinkType) Enum.Parse(typeof (LiquidPipe.PipeLinkType), record.GetFieldValue<string>("terminator", LiquidPipe.PipeLinkType.None.ToString()), true);
      this.PumpAttachment = (LiquidPipe.PipeLinkType) Enum.Parse(typeof (LiquidPipe.PipeLinkType), record.GetFieldValue<string>("pumpAttachment", LiquidPipe.PipeLinkType.None.ToString()), true);
      DataFileRecordList dataFileRecordList = record.SelectRecords("PipePart");
      if (dataFileRecordList.Count <= 0)
        return;
      this._pipeParts = new List<LiquidPipe.PipePart>();
      foreach (DataFileRecord dataFileRecord in dataFileRecordList)
        this._pipeParts.Add(new LiquidPipe.PipePart()
        {
          _frame = dataFileRecord.GetFieldValue<int>("frame"),
          _links = (LiquidPipe.PipeLinkType) Enum.Parse(typeof (LiquidPipe.PipeLinkType), dataFileRecord.GetFieldValue<string>("links"), true),
          _pumpAttachment = (LiquidPipe.PipeLinkType) Enum.Parse(typeof (LiquidPipe.PipeLinkType), dataFileRecord.GetFieldValue<string>("pumpAttachment", LiquidPipe.PipeLinkType.None.ToString()), true),
          _terminator = (LiquidPipe.PipeLinkType) Enum.Parse(typeof (LiquidPipe.PipeLinkType), dataFileRecord.GetFieldValue<string>("terminator", LiquidPipe.PipeLinkType.None.ToString()), true)
        });
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    [Flags]
    public enum PipeLinkType
    {
      None = 0,
      Up = 1,
      Down = 2,
      Left = 4,
      Right = 8,
    }

    private struct PipeSection
    {
      public LiquidPipe.PipeLinkType _direction;
      public int _quantity;

      public static LiquidPipe.PipeSection Parse(string str)
      {
        string upper = str.Substring(0, 1).ToUpper();
        int int32 = Convert.ToInt32(str.Substring(1));
        LiquidPipe.PipeSection pipeSection = new LiquidPipe.PipeSection();
        if (upper == "R")
          pipeSection._direction = LiquidPipe.PipeLinkType.Right;
        if (upper == "L")
          pipeSection._direction = LiquidPipe.PipeLinkType.Left;
        if (upper == "U")
          pipeSection._direction = LiquidPipe.PipeLinkType.Up;
        if (upper == "D")
          pipeSection._direction = LiquidPipe.PipeLinkType.Down;
        pipeSection._quantity = int32;
        return pipeSection;
      }

      public static List<LiquidPipe.PipeSection> ParsePipeString(string str)
      {
        List<LiquidPipe.PipeSection> pipeString = new List<LiquidPipe.PipeSection>();
        string str1 = str;
        char[] chArray = new char[1]{ ';' };
        foreach (string str2 in str1.Split(chArray))
          pipeString.Add(LiquidPipe.PipeSection.Parse(str2));
        return pipeString;
      }
    }

    private class PipePart
    {
      public LiquidPipe.PipeLinkType _links;
      public int _frame;
      public LiquidPipe.PipeLinkType _pumpAttachment;
      public LiquidPipe.PipeLinkType _terminator;

      public bool IsPumpAttachment => this._pumpAttachment != LiquidPipe.PipeLinkType.None;

      public bool IsTerminator => this._terminator != LiquidPipe.PipeLinkType.None;
    }

    private struct PipePartSprite
    {
      public Vector2 _position;
      public int _frame;
    }
  }
}
