using UnityEngine;

public class TitleScreen : MonoBehaviour
{
    public GameObject MapScreen;
    public CanvasGroup TextGroup;
    
    private void Update()
    {
        TextGroup.alpha = (Mathf.Sin(Time.timeSinceLevelLoad) + 1) / 2;
        
        if (Input.anyKeyDown)
        {
            gameObject.SetActive(false);
            MapScreen.SetActive(true);
        }
    }
}
