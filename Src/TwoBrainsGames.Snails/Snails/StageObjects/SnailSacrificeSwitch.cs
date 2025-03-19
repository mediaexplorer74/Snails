
// Type: TwoBrainsGames.Snails.StageObjects.SnailSacrificeSwitch
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class SnailSacrificeSwitch : Switch
  {
    public const string ID = "SNAIL_SACRIFICE";
    private const int BB_IDX_TUBE = 1;
    private const int BB_IDX_DISCARD = 2;
    private const int BB_IDX_SUCK_TUBE1 = 3;
    private const int BB_IDX_SUCK_TUBE2 = 4;
    private const int BB_IDX_SUCK_TUBE3 = 5;
    private const int BB_IDX_SUCK_TUBE4 = 6;
    private const int BB_IDX_SUCK_TUBE5 = 7;
    private const int BB_IDX_SUCK = 8;
    private const int BB_IDX_BUTTONS = 9;
    private const int BB_IDX_FLOW = 10;
    private const int BB_IDX_STORAGE = 11;
    private const int BB_IDX_SOOP = 12;
    private const int BB_IDX_RED_LIGHT = 13;
    private const int BB_IDX_GREEN_LIGHT = 14;
    private const int BB_IDX_SNAIL = 15;
    private const int BB_IDX_SHELL = 16;
    private const int BB_IDX_SNAIL_COLLISION_RIGHT = 17;
    private const int BB_IDX_SNAIL_COLLISION_LEFT = 18;
    private const int BB_IDX_SNAIL_COUNTER = 19;
    private const int SOOP_HEIGHT_PER_SNAIL = 7;
    private int _numSacrifices;
    private string _numSacrificesString;
    private SpriteAnimationQueue _animations;
    private SpriteAnimationQueueItem _soopAnimation;
    private SpriteAnimationQueueItem _snailAnimation;
    private Vector2 _storagePosition;
    private Sprite _storageSprite;
    private BoundingSquare _bsSoop;
    private Rectangle _rcSoop;
    private Sprite _soopSprite;
    private Sprite _redLightSprite;
    private Vector2 _redLightPosition;
    private Sprite _greenLightSprite;
    private Vector2 _greenLightPosition;
    private BoundingSquare _bsSnailCollisionRight;
    private BoundingSquare _bsSnailCollisionLeft;
    private Snail _currentSnailSooped;
    private TextFont _font;
    private Rectangle _rcCounter;
    private SpriteAnimation _onTextAnimation;
    private Sample _switchSound;

    public int SnailsToSacrifice { get; set; }

    private bool IsSuckingSnail => this._animations.Active;

    public SnailSacrificeSwitch()
      : base(StageObjectType.SnailSacrifice)
    {
    }

    public override void LoadContent()
    {
      base.LoadContent();
      Vector2 upperLeft1 = this.Sprite.BoundingBoxes[1].UpperLeft;
      Vector2 upperLeft2 = this.Sprite.BoundingBoxes[2].UpperLeft;
      Vector2 upperLeft3 = this.Sprite.BoundingBoxes[3].UpperLeft;
      Vector2 upperLeft4 = this.Sprite.BoundingBoxes[4].UpperLeft;
      Vector2 upperLeft5 = this.Sprite.BoundingBoxes[5].UpperLeft;
      Vector2 upperLeft6 = this.Sprite.BoundingBoxes[6].UpperLeft;
      Vector2 upperLeft7 = this.Sprite.BoundingBoxes[7].UpperLeft;
      Vector2 upperLeft8 = this.Sprite.BoundingBoxes[8].UpperLeft;
      Vector2 upperLeft9 = this.Sprite.BoundingBoxes[15].UpperLeft;
      Vector2 upperLeft10 = this.Sprite.BoundingBoxes[9].UpperLeft;
      Vector2 upperLeft11 = this.Sprite.BoundingBoxes[10].UpperLeft;
      this._animations = new SpriteAnimationQueue(false, false);
      this._snailAnimation = this._animations.AddItem(new SpriteAnimation("spriteset/snails-switch/Snail", "__TEMPORARY__"), upperLeft9, false, false);
      this._animations.AddItem(new SpriteAnimation("spriteset/snails-switch/SnailSucking", "__TEMPORARY__"), upperLeft8, true, true, "sfx/objects/snail-switch-suck", (Object2D) this);
      this._animations.AddItem(new SpriteAnimation("spriteset/snails-switch/SuckTube1", "__TEMPORARY__"), upperLeft3, true, true);
      this._animations.AddItem(new SpriteAnimation("spriteset/snails-switch/SuckTube2", "__TEMPORARY__"), upperLeft4, true, true);
      this._animations.AddItem(new SpriteAnimation("spriteset/snails-switch/SuckTube3", "__TEMPORARY__"), upperLeft5, true, true);
      this._animations.AddItem(new SpriteAnimation("spriteset/snails-switch/SuckTube4", "__TEMPORARY__"), upperLeft6, true, true);
      this._animations.AddItem(new SpriteAnimation("spriteset/snails-switch/SuckTube5", "__TEMPORARY__"), upperLeft7, true, true);
      this._animations.AddItem(new SpriteAnimation("spriteset/snails-switch/Lights", "__TEMPORARY__"), upperLeft10, true, true, "sfx/objects/snail-switch-processing", (Object2D) this, new SpriteAnimationQueueItem.LastFrameCallBackDelegate(this.ButtonsAnimationEnded));
      this._animations.AddItem(new SpriteAnimation("spriteset/snails-switch/ShellDiscard", "__TEMPORARY__"), upperLeft2, true, false, "sfx/objects/snail-throw", (Object2D) this);
      this._animations.AddItem(new SpriteAnimation("spriteset/snails-switch/Tube", "__TEMPORARY__"), upperLeft1, true, false);
      this._animations.AddItem(new SpriteAnimation("spriteset/snails-switch/Flow", "__TEMPORARY__"), upperLeft11, false, false, "sfx/objects/snail-switch-soup", (Object2D) this);
      this._soopAnimation = this._animations.AddItem(new SpriteAnimation("spriteset/snails-switch/SoopWaves", "__TEMPORARY__"), Vector2.Zero, false, true);
      this._animations.Reset();
      this._animations.OnAnimationEnded += new SpriteAnimationQueue.AnimationEndedHandler(this._animations_OnAnimationEnded);
      this._storagePosition = this.Sprite.BoundingBoxes[11].UpperLeft;
      this._storageSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/snails-switch/Storage");
      this._bsSoop = this.Sprite.BoundingBoxes[12];
      this._soopSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/snails-switch/Soop");
      this._onTextAnimation = new SpriteAnimation("spriteset/snails-switch/On", "__TEMPORARY__");
      this._redLightSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/snails-switch/RedLight");
      this._redLightPosition = this.Sprite.BoundingBoxes[13].UpperLeft;
      this._greenLightSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/snails-switch/GreenLight");
      this._greenLightPosition = this.Sprite.BoundingBoxes[14].UpperLeft;
      this.ComputeSoopRect();
      this._font = BrainGame.ResourceManager.Load<TextFont>("fonts/snails-switch", ResourceManager.ResourceManagerCacheType.Temporary);
      this._switchSound = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/switch", (Object2D) this);
      this.UpdateCounterString();
    }

    public override void Initialize()
    {
      base.Initialize();
      this._bsSnailCollisionRight = this.TransformSpriteFrameBB(17).ToBoundingSquare();
      this._bsSnailCollisionLeft = this.TransformSpriteFrameBB(18).ToBoundingSquare();
      this._rcCounter = this.TransformSpriteFrameBB(19).ToRect();
      this._onTextAnimation.Position = new Vector2((float) this._rcCounter.Left, (float) this._rcCounter.Top);
    }

    protected override void OnSnailCollided(Snail snail)
    {
      if (this.IsOn || this.IsSuckingSnail || !snail.CanBeSuckedBySwitch)
        return;
      if (snail.Direction == MovingObject.WalkDirection.Clockwise)
      {
        if (!snail.CheckCollisionWithHead(this._bsSnailCollisionRight))
          return;
        this._snailAnimation._animation._spriteEffect = SpriteEffects.None;
      }
      else
      {
        if (!snail.CheckCollisionWithHead(this._bsSnailCollisionLeft))
          return;
        this._snailAnimation._animation._spriteEffect = SpriteEffects.FlipHorizontally;
      }
      this._animations.Activate();
      snail.KillBySacrificeSwitch();
      this._currentSnailSooped = snail;
      ++this._numSacrifices;
      this.UpdateCounterString();
      if (this._numSacrifices < this.SnailsToSacrifice)
        return;
      this.SwitchOn();
      this._switchSound.Play();
    }

    private void ButtonsAnimationEnded()
    {
      Prop shell = Prop.CreateShell(this.Position + this.Sprite.BoundingBoxes[16].UpperLeft);
      shell.DrawInForeground = true;
      Stage.CurrentStage.AddObjectInRuntime((StageObject) shell);
      shell.ProjectWithRotation(Mathematics.RandomizeVector(70, 110), 50f, 30f);
      this._currentSnailSooped.DisposeFromStage();
    }

    private void _animations_OnAnimationEnded() => this.ComputeSoopRect();

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      this._animations.Update(gameTime);
      if (this.IsOn)
        this._onTextAnimation.Update(gameTime);
      else
        this.DoQuadtreeCollisions(0);
    }

    public override void Draw(bool shadow)
    {
      base.Draw(shadow);
      Vector2 zero = Vector2.Zero;
      Color color = this.BlendColor;
      Rectangle rcSoop = this._rcSoop;
      if (shadow)
      {
        zero += GenericConsts.ShadowDepth;
        color = this.ShadowColor;
        rcSoop.X += (int) zero.X;
        rcSoop.Y += (int) zero.Y;
      }
      this._animations.Draw(this.Position + zero, color, Stage.CurrentStage.SpriteBatch);
      this._soopSprite.Draw(rcSoop, 0, color, Stage.CurrentStage.SpriteBatch);
      this._storageSprite.Draw(this.Position + this._storagePosition + zero, 0, color, Stage.CurrentStage.SpriteBatch);
      if (this.IsOff)
        this._redLightSprite.Draw(this.Position + this._redLightPosition, Stage.CurrentStage.SpriteBatch);
      else
        this._greenLightSprite.Draw(this.Position + this._greenLightPosition, Stage.CurrentStage.SpriteBatch);
      if (!this.IsOn)
        this._font.DrawString(Stage.CurrentStage.SpriteBatch, this._numSacrificesString, this._rcCounter, TextFont.TextHorizontalAlign.Center);
      else
        this._onTextAnimation.Draw(Stage.CurrentStage.SpriteBatch);
    }

    private void ComputeSoopRect()
    {
      float height = (float) (this._numSacrifices * 7);
      this._rcSoop = new Rectangle((int) ((double) this.Position.X + (double) this._bsSoop.Left), (int) ((double) this.Position.Y + ((double) this._bsSoop.Top + (double) this._bsSoop.Height - (double) height)), (int) this._bsSoop.Width, (int) height);
      this._soopAnimation._position = new Vector2(this._bsSoop.Left, (float) this._rcSoop.Top - this.Position.Y);
    }

    private void UpdateCounterString()
    {
      this._numSacrificesString = (this.SnailsToSacrifice - this._numSacrifices).ToString();
    }

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord(context);
      if (context == ToDataFileRecordContext.StageSave)
        dataFileRecord.AddField("snailsToSacrifice", (object) this.SnailsToSacrifice);
      return dataFileRecord;
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      this.SnailsToSacrifice = record.GetFieldValue<int>("snailsToSacrifice", this.SnailsToSacrifice);
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }
  }
}
