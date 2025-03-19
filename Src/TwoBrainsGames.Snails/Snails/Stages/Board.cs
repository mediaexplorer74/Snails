
// Type: TwoBrainsGames.Snails.Stages.Board
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.SpacePartitioning;
using TwoBrainsGames.Snails.StageObjects;


namespace TwoBrainsGames.Snails.Stages
{
  public class Board : IBrainComponent, ISnailsDataFileSerializable, IDataFileSerializable
  {
    public const int MINIMUM_BOARD_COLS = 19;
    public const int MINIMUM_BOARD_ROWS = 12;
    private List<BoardPathNode> _nodeAddList = new List<BoardPathNode>(32);
    private List<BoardPathNode> collisionNodes;
    private SamplerState _samplerState;
    private SpriteAnimation _arrowAnimation;
    private List<TileCell> _drawBackgroundTilesList;
    private List<TileCell> _drawForegroundTilesList;

    public int Columns { get; set; }

    public int Rows { get; set; }

    public int TileWidth => 60;

    public int TileHeight => 60;

    public int Width => this.Columns * this.TileWidth;

    public int Height => this.Rows * this.TileHeight;

    public float WidthInCameraWorld => (float) this.Width * Stage.CurrentStage.Camera.Scale.X;

    public float HeightInCameraWorld => (float) this.Height * Stage.CurrentStage.Camera.Scale.Y;

    public TileCell[,] Tiles { get; set; }

    public BoardPath Paths { get; set; }

    public BoundingSquare BoundingBox { get; private set; }

    public Quadtree Quadtree { get; private set; }

    public SpriteBatch SpriteBatch => Levels.CurrentLevel.SpriteBatch;

    public Board()
      : this(19, 12)
    {
      this._drawBackgroundTilesList = new List<TileCell>();
      this._drawForegroundTilesList = new List<TileCell>();
    }

    public Board(int columns, int rows)
    {
      this.Columns = columns;
      this.Rows = rows;
      this.Tiles = new TileCell[rows, columns];
      this.Paths = new BoardPath();
      this.UpdateBoundingBox();
      this.collisionNodes = new List<BoardPathNode>();
    }

    public static Board FromDataFileRecord(DataFileRecord record)
    {
      Board board = new Board();
      board.InitFromDataFileRecord(record);
      return board;
    }

    public void UpdateBoundingBox()
    {
      this.BoundingBox = new BoundingSquare(new Vector2(0.0f, 0.0f), new Vector2((float) this.Width, (float) this.Height));
      this.Quadtree = new Quadtree(this.BoundingBox, 10, 200, 200, 3);
    }

    public void AddObjectToQuadtree(StageObject obj, int listIdx)
    {
      this.Quadtree.AddObject((IQuadtreeContainable) obj, listIdx);
    }

    public void RemoveObjectFromQuadtree(StageObject obj)
    {
      this.Quadtree.RemoveObject((IQuadtreeContainable) obj);
    }

    public void RepositionObjectInQuadtree(StageObject obj)
    {
      if (obj.IsDisposed)
        return;
      this.Quadtree.ObjectMoved((IQuadtreeContainable) obj);
    }

    public void Initialize()
    {
    }

    public void LoadContent()
    {
      this._samplerState = new SamplerState();
      this._samplerState.AddressU = TextureAddressMode.Wrap;
      this._samplerState.AddressV = TextureAddressMode.Wrap;
      this._arrowAnimation = new SpriteAnimation(BrainGame.ResourceManager.GetSpriteTemporary("spriteset/common-tiles/DirectionalBoxArrow"));
    }

    public void Update(BrainGameTime gameTime)
    {
      if (Stage.CurrentStage._state == Stage.StageState.Startup)
        return;
      this._arrowAnimation.Update(gameTime);
    }

    public void DrawBackground(bool shadow)
    {
      if (!shadow)
      {
        foreach (TileCell drawBackgroundTiles in this._drawBackgroundTilesList)
          drawBackgroundTiles.Tile.Draw(drawBackgroundTiles.Position);
      }
      else
      {
        foreach (TileCell drawBackgroundTiles in this._drawBackgroundTilesList)
          drawBackgroundTiles.Tile.DrawShadow(drawBackgroundTiles.Position);
      }
    }

    public void Draw() => throw new SnailsException("Not implemented.");

    private void DrawPathWithPresentation()
    {
      foreach (BoardPathNode boardPathNode in this.Paths.Container)
      {
        if (!(boardPathNode.Value == (PathSegment) null))
        {
          if (boardPathNode.Value.Behavior == PathSegmentBehavior.WalkableCW)
            this._arrowAnimation.Draw(boardPathNode.Value.Center, boardPathNode.Value.Rotation, Stage.CurrentStage.SpriteBatch);
          else if (boardPathNode.Value.Behavior == PathSegmentBehavior.WalkableCCW)
            this._arrowAnimation.Draw(boardPathNode.Value.Center, boardPathNode.Value.Rotation, SpriteEffects.FlipHorizontally, Stage.CurrentStage.SpriteBatch);
        }
      }
    }

    public void Draw(bool shadow)
    {
      if (!shadow)
      {
        foreach (TileCell drawForegroundTiles in this._drawForegroundTilesList)
          drawForegroundTiles.Tile.Draw(drawForegroundTiles.Position);
      }
      else
      {
        foreach (TileCell drawForegroundTiles in this._drawForegroundTilesList)
          drawForegroundTiles.Tile.DrawShadow(drawForegroundTiles.Position);
      }
      this.DrawPathWithPresentation();
    }

