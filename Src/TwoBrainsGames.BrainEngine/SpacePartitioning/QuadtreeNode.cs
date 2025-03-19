
// Type: TwoBrainsGames.BrainEngine.SpacePartitioning.QuadtreeNode
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine.Collision;


namespace TwoBrainsGames.BrainEngine.SpacePartitioning
{
  public class QuadtreeNode
  {
    private Quadtree QuadtreeOwner { get; set; }

    private QuadtreeNode ParentNode { get; set; }

    private QuadtreeNode[] Nodes { get; set; }

    public BoundingSquare BoundingBox { get; set; }

    public List<IQuadtreeContainable>[] ObjectLists { get; private set; }

    public bool IsLeafNode => this.Nodes == null;

    private int ObjectCount { get; set; }

    public QuadtreeNode(Vector2 ul, Vector2 lr, QuadtreeNode parentNode, Quadtree quadtreeOwner)
    {
      this.BoundingBox = new BoundingSquare(ul, lr);
      this.ParentNode = parentNode;
      this.QuadtreeOwner = quadtreeOwner;
      this.ObjectLists = new List<IQuadtreeContainable>[this.QuadtreeOwner.ObjectListsPerNode];
      for (int index = 0; index < this.QuadtreeOwner.ObjectListsPerNode; ++index)
        this.ObjectLists[index] = new List<IQuadtreeContainable>();
    }

    public void PutObject(IQuadtreeContainable obj, int listIdx)
    {
      this.ObjectLists[listIdx].Add(obj);
      if (obj.QuadtreeNodes == null)
        obj.QuadtreeNodes = new List<QuadtreeNode>();
      obj.QuadtreeNodes.Add(this);
      obj.ObjectListIdx = listIdx;
      ++this.ObjectCount;
    }

    public void AddObject(IQuadtreeContainable obj, int listIdx)
    {
      if (this.IsLeafNode)
      {
        if (this.ObjectCount < this.QuadtreeOwner.MaxObjectsPerNode || (double) this.BoundingBox.Width < (double) this.QuadtreeOwner.MinNodeWidth || (double) this.BoundingBox.Height < (double) this.QuadtreeOwner.MinNodeHeight)
        {
          this.PutObject(obj, listIdx);
          return;
        }
        this.SplitNode();
        this.ReassignObjectsToChildNodes();
      }
      this.AssignObjectToChildNode(obj, listIdx);
    }

    private void SplitNode()
    {
      this.Nodes = new QuadtreeNode[4];
      float num1 = this.BoundingBox.Width / 2f;
      float num2 = this.BoundingBox.Height / 2f;
      float x = this.BoundingBox.UpperLeft.X;
      float y = this.BoundingBox.UpperLeft.Y;
      this.Nodes[0] = new QuadtreeNode(new Vector2(x, y), new Vector2(x + num1, y + num2), this, this.QuadtreeOwner);
      this.Nodes[1] = new QuadtreeNode(new Vector2(x + num1, y), new Vector2(x + num1 * 2f, y + num2), this, this.QuadtreeOwner);
      this.Nodes[2] = new QuadtreeNode(new Vector2(x, y + num2), new Vector2(x + num1, y + num2 * 2f), this, this.QuadtreeOwner);
      this.Nodes[3] = new QuadtreeNode(new Vector2(x + num1, y + num2), new Vector2(x + num1 * 2f, y + num2 * 2f), this, this.QuadtreeOwner);
    }

    private void ReassignObjectsToChildNodes()
    {
      for (int index1 = 0; index1 < this.ObjectLists.Length; ++index1)
      {
        for (int index2 = 0; index2 < this.ObjectLists[index1].Count; ++index2)
        {
          this.ObjectLists[index1][index2].QuadtreeNodes.Remove(this);
          this.AssignObjectToChildNode(this.ObjectLists[index1][index2], this.ObjectLists[index1][index2].ObjectListIdx);
        }
        this.ObjectLists[index1].Clear();
      }
      this.ObjectCount = 0;
    }

    private void AssignObjectToChildNode(IQuadtreeContainable obj, int listIdx)
    {
      foreach (QuadtreeNode node in this.Nodes)
      {
        if (obj.Collides(node.BoundingBox))
          node.AddObject(obj, listIdx);
      }
    }

    public void RemoveObject(IQuadtreeContainable obj)
    {
      this.ObjectLists[obj.ObjectListIdx].Remove(obj);
      --this.ObjectCount;
    }

    public void DoCollisions(BoundingSquare bs, int listIdx, List<IQuadtreeContainable> objList)
    {
      if (this.ObjectLists[listIdx].Count > 0)
      {
        foreach (IQuadtreeContainable quadtreeContainable in this.ObjectLists[listIdx])
        {
          if (quadtreeContainable.Collides(bs) && !objList.Contains(quadtreeContainable))
            objList.Add(quadtreeContainable);
        }
      }
      if (this.Nodes == null)
        return;
      for (int index = 0; index < this.Nodes.Length; ++index)
      {
        if (this.Nodes[index].BoundingBox.Collides(bs))
          this.Nodes[index].DoCollisions(bs, listIdx, objList);
      }
    }

    public void DoCollisions(
      IQuadtreeContainable obj,
      int listIdx,
      List<IQuadtreeContainable> collidingObjects)
    {
      foreach (IQuadtreeContainable quadtreeContainable in this.ObjectLists[listIdx])
      {
        if (obj != quadtreeContainable && obj.ShouldTestCollisions && quadtreeContainable.ShouldTestCollisions && quadtreeContainable.Collides(obj))
        {
          if (collidingObjects != null)
            collidingObjects.Add(quadtreeContainable);
          else
            obj.OnCollide(quadtreeContainable, listIdx);
        }
      }
    }

    public bool Find(IQuadtreeContainable objToFind)
    {
      foreach (List<IQuadtreeContainable> objectList in this.ObjectLists)
      {
        foreach (IQuadtreeContainable quadtreeContainable in objectList)
        {
          if (quadtreeContainable == objToFind)
            return true;
        }
      }
      if (!this.IsLeafNode)
      {
        foreach (QuadtreeNode node in this.Nodes)
        {
          if (node.Find(objToFind))
            return true;
        }
      }
      return false;
    }

    public void Draw(Color color, SpriteFont font, SpriteBatch spriteBatch)
    {
      this.DrawNode(color, font, spriteBatch);
      if (this.IsLeafNode)
        return;
      foreach (QuadtreeNode node in this.Nodes)
        node.Draw(color, font, spriteBatch);
    }

    public void DrawNode(Color color, SpriteFont font, SpriteBatch spriteBatch)
    {
      this.BoundingBox.Draw(color, BrainGame.Instance.ActiveCamera.Position);
      if (!this.IsLeafNode)
        return;
      spriteBatch.DrawString(font, this.ObjectCount.ToString(), this.BoundingBox.UpperLeft, color, 0.0f, new Vector2(0.0f, 0.0f), 1f, SpriteEffects.None, 1f);
    }
  }
}
