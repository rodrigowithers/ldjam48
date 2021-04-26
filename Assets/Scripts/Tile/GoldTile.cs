using UI;

namespace Tile
{
    public class GoldTile : BreakableTile
    {
        protected override void OnBreak()
        {
            // Add to Achievements
            AchievementSystem.AchievementHandler.IncreaseAchievementCounter(4);
            AchievementSystem.AchievementHandler.IncreaseAchievementCounter(5);
            
            base.OnBreak();
            Player.ResourceStorage.Gold += 1;
            
            Logger.Instance.Log("Gathered Gold");
        }
    }
}