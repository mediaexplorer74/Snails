
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UICreditsPanel
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UICreditsPanel : UIPanel
  {
    private const float HEADING_LINE_SPACING = 500f;
    private const float LINE_SPACING = 450f;
    private const float CATEGORY_SPACING = 100f;
    private static Color HeadingsColor = new Color(200, 80, (int) byte.MaxValue);
    private static Color NamesColor = new Color(118, 230, 80);
    private float _posY;

    public override bool Visible
    {
      get => base.Visible;
      set
      {
        base.Visible = value;
        foreach (UIControl control in this.Controls)
          control.Visible = value;
      }
    }

    public UICreditsPanel(UIScreen screenOwner)
      : base(screenOwner)
    {
      this._posY = 0.0f;
      this.Size = new Size(0.0f, 0.0f);
    }

    public void AddCategory(string textResourceId)
    {
      if (this.Controls.Count > 0)
        this._posY += 100f;
      UICaption control = new UICaption(this.ScreenOwner, "", UICreditsPanel.HeadingsColor, UICaption.CaptionStyle.CreditsCategory);
      control.TextResourceId = textResourceId;
      control.Position = new Vector2(0.0f, this._posY);
      this.Controls.Add((UIControl) control);
      this._posY += 500f;
      if ((double) control.Size.Width > (double) this.Size.Width)
        this.Size = new Size(control.Size.Width, this.Size.Height);
      if ((double) control.Position.Y + (double) control.Size.Height <= (double) this.Size.Height)
        return;
      this.Size = new Size(this.Size.Width, control.Position.Y + control.Size.Height);
    }

    public void AddName(string text)
    {
      UICaption control = new UICaption(this.ScreenOwner, text, UICreditsPanel.NamesColor, UICaption.CaptionStyle.CreditsName);
      control.Position = new Vector2(0.0f, this._posY);
      this.Controls.Add((UIControl) control);
      this._posY += 450f;
      if ((double) control.Size.Width > (double) this.Size.Width)
        this.Size = new Size(control.Size.Width, this.Size.Height);
      if ((double) control.Position.Y + (double) control.Size.Height <= (double) this.Size.Height)
        return;
      this.Size = new Size(this.Size.Width, control.Position.Y + control.Size.Height);
    }
  }
}