    public void UnloadContent()
    {
    }

    public List<PathSegment> GetTileSegmentsToAdd(TileCell tileCell)
    {
      List<PathSegment> tileSegmentsToAdd = new List<PathSegment>(4);
      if (tileCell != null && tileCell.Tile != null)
      {
        int boardX = tileCell.BoardX;
        int boardY = tileCell.BoardY;
        int num1 = boardX * this.TileWidth;
        int num2 = boardY * this.TileHeight;
        int num3 = num1 + this.TileWidth;
        int num4 = num2 + this.TileHeight;
        if (tileCell.Tile._topPath != PathBehaviour.None && boardY - 1 >= 0 && boardY - 1 < this.Rows && boardX >= 0 && boardX < this.Columns && this.GetTileAt(boardY - 1, boardX) == null)
        {
          if (num2 <= 0)
            tileCell.TopSegment.Behavior = PathSegmentBehavior.ReverseWalk;
          tileSegmentsToAdd.Add(tileCell.TopSegment);
        }
        if (tileCell.Tile._rightPath != PathBehaviour.None && boardY >= 0 && boardY < this.Rows && boardX + 1 >= 0 && boardX + 1 < this.Columns && this.GetTileAt(boardY, boardX + 1) == null)
        {
          if (num3 >= this.Width)
            tileCell.RightSegment.Behavior = PathSegmentBehavior.ReverseWalk;
          tileSegmentsToAdd.Add(tileCell.RightSegment);
        }
        if (tileCell.Tile._bottomPath != PathBehaviour.None && boardY + 1 >= 0 && boardY + 1 < this.Rows && boardX >= 0 && boardX < this.Columns && this.GetTileAt(boardY + 1, boardX) == null)
        {
          if (num4 >= this.Height)
            tileCell.BottomSegment.Behavior = PathSegmentBehavior.ReverseWalk;
          tileSegmentsToAdd.Add(tileCell.BottomSegment);
        }
        if (tileCell.Tile._leftPath != PathBehaviour.None && boardY >= 0 && boardY < this.Rows && boardX - 1 >= 0 && boardX - 1 < this.Columns && this.GetTileAt(boardY, boardX - 1) == null)
        {
          if (num1 <= 0)
            tileCell.LeftSegment.Behavior = PathSegmentBehavior.ReverseWalk;
          tileSegmentsToAdd.Add(tileCell.LeftSegment);
        }
        if (tileCell.Tile._topPath == PathBehaviour.Invert)
          tileSegmentsToAdd.Add(tileCell.TopSegment);
        if (tileCell.Tile._rightPath == PathBehaviour.Invert)
          tileSegmentsToAdd.Add(tileCell.RightSegment);
        if (tileCell.Tile._bottomPath == PathBehaviour.Invert)
          tileSegmentsToAdd.Add(tileCell.BottomSegment);
        if (tileCell.Tile._leftPath == PathBehaviour.Invert)
          tileSegmentsToAdd.Add(tileCell.LeftSegment);
      }
      return tileSegmentsToAdd;
    }

    public List<PathSegment> GetNeighborTileSegmentsToAdd(TileCell tileCell)
    {
      List<PathSegment> tileSegmentsToAdd = new List<PathSegment>(4);
      if (tileCell != null && tileCell.Tile != null)
      {
        int boardX = tileCell.BoardX;
        int boardY = tileCell.BoardY;
        int num1 = boardX * this.TileWidth;
        int num2 = boardY * this.TileHeight;
        int num3 = num1 + this.TileWidth;
        int num4 = num2 + this.TileHeight;
        if (tileCell.Tile._topPath != PathBehaviour.None && boardY - 1 >= 0 && boardY - 1 < this.Rows && boardX >= 0 && boardX < this.Columns && this.GetTileAt(boardY - 1, boardX) == null)
        {
          if (num2 <= 0)
            tileCell.TopSegment.Behavior = PathSegmentBehavior.ReverseWalk;
          tileSegmentsToAdd.Add(tileCell.TopSegment);
        }
        if (tileCell.Tile._rightPath != PathBehaviour.None && boardY >= 0 && boardY < this.Rows && boardX + 1 >= 0 && boardX + 1 < this.Columns && this.GetTileAt(boardY, boardX + 1) == null)
        {
          if (num3 >= this.Width)
            tileCell.RightSegment.Behavior = PathSegmentBehavior.ReverseWalk;
          tileSegmentsToAdd.Add(tileCell.RightSegment);
        }
        if (tileCell.Tile._bottomPath != PathBehaviour.None && boardY + 1 >= 0 && boardY + 1 < this.Rows && boardX >= 0 && boardX < this.Columns && this.GetTileAt(boardY + 1, boardX) == null)
        {
          if (num4 >= this.Height)
            tileCell.BottomSegment.Behavior = PathSegmentBehavior.ReverseWalk;
          tileSegmentsToAdd.Add(tileCell.BottomSegment);
        }
        if (tileCell.Tile._leftPath != PathBehaviour.None && boardY >= 0 && boardY < this.Rows && boardX - 1 >= 0 && boardX - 1 < this.Columns && this.GetTileAt(boardY, boardX - 1) == null)
        {
          if (num1 <= 0)
            tileCell.LeftSegment.Behavior = PathSegmentBehavior.ReverseWalk;
          tileSegmentsToAdd.Add(tileCell.LeftSegment);
        }
      }
      return tileSegmentsToAdd;
    }

