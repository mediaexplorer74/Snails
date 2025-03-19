
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UIFooterMessage
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UIFooterMessage : UIControl
  {
    private const float SPEED = 0.3f;
    private UISpriteFontLabel _lblMessage;

    private float Speed { get; set; }

    public UIFooterMessage(UIScreen screenOwner)
      : base(screenOwner)
    {
      this._lblMessage = new UISpriteFontLabel(screenOwner);
      this.Controls.Add((UIControl) this._lblMessage);
      this.ParentAlignment = AlignModes.Bottom;
      this.Speed = 0.3f;
      this.Position = new Vector2(this.PixelsToScreenUnitsX((float) BrainGame.ScreenWidth / 1.5f), 0.0f);
      this.OnLanguageChanged += new UIControl.UIEvent(this.UIFooterMessage_OnLanguageChanged);
      this.AcceptControllerInput = false;
      string assetName = "fonts/footerMessage";
      if (BrainGame.Settings.Platform == BrainSettings.PlaformType.WP7 || BrainGame.Settings.Platform == BrainSettings.PlaformType.iOS || BrainGame.Settings.Platform == BrainSettings.PlaformType.Andriod)
        assetName += "WP";
      this._lblMessage.Font = BrainGame.ResourceManager.Load<SpriteFont>(assetName);
      this._lblMessage.BlendColor = Color.White;
    }

    private void UIFooterMessage_OnLanguageChanged(IUIControl sender)
    {
      this.BuildMessage(Game1.FooterMessages);
    }

    public void Initialize()
    {
      this.Position = this.ScreenOwner.Navigator.GlobalCache.Get<Vector2>("FOOTER_MESSAGE_POSITION", this.Position);
      this.BuildMessage(Game1.FooterMessages);
      this.BringToFront();
    }

        public override void Update(BrainGameTime gameTime)
        {
            if (string.IsNullOrEmpty(this._lblMessage.Text))
                return;
            this.Position = this.Position - new Vector2(this.Speed * (float)gameTime.ElapsedRealTime.TotalMilliseconds, 0.0f);
            if ((double)this.Position.X + (double)this.Size.Width < 0.0)
                this.Position = new Vector2(this.PixelsToScreenUnitsX((float)BrainGame.ScreenWidth), this.Position.Y);
            this.ScreenOwner.Navigator.GlobalCache.Set("FOOTER_MESSAGE_POSITION", (object)this.Position);
        }

    private void BuildMessage(List<string> messages)
    {
      this._lblMessage.Text = string.Empty;
      for (int index = 0; index < messages.Count; ++index)
      {
        this._lblMessage.Text += messages[index];
        if (index < messages.Count - 1)
          this._lblMessage.Text += "      ";
      }
      this.Size = this._lblMessage.Size;
    }
  }
}
