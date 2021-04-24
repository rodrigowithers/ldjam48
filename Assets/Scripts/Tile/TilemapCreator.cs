using System.Collections.Generic;
using UnityEngine;

namespace Tile
{
    [System.Serializable]
    public class WeightedTile
    {
        public GameObject Prefab;
        public float Weight = 1;
    }
    
    public class TilemapCreator : MonoBehaviour
    {
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
        
        [ContextMenu("Create")]
        private void Create()
        {
            int stepsX = (int) (Bounds.extents.x * 2 / TileSize);
            int stepsY = (int) (Bounds.extents.y * 2 / TileSize);
            Vector2 initialPos = transform.position;

            Vector2 firstPosition = initialPos + Vector2.left * Bounds.extents.x + Vector2.up * Bounds.extents.y;
            Vector2 currentPos = firstPosition;
            
            for (int x = 0; x < stepsX; x++)
            {
                for (int y = 0; y < stepsY; y++)
                {
                    currentPos = firstPosition + Vector2.right * TileSize * x 
                                               + Vector2.down * TileSize * y;

                    Instantiate(GetTile(GetTotalWeight()).Prefab, currentPos, Quaternion.identity, transform);
                }
            }
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
