
// Type: TwoBrainsGames.Snails.Player.Achievements
// Assembly: TwoBrainsGames.Snails, Version=1.0.4923.37504, Culture=neutral, PublicKeyToken=null
// MVID: B19A8606-1885-4B3A-BBAA-3363A0A3FD71
// Modded by [M]edia[E]xplorer

using TwoBrainsGames.BrainEngine;
using TwoBrainsGames.BrainEngine.Player;


namespace TwoBrainsGames.Snails.Player
{
  public static class Achievements
  {
    public const int Kill50WithASingleDynamite_Quantity = 70;
    public const int Kill50WithASingleBox_Quantity = 20;

    private static void RegisterAchievement(int eventType, Callback handler)
    {
      if (Game1.ProfilesManager.CurrentProfile.IsAchievementEarned(eventType))
        return;
      BrainGame.AchievementsManager.Register(eventType, handler);
    }

    private static void AchievementEarned(int eventType)
    {
      Game1.ProfilesManager.CurrentProfile.MarkAchievementEarned(eventType);
      Game1.ProfilesManager.Save();
      if (!Game1.GameSettings.WithAppStore && BrainGame.AchievementsManager.GetAchievement(eventType).ShowOnAppStore)
        return;
      BrainGame.AchievementsManager.QueueAchievement(eventType);
    }

    public static void Register()
    {
      Achievements.RegisterAchievement(6, new Callback(Achievements.OnSafelyEscortAllSnailsInOneSpecificStage));
      Achievements.RegisterAchievement(3, new Callback(Achievements.OnSafelyEscort100Snails));
      Achievements.RegisterAchievement(4, new Callback(Achievements.OnSafelyEscort300Snails));
      Achievements.RegisterAchievement(5, new Callback(Achievements.OnSafelyEscort600Snails));
      Achievements.RegisterAchievement(8, new Callback(Achievements.OnSave5SnailsKing));
      Achievements.RegisterAchievement(9, new Callback(Achievements.OnSave10SnailsKing));
      Achievements.RegisterAchievement(10, new Callback(Achievements.OnBurn20Snails));
      Achievements.RegisterAchievement(13, new Callback(Achievements.OnKill20SnailsWithASingleBox));
      Achievements.RegisterAchievement(14, new Callback(Achievements.OnKill50SnailsWithDynamite));
      Achievements.RegisterAchievement(15, new Callback(Achievements.OnKill50WithASingleDynamite));
      Achievements.RegisterAchievement(17, new Callback(Achievements.OnKill20SnailsInSpikes));
      Achievements.RegisterAchievement(18, new Callback(Achievements.OnKill20SnailsWithLazer));
      Achievements.RegisterAchievement(19, new Callback(Achievements.OnEmpaleOneSnailWhileFalling));
      Achievements.RegisterAchievement(20, new Callback(Achievements.OnHibernateASnail));
      Achievements.RegisterAchievement(22, new Callback(Achievements.OnBoost50Snails));
      Achievements.RegisterAchievement(25, new Callback(Achievements.OnGet50BronzeMedals));
      Achievements.RegisterAchievement(26, new Callback(Achievements.OnGet100BronzeMedals));
      Achievements.RegisterAchievement(29, new Callback(Achievements.OnGet50SilverMedals));
      Achievements.RegisterAchievement(30, new Callback(Achievements.OnGet100SilverMedals));
      Achievements.RegisterAchievement(32, new Callback(Achievements.OnGet50GoldMedals));
      Achievements.RegisterAchievement(33, new Callback(Achievements.OnGet100GoldMedals));
      Achievements.RegisterAchievement(34, new Callback(Achievements.OnGet200BronzeMedals));
      Achievements.RegisterAchievement(35, new Callback(Achievements.OnGet200SilverMedals));
      Achievements.RegisterAchievement(36, new Callback(Achievements.OnGet200GoldMedals));
      Achievements.RegisterAchievement(37, new Callback(Achievements.OnClearAllWildNatureStages));
      Achievements.RegisterAchievement(38, new Callback(Achievements.OnUnlockEgyptTheme));
      Achievements.RegisterAchievement(39, new Callback(Achievements.OnClearAllEgyptStages));
      Achievements.RegisterAchievement(40, new Callback(Achievements.OnUnlockGraveyardTheme));
      Achievements.RegisterAchievement(41, new Callback(Achievements.OnClearAllGraveyardStages));
      Achievements.RegisterAchievement(42, new Callback(Achievements.OnUnlockGoldminesTheme));
      Achievements.RegisterAchievement(43, new Callback(Achievements.OnClearAllGoldminesStages));
      Achievements.RegisterAchievement(44, new Callback(Achievements.OnGetAllBronzeMedals));
      Achievements.RegisterAchievement(45, new Callback(Achievements.OnGetAllSilverMedals));
      Achievements.RegisterAchievement(46, new Callback(Achievements.OnGetAllGoldMedals));
      Achievements.RegisterAchievement(47, new Callback(Achievements.OnPurchaseTheGame));
      Achievements.RegisterAchievement(48, new Callback(Achievements.OnKillSnailsIn11DifferentWays));
    }

