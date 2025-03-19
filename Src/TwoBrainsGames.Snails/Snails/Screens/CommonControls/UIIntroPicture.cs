
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UIIntroPicture
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UIIntroPicture : UIControl
  {
    private const int BKG_ALPHA = 120;
    private const float CLOUD1_SPEED = 0.08f;
    private const float CLOUD2_SPEED = 0.1f;
    private const float CLOUD3_SPEED = 0.05f;
    private const float SUNLIGHT_SPEED = 0.008f;
    private const int BB_IDX_CLOUDS = 0;
    private const int BB_IDX_SKY = 1;
    private const int BB_IDX_SUN = 2;
    private const int BB_IDX_CROWN = 3;
    private const int BB_CROWN_COUNT = 4;
    private UIImage _imgFrame;
    private UIImage _imgCloud1;
    private UIImage _imgCloud2;
    private UIImage _imgCloud3;
    private UIImage _imgSun;
    private UIImage _imgSky;
    private UIImage _imgSunLight;
    private UIImage _imgCrownShine;
    private UISnailsTitle _gameTitle;
    private UIPanel _bkgPanel;

    private BoundingSquare SkyArea
    {
      get => this.PixelsToScreenUnits(this._imgFrame.Sprite.BoundingBoxes[1]);
    }

    private BoundingSquare SunArea
    {
      get => this.PixelsToScreenUnits(this._imgFrame.Sprite.BoundingBoxes[2]);
    }

    public UIIntroPicture(UIScreen ownerScreen)
      : base(ownerScreen)
    {
      this._imgSky = new UIImage(this.ScreenOwner, "spriteset/intro2-2/Sky", "__TEMPORARY__");
      this._imgSky.SizeMode = ImageSizeMode.Stretch;
      this._imgSky.Name = nameof (_imgSky);
      this.Controls.Add((UIControl) this._imgSky);
      this._imgSunLight = new UIImage(this.ScreenOwner, "spriteset/intro2-2/SunLight", "__TEMPORARY__");
      this._imgSunLight.Scale = new Vector2(5f, 5f);
      this._imgSunLight.Name = "SunLight";
      this.Controls.Add((UIControl) this._imgSunLight);
      this._imgSun = new UIImage(this.ScreenOwner, "spriteset/intro2-2/Sun", "__TEMPORARY__");
      this.Controls.Add((UIControl) this._imgSun);
      this._imgCloud1 = new UIImage(this.ScreenOwner, "spriteset/intro2-2/Cloud1", "__TEMPORARY__");
      this.Controls.Add((UIControl) this._imgCloud1);
      this._imgCloud2 = new UIImage(this.ScreenOwner, "spriteset/intro2-2/Cloud2", "__TEMPORARY__");
      this.Controls.Add((UIControl) this._imgCloud2);
      this._imgCloud3 = new UIImage(this.ScreenOwner, "spriteset/intro2-2/Cloud3", "__TEMPORARY__");
      this.Controls.Add((UIControl) this._imgCloud3);
      this._imgFrame = new UIImage(this.ScreenOwner, "spriteset/intro2-1/Frame", "__TEMPORARY__");
      this._imgFrame.ParentAlignment = AlignModes.Bottom;
      this._imgFrame.Margins.Bottom = -1f;
      this.Controls.Add((UIControl) this._imgFrame);
      this._imgCrownShine = new UIImage(this.ScreenOwner, "spriteset/intro2-2/CrownShine", "__TEMPORARY__");
      this._imgCrownShine.OnLastFrame += new UIImage.LastFrameHandler(this._imgCrownShine_OnLastFrame);
      this.Controls.Add((UIControl) this._imgCrownShine);
      this._bkgPanel = new UIPanel(this.ScreenOwner);
      this._bkgPanel.BackgroundColor = new Color(0, 0, 0, 0);
      this._bkgPanel.Visible = false;
      this._bkgPanel.UseBlendColorOnBackground = true;
      this._bkgPanel.CanBeScaled = false;
      this._bkgPanel.Size = this.ScreenOwner.Size;
      this._bkgPanel.ShowEffect = (TransformEffectBase) new ColorEffect(new Color(0, 0, 0, 0), new Color(0, 0, 0, 120), 0.05f, false);
      this._bkgPanel.HideEffect = (TransformEffectBase) new ColorEffect(new Color(0, 0, 0, 120), new Color(0, 0, 0, 0), 0.05f, false);
      this.Controls.Add((UIControl) this._bkgPanel);
      this._gameTitle = new UISnailsTitle(this.ScreenOwner);
      this._gameTitle.Mode = UISnailsTitle.TitleMode.Leaf;
      this._gameTitle.Position = new Vector2(0.0f, 300f);
      this.Controls.Add((UIControl) this._gameTitle);
      this.Size = this.ScreenOwner.Size;
      this._imgSky.Size = this.Size;
      this.AcceptControllerInput = false;
      this.OnInitializeFromContent += new UIControl.UIEvent(this.UIIntroPicture_OnInitializeFromContent);
    }

    public override void InitializeFromContent()
    {
      base.InitializeFromContent();
      this.InitializeFromContent(nameof (UIIntroPicture));
    }

    private void UIIntroPicture_OnInitializeFromContent(IUIControl sender)
    {
      this._gameTitle.Scale = this.GetContentPropertyValue<Vector2>("titleScale", this._gameTitle.Scale);
    }

    public override void Load()
    {
      this._imgCloud1.Position = new Vector2(200f, 1000f);
      this._imgCloud2.Position = new Vector2(4000f, 2250f);
      this._imgCloud3.Position = new Vector2(7500f, 1600f);
    }

    public void Initialize()
    {
      this.PlaceCrownShine();
      this._imgSun.Position = this._imgFrame.Position + new Vector2(this.SunArea.Left * this._imgSun.Scale.X, this.SunArea.Top * this._imgSun.Scale.Y);
      this._imgSunLight.Position = this._imgSun.Position;
    }

    public override void Update(BrainGameTime gameTime)
    {
      this.UpdateCloud(this._imgCloud1, 0.08f, gameTime);
      this.UpdateCloud(this._imgCloud2, 0.1f, gameTime);
      this.UpdateCloud(this._imgCloud3, 0.05f, gameTime);
      this._imgSunLight.Rotation += (float) (0.00800000037997961 * gameTime.ElapsedGameTime.TotalMilliseconds);
    }

    private void UpdateCloud(UIImage cloud, float speed, BrainGameTime gameTime)
    {
      UIImage uiImage = cloud;
      uiImage.Position = uiImage.Position + new Vector2(speed * (float) gameTime.ElapsedGameTime.TotalMilliseconds, 0.0f);
      if ((double) cloud.Position.X <= 10000.0)
        return;
      cloud.Position = new Vector2(-cloud.Width, cloud.Position.Y);
    }

    private void PlaceCrownShine()
    {
      BoundingSquare boundingBox = this._imgFrame.Sprite.BoundingBoxes[BrainGame.Rand.Next(4) + 3];
      this._imgCrownShine.Position = this._imgFrame.Position + this.PixelsToScreenUnits(new Vector2(boundingBox.Left, boundingBox.Top) * this._imgCrownShine.Scale);
    }

    private void _imgCrownShine_OnLastFrame() => this.PlaceCrownShine();

    public void ResetBackgroundAlpha()
    {
      this._bkgPanel.BackgroundColor = new Color(0, 0, 0, 0);
      this._bkgPanel.BlendColor = new Color(0, 0, 0, 0);
      this._bkgPanel.Visible = false;
    }

    public void SetBackgroundAlpha()
    {
      this._bkgPanel.BackgroundColor = new Color(0, 0, 0, 120);
      this._bkgPanel.BlendColor = new Color(0, 0, 0, 120);
      this._bkgPanel.Visible = true;
    }

    public void FadeInBackground() => this._bkgPanel.Show();

    public void FadeOutBackground() => this._bkgPanel.Hide();

    public void ShowBackgroundPanel() => this._bkgPanel.Visible = true;

    public UIIntroPicture.IntroPictureSaveState GetSaveState()
    {
      return new UIIntroPicture.IntroPictureSaveState()
      {
        cloud1Pos = this._imgCloud1.Position,
        cloud2Pos = this._imgCloud2.Position,
        cloud3Pos = this._imgCloud3.Position,
        sunLightRot = this._imgSunLight.Rotation
      };
    }

    public void SetSaveState(UIIntroPicture.IntroPictureSaveState save)
    {
      if (save == null)
        return;
      this._imgCloud1.Position = save.cloud1Pos;
      this._imgCloud2.Position = save.cloud2Pos;
      this._imgCloud3.Position = save.cloud3Pos;
      this._imgSunLight.Rotation = save.sunLightRot;
    }

    public class IntroPictureSaveState
    {
      public Vector2 cloud1Pos;
      public Vector2 cloud2Pos;
      public Vector2 cloud3Pos;
      public float sunLightRot;
    }
  }
}
