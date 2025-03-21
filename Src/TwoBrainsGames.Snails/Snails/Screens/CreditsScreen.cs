
// Type: TwoBrainsGames.Snails.Screens.CreditsScreen
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;
using TwoBrainsGames.Snails.Screens.CommonControls;
using TwoBrainsGames.Snails.Screens.Transitions;


namespace TwoBrainsGames.Snails.Screens
{
  internal class CreditsScreen(ScreenNavigator owner) : SnailsScreen(owner, ScreenType.Credits)
  {
    private UICreditsPanel _pnlGameDesign;
    private UICreditsPanel _pnlArt;
    private UICreditsPanel _pnlMusic;
    private UICreditsPanel _pnlAddLevelDesign;
    private UICreditsPanel _pnlSpecialThanks;
    private UICreditsPanel _pnlTeam;
    private UIMiniSnailsTitle _title;
    private UIImage _imgBrainsLogo;
    private UITimer _tmrShowPanels;
    private UISpriteFontLabel _versionAndContact;
    private Sample _showSound;
    private Sample _applauseSound;

    private CreditsScreen.ScreenState State { get; set; }

    public float PanelSpacing { get; set; }

    public float Column1Position { get; set; }

    public float Column2Position { get; set; }

    public float YPosition { get; set; }

    public override void OnLoad()
    {
      base.OnLoad();
      this.Name = "Credits";
      this.BackgroundImageBlendColor = Colors.CreditsScrBkColor;
      this.OnInitializeFromContent += new UIControl.UIEvent(this.CreditsScreen_OnInitializeFromContent);
      this.OnAfterInitializeFromContent += new UIControl.UIEvent(this.CreditsScreen_OnAfterInitializeFromContent);
      this.BackgroundType = SnailsScreen.ScreenBackgroundType.Image;
      this._pnlGameDesign = new UICreditsPanel((UIScreen) this);
      this._pnlGameDesign.AddCategory("CREDITS_DESIGN");
      this._pnlGameDesign.AddName("Alexandre Fontoura");
      this._pnlGameDesign.AddName("Jorge Lima");
      this.Controls.Add((UIControl) this._pnlGameDesign);
      this._pnlArt = new UICreditsPanel((UIScreen) this);
      this._pnlArt.AddCategory("CREDITS_ART");
      this._pnlArt.AddName("Jorge Lima");
      this.Controls.Add((UIControl) this._pnlArt);
      this._pnlMusic = new UICreditsPanel((UIScreen) this);
      this._pnlMusic.AddCategory("CREDITS_MUSIC");
      this._pnlMusic.AddName("Rui Querido");
      this.Controls.Add((UIControl) this._pnlMusic);
      this._pnlAddLevelDesign = new UICreditsPanel((UIScreen) this);
      this._pnlAddLevelDesign.AddCategory("CREDITS_ADDLEVEL");
      this._pnlAddLevelDesign.AddName("Francisco Ferreira");
      this.Controls.Add((UIControl) this._pnlAddLevelDesign);
      this._pnlSpecialThanks = new UICreditsPanel((UIScreen) this);
      this._pnlSpecialThanks.Name = "_pnlSpecialThanks";
      this._pnlSpecialThanks.AddCategory("CREDITS_THANKS");
      this._pnlSpecialThanks.AddName("Adrien Grandemange");
      this._pnlSpecialThanks.AddName("Emilio Bologna");
      this._pnlSpecialThanks.AddName("Lennart Böwering");
      this._pnlSpecialThanks.AddName("Leonor Cachapuz");
      this._pnlSpecialThanks.AddName("Marisa David");
      this._pnlSpecialThanks.AddName("Michelle Silva");
      this._pnlSpecialThanks.AddName("Miguel Lima");
      this._pnlSpecialThanks.AddName("Rodrigo A. Gusman");
      this.Controls.Add((UIControl) this._pnlSpecialThanks);
      this._pnlTeam = new UICreditsPanel((UIScreen) this);
      this._pnlTeam.AddCategory("CREDITS_TEAM");
      this._pnlTeam.AddName("Alexandre Fontoura");
      this._pnlTeam.AddName("Jorge Lima");
      this.Controls.Add((UIControl) this._pnlTeam);
      this._title = new UIMiniSnailsTitle((UIScreen) this);
      this._title.Name = "_title";
      this._title.ShowEffect = (TransformEffectBase) new SquashEffect(this._title.Scale.X * 0.9f, 2f, 0.01f, this._title.BlendColor, this._title.Scale);
      this.Controls.Add((UIControl) this._title);
      this._imgBrainsLogo = new UIImage((UIScreen) this, "spriteset/common-elements-1/BrainsLogo", "__STATIC__");
      this._imgBrainsLogo.Name = "_imgBrainsLogo";
      this._imgBrainsLogo.ShowEffect = (TransformEffectBase) new ColorEffect(new Color(0.0f, 0.0f, 0.0f, 0.0f), Color.White, 0.005f, false);
      this._imgBrainsLogo.ShowEffect = (TransformEffectBase) new SquashEffect(0.9f, 2f, 0.01f, this._imgBrainsLogo.BlendColor, this._imgBrainsLogo.Scale);
      this.Controls.Add((UIControl) this._imgBrainsLogo);
      this._tmrShowPanels = new UITimer((UIScreen) this, 500.0, true);
      this._tmrShowPanels.OnTimer += new UIControl.UIEvent(this._tmrShowPanels_OnTimer);
      this._tmrShowPanels.Parameter = (object) 0;
      this.ShowTrialTag = false;
      this.Controls.Add((UIControl) this._tmrShowPanels);
      this.ShowTrialTag = false;
      this._showSound = BrainGame.ResourceManager.GetSampleStatic("sfx/menu-item-shown");
      this._applauseSound = BrainGame.ResourceManager.GetSampleStatic("sfx/gold-medal");
      this._versionAndContact = new UISpriteFontLabel((UIScreen) this);
      this._versionAndContact.Margins.Left = 150f;
      this._versionAndContact.ParentAlignment = AlignModes.Bottom | AlignModes.Left;
      this._versionAndContact.Font = BrainGame.ResourceManager.Load<SpriteFont>("fonts/contactInfo");
      this._versionAndContact.Text = string.Format("Product version {0}, support contact {1}",
          (object) BrainGame.Settings.GameVersion, (object) BrainGame.Settings.SupportContact);
      this.Controls.Add((UIControl) this._versionAndContact);
    }

