
// Type: TwoBrainsGames.Snails.StageObjects.TutorialSign
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  internal class TutorialSign : StageObject, ICursorInteractable
  {
    private Sprite _signSprite;
    private Vector2 _signPosition;
    private BoundingCircle _bsSelectArea;
    private ScaleEffect _scaleEffect;

    public int[] TutorialTopics { get; private set; }

    public string TopicsString { get; set; }

    public TutorialSign()
      : base(StageObjectType.TutorialSign)
    {
    }

    public TutorialSign(TutorialSign other)
      : base((StageObject) other)
    {
      this.Copy((StageObject) other);
    }

    public override void Copy(StageObject other)
    {
      base.Copy(other);
      this.TopicsString = (other as TutorialSign).TopicsString;
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this._signSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "Sign");
    }

    public override void Initialize()
    {
      base.Initialize();
      this._signPosition = this.TransformSpriteFrameBB(0).GetCenter();
      this._scaleEffect = new ScaleEffect(new Vector2(1.1f, 1.1f), 0.5f, new Vector2(0.93f, 0.93f), true);
      this._bsSelectArea = this._signSprite._boundingSpheres[0].Transform(this._signPosition);
      if (string.IsNullOrEmpty(this.TopicsString))
        return;
      string[] strArray = this.TopicsString.Split(',');
      this.TutorialTopics = new int[strArray.Length];
      for (int index = 0; index < this.TutorialTopics.Length; ++index)
        this.TutorialTopics[index] = Convert.ToInt32(strArray[index]);
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      this._scaleEffect.Update(gameTime);
      if (!Stage.CurrentStage.Input.IsActionPressed || !this.QueryCursorInsideInteractingZone())
        return;
      Stage.CurrentStage.Cursor.SetInteractingObject((ICursorInteractable) this);
    }

    public override void Draw(bool shadow)
    {
      base.Draw(shadow);
      this._signSprite.Draw(this._signPosition, 0, 0.0f, SpriteEffects.None, Color.White, this._scaleEffect.Scale.X, Stage.CurrentStage.SpriteBatch);
    }

    private bool QueryCursorInsideInteractingZone()
    {
      return this._bsSelectArea.Contains(Stage.CurrentStage.Cursor.Position);
    }

    public StageCursor.CursorType QueryCursor() => StageCursor.CursorType.Select;

    public bool QueryInterating() => this.QueryCursorInsideInteractingZone();

    public void CursorActionPressed(Vector2 cursorPos)
    {
    }

    public void CursorActionReleased()
    {
    }

    public void CursorActionSelected()
    {
      if (this.TutorialTopics == null)
        return;
      Stage.CurrentStage.ShowTutorialTopics(this.TutorialTopics, true);
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this.TopicsString = record.GetFieldValue<string>("tutorialTopics");
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord(context);
      dataFileRecord.AddField("tutorialTopics", (object) this.TopicsString);
      return dataFileRecord;
    }
  }
}
