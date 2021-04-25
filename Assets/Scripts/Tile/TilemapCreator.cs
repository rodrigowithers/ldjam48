using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace Tile
{
    [Serializable]
    public class WeightedTile
    {
        public GameObject Prefab;
        public float Weight = 1;
        public float Offset = 0;
    }

    public class TilemapCreator : MonoBehaviour
    {
        public static TilemapCreator Instance { get; set; }

        public Bounds Bounds;

        public List<WeightedTile> Tiles;
        private float TileSize => 0.5f;

        private WeightedTile GetTile(float totalWeight)
        {
            float random = Random.Range(0f, totalWeight);
            WeightedTile selected = null;

            foreach (WeightedTile tile in Tiles)
            {
                if (random <= tile.Weight)
                {
                    selected = tile;
                    break;
                }

                random -= tile.Weight;
            }

            return selected;
        }

        private float GetTotalWeight()
        {
            float total = 0;

            foreach (WeightedTile tile in Tiles)
                total += tile.Weight;

            return total;
        }

        
        [ContextMenu("Clear")]
        private void DeleteTiles()
        {
            Transform t = transform;
            int count = t.childCount;

#if UNITY_EDITOR
            var tempList = transform.Cast<Transform>().ToList();
            foreach(var child in tempList)
            {
                DestroyImmediate(child.gameObject);
            }                 
#else
            for (int i = 0; i < count; i++)
            {
                    Destroy(t.GetChild(i).gameObject);
            }
#endif
        }


        
        [ContextMenu("Create")]
        public void Create()
        {
            DeleteTiles();

            int stepsX = (int) (Bounds.extents.x * 2 / TileSize);
            int stepsY = (int) (Bounds.extents.y * 2 / TileSize);
            Vector2 initialPos = transform.position;

            float iniY = initialPos.y + Bounds.extents.y;
            float finalY = initialPos.y - Bounds.extents.y;

            float weight = GetTotalWeight();
            
            Vector2 firstPosition = initialPos + Vector2.left * Bounds.extents.x + Vector2.up * Bounds.extents.y;

            for (int y = 0; y < stepsY; y++)
            {
                float posY = iniY - TileSize * y;
                float depth = Mathf.InverseLerp(iniY, finalY, posY);

                for (int x = 0; x < stepsX; x++)
                {
                    var currentPos = firstPosition + Vector2.right * (TileSize * x)
                                                   + Vector2.down * (TileSize * y);

                    Instantiate(GetTile(weight * depth).Prefab, currentPos, Quaternion.identity, transform);
                }
            }
        }

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            Create();
        }

        private void OnDrawGizmos()
        {
            var b = Bounds;
            b.center = transform.position;
            DebugExtension.DrawBounds(b, Color.gray);
        }
    }
}