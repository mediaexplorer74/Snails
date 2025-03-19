
// Type: TwoBrainsGames.Snails.Stages.Stage
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Collision;
using TwoBrainsGames.BrainEngine.Data.DataFiles;
using TwoBrainsGames.BrainEngine.Effects;
using TwoBrainsGames.BrainEngine.Effects.Shades;
using TwoBrainsGames.BrainEngine.Graphics;
using TwoBrainsGames.BrainEngine.Input;
using TwoBrainsGames.BrainEngine.Resources;
using TwoBrainsGames.BrainEngine.UI.Screens;
using TwoBrainsGames.BrainEngine.UI.Screens.Transitions;
using TwoBrainsGames.Snails.Input;
using TwoBrainsGames.Snails.Player;
using TwoBrainsGames.Snails.Screens;
using TwoBrainsGames.Snails.Screens.Transitions;
using TwoBrainsGames.Snails.StageObjects;
using TwoBrainsGames.Snails.Stages.HUD;
using TwoBrainsGames.Snails.ToolObjects;


namespace TwoBrainsGames.Snails.Stages
{
  public class Stage : IBrainComponent, ISnailsDataFileSerializable, IDataFileSerializable
  {
    public const int TIME_WARP_MULTIPLIER = 4;
    public const int NORMAL_TIME_MULTIPLIER = 1;
    public const double STAGE_COMPLETION_DELAY = 3000.0;
    public const double STAGE_COMPLETION_DELAY_MORE_SNAILS = 1000.0;
    public const double STAGE_FAILED_DELAY = 3000.0;
    public const double STAGE_MISSION_CHECK_TIMER = 500.0;
    public const string STAGES_ID = "stages/{0}/{1}";
    public const string STAGES_THEME_ID = "stages/{0}/{0}";
    public const string SPRITESET_ID = "spriteset/{0}/{0}";
    public const string STAGES_ASSET_TILES = "tiles";
    public const string STAGES_ASSET_OBJECTS = "spriteset/{0}/{0}-objects";
    public const string STAGES_ASSET_TILESFRAGMENTS = "spriteset/{0}/{0}-tiles-fragments";
    public const string STAGES_ASSET_TEXTFONT_ID = "fonts/{0}/{0}-{1}";
    public const int QUADTREE_MAX_OBJECTS_PER_NODE = 10;
    public const int QUADTREE_MIN_NODE_SIZE = 200;
    public const int QUADTREE_OBJ_LIST_COUNT = 3;
    public const int QUADTREE_SNAIL_LIST_IDX = 0;
    public const int QUADTREE_STAGEOBJ_LIST_IDX = 1;
    public const int QUADTREE_PATH_LIST_IDX = 2;
    public const int TRANSF_EFFECT_CAMERA_SHAKE = 1;
    public const int TILE_WIDTH = 60;
    public const int TILE_HEIGHT = 60;
    public const int BONUS_SNAILS_DELIVERED = 5;
    public const int BONUS_TIME_TAKEN = 1;
    public const int BONUS_UNUSED_TOOL = 5;
    public const int BONUS_BRONZE_COINS = 20;
    public const int BONUS_SILVER_COINS = 50;
    public const int BONUS_GOLD_COINS = 100;
    public static Stage CurrentStage;
    private Stage.MissionFailedReasonType _missionFailedReason;
    private double _missionStateCheckTimer;
    private double _stageEndedTimer;
    private double _stageCompletedDelay;
    private bool fireIsPlaying;
    private WaterShadeEffect _waterShadeEffect;
    public bool HasWater;
    public Texture2D WaterTexture;
    private RenderTarget2D _waterRenderTarget;
    private GameplayRecorder _gameplayRecorder;
    public string Id;
    public string Description;
    public bool _withShadows;
    public StageHUD StageHUD;
    public StageStats Stats;
    public LevelStage LevelStage;
    public Board Board;
    public StageCursor Cursor = new StageCursor();
    public InGameCamera Camera;
    public List<Snail> Snails;
    public List<StageObject> Objects = new List<StageObject>();
    public List<StageObject> ObjectsRemove = new List<StageObject>();
    public List<StageObject> ObjectsAdd = new List<StageObject>();
    public List<StageObject> LinksToRemove = new List<StageObject>();
    public List<Liquid> LiquidObjects = new List<Liquid>();
    public List<SnailsBackgroundLayer> Layers = new List<SnailsBackgroundLayer>();
    public List<StageObject> BackgroundObjectsDrawList = new List<StageObject>();
    public List<StageObject> ForegroundObjectsDrawList = new List<StageObject>();
    public List<StageObject> ForegroundWaterDrawList = new List<StageObject>();
    public List<ParticlesEffect> Particles = new List<ParticlesEffect>();
    private RenderTarget2D _renderTarget;
    public bool IsPaused;
    public Vector2 _backgroundLayersOffset;
    private Stage.MissionStateType _missionState;
    public ControllerRumble Rumble;
    public Stage.StageState _state;
    public bool _withLiquids;
    private Sample _stageCompletedSound;

    public event Stage.StageDrawEventHandler OnBeforeObjectsDraw;

    public bool InTimeWarp { get; private set; }

    public string Key => this.LevelStage.StageKey;

    public GameplayInput Input => GameplayScreen.Instance.Input;

    public PlayerProfile Player => Game1.ProfilesManager.CurrentProfile;

    public StageData StageData => Levels._instance.StageData;

    public RenderTarget2D RenderTarget
    {
      get => this._renderTarget;
      private set => this._renderTarget = value;
    }

    public Stage.MissionStateType MissionState
    {
      get => this._missionState;
      set
      {
        if (this._missionState == value)
          return;
        this._missionState = value;
        if (this.StageHUD == null)
          return;
        this.StageHUD.MissionStateChanged();
      }
    }

    public GameplayRecorder GameplayRecorder => this._gameplayRecorder;

    public SpriteBatch SpriteBatch => Levels.CurrentLevel.SpriteBatch;

    public int[] StartupTutorialTopics { get; private set; }

    public string StartupTopicsString { get; set; }

    public Vector2 StartupCameraPOI
    {
      get
      {
        return new Vector2((float) (((double) this.StartupCenter.X - 1.0) * 60.0 + 30.0), (float) (((double) this.StartupCenter.Y - 1.0) * 60.0 + 30.0));
      }
    }

    public Vector2 StartupCenter { get; set; }

    public int BuildNr { get; private set; }

    public MedalScoreCriteria GoldMedalScoreCriteria { get; set; }

    public MedalScoreCriteria SilverMedalScoreCriteria { get; set; }

    public MedalScoreCriteria BronzeMedalScoreCriteria { get; set; }

    public bool FireIsPlaying
    {
      get => this.fireIsPlaying;
      set => this.fireIsPlaying = value;
    }

    public bool IsCustomStage => this.LevelStage.IsCustomStage;

    public static Stage.StageLoadingContext LoadingContext { get; set; }

    public LightManager LightManager { get; private set; }

    public Color ShadowColor => Levels.CurrentThemeSettings._shadowColor;

    public Stage()
      : this((LevelStage) null)
    {
    }

