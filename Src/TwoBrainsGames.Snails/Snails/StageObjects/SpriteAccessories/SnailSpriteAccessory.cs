
// Type: TwoBrainsGames.Snails.StageObjects.SpriteAccessories.SnailSpriteAccessory
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects.SpriteAccessories
{
  public class SnailSpriteAccessory
  {
    protected Sprite _spriteWalk;
    protected Sprite _spriteTurnDown;
    protected Sprite _spriteTurnUp;
    private Sprite _spriteDeathWithStageExit;
    private Sprite _spriteByte;
    private Sprite _spriteChew;
    private Sprite _spriteEmpaled;
    private Sprite _spriteDeadBrokenShell;
    private Sprite _spriteHiddingInShell;
    private Sprite _spriteAccessory;
    protected Snail _snail;
    private bool _visible;
    private bool _shouldUpdateSprite;

    public virtual bool Visible
    {
      get => this._visible;
      set
      {
        this._visible = value;
        this._shouldUpdateSprite = true;
      }
    }

    public SnailSpriteAccessory(Snail snail)
    {
      this._snail = snail;
      this._visible = false;
    }

    public virtual void LoadContent(string spriteSet)
    {
      this._spriteWalk = BrainGame.ResourceManager.GetSpriteTemporary(spriteSet, "SnailWalk");
      this._spriteTurnDown = BrainGame.ResourceManager.GetSpriteTemporary(spriteSet, "SnailTurnDown");
      this._spriteTurnUp = BrainGame.ResourceManager.GetSpriteTemporary(spriteSet, "SnailTurnUp");
      if (BrainGame.ResourceManager.ContainsSprite(spriteSet, "DeathWithStageExit"))
        this._spriteDeathWithStageExit = BrainGame.ResourceManager.GetSpriteTemporary(spriteSet, "DeathWithStageExit");
      if (BrainGame.ResourceManager.ContainsSprite(spriteSet, "Byte"))
        this._spriteByte = BrainGame.ResourceManager.GetSpriteTemporary(spriteSet, "Byte");
      if (BrainGame.ResourceManager.ContainsSprite(spriteSet, "Chew"))
        this._spriteChew = BrainGame.ResourceManager.GetSpriteTemporary(spriteSet, "Chew");
      if (BrainGame.ResourceManager.ContainsSprite(spriteSet, "EmpaledSnail"))
        this._spriteEmpaled = BrainGame.ResourceManager.GetSpriteTemporary(spriteSet, "EmpaledSnail");
      if (BrainGame.ResourceManager.ContainsSprite(spriteSet, "DeadSnailBrokenShell"))
        this._spriteDeadBrokenShell = BrainGame.ResourceManager.GetSpriteTemporary(spriteSet, "DeadSnailBrokenShell");
      if (BrainGame.ResourceManager.ContainsSprite(spriteSet, "DeadSnailBrokenShell"))
        this._spriteDeadBrokenShell = BrainGame.ResourceManager.GetSpriteTemporary(spriteSet, "DeadSnailBrokenShell");
      if (!BrainGame.ResourceManager.ContainsSprite(spriteSet, "HiddingInShell"))
        return;
      this._spriteHiddingInShell = BrainGame.ResourceManager.GetSpriteTemporary(spriteSet, "HiddingInShell");
    }

    public virtual void LoadContent()
    {
    }

    public virtual void Initialize() => this.Visible = false;

    public virtual void Update(BrainGameTime gameTime)
    {
    }

    public virtual void Draw(bool shadow, SpriteBatch spriteBatch)
    {
      if (!this.Visible)
        return;
      if (this._shouldUpdateSprite)
        this.UpdateActiveSprite();
      if (this._spriteAccessory == null)
        return;
      this._snail.DrawParentChild(shadow, this._snail.Sprite, this._spriteAccessory);
    }

    public virtual void OnEnterLiquid()
    {
    }

    public virtual void OnExitLiquid()
    {
    }

    public virtual void UpdateActiveSprite()
    {
      this._shouldUpdateSprite = false;
      if (this._snail.Sprite == this._snail.WalkSprite)
        this._spriteAccessory = this._spriteWalk;
      else if (this._snail.Sprite == this._snail.OuterTurnSprite)
        this._spriteAccessory = this._spriteTurnDown;
      else if (this._snail.Sprite == this._snail.InnerTurnSprite)
        this._spriteAccessory = this._spriteTurnUp;
      else if (this._snail.Sprite == this._snail.DeathWithStageExitSprite)
        this._spriteAccessory = this._spriteDeathWithStageExit;
      else if (this._snail.Sprite == this._snail.SpriteByting)
        this._spriteAccessory = this._spriteByte;
      else if (this._snail.Sprite == this._snail.SpriteChewing)
        this._spriteAccessory = this._spriteChew;
      else if (this._snail.Sprite == this._snail.SpriteEmpaled)
        this._spriteAccessory = this._spriteEmpaled;
      else if (this._snail.Sprite == this._snail.SpriteDeadBrokenShell)
        this._spriteAccessory = this._spriteDeadBrokenShell;
      else if (this._snail.Sprite == this._snail.SpriteHiddingInShell)
        this._spriteAccessory = this._spriteHiddingInShell;
      else
        this._spriteAccessory = (Sprite) null;
    }

    public virtual void Project() => this.Project(((ISnailSpriteAccessory) this).CreateProp());

    protected void Project(Prop prop)
    {
      prop.Position = this._snail.Position;
      Stage.CurrentStage.AddObjectInRuntime((StageObject) prop);
      prop.DrawInForeground = true;
      float degrees = (float) (BrainGame.Rand.Next(90) - 135);
      float y = (float) Math.Sin((double) MathHelper.ToRadians(degrees));
      Vector2 direction = new Vector2((float) Math.Cos((double) MathHelper.ToRadians(degrees)), y);
      prop.ProjectWithRotation(direction, (float) BrainGame.Rand.Next(30, 50), 30f);
    }
  }
}
