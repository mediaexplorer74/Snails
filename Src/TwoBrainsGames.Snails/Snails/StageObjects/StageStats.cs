
// Type: TwoBrainsGames.Snails.StageObjects.StageStats
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using System;
using TwoBrainsGames.Snails.Stages;


namespace TwoBrainsGames.Snails.StageObjects
{
  public class StageStats
  {
    protected string _stageId;
    protected TimeSpan _timer = new TimeSpan(0L);
    protected int _numSnailsToRelease;
    protected int _numSnailsSafe;
    protected int _numSnailsReleased;
    protected int _numSnailsDisposed;
    protected int _numSnailsToSave;
    protected int _numSnailsActive;
    protected bool _snailKingDelivered;
    protected bool _snailKingDead;
    protected int _numGoldCoins;
    protected int _numSilverCoins;
    protected int _numBronzeCoins;
    protected int _numSnailsDeadByFire;
    protected int _numSnailsDeadBySpikes;
    protected int _numSnailsDeadByDynamite;
    protected int _numSnailsDeadByTools;
    protected int _numApplesUsed;
    protected int _numVitaminUsed;
    protected int _numBoxedUsed;
    protected int _numCopperUsed;
    protected int _numBoxesUsed;
    protected int _numDynamitesUsed;

    public string StageId
    {
      get => this._stageId;
      set => this._stageId = value;
    }

    public TimeSpan Timer
    {
      get => this._timer;
      set => this._timer = value;
    }

    public TimeSpan TimeTaken { get; set; }

    public int NumGoldCoins
    {
      get => this._numGoldCoins;
      set => this._numGoldCoins = value;
    }

    public int NumSilverCoins
    {
      get => this._numSilverCoins;
      set => this._numSilverCoins = value;
    }

    public int NumBronzeCoins
    {
      get => this._numBronzeCoins;
      set => this._numBronzeCoins = value;
    }

    public int NumSnailsToRelease
    {
      get => this._numSnailsToRelease;
      set
      {
        if (this._numSnailsToRelease == value)
          return;
        this._numSnailsToRelease = value;
        Stage.CurrentStage.SnailsStageStatsChanged();
      }
    }

    public int NumSnailsSafe
    {
      get => this._numSnailsSafe;
      set
      {
        if (this._numSnailsSafe == value)
          return;
        this._numSnailsSafe = value;
        Stage.CurrentStage.SnailsStageStatsChanged();
      }
    }

    public int NumSnailsReleased
    {
      get => this._numSnailsReleased;
      set
      {
        if (this._numSnailsReleased == value)
          return;
        this._numSnailsReleased = value;
        Stage.CurrentStage.SnailsStageStatsChanged();
      }
    }

    public int NumSnailsDisposed
    {
      get => this._numSnailsDisposed;
      set
      {
        if (this._numSnailsDisposed == value)
          return;
        this._numSnailsDisposed = value;
        Stage.CurrentStage.SnailsStageStatsChanged();
      }
    }

    public int NumSnailsDead => this._numSnailsDisposed - this._numSnailsSafe;

    public int NumSnailsToSave
    {
      get => this._numSnailsToSave;
      set
      {
        if (this._numSnailsToSave == value)
          return;
        this._numSnailsToSave = value;
        Stage.CurrentStage.SnailsStageStatsChanged();
      }
    }

    public int NumSnailsActive
    {
      get => this._numSnailsActive;
      set
      {
        if (this._numSnailsActive == value)
          return;
        this._numSnailsActive = value;
        Stage.CurrentStage.SnailsStageStatsChanged();
      }
    }

    public bool SnailKingDelivered
    {
      get => this._snailKingDelivered;
      set
      {
        if (this._snailKingDelivered == value)
          return;
        this._snailKingDelivered = value;
      }
    }

    public bool SnailKingDead
    {
      get => this._snailKingDead;
      set
      {
        if (this._snailKingDead == value)
          return;
        this._snailKingDead = value;
      }
    }

    public int TotalSnails => this._numSnailsActive + this._numSnailsToRelease;

    public int SnailsDeliveredPointsWon { get; set; }

    public int TimePointsWon { get; set; }

    public int CoinPointsWon { get; set; }

    public MedalType MedalWon { get; set; }

    public int TotalScore { get; set; }
  }
}
