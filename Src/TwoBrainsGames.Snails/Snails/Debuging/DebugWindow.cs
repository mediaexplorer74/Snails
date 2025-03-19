
// Type: TwoBrainsGames.Snails.Debuging.DebugWindow
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;


namespace TwoBrainsGames.Snails.Debuging
{
  public class DebugWindow(Game1 game) : DrawableGameComponent((Game) game)
  {
    public const Keys KEY_SHOW_HIDE_HELP = Keys.F1;
    public const Keys KEY_SHOW_HIDE_SPRITES = Keys.F2;
    public const Keys KEY_SHOW_HIDE_TILES = Keys.T;
    public const Keys KEY_SHOW_HIDE_BOUNDINGBOXES = Keys.B;
    public const Keys KEY_SHOW_HIDE_PATHS = Keys.P;
    public const Keys KEY_SHOW_QUADTREE = Keys.Q;
    public const Keys KEY_STAGE_EDITOR = Keys.F11;
    public const Keys KEY_RELOAD_STAGE = Keys.R;
    public const Keys KEY_NEXT_STAGE = Keys.M;
    public const Keys KEY_PREV_STAGE = Keys.N;
    public const Keys KEY_SHOW_HIDE_DEBUG_INFO = Keys.D;
    public const Keys KEY_ENABLE_AVERAGES = Keys.A;
    public const Keys KEY_DEBUG_INFO_POSITION = Keys.I;
    public const Keys KEY_GENERATE_THUMBS = Keys.G;
    private Rectangle _Rect;
    private Color _BackColor;
    private Vector2 _Position;
    private SpriteFont _Font;
    private Color _Textcolor;
    private List<DebugWindow.KeyHelpItem> _KeyList;

    protected override void LoadContent()
    {
      int x = (int) ((double) BrainGame.ScreenWidth * 0.10000000149011612);
      int y = (int) ((double) BrainGame.ScreenHeight * 0.10000000149011612);
      this._Rect = new Rectangle(x, y, BrainGame.ScreenWidth - x * 2, BrainGame.ScreenHeight - y * 2);
      this._Position = new Vector2((float) x, (float) y);
      this._BackColor = new Color(0, 0, 100, 128);
      this._Font = BrainGame.ResourceManager.Load<SpriteFont>("fonts/dbgWindow", ResourceManager.ResourceManagerCacheType.Static);
      this._KeyList = new List<DebugWindow.KeyHelpItem>();
      this._KeyList.Add(new DebugWindow.KeyHelpItem(Keys.F1, 5, "Show / hide this help"));
      this._KeyList.Add(new DebugWindow.KeyHelpItem(Keys.F2, 9, "Show / hide sprites"));
      this._KeyList.Add(new DebugWindow.KeyHelpItem(Keys.T, 8, "Show / hide tiles"));
      this._KeyList.Add(new DebugWindow.KeyHelpItem(Keys.P, 11, "Show / hide paths"));
      this._KeyList.Add(new DebugWindow.KeyHelpItem(Keys.Q, 10, "Show / hide quadtree"));
      this._KeyList.Add(new DebugWindow.KeyHelpItem(Keys.B, 0, "Show / hide bounding boxes"));
      this._KeyList.Add(new DebugWindow.KeyHelpItem(Keys.D, 1, "Show / hide debug info"));
      this._KeyList.Add(new DebugWindow.KeyHelpItem(Keys.A, 3, "Enable / disable averages"));
      this._KeyList.Add(new DebugWindow.KeyHelpItem(Keys.R, 2, "Reset stage"));
      this._KeyList.Add(new DebugWindow.KeyHelpItem(Keys.I, 12, "Change Debug Info position"));
      this._KeyList.Add(new DebugWindow.KeyHelpItem(Keys.M, 14, "Go to next stage"));
      this._KeyList.Add(new DebugWindow.KeyHelpItem(Keys.N, 15, "Go to previous stage"));
      this._KeyList.Add(new DebugWindow.KeyHelpItem(Keys.G, 0, "Generate Stage Thumbnails"));
      this._Textcolor = Color.Yellow;
    }

    public override void Update(GameTime gameTime)
    {
      int num = this.Visible ? 1 : 0;
    }

    public override void Draw(GameTime gameTime)
    {
      BrainGame.SpriteBatch.Begin();
      BrainGame.DrawRectangleFilled(BrainGame.SpriteBatch, this._Rect, this._BackColor);
      float x = 20f;
      float y = 20f;
      foreach (DebugWindow.KeyHelpItem key in this._KeyList)
      {
        Vector2 vector2 = new Vector2(x, y);
        if (!Game1.GameSettings.UseGamepad)
          BrainGame.SpriteBatch.DrawString(this._Font, string.Format("({0})", (object) key._Key.ToString()), this._Position + vector2 + new Vector2(0.0f, 15f), this._Textcolor, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
        BrainGame.SpriteBatch.DrawString(this._Font, key._HelpText, this._Position + vector2 + new Vector2(55f, 15f), this._Textcolor, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
        y += 50f;
        if ((double) y + 80.0 > (double) this._Rect.Height)
        {
          y = 20f;
          x += 300f;
        }
      }
      BrainGame.SpriteBatch.End();
    }

    private struct KeyHelpItem(Keys key, int xboxControlFrameNr, string helpText)
    {
      public int _XboxControlFrameNr = xboxControlFrameNr;
      public string _HelpText = helpText;
      public Keys _Key = key;
    }
  }
}
