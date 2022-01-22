using Player;
using UnityEngine;

public class PlayPauseButton : MonoBehaviour
{
    public void Click()
    {
        PlayerController.Instance.ToggleMovement();
    }
}
