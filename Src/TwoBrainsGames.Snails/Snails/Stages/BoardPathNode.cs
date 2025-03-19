
// Type: TwoBrainsGames.Snails.Stages.BoardPathNode
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.SpacePartitioning;


namespace TwoBrainsGames.Snails.Stages
{
  public class BoardPathNode : IQuadtreeContainable
  {
    public BoardPathNode Next { get; set; }

    public BoardPathNode Previous { get; set; }

    public PathSegment Value { get; set; }

    public BoardPathNode(PathSegment segment)
      : this(segment, (BoardPathNode) null)
    {
    }

    public BoardPathNode(PathSegment segment, BoardPathNode previousNode)
    {
      this.Value = segment;
      this.Previous = previousNode;
    }

    public Quadtree Quadtree { get; set; }

    public List<QuadtreeNode> QuadtreeNodes { get; set; }

    public int ObjectListIdx { get; set; }

    public bool ShouldTestCollisions => true;

    public bool Collides(IQuadtreeContainable obj)
    {
      return obj.Contains(this.Value.P0) || obj.Contains(this.Value.P1) || obj.Collides(this.Value.P0, this.Value.P1);
    }

    public bool Collides(Vector2 p0, Vector2 p1)
    {
      return Mathematics.LineLineIntersection(p0, p1, this.Value.P0, this.Value.P1, out Vector2 _);
    }

    public void OnCollide(IQuadtreeContainable obj, int listIdx)
    {
    }

    public bool IsContained(QuadtreeNode node)
    {
      return node.BoundingBox.Contains(this.Value.P0) && node.BoundingBox.Contains(this.Value.P1);
    }

    public bool Collides(BoundingSquare bs)
    {
      return this.Value != (PathSegment) null && this.Value.Intersects(bs);
    }

    public bool Contains(Vector2 P) => false;

    public string FormatStringToDumpFile(QuadtreeNode currentNode) => this.GetType().ToString();
  }
}
