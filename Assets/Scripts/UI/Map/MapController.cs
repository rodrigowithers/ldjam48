using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapController : MonoBehaviour
{
    public Text PlanetInfo;
    public GameObject Selector;

    public Button GoButton;
    
    public void SelectPlanet()
    {
        PlanetInfo.gameObject.SetActive(true);
        Selector.SetActive(true);

        GoButton.interactable = true;
    }
    
    public void Go()
    {
        SceneManager.LoadScene(1);
    }
}
