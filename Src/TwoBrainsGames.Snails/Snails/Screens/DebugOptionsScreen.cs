
// Type: TwoBrainsGames.Snails.Screens.DebugOptionsScreen
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Input;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.Screens
{
  internal class DebugOptionsScreen(ScreenNavigator owner) : Screen(owner)
  {
    private const int THUMBNAIL_WIDTH = 200;
    private const int THUMBNAIL_HEIGHT = 127;
    public DebugOptionsInput Input;
    private Rectangle _Rect;
    private Color _BackColor;
    private Vector2 _Position;
    private SpriteFont _Font;
    private Color _Textcolor;
    private List<DebugOptionsScreen.KeyHelpItem> _KeyList;

    public override void OnLoad()
    {
      this._inputController = (InputBase) new DebugOptionsInput();
      this._inputController.Initialize();
      this.Input = (DebugOptionsInput) this._inputController;
      int num1 = (int) ((double) BrainGame.ScreenWidth * 0.10000000149011612);
      int num2 = (int) ((double) BrainGame.ScreenHeight * 0.10000000149011612);
      this._Position = new Vector2((float) (num1 - (int) BrainGame.DefaultCamera.Origin.X), (float) (num2 - (int) BrainGame.DefaultCamera.Origin.Y));
      this._Rect = new Rectangle((int) this._Position.X, (int) this._Position.Y, BrainGame.ScreenWidth - num1 * 2, BrainGame.ScreenHeight - num2 * 2);
      this._BackColor = new Color(0, 0, 100, 128);
      this._KeyList = new List<DebugOptionsScreen.KeyHelpItem>();
      this._KeyList.Add(new DebugOptionsScreen.KeyHelpItem(Keys.F9, 5, "Show / hide this help"));
      this._KeyList.Add(new DebugOptionsScreen.KeyHelpItem(Keys.F2, 9, "Show / hide sprites"));
      this._KeyList.Add(new DebugOptionsScreen.KeyHelpItem(Keys.T, 8, "Show / hide tiles"));
      this._KeyList.Add(new DebugOptionsScreen.KeyHelpItem(Keys.P, 11, "Show / hide paths"));
      this._KeyList.Add(new DebugOptionsScreen.KeyHelpItem(Keys.Q, 10, "Show / hide quadtree"));
      this._KeyList.Add(new DebugOptionsScreen.KeyHelpItem(Keys.B, 0, "Show / hide bounding boxes"));
      this._KeyList.Add(new DebugOptionsScreen.KeyHelpItem(Keys.D, 1, "Show / hide debug info"));
      this._KeyList.Add(new DebugOptionsScreen.KeyHelpItem(Keys.A, 3, "Enable / disable averages"));
      this._KeyList.Add(new DebugOptionsScreen.KeyHelpItem(Keys.R, 2, "Reset stage"));
      this._KeyList.Add(new DebugOptionsScreen.KeyHelpItem(Keys.I, 12, "Change Debug Info position"));
      this._KeyList.Add(new DebugOptionsScreen.KeyHelpItem(Keys.M, 14, "Go to next stage"));
      this._KeyList.Add(new DebugOptionsScreen.KeyHelpItem(Keys.N, 15, "Go to previous stage"));
      this._KeyList.Add(new DebugOptionsScreen.KeyHelpItem(Keys.G, 0, "Generate Thumbnails Textures"));
      this._KeyList.Add(new DebugOptionsScreen.KeyHelpItem(Keys.H, 0, "Generate Stage Thumbnail"));
      this._Textcolor = Color.Yellow;
      this._Font = BrainGame.ResourceManager.Load<SpriteFont>("fonts/dbgWindow", ResourceManager.ResourceManagerCacheType.Static);
    }

    public override void OnUpdate(BrainGameTime gameTime)
    {
      bool flag = false;
      if (this.Input.IsHelpButtonSet(DebugOptionsInput.GameHelpButtons.HideDebugOptionsInput))
      {
        this.Close();
      }
      else
      {
        if (this.Input.IsHelpButtonSet(DebugOptionsInput.GameHelpButtons.ShowHideSprites))
        {
          Game1.GameSettings.ShowSprites = !Game1.GameSettings.ShowSprites;
          flag = true;
        }
        if (this.Input.IsHelpButtonSet(DebugOptionsInput.GameHelpButtons.ShowHidePaths))
        {
          Game1.GameSettings.ShowPaths = !Game1.GameSettings.ShowPaths;
          flag = true;
        }
        if (this.Input.IsHelpButtonSet(DebugOptionsInput.GameHelpButtons.ShowHideQuatree))
        {
          Game1.GameSettings.ShowQuadtree = !Game1.GameSettings.ShowQuadtree;
          flag = true;
        }
        if (this.Input.IsHelpButtonSet(DebugOptionsInput.GameHelpButtons.ShowTiles))
        {
          Game1.GameSettings.ShowTiles = !Game1.GameSettings.ShowTiles;
          flag = true;
        }
        if (this.Input.IsHelpButtonSet(DebugOptionsInput.GameHelpButtons.ShowBoundingBoxes))
        {
          BrainGame.Settings.ShowBoundingBoxes = !BrainGame.Settings.ShowBoundingBoxes;
          flag = true;
        }
        if (this.Input.IsHelpButtonSet(DebugOptionsInput.GameHelpButtons.NextStage))
        {
          Levels.CurrentLevel.StartNextStage();
          flag = true;
        }
        if (this.Input.IsHelpButtonSet(DebugOptionsInput.GameHelpButtons.PrevStage))
        {
          Levels.CurrentLevel.StartPrevStage();
          flag = true;
        }
        this.Input.IsHelpButtonSet(DebugOptionsInput.GameHelpButtons.ReloadStage);
        if (this.Input.IsHelpButtonSet(DebugOptionsInput.GameHelpButtons.GenerateAllThumbs))
          this.GenerateAllThumbs();
        if (this.Input.IsHelpButtonSet(DebugOptionsInput.GameHelpButtons.GenerateCurrentThumb))
          this.GenerateThumb((int) Levels.CurrentTheme, Levels.CurrentStageNr);
        if (!flag)
          return;
        Game1.GameSettings.SaveToFile();
      }
    }

    public override void OnDraw()
    {
      this.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, (DepthStencilState) null, (RasterizerState) null, (Effect) null, BrainGame.DefaultCamera.Transform);
      BrainGame.DrawRectangleFilled(this.SpriteBatch, this._Rect, this._BackColor);
      float x = 20f;
      float y = 20f;
      foreach (DebugOptionsScreen.KeyHelpItem key in this._KeyList)
      {
        Vector2 vector2 = new Vector2(x, y);
        if (!Game1.GameSettings.UseGamepad)
          this.SpriteBatch.DrawString(this._Font, string.Format("({0})", (object) key._Key.ToString()), this._Position + vector2 + new Vector2(0.0f, 15f), this._Textcolor, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
        this.SpriteBatch.DrawString(this._Font, key._HelpText, this._Position + vector2 + new Vector2(55f, 15f), this._Textcolor, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
        y += 50f;
        if ((double) y + 80.0 > (double) this._Rect.Height)
        {
          y = 20f;
          x += 300f;
        }
      }
      this.SpriteBatch.End();
    }

    private void GenerateThumb(int theme, int stageId)
    {
    }

    private void GenerateAllThumbs()
    {
    }

    private void GenerateThumbsTexture(int theme)
    {
    }

    private struct KeyHelpItem(Keys key, int xboxControlFrameNr, string helpText)
    {
      public int _XboxControlFrameNr = xboxControlFrameNr;
      public string _HelpText = helpText;
      public Keys _Key = key;
    }
  }
}