    public List<PathSegment> GetInitialTileSegmentsToAdd(TileCell tileCell)
    {
      List<PathSegment> tileSegmentsToAdd = new List<PathSegment>(4);
      if (tileCell != null && tileCell.Tile != null)
      {
        int boardX = tileCell.BoardX;
        int boardY = tileCell.BoardY;
        int num1 = boardX * this.TileWidth;
        int num2 = boardY * this.TileHeight;
        int num3 = num1 + this.TileWidth;
        int num4 = num2 + this.TileHeight;
        if (tileCell.Tile._topPath != PathBehaviour.None)
        {
          if (num2 <= 0)
            tileCell.TopSegment.Behavior = PathSegmentBehavior.ReverseWalk;
          tileSegmentsToAdd.Add(tileCell.TopSegment);
        }
        if (tileCell.Tile._rightPath != PathBehaviour.None)
        {
          if (num3 >= this.Width)
            tileCell.RightSegment.Behavior = PathSegmentBehavior.ReverseWalk;
          tileSegmentsToAdd.Add(tileCell.RightSegment);
        }
        if (tileCell.Tile._bottomPath != PathBehaviour.None)
        {
          if (num4 >= this.Height)
            tileCell.BottomSegment.Behavior = PathSegmentBehavior.ReverseWalk;
          tileSegmentsToAdd.Add(tileCell.BottomSegment);
        }
        if (tileCell.Tile._leftPath != PathBehaviour.None)
        {
          if (num1 <= 0)
            tileCell.LeftSegment.Behavior = PathSegmentBehavior.ReverseWalk;
          tileSegmentsToAdd.Add(tileCell.LeftSegment);
        }
      }
      return tileSegmentsToAdd;
    }

    public List<PathSegment> GetAllSegments()
    {
      List<PathSegment> allSegments = new List<PathSegment>();
      if (this.Tiles != null)
      {
        for (int index1 = 0; index1 < this.Rows; ++index1)
        {
          for (int index2 = 0; index2 < this.Columns; ++index2)
          {
            TileCell tile = this.Tiles[index1, index2];
            if (tile != null)
              allSegments.AddRange((IEnumerable<PathSegment>) this.GetInitialTileSegmentsToAdd(tile));
          }
        }
      }
      return allSegments;
    }

    internal BoardPathNode GetLinkedNode(
      BoardPathNode node,
      BoardPathNode linked,
      BoardPathNode newNode)
    {
      if (linked != null && linked.Value != (PathSegment) null)
      {
        PathSegment pathSegment1 = node.Value;
        PathSegment pathSegment2 = linked.Value;
        PathSegment pathSegment3 = newNode.Value;
        Vector2 v1 = pathSegment1.P1 - pathSegment1.P0;
        Vector2 v2_1 = pathSegment2.P1 - pathSegment2.P0;
        Vector2 v2_2 = pathSegment3.P1 - pathSegment3.P0;
        if ((double) MathHelper.ToDegrees(BrainHelper.FindAngleBetweenTwoVectors(v1, v2_1)) >= (double) MathHelper.ToDegrees(BrainHelper.FindAngleBetweenTwoVectors(v1, v2_2)))
          return linked;
      }
      return newNode;
    }

    public void AddPathSegments(List<PathSegment> addSegments)
    {
      if (addSegments == null || addSegments.Count <= 0)
        return;
      foreach (PathSegment addSegment in addSegments)
      {
        BoardPathNode boardPathNode1 = this.Paths.FindSegmentNode(addSegment) ?? this.Paths.AddPathNode(addSegment);
        foreach (BoardPathNode boardPathNode2 in this.Paths.Container)
        {
          if (boardPathNode2.Value != (PathSegment) null && boardPathNode1.Value != (PathSegment) null && boardPathNode2.Value.P1 == boardPathNode1.Value.P0)
          {
            boardPathNode1.Previous = this.GetLinkedNode(boardPathNode1, boardPathNode1.Previous, boardPathNode2);
            boardPathNode2.Next = this.GetLinkedNode(boardPathNode2, boardPathNode2.Next, boardPathNode1);
          }
          if (boardPathNode2.Value != (PathSegment) null && boardPathNode1.Value != (PathSegment) null && boardPathNode2.Value.P0 == boardPathNode1.Value.P1)
          {
            boardPathNode2.Previous = this.GetLinkedNode(boardPathNode2, boardPathNode2.Previous, boardPathNode1);
            boardPathNode1.Next = this.GetLinkedNode(boardPathNode1, boardPathNode1.Next, boardPathNode2);
          }
          if (boardPathNode2.Next != null)
            boardPathNode2.Next.Previous = boardPathNode2;
        }
        this._nodeAddList.Add(boardPathNode1);
      }
    }

