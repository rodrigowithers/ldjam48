using Player;
using UnityEngine;

public class MoveButton : MonoBehaviour
{
    public void BeginRotate(int dir)
    {
        PlayerController.Instance.BeginRotate(dir);
    }

    public void StopRotate()
    {
        PlayerController.Instance.StopRotate();
    }
}
