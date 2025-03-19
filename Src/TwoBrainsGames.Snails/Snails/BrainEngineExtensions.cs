
// Type: TwoBrainsGames.Snails.BrainEngineExtensions
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.Snails.Stages;
using TwoBrainsGames.Snails.ToolObjects;


namespace TwoBrainsGames.Snails
{
  public static class BrainEngineExtensions
  {
    public static Sprite GetSpriteTemporary(this ResourceManager res, Tile tile)
    {
      return !string.IsNullOrEmpty(tile.ResourceId) && !string.IsNullOrEmpty(tile.SpriteId) ? BrainGame.ResourceManager.GetSpriteTemporary(tile.ResourceId, tile.SpriteId) : (Sprite) null;
    }

    public static Sprite GetSpriteStatic(this ResourceManager res, Tile tile)
    {
      return !string.IsNullOrEmpty(tile.ResourceId) && !string.IsNullOrEmpty(tile.SpriteId) ? BrainGame.ResourceManager.GetSpriteStatic(tile.ResourceId, tile.SpriteId) : (Sprite) null;
    }

    public static Sprite GetSpriteTemporary(this ResourceManager res, StageObject obj)
    {
      return !string.IsNullOrEmpty(obj.ResourceId) && !string.IsNullOrEmpty(obj.SpriteId) ? BrainGame.ResourceManager.GetSpriteTemporary(obj.ResourceId, obj.SpriteId) : (Sprite) null;
    }

    public static Sprite GetSpriteStatic(this ResourceManager res, StageObject obj)
    {
      return !string.IsNullOrEmpty(obj.ResourceId) && !string.IsNullOrEmpty(obj.SpriteId) ? BrainGame.ResourceManager.GetSpriteStatic(obj.ResourceId, obj.SpriteId) : (Sprite) null;
    }

    public static Sprite GetSpriteTemporary(this ResourceManager res, ToolObject tool)
    {
      return !string.IsNullOrEmpty(tool.ResourceId) && !string.IsNullOrEmpty(tool.SpriteId) ? BrainGame.ResourceManager.GetSpriteTemporary(tool.ResourceId, tool.SpriteId) : (Sprite) null;
    }

    public static Sprite GetSpriteStatic(this ResourceManager res, ToolObject tool)
    {
      return !string.IsNullOrEmpty(tool.ResourceId) && !string.IsNullOrEmpty(tool.SpriteId) ? BrainGame.ResourceManager.GetSpriteStatic(tool.ResourceId, tool.SpriteId) : (Sprite) null;
    }
  }
}
