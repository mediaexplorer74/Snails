
// Type: TwoBrainsGames.BrainEngine.SpacePartitioning.IQuadtreeContainable
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine.Collision;


namespace TwoBrainsGames.BrainEngine.SpacePartitioning
{
  public interface IQuadtreeContainable
  {
    Quadtree Quadtree { get; set; }

    List<QuadtreeNode> QuadtreeNodes { get; set; }

    int ObjectListIdx { get; set; }

    bool ShouldTestCollisions { get; }

    bool Collides(IQuadtreeContainable obj);

    bool Collides(BoundingSquare obj);

    bool Collides(Vector2 p0, Vector2 p1);

    bool IsContained(QuadtreeNode node);

    void OnCollide(IQuadtreeContainable obj, int listIdx);

    bool Contains(Vector2 P);

    string FormatStringToDumpFile(QuadtreeNode currentNode);
  }
}
