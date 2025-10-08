using UnityEngine;
using UnityEngine.SceneManagement;

public class ChoiceQuit : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }

    public void LoadMainMenu()
    {
        SceneManager.LoadSceneAsync("MainMenu");
    }
}
