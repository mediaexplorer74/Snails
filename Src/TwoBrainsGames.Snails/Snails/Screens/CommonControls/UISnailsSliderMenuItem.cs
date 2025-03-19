
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UISnailsSliderMenuItem
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UISnailsSliderMenuItem : UISnailsMenuItem
  {
    private UISnailsSlider _slider;
    private UITextFontLabel _plusLabel;

    public event UIControl.UIEvent OnValueChanged;

    public event UIControl.UIEvent OnValueChangedEnded;

    public float Value
    {
      get => this._slider.Value;
      set => this._slider.Value = value;
    }

    public bool PlayChangedSound
    {
      get => this._slider.PlayChangedSound;
      set => this._slider.PlayChangedSound = value;
    }

    public UISnailsSliderMenuItem(UIScreen ownerScreen, UISnailsMenu snailsMenuOwner)
      : base(ownerScreen, (string) null, (TextFont) null, snailsMenuOwner, false)
    {
      this.HotSpotBBIndex = 1;
      this.Image = (Sprite) null;
      this._slider = new UISnailsSlider(ownerScreen);
      this._slider.OnValueChanged += new UIControl.UIEvent(this._slider_OnValueChanged);
      this._slider.OnValueChangedEnded += new UIControl.UIEvent(this._slider_OnValueChangedEnded);
      this._slider.OnFocus += new UIControl.UIEvent(this._slider_OnFocus);
      this._slider.OnLostFocus += new UIControl.UIEvent(this._slider_OnLostFocus);
      this.Controls.Add((UIControl) this._slider);
      this._plusLabel = new UITextFontLabel(ownerScreen, snailsMenuOwner.MenuItemsFontUnselected);
      this._plusLabel.Text = "+";
      this._plusLabel.ParentAlignment = AlignModes.Right;
      this._plusLabel.Margins.Right = this.NativeResolutionX(150f);
      this._plusLabel.Position = this.NativeResolution(new Vector2(0.0f, 340f));
      this._slider.Controls.Add((UIControl) this._plusLabel);
      this.Label.ParentAlignment = AlignModes.None;
      this.Label.Position = this.NativeResolution(new Vector2(250f, 340f));
      this.Label.BringToFront();
      this.Size = this._slider.Size;
      this.AutoHideMenu = false;
      this.BackgroundImage = this._slider.Image;
      this.Reset();
    }

    public override void Reset()
    {
      base.Reset();
      if (this._plusLabel == null)
        return;
      this._plusLabel.Font = this.SnailsMenuOwner.MenuItemsFontUnselected;
      this._plusLabel.BlendColor = Colors.MenuItem;
      this._plusLabel.BlendScaleWithParent = false;
    }

    private void _slider_OnLostFocus(IUIControl sender) => this.InvokeOnLostFocus();

    private void _slider_OnFocus(IUIControl sender) => this.InvokeOnFocus();

    private void _slider_OnValueChanged(IUIControl sender)
    {
      if (this.OnValueChanged == null)
        return;
      this.OnValueChanged((IUIControl) this);
    }

    private void _slider_OnValueChangedEnded(IUIControl sender)
    {
      if (this.OnValueChangedEnded == null)
        return;
      this.OnValueChangedEnded((IUIControl) this);
    }

    protected override void Resize()
    {
      base.Resize();
      if (this._slider == null)
        return;
      this.Size = this._slider.Size;
    }

    public override void GotFocus()
    {
      base.GotFocus();
      this._plusLabel.BlendColor = Colors.MenuItemSelected;
      this._plusLabel.BlendScaleWithParent = true;
      this._plusLabel.Font = this.SnailsMenuOwner.MenuItemsFont;
    }
  }
}
