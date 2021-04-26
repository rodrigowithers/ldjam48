using UI;

namespace Tile
{
    public class IronTile : BreakableTile
    {
        protected override void OnBreak()
        {
            // Add to Achievements
            AchievementSystem.AchievementHandler.IncreaseAchievementCounter(2);
            AchievementSystem.AchievementHandler.IncreaseAchievementCounter(3);
            
            base.OnBreak();
            Player.ResourceStorage.Iron += 1;
            
            Logger.Instance.Log("Gathered Iron");
        }
    }
}