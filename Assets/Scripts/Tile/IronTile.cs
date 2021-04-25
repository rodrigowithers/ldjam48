using UI;

namespace Tile
{
    public class IronTile : BreakableTile
    {
        protected override void OnBreak()
        {
            base.OnBreak();
            Player.ResourceStorage.Iron += 1;
            
            Logger.Instance.Log("Gathered Iron");
        }
    }
}