    private static int OnSafelyEscort100Snails()
    {
      int totalSnailsSafe = Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsSafe;
      int quantity = BrainGame.AchievementsManager.GetAchievement(3).Quantity;
      if (totalSnailsSafe >= quantity)
        Achievements.AchievementEarned(3);
      return totalSnailsSafe;
    }

    private static int OnSafelyEscort300Snails()
    {
      int totalSnailsSafe = Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsSafe;
      int quantity = BrainGame.AchievementsManager.GetAchievement(4).Quantity;
      if (totalSnailsSafe >= quantity)
        Achievements.AchievementEarned(4);
      return totalSnailsSafe;
    }

    private static int OnSafelyEscort600Snails()
    {
      int totalSnailsSafe = Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsSafe;
      int quantity = BrainGame.AchievementsManager.GetAchievement(5).Quantity;
      if (totalSnailsSafe >= quantity)
        Achievements.AchievementEarned(5);
      return totalSnailsSafe;
    }

    private static int OnSafelyEscortAllSnailsInOneSpecificStage()
    {
      Achievements.AchievementEarned(6);
      return -1;
    }

    private static int OnSave5SnailsKing()
    {
      int totalSnailsKingSafe = Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsKingSafe;
      int quantity = BrainGame.AchievementsManager.GetAchievement(8).Quantity;
      if (totalSnailsKingSafe >= quantity)
        Achievements.AchievementEarned(8);
      return totalSnailsKingSafe;
    }

    private static int OnSave10SnailsKing()
    {
      int totalSnailsKingSafe = Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsKingSafe;
      int quantity = BrainGame.AchievementsManager.GetAchievement(9).Quantity;
      if (totalSnailsKingSafe >= quantity)
        Achievements.AchievementEarned(9);
      return totalSnailsKingSafe;
    }

    private static int OnBurn20Snails()
    {
      int snailsDeadByFire = Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByFire;
      int quantity = BrainGame.AchievementsManager.GetAchievement(10).Quantity;
      if (snailsDeadByFire >= quantity)
        Achievements.AchievementEarned(10);
      return snailsDeadByFire;
    }

    private static int OnKill20SnailsWithASingleBox()
    {
      Achievements.AchievementEarned(13);
      return -1;
    }

    private static int OnKill50SnailsWithDynamite()
    {
      int snailsDeadByDynamite = Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByDynamite;
      int quantity = BrainGame.AchievementsManager.GetAchievement(14).Quantity;
      if (snailsDeadByDynamite >= quantity)
        Achievements.AchievementEarned(14);
      return snailsDeadByDynamite;
    }

    private static int OnKill50WithASingleDynamite()
    {
      Achievements.AchievementEarned(15);
      return -1;
    }

    private static int OnKill20SnailsInSpikes()
    {
      int snailsDeadBySpikes = Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadBySpikes;
      int quantity = BrainGame.AchievementsManager.GetAchievement(17).Quantity;
      if (snailsDeadBySpikes >= quantity)
        Achievements.AchievementEarned(17);
      return snailsDeadBySpikes;
    }

    private static int OnKill20SnailsWithLazer()
    {
      int snailsDeadByLaser = Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSnailsDeadByLaser;
      int quantity = BrainGame.AchievementsManager.GetAchievement(18).Quantity;
      if (snailsDeadByLaser >= quantity)
        Achievements.AchievementEarned(18);
      return snailsDeadByLaser;
    }

    private static int OnEmpaleOneSnailWhileFalling()
    {
      Achievements.AchievementEarned(19);
      return -1;
    }

    private static int OnHibernateASnail()
    {
      Achievements.AchievementEarned(20);
      return -1;
    }

    private static int OnBoost50Snails()
    {
      int totalBoosts = Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalBoosts;
      int quantity = BrainGame.AchievementsManager.GetAchievement(22).Quantity;
      if (totalBoosts >= quantity)
        Achievements.AchievementEarned(22);
      return totalBoosts;
    }