    public Stage(LevelStage levelStage)
    {
      Stage.CurrentStage = this;
      this.LevelStage = levelStage;
      this.Snails = new List<Snail>();
      this.Stats = new StageStats();
      this.StageHUD = new StageHUD();
      this.Board = new Board();
      this._waterRenderTarget = (RenderTarget2D) null;
      this.GoldMedalScoreCriteria = new MedalScoreCriteria(this);
      this.SilverMedalScoreCriteria = new MedalScoreCriteria(this);
      this.BronzeMedalScoreCriteria = new MedalScoreCriteria(this);
      this.MissionState = Stage.MissionStateType.None;
      this.Camera = new InGameCamera(this);
      this.LightManager = new LightManager();
      this._gameplayRecorder = new GameplayRecorder(this);
    }

    public void Initialize()
    {
      GameplayScreen.Instance.TimeMultiplier = 1f;
      this.Stats = new StageStats();
      this.Stats.NumSnailsToSave = this.LevelStage._snailsToSave;
      if (this.LevelStage._goal == GoalType.TimeAttack)
        this.Stats.Timer = this.LevelStage._targetTime;
      this.StageHUD.Initialize();
      this.Cursor.Initialize();
      this.Camera.Initialize();
      this.Board.Initialize();
      Game1.Instance.ActiveCamera = (Camera2D) this.Camera;
      foreach (SnailsBackgroundLayer layer in this.Layers)
        layer.Initialize();
      this._stageEndedTimer = 0.0;
      this._missionStateCheckTimer = 0.0;
      BrainGame.GameTime.Reset();
      Game1.Tutorial.Initialize();
      if (Game1.GameSettings.WithRumbble)
        this.Rumble = new ControllerRumble(Stage.CurrentStage.Input.GamePad);
      if (this.HasWater && Game1.GameSettings.UseWaterEffect)
      {
        this._waterShadeEffect = new WaterShadeEffect((Screen) GameplayScreen.Instance);
        this._waterShadeEffect.Initialize();
      }
      this._gameplayRecorder.Initialize();
      this.StartupTutorialTopics = (int[]) null;
      if (!string.IsNullOrEmpty(this.StartupTopicsString))
      {
        string[] strArray = this.StartupTopicsString.Split(',');
        this.StartupTutorialTopics = new int[strArray.Length];
        for (int index = 0; index < this.StartupTutorialTopics.Length; ++index)
          this.StartupTutorialTopics[index] = Convert.ToInt32(strArray[index]);
      }
      this.LightManager.Initialize();
      PresentationParameters presentationParameters = this.SpriteBatch.GraphicsDevice.PresentationParameters;
      this.RenderTarget = new RenderTarget2D(this.SpriteBatch.GraphicsDevice, BrainGame.Viewport.Width, BrainGame.Viewport.Height, false, presentationParameters.BackBufferFormat, presentationParameters.DepthStencilFormat);
    }

    public void LoadContent()
    {
      this.StageHUD.LoadContent();
      this.Cursor.LoadContent();
      this.Board.LoadContent();
      this.Stats.NumSnailsToRelease = this.GetTotalSnailsToRelease();
      this._waterRenderTarget = (RenderTarget2D) null;
      foreach (BackgroundLayer layer in this.Layers)
        layer.LoadContent();
      this.MissionState = Stage.MissionStateType.Starting;
      this._missionFailedReason = Stage.MissionFailedReasonType.Incomplete;
      this.Camera.SetOriginToHudCenter(this.StageHUD);
      if (this.HasWater && Game1.GameSettings.UseWaterEffect)
        this._waterShadeEffect.LoadContent();
      this._gameplayRecorder.LoadContent();
      this._stageCompletedSound = BrainGame.ResourceManager.GetSampleTemporary("sfx/stage-completed");
    }

    public int GetTotalSnailsToRelease()
    {
      int totalSnailsToRelease = 0;
      foreach (StageObject stageObject in this.Objects)
      {
        if (stageObject is StageEntrance)
        {
          StageEntrance stageEntrance = (StageEntrance) stageObject;
          if (!stageEntrance.ReleasesEvilSnails)
            totalSnailsToRelease += stageEntrance.TotalSnailsToRelease;
        }
      }
      return totalSnailsToRelease;
    }

    public void OnOpenTransitionEnded()
    {
      if (this.StartupTutorialTopics == null || this.StartupTutorialTopics.Length <= 0)
        return;
      this.ShowTutorialTopics(this.StartupTutorialTopics, true);
    }

    public void Start()
    {
      Levels.CurrentLevel.StageSound.PlayAmbience();
      if (this.StartupCameraPOI != Vector2.Zero)
        this.Camera.MoveTo(this.StartupCameraPOI);
      else
        this.Camera.MoveToOrigin();
      if ((double) Game1.GameSettings.MaxZoomOut != 1.0)
      {
        this.Camera.FullZoomOut();
        this.Camera.CenterInStage();
      }
      this._state = Stage.StageState.Startup;
      this.StageHUD.ShowIncomingMessage(IncomingMessage.MessageType.ClickToStart);
      this.QueryGameplayLoadingRecording();
      this.UpdateAudibleBoundinBox();
    }

    public void StartupEnded()
    {
      Levels.CurrentLevel.StageSound.PlayMusic();
      this._state = Stage.StageState.Playing;
      this.MissionState = Stage.MissionStateType.Running;
      this.StageHUD.OnStageStarted();
      switch (this.LevelStage._goal)
      {
        case GoalType.SnailDelivery:
          this.StageHUD.ShowIncomingMessage(IncomingMessage.MessageType.DeliveryStart);
          break;
        case GoalType.SnailKiller:
          this.StageHUD.ShowIncomingMessage(IncomingMessage.MessageType.SnailKillerStart);
          break;
        case GoalType.SnailKing:
          this.StageHUD.ShowIncomingMessage(IncomingMessage.MessageType.SnailKingStart);
          break;
        case GoalType.TimeAttack:
          this.StageHUD.ShowIncomingMessage(IncomingMessage.MessageType.TimeAttackStart);
          break;
      }
      this.Camera.StageStartupZoomIn(this.StartupCameraPOI);
      foreach (StageObject stageObject in this.Objects)
        stageObject.StageStartupPhaseEnded();
    }

    public void QueryGameplayLoadingRecording()
    {
      this._gameplayRecorder.Enabled = false;
      this.Cursor.EndPanEaseOutEnabled = !this._gameplayRecorder.Enabled;
    }

    public void HandleEvents(BrainGameTime gameTime)
    {
      Vector2 zero = Vector2.Zero;
      if (Game1.GameSettings.UseKeyboard)
      {
        if (this.Input.IsCameraUpPressed)
          zero.Y -= 5f;
        if (this.Input.IsCameraDownPressed)
          zero.Y += 5f;
        if (this.Input.IsCameraLeftPressed)
          zero.X -= 5f;
        if (this.Input.IsCameraRightPressed)
          zero.X += 5f;
      }
      else if (Game1.GameSettings.UseGamepad && Stage.CurrentStage.Input.CameraMotion)
      {
        zero.X += Stage.CurrentStage.Input.CameraMotionPosition.X * 7f;
        zero.Y += Stage.CurrentStage.Input.CameraMotionPosition.Y * -7f;
      }
      if (zero != Vector2.Zero)
        this.Camera.MoveByOffset(zero);
      if (!this.Input.ActionPause || this.StageHUD.ControlButtonsVisible)
        return;
      this.PauseGame();
    }

