using UI;

namespace Tile
{
    public class GoldTile : BreakableTile
    {
        protected override void OnBreak()
        {
            base.OnBreak();
            Player.ResourceStorage.Gold += 1;
            
            Logger.Instance.Log("Gathered Gold");
        }
    }
}