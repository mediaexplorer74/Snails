
// Type: TwoBrainsGames.Snails.Stages.HUD.HUDItem
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Resources;


namespace TwoBrainsGames.Snails.Stages.HUD
{
  internal class HUDItem
  {
    public Vector2 _size;
    public Vector2 _position;
    protected TextFont _font;
    public float _width;
    public bool _autoPosition;
    public bool _visible;

    public HUDItem() => this._autoPosition = true;

    public virtual void Initialize(Vector2 position) => this._position = position;

    public virtual void LoadContent()
    {
      this._font = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-medium", ResourceManager.ResourceManagerCacheType.Static);
    }

    public virtual void Update(BrainGameTime gameTime)
    {
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
    }

    public virtual void UnloadContent()
    {
    }

    public virtual void SnailsStageStatsChanged()
    {
    }

    public virtual void HandleInput(BrainGameTime gameTime)
    {
    }

    public virtual void MissionStateChanged()
    {
    }

    public virtual void TimeWarpChanged()
    {
    }

    public virtual void StageAreaChanged(BoundingSquare newStageArea)
    {
    }
  }
}