    private void UpdateMissionState(BrainGameTime gameTime)
    {
      if (this.MissionState == Stage.MissionStateType.Running)
      {
        this._missionStateCheckTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
        if (this._missionStateCheckTimer <= 500.0)
          return;
        this._missionStateCheckTimer = 0.0;
        Stage.MissionStateType missionState = this.MissionState;
        this.MissionState = this.CheckMissionState(out this._missionFailedReason);
        if (missionState == this.MissionState)
          return;
        switch (this.MissionState)
        {
          case Stage.MissionStateType.Completed:
            this._stageCompletedSound.Play();
            this.StageHUD.ShowIncomingMessage(IncomingMessage.MessageType.MissionCompleted);
            this._stageCompletedDelay = this.Stats.NumSnailsActive <= 0 ? 3000.0 : 1000.0;
            this.StageHUD.StopTimer();
            this.Stats.TimeTaken = this.Stats.Timer;
            if (this.LevelStage._goal != GoalType.TimeAttack)
              break;
            this.Stats.TimeTaken = this.LevelStage._targetTime - this.Stats.Timer;
            break;
          case Stage.MissionStateType.Failed:
            if (this._missionFailedReason == Stage.MissionFailedReasonType.TimeExpired)
              this.StageHUD.ShowIncomingMessage(IncomingMessage.MessageType.TimeIsUp);
            else
              this.StageHUD.ShowIncomingMessage(IncomingMessage.MessageType.MissionFailed);
            this._stageCompletedDelay = 3000.0;
            this.StageHUD.StopTimer();
            this._state = Stage.StageState.Ended;
            break;
        }
      }
      else
      {
        if ((this.Stats.NumSnailsActive != 0 || this.Stats.NumSnailsToRelease != 0) && this.MissionState != Stage.MissionStateType.Failed)
          return;
        this._stageEndedTimer += gameTime.ElapsedRealTime.TotalMilliseconds;
        if (this._stageEndedTimer <= this._stageCompletedDelay)
          return;
        this.EndMission();
      }
    }

    public void Update(BrainGameTime gameTime)
    {
      if (this._gameplayRecorder.Enabled)
        this._gameplayRecorder.Update(gameTime);
      if (this._state == Stage.StageState.Startup)
      {
        this.StageHUD.UpdateIncomingMessage(gameTime);
        this.Cursor.ControllerEvents(gameTime);
        this.Cursor.Update(gameTime);
        this.Camera.Update(gameTime);
        this.Board.Update(gameTime);
        this.LightManager.Update(gameTime);
        this.UpdateAudibleBoundinBox();
        Game1.Tutorial.Update(gameTime);
        foreach (SnailsBackgroundLayer layer in this.Layers)
          layer.Update(gameTime, false);
      }
      else
      {
        Game1.Tutorial.Update(gameTime);
        if (Game1.Tutorial.TopicVisible && Game1.GameSettings.PauseInTutorial)
        {
          this.Cursor.ControllerEvents(gameTime);
        }
        else
        {
          this.UpdateMissionState(gameTime);
          this.StageHUD.Update(gameTime);
          this.Cursor.ControllerEvents(gameTime);
          this.Cursor.Update(gameTime);
          if (this.HasWater && Game1.GameSettings.UseWaterEffect)
            this._waterShadeEffect.Update(gameTime);
          foreach (SnailsBackgroundLayer layer in this.Layers)
            layer.Update(gameTime, true);
          this.Board.Update(gameTime);
          if (this.Objects.Count > 0)
          {
            foreach (Object2D object2D in this.Objects)
              object2D.Update(gameTime);
          }
          if (this.Snails.Count > 0)
          {
            foreach (Object2D snail in this.Snails)
              snail.Update(gameTime);
          }
          foreach (StageObject stageObject in this.Objects)
            stageObject.AfterUpdate(gameTime);
          foreach (StageObject snail in this.Snails)
            snail.AfterUpdate(gameTime);
          if (this.Particles.Count > 0)
          {
            foreach (ParticlesEffect particle in this.Particles)
            {
              if (!particle.Ended)
                particle.Update(gameTime);
            }
          }
          this.Camera.Update(gameTime);
          this.LightManager.Update(gameTime);
          if (this.Rumble != null)
            this.Rumble.Update(gameTime);
          this.ProcessObjectsToRemove();
          this.ProcessParticlesToRemove();
          this.ProcessObjectsToAdd();
          Levels.CurrentLevel.StageSound.Update(gameTime);
          Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalPlayingTime += gameTime.ElapsedRealTime;
          this.UpdateAudibleBoundinBox();
        }
      }
    }

    private void UpdateAudibleBoundinBox()
    {
      BrainGame.SampleManager.AudibleBoundingSquare = this.StageHUD._stageArea.Transform(this.Camera.Position - this.Camera.Origin);
    }

    public void DrawToRenderTarget()
    {
      bool visible = BrainGame.GameCursor.Visible;
      BrainGame.GameCursor.Visible = false;
      BrainGame.Graphics.SetRenderTarget(this.RenderTarget);
      this.Draw();
      BrainGame.Graphics.SetRenderTarget((RenderTarget2D) null);
      BrainGame.Graphics.Viewport = BrainGame.Viewport;
      BrainGame.GameCursor.Visible = visible;
    }

    public void DrawToRenderTargetThumbs()
    {
      bool visible = BrainGame.GameCursor.Visible;
      BrainGame.GameCursor.Visible = false;
      PresentationParameters presentationParameters = this.SpriteBatch.GraphicsDevice.PresentationParameters;
      this.RenderTarget = new RenderTarget2D(this.SpriteBatch.GraphicsDevice, BrainGame.Viewport.Width, BrainGame.Viewport.Height, false, presentationParameters.BackBufferFormat, presentationParameters.DepthStencilFormat);
      BrainGame.Graphics.SetRenderTarget(this.RenderTarget);
      this.DrawForThumbs();
      BrainGame.Graphics.SetRenderTarget((RenderTarget2D) null);
      BrainGame.Graphics.Viewport = BrainGame.Viewport;
      BrainGame.GameCursor.Visible = visible;
    }

    public void DrawUnderWaterToRenderTarget()
    {
      if (this._waterRenderTarget == null)
      {
        PresentationParameters presentationParameters = this.SpriteBatch.GraphicsDevice.PresentationParameters;
        this._waterRenderTarget = new RenderTarget2D(this.SpriteBatch.GraphicsDevice, BrainGame.Viewport.Width, BrainGame.Viewport.Height, false, presentationParameters.BackBufferFormat, presentationParameters.DepthStencilFormat, 1, RenderTargetUsage.DiscardContents);
      }
      bool visible = BrainGame.GameCursor.Visible;
      BrainGame.GameCursor.Visible = false;
      BrainGame.Graphics.SetRenderTarget(this._waterRenderTarget);
      this.DrawBackgroundLayers();
      this.BeginDraw();
      this.Draw(false, true);
      this.EndDraw();
      this.DrawForegroundLayers();
      BrainGame.Graphics.SetRenderTarget((RenderTarget2D) null);
      BrainGame.Graphics.Viewport = BrainGame.Viewport;
      BrainGame.GameCursor.Visible = visible;
    }

