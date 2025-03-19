
// Type: TwoBrainsGames.BrainEngine.UI.Screens.ScreenNavigator
// Assembly: TwoBrainsGames.BrainEngine, Version=1.0.4922.27344, Culture=neutral, PublicKeyToken=null
// MVID: C528D6E9-D040-4064-BD08-2B0011881660
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;


namespace TwoBrainsGames.BrainEngine.UI.Screens
{
  public class ScreenNavigator
  {
    private BrainGame _game;
    private ScreensData _screensData;
    private List<Screen> _screens;
    private List<Screen> _activeScreens;
    private List<ScreenNavigator.ScreenAction> _queuedScreenActions;
    private string _currentGroupId;
    private Transition _transition;
    private ScreenNavigator.TransitionType _currentTransitionType;
    //private Thread _loadingThread;
    private bool _isLoadingGroup;

    private int LastScreenIdx => this._activeScreens.Count - 1;

    public bool Enabled { get; private set; }

    public InputBase InputController { get; private set; }

    public SpriteBatch SpriteBatch => BrainGame.SpriteBatch;

    public ScreenGlobalCache GlobalCache { get; private set; }

    public bool UseAssyncGroupLoading { get; set; }

    public bool StencilBufferEnabled { get; set; }

    public bool DrawEnabled { get; set; }

    public Screen ActiveScreen
    {
      get
      {
        return this.LastScreenIdx >= 0 && this.LastScreenIdx < this._activeScreens.Count ? this._activeScreens[this.LastScreenIdx] : (Screen) null;
      }
    }

    public ScreenNavigator(BrainGame game)
    {
      this._queuedScreenActions = new List<ScreenNavigator.ScreenAction>();
      this._game = game;
      this.GlobalCache = new ScreenGlobalCache();
      this.InputController = new InputBase();
      this.InputController.Initialize();
      this.DrawEnabled = true;
      this.Enabled = true;
    }

    public void LoadContent()
    {
      if (string.IsNullOrEmpty(BrainGame.Settings.NavigatorContentId))
        throw new BrainException("Error loading Navigator data. BrainGame.Settings.NavigatorContentId must be initialized");
      if (string.IsNullOrEmpty(BrainGame.Settings.NavigatorContentFolder))
        throw new BrainException("Error loading Navigator data. BrainGame.Settings.NavigatorContentFolder must be initialized");
      this._activeScreens = new List<Screen>();
      this._screens = new List<Screen>();
            try
            {
                this._screensData = BrainGame.ResourceManager.Load<ScreensData>
               (
                    //RnD
                    Path.Combine(BrainGame.Settings.NavigatorContentFolder, 
                    BrainGame.Settings.NavigatorContentId),
                    //ResourceManager.ResourceManagerCacheType.Temporary
                    ResourceManager.ResourceManagerCacheType.Static
               );
            }
            catch { }
    }

    private void LoadGroupThread(object param)
    {
      this.LoadGroupData(param.ToString());
      BrainGame.IsLoading = false;
    }

    private void LoadGroup(string groupId)
    {
      this.UnloadCurrentGroup();
      if (this.UseAssyncGroupLoading)
      {
        if (BrainGame.HddAccessIcon != null)
          BrainGame.HddAccessIcon.Visible = true;
        BrainGame.IsLoading = true;

        //RnD
        //this._loadingThread = new Thread(new ParameterizedThreadStart(this.LoadGroupThread));
        this._isLoadingGroup = true;
        //this._loadingThread.Start((object) groupId);

        //Plan B
        this.LoadGroupData(groupId);
      }
      else
        this.LoadGroupData(groupId);
    }

    private void LoadGroupData(string groupId)
    {
      foreach (ScreensData.ScreenData screenData in this._screensData._groupsData[groupId].ScreensData)
      {
        Screen screen = this._game.CreateScreen(this, screenData.ScreenId);
        if (screen == null)
          throw new BrainException("Could not create screen with Id [" + screenData.ScreenId + "]. Please check overriden method YourGame.CreateScreen()");
        screen._id = screenData.ScreenId;
        screen._skipTime = screenData.SkipTime;
        screen.Initialize(this);
        screen.Load();
        screen.InitializeFromContent();
        this._screens.Add(screen);
      }
      this._currentGroupId = groupId;
    }

