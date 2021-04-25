using DG.Tweening;
using UnityEngine;

namespace Tile
{
    public class BreakableTile : MonoBehaviour
    {
        [SerializeField] protected CircleCollider2D _collider;
        
        public ParticleSystem BreakingParticles;
        public float Durability = 3f;
        public int Hardness = 1;

        protected virtual void OnBreak()
        {
            Player.ResourceStorage.Stone += 1;
            Destroy(_collider);

            transform.DOScale(Vector3.zero, 0.8f).SetEase(Ease.OutExpo);
        }

        public bool Damage(float dmg, float strength)
        {
            if (strength < Hardness)
                return false;
            
            Durability -= Time.deltaTime * dmg;
            
            BreakingParticles.Emit(1);
            
            if (Durability <= 0)
            {
                OnBreak();
                    
                if(strength > Hardness * 2)
                    return false;
                return true;
            }

            return false;
        }
    }
}