using System.IO;
using Newtonsoft.Json;
using UnityEngine;

namespace DTO
{
    public class PlayerInfo
    {
        public int DrillIdentifier;
        public int DrillTier;
        
        public int StoneCount;
        public int IronCount;
        public int GoldCount;
    }
    
    public static class PlayerDatabase
    {
        private static readonly string SavePath = Path.Combine(Application.dataPath, "player.json");

        public static PlayerInfo Info;

        private static void Save()
        {
            // Write a generic player
            Save(new PlayerInfo { DrillIdentifier = 0 });
        }
        
        public static void Save(PlayerInfo i)
        {
            Info = i;

            var json = JsonConvert.SerializeObject(Info, Formatting.Indented);
            File.WriteAllText(SavePath, json);
        }

        private static void Load()
        {
            if (!File.Exists(SavePath))
                Save();
                
            var json = File.ReadAllText(SavePath);
            Info = JsonConvert.DeserializeObject<PlayerInfo>(json);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            Load();
        }
    }
}