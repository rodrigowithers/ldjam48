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
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
            Application.Quit();
#elif UNITY_WEBGL
            Application.OpenURL("about:blank");
#endif
        }
    }
}