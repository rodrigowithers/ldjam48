namespace Tile
{
    public class StoneTile : BreakableTile
    {
        protected override void OnBreak()
        {
            // Add to Achievements
            AchievementSystem.AchievementHandler.IncreaseAchievementCounter(0);
            AchievementSystem.AchievementHandler.IncreaseAchievementCounter(1);
            
            base.OnBreak();
            Player.ResourceStorage.Stone += 1;
        }
    }
}