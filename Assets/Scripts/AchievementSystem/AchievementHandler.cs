using System.IO;
using UnityEngine;
using System.Linq;
using Newtonsoft.Json;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace AchievementSystem
{
    public static class AchievementHandler
    {
        private static Dictionary<int, SerializedAchievement> SerializedAchievements { get; set; }

        private static string AchievementDatabasePath => Path.Combine(Application.dataPath, "AchievementDatabase.json");
        
        public static void IncreaseAchievementCounter(int achievementID)
        {
            SerializedAchievement achievement = SerializedAchievements[achievementID];
            if (achievement.Completed) return;
            
            achievement.CurrentCount++;
            if (achievement.CurrentCount >= achievement.Requirement)
            {
                achievement.Completed = true;
                AchievementUIController.Instance.Show(achievement.Name, achievement.Description, achievement.ImagePath);
            }
            
            Save();
        }
        
#if UNITY_EDITOR
        [MenuItem("Databases/Clear Achievements")]
        private static void Clear()
        {
            string json = File.ReadAllText(AchievementDatabasePath);
            List<SerializedAchievement> achievements = JsonConvert.DeserializeObject<List<SerializedAchievement>>(json);
            foreach (var achievement in achievements)
            {
                achievement.Completed = false;
                achievement.CurrentCount = 0;
            }
            
            json = JsonConvert.SerializeObject(achievements, Formatting.Indented);
            File.WriteAllText(AchievementDatabasePath, json);
        }
#endif

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void Load()
        {
            string json = File.ReadAllText(AchievementDatabasePath);
            List<SerializedAchievement> achievements = JsonConvert.DeserializeObject<List<SerializedAchievement>>(json);

            SerializedAchievements = new Dictionary<int, SerializedAchievement>();
            
            foreach (var achievement in achievements)
            {
                SerializedAchievements.Add(achievement.ID, achievement);
            }
        }

        private static void Save()
        {
            List<SerializedAchievement> achievements = SerializedAchievements.Values.ToList();

            string json = JsonConvert.SerializeObject(achievements, Formatting.Indented);
            File.WriteAllText(AchievementDatabasePath, json);
        }
    }
}