    private void UnloadCurrentGroup()
    {
      if (this._currentGroupId == null)
        return;
      foreach (Screen screen in this._screens)
        screen.OnUnload();
      BrainGame.ResourceManager.UnloadTemporary();
      this._activeScreens.Clear();
      this._screens.Clear();
      this._currentGroupId = (string) null;
      GC.Collect();
    }

    public void NavigateToStartUp()
    {
      this.NavigateTo(this._screensData._startupGroup, this._screensData._startupScreenId);
    }

    public void NavigateTo(string screenId) => this.NavigateTo(this._currentGroupId, screenId);

    public void NavigateTo(string screenId, Transition closeTransition, Transition openTransition)
    {
      this.NavigateTo(this._currentGroupId, screenId, closeTransition, openTransition);
    }

    public void NavigateTo(
      string groupId,
      string screenId,
      Transition closeTransition,
      Transition openTransition)
    {
      if (closeTransition != null)
        this._queuedScreenActions.Add(new ScreenNavigator.ScreenAction(ScreenNavigator.ScreenActionType.CloseTransition, screenId, (object) closeTransition, true));
      this._queuedScreenActions.Add(new ScreenNavigator.ScreenAction(ScreenNavigator.ScreenActionType.GroupLoad, screenId, (object) groupId, true));
      this._queuedScreenActions.Add(new ScreenNavigator.ScreenAction(ScreenNavigator.ScreenActionType.NavigateTo, screenId, (object) groupId));
      if (openTransition != null)
        this._queuedScreenActions.Add(new ScreenNavigator.ScreenAction(ScreenNavigator.ScreenActionType.OpenTransition, screenId, (object) openTransition, true));
      this._queuedScreenActions.Add(new ScreenNavigator.ScreenAction(ScreenNavigator.ScreenActionType.RemoveTransition, screenId, (object) openTransition));
    }

    public void NavigateTo(string groupId, string screenId)
    {
      this._queuedScreenActions.Add(new ScreenNavigator.ScreenAction(ScreenNavigator.ScreenActionType.GroupLoad, screenId, (object) groupId, true));
      this._queuedScreenActions.Add(new ScreenNavigator.ScreenAction(ScreenNavigator.ScreenActionType.NavigateTo, screenId, (object) groupId));
    }

    public void PopUp(string screenId)
    {
      this._queuedScreenActions.Add(new ScreenNavigator.ScreenAction(ScreenNavigator.ScreenActionType.PopUp, screenId, (object) null, true));
    }

    private void ProcessQueuedScreenActions(BrainGameTime gameTime)
    {
      for (int index = 0; index < this._screens.Count; ++index)
      {
        if (this._screens[index]._closed && this._activeScreens.Contains(this._screens[index]))
        {
          this._activeScreens.Remove(this._screens[index]);
          this._screens[index].OnClose();
        }
      }
      if (this._queuedScreenActions.Count <= 0)
        return;
      for (int index = 0; index < this._queuedScreenActions.Count; index = index - 1 + 1)
      {
        this.ProcessScreenAction(this._queuedScreenActions[index], gameTime);
        if (this._queuedScreenActions[index]._syncronous)
        {
          this._queuedScreenActions.Remove(this._queuedScreenActions[index]);
          break;
        }
        this._queuedScreenActions.Remove(this._queuedScreenActions[index]);
      }
    }

    private Screen FindScreen(string screenId)
    {
      foreach (Screen screen in this._screens)
      {
        if (screen._id == screenId)
          return screen;
      }
      return (Screen) null;
    }

