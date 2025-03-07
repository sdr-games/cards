using UnityEngine;
using UnityEngine.SceneManagement;

public class NagivationSceneManager : MonoBehaviour
{
    public void OpenCombatScene()
    {
        SceneManager.LoadScene("CombatScene");
    }

    public void OpenTalentsScene()
    {
        SceneManager.LoadScene("TalentsScene");
    }

    public void OpenMapScene()
    {
        SceneManager.LoadScene("LocationMapScene");
    }

    public void OpenMainMenuScene()
    {
        SceneManager.LoadScene("MainMenuScene");
    }
}
