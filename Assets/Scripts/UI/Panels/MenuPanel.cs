using UnityEngine;

namespace UI.Panels
{
    public class MenuPanel : MonoBehaviour
    {
        public void Surface()
        {
            GameController.Instance.SetGameState(GameController.GameState.Shop);
        }

        public void Resume()
        {
            GameController.Instance.SetGameState(GameController.GameState.Game);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}