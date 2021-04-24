using DTO;

namespace Player
{
    [System.Serializable]
    public class Drill
    {
        public int TotalHealth;
        public int Health;
        
        public float Strength = 1;
        public float Speed = 1;

        public Drill(int totalHealth, float strength, float speed)
        {
            TotalHealth = totalHealth;
            Health = totalHealth;
            Strength = strength;
            Speed = speed;
        }

        public static Drill GetDrill(int id)
        {
            return DrillDatabase.Drills[id];
        }
    }
}
