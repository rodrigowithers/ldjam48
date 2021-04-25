using DG.Tweening;
using UnityEngine;

namespace Tile
{
    public class DirtTile : BreakableTile
    {
        protected override void OnBreak()
        {
            Destroy(_collider);

            transform.DOScale(Vector3.zero, 0.8f).SetEase(Ease.OutExpo);
        }
    }
}