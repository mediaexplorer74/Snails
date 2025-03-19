
// Type: TwoBrainsGames.Snails.Stages.HUD.HUDItemControls
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Input;
using TwoBrainsGames.Snails.StageObjects;


namespace TwoBrainsGames.Snails.Stages.HUD
{
  internal class HUDItemControls : HUDItem, ICursorInteractable
  {
    private const float MARGIN = 10f;
    private const float ITEM_SPACING = 20f;
    private static Color DefaultIconColor = Colors.StageHUDInfoColor;
    private static Color SelectedIconColor = new Color(125, 125, 20);
    private Sprite _spriteIcon;
    private List<HUDItemControls.ControlButton> _buttons;
    private Vector2 _itemSize;
    private HUDItemControls.ControlButton _warpButton;
    private HUDItemControls.ControlButton _endStageButton;
    private float _scale;
    private bool _isInsideControl;
    private BoundingSquare _bbController;

    public override void StageAreaChanged(BoundingSquare newStageArea)
    {
      this._itemSize = new Vector2((float) this._spriteIcon.Frames[0].Width * this._scale, (float) this._spriteIcon.Frames[0].Height * this._scale);
      this._position = new Vector2(10f * this._scale, (float) ((double) newStageArea.Bottom - (double) this._itemSize.Y - 10.0 * (double) this._scale));
      Vector2 position = this._position;
      for (int index = 0; index < this._buttons.Count; ++index)
      {
        this._buttons[index]._position = position;
        this._buttons[index]._bsButton = this._spriteIcon._boundingSpheres[0].Transform(position);
        this._buttons[index]._bsButton._radius *= this._scale;
        position += new Vector2(this._itemSize.X + 20f * this._scale, 0.0f);
      }
      this.UpdateControllerBB();
    }

    public override void LoadContent()
    {
      this._spriteIcon = BrainGame.ResourceManager.GetSpriteTemporary("spriteset/StageHUD", "GameplayControls");
    }

    public override void Initialize(Vector2 position)
    {
      this._buttons = new List<HUDItemControls.ControlButton>();
      this._buttons.Add(new HUDItemControls.ControlButton(HUDItemControls.ControlButtonType.Restart, HUDItemControls.DefaultIconColor, new HUDItemControls.ControlButton.ControlButtonEvent(this.Button_Restart), GameplayInput.GamePlayButtons.RestartStage));
      this._buttons.Add(new HUDItemControls.ControlButton(HUDItemControls.ControlButtonType.Pause, HUDItemControls.DefaultIconColor, new HUDItemControls.ControlButton.ControlButtonEvent(this.Button_PauseGame), GameplayInput.GamePlayButtons.Pause));
      this._warpButton = new HUDItemControls.ControlButton(HUDItemControls.ControlButtonType.FastForward, HUDItemControls.DefaultIconColor, new HUDItemControls.ControlButton.ControlButtonEvent(this.Button_FastForward), GameplayInput.GamePlayButtons.TimeWarp);
      this._buttons.Add(this._warpButton);
      this._endStageButton = new HUDItemControls.ControlButton(HUDItemControls.ControlButtonType.EndStage, HUDItemControls.DefaultIconColor, new HUDItemControls.ControlButton.ControlButtonEvent(this.Button_EndMission), GameplayInput.GamePlayButtons.None);
      this._endStageButton._visible = false;
      this._buttons.Add(this._endStageButton);
      this._scale = 1f;
      int presentationMode = (int) Game1.GameSettings.PresentationMode;
      this._isInsideControl = false;
      this.UpdateControllerBB();
    }

    private void UpdateControllerBB()
    {
      float y1 = this._buttons[0]._bsButton._center.Y - this._buttons[0]._bsButton._radius;
      float x1 = this._buttons[0]._bsButton._center.X - this._buttons[0]._bsButton._radius;
      float x2 = 0.0f;
      float y2 = 0.0f;
      foreach (HUDItemControls.ControlButton button in this._buttons)
      {
        if (button._visible)
        {
          if ((double) x2 < (double) button._bsButton._center.X + (double) button._bsButton._radius)
            x2 = button._bsButton._center.X + button._bsButton._radius;
          if ((double) y2 < (double) button._bsButton._center.Y + (double) button._bsButton._radius)
            y2 = button._bsButton._center.Y + button._bsButton._radius;
        }
      }
      this._bbController = new BoundingSquare(new Vector2(x1, y1), new Vector2(x2, y2));
    }

