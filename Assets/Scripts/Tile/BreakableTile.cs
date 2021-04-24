using UnityEngine;

namespace Tile
{
    public class BreakableTile : MonoBehaviour
    {
        public ParticleSystem BreakingParticles;
        public float Durability = 3f;
        public int Hardness = 1;

        protected virtual void OnBreak()
        {
            Player.ResourceStorage.Stone += 1;
            Destroy(this.gameObject);
        }
        
        public bool Damage(float dmg = 1)
        {
            if (dmg < Hardness)
                return false;
            
            Durability -= Time.deltaTime * dmg;
            BreakingParticles.Emit(1);
            
            if (Durability <= 0)
            {
                OnBreak();
                return true;
            }

            return false;
        }
    }
}