using Player;
using UnityEngine;

namespace UI
{
    public class DepthVisualizer : MonoBehaviour
    {
        public Transform Player;
        public RectTransform Caret;

        private const int Size = 76;

        private void Update()
        {
            Caret.anchoredPosition = new Vector2(0, Mathf.Lerp(Size, -Size, Mathf.InverseLerp(5, World.TotalDepth, Player.position.y)));
        }
    }
}
