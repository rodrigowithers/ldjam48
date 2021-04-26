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
        [Range(0f, 1f)] public float Weight = 1;
    }
    
    [Serializable]
    public class DepthLayer
    {
        public string Name;
        public List<WeightedTile> Tiles;
        [Range(0f, 1f)] public float MinDepth;
        [Range(0f, 1f)] public float MaxDepth;
    }

    public class TilemapCreator : MonoBehaviour
    {
        public static TilemapCreator Instance { get; set; }

        public Bounds Bounds;

        private float TileSize => 0.5f;

        public List<DepthLayer> Layers;
            
        private WeightedTile GetTileDepth(float depth)
        {
            float totalWeight = 0;
            List<WeightedTile> possibleTiles = new List<WeightedTile>();

            foreach (DepthLayer layer in Layers)
            {
                if(depth >= layer.MinDepth && depth < layer.MaxDepth)
                    possibleTiles.AddRange(layer.Tiles);
            }

            foreach (WeightedTile possibleTile in possibleTiles)
                totalWeight += possibleTile.Weight;
            
            float random = Random.Range(0f, totalWeight);

            WeightedTile selected = null;
            foreach (WeightedTile tile in possibleTiles)
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

            Vector2 firstPosition = initialPos + Vector2.left * Bounds.extents.x + Vector2.up * Bounds.extents.y;

            for (int y = 0; y < stepsY; y++)
            {
                float posY = iniY - TileSize * y;
                float depth = Mathf.InverseLerp(iniY, finalY, posY);

                for (int x = 0; x < stepsX; x++)
                {
                    var currentPos = firstPosition + Vector2.right * (TileSize * x)
                                                   + Vector2.down * (TileSize * y);

                    var tile = GetTileDepth(depth);
                    
                    if(tile.Prefab != null)
                        Instantiate(tile.Prefab, currentPos, Quaternion.identity, transform);
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