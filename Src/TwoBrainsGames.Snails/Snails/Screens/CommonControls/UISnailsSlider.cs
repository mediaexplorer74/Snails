
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UISnailsSlider
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UISnailsSlider : UISlider
  {
    private Sample _sliderSample;

    public bool PlayChangedSound { get; set; }

    public UISnailsSlider(UIScreen screenOwner)
      : base(screenOwner, "spriteset/boards/Slider", "spriteset/boards/SliderManipulator")
    {
      this.ParentAlignment = AlignModes.Horizontaly;
      this.SetSliderSlotToSpriteBB(true);
      this.OnValueChangedEnded += new UIControl.UIEvent(this.UISnailsSlider_OnValueChangedEnded);
      this.OnValueChanged += new UIControl.UIEvent(this.UISnailsSlider_OnValueChanged);
      this._sliderSample = BrainGame.ResourceManager.GetSampleStatic("sfx/slider-move");
    }

    private void UISnailsSlider_OnValueChangedEnded(IUIControl sender)
    {
      if (!this.PlayChangedSound)
        return;
      this._sliderSample.Stop();
    }

    private void UISnailsSlider_OnValueChanged(IUIControl sender)
    {
      if (!this.PlayChangedSound)
        return;
      this._sliderSample.Play();
    }
  }
}
