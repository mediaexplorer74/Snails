
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UISoundMenu
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UISoundMenu : UISnailsMenu
  {
    private UISnailsSliderMenuItem _fxSlider;
    private UISnailsSliderMenuItem _musicSlider;

    public bool StopPlayMusic { get; set; }

    private bool ControlReady { get; set; }

    public UISoundMenu(UIScreen ownerScreen)
      : base(ownerScreen)
    {
      this.TitleSize = UISnailsMenuTitle.TitleSize.Big;
      this.ItemSize = UISnailsMenu.MenuItemSize.Big;
      this.TextResourceId = "MNU_SOUND_SETTINGS";
      this._fxSlider = this.AddSliderItem("MNU_ITEM_SOUND_VOLUME");
      this._fxSlider.OnValueChanged += new UIControl.UIEvent(this._fxSlider_OnValueChanged);
      this._musicSlider = this.AddSliderItem("MNU_ITEM_MUSIC_VOLUME");
      this._musicSlider.OnValueChanged += new UIControl.UIEvent(this._musicSlider_OnValueChanged);
      this._musicSlider.OnValueChangedEnded += new UIControl.UIEvent(this._musicSlider_OnValueChangedEnded);
      this.OnMenuShownBegin += new UIControl.UIEvent(this.UISoundMenu_OnMenuShownBegin);
      this.OnMenuShown += new UIControl.UIEvent(this.UISoundMenu_OnMenuShown);
      this.OnBackPressed += new UIControl.UIEvent(this.MenuItem_OnBack);
      this.OnResize();
      this.WithBackButton = true;
    }

    private void UISoundMenu_OnMenuShown(IUIControl sender)
    {
      ((SnailsScreen) this.ScreenOwner).EnableInput();
      this._fxSlider.Focus();
      this.ScreenOwner.CursorSnapDirections = SnapDirection.Up | SnapDirection.Down;
      this.ControlReady = true;
    }

    private void UISoundMenu_OnMenuShownBegin(IUIControl sender)
    {
      this.ControlReady = false;
      this._fxSlider.Value = (float) Game1.ProfilesManager.CurrentProfile.SoundVolume;
      this._fxSlider.PlayChangedSound = true;
      this._musicSlider.Value = (float) Game1.ProfilesManager.CurrentProfile.MusicVolume;
      this._musicSlider.PlayChangedSound = false;
    }

    private void _fxSlider_OnValueChanged(IUIControl sender)
    {
      if (Game1.ProfilesManager.CurrentProfile != null)
        Game1.ProfilesManager.CurrentProfile.SoundVolume = (int) this._fxSlider.Value;
      BrainGame.SampleManager.MasterVolume = this._fxSlider.Value / 100f;
    }

    private void _musicSlider_OnValueChanged(IUIControl sender)
    {
      if (this.StopPlayMusic && this.ControlReady)
      {
        if (BrainGame.MusicManager.IsMusicPaused)
          BrainGame.MusicManager.ResumeMusic();
        else
          Levels.CurrentLevel.StageSound.PlayMusic();
      }
      if (Game1.ProfilesManager.CurrentProfile != null)
        Game1.ProfilesManager.CurrentProfile.MusicVolume = (int) this._musicSlider.Value;
      BrainGame.MusicManager.MasterVolume = this._musicSlider.Value / 100f;
    }

    private void _musicSlider_OnValueChangedEnded(IUIControl sender)
    {
      if (!this.StopPlayMusic || !this.ControlReady)
        return;
      BrainGame.MusicManager.PauseMusic();
    }

    private void MenuItem_OnBack(IUIControl sender) => this.InvokeOnHide();
  }
}
