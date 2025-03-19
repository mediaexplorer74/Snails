
// Type: TwoBrainsGames.Snails.Debuging.DebugItem
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine.Debugging;


namespace TwoBrainsGames.Snails.Debuging
{
  internal class DebugItem : DrawableGameComponent
  {
    private string Caption { get; set; }

    public object Value { get; set; }

    private SpriteBatch SpriteBatch { get; set; }

    private SpriteFont Font { get; set; }

    public Vector2 Position { get; set; }

    public Color Textcolor { get; set; }

    private BrainPerformanceCounter PerfCounter { get; set; }

    private DebugInfo Owner { get; set; }

    public DebugItem(
      Game game,
      DebugInfo debugInfo,
      string caption,
      Vector2 position,
      SpriteFont font,
      Color color,
      object value)
      : base(game)
    {
      this.Caption = caption;
      this.Position = position;
      this.Font = font;
      this.Textcolor = color;
      if (value is BrainPerformanceCounter)
        this.PerfCounter = (BrainPerformanceCounter) value;
      else
        this.Value = value;
      this.Owner = debugInfo;
      if (this.PerfCounter == null)
        return;
      game.Components.Add((IGameComponent) this.PerfCounter);
    }

    protected override void LoadContent()
    {
      this.SpriteBatch = new SpriteBatch(this.Game.GraphicsDevice);
    }

    public override void Update(GameTime gameTime)
    {
      base.Update(gameTime);
      if (this.PerfCounter == null)
        return;
      this.Value = (object) this.PerfCounter.Counter;
    }

    public override void Draw(GameTime gameTime)
    {
      if (!this.Owner.Visible)
        return;
      this.SpriteBatch.Begin();
      Color color = this.Textcolor;
      string text = string.Format("{0,-12}: {1,7:0.00}", (object) this.Caption, this.Value);
      if (this.PerfCounter != null)
      {
        text = string.Format("{0,-12}: {1,7:0.00} ({2,7:0.00})", (object) this.Caption, this.Value, (object) this.PerfCounter.Average);
        if (this.PerfCounter.AlarmOn)
          color = Color.Red;
        int num = this.PerfCounter.AverageOn ? 1 : 0;
      }
      this.SpriteBatch.DrawString(this.Font, text, this.Position + this.Owner.Position, color, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
      this.SpriteBatch.End();
    }
  }
}
