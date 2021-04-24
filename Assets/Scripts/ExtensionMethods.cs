using UnityEngine;

namespace Scripts
{
    public static partial class ExtensionMethods
    {
        public static Vector3Int ToVector3Int(this Vector2 v)
        {
            return new Vector3Int((int) v.x, (int)v.y, 0);
        }
    }
}