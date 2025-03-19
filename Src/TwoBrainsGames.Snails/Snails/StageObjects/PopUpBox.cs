
// Type: TwoBrainsGames.Snails.StageObjects.PopUpBox
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;


namespace TwoBrainsGames.Snails.StageObjects
{
  internal class PopUpBox : StageObject, ISwitchable
  {
    private Box.BoxType _boxType;
    private int _scaleDirection;
    private float _scale;

    public PopUpBox()
      : base(StageObjectType.PopUpBox)
    {
    }

    public override void Initialize()
    {
      base.Initialize();
      this.EffectsBlender.Add((ITransformEffect) new ScaleEffect(new Vector2(1f, 1f), 0.5f, new Vector2(0.9f, 0.9f), true));
      this.SpriteAnimationActive = false;
      this._scaleDirection = 1;
      this._scale = 1f;
      this.SpriteAnimationActive = false;
      this.CurrentFrame = (int) this._boxType;
    }

    public override void LoadContent() => base.LoadContent();

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      this._scale += (float) (gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0) * (float) this._scaleDirection;
      if ((double) this._scale < 0.5)
      {
        this._scaleDirection = 1;
        this._scale = 0.5f;
      }
      else if ((double) this._scale > 1.0)
      {
        this._scaleDirection = -1;
        this._scale = 1f;
      }
      this.Scale = new Vector2(this._scale, this._scale);
    }

    public void SwitchOn()
    {
      Box andDeploy = Box.CreateAndDeploy(this._boxType, this.Position);
      foreach (StageObject linkedObject in this._linkedObjects)
      {
        if (linkedObject is ISwitchable)
          andDeploy.AddSwitchableObject((ISwitchable) linkedObject);
      }
      this.DisposeFromStage();
    }

    public void SwitchOff()
    {
    }

    public bool IsOn => false;

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord(context);
      if (context == ToDataFileRecordContext.StageSave)
        dataFileRecord.AddField("boxType", (object) (int) this._boxType);
      return dataFileRecord;
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this._boxType = (Box.BoxType) record.GetFieldValue<int>("boxType", 0);
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }
  }
}
