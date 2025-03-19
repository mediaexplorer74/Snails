
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UIInstructionLabel
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UIInstructionLabel : UIControl
  {
    public UIInstructionLabel.LabelActionTypes _labelType;
    private UISpriteFontLabel _lblCaption;
    private UIImage _imgIcon;
    private UIInstructionLabel.ControllerKeys _controllerKey;

    public UIInstructionLabel.ControllerKeys ControllerKey
    {
      get => this._controllerKey;
      private set
      {
        this._controllerKey = value;
        switch (this._controllerKey)
        {
          case UIInstructionLabel.ControllerKeys.LeftStick:
            this._imgIcon.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/controller-buttons", "AnalogStick");
            break;
          case UIInstructionLabel.ControllerKeys.RightStick:
            this._imgIcon.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/controller-buttons", "AnalogStick");
            break;
          case UIInstructionLabel.ControllerKeys.A:
            this._imgIcon.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/controller-buttons", "A");
            break;
          case UIInstructionLabel.ControllerKeys.B:
            this._imgIcon.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/controller-buttons", "B");
            break;
          case UIInstructionLabel.ControllerKeys.LeftRight:
            this._imgIcon.Sprite = BrainGame.ResourceManager.GetSpriteStatic("spriteset/controller-buttons", "DPad");
            break;
        }
      }
    }

    public UIInstructionLabel.LabelActionTypes LabelType
    {
      get => this._labelType;
      set
      {
        this._labelType = value;
        switch (this._labelType)
        {
          case UIInstructionLabel.LabelActionTypes.Select:
            this.Caption = "Select";
            this.ControllerKey = UIInstructionLabel.ControllerKeys.A;
            break;
          case UIInstructionLabel.LabelActionTypes.Back:
            this.Caption = "Back";
            this.ControllerKey = UIInstructionLabel.ControllerKeys.B;
            break;
          case UIInstructionLabel.LabelActionTypes.TuneUpDown:
            this.Caption = "Tune Up/Down";
            this.ControllerKey = UIInstructionLabel.ControllerKeys.LeftStick;
            break;
          case UIInstructionLabel.LabelActionTypes.AdjustSize:
            this.Caption = "Adjust Size";
            this.ControllerKey = UIInstructionLabel.ControllerKeys.LeftStick;
            break;
          case UIInstructionLabel.LabelActionTypes.Accept:
            this.Caption = "Accept";
            this.ControllerKey = UIInstructionLabel.ControllerKeys.A;
            break;
          case UIInstructionLabel.LabelActionTypes.ToggleSlider:
            this.Caption = "Toggle";
            this.ControllerKey = UIInstructionLabel.ControllerKeys.LeftRight;
            break;
          case UIInstructionLabel.LabelActionTypes.Continue:
            this.Caption = "Continue";
            this.ControllerKey = UIInstructionLabel.ControllerKeys.A;
            break;
          case UIInstructionLabel.LabelActionTypes.Start:
            this.Caption = "Start";
            this.ControllerKey = UIInstructionLabel.ControllerKeys.A;
            break;
          case UIInstructionLabel.LabelActionTypes.StartNextStage:
            this.Caption = "Next Stage";
            this.ControllerKey = UIInstructionLabel.ControllerKeys.A;
            break;
          case UIInstructionLabel.LabelActionTypes.Quit:
            this.Caption = "Quit";
            this.ControllerKey = UIInstructionLabel.ControllerKeys.B;
            break;
          case UIInstructionLabel.LabelActionTypes.Retry:
            this.Caption = "Retry";
            this.ControllerKey = UIInstructionLabel.ControllerKeys.A;
            break;
        }
        this.ResizeControl();
      }
    }

    public string Caption
    {
      get => this._lblCaption.Text;
      set => this._lblCaption.Text = value;
    }

    public UIInstructionLabel(UIScreen ownerScreen, UIInstructionLabel.LabelActionTypes type)
      : base(ownerScreen)
    {
      this._lblCaption = new UISpriteFontLabel(ownerScreen, "fonts/instructionLabel", "");
      this._lblCaption.ParentAlignment = AlignModes.Vertically;
      this._lblCaption.DropShadow = true;
      this._lblCaption.ShadowDistance = new Vector2(1f, 1f);
      this._lblCaption.ShadowColor = Color.Black;
      this.Controls.Add((UIControl) this._lblCaption);
      this._imgIcon = new UIImage(ownerScreen);
      this._imgIcon.ParentAlignment = AlignModes.Vertically;
      this.Controls.Add((UIControl) this._imgIcon);
      this.ParentAlignment = AlignModes.Vertically;
      this.LabelType = type;
      this.ResizeControl();
    }

    private void ResizeControl()
    {
      this._imgIcon.Position = new Vector2(0.0f, 0.0f);
      this._lblCaption.Position = new Vector2(this._imgIcon.Size.Width + 50f, 0.0f);
      this.Size = new Size(this._lblCaption.Position.X + this._lblCaption.Size.Width, Math.Max(this._imgIcon.Size.Height, this._lblCaption.Size.Height));
    }

    public override string ToString() => this._lblCaption.Text;

    public enum LabelActionTypes
    {
      Select,
      Back,
      TuneUpDown,
      AdjustSize,
      Accept,
      ToggleSlider,
      Continue,
      Start,
      StartNextStage,
      Quit,
      Retry,
    }

    [Flags]
    public enum ControllerKeys
    {
      LeftStick = 0,
      RightStick = 1,
      A = 2,
      B = A | RightStick, // 0x00000003
      Y = 4,
      X = Y | RightStick, // 0x00000005
      LeftRight = Y | A, // 0x00000006
    }
  }
}
