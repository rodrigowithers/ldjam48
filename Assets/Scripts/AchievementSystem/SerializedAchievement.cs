using System;

namespace AchievementSystem
{ 
    [Serializable]
    public class SerializedAchievement
    {
        public int ID;
        public string Name;
        public string Description;

        public int Requirement;
        public int CurrentCount;
        
        public string ImagePath;

        public bool Completed;
        
        public SerializedAchievement() { }
    }
}
