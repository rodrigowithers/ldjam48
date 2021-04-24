using DTO;

namespace Player
{
    public static class ResourceStorage
    {
        public static int Stone { get; set; }
        public static int Iron { get; set; }
        public static int Gold { get; set; }

        public static void Save()
        {
            var i = PlayerDatabase.Info;

            i.StoneCount = Stone;
            i.IronCount = Iron;
            i.GoldCount = Gold;

            PlayerDatabase.Save(i);
        }

        public static void Load()
        {
            var i = PlayerDatabase.Info;

            Stone = i.StoneCount;
            Iron = i.IronCount;
            Gold = i.GoldCount;
        }
    }
}