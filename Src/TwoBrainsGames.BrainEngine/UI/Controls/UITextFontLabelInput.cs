
// Type: TwoBrainsGames.BrainEngine.UI.Controls.UITextFontLabelInput
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Input;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.BrainEngine.UI.Controls
{
  public class UITextFontLabelInput : UITextFontLabel
  {
    private UITimer _backTimer;
    private UITimer _backTimer2;
    private int _maxLength = 16;

    public int MaxLength
    {
      get => this._maxLength;
      set => this._maxLength = value;
    }

    public UITextFontLabelInput(UIScreen screenOwner, TextFont font)
      : this(screenOwner, font, string.Empty)
    {
    }

    public UITextFontLabelInput(UIScreen screenOwner, TextFont font, string text)
      : base(screenOwner, font, text)
    {
      this._backTimer = new UITimer(screenOwner, 75.0, true);
      this._backTimer.OnTimer += new UIControl.UIEvent(this.backTimer_OnTimer);
      this._backTimer.Enabled = true;
      this._backTimer2 = new UITimer(screenOwner, 50.0, true);
      this._backTimer2.OnTimer += new UIControl.UIEvent(this.backTimer2_OnTimer);
      this._backTimer2.Enabled = false;
      this.Controls.Add((UIControl) this._backTimer);
      this.Controls.Add((UIControl) this._backTimer2);
    }

    public bool HasText() => !string.IsNullOrEmpty(this.Text);

    private void backTimer_OnTimer(IUIControl sender)
    {
      if (!this.ScreenOwner.InputController.Keyboard.IsKeyDown(Keys.Back) || this._backTimer2.Enabled)
        return;
      this._backTimer2.Enabled = true;
    }

    private void backTimer2_OnTimer(IUIControl sender)
    {
      if (!this.ScreenOwner.InputController.Keyboard.IsKeyDown(Keys.Back) || string.IsNullOrEmpty(this.Text))
        return;
      this.Text = this.Text.Remove(this.Text.Length - 1, 1);
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      if (this.ScreenOwner.InputController.Keyboard.IsKeyReleased(Keys.Back))
      {
        this._backTimer.Enabled = false;
        this._backTimer.Reset();
        this._backTimer2.Enabled = false;
        this._backTimer2.Reset();
      }
      else if (this.ScreenOwner.InputController.Keyboard.IsKeyPressed(Keys.Back))
      {
        if (!string.IsNullOrEmpty(this.Text))
          this.Text = this.Text.Remove(this.Text.Length - 1, 1);
        this._backTimer.Enabled = false;
        this._backTimer.Reset();
      }
      else if (this.ScreenOwner.InputController.Keyboard.IsKeyDown(Keys.Back))
      {
        this._backTimer.Enabled = true;
      }
      else
      {
        Keys[] pressedKeys = this.ScreenOwner.InputController.Keyboard.GetPressedKeys();
        if (pressedKeys == null || pressedKeys.Length <= 0)
          return;
        foreach (Keys k in pressedKeys)
          this.ProcessKey(k);
      }
    }

    private bool IsAllowed(Keys key)
    {
      int k = (int) key;
      return this.IsNumber(k) || k >= 65 && k <= 90 || k == 32;
    }

    private bool IsNumber(Keys key) => this.IsNumber((int) key);

    private bool IsNumber(int k) => k >= 48 && k <= 57;

    private void ProcessKey(Keys k)
    {
      if (this.Text.Length > this._maxLength || !this.IsAllowed(k) 
          || !this.ScreenOwner.InputController.Keyboard.IsKeyPressed(k))
        return;
      char k1 = (char) k;
      if (k == Keys.Space)
      {
        this.Text += " ";
      }
      else
      {
        bool flag = false;
        if (this.ScreenOwner.InputController.Keyboard.IsShiftDown)
          flag = true;
        if (!flag && !this.IsNumber((int) k1))
          k1 += ' ';
        this.Text += k1.ToString();
      }
    }
  }
}