    private static int OnGet50BronzeMedals()
    {
      int totalBronzeCoins = Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalBronzeCoins;
      int quantity = BrainGame.AchievementsManager.GetAchievement(25).Quantity;
      if (totalBronzeCoins >= quantity)
        Achievements.AchievementEarned(25);
      return totalBronzeCoins;
    }

    private static int OnGet100BronzeMedals()
    {
      int totalBronzeCoins = Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalBronzeCoins;
      int quantity = BrainGame.AchievementsManager.GetAchievement(26).Quantity;
      if (totalBronzeCoins >= quantity)
        Achievements.AchievementEarned(26);
      return totalBronzeCoins;
    }

    private static int OnGet50SilverMedals()
    {
      int totalSilverCoins = Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSilverCoins;
      int quantity = BrainGame.AchievementsManager.GetAchievement(29).Quantity;
      if (totalSilverCoins >= quantity)
        Achievements.AchievementEarned(29);
      return totalSilverCoins;
    }

    private static int OnGet100SilverMedals()
    {
      int totalSilverCoins = Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSilverCoins;
      int quantity = BrainGame.AchievementsManager.GetAchievement(30).Quantity;
      if (totalSilverCoins >= quantity)
        Achievements.AchievementEarned(30);
      return totalSilverCoins;
    }

    private static int OnGet50GoldMedals()
    {
      int totalGoldCoins = Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalGoldCoins;
      int quantity = BrainGame.AchievementsManager.GetAchievement(32).Quantity;
      if (totalGoldCoins >= quantity)
        Achievements.AchievementEarned(32);
      return totalGoldCoins;
    }

    private static int OnGet100GoldMedals()
    {
      int totalGoldCoins = Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalGoldCoins;
      int quantity = BrainGame.AchievementsManager.GetAchievement(33).Quantity;
      if (totalGoldCoins >= quantity)
        Achievements.AchievementEarned(33);
      return totalGoldCoins;
    }

    private static int OnGet200BronzeMedals()
    {
      int totalBronzeCoins = Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalBronzeCoins;
      int quantity = BrainGame.AchievementsManager.GetAchievement(34).Quantity;
      if (totalBronzeCoins >= quantity)
        Achievements.AchievementEarned(34);
      return totalBronzeCoins;
    }

    private static int OnGet200SilverMedals()
    {
      int totalSilverCoins = Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalSilverCoins;
      int quantity = BrainGame.AchievementsManager.GetAchievement(35).Quantity;
      if (totalSilverCoins >= quantity)
        Achievements.AchievementEarned(35);
      return totalSilverCoins;
    }

    private static int OnGet200GoldMedals()
    {
      int totalGoldCoins = Game1.ProfilesManager.CurrentProfile.PlayerStats.TotalGoldCoins;
      int quantity = BrainGame.AchievementsManager.GetAchievement(36).Quantity;
      if (totalGoldCoins >= quantity)
        Achievements.AchievementEarned(36);
      return totalGoldCoins;
    }

    private static int OnClearAllWildNatureStages()
    {
      Achievements.AchievementEarned(37);
      return -1;
    }

    private static int OnUnlockEgyptTheme()
    {
      Achievements.AchievementEarned(38);
      return -1;
    }

    private static int OnClearAllEgyptStages()
    {
      Achievements.AchievementEarned(39);
      return -1;
    }

    private static int OnUnlockGraveyardTheme()
    {
      Achievements.AchievementEarned(40);
      return -1;
    }

    private static int OnClearAllGraveyardStages()
    {
      Achievements.AchievementEarned(41);
      return -1;
    }

    private static int OnUnlockGoldminesTheme()
    {
      Achievements.AchievementEarned(42);
      return -1;
    }

    private static int OnClearAllGoldminesStages()
    {
      Achievements.AchievementEarned(43);
      return -1;
    }

    private static int OnGetAllBronzeMedals()
    {
      Achievements.AchievementEarned(44);
      return -1;
    }

    private static int OnGetAllSilverMedals()
    {
      Achievements.AchievementEarned(45);
      return -1;
    }

    private static int OnGetAllGoldMedals()
    {
      Achievements.AchievementEarned(46);
      return -1;
    }

    private static int OnPurchaseTheGame()
    {
      Achievements.AchievementEarned(47);
      return -1;
    }

    private static int OnKillSnailsIn11DifferentWays()
    {
      int deadInDifferentWays = Game1.ProfilesManager.CurrentProfile.PlayerStats.SnailsDeadInDifferentWays;
      int quantity = BrainGame.AchievementsManager.GetAchievement(48).Quantity;
      if (deadInDifferentWays >= quantity)
        Achievements.AchievementEarned(48);
      return deadInDifferentWays;
    }
  }
}
