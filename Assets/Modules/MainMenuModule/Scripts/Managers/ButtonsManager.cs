using UnityEditor;

using UnityEngine;
using UnityEngine.SceneManagement;

namespace SDRGames.Whist.MainMenuModule.Managers
{
    public class ButtonsManager : MonoBehaviour
    {
        public void NewGameButtonClicked()
        {
            SceneManager.LoadSceneAsync("LocationMapScene");
        }

        public void ExitGameButtonClicked()
        {
            #if UNITY_EDITOR
                EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }
    }
}