    public override void Update(BrainGameTime gameTime)
    {
      base.Update(gameTime);
      this._isInsideControl = this._bbController.Contains(Stage.CurrentStage.Input.MotionPosition);
      if (this._isInsideControl)
        Stage.CurrentStage.Cursor.SetInteractingObject((ICursorInteractable) this);
      foreach (HUDItemControls.ControlButton button in this._buttons)
      {
        if (button._visible)
          button.Update(gameTime);
      }
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
      foreach (HUDItemControls.ControlButton button in this._buttons)
      {
        if (button._visible)
          this._spriteIcon.Draw(button._position, (int) button._buttonType, 0.0f, SpriteEffects.None, button._color, this._scale, spriteBatch);
      }
    }

    private void Button_PauseGame() => Stage.CurrentStage.PauseGame();

    private void Button_Restart() => Stage.CurrentStage.RestartMission();

    private void Button_EndMission() => Stage.CurrentStage.EndMission();

    private void Button_FastForward() => this.ToggleTimeWarp();

    public void ToggleTimeWarp()
    {
      Stage.CurrentStage.ToggleTimeWarp();
      if (Stage.CurrentStage.InTimeWarp)
        this._warpButton._buttonType = HUDItemControls.ControlButtonType.Play;
      else
        this._warpButton._buttonType = HUDItemControls.ControlButtonType.FastForward;
    }

    public override void MissionStateChanged()
    {
      if (Stage.CurrentStage.MissionState != Stage.MissionStateType.Completed || Stage.CurrentStage.Stats.NumSnailsActive + Stage.CurrentStage.Stats.NumSnailsToRelease <= 0)
        return;
      this._endStageButton._visible = true;
      this.UpdateControllerBB();
    }

    private HUDItemControls.ControlButton GetButton(Vector2 position)
    {
      for (int index = 0; index < this._buttons.Count; ++index)
      {
        if (this._buttons[index]._bsButton.Contains(Stage.CurrentStage.Input.MotionPosition) && this._buttons[index]._visible)
          return this._buttons[index];
      }
      return (HUDItemControls.ControlButton) null;
    }

    public StageCursor.CursorType QueryCursor() => StageCursor.CursorType.Select;

    public bool QueryInterating() => this._isInsideControl;

    public void CursorActionPressed(Vector2 cursorPos)
    {
      this.GetButton(Stage.CurrentStage.Input.MotionPosition)?.Select();
    }

    public void CursorActionReleased()
    {
    }

    public void CursorActionSelected()
    {
    }

    public bool CanAcceptCursorInteraction => this._visible;

    protected enum ControlButtonType
    {
      Pause,
      Play,
      FastForward,
      Restart,
      EndStage,
    }

    private class ControlButton
    {
      public Vector2 _position;
      public HUDItemControls.ControlButtonType _buttonType;
      public bool _visible;
      public BoundingCircle _bsButton;
      public Color _color;
      public Color _defaultColor;
      private UITimer _timer;
      public GameplayInput.GamePlayButtons _hotKey;

      public event HUDItemControls.ControlButton.ControlButtonEvent OnSelect;

      public ControlButton(
        HUDItemControls.ControlButtonType buttonType,
        Color color,
        HUDItemControls.ControlButton.ControlButtonEvent onSelectMethod,
        GameplayInput.GamePlayButtons hotKey)
      {
        this._buttonType = buttonType;
        this._visible = true;
        this._bsButton = new BoundingCircle();
        this._color = color;
        this._defaultColor = color;
        this._timer = new UITimer((UIScreen) null, 50.0, false);
        this._timer.Enabled = false;
        this._timer.OnTimer += new UIControl.UIEvent(this._timer_OnTimer);
        this.OnSelect += new HUDItemControls.ControlButton.ControlButtonEvent(onSelectMethod.Invoke);
        this._hotKey = hotKey;
      }

      private void _timer_OnTimer(IUIControl sender)
      {
        this._color = HUDItemControls.DefaultIconColor;
        this.OnSelect();
      }

      public void Select()
      {
        this._timer.Reset();
        this._timer.Enabled = true;
        this._color = HUDItemControls.SelectedIconColor;
      }

      public void Update(BrainGameTime gameTime)
      {
        if ((Stage.CurrentStage.Input.GameButtons & this._hotKey) == this._hotKey && this._hotKey != GameplayInput.GamePlayButtons.None)
          this.Select();
        if (!this._timer.Enabled)
          return;
        this._timer.Update(gameTime);
      }

      public delegate void ControlButtonEvent();
    }
  }
}
