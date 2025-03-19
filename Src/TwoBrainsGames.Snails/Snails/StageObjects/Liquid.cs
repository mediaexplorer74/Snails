
// Type: TwoBrainsGames.Snails.StageObjects.Liquid
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class Liquid : StageObject
  {
    private const int MARGINS = 10;
    public const float LIQUID_LEVEL_FULL = 1f;
    public const float LIQUID_LEVEL_EMPTY = 0.0f;
    protected Vector2 _size;
    protected int _signal = 1;
    protected double _elapsedTime;
    protected bool _isWaving;
    protected BoundingSquare _liquidAABB;
    protected Vector2 _sizeInPixels;
    protected float _liquidLevel;
    protected Rectangle _drawRectangle;
    private Color _liquidColor;
    private Color[] _colorsByTheme;
    private string _liquidColorString;
    private string _waveSpriteResId;
    private SpriteAnimation _waveAnimation;
    private Sprite _waveSprite;
    private Vector2 _wavesPosition;
    private Color _wavesBlendColor;
    private List<StageProp> _liquidSplashes;
    private bool _withSplashes;
    private string _splashPropId;
    private bool _killObjectsOnTouch;
    protected bool _allowCratesOnTop;

    private bool WithWaves => this._waveAnimation != null;

    public Vector2 Size
    {
      get => this._size;
      set
      {
        this._size = value;
        this.Resize();
      }
    }

    public override BoundingSquare QuadtreeCollisionBB => this._liquidAABB;

    public bool IsEmpty => (double) this._liquidLevel == 0.0;

    public bool IsFull => (double) this._liquidLevel == 1.0;

    public float LiquidLevel
    {
      get => this._liquidLevel;
      set => this._liquidLevel = value;
    }

    public Liquid(StageObjectType type)
      : base(type)
    {
      this._size = Vector2.One;
      this._liquidLevel = 1f;
      this._wavesBlendColor = Color.White;
      this._liquidSplashes = new List<StageProp>();
    }

    public Liquid(Liquid other)
      : base((StageObject) other)
    {
      this.Copy((StageObject) other);
    }

    public override void Copy(StageObject other)
    {
      base.Copy(other);
      Liquid liquid = other as Liquid;
      this._waveSpriteResId = liquid._waveSpriteResId;
      this._withSplashes = liquid._withSplashes;
      this._splashPropId = liquid._splashPropId;
      this._size = liquid._size;
      this.BlendColor = liquid.BlendColor;
      this._wavesBlendColor = liquid._wavesBlendColor;
      this._liquidColorString = liquid._liquidColorString;
      this._killObjectsOnTouch = liquid._killObjectsOnTouch;
      this._allowCratesOnTop = liquid._allowCratesOnTop;
    }

    public override void Initialize()
    {
      base.Initialize();
      this._colorsByTheme = new Color[4];
      if (this._liquidColorString != null)
      {
        string[] strArray = this._liquidColorString.Split(';');
        for (int index = 0; index < this._colorsByTheme.Length; ++index)
        {
          if (index < strArray.Length)
            this._colorsByTheme[index] = Parsers.ParseColor(strArray[index]);
        }
      }
      this._liquidColor = this._colorsByTheme[(int) Stage.CurrentStage.LevelStage.ThemeId];
      if (this._withSplashes)
        this._liquidSplashes = new List<StageProp>();
      this.Resize();
      Stage.CurrentStage.OnBeforeObjectsDraw += new Stage.StageDrawEventHandler(this.CurrentStage_OnBeforeObjectsDraw);
    }

    public override void LoadContent()
    {
      base.LoadContent();
      if (string.IsNullOrEmpty(this._waveSpriteResId))
        return;
      this._waveSprite = BrainGame.ResourceManager.GetSpriteTemporary(this._waveSpriteResId);
      this._waveAnimation = new SpriteAnimation(this._waveSprite);
      this._waveAnimation.BlendColor = this._wavesBlendColor;
    }

    protected virtual void Resize()
    {
      int num1 = 0;
      if (this.WithWaves)
        num1 = this._waveSprite.Frames[0].Height;
      float num2 = this._liquidLevel * this._sizeInPixels.Y;
      this._liquidAABB = new BoundingSquare(new Vector2(this.X - 10f, this.Y + (float) num1 + this._sizeInPixels.Y - num2), this._sizeInPixels.X + 20f, (float) ((double) num2 - (double) num1 + 10.0));
      this._drawRectangle = this._liquidAABB.ToRect();
      this._wavesPosition = new Vector2(this.Position.X, (float) (this._drawRectangle.Top - num1));
      if (this._withSplashes)
      {
        foreach (StageProp liquidSplash in this._liquidSplashes)
        {
          if (liquidSplash.IsVisible)
            liquidSplash.Position = new Vector2(liquidSplash.Position.X, this._liquidAABB.Top);
        }
      }
      if (this._allowCratesOnTop)
        return;
      this._crateCollisionBB = this._liquidAABB;
    }

    protected virtual void KillObject(StageObject stageObject)
    {
      stageObject.KillByTouchingDeadlyLiquid();
    }

    public override void OnCollide(IQuadtreeContainable obj, int listIdx)
    {
      StageObject stageObject = (StageObject) obj;
      if (stageObject.IgnoreLiquidCollisions || !stageObject.CheckCollisionWithLiquid(this) || stageObject.IsUnderLiquid)
        return;
      if (!this._killObjectsOnTouch)
      {
        stageObject.OnEnterLiquid(this);
        if (!stageObject.IsFalling || !this._withSplashes)
          return;
        this.AddSplashEffect(stageObject.Position);
      }
      else
      {
        if (!stageObject.CanDie)
          return;
        this.KillObject(stageObject);
      }
    }

    private void AddSplashEffect(Vector2 position)
    {
      StageProp stageProp = (StageProp) null;
      foreach (StageProp liquidSplash in this._liquidSplashes)
      {
        if (!liquidSplash.IsVisible)
        {
          stageProp = liquidSplash;
          break;
        }
      }
      if (stageProp == null)
      {
        stageProp = StageProp.Create(this._splashPropId);
        Stage.CurrentStage.AddObjectInRuntime((StageObject) stageProp);
      }
      stageProp.Position = new Vector2(position.X, this._liquidAABB.Top);
      stageProp.Show();
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      if (this.IsEmpty)
        return;
      if (this.WithWaves)
        this._waveAnimation.Update(gameTime);
      this.DoQuadtreeCollisions(0);
      this.DoQuadtreeCollisions(1);
    }

    private void Draw(bool shadow, Color blendColor, SpriteBatch spriteBatch)
    {
      if (this.WithWaves && !this.IsEmpty)
      {
        for (int index = 0; (double) index < (double) this.Size.X; ++index)
        {
          Vector2 wavesPosition = this._wavesPosition;
          wavesPosition.X += (float) (index * Stage.CurrentStage.Board.TileWidth);
          this._waveAnimation.BlendColor = blendColor;
          this._waveAnimation.Draw(wavesPosition, Stage.CurrentStage.SpriteBatch);
        }
      }
      BrainGame.DrawRectangleFilled(spriteBatch, this._drawRectangle, blendColor);
    }

    private void CurrentStage_OnBeforeObjectsDraw(bool shadow, SpriteBatch spriteBatch)
    {
      int a = (int) this._liquidColor.A;
    }

    public override void Draw(bool shadow)
    {
      if (!this.IsVisible)
        return;
      this.Draw(shadow, this.BlendColor, Stage.CurrentStage.SpriteBatch);
    }

    public void PumpLiquid(float quantity)
    {
      this._liquidLevel += quantity;
      if ((double) this._liquidLevel > 1.0)
        this._liquidLevel = 1f;
      if ((double) this._liquidLevel < 0.0)
        this._liquidLevel = 0.0f;
      this.Resize();
    }

    public float GetDepth(Vector2 point) => point.Y - this._liquidAABB.Top;

    public override bool CrateToolIsValid(BoundingSquare crateBs)
    {
      return this._allowCratesOnTop || !crateBs.Collides(this._crateCollisionBB);
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      int fieldValue1 = record.GetFieldValue<int>("sizeX", (int) this._size.X);
      int fieldValue2 = record.GetFieldValue<int>("sizeY", (int) this._size.Y);
      this._waveSpriteResId = record.GetFieldValue<string>("waveSpriteResId", this._waveSpriteResId);
      this._liquidLevel = record.GetFieldValue<float>("liquidLevel", this._liquidLevel);
      this._size = new Vector2((float) fieldValue1, (float) fieldValue2);
      this._sizeInPixels = new Vector2((float) (fieldValue1 * 60), (float) (fieldValue2 * 60));
      this._splashPropId = record.GetFieldValue<string>("splashPropId", this._splashPropId);
      this._withSplashes = !string.IsNullOrEmpty(this._splashPropId);
      this._wavesBlendColor = record.GetFieldValue<Color>("wavesBlendColor", this._wavesBlendColor);
      this._killObjectsOnTouch = record.GetFieldValue<bool>("killObjectsOnTouch", this._killObjectsOnTouch);
      this._liquidColorString = record.GetFieldValue<string>("liquidColor", this._liquidColorString);
      this._allowCratesOnTop = record.GetFieldValue<bool>("allowCrates", this._allowCratesOnTop);
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord(context);
      switch (context)
      {
        case ToDataFileRecordContext.StageDataSave:
          dataFileRecord.AddField("waveSpriteResId", (object) this._waveSpriteResId);
          dataFileRecord.AddField("splashPropId", (object) this._splashPropId);
          dataFileRecord.AddField("wavesBlendColor", (object) this._wavesBlendColor);
          dataFileRecord.AddField("liquidColor", (object) this._liquidColorString);
          dataFileRecord.AddField("killObjectsOnTouch", (object) this._killObjectsOnTouch);
          dataFileRecord.AddField("allowCrates", (object) this._allowCratesOnTop);
          break;
        case ToDataFileRecordContext.StageSave:
          dataFileRecord.RemoveField("color");
          break;
      }
      dataFileRecord.AddField("sizeX", (object) (int) this._size.X);
      dataFileRecord.AddField("sizeY", (object) (int) this._size.Y);
      dataFileRecord.AddField("liquidLevel", (object) this._liquidLevel);
      return dataFileRecord;
    }
  }
}
