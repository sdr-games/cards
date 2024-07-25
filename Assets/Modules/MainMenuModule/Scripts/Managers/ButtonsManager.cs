using UnityEditor;

using UnityEngine;

namespace SDRGames.Whist.MainMenuModule.Managers
{
    public class ButtonsManager : MonoBehaviour
    {
        public void NewGameButtonClicked()
        {
            
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
