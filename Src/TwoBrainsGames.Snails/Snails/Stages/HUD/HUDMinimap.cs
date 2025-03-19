
// Type: TwoBrainsGames.Snails.Stages.HUD.HUDMinimap
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.Snails.StageObjects;


namespace TwoBrainsGames.Snails.Stages.HUD
{
  internal class HUDMinimap : HUDItem
  {
    private const int MAP_WIDTH = 180;
    private const int MAP_HEIGHT = 101;
    private int tileSizeX;
    private int tileSizeY;
    private List<HUDMinimap.MinimapItem> items = new List<HUDMinimap.MinimapItem>();
    private int camX;
    private int camY;
    private int camW;
    private int camH;

    public override void Initialize(Vector2 position)
    {
      base.Initialize(position);
      this.ComputePositions();
    }

    public void ComputePositions()
    {
      int width = Stage.CurrentStage.Board.Width;
      int height = Stage.CurrentStage.Board.Height;
      int tileWidth = Stage.CurrentStage.Board.TileWidth;
      int rows = Stage.CurrentStage.Board.Rows;
      int columns = Stage.CurrentStage.Board.Columns;
      this.tileSizeX = 180 * tileWidth / width;
      this.tileSizeY = 101 * tileWidth / height;
      for (int index1 = 0; index1 < rows; ++index1)
      {
        for (int index2 = 0; index2 < columns; ++index2)
        {
          TileCell tile = Stage.CurrentStage.Board.Tiles[index1, index2];
          if (tile != null && tile.Tile != null && tile.Tile.Sprite != null)
          {
            HUDMinimap.MinimapItem minimapItem = new HUDMinimap.MinimapItem();
            minimapItem.type = 0;
            if (tile.Tile.IsBreakable)
              minimapItem.type = 1;
            minimapItem.position = new Vector2((float) ((int) this._position.X + index2 * this.tileSizeX), (float) ((int) this._position.Y + index1 * this.tileSizeY));
            this.items.Add(minimapItem);
          }
        }
      }
    }

    public void SetCameraProjections()
    {
      this.camW = (int) Stage.CurrentStage.StageHUD._stageArea.Width * 180 / Stage.CurrentStage.Board.Width;
      this.camH = Game1.GameSettings.ScreenHeight * 101 / Stage.CurrentStage.Board.Height;
    }

    private int ConvertToMinimapPointX(int posX) => posX * 180 / Stage.CurrentStage.Board.Width;

    private int ConvertToMinimapPointY(int posY) => posY * 101 / Stage.CurrentStage.Board.Height;

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      this.camX = this.ConvertToMinimapPointX((int) Stage.CurrentStage.Camera.Position.X);
      this.camY = this.ConvertToMinimapPointY((int) Stage.CurrentStage.Camera.Position.Y);
      this.camX += (int) this._position.X + 1;
      this.camY += (int) this._position.Y + 1;
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      BrainGame.DrawRectangleFilled(spriteBatch, new Rectangle((int) this._position.X, (int) this._position.Y, 180, 101), Color.Black);
      foreach (HUDMinimap.MinimapItem minimapItem in this.items)
      {
        Color color = minimapItem.type == 0 ? Color.Gray : Color.LightGray;
        BrainGame.DrawRectangleFilled(spriteBatch, new Rectangle((int) minimapItem.position.X, (int) minimapItem.position.Y, this.tileSizeX, this.tileSizeY), color);
      }
      BrainGame.DrawRectangleFrame(spriteBatch, new Rectangle((int) this._position.X, (int) this._position.Y, 180, 101), Color.Blue, 1);
      BrainGame.DrawRectangleFrame(spriteBatch, new Rectangle(this.camX, this.camY, this.camW, this.camH), Color.Red, 1);
      foreach (Snail snail in Stage.CurrentStage.Snails)
      {
        int x = this.ConvertToMinimapPointX((int) snail.Position.X) + (int) this._position.X;
        int y = this.ConvertToMinimapPointY((int) snail.Position.Y) + (int) this._position.Y - this.tileSizeY;
        BrainGame.DrawRectangleFilled(spriteBatch, new Rectangle(x, y, 5, 5), Color.YellowGreen);
      }
    }

    private struct MinimapItem
    {
      public int type;
      public Vector2 position;
    }
  }
}
