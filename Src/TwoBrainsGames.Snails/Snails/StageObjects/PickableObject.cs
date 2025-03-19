
// Type: TwoBrainsGames.Snails.StageObjects.PickableObject
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.Snails.ToolObjects;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class PickableObject : MovingObject, ISnailsDataFileSerializable, IDataFileSerializable
  {
    private const double QUANTITY_ANIMATION_TIME = 800.0;
    public PickableObject.PickableType _pickableType;
    private bool _PickedUp;
    private Sprite _toolSprite;
    private Sprite _PickedSprite;
    private Sprite _BubbleSprite;
    private SpriteAnimation _BubbleAnimation;
    private bool _IsTool;
    private bool _IsCoin;
    private bool _animateQuantity;
    private TextFont _font;
    private Vector2 _fontPosition;
    private string _quantityString;
    private double _quantityAnimationTime;
    private Sample _toolFoundSample;
    private Sample _coinFoundSample;

    public int Quantity { get; set; }

    public PickableObject()
      : base(StageObjectType.PickableObject)
    {
      this.Quantity = 1;
    }

    public override void Copy(StageObject other)
    {
      base.Copy(other);
      PickableObject pickableObject = other as PickableObject;
      this._pickableType = pickableObject._pickableType;
      this.Quantity = pickableObject.Quantity;
      this._BubbleSprite = pickableObject._BubbleSprite;
      this._IsCoin = pickableObject._IsCoin;
      this._IsTool = pickableObject._IsTool;
    }

    public override void LoadContent()
    {
      base.LoadContent();
      this._IsTool = this.QueryIsTool();
      this._IsCoin = this.QueryIsCoin();
      this._animateQuantity = this._IsTool;
      this.InitSpriteSet();
      if (this._IsTool)
      {
        this._BubbleSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "Bubble");
        this._BubbleAnimation = new SpriteAnimation(this._BubbleSprite);
      }
      this._font = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-small", ResourceManager.ResourceManagerCacheType.Static);
      this._quantityString = this.Quantity.ToString();
      this._fontPosition = new Vector2((float) ((double) this.AABoundingBox.Left + (double) this.AABoundingBox.Width / 2.0 - (double) this._font.MeasureString(this._quantityString, new Vector2(1f, 1f)) / 2.0), this.AABoundingBox.Top + this.AABoundingBox.Height / 2f - this._font.MeasureStringHeight(this._quantityString, new Vector2(1f, 1f)));
      this._toolFoundSample = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/tool_found", (Object2D) this);
      this._coinFoundSample = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/coin_found", (Object2D) this);
    }

    public override void Initialize()
    {
      base.Initialize();
      this.CurrentFrame = BrainGame.Rand.Next(this.Sprite.FrameCount);
    }

    public bool QueryIsTool() => PickableObject.IsTool(this._pickableType);

    public static bool IsTool(PickableObject.PickableType type)
    {
      switch (type)
      {
        case PickableObject.PickableType.Dynamite:
        case PickableObject.PickableType.Vitamin:
        case PickableObject.PickableType.Apple:
        case PickableObject.PickableType.Box:
        case PickableObject.PickableType.Copper:
        case PickableObject.PickableType.DynamiteBoxTriggered:
        case PickableObject.PickableType.DynamiteBox:
        case PickableObject.PickableType.Trampoline:
        case PickableObject.PickableType.Salt:
          return true;
        default:
          return false;
      }
    }

    public bool QueryIsCoin()
    {
      switch (this._pickableType)
      {
        case PickableObject.PickableType.GoldCoin:
        case PickableObject.PickableType.SilverCoin:
        case PickableObject.PickableType.CopperCoin:
          return true;
        default:
          return false;
      }
    }

    public override void OnLastFrame()
    {
      base.OnLastFrame();
      if (!this._PickedUp)
        return;
      if (!this._animateQuantity)
        Stage.CurrentStage.DisposeObject((StageObject) this);
      this.DynamicFlags &= ~StageObjectDynamicFlags.IsVisible;
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      if (this.IsDead || this.IsDisposed)
        return;
      if (!this._PickedUp)
      {
        if (this._BubbleAnimation != null)
          this._BubbleAnimation.Update(gameTime);
        this.DoQuadtreeCollisions(0);
      }
      else
      {
        if (!this._animateQuantity || this.Quantity <= 1)
          return;
        this._fontPosition = new Vector2(this._fontPosition.X, this._fontPosition.Y - (float) (gameTime.ElapsedGameTime.TotalMilliseconds * 0.05000000074505806));
        this._quantityAnimationTime += gameTime.ElapsedGameTime.TotalMilliseconds;
        if (this._quantityAnimationTime <= 800.0)
          return;
        this.DisposeFromStage();
      }
    }

    public override void OnCollide(IQuadtreeContainable obj, int listIdx)
    {
      if (this._PickedUp)
        return;
      if (listIdx == 0)
      {
        if (!((Snail) obj).CanPickupObject)
          return;
        if (this._IsTool)
        {
          ToolObjectType type = (ToolObjectType) Enum.Parse(typeof (ToolObjectType), this._pickableType.ToString(), true);
          if (Stage.CurrentStage.StageHUD._toolsMenu.AllowAddToolQuantity(type, this.Quantity))
          {
            if (this._toolFoundSample != null && !this._toolFoundSample.IsPlaying)
            {
              this._toolFoundSample.Play();
              this.FadeOut(1f);
            }
            this._PickedUp = true;
          }
        }
        else if (this._IsCoin)
        {
          if (this._coinFoundSample != null && !this._coinFoundSample.IsPlaying)
            this._coinFoundSample.Play();
          switch (this._pickableType)
          {
            case PickableObject.PickableType.GoldCoin:
              ++Stage.CurrentStage.Stats.NumGoldCoins;
              ++Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalGoldCoins;
              break;
            case PickableObject.PickableType.SilverCoin:
              ++Stage.CurrentStage.Stats.NumSilverCoins;
              ++Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSilverCoins;
              break;
            case PickableObject.PickableType.CopperCoin:
              ++Stage.CurrentStage.Stats.NumBronzeCoins;
              ++Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalBronzeCoins;
              break;
          }
          this._PickedUp = true;
        }
        if (!this._PickedUp)
          return;
        this.Sprite = this._PickedSprite;
        this.CurrentFrame = 0;
      }
      else
      {
        if (!(obj is TileObject))
          return;
        this._IsTool = false;
        this.StaticFlags &= ~StageObjectStaticFlags.CanCollide;
        this.StaticFlags |= StageObjectStaticFlags.CanFall;
      }
    }

    public override void Draw(bool shadow)
    {
      base.Draw(shadow);
      if (this._IsTool && !this._PickedUp)
        this._BubbleAnimation.Draw(this.Position, Stage.CurrentStage.SpriteBatch);
      if (this.Quantity <= 1)
        return;
      this._font.DrawString(Stage.CurrentStage.SpriteBatch, this._quantityString, this._fontPosition, new Vector2(1f, 1f), this.BlendColor);
    }

    private void InitSpriteSet()
    {
      this.ResourceId = "spriteset/pickable-objects";
      this.SpriteId = this._pickableType.ToString();
      this._toolSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, this.SpriteId);
      this.Sprite = this._toolSprite;
      this._PickedSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "BubblePickUp");
      if (!this._IsCoin)
        return;
      this._PickedSprite = BrainGame.ResourceManager.GetSpriteTemporary(this.ResourceId, "CoinPickUp");
    }

    public void SetPickableType(PickableObject.PickableType pickType)
    {
      this._pickableType = pickType;
      this.InitSpriteSet();
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
      string fieldValue = record.GetFieldValue<string>("pickableType", (string) null);
      if (!string.IsNullOrEmpty(fieldValue))
        this._pickableType = (PickableObject.PickableType) Enum.Parse(typeof (PickableObject.PickableType), fieldValue, true);
      this.Quantity = record.GetFieldValue<int>("quantity", 1);
    }

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord = base.ToDataFileRecord(context);
      dataFileRecord.AddField("pickableType", (object) (int) this._pickableType);
      dataFileRecord.AddField("quantity", (object) this.Quantity);
      return dataFileRecord;
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public enum PickableType
    {
      Proxy,
      GoldCoin,
      SilverCoin,
      CopperCoin,
      Dynamite,
      Vitamin,
      Apple,
      Box,
      Copper,
      DynamiteBoxTriggered,
      DynamiteBox,
      Trampoline,
      Salt,
    }
  }
}
