using System.IO;
using Newtonsoft.Json;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

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
        private static readonly string SavePath = Path.Combine(Application.persistentDataPath, "player.json");

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
            
            Debug.Log($"json file saved at {SavePath}.");
            File.WriteAllText(SavePath, json);
        }

        private static void Load()
        {
            if (!File.Exists(SavePath))
            {
                Debug.Log("File doesnt exist, creating new Player json file.");
                Save();
            }
                
            var json = File.ReadAllText(SavePath);
            Info = JsonConvert.DeserializeObject<PlayerInfo>(json);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            Load();
        }
        
#if UNITY_EDITOR
        [MenuItem("Databases/Clear Player")]
        private static void Clear()
        {
            var json = File.ReadAllText(SavePath);
            Info = JsonConvert.DeserializeObject<PlayerInfo>(json);

            Info.DrillIdentifier = 0;
            Info.DrillTier = 0;
            Info.StoneCount = 0;
            Info.IronCount = 0;
            Info.GoldCount = 0;
            
            json = JsonConvert.SerializeObject(Info, Formatting.Indented);
            File.WriteAllText(SavePath, json);
        }
#endif
    }
}