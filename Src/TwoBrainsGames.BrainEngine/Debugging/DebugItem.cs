
// Type: TwoBrainsGames.BrainEngine.Debugging.DebugItem
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace TwoBrainsGames.BrainEngine.Debugging
{
  internal class DebugItem : DrawableGameComponent
  {
    private string Caption { get; set; }

    private SpriteBatch SpriteBatch { get; set; }

    private SpriteFont Font { get; set; }

    private BrainPerformanceCounter PerfCounter { get; set; }

    private DebugInfo Owner { get; set; }

    public object Value { get; set; }

    public Vector2 Position { get; set; }

    public Color Textcolor { get; set; }

    public bool ShowCaption { get; set; }

    protected string StringFormat { get; set; }

    public DebugItem(
      Game game,
      DebugInfo debugInfo,
      string caption,
      Vector2 position,
      SpriteFont font,
      Color color,
      object value,
      string formatString)
      : base(game)
    {
      this.ShowCaption = false;
      this.Caption = caption;
      this.Position = position;
      this.Font = font;
      this.Textcolor = color;
      if (value is BrainPerformanceCounter)
        this.PerfCounter = (BrainPerformanceCounter) value;
      else
        this.Value = value;
      this.Owner = debugInfo;
      if (this.PerfCounter != null)
        game.Components.Add((IGameComponent) this.PerfCounter);
      this.StringFormat = formatString;
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
        text = this.ShowCaption ? string.Format("{0,-12}: {1,7:0.00} ({2,7:0.00})", (object) this.Caption, this.Value, (object) this.PerfCounter.Average) : string.Format(this.StringFormat, this.Value);
        if (this.PerfCounter.AlarmOn)
          color = Color.Red;
        int num = this.PerfCounter.AverageOn ? 1 : 0;
      }
      this.SpriteBatch.DrawString(this.Font, text, this.Position + this.Owner.Position, color, 0.0f, Vector2.Zero, 1f, SpriteEffects.None, 0.0f);
      this.SpriteBatch.End();
    }
  }
}
