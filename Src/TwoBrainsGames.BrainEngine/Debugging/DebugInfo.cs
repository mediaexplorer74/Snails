
// Type: TwoBrainsGames.BrainEngine.Debugging.DebugInfo
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine.Resources;


namespace TwoBrainsGames.BrainEngine.Debugging
{
  public class DebugInfo : DrawableGameComponent
  {
    private const int MARGIN = 10;
    private const int WINDOW_WIDTH = 240;
    private const int WINDOW_HEIGHT = 60;
    public const float FPSAlarmThreshold = 60f;
    public const float DrawTimeAlarmThreshold = 10f;
    public const float UpdateTimeAlarmThreshold = 12f;
    public const float CollisionTestsAlarmThreshold = 500f;
    public Vector2 Position;
    public BrainPerformanceCounter SnailCounter;
    public BrainPerformanceCounter ObjectsCounter;
    public BrainPerformanceCounter CollisionCounter;
    private DebugInfo.DockType _DockStyle;
    private SpriteFont Font;
    private SpriteBatch SpriteBatch;
    private FPSCounter FPSCounter;
    private MemoryPeakCounter MemoryPeakCounter;
    private MemoryUsageCounter MemoryUsageCounter;
    private TimerCounter DrawPerformanceTimer;
    private TimerCounter UpdatePerformanceTimer;
    private Color Textcolor;
    private bool DockStyleChanged;
    private bool Averaging;
    private Rectangle Rect;
    private List<string> _debugMessages;

    public DebugInfo.DockType DockStyle
    {
      get => this._DockStyle;
      set
      {
        this._DockStyle = value;
        this.DockStyleChanged = true;
      }
    }

    public DebugInfo(Game game, Vector2 position)
      : base(game)
    {
      this.Position = position;
      this.Textcolor = Color.Yellow;
      this.DockStyle = DebugInfo.DockType.UpperLeft;
      this.DockStyleChanged = true;
      this.Visible = true;
      this.Rect = new Rectangle((int) this.Position.X, (int) this.Position.Y, BrainGame.ScreenWidth, 60);
      this._debugMessages = new List<string>();
    }

    protected override void LoadContent()
    {
      this.Font = BrainGame.ResourceManager.Load<SpriteFont>("fonts/debug", ResourceManager.ResourceManagerCacheType.Static);
      this.FPSCounter = new FPSCounter(this.Game);
      this.MemoryPeakCounter = new MemoryPeakCounter(this.Game);
      this.MemoryPeakCounter.SetAlarmThreshold(9.437184E+07f, BrainPerformanceCounter.AlertConditions.Greater);
      this.MemoryUsageCounter = new MemoryUsageCounter(this.Game);
      this.MemoryUsageCounter.SetAlarmThreshold(9.437184E+07f, BrainPerformanceCounter.AlertConditions.Greater);
      this.DrawPerformanceTimer = new TimerCounter(this.Game);
      this.DrawPerformanceTimer.SetAlarmThreshold(10f, BrainPerformanceCounter.AlertConditions.Greater);
      this.UpdatePerformanceTimer = new TimerCounter(this.Game);
      this.UpdatePerformanceTimer.SetAlarmThreshold(12f, BrainPerformanceCounter.AlertConditions.Greater);
      this.CollisionCounter = new BrainPerformanceCounter(this.Game);
      this.CollisionCounter.SetAlarmThreshold(500f, BrainPerformanceCounter.AlertConditions.Greater);
      this.SnailCounter = new BrainPerformanceCounter(this.Game);
      this.ObjectsCounter = new BrainPerformanceCounter(this.Game);
      this.SpriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
      this.Game.Components.Add((IGameComponent) new DebugItem(this.Game, this, "FPSCounter", new Vector2(10f, 5f), this.Font, this.Textcolor, (object) this.FPSCounter, "{0}"));
      this.Game.Components.Add((IGameComponent) new DebugItem(this.Game, this, "MemoryPeak", new Vector2(10f, 20f), this.Font, this.Textcolor, (object) this.MemoryPeakCounter, "{0:###,##0} Kb"));
      this.Game.Components.Add((IGameComponent) new DebugItem(this.Game, this, "MemoryUsage", new Vector2(10f, 35f), this.Font, this.Textcolor, (object) this.MemoryUsageCounter, "{0:###,##0} Kb"));
    }