    public List<PathSegment> RemovePathSegments(List<PathSegment> removeSegments)
    {
      List<PathSegment> pathSegmentList = new List<PathSegment>();
      if (removeSegments != null && removeSegments.Count > 0 && this.Paths.Container.Count > 0)
      {
        List<PathSegment> addSegments = new List<PathSegment>();
        using (List<PathSegment>.Enumerator enumerator = removeSegments.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            PathSegment seg = enumerator.Current;
            BoardPathNode node = this.Paths.Container.Find<BoardPathNode>((Func<BoardPathNode, bool>) (match => match.Value == seg));
            if (node != null)
            {
              this.Paths.Remove(node);
              pathSegmentList.Add(node.Value);
              node.Value = (PathSegment) null;
            }
          }
        }
        using (List<PathSegment>.Enumerator enumerator = removeSegments.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            PathSegment seg = enumerator.Current;
            BoardPathNode boardPathNode = this.Paths.Container.Find<BoardPathNode>((Func<BoardPathNode, bool>) (match => match.Next != null && match.Next.Value == seg || match.Previous != null && match.Previous.Value == seg));
            if (boardPathNode != null)
            {
              boardPathNode.Next = (BoardPathNode) null;
              boardPathNode.Previous = (BoardPathNode) null;
              addSegments.Add(boardPathNode.Value);
            }
          }
        }
        this.AddPathSegments(addSegments);
      }
      return pathSegmentList;
    }

    public void ComputePaths()
    {
      this.Paths = new BoardPath(this.Quadtree);
      this.AddPathSegments(this.GetAllSegments());
      this.RemoveCoincidentPathSegments();
    }

    public void RemoveCoincidentPathSegments()
    {
      if (this._nodeAddList == null || this._nodeAddList.Count <= 0)
        return;
      for (int index = 0; index < this._nodeAddList.Count; ++index)
      {
        BoardPathNode node = this._nodeAddList[index];
        if (node != null && node.Value != (PathSegment) null)
        {
          BoardPathNode node1 = this.Paths.Container.Find<BoardPathNode>((Func<BoardPathNode, bool>) (match => match.Value != (PathSegment) null && node.Value != (PathSegment) null && match.Value.P0 == node.Value.P1 && match.Value.P1 == node.Value.P0));
          if (node1 != null)
          {
            this.Paths.RemoveCoicident(node);
            this.Paths.RemoveCoicident(node1);
          }
        }
      }
      foreach (BoardPathNode boardPathNode in this.Paths.Container)
      {
        if (boardPathNode.Next != null)
          boardPathNode.Next.Previous = boardPathNode;
      }
      this._nodeAddList.Clear();
    }

    public TileCell SetTileAtWithoutPaths(Tile tile, int col, int row)
    {
      bool flag = false;
      if (this.Tiles[row, col] == null)
      {
        this.Tiles[row, col] = new TileCell(this, tile, col, row);
        if (tile.Sprite != null)
          flag = true;
      }
      else
      {
        if (this.Tiles[row, col].Tile != null && this.Tiles[row, col].Tile.Sprite == null)
          flag = true;
        this.Tiles[row, col].SetTile(tile);
      }
      if (flag)
      {
        if (this.Tiles[row, col].DrawUnderWater)
          this._drawBackgroundTilesList.Add(this.Tiles[row, col]);
        else
          this._drawForegroundTilesList.Add(this.Tiles[row, col]);
      }
      return this.Tiles[row, col];
    }

    public void SetTileAt(Tile tile, int col, int row)
    {
      TileCell tileCell = this.SetTileAtWithoutPaths(tile, col, row);
      List<PathSegment> addSegments = new List<PathSegment>();
      addSegments.AddRange((IEnumerable<PathSegment>) this.GetTileSegmentsToAdd(tileCell));
      this.AddPathSegments(addSegments);
      this.RemoveCoincidentPathSegments();
    }

    public void SetTileAt(Tile tile, int col, int row, ref List<PathSegment> segments)
    {
      TileCell tileCell = this.SetTileAtWithoutPaths(tile, col, row);
      segments.AddRange((IEnumerable<PathSegment>) this.GetTileSegmentsToAdd(tileCell));
    }

    public void SetTileAt(int styleGroupId, int col, int row)
    {
      Tile tile1 = (Tile) null;
      if (col > 0 && row > 0 && this.Tiles[row - 1, col - 1] != null && this.Tiles[row - 1, col - 1].Tile.StyleGroupId == styleGroupId)
        tile1 = this.Tiles[row - 1, col - 1].Tile;
      Tile tile2 = (Tile) null;
      if (row > 0 && this.Tiles[row - 1, col] != null && this.Tiles[row - 1, col].Tile.StyleGroupId == styleGroupId)
        tile2 = this.Tiles[row - 1, col].Tile;
      Tile tile3 = (Tile) null;
      if (col < this.Columns - 1 && row > 0 && this.Tiles[row - 1, col + 1] != null && this.Tiles[row - 1, col + 1].Tile.StyleGroupId == styleGroupId)
        tile3 = this.Tiles[row - 1, col + 1].Tile;
      Tile tile4 = (Tile) null;
      if (col < this.Columns - 1 && this.Tiles[row, col + 1] != null && this.Tiles[row, col + 1].Tile.StyleGroupId == styleGroupId)
        tile4 = this.Tiles[row, col + 1].Tile;
      Tile tile5 = (Tile) null;
      if (col < this.Columns - 1 && row < this.Rows - 1 && this.Tiles[row + 1, col + 1] != null && this.Tiles[row + 1, col + 1].Tile.StyleGroupId == styleGroupId)
        tile5 = this.Tiles[row + 1, col + 1].Tile;
      Tile tile6 = (Tile) null;
      if (row < this.Rows - 1 && this.Tiles[row + 1, col] != null && this.Tiles[row + 1, col].Tile.StyleGroupId == styleGroupId)
        tile6 = this.Tiles[row + 1, col].Tile;
      Tile tile7 = (Tile) null;
      if (col > 0 && row < this.Rows - 1 && this.Tiles[row + 1, col - 1] != null && this.Tiles[row + 1, col - 1].Tile.StyleGroupId == styleGroupId)
        tile7 = this.Tiles[row + 1, col - 1].Tile;
      Tile tile8 = (Tile) null;
      if (col > 0 && this.Tiles[row, col - 1] != null && this.Tiles[row, col - 1].Tile.StyleGroupId == styleGroupId)
        tile8 = this.Tiles[row, col - 1].Tile;
      WalkFlags flags1 = WalkFlags.All;
      if (tile2 != null)
      {
        flags1 &= ~WalkFlags.Top;
        WalkFlags flags2 = tile2._walkFlags & ~WalkFlags.Bottom;
        if (tile1 != null && tile1.StyleGroupId == styleGroupId && tile8 == null)
          flags2 |= WalkFlags.LLCorner;
        if (tile3 != null && tile3.StyleGroupId == styleGroupId && tile4 == null)
          flags2 |= WalkFlags.LRCorner;
        this.Tiles[row - 1, col].Tile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(styleGroupId, flags2);
      }
      if (tile8 != null)
      {
        flags1 &= ~WalkFlags.Left;
        WalkFlags flags3 = tile8._walkFlags & ~WalkFlags.Right;
        if (tile1 != null && tile1.StyleGroupId == styleGroupId && tile2 == null)
          flags3 |= WalkFlags.URCorner;
        if (tile7 != null && tile7.StyleGroupId == styleGroupId && tile6 == null)
          flags3 |= WalkFlags.LRCorner;
        this.Tiles[row, col - 1].Tile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(styleGroupId, flags3);
      }
      if (tile6 != null)
      {
        flags1 &= ~WalkFlags.Bottom;
        WalkFlags flags4 = tile6._walkFlags & ~WalkFlags.Top;
        if (tile7 != null && tile7.StyleGroupId == styleGroupId && tile8 == null)
          flags4 |= WalkFlags.ULCorner;
        if (tile5 != null && tile5.StyleGroupId == styleGroupId && tile4 == null)
          flags4 |= WalkFlags.URCorner;
        this.Tiles[row + 1, col].Tile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(styleGroupId, flags4);
      }
      if (tile4 != null)
      {
        flags1 &= ~WalkFlags.Right;
        WalkFlags flags5 = tile4._walkFlags & ~WalkFlags.Left;
        if (tile3 != null && tile3.StyleGroupId == styleGroupId && tile2 == null)
          flags5 |= WalkFlags.ULCorner;
        if (tile5 != null && tile5.StyleGroupId == styleGroupId && tile6 == null)
          flags5 |= WalkFlags.LLCorner;
        this.Tiles[row, col + 1].Tile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(styleGroupId, flags5);
      }
      if (tile1 != null)
        this.Tiles[row - 1, col - 1].Tile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(styleGroupId, tile1._walkFlags & ~WalkFlags.LRCorner);
      if (tile3 != null)
        this.Tiles[row - 1, col + 1].Tile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(styleGroupId, tile3._walkFlags & ~WalkFlags.LLCorner);
      if (tile5 != null)
        this.Tiles[row + 1, col + 1].Tile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(styleGroupId, tile5._walkFlags & ~WalkFlags.ULCorner);
      if (tile7 != null)
        this.Tiles[row + 1, col - 1].Tile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(styleGroupId, tile7._walkFlags & ~WalkFlags.URCorner);
      if (tile2 != null && tile8 != null && tile1 == null)
        flags1 |= WalkFlags.ULCorner;
      if (tile2 != null && tile4 != null && tile3 == null)
        flags1 |= WalkFlags.URCorner;
      if (tile6 != null && tile8 != null && tile7 == null)
        flags1 |= WalkFlags.LLCorner;
      if (tile6 != null && tile4 != null && tile5 == null)
        flags1 |= WalkFlags.LRCorner;
      this.SetTileAt(Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(styleGroupId, flags1), col, row);
    }

    public void RemoveCellAt(int col, int row)
    {
      if (this.Tiles[row, col] == null)
        return;
      this.RemoveTileAt(col, row);
      this.Tiles[row, col] = (TileCell) null;
    }

    public void RemoveTileAt(int col, int row)
    {
      if (this.Tiles[row, col] == null || this.Tiles[row, col].Tile == null)
        return;
      int styleGroupId = this.Tiles[row, col].Tile.StyleGroupId;
      TileCell tileCell1 = (TileCell) null;
      if (row > 0 && this.Tiles[row - 1, col] != null)
        tileCell1 = this.Tiles[row - 1, col];
      TileCell tileCell2 = (TileCell) null;
      if (col < this.Columns - 1 && this.Tiles[row, col + 1] != null)
        tileCell2 = this.Tiles[row, col + 1];
      TileCell tileCell3 = (TileCell) null;
      if (row < this.Rows - 1 && this.Tiles[row + 1, col] != null)
        tileCell3 = this.Tiles[row + 1, col];
      TileCell tileCell4 = (TileCell) null;
      if (col > 0 && this.Tiles[row, col - 1] != null)
        tileCell4 = this.Tiles[row, col - 1];
      bool flag1 = false;
      bool flag2 = false;
      bool flag3 = false;
      bool flag4 = false;
      List<PathSegment> segments = new List<PathSegment>(16);
      List<PathSegment> removeSegments = new List<PathSegment>(4);
      segments.Clear();
      removeSegments.Clear();
      removeSegments.AddRange((IEnumerable<PathSegment>) this.Tiles[row, col].Segments);
      this.RemovePathSegments(removeSegments);
      if (this.Tiles[row, col].DrawUnderWater)
        this._drawBackgroundTilesList.Remove(this.Tiles[row, col]);
      else
        this._drawForegroundTilesList.Remove(this.Tiles[row, col]);
      this.Tiles[row, col].SetTile((Tile) null);
      this.Tiles[row, col] = (TileCell) null;
      if (tileCell4 != null && tileCell4.Tile != null)
      {
        if (tileCell4.Tile.StyleGroupId == styleGroupId)
        {
          WalkFlags flags = (tileCell4.Tile._walkFlags | WalkFlags.Right) & ~(WalkFlags.URCorner | WalkFlags.LRCorner);
          Tile matchingWalkFlags = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(tileCell4.Tile.StyleGroupId, flags);
          this.SetTileAt(matchingWalkFlags, col - 1, row, ref segments);
          flag2 = (matchingWalkFlags._walkFlags & WalkFlags.Right) == WalkFlags.Right;
        }
        else
          segments.Add(tileCell4.RightSegment);
      }
      if (tileCell1 != null && tileCell1.Tile != null)
      {
        if (tileCell1.Tile.StyleGroupId == styleGroupId)
        {
          WalkFlags flags = (tileCell1.Tile._walkFlags | WalkFlags.Bottom) & ~(WalkFlags.LRCorner | WalkFlags.LLCorner);
          Tile matchingWalkFlags = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(tileCell1.Tile.StyleGroupId, flags);
          this.SetTileAt(matchingWalkFlags, col, row - 1, ref segments);
          flag1 = (matchingWalkFlags._walkFlags & WalkFlags.Bottom) == WalkFlags.Bottom;
        }
        else
          segments.Add(tileCell1.BottomSegment);
      }
      if (tileCell2 != null && tileCell2.Tile != null)
      {
        if (tileCell2.Tile.StyleGroupId == styleGroupId)
        {
          WalkFlags flags = (tileCell2.Tile._walkFlags | WalkFlags.Left) & ~(WalkFlags.ULCorner | WalkFlags.LLCorner);
          Tile matchingWalkFlags = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(tileCell2.Tile.StyleGroupId, flags);
          this.SetTileAt(matchingWalkFlags, col + 1, row, ref segments);
          flag4 = (matchingWalkFlags._walkFlags & WalkFlags.Left) == WalkFlags.Left;
        }
        else
          segments.Add(tileCell2.LeftSegment);
      }
      if (tileCell3 != null && tileCell3.Tile != null)
      {
        if (tileCell3.Tile.StyleGroupId == styleGroupId)
        {
          WalkFlags flags = (tileCell3.Tile._walkFlags | WalkFlags.Top) & ~(WalkFlags.ULCorner | WalkFlags.URCorner);
          Tile matchingWalkFlags = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(tileCell3.Tile.StyleGroupId, flags);
          this.SetTileAt(matchingWalkFlags, col, row + 1, ref segments);
          flag3 = (matchingWalkFlags._walkFlags & WalkFlags.Top) == WalkFlags.Top;
        }
        else
          segments.Add(tileCell3.TopSegment);
      }
      if (row - 1 >= 0 && col - 1 >= 0)
        segments.AddRange((IEnumerable<PathSegment>) this.GetNeighborTileSegmentsToAdd(this.Tiles[row - 1, col - 1]));
      if (row - 1 >= 0 && col + 1 < this.Columns)
        segments.AddRange((IEnumerable<PathSegment>) this.GetNeighborTileSegmentsToAdd(this.Tiles[row - 1, col + 1]));
      if (row + 1 < this.Rows && col - 1 >= 0)
        segments.AddRange((IEnumerable<PathSegment>) this.GetNeighborTileSegmentsToAdd(this.Tiles[row + 1, col - 1]));
      if (row + 1 < this.Rows && col + 1 < this.Columns)
        segments.AddRange((IEnumerable<PathSegment>) this.GetNeighborTileSegmentsToAdd(this.Tiles[row + 1, col + 1]));
      this.AddPathSegments(segments);
      this.RemoveCoincidentPathSegments();
      if (flag1 && flag2 && this.Tiles[row - 1, col - 1] != null)
      {
        Tile tile = this.Tiles[row - 1, col - 1].Tile;
        if (tile != null && tile.StyleGroupId == styleGroupId)
          this.Tiles[row - 1, col - 1].Tile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(styleGroupId, tile._walkFlags | WalkFlags.LRCorner);
      }
      if (flag1 && flag4 && this.Tiles[row - 1, col + 1] != null)
      {
        Tile tile = this.Tiles[row - 1, col + 1].Tile;
        if (tile != null && tile.StyleGroupId == styleGroupId)
          this.Tiles[row - 1, col + 1].Tile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(styleGroupId, tile._walkFlags | WalkFlags.LLCorner);
      }
      if (flag4 && flag3 && this.Tiles[row + 1, col + 1] != null)
      {
        Tile tile = this.Tiles[row + 1, col + 1].Tile;
        if (tile != null && tile.StyleGroupId == styleGroupId)
          this.Tiles[row + 1, col + 1].Tile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(styleGroupId, tile._walkFlags | WalkFlags.ULCorner);
      }
      if (!flag2 || !flag3 || this.Tiles[row + 1, col - 1] == null)
        return;
      Tile tile1 = this.Tiles[row + 1, col - 1].Tile;
      if (tile1 == null || tile1.StyleGroupId != styleGroupId)
        return;
      this.Tiles[row + 1, col - 1].Tile = Stage.CurrentStage.StageData.GetTileMatchingWalkFlags(styleGroupId, tile1._walkFlags | WalkFlags.URCorner);
    }

    public TileCellCoords GetCoordsFromPosition(Vector2 pos)
    {
      return new TileCellCoords((int) ((double) pos.X / (double) this.TileWidth), (int) ((double) pos.Y / (double) this.TileHeight));
    }

    public TileCell GetTileCellAt(Vector2 v)
    {
      int index1 = (int) ((double) v.X / (double) this.TileWidth);
      int index2 = (int) ((double) v.Y / (double) this.TileHeight);
      return this.Tiles.GetLength(0) <= index2 || this.Tiles.GetLength(1) <= index1 ? (TileCell) null : this.Tiles[index2, index1];
    }

    public Tile GetTileAt(Vector2 v) => this.GetTileCellAt(v)?.Tile;

    public Tile GetTileAt(int row, int col)
    {
      if (row >= this.Rows || col >= this.Columns)
        return (Tile) null;
      return this.Tiles[row, col] == null ? (Tile) null : this.Tiles[row, col].Tile;
    }

    public TileCellCoords GetTileCellCoordsAt(Vector2 v)
    {
      return new TileCellCoords((int) ((double) v.X / (double) this.TileWidth), (int) ((double) v.Y / (double) this.TileHeight));
    }

    public void Resize(int cols, int rows)
    {
      TileCell[,] tileCellArray = new TileCell[rows, cols];
      for (int index1 = 0; index1 < cols && index1 < this.Columns; ++index1)
      {
        for (int index2 = 0; index2 < rows && index2 < this.Rows; ++index2)
          tileCellArray[index2, index1] = this.Tiles[index2, index1];
      }
      this.Columns = cols;
      this.Rows = rows;
      this.Tiles = tileCellArray;
      this.ComputePaths();
      this.UpdateBoundingBox();
      this.RefreshDrawLists();
    }

    private void RefreshDrawLists()
    {
      this._drawBackgroundTilesList.Clear();
      this._drawForegroundTilesList.Clear();
      for (int index1 = 0; index1 < this.Rows; ++index1)
      {
        for (int index2 = 0; index2 < this.Columns; ++index2)
        {
          if (this.Tiles[index1, index2] != null)
          {
            if (!this.Tiles[index1, index2].DrawUnderWater)
              this._drawForegroundTilesList.Add(this.Tiles[index1, index2]);
            else
              this._drawBackgroundTilesList.Add(this.Tiles[index1, index2]);
          }
        }
      }
    }

    public bool IsObjectInBounds(StageObject obj) => obj.Collides(this.BoundingBox);

    public void RefreshTiles()
    {
      for (int index1 = 0; index1 < this.Columns; ++index1)
      {
        for (int index2 = 0; index2 < this.Rows; ++index2)
        {
          if (this.Tiles[index2, index1] != null && this.Tiles[index2, index1].Tile != null)
            this.Tiles[index2, index1].Tile = Stage.CurrentStage.StageData.GetTile(this.Tiles[index2, index1].Tile._id);
        }
      }
    }

    public int Collides(
      StageObject obj,
      ref OOBoundingBox bbOld,
      ref OOBoundingBox bbNew,
      out Vector2 retIntersectPt,
      out BoardPathNode retNode,
      out MovingObject.CollidingCorner cornerOfCollision,
      out Vector2 bbCollidingPoint)
    {
      int num1 = 0;
      cornerOfCollision = MovingObject.CollidingCorner.Node;
      retNode = (BoardPathNode) null;
      bbCollidingPoint = Vector2.Zero;
      PathSegment pathSegment1 = new PathSegment(bbOld.P0, bbNew.P0);
      PathSegment pathSegment2 = new PathSegment(bbOld.P1, bbNew.P1);
      PathSegment pathSegment3 = new PathSegment(bbOld.P3, bbNew.P3);
      PathSegment pathSegment4 = new PathSegment(bbOld.P2, bbNew.P2);
      float num2 = 999999f;
      Vector2 I = new Vector2(0.0f, 0.0f);
      retIntersectPt = I;
      foreach (QuadtreeNode quadtreeNode in obj.QuadtreeNodes)
      {
        foreach (BoardPathNode boardPathNode in quadtreeNode.ObjectLists[2])
        {
          if ((double) pathSegment2.Classify(boardPathNode.Value) < 0.0)
          {
            if (pathSegment2.Collides(boardPathNode.Value, ref I))
            {
              float num3 = Vector2.Distance(new Vector2(I.X, I.Y), pathSegment2.P0);
              if ((double) num3 < (double) num2)
              {
                retNode = boardPathNode;
                num2 = num3;
                retIntersectPt = I;
                cornerOfCollision = MovingObject.CollidingCorner.UpperRight;
                ++num1;
                bbCollidingPoint = pathSegment2.P1;
              }
            }
            if (pathSegment1.Collides(boardPathNode.Value, ref I))
            {
              float num4 = Vector2.Distance(new Vector2(I.X, I.Y), pathSegment1.P0);
              if ((double) num4 < (double) num2)
              {
                retNode = boardPathNode;
                num2 = num4;
                retIntersectPt = I;
                cornerOfCollision = MovingObject.CollidingCorner.UpperLeft;
                ++num1;
                bbCollidingPoint = pathSegment1.P1;
              }
            }
            if (pathSegment4.Collides(boardPathNode.Value, ref I))
            {
              float num5 = Vector2.Distance(new Vector2(I.X, I.Y), pathSegment4.P0);
              if ((double) num5 < (double) num2)
              {
                retNode = boardPathNode;
                num2 = num5;
                retIntersectPt = I;
                cornerOfCollision = MovingObject.CollidingCorner.LowerRight;
                ++num1;
                bbCollidingPoint = pathSegment4.P1;
              }
            }
            if (pathSegment3.Collides(boardPathNode.Value, ref I))
            {
              float num6 = Vector2.Distance(new Vector2(I.X, I.Y), pathSegment3.P0);
              if ((double) num6 < (double) num2)
              {
                retNode = boardPathNode;
                num2 = num6;
                retIntersectPt = I;
                cornerOfCollision = MovingObject.CollidingCorner.LowerLeft;
                ++num1;
                bbCollidingPoint = pathSegment3.P1;
              }
            }
          }
        }
      }
      if (retNode == null)
        return 0;
      int num7 = num1;
      int num8 = num7 + 1;
      return num7;
    }

    public bool Collides(StageObject obj, PathSegment S1, ref Point I, ref BoardPathNode retNode)
    {
      this.collisionNodes.Clear();
      foreach (QuadtreeNode quadtreeNode in obj.QuadtreeNodes)
      {
        foreach (IQuadtreeContainable quadtreeContainable in quadtreeNode.ObjectLists[2])
        {
          BoardPathNode boardPathNode = (BoardPathNode) quadtreeContainable;
          if ((double) S1.Classify(boardPathNode.Value) < 0.0 && S1.Collides(boardPathNode.Value, ref I))
            this.collisionNodes.Add((BoardPathNode) quadtreeContainable);
        }
      }
      if (this.collisionNodes.Count <= 0)
        return false;
      if (this.collisionNodes.Count == 1)
      {
        retNode = this.collisionNodes[0];
      }
      else
      {
        float num1 = -1f;
        for (int index = 1; index < this.collisionNodes.Count; ++index)
        {
          float num2 = this.collisionNodes[index].Value.P0.Y;
          float y = this.collisionNodes[index].Value.P1.Y;
          if ((double) y < (double) num2)
            num2 = y;
          if ((double) num1 == -1.0 || (double) num2 < (double) num1)
          {
            num1 = num2;
            retNode = this.collisionNodes[index];
          }
        }
      }
      return true;
    }

    public BoardPathNode PathCollidesWithObject(StageObject obj)
    {
      foreach (BoardPathNode boardPathNode in this.Paths.Container)
      {
        if (boardPathNode.Collides((IQuadtreeContainable) obj))
          return boardPathNode;
      }
      return (BoardPathNode) null;
    }

    public BoardPathNode PathCollidesWithBoundingSquare(BoundingSquare bs)
    {
      foreach (BoardPathNode boardPathNode in this.Paths.Container)
      {
        if (boardPathNode.Collides(bs))
          return boardPathNode;
      }
      return (BoardPathNode) null;
    }

    public BoardPathNode PathCollidesWithVector(Vector2 p0, Vector2 p1)
    {
      foreach (BoardPathNode boardPathNode in this.Paths.Container)
      {
        if (boardPathNode.Collides(p0, p1))
          return boardPathNode;
      }
      return (BoardPathNode) null;
    }

    public void InitFromDataFileRecord(DataFileRecord boardRecord)
    {
      this.Columns = boardRecord.GetFieldValue<int>("columns");
      this.Rows = boardRecord.GetFieldValue<int>("rows");
      DataFileRecord dataFileRecord = boardRecord.SelectRecord("Tiles");
      this.Tiles = new TileCell[this.Rows, this.Columns];
      this.UpdateBoundingBox();
      foreach (DataFileRecord selectRecord in dataFileRecord.SelectRecords("Tile"))
      {
        string fieldValue1 = selectRecord.GetFieldValue<string>("id");
        int fieldValue2 = selectRecord.GetFieldValue<int>("boardX");
        int fieldValue3 = selectRecord.GetFieldValue<int>("boardY");
        Tile tile = Stage.CurrentStage.StageData.GetTile(fieldValue1);
        this.Tiles[fieldValue3, fieldValue2] = new TileCell(this, tile, fieldValue2, fieldValue3);
      }
      this.RefreshDrawLists();
    }

    public virtual DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord1 = new DataFileRecord(nameof (Board));
      dataFileRecord1.AddField("columns", (object) this.Columns);
      dataFileRecord1.AddField("rows", (object) this.Rows);
      DataFileRecord record = new DataFileRecord("Tiles");
      record.AddField("width", (object) this.TileWidth);
      record.AddField("height", (object) this.TileHeight);
      dataFileRecord1.AddRecord(record);
      for (int index1 = 0; index1 < this.Rows; ++index1)
      {
        for (int index2 = 0; index2 < this.Columns; ++index2)
        {
          if (this.Tiles[index1, index2] != null && this.Tiles[index1, index2].Tile != null)
          {
            DataFileRecord dataFileRecord2 = this.Tiles[index1, index2].Tile.ToDataFileRecord(context);
            dataFileRecord2.AddField("boardX", (object) index2.ToString());
            dataFileRecord2.AddField("boardY", (object) index1.ToString());
            record.AddRecord(dataFileRecord2);
          }
        }
      }
      return dataFileRecord1;
    }
  }
}