    internal void PopUpClosed(Screen popUpScreen)
    {
      for (int index = 0; index < this._activeScreens.Count; ++index)
      {
        if (this._activeScreens[index] == popUpScreen)
        {
          if (index - 1 < 0)
            break;
          this._activeScreens[index - 1]._active = true;
          this._activeScreens[index - 1].InvokePopUpClosed();
          break;
        }
      }
    }

    public void SendScreenModeChangedToScreens()
    {
      for (int index = 0; index < this._activeScreens.Count; ++index)
      {
        if (this._activeScreens[index]._active)
          this._activeScreens[index].ScreenModeChanged();
      }
    }

    private void SendLostFocusMessageToScreens()
    {
      for (int index = 0; index < this._activeScreens.Count; ++index)
      {
        if (this._activeScreens[index]._active)
          this._activeScreens[index].GameLostFocus();
      }
    }

    private void SendGotFocusMessageToScreens()
    {
      for (int index = 0; index < this._activeScreens.Count; ++index)
      {
        if (this._activeScreens[index]._active)
          this._activeScreens[index].GameGotFocus();
      }
    }

    internal void LanguageChanged()
    {
      foreach (Screen screen in this._screens)
        screen.InternalLanguageChanged();
    }

    internal void GameplayModeChanged()
    {
      foreach (Screen screen in this._screens)
        screen.InternalGameplayModeChanged();
    }

    internal void GameWindowActivated() => this.SendGotFocusMessageToScreens();

    internal void GameWindowDeactivated() => this.SendLostFocusMessageToScreens();

    public void Update(BrainGameTime gameTime)
    {
      if (!this.Enabled)
        return;
      if (BrainGame.IsGameActive)
      {
        this.InputController.Update(gameTime);
        BrainGame.GameCursor.Update(gameTime);
      }
      if (BrainGame.DisplayHDDAccessIcon && BrainGame.HddAccessIcon != null && BrainGame.HddAccessIcon.Visible)
        BrainGame.HddAccessIcon.Update(gameTime);
      if (BrainGame.Settings.UseAchievements && BrainGame.AchievementsManager.HasAchievementsInQueue)
        BrainGame.AchievementsManager.Update(gameTime);
      if (this._isLoadingGroup)
      {
        //RnD
        this._isLoadingGroup = default;//this._loadingThread.IsAlive;
        if (this._isLoadingGroup)
          return;
        if (BrainGame.HddAccessIcon != null)
          BrainGame.HddAccessIcon.Visible = false;
        this.ProcessQueuedScreenActions(gameTime);
      }
      else if (this._transition != null)
      {
        this._transition.Update(gameTime);
        if (!this._transition._ended)
          return;
        if (this.LastScreenIdx >= 0 && this._currentTransitionType == ScreenNavigator.TransitionType.Open)
          this._activeScreens[this.LastScreenIdx].LauchOnOpenTransitionEnded();
        this.ProcessQueuedScreenActions(gameTime);
      }
      else
      {
        this.ProcessQueuedScreenActions(gameTime);
        if (this.LastScreenIdx < 0)
          return;
        if (BrainGame.IsGameActive)
          this._activeScreens[this.LastScreenIdx]._inputController.Update(gameTime);
        for (int index = 0; index < this._activeScreens.Count; ++index)
        {
          Screen activeScreen = this._activeScreens[index];
          if (activeScreen._active)
          {
            if (!activeScreen._skipped && (double) activeScreen._skipTime != 0.0)
            {
              activeScreen._elapsedTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
              if ((double) activeScreen._elapsedTime >= (double) activeScreen._skipTime)
              {
                activeScreen._elapsedTime = 0.0f;
                activeScreen._skipped = true;
              }
            }
            gameTime.SetMultiplier(activeScreen.TimeMultiplier);
            activeScreen.Update(gameTime);
            activeScreen._inputController.Reset();
          }
        }
      }
    }

