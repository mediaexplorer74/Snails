
// Type: TwoBrainsGames.Snails.Stages.BoardPath
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine.SpacePartitioning;


namespace TwoBrainsGames.Snails.Stages
{
  public class BoardPath
  {
    public List<BoardPathNode> Container { get; private set; }

    private Quadtree Quadtree { get; set; }

    public BoardPath()
    {
    }

    public BoardPath(Quadtree quadtree)
    {
      this.Container = new List<BoardPathNode>(100);
      this.Quadtree = quadtree;
    }

    public void Clear() => this.Container.Clear();

    public BoardPathNode AddPathNode(PathSegment segment)
    {
      BoardPathNode boardPathNode = new BoardPathNode(segment);
      this.Container.Add(boardPathNode);
      this.Quadtree.AddObject((IQuadtreeContainable) boardPathNode, 2);
      return boardPathNode;
    }

    public BoardPathNode AddNextToPathNode(BoardPathNode node, PathSegment segment)
    {
      BoardPathNode pathNode = new BoardPathNode(segment, node);
      node.Next = pathNode;
      this.Container.Add(pathNode);
      return pathNode;
    }

    public void Remove(BoardPathNode node)
    {
      if (node == null)
        return;
      if (node.Previous != null)
        node.Previous.Next = (BoardPathNode) null;
      if (node.Next != null)
        node.Next.Previous = (BoardPathNode) null;
      this.Container.Remove(node);
      this.Quadtree.RemoveObject((IQuadtreeContainable) node);
    }

    public void RemoveCoicident(BoardPathNode node)
    {
      this.Container.Remove(node);
      this.Quadtree.RemoveObject((IQuadtreeContainable) node);
      node.Next = (BoardPathNode) null;
      node.Previous = (BoardPathNode) null;
      node.Value = (PathSegment) null;
      node = (BoardPathNode) null;
    }

    public BoardPathNode FindNode(Vector2 p)
    {
      return this.Container.Find<BoardPathNode>((Func<BoardPathNode, bool>) (match =>
      {
        PathSegment pathSegment = match.Value;
        return (double) pathSegment.P1.X >= (double) p.X && (double) pathSegment.P0.X <= (double) p.X && (double) pathSegment.P1.Y >= (double) p.Y && (double) pathSegment.P0.Y <= (double) p.Y;
      }));
    }

    public bool ExistsNode(BoardPathNode n)
    {
      return this.Container.Exists<BoardPathNode>((Func<BoardPathNode, bool>) (match => match.Value == n.Value));
    }

    public BoardPathNode FindSegmentNode(PathSegment seg)
    {
      return this.Container.Find<BoardPathNode>((Func<BoardPathNode, bool>) (match => seg == match.Value));
    }
  }
}
