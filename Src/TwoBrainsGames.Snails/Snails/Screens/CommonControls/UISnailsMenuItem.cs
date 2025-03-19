
// Type: TwoBrainsGames.Snails.Screens.CommonControls.UISnailsMenuItem
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.TransformEffects;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.Snails.Effects;


namespace TwoBrainsGames.Snails.Screens.CommonControls
{
  internal class UISnailsMenuItem : UIMenuItem
  {
    protected const float SELECTED_ITEM_SCALE = 1f;
    public static Vector2 DEFAULT_SCALE = new Vector2(0.9f, 0.9f);
    private Sample _itemSelectedSample;
    private Sample _itemFocusSample;

    public event UIControl.UIEvent OnSelect;

    public UIInstructionLabel.LabelActionTypes InstructionLabelType { get; protected set; }

    public bool AutoHideMenu { get; set; }

    public UISnailsMenu SnailsMenuOwner { get; private set; }

    private UISnailsMenu.MenuItemSize ItemSize { get; set; }

    public new bool WithFocus { get; private set; }

    public UISnailsMenuItem(
      UIScreen ownerScreen,
      string textResourceId,
      TextFont spriteFont,
      UISnailsMenu snailsMenuOwner,
      bool allowsSelection)
      : base(ownerScreen, textResourceId, spriteFont)
    {
      this.HotSpotBBIndex = 0;
      switch (snailsMenuOwner.ItemSize)
      {
        case UISnailsMenu.MenuItemSize.Medium:
          this.Image = BrainGame.ResourceManager.GetSpriteStatic("spriteset/boards", "MenuItemMedium");
          break;
        case UISnailsMenu.MenuItemSize.Big:
          this.Image = BrainGame.ResourceManager.GetSpriteStatic("spriteset/boards", "MenuItemBig");
          break;
      }
      this.AutoHideMenu = true;
      this.InvokeOnAcceptOnMotionUp = true;
      this.SnailsMenuOwner = snailsMenuOwner;
      if (allowsSelection)
        this.OnAcceptBegin += new UIControl.UIEvent(this.UISnailsMenuItem_OnAcceptBegin);
      this._itemSelectedSample = BrainGame.ResourceManager.GetSampleStatic("sfx/menu-item-selected");
      this.OnFocus += new UIControl.UIEvent(this.UISnailsMenuItem_OnFocus);
      this.Initialize();
      if (Game1.GameSettings.UseTouch)
        return;
      this._itemFocusSample = BrainGame.ResourceManager.GetSampleStatic("sfx/menu-item-focus");
    }

    private void UISnailsMenuItem_OnFocus(IUIControl sender)
    {
    }

    private void UISnailsMenuItem_OnAcceptBegin(IUIControl sender)
    {
      this._itemSelectedSample.Play();
    }

    public virtual void Initialize()
    {
      this.Reset();
      this.TextScale = new Vector2(1f, 1f);
      this.ParentAlignment = AlignModes.Horizontaly;
      this.InstructionLabelType = UIInstructionLabel.LabelActionTypes.Accept;
      this.ShowEffect = (TransformEffectBase) new SquashEffect(0.73f, 4f, 0.04f, this.BlendColor, this.Scale);
      this.HideEffect = (TransformEffectBase) new PopOutEffect(new Vector2(1.2f, 1.2f), 6f, this.BlendColor, this.Scale);
      this.Label.BlendColor = Colors.MenuItem;
      this.Resize();
      this.Scale = UISnailsMenuItem.DEFAULT_SCALE;
    }

    public void InvokeOnSelect()
    {
      if (this.OnSelect == null)
        return;
      this.OnSelect((IUIControl) this);
    }

    public override void Show()
    {
      this.Scale = UISnailsMenuItem.DEFAULT_SCALE;
      this.Label.BlendColor = Colors.MenuItem;
      base.Show();
    }

    public virtual void Reset()
    {
      this.Scale = UISnailsMenuItem.DEFAULT_SCALE;
      this.Label.Effect = (ITransformEffect) null;
      this.Label.BlendColor = Colors.MenuItem;
      this.Label.BlendScaleWithParent = false;
      this.Effect = (ITransformEffect) null;
      this.WithFocus = false;
      ((UITextFontLabel) this.Label).Font = this.SnailsMenuOwner.MenuItemsFontUnselected;
    }

    public override void Hide()
    {
      this.Label.BlendScaleWithParent = true;
      ((UITextFontLabel) this.Label).Font = this.SnailsMenuOwner.MenuItemsFont;
      base.Hide();
    }

    public virtual void GotFocus()
    {
      this.Effect = (ITransformEffect) new ScaleEffect(UISnailsMenuItem.DEFAULT_SCALE, 2f, new Vector2(1f, 1f), false);
      this.Label.BlendColor = Colors.MenuItemSelected;
      this.Label.BlendScaleWithParent = true;
      ((UITextFontLabel) this.Label).Font = this.SnailsMenuOwner.MenuItemsFont;
      this.WithFocus = true;
      if (this._itemFocusSample == null)
        return;
      this._itemFocusSample.Play();
    }
  }
}
