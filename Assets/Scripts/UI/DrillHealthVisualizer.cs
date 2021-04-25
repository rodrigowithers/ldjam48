using System;
using DG.Tweening;
using Player;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class DrillHealthVisualizer : MonoBehaviour
    {
        public PlayerController Player;
        
        public Image HealthImage;
        public Sprite[] HealthStages;   // 5

        private void Start()
        {
            Player.OnDrillDamage += PlayerOnOnDrillDamage;
        }

        private void PlayerOnOnDrillDamage()
        {
            transform.DOShakePosition(0.25f, 1f, 50);
        }

        public void Update()
        {
            // 0 -> 1
            float health = (float) Player.Drill.Health / Player.Drill.TotalHealth;

            if (health > 0.75f)
            {
                HealthImage.sprite = HealthStages[0];
            }
            else if (health > 0.5f)
            {
                HealthImage.sprite = HealthStages[1];
            }
            else if (health > 0.25f)
            {
                HealthImage.sprite = HealthStages[2];
            }
            else if (health > 0f)
            {
                HealthImage.sprite = HealthStages[3];
            }
            else
            {
                HealthImage.sprite = HealthStages[4];
            }
        }
    }
}