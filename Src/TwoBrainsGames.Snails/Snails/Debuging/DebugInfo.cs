
// Type: TwoBrainsGames.Snails.Debuging.DebugInfo
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Debugging;
using TwoBrainsGames.BrainEngine.Resources;


namespace TwoBrainsGames.Snails.Debuging
{
  internal class DebugInfo : DrawableGameComponent
  {
    private const int MARGIN = 10;
    private const int WINDOW_WIDTH = 240;
    private const int WINDOW_HEIGHT = 60;
    public const float FPSAlarmThreshold = 60f;
    public const float DrawTimeAlarmThreshold = 10f;
    public const float UpdateTimeAlarmThreshold = 12f;
    public const float CollisionTestsAlarmThreshold = 500f;
    private DebugInfo.DockType _DockStyle;
    private bool Averaging;

    private SpriteFont Font { get; set; }

    private SpriteBatch SpriteBatch { get; set; }

    public Vector2 Position { get; private set; }

    private FPSCounter FPSCounter { get; set; }

    private TimerCounter DrawPerformanceTimer { get; set; }

    private TimerCounter UpdatePerformanceTimer { get; set; }

    public BrainPerformanceCounter CollisionCounter { get; private set; }

    public BrainPerformanceCounter SnailCounter { get; private set; }

    public BrainPerformanceCounter ObjectsCounter { get; private set; }

    private Color Opacity { get; set; }

    private Color Textcolor { get; set; }

    private Color BackColor { get; set; }

    private bool DockStyleChanged { get; set; }

    private Rectangle Rect { get; set; }

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
      this.DockStyle = DebugInfo.DockType.LowerLeft;
      this.DockStyleChanged = true;
      this.Visible = true;
      this.Rect = new Rectangle((int) this.Position.X, (int) this.Position.Y, 240, 60);
      this.BackColor = new Color(0, 0, 100, 128);
    }

    protected override void LoadContent()
    {
      this.Font = BrainGame.ResourceManager.Load<SpriteFont>("fonts/debug", ResourceManager.ResourceManagerCacheType.Static);
      this.FPSCounter = new FPSCounter(this.Game);
      this.FPSCounter.SetAlarmThreshold(60f, BrainPerformanceCounter.AlertConditions.Smaller);
      this.DrawPerformanceTimer = new TimerCounter(this.Game);
      this.DrawPerformanceTimer.SetAlarmThreshold(10f, BrainPerformanceCounter.AlertConditions.Greater);
      this.UpdatePerformanceTimer = new TimerCounter(this.Game);
      this.UpdatePerformanceTimer.SetAlarmThreshold(12f, BrainPerformanceCounter.AlertConditions.Greater);
      this.CollisionCounter = new BrainPerformanceCounter(this.Game);
      this.CollisionCounter.SetAlarmThreshold(500f, BrainPerformanceCounter.AlertConditions.Greater);
      this.SnailCounter = new BrainPerformanceCounter(this.Game);
      this.ObjectsCounter = new BrainPerformanceCounter(this.Game);
      this.SpriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
      this.Opacity = new Color((int) byte.MaxValue, (int) byte.MaxValue, (int) byte.MaxValue, 170);
      this.Game.Components.Add((IGameComponent) new DebugItem(this.Game, this, "FPSCounter", new Vector2(10f, 5f), this.Font, this.Textcolor, (object) this.FPSCounter));
      this.Game.Components.Add((IGameComponent) new DebugItem(this.Game, this, "Update", new Vector2(10f, 20f), this.Font, this.Textcolor, (object) this.UpdatePerformanceTimer));
      this.Game.Components.Add((IGameComponent) new DebugItem(this.Game, this, "Draw", new Vector2(10f, 35f), this.Font, this.Textcolor, (object) this.DrawPerformanceTimer));
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
      this.SpriteBatch.Begin();
      BrainGame.DrawRectangleFilled(this.SpriteBatch, this.Rect, this.BackColor);
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
          this.Position = new Vector2((float) (Game1.GameSettings.ScreenWidth - width - 10), 10f);
          break;
        case DebugInfo.DockType.LowerRight:
          this.Position = new Vector2((float) (Game1.GameSettings.ScreenWidth - width - 10), (float) (Game1.GameSettings.ScreenHeight - height - 10));
          break;
        case DebugInfo.DockType.LowerLeft:
          this.Position = new Vector2(10f, (float) (Game1.GameSettings.ScreenHeight - height - 10));
          break;
      }
      this.Rect = new Rectangle((int) this.Position.X, (int) this.Position.Y, this.Rect.Width, this.Rect.Height);
    }

    public enum DockType
    {
      UpperLeft,
      UpperRight,
      LowerRight,
      LowerLeft,
    }
  }
}