    public void TogglePosition()
    {
      int num = (int) (this.DockStyle + 1);
      if (num > 3)
        num = 0;
      this.DockStyle = (DebugInfo.DockType) num;
    }

    public void ToggleAverage()
    {
      if (!this.Averaging)
      {
        this.FPSCounter.BeginAverage();
        this.UpdatePerformanceTimer.BeginAverage();
        this.DrawPerformanceTimer.BeginAverage();
        this.CollisionCounter.BeginAverage();
        this.SnailCounter.BeginAverage();
        this.ObjectsCounter.BeginAverage();
        this.Averaging = true;
      }
      else
      {
        this.FPSCounter.EndAverage();
        this.UpdatePerformanceTimer.EndAverage();
        this.DrawPerformanceTimer.EndAverage();
        this.CollisionCounter.EndAverage();
        this.SnailCounter.EndAverage();
        this.ObjectsCounter.EndAverage();
        this.Averaging = false;
      }
    }

    public override void Update(GameTime gameTime)
    {
      if (!this.DockStyleChanged)
        return;
      this.Reposition();
      this.DockStyleChanged = false;
    }

    public override void Draw(GameTime gameTime)
    {
      if (!this.Visible)
        return;
      this.Rect = new Rectangle((int) this.Position.X, (int) this.Position.Y, BrainGame.ScreenWidth, 60 + this._debugMessages.Count * 15);
      this.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, (DepthStencilState) null, (RasterizerState) null, (Effect) BrainGame.RenderEffect);
      BrainGame.DrawRectangleFilled(this.SpriteBatch, this.Rect, new Color(0, 0, 0, 150));
      float y = (float) (this.Rect.Top + 45);
      foreach (string debugMessage in this._debugMessages)
      {
        this.SpriteBatch.DrawString(this.Font, debugMessage, new Vector2(10f, y), Color.White);
        y += 15f;
      }
      this.SpriteBatch.End();
      base.Draw(gameTime);
    }

    public void UpdateStarted()
    {
      this.UpdatePerformanceTimer.Reset();
      this.CollisionCounter.Reset();
    }

    public void UpdateEnded() => this.UpdatePerformanceTimer.End();

    public void DrawStarted() => this.DrawPerformanceTimer.Reset();

    public void DrawEnded() => this.DrawPerformanceTimer.End();

    private void Reposition()
    {
      int width = this.Rect.Width;
      int height = this.Rect.Height;
      switch (this.DockStyle)
      {
        case DebugInfo.DockType.UpperLeft:
          this.Position = new Vector2(10f, 10f);
          break;
        case DebugInfo.DockType.UpperRight:
          this.Position = new Vector2((float) (BrainGame.ScreenWidth - width - 10), 10f);
          break;
        case DebugInfo.DockType.LowerRight:
          this.Position = new Vector2((float) (BrainGame.ScreenWidth - width - 10), (float) (BrainGame.ScreenHeight - height - 10));
          break;
        case DebugInfo.DockType.LowerLeft:
          this.Position = new Vector2(10f, (float) (BrainGame.ScreenHeight - height - 10));
          break;
      }
      this.Rect = new Rectangle((int) this.Position.X, (int) this.Position.Y, this.Rect.Width, this.Rect.Height);
    }

    public void ClearMessages() => this._debugMessages.Clear();

    public void AddMessage(string message) => this._debugMessages.Add(message);

    public enum DockType
    {
      UpperLeft,
      UpperRight,
      LowerRight,
      LowerLeft,
    }
  }
}