    private void Draw(bool shadow) => this.Draw(shadow, false);

    private void Draw(bool shadow, bool drawForWaterShade)
    {
      if (this.OnBeforeObjectsDraw != null)
        this.OnBeforeObjectsDraw(shadow, this.SpriteBatch);
      foreach (StageObject backgroundObjectsDraw in this.BackgroundObjectsDrawList)
        backgroundObjectsDraw.Draw(shadow);
      foreach (StageObject snail in this.Snails)
        snail.Draw(shadow);
      this.Board.DrawBackground(shadow);
      if (drawForWaterShade)
      {
        foreach (StageObject foregroundWaterDraw in this.ForegroundWaterDrawList)
        {
          if (!(foregroundWaterDraw is Liquid))
            foregroundWaterDraw.Draw(shadow);
        }
      }
      else
      {
        foreach (StageObject foregroundWaterDraw in this.ForegroundWaterDrawList)
          foregroundWaterDraw.Draw(shadow);
      }
      this.Board.Draw(shadow);
      if (drawForWaterShade)
        return;
      foreach (StageObject foregroundObjectsDraw in this.ForegroundObjectsDrawList)
        foregroundObjectsDraw.Draw(shadow);
      foreach (StageObject backgroundObjectsDraw in this.BackgroundObjectsDrawList)
        backgroundObjectsDraw.ForegroundDraw();
      if (this.Particles.Count <= 0)
        return;
      foreach (ParticlesEffect particle in this.Particles)
      {
        if (!particle.Ended)
          particle.Draw(this.SpriteBatch);
      }
    }

    public void BeginDraw()
    {
      this.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, (DepthStencilState) null, (RasterizerState) null, (Effect) this.Camera.StageRenderEffect);
    }

    public void EndDraw() => this.SpriteBatch.End();

    public void DrawWaterTexture()
    {
      if (!this.HasWater || !Game1.GameSettings.UseWaterEffect)
        return;
      this.DrawUnderWaterToRenderTarget();
      this.WaterTexture = (Texture2D) this._waterRenderTarget;
      this.WaterTexture = this._waterShadeEffect.Draw(this.WaterTexture);
    }

