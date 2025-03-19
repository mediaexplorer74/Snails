
// Type: TwoBrainsGames.BrainEngine.UI.Screens.Screen
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI.Controls;
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;


namespace TwoBrainsGames.BrainEngine.UI.Screens
{
  public class Screen
  {
    internal string _id;
    internal bool _closed;
    internal bool _active;
    internal bool _started;
    internal bool _skipped;
    private ScreenNavigator _navigator;
    public InputBase _inputController;
    internal float _skipTime;
    internal float _elapsedTime;

    public event UIControl.UIEvent OnOpenTransitionEnded;

    public event UIControl.UIEvent OnScreenModeChanged;

    public event UIControl.UIEvent OnGameLostFocus;

    public event UIControl.UIEvent OnGameGotFocus;

    public event UIControl.UIEvent OnLanguageChanged;

    public event UIControl.UIEvent OnGameplayModeChanged;

    public event UIControl.UIEvent OnPopupClosed;

    public event UIControl.UIEvent OnInitializeFromContent;

    public string Name { get; set; }

    public float TimeMultiplier { get; set; }

    public bool Closed => this._closed;

    public bool IsActive => this._active;

    public bool IsSkipped => this._skipped;

    public ScreenNavigator Navigator => this._navigator;

    internal DataFileRecord ScreenContentRootRecord { get; set; }

    public SpriteBatch SpriteBatch => BrainGame.SpriteBatch;

    internal bool Starting { get; set; }

    public Screen(ScreenNavigator navigator)
    {
      this._navigator = navigator;
      this._active = false;
      this._skipped = false;
      this.TimeMultiplier = 1f;
    }

    internal void Initialize(ScreenNavigator navigator)
    {
      this._navigator = navigator;
      this._inputController = new InputBase();
      this._inputController.Initialize();
    }

    internal virtual void Load()
    {
      this.OnLoad();
      this.OnAfterLoad();
    }

    internal virtual void InitializeFromContent()
    {
      if (string.IsNullOrEmpty(this.Name))
        return;
      this.ScreenContentRootRecord = BrainGame.ResourceManager.Load<DataFileRecord>(Path.Combine(BrainGame.Settings.NavigatorContentFolder, this.Name), ResourceManager.ResourceManagerCacheType.Temporary);
    }

    internal virtual void Start()
    {
      this.Starting = true;
      this._active = true;
      this._closed = false;
      this._skipped = false;
      this._started = true;
      this.OnStart();
      this.OnAfterStart();
      this.Starting = false;
    }

    internal virtual void Draw() => this.OnDraw();

    internal virtual void Update(BrainGameTime gameTime) => this.OnUpdate(gameTime);

    internal virtual void ScreenModeChanged()
    {
      if (this.OnScreenModeChanged == null)
        return;
      this.OnScreenModeChanged((IUIControl) null);
    }

    internal virtual void GameLostFocus()
    {
      if (this.OnGameLostFocus == null)
        return;
      this.OnGameLostFocus((IUIControl) null);
    }

    internal virtual void GameGotFocus()
    {
      if (this.OnGameGotFocus == null)
        return;
      this.OnGameGotFocus((IUIControl) null);
    }

    internal virtual void InternalLanguageChanged()
    {
      if (this.OnLanguageChanged == null)
        return;
      this.OnLanguageChanged((IUIControl) null);
    }

    internal virtual void InternalGameplayModeChanged()
    {
      if (this.OnGameplayModeChanged == null)
        return;
      this.OnGameplayModeChanged((IUIControl) null);
    }

    protected virtual void Close()
    {
      this._closed = true;
      this._active = false;
      this.Navigator.PopUpClosed(this);
    }

    public virtual void OnLoad()
    {
    }

    public virtual void OnAfterLoad()
    {
    }

    public virtual void OnStart()
    {
    }

    public virtual void OnAfterStart()
    {
    }

    public virtual void OnClose()
    {
    }

    public virtual void OnUnload()
    {
    }

    public virtual void OnUpdate(BrainGameTime gameTime)
    {
    }

    public virtual void OnDraw()
    {
    }

    public void NavigateTo(string groupId, string screenId)
    {
      this._active = false;
      this._navigator.NavigateTo(groupId, screenId);
    }

    public void NavigateTo(string screenId)
    {
      this._active = false;
      this._navigator.NavigateTo(screenId);
    }

    public void NavigateTo(string screenId, Transition closeTransition, Transition openTransition)
    {
      this._active = false;
      this._navigator.NavigateTo(screenId, closeTransition, openTransition);
    }

    public void NavigateTo(
      string groupId,
      string screenId,
      Transition closeTransition,
      Transition openTransition)
    {
      this._active = false;
      this._navigator.NavigateTo(groupId, screenId, closeTransition, openTransition);
    }

    public void PopUp(string screenId, bool currentRemainActive)
    {
      this._active = currentRemainActive;
      this._navigator.PopUp(screenId);
    }

    public override string ToString() => this._id == null ? "" : this._id;

    public void LauchOnOpenTransitionEnded()
    {
      if (this.OnOpenTransitionEnded == null)
        return;
      this.OnOpenTransitionEnded((IUIControl) null);
    }

    public void CenterCursor()
    {
      int x = BrainGame.ScreenWidth / 2;
      int y = BrainGame.ScreenHeight / 2;
      this._inputController.SetMotionPosition(new Vector2((float) x, (float) y));
      BrainGame.GameCursor.Position = new Vector2((float) x, (float) y);
    }

    public void InvokeInitializeFromContent()
    {
      if (this.OnInitializeFromContent == null)
        return;
      this.OnInitializeFromContent((IUIControl) null);
    }

    public void InvokePopUpClosed()
    {
      if (this.OnPopupClosed == null)
        return;
      this.OnPopupClosed((IUIControl) null);
    }

    protected T ReadCustomContentField<T>(string dataFileRecordPath, string name)
    {
      DataFileRecord dataFileRecord = this.ScreenContentRootRecord != null ? this.ScreenContentRootRecord.SelectRecord(dataFileRecordPath) : throw new BrainException("Screen.InitializeFromContent() not called. Set Screen.Name to a valid xdf file in the game content.");
      if (dataFileRecord == null)
        throw new BrainException("Screen content record with path [" + dataFileRecordPath + "] not found.");
      return dataFileRecord.GetFieldByName(name) != null ? dataFileRecord.GetFieldValue<T>(name) : throw new BrainException("Screen content field [" + name + "] in record with path [" + dataFileRecordPath + "] not found.");
    }

    protected T ReadCustomContentField<T>(
      string dataFileRecordPath,
      string name,
      string conditionFieldName,
      object conditionFieldValue)
    {
      if (this.ScreenContentRootRecord == null)
        throw new BrainException("Screen.InitializeFromContent() not called. Set Screen.Name to a valid xdf file in the game content.");
      DataFileRecord dataFileRecord = this.ScreenContentRootRecord.SelectRecordByField(dataFileRecordPath, conditionFieldName, conditionFieldValue);
      if (dataFileRecord == null)
        throw new BrainException("Screen content record with path [" + dataFileRecordPath + "] not found.");
      return dataFileRecord.GetFieldByName(name) != null ? dataFileRecord.GetFieldValue<T>(name) : throw new BrainException("Screen content field [" + name + "] in record with path [" + dataFileRecordPath + "] not found.");
    }
  }
}
