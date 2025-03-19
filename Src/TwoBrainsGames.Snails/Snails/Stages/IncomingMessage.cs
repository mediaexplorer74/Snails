
// Type: TwoBrainsGames.Snails.Stages.IncomingMessage
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Localization;
using TwoBrainsGames.BrainEngine.Resources;


namespace TwoBrainsGames.Snails.Stages
{
  public class IncomingMessage
  {
    private const float SPEED = 3f;
    private const double BLINK_TIME = 300.0;
    private string[] _textLines;
    private Vector2[] _linesOffsets;
    private Vector2 _position;
    private TextFont _font;
    private TextFont _fontMedium;
    private TextFont _fontBig;
    private IncomingMessage.MessageState _state;
    private Color _color;
    private float _textStopPosition;
    private double _pause;
    private double _messagePause;
    private BoundingSquare _stageArea;
    private bool _visible;
    private bool _withAnimation;
    private double _blinkTime;
    private Sample _incStartSample;
    private Sample _incEndSample;

    public bool IsActive
    {
      get
      {
        return this._state != IncomingMessage.MessageState.Inactive && !Game1.Tutorial.TopicVisible;
      }
    }

    public IncomingMessage(BoundingSquare stageArea)
    {
      this._state = IncomingMessage.MessageState.Inactive;
      this._stageArea = stageArea;
      this._withAnimation = true;
    }

    public void LoadContent()
    {
      this._fontMedium = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-medium", ResourceManager.ResourceManagerCacheType.Static);
      this._fontBig = BrainGame.ResourceManager.Load<TextFont>("fonts/main-font-big", ResourceManager.ResourceManagerCacheType.Static);
      this._incStartSample = BrainGame.ResourceManager.GetSampleTemporary("sfx/incomming_start");
      this._incEndSample = BrainGame.ResourceManager.GetSampleTemporary("sfx/incomming_end");
    }

    public void Update(BrainGameTime gameTime)
    {
      switch (this._state)
      {
        case IncomingMessage.MessageState.Entering:
          this._position += new Vector2((float) (3.0 * gameTime.ElapsedRealTime.TotalMilliseconds), 0.0f);
          if ((double) this._position.X < (double) this._textStopPosition)
            break;
          this._position.X = this._textStopPosition;
          this._state = IncomingMessage.MessageState.Active;
          this._pause = 0.0;
          break;
        case IncomingMessage.MessageState.Active:
          if (this._withAnimation)
          {
            this._pause += gameTime.ElapsedRealTime.TotalMilliseconds;
            if (this._pause <= this._messagePause)
              break;
            this._state = IncomingMessage.MessageState.Leaving;
            this._visible = true;
            this._incEndSample.Play();
            break;
          }
          this._blinkTime += gameTime.ElapsedRealTime.TotalMilliseconds;
          if (this._blinkTime <= 300.0 * (this._visible ? 5.0 : 1.0))
            break;
          this._blinkTime = 0.0;
          this._visible = !this._visible;
          if (!this._visible)
            break;
          break;
        case IncomingMessage.MessageState.Leaving:
          this._position += new Vector2((float) (3.0 * gameTime.ElapsedRealTime.TotalMilliseconds), 0.0f);
          if ((double) this._position.X < (double) BrainGame.ScreenWidth)
            break;
          this._state = IncomingMessage.MessageState.Inactive;
          break;
      }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
      if (!this._visible)
        return;
      for (int index = 0; index < this._textLines.Length; ++index)
        this._font.DrawString(spriteBatch, this._textLines[index], this._position + this._linesOffsets[index], new Vector2(1f, 1f), this._color);
    }