    private void DrawBackgroundLayers()
    {
      foreach (SnailsBackgroundLayer layer in this.Layers)
      {
        if (layer._layerType == LayerType.Background)
        {
          this.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, (DepthStencilState) null, (RasterizerState) null, (Effect) layer.BackgroundEffect);
          layer.Draw(this.SpriteBatch);
          this.SpriteBatch.End();
        }
      }
    }

    private void DrawForegroundLayers()
    {
      foreach (SnailsBackgroundLayer layer in this.Layers)
      {
        if (layer._layerType == LayerType.Foreground)
        {
          this.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, (DepthStencilState) null, (RasterizerState) null, (Effect) layer.BackgroundEffect);
          layer.Draw(this.SpriteBatch);
          this.SpriteBatch.End();
        }
      }
    }

    public void Draw()
    {
      this.DrawBackgroundLayers();
      this.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, (DepthStencilState) null, (RasterizerState) null, (Effect) this.Camera.StageRenderEffect);
      if (this._withShadows)
        this.Draw(true);
      this.Draw(false);
      this.SpriteBatch.End();
      this.DrawForegroundLayers();
      this.LightManager.Draw();
      this.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, (DepthStencilState) null, (RasterizerState) null, (Effect) BrainGame.RenderEffect);
      this.StageHUD.Draw(this.SpriteBatch);
      this.Cursor.Draw();
      this.SpriteBatch.End();
      if (!this._gameplayRecorder.Enabled)
        return;
      this.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, (DepthStencilState) null, (RasterizerState) null, (Effect) BrainGame.RenderEffect);
      this._gameplayRecorder.Draw(Stage.CurrentStage.SpriteBatch);
      this.SpriteBatch.End();
    }

    public void DrawForThumbs()
    {
      this.Camera.Update((BrainGameTime) null);
      foreach (SnailsBackgroundLayer layer in this.Layers)
        layer.Update((BrainGameTime) null, false);
      this.DrawBackgroundLayers();
      this.SpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, BrainGame.CurrentSampler, (DepthStencilState) null, (RasterizerState) null, (Effect) this.Camera.StageRenderEffect);
      this.Draw(false);
      this.SpriteBatch.End();
      this.DrawForegroundLayers();
      this.LightManager.Draw();
    }

    public void UnloadContent()
    {
      if (this.RenderTarget != null)
      {
        this.RenderTarget.Dispose();
        this.RenderTarget = (RenderTarget2D) null;
      }
      if (this.LightManager != null)
        this.LightManager.Unload();
      if (this._waterRenderTarget == null)
        return;
      this._waterRenderTarget.Dispose();
      this._waterRenderTarget = (RenderTarget2D) null;
    }

    public Stage.MissionStateType CheckMissionState(out Stage.MissionFailedReasonType failedReason)
    {
      failedReason = Stage.MissionFailedReasonType.Incomplete;
      if ((this.LevelStage._goal == GoalType.SnailDelivery || this.LevelStage._goal == GoalType.SnailKiller || this.LevelStage._goal == GoalType.SnailKing) && this.Stats.Timer.TotalMinutes >= 60.0)
      {
        failedReason = Stage.MissionFailedReasonType.TimeExpired;
        return Stage.MissionStateType.Failed;
      }
      switch (this.LevelStage._goal)
      {
        case GoalType.SnailDelivery:
        case GoalType.TimeAttack:
          if (this.Stats.NumSnailsSafe >= this.Stats.NumSnailsToSave)
            return Stage.MissionStateType.Completed;
          if (this.Stats.NumSnailsToRelease + this.Stats.NumSnailsSafe + this.Stats.NumSnailsActive < this.Stats.NumSnailsToSave)
          {
            failedReason = Stage.MissionFailedReasonType.NotEnoughSnails;
            return Stage.MissionStateType.Failed;
          }
          if (this.LevelStage._goal == GoalType.TimeAttack && this.Stats.Timer.TotalSeconds == 0.0)
          {
            failedReason = Stage.MissionFailedReasonType.TimeExpired;
            return Stage.MissionStateType.Failed;
          }
          break;
        case GoalType.SnailKiller:
          if (this.Stats.NumSnailsDead >= this.LevelStage._snailsToRelease)
            return Stage.MissionStateType.Completed;
          break;
        case GoalType.SnailKing:
          if (this.Stats.SnailKingDelivered)
            return Stage.MissionStateType.Completed;
          if (this.Stats.SnailKingDead)
          {
            failedReason = Stage.MissionFailedReasonType.KingIsDead;
            return Stage.MissionStateType.Failed;
          }
          break;
      }
      return Stage.MissionStateType.Running;
    }

    public void ProcessObjectsToRemove()
    {
      foreach (StageObject stageObject in this.ObjectsRemove)
      {
        if (stageObject.IsSnail)
          this.RemoveSnail((Snail) stageObject);
        else
          this.Objects.Remove(stageObject);
        this.Board.RemoveObjectFromQuadtree(stageObject);
        this.RemoveObjectLinks(stageObject);
      }
      this.ObjectsRemove.Clear();
    }

    public void ProcessCollisionDetection()
    {
      foreach (StageObject snail in this.Snails)
        snail.DoQuadtreeCollisions(1);
    }

    public void ProcessParticlesToRemove()
    {
      if (this.Particles.Count <= 0)
        return;
      List<ParticlesEffect> particlesEffectList = new List<ParticlesEffect>();
      foreach (ParticlesEffect particle in this.Particles)
      {
        if (particle.Ended)
          particlesEffectList.Add(particle);
      }
      foreach (ParticlesEffect particlesEffect in particlesEffectList)
        this.Particles.Remove(particlesEffect);
      particlesEffectList.Clear();
    }

    public void AddSnail(Snail snail)
    {
      this.Snails.Add(snail);
      this.Board.AddObjectToQuadtree((StageObject) snail, 0);
    }

    public void ReleaseSnailKing(Vector2 pos, MovingObject.WalkDirection dir)
    {
      this.ReleaseSnail((Snail) this.StageData.GetObject("SNAIL_KING"), pos, dir);
    }

    public void ReleaseSnail(Vector2 pos, MovingObject.WalkDirection dir, string snailId)
    {
      this.ReleaseSnail((Snail) this.StageData.GetObject(snailId), pos, dir);
    }

    private void ReleaseSnail(Snail snail, Vector2 pos, MovingObject.WalkDirection dir)
    {
      snail.X = pos.X + (float) (Stage.CurrentStage.Board.TileWidth / 2);
      snail.Y = pos.Y + (float) Stage.CurrentStage.Board.TileHeight;
      snail.SetDirection(dir);
      snail.UpdateBoundingBox();
      if (snail.AllowStageStatistics)
      {
        ++this.Stats.NumSnailsReleased;
        --this.Stats.NumSnailsToRelease;
        ++this.Stats.NumSnailsActive;
      }
      Stage.CurrentStage.AddSnail(snail);
      snail.OnEnterStage();
    }

    public void ProcessObjectsToAdd()
    {
      foreach (StageObject stageObject in this.ObjectsAdd)
      {
        stageObject.UpdateBoundingBox();
        this.AddObject(stageObject, true);
      }
      this.ObjectsAdd.Clear();
    }

    public void AddObjectInRuntime(StageObject obj) => this.ObjectsAdd.Add(obj);

    public void AddObjectFromStageEditor(StageObject obj) => this.AddObject(obj, false);

    private void AddObject(StageObject obj) => this.AddObject(obj, false);

    private void AddObject(StageObject obj, bool lauchOnAddEvent)
    {
      this.Objects.Add(obj);
      if (obj.CanCollide)
        this.Board.AddObjectToQuadtree(obj, 1);
      if (obj is Liquid)
      {
        this.HasWater = true;
        this.ForegroundWaterDrawList.Add(obj);
        this.LiquidObjects.Add(obj as Liquid);
      }
      else if (obj is TileObject)
        this.ForegroundWaterDrawList.Insert(0, obj);
      else if (obj.DrawInForeground)
        this.ForegroundObjectsDrawList.Add(obj);
      else
        this.BackgroundObjectsDrawList.Add(obj);
      if (!lauchOnAddEvent)
        return;
      obj.OnAddedToStage();
    }

    public void RemoveObject(StageObject obj)
    {
      obj.StopSamples();
      obj.DynamicFlags = StageObjectDynamicFlags.IsDisposed;
      this.ObjectsRemove.Add(obj);
      if (obj is Liquid)
      {
        this.LiquidObjects.Remove(obj as Liquid);
        this.ForegroundWaterDrawList.Remove(obj);
      }
      else if (obj is TileObject)
        this.ForegroundWaterDrawList.Remove(obj);
      else if (obj.DrawInForeground)
        this.ForegroundObjectsDrawList.Remove(obj);
      else
        this.BackgroundObjectsDrawList.Remove(obj);
      if (obj.IsSnail)
      {
        Snail snail = (Snail) obj;
        if (snail.AllowStageStatistics && !snail._inactiveAccounted)
        {
          --Stage.CurrentStage.Stats.NumSnailsActive;
          snail._inactiveAccounted = true;
        }
      }
      obj.OnStageRemoved();
    }

    private void RemoveSnail(Snail snail)
    {
      this.Snails.Remove(snail);
      if (snail.AllowStageStatistics && !snail._deathAccounted)
      {
        ++this.Stats.NumSnailsDisposed;
        snail._deathAccounted = true;
      }
      if (!snail.IsSnailKing)
        return;
      this.Stats.SnailKingDead = true;
    }

    public void RemoveObjectLinks(StageObject obj)
    {
      foreach (StageObject stageObject in this.Objects)
      {
        if (stageObject.WithLinks && stageObject.LinkedObjects.Contains(obj))
          stageObject.LinkedObjects.Remove(obj);
      }
    }

    private void SetObjectLinks()
    {
      foreach (StageObject stageObject in this.Objects)
      {
        if (!string.IsNullOrEmpty(stageObject.LinkString))
        {
          string linkString = stageObject.LinkString;
          char[] chArray = new char[1]{ ';' };
          foreach (string uid in linkString.Split(chArray))
          {
            StageObject objectByUid = this.GetObjectByUid(uid);
            if (objectByUid != null)
              stageObject.AddLinkedObject(objectByUid);
          }
        }
      }
    }

    private StageObject GetObjectByUid(string uid)
    {
      foreach (StageObject objectByUid in this.Objects)
      {
        if (objectByUid.UniqueId == uid)
          return objectByUid;
      }
      return (StageObject) null;
    }

    public void OnObjectPicked(PickableObject.PickableType type, int quantity)
    {
      if (!PickableObject.IsTool(type))
        return;
      this.StageHUD._toolsMenu.AddToolQuantity((ToolObjectType) Enum.Parse(typeof (ToolObjectType), type.ToString(), true), quantity);
    }

    public void DisposeObject(StageObject obj)
    {
      this.RemoveObject(obj);
      obj.StaticFlags = StageObjectStaticFlags.None;
      obj.DynamicFlags = StageObjectDynamicFlags.IsDead | StageObjectDynamicFlags.IsDisposed;
    }

    public void SendExplosionNotification(Explosion explosion)
    {
      foreach (StageObject stageObject in this.Objects)
      {
        if (!stageObject.IsDead && !stageObject.IsDisposed)
          stageObject.OnExplosion(explosion);
      }
    }

    public void ToggleTimeWarp()
    {
      if (Stage.CurrentStage.InTimeWarp)
      {
        this.InTimeWarp = false;
        BrainGame.SampleManager.ChangePlayingPitch(0.0f);
        GameplayScreen.Instance.TimeMultiplier = 1f;
      }
      else
      {
        GameplayScreen.Instance.TimeMultiplier = 4f;
        this.InTimeWarp = true;
        BrainGame.SampleManager.ChangePlayingPitch(0.7f);
      }
      if (Stage.CurrentStage._state == Stage.StageState.Startup)
        Stage.CurrentStage.StartupEnded();
      this.StageHUD.TimeWarpChanged();
    }

    public void PauseGame()
    {
      if (this.IsPaused)
        return;
      this.IsPaused = true;
      BrainGame.SampleManager.PauseAll();
      BrainGame.MusicManager.PauseMusic();
      GameplayScreen.Instance.PopUp(ScreenType.InGameOptions.ToString(), false);
      if (!this._gameplayRecorder.Enabled)
        return;
      this._gameplayRecorder.Pause();
    }

    public void ResumeGame()
    {
      this.IsPaused = false;
      BrainGame.SampleManager.ResumeAll();
      BrainGame.MusicManager.ResumeMusic();
      BrainGame.GameCursor.Visible = Game1.GameSettings.ShowCursor;
      BrainGame.SampleManager.UseAudibleBoundingSquare = true;
      if (!this._gameplayRecorder.Enabled)
        return;
      this._gameplayRecorder.Resume();
    }

    public void QuitStage() => BrainGame.MusicManager.StopMusic();

    public void ShowTutorialTopic(int topicId, bool showIfAlreadyViewed)
    {
      this.ShowTutorialTopics(new int[1]{ topicId }, showIfAlreadyViewed);
    }

    public void ShowTutorialTopics(int[] topicsIds, bool showIfAlreadyViewed)
    {
      HowToPlayScreen.PopUp(Game1.Tutorial.GetTopics(topicsIds), true);
    }

    public void EndMission()
    {
      BrainGame.SampleManager.StopAll();
      if (Game1.GameSettings.UseGamepad)
        BrainGame.GameCursor.Visible = false;
      this.ComputeStageScore();
      if (this._gameplayRecorder.IsRecording)
        this._gameplayRecorder.Stop();
      this._gameplayRecorder.Enabled = false;
      Game1.ProfilesManager.Save();
      if (this.MissionState == Stage.MissionStateType.Completed)
      {
        GameplayScreen.Instance.NavigateTo(ScreenType.StageCompleted.ToString());
      }
      else
      {
        GameplayScreen.Instance.Navigator.GlobalCache.Set("MISSION_FAILED_REASON", (object) this._missionFailedReason);
        GameplayScreen.Instance.NavigateTo(ScreenType.MissionFailed.ToString());
      }
    }

    public void RestartMission()
    {
      BrainGame.SampleManager.StopAll();
      BrainGame.MusicManager.FadeMusic(0.0f, 500);
      if (this._gameplayRecorder.Enabled)
        this._gameplayRecorder.Stop();
      GameplayScreen.Instance.Navigator.GlobalCache.Set("STAGE_START_SHOW_STAGE_INFO", (object) false);
      GameplayScreen.Instance.Navigator.GlobalCache.Set("STAGE_START_SHOW_XBOX_HELP", (object) false);
      GameplayScreen.Instance.NavigateTo(ScreenType.StageStart.ToString(), (Transition) ScreenTransitions.LeafsClosing, (Transition) null);
    }

    public bool IsCropAreaEmpty(int newColCount, int newRowCount)
    {
      if (newColCount >= this.Board.Columns && newRowCount >= this.Board.Rows)
        return true;
      for (int index1 = 0; index1 < Math.Max(this.Board.Columns, newColCount); ++index1)
      {
        if (index1 >= newColCount)
        {
          for (int index2 = newRowCount; index2 < this.Board.Rows; ++index2)
          {
            if (this.Board.Tiles[index2, index1] != null)
              return false;
          }
        }
      }
      BoundingSquare boundingSquare = new BoundingSquare(new Vector2(0.0f, 0.0f), new Vector2((float) (newColCount * this.Board.TileWidth), (float) (newRowCount * this.Board.TileHeight)));
      foreach (StageObject stageObject in this.Objects)
      {
        if (!boundingSquare.Contains(stageObject.AABoundingBox))
          return false;
      }
      foreach (Snail snail in this.Snails)
      {
        if (!boundingSquare.Contains(snail.AABoundingBox))
          return false;
      }
      return true;
    }

    public void ResizeBoard(int cols, int rows)
    {
      this.Board.Resize(cols, rows);
      for (int index = 0; index < this.Objects.Count; ++index)
      {
        if (!this.Board.BoundingBox.Contains(this.Objects[index].AABoundingBox))
        {
          this.Objects.Remove(this.Objects[index]);
          --index;
        }
      }
      for (int index = 0; index < this.Snails.Count; ++index)
      {
        if (!this.Board.BoundingBox.Contains(this.Snails[index].AABoundingBox))
        {
          this.RemoveSnail(this.Snails[index]);
          --index;
        }
      }
    }

    public int GetUnusedToolsCount() => this.StageHUD._toolsMenu.GetTotalTools();

    public int GetUnusedToolsPoints() => this.StageHUD._toolsMenu.GetTotalTools() * 5;

    public int GetCoinPoints()
    {
      return this.Stats.NumBronzeCoins * 20 + this.Stats.NumSilverCoins * 50 + this.Stats.NumGoldCoins * 100;
    }

    private int GetTotalPointsForSeconds(int totalSeconds) => totalSeconds;

    public int GetTimePoints()
    {
      if (this.LevelStage._goal != GoalType.TimeAttack)
        return this.GetTotalPointsForSeconds(this.GetTotalSecondsBellowTargeTime());
      TimeSpan timeSpan = this.Stats.Timer;
      if (timeSpan.Milliseconds > 0)
        timeSpan = timeSpan.Seconds >= 59 ? new TimeSpan(timeSpan.Hours, timeSpan.Minutes + 1, 0) : new TimeSpan(timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds + 1);
      return (int) timeSpan.TotalSeconds;
    }

    public int SnailsDeliveredPoints(out int totalSnails)
    {
      totalSnails = 0;
      if (this.LevelStage._goal == GoalType.SnailKiller)
      {
        totalSnails = this.Stats.NumSnailsDead;
        return this.Stats.NumSnailsDead * 5;
      }
      totalSnails = this.Stats.NumSnailsSafe;
      return this.Stats.NumSnailsSafe * 5;
    }

    public int GetTotalSecondsBellowTargeTime()
    {
      return this.Stats.Timer < this.LevelStage._targetTime ? (int) Math.Ceiling((this.LevelStage._targetTime - this.Stats.Timer).TotalSeconds) : 0;
    }

    public void SnailsStageStatsChanged() => this.StageHUD.SnailsStageStatsChanged();

    public void SnailKingDelivered() => this.Stats.SnailKingDelivered = true;

    public void IncrementBuildNr() => ++this.BuildNr;

    public StageObject GetObjectById(string id)
    {
      foreach (StageObject objectById in this.Objects)
      {
        if (objectById.UniqueId == id)
          return objectById;
      }
      return (StageObject) null;
    }

    public ToolObject GetToolByType(ToolObjectType type)
    {
      foreach (ToolObject tool in this.StageHUD._toolsMenu.Tools)
      {
        if (tool.Type == type)
          return tool;
      }
      return (ToolObject) null;
    }

    public void GameLostFocus()
    {
      if (!Game1.GameSettings.PauseGameWhenFocusLost)
        return;
      this.Cursor.GameLostFocus();
      this.PauseGame();
    }

    public void TutorialTopicOpened() => this.Cursor.TutorialTopicOpened();

    public void TutorialTopicClosed() => this.Cursor.TutorialTopicClosed();

    public static LevelStage GetLevelStageFromDataFileRecord(DataFileRecord stageRecord)
    {
      return new LevelStage()
      {
        _snailsToSave = stageRecord.GetFieldValue<int>("snailsToSave"),
        _snailsToRelease = stageRecord.GetFieldValue<int>("snailsToRelease"),
        _targetTime = stageRecord.GetFieldValue<TimeSpan>("targetTime"),
        _goldMedalTime = stageRecord.GetFieldValue<TimeSpan>("goldMedalTime"),
        _goldMedalScore = stageRecord.GetFieldValue<int>("goldMedalScore"),
        _goal = (GoalType) stageRecord.GetFieldValue<int>("goal"),
        AvailableInDemo = stageRecord.GetFieldValue<bool>("availableInDemo"),
        ThemeId = (ThemeType) stageRecord.GetFieldValue<int>("theme"),
        StageId = stageRecord.GetFieldValue<string>("id"),
        StageKey = stageRecord.GetFieldValue<string>("key", (string) null),
        IsCustomStage = stageRecord.GetFieldValue<bool>("isCustomStage", false)
      };
    }

    public void RefreshTiles() => this.Board.RefreshTiles();

    public void SetLayers(List<SnailsBackgroundLayer> layers)
    {
      this.Layers.Clear();
      foreach (SnailsBackgroundLayer layer in layers)
        this.Layers.Add(layer);
    }

    public int CountPickableObjects(PickableObject.PickableType objType)
    {
      int num = 0;
      foreach (StageObject stageObject in this.Objects)
      {
        if (stageObject is PickableObject && ((PickableObject) stageObject)._pickableType == objType)
          ++num;
      }
      return num;
    }

    private void ComputeStageScore()
    {
      int totalSnails = 0;
      this.Stats.TimePointsWon = this.GetTimePoints();
      this.Stats.SnailsDeliveredPointsWon = this.SnailsDeliveredPoints(out totalSnails);
      this.Stats.CoinPointsWon = this.GetCoinPoints();
      this.Stats.TotalScore = this.Stats.SnailsDeliveredPointsWon + this.Stats.TimePointsWon + this.Stats.CoinPointsWon;
      this.Stats.MedalWon = MedalType.Bronze;
      if (this.Stats.TotalScore >= this.GoldMedalScoreCriteria.Score && totalSnails >= this.GoldMedalScoreCriteria.SnailsNeeded && Math.Floor(this.Stats.TimeTaken.TotalSeconds) <= Math.Floor(this.GoldMedalScoreCriteria.TimeNeeded.TotalSeconds) && this.Stats.NumBronzeCoins >= this.GoldMedalScoreCriteria.BronzeCoinsNeeded && this.Stats.NumSilverCoins >= this.GoldMedalScoreCriteria.SilverCoinsNeeded && this.Stats.NumGoldCoins >= this.GoldMedalScoreCriteria.GoldCoinsNeeded)
      {
        this.Stats.MedalWon = MedalType.Gold;
      }
      else
      {
        if (this.Stats.TotalScore < this.SilverMedalScoreCriteria.Score || totalSnails < this.SilverMedalScoreCriteria.SnailsNeeded || Math.Floor(this.Stats.TimeTaken.TotalSeconds) >= Math.Floor(this.SilverMedalScoreCriteria.TimeNeeded.TotalSeconds) || this.Stats.NumBronzeCoins < this.SilverMedalScoreCriteria.BronzeCoinsNeeded || this.Stats.NumSilverCoins < this.SilverMedalScoreCriteria.SilverCoinsNeeded || this.Stats.NumGoldCoins < this.SilverMedalScoreCriteria.GoldCoinsNeeded)
          return;
        this.Stats.MedalWon = MedalType.Silver;
      }
    }

    public virtual void InitFromDataFileRecord(DataFileRecord stageRecord)
    {
      this.LightManager = new LightManager();
      this.Description = stageRecord.GetFieldValue<string>("desc");
      this.Id = stageRecord.GetFieldValue<string>("id");
      this.BuildNr = stageRecord.GetFieldValue<int>("buildNr", 0);
      if (this.LevelStage == null)
        this.LevelStage = new LevelStage();
      this.LevelStage._snailsToSave = stageRecord.GetFieldValue<int>("snailsToSave", this.LevelStage._snailsToSave);
      this.LevelStage._snailsToRelease = stageRecord.GetFieldValue<int>("snailsToRelease", this.LevelStage._snailsToRelease);
      this.LevelStage._targetTime = stageRecord.GetFieldValue<TimeSpan>("targetTime", this.LevelStage._targetTime);
      this.LevelStage._goldMedalTime = stageRecord.GetFieldValue<TimeSpan>("goldMedalTime", this.LevelStage._goldMedalTime);
      this.LevelStage._goldMedalScore = stageRecord.GetFieldValue<int>("goldMedalScore", this.LevelStage._goldMedalScore);
      this.LevelStage._goal = (GoalType) stageRecord.GetFieldValue<int>("goal", (int) this.LevelStage._goal);
      this.LevelStage.AvailableInDemo = stageRecord.GetFieldValue<bool>("availableInDemo", this.LevelStage.AvailableInDemo);
      this.LevelStage = Stage.GetLevelStageFromDataFileRecord(stageRecord);
      this.LevelStage.IsCustomStage = false;
      this.LevelStage.CustomStageFilename = (string) null;
      this.LightManager.LightEnabled = stageRecord.GetFieldValue<bool>("lightEnabled", false);
      this.LightManager.LightColor = stageRecord.GetFieldValue<Color>("lightColor", Color.Black);
      this._withShadows = stageRecord.GetFieldValue<bool>("withShadows", false);
      this.StartupTopicsString = stageRecord.GetFieldValue<string>("startupTutorialTopics", (string) null);
      this.StartupCenter = new Vector2((float) stageRecord.GetFieldValue<int>("startupCenterX", 0), (float) stageRecord.GetFieldValue<int>("startupCenterY", 0));
      this._backgroundLayersOffset = new Vector2((float) stageRecord.GetFieldValue<int>("backLayersOffsetX", 0), (float) stageRecord.GetFieldValue<int>("backLayersOffsetY", 0));
      foreach (DataFileRecord selectRecord in stageRecord.SelectRecords("Tools\\Tool"))
      {
        ToolObject tool = this.StageData.GetTool(selectRecord.GetFieldValue<string>("id")).Clone();
        tool.InitFromDataFileRecord(selectRecord);
        this.StageHUD._toolsMenu.AddTool(tool);
      }
      this.Board = Board.FromDataFileRecord(stageRecord.SelectRecord("Board"));
      this._withLiquids = false;
      foreach (DataFileRecord selectRecord in stageRecord.SelectRecords("Objects\\Object"))
      {
        StageObject objectNoInitialize = this.StageData.GetObjectNoInitialize(selectRecord.GetFieldValue<string>("id"));
        objectNoInitialize.InitFromDataFileRecord(selectRecord);
        objectNoInitialize.PreviousPosition = objectNoInitialize.Position;
        objectNoInitialize.LoadContent();
        objectNoInitialize.Initialize();
        objectNoInitialize.StageInitialize();
        objectNoInitialize.UpdateBoundingBox();
        objectNoInitialize.UpdateCrateCollisionBoundingBox();
        if (objectNoInitialize is Liquid)
          this._withLiquids = true;
        this.AddObject(objectNoInitialize);
      }
      this.Board.ComputePaths();
      foreach (StageObject stageObject in this.Objects)
        stageObject.AfterBoardInitialize();
      this.Layers = new List<SnailsBackgroundLayer>();
      foreach (DataFileRecord selectRecord in stageRecord.SelectRecords("Layers\\Layer"))
      {
        SnailsBackgroundLayer layer = this.StageData.GetLayer(selectRecord.GetFieldValue<string>("id"));
        layer.InitFromDataFileRecord(selectRecord);
        this.Layers.Add(layer);
      }
      this.SetObjectLinks();
      this.GoldMedalScoreCriteria = MedalScoreCriteria.CreateFromDataFileRecord(stageRecord.SelectRecord("goldMedalCriteria"), this);
      this.SilverMedalScoreCriteria = MedalScoreCriteria.CreateFromDataFileRecord(stageRecord.SelectRecord("silverMedalCriteria"), this);
      this.BronzeMedalScoreCriteria = MedalScoreCriteria.CreateFromDataFileRecord(stageRecord.SelectRecord("bronzeMedalCriteria"), this);
      bool flag = false;
      foreach (StageObject stageObject in this.Objects)
      {
        if (stageObject is TutorialSign && Game1.ProfilesManager.CurrentProfile != null)
        {
          TutorialSign tutorialSign = (TutorialSign) stageObject;
          if (tutorialSign.TutorialTopics != null)
          {
            foreach (int tutorialTopic in tutorialSign.TutorialTopics)
            {
              if (!Game1.ProfilesManager.CurrentProfile.IsTutorialTopicRead(tutorialTopic))
              {
                Game1.ProfilesManager.CurrentProfile.MarkTutorialTopicAsRead(tutorialTopic);
                flag = true;
              }
            }
          }
        }
      }
      if (!flag)
        return;
      Game1.ProfilesManager.Save();
    }

    public virtual DataFileRecord ToDataFileRecord()
    {
      return this.ToDataFileRecord(ToDataFileRecordContext.StageDataSave);
    }

    public virtual DataFileRecord ToDataFileRecord(ToDataFileRecordContext context)
    {
      DataFileRecord dataFileRecord1 = new DataFileRecord(nameof (Stage));
      dataFileRecord1.AddField("id", (object) this.Id);
      dataFileRecord1.AddField("key", (object) this.Key);
      dataFileRecord1.AddField("buildNr", (object) this.BuildNr);
      dataFileRecord1.AddField("desc", (object) this.Description);
      dataFileRecord1.AddField("snailsToSave", (object) this.LevelStage._snailsToSave);
      dataFileRecord1.AddField("snailsToRelease", (object) this.LevelStage._snailsToRelease);
      dataFileRecord1.AddField("targetTime", (object) this.LevelStage._targetTime);
      dataFileRecord1.AddField("goldMedalTime", (object) this.LevelStage._goldMedalTime);
      dataFileRecord1.AddField("goldMedalScore", (object) this.LevelStage._goldMedalScore);
      dataFileRecord1.AddField("goal", (object) (int) this.LevelStage._goal);
      dataFileRecord1.AddField("theme", (object) (int) this.LevelStage.ThemeId);
      dataFileRecord1.AddField("isCustomStage", (object) this.LevelStage.IsCustomStage);
      dataFileRecord1.AddField("availableInDemo", (object) this.LevelStage.AvailableInDemo);
      dataFileRecord1.AddField("lightEnabled", (object) this.LightManager.LightEnabled);
      dataFileRecord1.AddField("lightColor", (object) this.LightManager.LightColor);
      dataFileRecord1.AddField("withShadows", (object) this._withShadows);
      dataFileRecord1.AddField("startupTutorialTopics", (object) this.StartupTopicsString);
      dataFileRecord1.AddField("startupCenterX", (object) this.StartupCenter.X);
      dataFileRecord1.AddField("startupCenterY", (object) this.StartupCenter.Y);
      dataFileRecord1.AddField("backLayersOffsetX", (object) this._backgroundLayersOffset.X);
      dataFileRecord1.AddField("backLayersOffsetY", (object) this._backgroundLayersOffset.Y);
      dataFileRecord1.AddRecord(this.GoldMedalScoreCriteria.ToDataFileRecord("goldMedalCriteria"));
      dataFileRecord1.AddRecord(this.SilverMedalScoreCriteria.ToDataFileRecord("silverMedalCriteria"));
      dataFileRecord1.AddRecord(this.BronzeMedalScoreCriteria.ToDataFileRecord("bronzeMedalCriteria"));
      DataFileRecord dataFileRecord2 = dataFileRecord1.AddRecord("Layers");
      foreach (SnailsBackgroundLayer layer in this.Layers)
        dataFileRecord2.AddRecord(layer.ToDataFileRecord(context));
      DataFileRecord dataFileRecord3 = dataFileRecord1.AddRecord("Tools");
      foreach (ToolObject tool in this.StageHUD._toolsMenu.Tools)
      {
        if (!(tool is ToolEndMission))
          dataFileRecord3.AddRecord(tool.ToDataFileRecord(context));
      }
      DataFileRecord dataFileRecord4 = dataFileRecord1.AddRecord("Objects");
      foreach (StageObject stageObject in this.Objects)
        dataFileRecord4.AddRecord(stageObject.ToDataFileRecord(context));
      dataFileRecord1.AddRecord(this.Board.ToDataFileRecord(context));
      return dataFileRecord1;
    }

    public enum StageLoadingContext
    {
      Gameplay,
      StageEditor,
    }

    public enum MissionStateType
    {
      None,
      Starting,
      Running,
      Completed,
      Failed,
    }

    public enum MissionFailedReasonType
    {
      Incomplete,
      NotEnoughSnails,
      TimeExpired,
      KingIsDead,
    }

    public enum StageState
    {
      Startup,
      Playing,
      Paused,
      Ended,
      LoadingGameplay,
    }

    public delegate void StageDrawEventHandler(bool shadow, SpriteBatch spriteBatch);
  }
}