    public override void OnStart()
    {
      base.OnStart();
      this._pnlGameDesign.Visible = false;
      this._pnlArt.Visible = false;
      this._pnlMusic.Visible = false;
      this._pnlAddLevelDesign.Visible = false;
      this._pnlSpecialThanks.Visible = false;
      this._pnlTeam.Visible = false;
      this._tmrShowPanels.Enabled = true;
      this._imgBrainsLogo.Visible = false;
      this._title.Visible = false;
      this._tmrShowPanels.Parameter = (object) 0;
      this._tmrShowPanels.Time = 500.0;
      this._tmrShowPanels.Reset();
      this.State = CreditsScreen.ScreenState.ShowingPanels;
    }

    private void CreditsScreen_OnInitializeFromContent(IUIControl sender)
    {
      this.PanelSpacing = this.GetContentPropertyValue<float>("panelSpacing", 0.0f);
      this.Column1Position = this.GetContentPropertyValue<float>("column1Position", 0.0f);
      this.Column2Position = this.GetContentPropertyValue<float>("column2Position", 0.0f);
      this.YPosition = this.GetContentPropertyValue<float>("yPosition", 0.0f);
    }

    private void CreditsScreen_OnAfterInitializeFromContent(IUIControl sender)
    {
      this._pnlGameDesign.Position = new Vector2(this.Column1Position, this.YPosition);
      this._pnlArt.Position = new Vector2(this._pnlGameDesign.Left, this._pnlGameDesign.Bottom + this.PanelSpacing);
      this._pnlMusic.Position = new Vector2(this._pnlGameDesign.Left, this._pnlArt.Bottom + this.PanelSpacing);
      this._pnlAddLevelDesign.Position = new Vector2(this._pnlGameDesign.Left, this._pnlMusic.Bottom + this.PanelSpacing);
      this._pnlTeam.Position = new Vector2(this._pnlSpecialThanks.Left, this._pnlAddLevelDesign.Bottom + this.PanelSpacing);
    }

    public override void OnUpdate(BrainGameTime gameTime)
    {
      switch (this.State)
      {
        case CreditsScreen.ScreenState.ShowingPanels:
          if (!this._inputController.ActionAccept)
            break;
          this._tmrShowPanels.Enabled = false;
          this._pnlGameDesign.Visible = true;
          this._pnlArt.Visible = true;
          this._pnlMusic.Visible = true;
          this._pnlAddLevelDesign.Visible = true;
          this._pnlSpecialThanks.Visible = true;
          this._pnlTeam.Visible = true;
          this._imgBrainsLogo.Visible = true;
          this._title.Visible = true;
          this.State = CreditsScreen.ScreenState.Active;
          this._applauseSound.Play();
          break;
        case CreditsScreen.ScreenState.Active:
          if (!this._inputController.ActionBack && !this._inputController.ActionAccept)
            break;
          switch (this.Navigator.GlobalCache.Get<ScreenType>("CREDITS_SCREEN_CALLER"))
          {
            case ScreenType.MainMenu:
              this.Navigator.GlobalCache.Set("MAIN_SCREEN_STARTUP_MODE", (object) MainMenuScreen.StartupType.TitleAndMenuVisible);
              this.NavigateTo(ScreenType.MainMenu.ToString(), (Transition) ScreenTransitions.LeafsClosing, (Transition) ScreenTransitions.LeafsOpening);
              return;
            case ScreenType.Options:
              this.Navigator.GlobalCache.Set("OPTIONS_STARTUP_MODE", (object) OptionsScreen.StartupType.MenuVisible);
              this.NavigateTo(ScreenType.Options.ToString(), (Transition) ScreenTransitions.LeafsClosing, (Transition) ScreenTransitions.LeafsOpening);
              return;
            default:
              return;
          }
      }
    }

    private void _tmrShowPanels_OnTimer(IUIControl sender)
    {
      int parameter = (int) this._tmrShowPanels.Parameter;
      if (parameter <= 7)
        this._showSound.Play();
      switch (parameter)
      {
        case 0:
          this._title.Show();
          this._tmrShowPanels.Time = 500.0;
          break;
        case 1:
          this._imgBrainsLogo.Show();
          this._tmrShowPanels.Time = 500.0;
          break;
        case 2:
          this._pnlGameDesign.Show();
          break;
        case 3:
          this._pnlArt.Show();
          break;
        case 4:
          this._pnlMusic.Show();
          break;
        case 5:
          this._pnlAddLevelDesign.Show();
          break;
        case 6:
          this._pnlSpecialThanks.Show();
          break;
        case 7:
          this._pnlTeam.Show();
          this._applauseSound.Play();
          break;
        default:
          this._tmrShowPanels.Enabled = false;
          this.State = CreditsScreen.ScreenState.Active;
          break;
      }
      int num;
      this._tmrShowPanels.Parameter = (object) (num = parameter + 1);
    }

    private enum ScreenState
    {
      ShowingPanels,
      Active,
    }
  }
}
