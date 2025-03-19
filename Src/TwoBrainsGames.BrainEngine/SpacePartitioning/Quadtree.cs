
// Type: TwoBrainsGames.BrainEngine.SpacePartitioning.Quadtree
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine.Collision;


namespace TwoBrainsGames.BrainEngine.SpacePartitioning
{
  public class Quadtree
  {
    private QuadtreeNode RootNode { get; set; }

    public int MaxObjectsPerNode { get; set; }

    public int MinNodeWidth { get; set; }

    public int MinNodeHeight { get; set; }

    public int ObjectListsPerNode { get; private set; }

    public Quadtree(
      BoundingSquare bb,
      int maxObjectsPerNode,
      int minNodeWidth,
      int minNodeHeight,
      int objectListsPerNode)
    {
      this.MaxObjectsPerNode = maxObjectsPerNode;
      this.MinNodeWidth = minNodeWidth;
      this.MinNodeHeight = minNodeHeight;
      this.ObjectListsPerNode = objectListsPerNode;
      this.RootNode = new QuadtreeNode(bb.UpperLeft, bb.LowerRight, (QuadtreeNode) null, this);
    }

    public void AddObject(IQuadtreeContainable obj, int listIdx)
    {
      this.RootNode.AddObject(obj, listIdx);
      obj.Quadtree = this;
    }

    public void ObjectMoved(IQuadtreeContainable obj)
    {
      if (obj.QuadtreeNodes == null || obj.QuadtreeNodes.Count == 1 && obj.IsContained(obj.QuadtreeNodes[0]))
        return;
      this.RemoveObject(obj);
      this.AddObject(obj, obj.ObjectListIdx);
    }

    public void RemoveObject(IQuadtreeContainable obj)
    {
      if (obj.Quadtree != null && obj.QuadtreeNodes != null)
      {
        foreach (QuadtreeNode quadtreeNode in obj.QuadtreeNodes)
          quadtreeNode.RemoveObject(obj);
      }
      obj.Quadtree = (Quadtree) null;
      obj.QuadtreeNodes = (List<QuadtreeNode>) null;
    }

    public List<IQuadtreeContainable> GetCollidingObjects(BoundingSquare bs, int listIdx)
    {
      List<IQuadtreeContainable> objList = new List<IQuadtreeContainable>();
      this.RootNode.DoCollisions(bs, listIdx, objList);
      return objList;
    }

    public List<IQuadtreeContainable> GetCollidingObjects(IQuadtreeContainable obj, int listIdx)
    {
      List<IQuadtreeContainable> collidingObjects = new List<IQuadtreeContainable>();
      foreach (QuadtreeNode quadtreeNode in obj.QuadtreeNodes)
        quadtreeNode.DoCollisions(obj, listIdx, collidingObjects);
      return collidingObjects;
    }

    public void DoCollisions(IQuadtreeContainable obj, int listIdx)
    {
      if (!obj.ShouldTestCollisions)
        return;
      foreach (QuadtreeNode quadtreeNode in obj.QuadtreeNodes)
        quadtreeNode.DoCollisions(obj, listIdx, (List<IQuadtreeContainable>) null);
    }

    public bool Find(IQuadtreeContainable obj) => this.RootNode.Find(obj);

    public void Draw(Color color, SpriteFont font, SpriteBatch spriteBatch)
    {
      this.RootNode.Draw(color, font, spriteBatch);
    }

    public bool IsObjectInBounds(IQuadtreeContainable obj)
    {
      return obj.Collides(this.RootNode.BoundingBox);
    }
  }
}
