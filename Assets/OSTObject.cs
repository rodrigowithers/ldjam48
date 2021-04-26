using UnityEngine;

public class OSTObject : MonoBehaviour
{
    public static OSTObject Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }
}
