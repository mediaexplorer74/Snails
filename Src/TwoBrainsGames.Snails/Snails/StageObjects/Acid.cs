
// Type: TwoBrainsGames.Snails.StageObjects.Acid
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class Acid : Liquid
  {
    private const int BUBBLES_PER_TILE = 1;
    private const float BUBBLES_SPEED = 50f;
    private const float NO_BUBBLES_LEVEL = 0.1f;
    private Sprite _bubbleSprite;
    private List<Acid.Bubble> _bubbles;
    private int _bubbleCount;
    private Sample[] _bubblesSound;

    public Acid()
      : base(StageObjectType.Acid)
    {
    }

    public override void Copy(StageObject other) => base.Copy(other);

    public override void LoadContent()
    {
      base.LoadContent();
      this._bubbleSprite = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/acid", "Bubble");
      this._bubblesSound = new Sample[3];
      this._bubblesSound[0] = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/bubble-pop", (Object2D) this);
      this._bubblesSound[1] = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/bubble-pop-2", (Object2D) this);
      this._bubblesSound[2] = BrainGame.ResourceManager.GetSampleTemporary("sfx/objects/bubble-pop-3", (Object2D) this);
    }

    public override void Initialize()
    {
      base.Initialize();
      this._bubbles = new List<Acid.Bubble>();
      this._bubbleCount = (int) (1.0 * (double) this.Size.X * (double) this.Size.Y);
      for (int index = 0; index < this._bubbleCount; ++index)
      {
        Acid.Bubble bubble = new Acid.Bubble()
        {
          _popAnimation = new SpriteAnimation(BrainGame.ResourceManager.GetSpriteTemporary("spriteset/acid", "BubblePop"))
        };
        bubble._popAnimation.Visible = false;
        bubble._popAnimation.Autohide = true;
        bubble._position = new Vector2(bubble.RandomizeX(this._liquidAABB.Width), this._sizeInPixels.Y - (float) BrainGame.Rand.Next((int) this._liquidAABB.Height));
        bubble._position += this.Position;
        bubble.RandomizeScale();
        this._bubbles.Add(bubble);
      }
      this.RefreshBubbleCount();
    }

    private void RefreshBubbleCount()
    {
      this._bubbleCount = (int) ((double) this._liquidLevel * (double) this._bubbles.Count);
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      if (this.IsEmpty)
        return;
      this.RefreshBubbleCount();
      if ((double) this._liquidLevel < 0.10000000149011612)
        this._bubbleCount = 0;
      for (int index = 0; index < this._bubbleCount; ++index)
      {
        Acid.Bubble bubble = this._bubbles[index];
        float num = (float) (50.0 * (gameTime.ElapsedGameTime.TotalMilliseconds / 1000.0)) * bubble._scale;
        bubble._position = new Vector2(bubble._position.X, bubble._position.Y - num);
        if ((double) bubble._position.Y < (double) this._drawRectangle.Y)
        {
          this.PlayPop(bubble);
          bubble._popAnimation.Position = new Vector2(bubble._position.X, (float) this._drawRectangle.Y);
          bubble._popAnimation.Visible = true;
          bubble._popAnimation.Scale = new Vector2(bubble._scale, bubble._scale);
          bubble._position = new Vector2(bubble.RandomizeX(this._sizeInPixels.X), this._sizeInPixels.Y);
          bubble._position += this.Position;
          bubble.RandomizeScale();
        }
        if (bubble._popAnimation.Visible)
        {
          bubble._popAnimation.Update(gameTime);
          bubble._popAnimation.Position = new Vector2(bubble._popAnimation.Position.X, (float) this._drawRectangle.Y);
        }
      }
    }

    private void PlayPop(Acid.Bubble bubble)
    {
      this._bubblesSound[BrainGame.Rand.Next(this._bubblesSound.Length)].Play(bubble._scale);
    }

    public override void Draw(bool shadow)
    {
      base.Draw(shadow);
      if (this.IsEmpty)
        return;
      for (int index = 0; index < this._bubbleCount; ++index)
      {
        Acid.Bubble bubble = this._bubbles[index];
        this._bubbleSprite.Draw(bubble._position, 0, 0.0f, SpriteEffects.None, Color.White, bubble._scale, Stage.CurrentStage.SpriteBatch);
        if (bubble._popAnimation.Visible)
          bubble._popAnimation.Draw(Stage.CurrentStage.SpriteBatch);
      }
    }

    public override void InitFromDataFileRecord(DataFileRecord record)
    {
      base.InitFromDataFileRecord(record);
    }

    public override DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public override DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      return base.ToDataFileRecord(context);
    }

    private class Bubble
    {
      public Vector2 _position;
      public float _scale;
      public SpriteAnimation _popAnimation;

      public Bubble()
      {
        this._position = Vector2.Zero;
        this._scale = 1f;
      }

      public void RandomizeScale() => this._scale = (float) (5 + BrainGame.Rand.Next(5)) / 10f;

      public float RandomizeX(float xInterval) => (float) BrainGame.Rand.Next((int) xInterval);
    }
  }
}
