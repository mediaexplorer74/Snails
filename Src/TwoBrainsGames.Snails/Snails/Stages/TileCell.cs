
// Type: TwoBrainsGames.Snails.Stages.TileCell
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.ParticlesEffects;
using TwoBrainsGames.BrainEngine.Graphics;


namespace TwoBrainsGames.Snails.Stages
{
  public class TileCell
  {
    private Board _board;

    public int BoardX { get; set; }

    public int BoardY { get; set; }

    public Vector2 Position { get; set; }

    public Tile Tile { get; set; }

    public int Width => this._board.TileWidth;

    public int Height => this._board.TileHeight;

    public int BoardWidth => this._board.Width;

    public int BoardHeight => this._board.Height;

    public PathSegment[] Segments { get; private set; }

    public PathSegment TopSegment => this.Segments[0];

    public PathSegment RightSegment => this.Segments[1];

    public PathSegment BottomSegment => this.Segments[2];

    public PathSegment LeftSegment => this.Segments[3];

    public bool DrawUnderWater
    {
      get => this.Tile != null && this.Tile.DrawUnderWater;
      set
      {
        if (this.Tile == null)
          return;
        this.Tile.DrawUnderWater = value;
      }
    }

    public TileCell(Board board, Tile tile, int col, int row)
    {
      this._board = board;
      this.Tile = tile;
      this.BoardX = col;
      this.BoardY = row;
      this.Position = new Vector2((float) (this.BoardX * this.Width), (float) (this.BoardY * this.Height));
      this.ComputeSegments();
    }

    public void SetTile(Tile tile)
    {
      this.Tile = tile;
      if (tile != null)
        this.ComputeSegments();
      else
        this.Segments = (PathSegment[]) null;
    }

    public bool IsEmpty() => this.Tile == null;

    internal void ComputeSegments()
    {
      this.Segments = new PathSegment[4];
      if (this.Tile == null)
        return;
      int x1 = this.BoardX * this.Width;
      int y1 = this.BoardY * this.Height;
      int x2 = x1 + this.Width;
      int y2 = y1 + this.Height;
      if (this.Tile._topPath != PathBehaviour.None)
      {
        PathSegmentBehavior behavior = y1 != 0 ? (PathSegmentBehavior) this.Tile._topPath : PathSegmentBehavior.ReverseWalk;
        this.Segments[0] = new PathSegment(new Vector2((float) x1, (float) y1), new Vector2((float) x2, (float) y1), behavior, this.Tile.IsBreakable);
      }
      if (this.Tile._rightPath != PathBehaviour.None)
      {
        PathSegmentBehavior behavior = x2 != this.BoardWidth ? (PathSegmentBehavior) this.Tile._rightPath : PathSegmentBehavior.ReverseWalk;
        this.Segments[1] = new PathSegment(new Vector2((float) x2, (float) y1), new Vector2((float) x2, (float) y2), behavior, this.Tile.IsBreakable);
      }
      if (this.Tile._bottomPath != PathBehaviour.None)
      {
        PathSegmentBehavior behavior = y2 != this.BoardHeight ? (PathSegmentBehavior) this.Tile._bottomPath : PathSegmentBehavior.ReverseWalk;
        this.Segments[2] = new PathSegment(new Vector2((float) x2, (float) y2), new Vector2((float) x1, (float) y2), behavior, this.Tile.IsBreakable);
      }
      if (this.Tile._leftPath == PathBehaviour.None)
        return;
      PathSegmentBehavior behavior1 = x1 != 0 ? (PathSegmentBehavior) this.Tile._leftPath : PathSegmentBehavior.ReverseWalk;
      this.Segments[3] = new PathSegment(new Vector2((float) x1, (float) y2), new Vector2((float) x1, (float) y1), behavior1, this.Tile.IsBreakable);
    }

    public TileCell Clone()
    {
      return new TileCell(this._board, this.Tile, this.BoardX, this.BoardY)
      {
        Segments = this.Segments
      };
    }

    public void Break()
    {
      if (this.Tile == null)
        return;
      Sprite fragmentSprite = Stage.CurrentStage.StageData.GetFragmentSprite(this.Tile.StyleGroupId);
      if (fragmentSprite == null)
        return;
      Vector2 vector2 = new Vector2((float) (this.BoardX * Stage.CurrentStage.Board.TileWidth + Stage.CurrentStage.Board.TileWidth / 2), (float) (this.BoardY * Stage.CurrentStage.Board.TileHeight + Stage.CurrentStage.Board.TileWidth / 2));
      Stage.CurrentStage.Particles.Add((ParticlesEffect) new ExplosionEffect(vector2.X, vector2.Y, fragmentSprite, 1f, Game1.GameSettings.Gravity)
      {
        Color = this.Tile.BlendColor
      });
    }
  }
}
