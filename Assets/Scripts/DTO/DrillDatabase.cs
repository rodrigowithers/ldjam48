using Player;
using System.IO;
using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace DTO
{
    public static class DrillDatabase
    {
        private static readonly string SavePath = Path.Combine(Application.persistentDataPath, "drills.json");

        public static List<Drill> Drills;

        private static void Save()
        {
            // Write new list of drills to use in game
            Drills = new List<Drill>
            {
                new Drill(50, 3, 1),
                new Drill(80, 5, 2),
                new Drill(40, 8, 3),
            };

            var json = JsonConvert.SerializeObject(Drills, Formatting.Indented);
            File.WriteAllText(SavePath, json);
        }

        private static void Load()
        {
            if (!File.Exists(SavePath))
                Save();
                
            var json = File.ReadAllText(SavePath);
            Drills = JsonConvert.DeserializeObject<List<Drill>>(json);
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Initialize()
        {
            Load();
        }
    }
}