    private void ProcessScreenAction(ScreenNavigator.ScreenAction action, BrainGameTime gameTime)
    {
      switch (action._actionType)
      {
        case ScreenNavigator.ScreenActionType.PopUp:
          Screen screen1 = this.FindScreen(action._screenId);
          screen1._started = false;
          screen1._active = true;
          if (this._activeScreens.Count > this.LastScreenIdx)
            this._activeScreens[this.LastScreenIdx]._active = false;
          this._activeScreens.Add(screen1);
          screen1._inputController.Reset();
          screen1.Start();
          break;
        case ScreenNavigator.ScreenActionType.NavigateTo:
          Screen screen2 = this.FindScreen(action._screenId);
          if (screen2 == null)
            throw new BrainException("Could not find screen with Id " + action._screenId);
          if (this.ActiveScreen != null)
            this.ActiveScreen.OnClose();
          this._activeScreens.Clear();
          this._activeScreens.Add(screen2);
          screen2.Start();
          screen2.Update(gameTime);
          break;
        case ScreenNavigator.ScreenActionType.GroupLoad:
          if (!((string) action._param != this._currentGroupId))
            break;
          this.LoadGroup((string) action._param);
          break;
        case ScreenNavigator.ScreenActionType.OpenTransition:
          this._transition = (Transition) action._param;
          this._transition.Initialize();
          this._currentTransitionType = ScreenNavigator.TransitionType.Open;
          this._transition.OnStart();
          break;
        case ScreenNavigator.ScreenActionType.CloseTransition:
          this._transition = (Transition) action._param;
          this._transition.Initialize();
          this._currentTransitionType = ScreenNavigator.TransitionType.Close;
          this._transition.OnStart();
          break;
        case ScreenNavigator.ScreenActionType.RemoveTransition:
          this._transition = (Transition) null;
          break;
      }
    }

    public void Draw()
    {
      if (!this.DrawEnabled || !this.Enabled)
        return;
      BrainGame.Graphics.Clear(ClearOptions.Target, BrainGame.ClearColor, 0.0f, 0);
      if (!this._isLoadingGroup)
      {
        foreach (Screen activeScreen in this._activeScreens)
        {
          if (activeScreen._started)
            activeScreen.Draw();
        }
      }
      if (this._transition != null)
        this._transition.Draw();
      if (BrainGame.DisplayHDDAccessIcon && BrainGame.HddAccessIcon != null && BrainGame.HddAccessIcon.Visible)
      {
        this.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, (DepthStencilState) null, (RasterizerState) null, (Effect) BrainGame.RenderEffect);
        BrainGame.HddAccessIcon.Draw(this.SpriteBatch);
        this.SpriteBatch.End();
      }
      if (BrainGame.Settings.UseAchievements && BrainGame.AchievementsManager.HasAchievementsInQueue)
      {
        this.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, (DepthStencilState) null, (RasterizerState) null, (Effect) BrainGame.RenderEffect);
        BrainGame.AchievementsManager.Draw(this.SpriteBatch);
        this.SpriteBatch.End();
      }
      if (!BrainGame.GameCursor.Visible)
        return;
      BrainGame.GameCursor.Draw(this.SpriteBatch);
    }

    public void Enable() => this.Enabled = true;

    public void Disable() => this.Enabled = false;

    private enum TransitionType
    {
      Open,
      Close,
    }

    private enum ScreenActionType
    {
      PopUp,
      Close,
      NavigateTo,
      GroupLoad,
      OpenTransition,
      CloseTransition,
      RemoveTransition,
    }

    private class ScreenAction
    {
      public ScreenNavigator.ScreenActionType _actionType;
      public string _screenId;
      public object _param;
      public bool _syncronous;

      public ScreenAction(
        ScreenNavigator.ScreenActionType actionType,
        string screenId,
        object param)
        : this(actionType, screenId, param, false)
      {
      }

      public ScreenAction(
        ScreenNavigator.ScreenActionType actionType,
        string screenId,
        object param,
        bool syncronous)
      {
        this._actionType = actionType;
        this._screenId = screenId;
        this._param = param;
        this._syncronous = syncronous;
      }
    }
  }
}