    private void Show(
      string text,
      Color color,
      bool withAnimation,
      TextFont font,
      IncomingMessage.MessagePositionType positionType)
    {
      this._font = font;
      this._textLines = text.Split('|');
      float y = 0.0f;
      float num1 = 0.0f;
      this._linesOffsets = new Vector2[this._textLines.Length];
      for (int index = 0; index < this._textLines.Length; ++index)
      {
        float num2 = this._font.MeasureString(this._textLines[index], new Vector2(1f, 1f));
        float num3 = this._font.MeasureStringHeight(this._textLines[index], new Vector2(1f, 1f));
        if ((double) num2 > (double) num1)
          num1 = num2;
        this._linesOffsets[index] = new Vector2((float) -((double) num2 / 2.0), y);
        y += num3;
      }
      switch (positionType)
      {
        case IncomingMessage.MessagePositionType.Center:
          this._position = new Vector2(this._stageArea.Left - num1 * 2f + (float) BrainGame.ScreenRectangle.X, this._stageArea.Top + this._stageArea.Height / 2f - y);
          break;
        case IncomingMessage.MessagePositionType.Bottom:
          this._position = new Vector2(this._stageArea.Left - num1 * 2f + (float) BrainGame.ScreenRectangle.X, (float) ((double) this._stageArea.Bottom - (double) y - 30.0));
          break;
      }
      this._state = IncomingMessage.MessageState.Entering;
      this._color = color;
      this._textStopPosition = this._stageArea.Left + this._stageArea.Width / 2f + (float) BrainGame.ScreenRectangle.X;
      this._messagePause = 1000.0;
      this._visible = true;
      this._withAnimation = withAnimation;
      this._blinkTime = 0.0;
      if (!this._withAnimation)
      {
        this._state = IncomingMessage.MessageState.Active;
        this._position = new Vector2(this._textStopPosition, this._position.Y);
      }
      else
        this._incStartSample.Play();
    }

    public void Hide()
    {
      this._visible = false;
      this._state = IncomingMessage.MessageState.Inactive;
    }

    public void Show(IncomingMessage.MessageType message)
    {
      switch (message)
      {
        case IncomingMessage.MessageType.ClickToStart:
          this.Show(LanguageManager.GetString("INCOMING_MSG_CLICK_TO_START"), Color.Orange, false, this._fontMedium, IncomingMessage.MessagePositionType.Bottom);
          break;
        case IncomingMessage.MessageType.DeliveryStart:
          this.Show(LanguageManager.GetString("INCOMING_MSG_DELIVERY"), Colors.IncomingMessageInfo, true, this._fontBig, IncomingMessage.MessagePositionType.Center);
          break;
        case IncomingMessage.MessageType.TimeAttackStart:
          this.Show(LanguageManager.GetString("INCOMING_MSG_TIME_ATTACK"), Colors.IncomingMessageInfo, true, this._fontBig, IncomingMessage.MessagePositionType.Center);
          break;
        case IncomingMessage.MessageType.MissionFailed:
          this.Show(LanguageManager.GetString("INCOMING_MSG_MISSION_FAILED"), Colors.IncomingMessageError, true, this._fontBig, IncomingMessage.MessagePositionType.Center);
          break;
        case IncomingMessage.MessageType.MissionCompleted:
          this.Show(LanguageManager.GetString("INCOMING_MSG_STAGE_COMPL"), Colors.IncomingMessageInfo, true, this._fontBig, IncomingMessage.MessagePositionType.Center);
          break;
        case IncomingMessage.MessageType.TimeIsUp:
          this.Show(LanguageManager.GetString("INCOMING_MSG_TIME_UP"), Colors.IncomingMessageError, true, this._fontBig, IncomingMessage.MessagePositionType.Center);
          break;
        case IncomingMessage.MessageType.SnailKillerStart:
          this.Show(LanguageManager.GetString("INCOMING_MSG_SNAIL_KILLER"), Colors.IncomingMessageInfo, true, this._fontBig, IncomingMessage.MessagePositionType.Center);
          break;
        case IncomingMessage.MessageType.SnailKingStart:
          this.Show(LanguageManager.GetString("INCOMING_MSG_SNAIL_KING"), Colors.IncomingMessageInfo, true, this._fontBig, IncomingMessage.MessagePositionType.Center);
          break;
      }
    }

    private enum MessagePositionType
    {
      Center,
      Bottom,
    }

    private enum MessageState
    {
      Inactive,
      Entering,
      Active,
      Leaving,
    }

    public enum MessageType
    {
      ClickToStart,
      DeliveryStart,
      TimeAttackStart,
      MissionFailed,
      MissionCompleted,
      TimeIsUp,
      SnailKillerStart,
      SnailKingStart,
    }
  }
}
