
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UIInstructionBar
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UIInstructionBar : UIControl
  {
    private const float LABEL_SPACE = 200f;
    private List<UIInstructionLabel> _labels;

    public override bool Visible
    {
      get => base.Visible && this.AnyLabelVisible();
      set
      {
        if (base.Visible == value)
          return;
        base.Visible = value;
        if (!base.Visible)
          return;
        this.HideAllLabels();
      }
    }

    public UIInstructionBar(UIScreen ownerScreen)
      : base(ownerScreen)
    {
      this._labels = new List<UIInstructionLabel>();
      this.ParentAlignment = AlignModes.Horizontaly | AlignModes.Bottom;
      this.Margins.Bottom = -50f;
      this.BackgroundColor = Colors.InstructionBarBackground;
      this.Size = new Size(ownerScreen.Size.Width, 600f);
      this.AcceptControllerInput = false;
      this.RepositionLabels();
    }

    public void AddLabel(UIInstructionLabel.LabelActionTypes labelType)
    {
      UIInstructionLabel control = new UIInstructionLabel(this.ScreenOwner, labelType);
      this.Controls.Add((UIControl) control);
      this._labels.Add(control);
      this.RepositionLabels();
    }

    private void RepositionLabels()
    {
      float num = 0.0f;
      foreach (UIInstructionLabel label in this._labels)
      {
        if (label.Visible)
        {
          num += label.Size.Width;
          num += 200f;
        }
      }
      float x = (float) ((double) this.Size.Width / 2.0 - (double) num / 2.0);
      foreach (UIInstructionLabel label in this._labels)
      {
        if (label.Visible)
        {
          label.Position = new Vector2(x, 0.0f);
          x += label.Size.Width + 200f;
        }
      }
    }

    private bool AnyLabelVisible()
    {
      foreach (UIControl label in this._labels)
      {
        if (label.Visible)
          return true;
      }
      return false;
    }

    public void ShowLabel(UIInstructionLabel.LabelActionTypes labelType)
    {
      foreach (UIInstructionLabel label in this._labels)
      {
        if (label.LabelType == labelType)
        {
          label.Visible = true;
          return;
        }
      }
      this.AddLabel(labelType);
      this.OrderLabels();
    }

    public void HideLabel(UIInstructionLabel.LabelActionTypes labelType)
    {
      foreach (UIInstructionLabel label in this._labels)
      {
        if (label.LabelType == labelType)
        {
          label.Visible = false;
          break;
        }
      }
    }

    public void HideAllLabels()
    {
      if (this._labels == null)
        return;
      foreach (UIControl label in this._labels)
        label.Visible = false;
    }

    public override void Update(BrainGameTime gameTime) => this.RepositionLabels();

    public override void Draw() => base.Draw();

    public void OrderLabels()
    {
      for (int index = 0; index < this.Controls.Count; ++index)
      {
        if (this.Controls[index] is UIInstructionLabel)
        {
          UIInstructionLabel control = (UIInstructionLabel) this.Controls[index];
          if (control.ControllerKey == UIInstructionLabel.ControllerKeys.B)
          {
            this._labels.RemoveAt(index);
            this._labels.Insert(this._labels.Count, control);
            this.Controls.RemoveAt(index);
            this.Controls.Insert(this.Controls.Count, (UIControl) control);
          }
        }
      }
    }
  }
}
