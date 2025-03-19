
// Type: TwoBrainsGames.Snails.HddAccessIcon
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.BrainEngine.Resources;


namespace TwoBrainsGames.Snails
{
  internal class HddAccessIcon : IHddIndicator, IBrainComponent
  {
    private SpriteAnimation _snailAnim;
    private Vector2 _snailPosition;
    private Vector2 _textPosition;
    private Vector2 _position;
    private Color _shadowColor;
    private Vector2 _shadowDistance;
    private TextFont _font;
    private string _loadingText;
    private Color _textColor;

    public bool Visible { get; set; }

    public SpriteBatch SpriteBatch => throw new NotImplementedException();

    public void Initialize()
    {
    }

    public void LoadContent()
    {
      this._snailAnim = new SpriteAnimation(new Sprite(BrainGame.ResourceManager.GetSpriteStatic("spriteset/common-elements-1", "LoadingAnim")));
      this._position = new Vector2((float) (BrainGame.ScreenWidth - 110), (float) (BrainGame.ScreenHeight - 30));
      this._snailPosition = new Vector2(-0.0f, 25f);
      this._shadowColor = new Color(0, 0, 0, 100);
      this._shadowDistance = new Vector2(3f, 2f);
      Game1.Instance.OnLanguageChanged += new EventHandler(this.Instance_OnLanguageChanged);
      this._font = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-medium", ResourceManager.ResourceManagerCacheType.Static);
      this._textColor = new Color((int) byte.MaxValue, 180, 45);
      this.UpdateLoadingText();
    }

    private void Instance_OnLanguageChanged(object sender, EventArgs e) => this.UpdateLoadingText();

    private void UpdateLoadingText()
    {
      this._loadingText = LanguageManager.GetString("LBL_LOADING");
      this._textPosition = new Vector2((float) (-(double) this._font.MeasureString(this._loadingText) / 2.0), -50f);
    }

    public void Update(BrainGameTime gameTime) => this._snailAnim.Update(gameTime);

    public void Draw()
    {
    }

    public void Draw(SpriteBatch spriteBatch) => this.Draw(spriteBatch, this._position);

    public void Draw(SpriteBatch spriteBatch, Vector2 position)
    {
      this._font.DrawString(spriteBatch, this._loadingText, position + this._textPosition + this._shadowDistance, Vector2.One, this._shadowColor);
      this._font.DrawString(spriteBatch, this._loadingText, position + this._textPosition, Vector2.One, this._textColor);
      this._snailAnim.Draw(this._snailPosition + this._shadowDistance + position, this._shadowColor, spriteBatch);
      this._snailAnim.Draw(this._snailPosition + position, spriteBatch);
    }

    public void UnloadContent()
    {
    }
  }
}
